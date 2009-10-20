using System;
using System.Collections.Generic;
using System.Text;
using MHydrax;
using Mogre;

namespace Wof.View.Effects
{
    /// <summary>
    /// Klasa odpowiedzialna za zarz¹dzanie hydraxem
    /// </summary>
    public class HydraxManager
    {
        protected MHydrax.MHydrax hydrax = null;
        private static readonly HydraxManager singleton = new HydraxManager();


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
             if(hydrax != null && hydrax.IsCreated)  hydrax.Update(evt.timeSinceLastFrame);
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
        	if(hydrax != null)
        	{
        		hydrax.Dispose();
        		hydrax = null;
        	}
            hydrax = new MHydrax.MHydrax(sceneMgr, camera, viewport);

            MProjectedGrid module = new MProjectedGrid(// Hydrax parent pointer
                                                hydrax,
                // Noise module
                                                new MHydrax.MPerlin(),
                // Base plane
                                                new Mogre.Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0)),
                // Normal mode
                                                MMaterialManager.MNormalMode.NM_VERTEX,
                // Projected grid options
                                                new MProjectedGrid.MOptions(164, 35, 50, false, false, true, 3.75f));

          
            hydrax.SetModule(module);
            hydrax.LoadCfg(cfgFileName);
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
            	if(hydrax.IsCreated)
            	{
            		//hydrax.Visible = false;
            		//hydrax.Update(0);
            	}
                hydrax.Dispose();
                hydrax = null;
            }

        }
    }
}
