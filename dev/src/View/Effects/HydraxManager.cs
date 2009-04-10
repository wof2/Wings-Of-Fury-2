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
        private string[] hydraxDepthMaterials = new string[] { "Island", "Concrete", "Steel", "Effects/Cloud1", "Effects/Cloud2" };

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
                                                new MProjectedGrid.MOptions(256, 35, 50, false, false, true, 3.75f));


            hydrax.SetModule(module);
            hydrax.LoadCfg(cfgFileName);
            hydrax.Create();
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
