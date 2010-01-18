/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using System.Collections;
using System.Collections.Generic;

using Mogre;
using System.Windows.Forms;
using Wof.Languages;
using Math = Mogre.Math;

namespace Wof.Controller
{
    /// <summary>
    /// Description of PerformanceTestFramework.
    /// </summary>
    public class PerformanceTestFramework : FrameWorkForm
    {

        private float testRunningTime = 2f;
        public enum GraphicsQuality
        {
            VeryLow, Low, Medium, UpperMedium, High, VeryHigh, Superb
        } ;
		protected float time = 0;
		protected AnimationState[] soldiersState;
	    private bool hasResults = false;

		public PerformanceTestFramework() : base()
		{
		}

	    private string fullScreen;
	    private string videoMode;

        private string antialiasing;

	    public string FullScreen
	    {
	        get { return fullScreen; }
	    }

	    public string VideoMode
	    {
	        get { return videoMode; }
	    }

        public string Antialiasing
        {
            get { return antialiasing; }
        }

	    public bool HasResults
	    {
	        get { return hasResults; }
	    }

	    public float GetAverageFPS()
		{
			if(window !=null) return window.AverageFPS;
			return 0;
		}

        public GraphicsQuality EstimateQualitySettingsAndWriteEngineConfig()
		{
            

            hasResults = true;
		    float fps = GetAverageFPS();
            EngineConfig.LoadEngineConfig();
            fullScreen = "Yes";
		    GraphicsQuality quality;
            bool hiEndVS = root.RenderSystem.Capabilities.IsShaderProfileSupported("vs_3_0");
           
            List<String> videoOptions= FrameWorkStaticHelper.GetVideoModes(true, 800, 600);
            
            
            if(videoOptions.Count == 0)
            {
            	videoOptions.Add("800 x 600 @ 32-bit colour");
            	MessageBox.Show("Your system is incapable of displaying 800x600 or better resolution. Trying to force 800x600","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			int maxVO = videoOptions.Count - 1;
           
            List<String> aaOptions = FrameWorkStaticHelper.GetAntialiasingModes();
            int maxAA = aaOptions.Count - 1;
            
            
            if(fps < 50)
            {
                // very low
                quality= GraphicsQuality.VeryLow;
                videoMode = videoOptions[0];
                antialiasing = aaOptions[0];
                EngineConfig.UseHydrax = false;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.None;
                EngineConfig.LowDetails = true;
                EngineConfig.BloomEnabled = false;
            } else 
            if(fps < 250)
            {
                // low
                quality = GraphicsQuality.Low;
                videoMode = videoOptions[(int)System.Math.Floor(maxVO * 0.10f)];
                antialiasing = aaOptions[0];
                EngineConfig.UseHydrax = false;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.None;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = false;
            }
            else 
            if (fps < 400)
            {
                // medium
                quality = GraphicsQuality.Medium;
                videoMode = videoOptions[(int)System.Math.Floor(maxVO * 0.20f)];
                antialiasing = aaOptions[0];
                EngineConfig.UseHydrax = false;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.Low;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = false;
            }
            else
            if (fps < 600)
            {
                // medium - better
                quality = GraphicsQuality.UpperMedium;
                videoMode = videoOptions[(int)System.Math.Floor(maxVO * 0.40f)];
                antialiasing = aaOptions[(int)System.Math.Floor(maxAA * 0.50f)];
                EngineConfig.UseHydrax = false;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.Low;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = false;
            }
            else
            if (fps < 750)
            {
                // high
                quality = GraphicsQuality.High;
                videoMode = videoOptions[(int)System.Math.Floor(maxVO * 0.75f)];
                antialiasing = aaOptions[(int)System.Math.Floor(maxAA * 0.75f)];
                EngineConfig.UseHydrax = hiEndVS;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.Low;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = false;
            }
            else
            if (fps < 900)
            {
                // higher
                quality = GraphicsQuality.VeryHigh;
                videoMode = videoOptions[(int)System.Math.Floor(maxVO * 0.85f)];
                antialiasing = aaOptions[(int)System.Math.Floor(maxAA * 0.85f)];
                EngineConfig.UseHydrax = hiEndVS;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.Medium;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = true;
            }
            else
            {
                // hi-end
                quality = GraphicsQuality.Superb;
                videoMode = videoOptions[maxVO];
                antialiasing = aaOptions[maxAA];
                EngineConfig.UseHydrax = hiEndVS;
                EngineConfig.ShadowsQuality = EngineConfig.ShadowsQualityTypes.High;
                EngineConfig.LowDetails = false;
                EngineConfig.BloomEnabled = true;
            }
            
            EngineConfig.SaveEngineConfig();

		    return quality;
		}


        protected override bool Configure()
		{
			    RenderSystemList renderSystems = root.GetAvailableRenderers();
                IEnumerator<RenderSystem> enumerator = renderSystems.GetEnumerator();

                // jako stan startowy moze zostac wybrany tylko directx system
                RenderSystem d3dxSystem = null;
				
                while (enumerator.MoveNext())
                {
                    RenderSystem renderSystem = enumerator.Current;
                    if (renderSystem.Name.Contains("Direct"))
                    {
                        d3dxSystem = renderSystem;
                        break;
                    }
                }

                
                root.RenderSystem = d3dxSystem;

             


                string fullScreen = "No";
                string videoMode = "800 x 600 @ 32-bit colour";

                d3dxSystem.SetConfigOption("Full Screen", fullScreen);
                d3dxSystem.SetConfigOption("Video Mode", videoMode);
                d3dxSystem.SetConfigOption("VSync", "No");
		        window = root.Initialise(true, "Wings Of Fury 2 - test");
                
            
               

                windowHeight = window.Height;
                windowWidth = window.Width;
                
         //       Resize += new EventHandler(OgreForm_Resize);
           //     root.SaveConfig();
                return true;
		}
		
		public override void ChooseSceneManager()
        {
            // Get the SceneManager, in this case a generic one
            sceneMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "SceneMgr");
	    }

        protected override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("mainCamera");
            camera.NearClipDistance = 1.0f;
		}


        protected override void CreateViewports()
        {
            // zwolnij zasoby
            if (viewport != null && CompositorManager.Singleton.HasCompositorChain(viewport)) CompositorManager.Singleton.RemoveCompositorChain(viewport);            
            window.RemoveAllViewports();
			CreateMainViewport(1, 0, 0, 1, 1);
            
	    }
		
        protected override void OnUpdateModel(FrameEvent evt)
        {
          
        }
        
        public override void CreateScene()
        { 
        	float rot, xpos, zpos, ypos;
        	    // SOLDIERS
            SceneNode soldierNode;
            Entity soldier;
            soldiersState = new AnimationState[16];
            int k = 0;
            for (int j = 0; j < 2; j++)
            {
            	if(j ==0)
            	{
            		xpos = -2;
            	} else
            	{
            		xpos = 2;
            	}
            	
	            for (int i = 0; i < soldiersState.Length / 2; i++)
	            {
	                soldier = sceneMgr.CreateEntity("Soldier" + k, "Soldier.mesh");
	               
	                rot = Mogre.Math.RangeRandom(-0.2f, 0.2f);
	                zpos = -i * 5;              
	                ypos = -1.0f;
	                xpos += Mogre.Math.RangeRandom(-0.2f, 0.2f);
	
	                soldierNode =
	                    SceneMgr.RootSceneNode.CreateChildSceneNode("SoldierNode" + k, new Vector3(xpos, ypos, zpos));
	                soldierNode.Rotate(Vector3.UNIT_Y, rot);
	                soldierNode.AttachObject(soldier);
	
					soldiersState[k] = soldier.GetAnimationState("run");
	                soldiersState[k].TimePosition = Mogre.Math.RangeRandom(0f, 1.0f);	  
	                soldiersState[k].Weight = Mogre.Math.RangeRandom(0.7f, 1.0f);	  
	                soldiersState[k].Loop = true;
	                soldiersState[k].Enabled = true;
	                k++;
	            }
	           
            }
        }

        protected override void ModelFrameStarted(FrameEvent evt)
        {
            
        }

        protected override bool FrameStarted(FrameEvent evt)
        {
        	time += evt.timeSinceLastFrame;
        	bool ret = base.FrameStarted(evt);
        	for (int i = 0; i < soldiersState.Length; i++)
            {
                soldiersState[i].AddTime(evt.timeSinceLastFrame);
            }
        	
        	if(time > testRunningTime)
        	{
                // sprawdz fpsy i wyestymuj ustawienia graficzne dla tego komputera
                EstimateQualitySettingsAndWriteEngineConfig();
                
        		return false;
        	}
        	
        	
        	
        	return ret;
        }
        
        
		protected override bool Setup()
        {  
            bool carryOn = false;        
            try
            {               
                root = new Root();
                LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME);
                LogManager.Singleton.LogMessage("Starting " + EngineConfig.C_GAME_NAME + " [performance test] ver. " + EngineConfig.C_WOF_VERSION);
                SetupResources("test_resources.cfg");

                carryOn = Configure();
             

                ConfigOptionMap map = root.RenderSystem.GetConfigOptions();
                if (map.ContainsKey("Rendering Device"))
                {
                    ConfigOptionMap.Iterator iterator = map.Find("Rendering Device");
                    if (iterator != null && !iterator.Value.IsNull)
                    {
                        LogManager.Singleton.LogMessage("Rendering device: " + iterator.Value.currentValue);
                    }
                }

                if (!carryOn) return false;
               
                LoadResources();
               



                ChooseSceneManager();
                CreateCamera();
                CreateViewports();                
      
            	// Create the scene
       
                CreateScene();
                WireEventListeners();

                
            }
            finally
            {              
                if (carryOn) window.SetVisible(true);
            }
            return true;
        }
	}
}
