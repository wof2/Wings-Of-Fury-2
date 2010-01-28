using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MHydrax;
using Mogre;
using Wof.Controller;
using Timer=Mogre.Timer;

namespace Wof.View.Effects
{
    /// <summary>
    /// Klasa odpowiedzialna za zarz¹dzanie hydraxem
    /// </summary>
    public class HydraxManager
    {

        public readonly int ASYNC_FRAME_TIME = 30; // w millisekundach
        public readonly bool USE_UPDATER_THREAD = false;
        private bool stopUpdater = false; 


        protected MHydrax.MHydrax hydrax = null;
        private static readonly HydraxManager singleton = new HydraxManager();

      

        private Thread updater;
        /// <summary>
        /// Które materia³y maja byc dolaczone do hydrax depth techniques
        /// </summary>
        private List<string> hydraxDepthMaterials = new List<string> { "Island", "Concrete", "Steel", "Torpedo" };

        /// <summary>
        /// Zawiera mapê: nazwa materialu vs. iloœæ technik (przed dodaniem depth technique). Umozliwia to pozniejsze usuniecie depthtechnique
        /// </summary>
        private Dictionary<string, ushort> hydraxDepthMaterialsMap;

        public static HydraxManager Singleton
        {
            get { return singleton; }
        }

        static HydraxManager()
        {
        }

        private HydraxManager()
        {
            this.hydraxDepthMaterialsMap = new Dictionary<string, ushort>();
            BuildHydraxDepthMaterialsInfo();

        }


        private void BuildHydraxDepthMaterialsInfo()
        {
            foreach (string material in hydraxDepthMaterials)
            {
                if (MaterialManager.Singleton.GetByName(material) != null)
                {
                    hydraxDepthMaterialsMap[material] = ((MaterialPtr)MaterialManager.Singleton.GetByName(material)).NumTechniques;
                }
            }
        }
        public void RemoveHydraxDepthTechniques()
        {
			
            foreach (string material in hydraxDepthMaterials)
            {
                MaterialPtr m = MaterialManager.Singleton.GetByName(material);
                if (m != null && m.GetTechnique("_Hydrax_Depth_Technique") != null)
                {
                    m.RemoveTechnique(hydraxDepthMaterialsMap[material]);
                    m = null;
                }
            }
        }
        
        public void AddHydraxDepthTechnique(String materialName)
        {
        	if(hydrax == null || !hydrax.IsCreated) return;
        	
            MaterialPtr m = MaterialManager.Singleton.GetByName(materialName);
            if (m != null && m.GetTechnique("_Hydrax_Depth_Technique") == null)
            {	
            	hydraxDepthMaterialsMap[materialName] = ((MaterialPtr)m).NumTechniques;
            	hydraxDepthMaterials.Add(materialName);

                Technique t = m.CreateTechnique();
             
                hydrax.MaterialManager.AddDepthTechnique(t);        
               
                m = null;
            }
            
            foreach (Technique t in hydrax.MaterialManager.DepthTechniques)
            {
                t.SetFog(true, FogMode.FOG_NONE);               
            }
            
        }

        /// <summary>
        /// Dodaje materia³y okreœlone w tablicy "hydraxDepthMaterials" do materia³ów które s¹ poprawnie wyœwietlane pod wod¹
        /// </summary>
        public void AddHydraxDepthTechniques()
        {
            foreach (string material in hydraxDepthMaterials)
            {
                MaterialPtr m = MaterialManager.Singleton.GetByName(material);
                if (m != null && m.GetTechnique("_Hydrax_Depth_Technique") == null)
                {
                    
                    hydrax.MaterialManager.AddDepthTechnique(m.CreateTechnique());                   
                    m = null;
                }
            }
            // podwodne elementy nie powinny byc zamglone
            foreach (Technique t in hydrax.MaterialManager.DepthTechniques)
            {
                t.SetFog(true, FogMode.FOG_NONE);               
            }

        }

        /// <summary>
        /// Odœwie¿a powierzchniê wody
        /// </summary>
        /// <param name="evt"></param>
        public void Update(FrameEvent evt)
        {


            if (USE_UPDATER_THREAD)
            {
                if(updater != null && !updater.IsAlive)
                {
                    // staly FPS
                    updater.Start(ASYNC_FRAME_TIME);
                }
            }
            else
            {
                Update(evt.timeSinceLastFrame);
                /*
                if (hydrax != null && hydrax.IsCreated)
                {
                    hydrax.Update(evt.timeSinceLastFrame);
                } */
            }

             
        }

        private float timeSinceUpdate = 1000;
        private bool forceUpdate = false;

        public void ForceUpdate()
        {
            forceUpdate = true;
        }

        public void Update(float timeSinceLastFrame)
        {

            if(EngineConfig.UpdateHydraxEveryFrame)
            {
                if (hydrax != null && hydrax.IsCreated) hydrax.Update(timeSinceLastFrame);
                return;
            }
            else
            {
                float updateEvery = 0.05f; // 20fps


                if (hydrax != null && hydrax.IsCreated)
                {
                    timeSinceUpdate += timeSinceLastFrame;
                    if (forceUpdate || timeSinceUpdate > updateEvery)
                    {
                        hydrax.Update(timeSinceUpdate);
                        timeSinceUpdate = 0;
                        forceUpdate = false;
                    }

                }
            }
          
          
           

        }


        public void UpdateLoop(object frameTotalTime)
        {
            int frameTime = (int)frameTotalTime;
           // 20 ms -> 50 fps
            Timer timer = new Timer();
            timer.Reset();
            uint start, end;
            start = timer.Milliseconds;
            float timeSinceLastFrame;
            while(updater.IsAlive)
            {
                Monitor.Enter(HydraxManager.Singleton);
                try
                {
                    if (stopUpdater)
                    {
                        return;
                    }
                }
                finally
                {
                    Monitor.Exit(HydraxManager.Singleton); 
                }
             
               
               
                int sleepTime = 50;
                if (hydrax != null && hydrax.IsCreated)
                {
                    timeSinceLastFrame = (timer.Milliseconds - start) / 1000.0f; // w sekundach                       
                    start = timer.Milliseconds;

                    Monitor.Enter(HydraxManager.Singleton);
                    hydrax.Update(timeSinceLastFrame);
                    Monitor.Exit(HydraxManager.Singleton); 
                   

                    end = timer.Milliseconds;

                    int duration = (int)(end - start);

                    sleepTime = frameTime - duration;
                    if (sleepTime < 0) sleepTime = 1;

                   

                }
                Thread.Sleep(sleepTime);
            }
            
        }

        public MHydrax.MHydrax GetHydrax()
        {
            return this.hydrax;
        }

        public void TranslateSurface(Vector3 vector)
        {
            if(hydrax == null || !hydrax.IsCreated) return;
          //  hydrax.Mesh.SceneNode.Translate(vector);
           
           // hydrax.Position = new Vector3(hydrax.Position.x + vector.x, hydrax.Position.y + vector.y, hydrax.Position.z + vector.z);
        }

        /// <summary>
        /// Tworzy gotow¹ powierzchniê wody. 
        /// </summary>
        /// <param name="cfgFileName"></param>
        /// <param name="sceneMgr"></param>
        /// <param name="camera"></param>
        /// <param name="viewport"></param>
        public void CreateHydrax(string cfgFileName, SceneManager sceneMgr, Camera camera, Viewport viewport)
        {
            
            if (hydrax != null)
            {
                hydrax.Dispose();
                hydrax = null;
            }
            if (USE_UPDATER_THREAD)
            {
                stopUpdater = false;
                updater = new Thread(UpdateLoop);
            }


            hydrax = new MHydrax.MHydrax(sceneMgr, camera, viewport);

            MProjectedGrid module = new MProjectedGrid( // Hydrax parent pointer
                hydrax,
                // Noise module
                new MHydrax.MPerlin(),
                // Base plane
                new Mogre.Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0)),
                // Normal mode
                MMaterialManager.MNormalMode.NM_VERTEX,
                // Projected grid options
                new MProjectedGrid.MOptions(32, 10, 5, false, false, true, 3.75f));

        
            hydrax.SetModule(module);
            if(hydrax.LoadCfg(cfgFileName))
            {
              //  Console.WriteLine("OKOKOK");
            }
            hydrax.Create();
           

            //    MaterialPtr m = hydrax.MaterialManager.GetMaterial(MMaterialManager.MMaterialType.MAT_UNDERWATER_COMPOSITOR);
       //    m.GetBestTechnique().GetPass(0).GetTextureUnitState(1).SetTextureName("UnderwaterDistortion_none.png");
       //     m = null;
          //   hydrax.MaterialManager.RemoveCompositor();
        //   CompositorPtr com =  hydrax.MaterialManager.GetCompositor(MHydrax.MMaterialManager.MCompositorType.COMP_UNDERWATER); 
        //    com.RemoveAllTechniques();
        }

        /// <summary>
        /// Zwalnia zasoby
        /// </summary>
        public void DisposeHydrax()
        {
            if(hydrax != null)
            {
                if (USE_UPDATER_THREAD)
                {
                    Monitor.Enter(HydraxManager.Singleton); 
                    stopUpdater = true;
                    Monitor.Exit(HydraxManager.Singleton); 
                    updater.Join();

                    Monitor.Enter(HydraxManager.Singleton); 
                    stopUpdater = false;
                    Monitor.Exit(HydraxManager.Singleton); 
                }

                if(hydrax.IsCreated)
            	{
            		//hydrax.Visible = false;
            		//hydrax.UpdateLoop(0);
            	}
                hydrax.Dispose();
                hydrax = null;
            }

        }
    }
}
