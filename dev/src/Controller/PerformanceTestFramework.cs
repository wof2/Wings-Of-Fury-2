/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using System.Collections.Generic;
using Mogre;
using Wof.Languages;

namespace Wof.Controller
{
	/// <summary>
	/// Description of PerformanceTestFramework.
	/// </summary>
	public class PerformanceTestFramework : FrameWork
	{   
		protected float time = 0;
		protected AnimationState[] soldiersState;
		public PerformanceTestFramework() : base()
		{
		}
		
		public float GetAverageFPS()
		{
			if(window !=null) return window.AverageFPS;
			return 0;
		}
		
		public 
		
		public override bool Configure()
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
		
		public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("mainCamera");
            camera.NearClipDistance = 1.0f;
		}
		
		public override void CreateViewports()
        {
            // zwolnij zasoby
            if (viewport != null && CompositorManager.Singleton.HasCompositorChain(viewport)) CompositorManager.Singleton.RemoveCompositorChain(viewport);            
            window.RemoveAllViewports();
			CreateMainViewport(1, 0, 0, 1, 1);
            
	    }
		
        protected override void HandleInput(FrameEvent evt)
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
        public override bool FrameStarted(FrameEvent evt)
        {
        	time += evt.timeSinceLastFrame;
        	bool ret = base.FrameStarted(evt);
        	for (int i = 0; i < soldiersState.Length; i++)
            {
                soldiersState[i].AddTime(evt.timeSinceLastFrame);
            }
        	
        	if(time > 5)
        	{        	
        		return false;
        	}
        	
        	
        	
        	return ret;
        }
        
        
		public override bool Setup()
        {  
            bool carryOn = false;        
            try
            {               
                root = new Root();
                
                LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME);
                LogManager.Singleton.LogMessage("Starting Wings of Fury 2 [performance test] ver. " + EngineConfig.C_WOF_VERSION);

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
                CreateFrameListener();

                
            }
            finally
            {              
                if (carryOn) window.SetVisible(true);
            }
            return true;
        }
	}
}
