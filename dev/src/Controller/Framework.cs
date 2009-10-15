/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
 *
 * This file is part of Wings Of Fury 2.
 * 
 * Freeware Licence Agreement
 * 
 * This licence agreement only applies to the free version of this software.
 * Terms and Conditions
 * 
 * BY DOWNLOADING, INSTALLING, USING, TRANSMITTING, DISTRIBUTING OR COPYING THIS SOFTWARE ("THE SOFTWARE"), YOU AGREE TO THE TERMS OF THIS AGREEMENT (INCLUDING THE SOFTWARE LICENCE AND DISCLAIMER OF WARRANTY) WITH WINGSOFFURY2.COM THE OWNER OF ALL RIGHTS IN RESPECT OF THE SOFTWARE.
 * 
 * PLEASE READ THIS DOCUMENT CAREFULLY BEFORE USING THE SOFTWARE.
 *  
 * IF YOU DO NOT AGREE TO ANY OF THE TERMS OF THIS LICENCE THEN DO NOT DOWNLOAD, INSTALL, USE, TRANSMIT, DISTRIBUTE OR COPY THE SOFTWARE.
 * 
 * THIS DOCUMENT CONSTITUES A LICENCE TO USE THE SOFTWARE ON THE TERMS AND CONDITIONS APPEARING BELOW.
 * 
 * The Software is licensed to you without charge for use only upon the terms of this licence, and WINGSOFFURY2.COM reserves all rights not expressly granted to you. WINGSOFFURY2.COM retains ownership of all copies of the Software.
 * 1. Licence
 * 
 * You may use the Software without charge.
 *  
 * You may distribute exact copies of the Software to anyone.
 * 2. Restrictions
 * 
 * WINGSOFFURY2.COM reserves the right to revoke the above distribution right at any time, for any or no reason.
 *  
 * YOU MAY NOT MODIFY, ADAPT, TRANSLATE, RENT, LEASE, LOAN, SELL, REQUEST DONATIONS OR CREATE DERIVATE WORKS BASED UPON THE SOFTWARE OR ANY PART THEREOF.
 * 
 * The Software contains trade secrets and to protect them you may not decompile, reverse engineer, disassemble or otherwise reduce the Software to a humanly perceivable form. You agree not to divulge, directly or indirectly, until such trade secrets cease to be confidential, for any reason not your own fault.
 * 3. Termination
 * 
 * This licence is effective until terminated. The Licence will terminate automatically without notice from WINGSOFFURY2.COM if you fail to comply with any provision of this Licence. Upon termination you must destroy the Software and all copies thereof. You may terminate this Licence at any time by destroying the Software and all copies thereof. Upon termination of this licence for any reason you shall continue to be bound by the provisions of Section 2 above. Termination will be without prejudice to any rights WINGSOFFURY2.COM may have as a result of this agreement.
 * 4. Disclaimer of Warranty, Limitation of Remedies
 * 
 * TO THE FULL EXTENT PERMITTED BY LAW, WINGSOFFURY2.COM HEREBY EXCLUDES ALL CONDITIONS AND WARRANTIES, WHETHER IMPOSED BY STATUTE OR BY OPERATION OF LAW OR OTHERWISE, NOT EXPRESSLY SET OUT HEREIN. THE SOFTWARE, AND ALL ACCOMPANYING FILES, DATA AND MATERIALS ARE DISTRIBUTED "AS IS" AND WITH NO WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED. WINGSOFFURY2.COM DOES NOT WARRANT, GUARANTEE OR MAKE ANY REPRESENTATIONS REGARDING THE USE, OR THE RESULTS OF THE USE, OF THE SOFTWARE WITH RESPECT TO ITS CORRECTNESS, ACCURACY, RELIABILITY, CURRENTNESS OR OTHERWISE. THE ENTIRE RISK OF USING THE SOFTWARE IS ASSUMED BY YOU. WINGSOFFURY2.COM MAKES NO EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS INCLUDING, WITHOUT LIMITATION, THE WARRANTIES OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE WITH RESPECT TO THE SOFTWARE. NO ORAL OR WRITTEN INFORMATION OR ADVICE GIVEN BY WINGSOFFURY2.COM, IT'S DISTRIBUTORS, AGENTS OR EMPLOYEES SHALL CREATE A WARRANTY, AND YOU MAY NOT RELY ON ANY SUCH INFORMATION OR ADVICE.
 * 
 * IMPORTANT NOTE: Nothing in this Agreement is intended or shall be construed as excluding or modifying any statutory rights, warranties or conditions which by virtue of any national or state Fair Trading, Trade Practices or other such consumer legislation may not be modified or excluded. If permitted by such legislation, however, WINGSOFFURY2.COM' liability for any breach of any such warranty or condition shall be and is hereby limited to the supply of the Software licensed hereunder again as WINGSOFFURY2.COM at its sole discretion may determine to be necessary to correct the said breach.
 * 
 * IN NO EVENT SHALL WINGSOFFURY2.COM BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, AND THE LOSS OF BUSINESS INFORMATION OR COMPUTER PROGRAMS), EVEN IF WINGSOFFURY2.COM OR ANY WINGSOFFURY2.COM REPRESENTATIVE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. IN ADDITION, IN NO EVENT DOES WINGSOFFURY2.COM AUTHORISE YOU TO USE THE SOFTWARE IN SITUATIONS WHERE FAILURE OF THE SOFTWARE TO PERFORM CAN REASONABLY BE EXPECTED TO RESULT IN A PHYSICAL INJURY, OR IN LOSS OF LIFE. ANY SUCH USE BY YOU IS ENTIRELY AT YOUR OWN RISK, AND YOU AGREE TO HOLD WINGSOFFURY2.COM HARMLESS FROM ANY CLAIMS OR LOSSES RELATING TO SUCH UNAUTHORISED USE.
 * 5. General
 * 
 * All rights of any kind in the Software which are not expressly granted in this Agreement are entirely and exclusively reserved to and by WINGSOFFURY2.COM.
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using FSLOgreCS;
using Microsoft.DirectX.DirectSound;
using Mogre;
using MOIS;
using OIS;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.View.Effects;
using Exception = System.Exception;
using FontManager = Mogre.FontManager;
using InputManager = MOIS.InputManager;
using JoyStick = MOIS.JoyStick;
using Keyboard = MOIS.Keyboard;
using Math = Mogre.Math;
using Mouse = MOIS.Mouse;
using Plane = Wof.Model.Level.Planes.Plane;
using Timer=Mogre.Timer;
using Type = MOIS.Type;
using Vector3 = Mogre.Vector3;

namespace Wof.Controller
{
    /// <summary>
    /// Framework zapewniający podstawową funkcjonalność silnika
    /// <author>Adam Witczak</author>
    /// </summary>
    public abstract class FrameWork : Form
    {

        public float CameraZoom
        {
            get { return cameraZoom; }
            set { cameraZoom = value; }
        }
        protected float cameraZoom = 0;


        protected float minimapHeight = 0.14f;
        protected float minimapWidth = 0.3f;

        private PerformanceTestFramework performanceTestFramework;

        protected static bool displayMinimap = true;

        public static bool DisplayMinimap
        {
            get { return displayMinimap; }
        }

        public void SetDisplayMinimap(bool enabled)
        {
            displayMinimap = enabled;
        }

        private BackgroundWorker modelWorker = new BackgroundWorker();

        protected Root root;
        protected Camera camera, minimapCamera, minimapNoseCamera, overlayCamera;

        public Camera Camera
        {
            get { return camera; }
        }

        public Camera MinimapCamera
        {
            get { return minimapCamera; }
        }

        public Camera MinimapNoseCamera
        {
            get { return minimapNoseCamera; }
        }

        public Camera OverlayCamera
        {
            get { return overlayCamera; }
        }

        protected Viewport viewport, minimapViewport, overlayViewport;

        public Viewport Viewport
        {
            get { return viewport; }
        }

        public Viewport MinimapViewport
        {
            get { return minimapViewport; }
        }

        public Viewport OverlayViewport
        {
            get { return overlayViewport; }
        }

        protected static SceneManager sceneMgr, minimapMgr, overlayMgr;

        public static SceneManager SceneMgr
        {
            get { return sceneMgr; }
        }

        public static SceneManager MinimapMgr
        {
            get { return minimapMgr; }
        }

        public static SceneManager OverlayMgr
        {
            get { return overlayMgr; }
        }

        protected RenderWindow window;

        public RenderWindow Window
        {
            get { return window;}
        }
        protected uint windowWidth;
        protected uint windowHeight;

        protected Overlay debugOverlay;
        protected InputManager inputManager;
        protected Keyboard inputKeyboard;
        protected JoyStick inputJoystick;
        protected Mouse inputMouse;

        protected bool showDebugOverlay = true;
        protected float debugTextDelay = 0.0f;
        protected float toggleDelay = 1.0f;
        protected float statDelay = 0.0f;
        protected TextureFilterOptions filtering = TextureFilterOptions.TFO_BILINEAR;
        protected uint aniso = 1;


        protected static float camSpeed = 50f;
        protected static Degree rotateSpeed = 36;

        protected bool shutDown = false;
        protected string mDebugText;

        // urzadzenie DirectSound
        protected Device directSound;


        public static void ShowOgreException()
        {
            if (OgreException.IsThrown)
                MessageBox.Show(OgreException.LastException.FullDescription, "Wings of Fury 2 - Engine error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWofException(Exception ex)
        {
            MessageBox.Show(ex.Message + "\r\n" + "Stack trace: "+ex.StackTrace, "Wings of Fury 2 - Runtime error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public virtual void Go()
        {
            if (!Setup())
                return;
            modelWorker.WorkerSupportsCancellation = true;
            modelWorker.RunWorkerAsync();
            root.StartRendering();
            
            // clean up
            modelWorker.CancelAsync();

            if (Game.getGame() != null && Game.getGame().CurrentScreen != null)
            {
                Game.getGame().CurrentScreen.CleanUp(false);
            }
                         
            

            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CleanUp");
           
            if(!(this is PerformanceTestFramework))
            {
                EffectsManager.Singleton.Clear();
            }
            DestroyScenes();
            TextureManager.Singleton.UnloadAll();
            MaterialManager.Singleton.UnloadAll();
            CompositorManager.Singleton.RemoveAll();
            CompositorManager.Singleton.UnloadAll();
            MeshManager.Singleton.UnloadAll();
            FontManager.Singleton.UnloadAll();
            GpuProgramManager.Singleton.UnloadAll();
            HighLevelGpuProgramManager.Singleton.UnloadAll();
            window.RemoveAllListeners();
            window.RemoveAllViewports(); 
            Root.Singleton.RenderSystem.DestroyRenderWindow(window.Name);
         
            window.Dispose();
            window = null;

            ResourceGroupManager.Singleton.Dispose();
            root.Shutdown();
            root.Dispose();
            root = null;

        
            //Console.ReadLine();
        }
       


        public void InjectPerformanceTestResults(PerformanceTestFramework performanceTestFramework)
        {
            this.performanceTestFramework = performanceTestFramework;
        }
       
        public virtual bool Setup()
        {
           
            Splash splash = new Splash();
            splash.Show();
            splash.SetStepsCount(10);
            bool carryOn = false;
            string splashFormat = "{0}...";
            try
            {
                splash.Increment(
                    String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingTheRootObject)));
               

                root = new Root();
              
                //LogManager.Singleton.SetLogDetail(LoggingLevel.LL_LOW);
               // LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME);
                LogManager.Singleton.LogMessage("Starting Wings of Fury 2 ver. " + EngineConfig.C_WOF_VERSION);


                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.SetupingResources)));
                SetupResources();
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


                /*foreach (KeyValuePair<string,ConfigOption_NativePtr> m in map)
                {
                    Console.WriteLine(m.Value.name);
                    foreach (String s in m.Value.possibleValues)
                    {
                        Console.WriteLine("\t"+s);
                    }
                }*/


                if (!carryOn) return false;
                splash.Activate();
              
                
                // Set default mipmap level (NB some APIs ignore this)
                // TextureManager.Singleton.DefaultNumMipmaps = 32;
                // Create any resource listeners (for loading screens)
                CreateResourceListener();
                // Load resources
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.LoadingResources)));
                LoadResources();
              
                splash.Increment(
                  String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingGameObjects)));

                if (!EngineConfig.DebugStart)
                {
                    ChooseSceneManager();
                    CreateCamera();
                    CreateViewports();
                }


              
                splash.Increment(4);


                // load presets


                // InitializeSound sound
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.InitializingSound)));
                InitDirectSound(splash.Handle);
                SetupEngineConfig();
                
                
                if (!EngineConfig.DebugStart)
                {
                    // Jesli jest debugstart to nie ma jeszcze kamery wiec nie moge zrobic sound system. Zrobi sie samo przy StartGame()
                    if (!CreateSoundSystem(camera, EngineConfig.SoundSystem))
                        EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;
                }
            


            // Create the scene
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingScene)));
                CreateScene();

                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.AddingCompositors)));
               // AddCompositors();
                // SetCompositorEnabled(CompositorTypes.OLDMOVIE, true);

                CreateFrameListener();

                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingInput)));
                CreateInput();
            }
            finally
            {
                splash.Close();
                splash.Dispose();
                if (carryOn) window.SetVisible(true);
            }
            return true;
        }

        private void SetupEngineConfig()
        {

            EngineConfig.LoadEngineConfig();
            SoundManager.Instance.SoundDisabled = !EngineConfig.SoundEnabled;
        }

        protected void InitDirectSound(IntPtr handle)
        {
            try
            {
                directSound = new Device();

                // po co mu ten handle?? 
                directSound.SetCooperativeLevel(handle, CooperativeLevel.Normal);

                SoundManager.DsDevice = directSound;
                SoundManager manager = SoundManager.Instance;
            }
            catch (Exception ex)
            {
                if(EngineConfig.SoundEnabled) LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Problem with sound: "+ex.Message);
                SoundManager.Instance.ProblemWithSound = true;
            }
        }


        private void OgreForm_Resize(object sender, EventArgs e)
        {
            //   window.WindowMovedOrResized( );
        }
        
      


        /// <summary>
        /// Configures the application - returns false if the user chooses to abandon configuration.
        /// </summary>
        public virtual bool Configure()
        {
            if (root.RestoreConfig())
            {
                //ResourceBackgroundQueue.Singleton.StartBackgroundThread = true;
                //ResourceBackgroundQueue.Singleton.Initialise();
                //ResourceBackgroundQueue.Singleton.InitialiseAllResourceGroups();
                try
                {
                    window = root.Initialise(true, "Wings Of Fury 2");
                
                }
                catch (Exception)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Unable to initialize requested display device. Deleting ogre.cfg (have you change your graphics card? Please try to run game again)");
                    File.Delete("ogre.cfg");
                    throw;
                }
             
                windowHeight = window.Height;
                windowWidth = window.Width;
                //window.SetVisible(false);
                return true;
            }

           
           
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
            
     
            string fullScreen = "Yes";
            string videoMode = "800 x 600 @ 32-bit colour";

            if (performanceTestFramework != null && performanceTestFramework.HasResults)
            {
                fullScreen = performanceTestFramework.FullScreen;
                videoMode = performanceTestFramework.VideoMode;
            }
            
            d3dxSystem.SetConfigOption("Full Screen", fullScreen);
            d3dxSystem.SetConfigOption("Video Mode", videoMode);
           

          
            
    
          	
            window = root.Initialise(true, "Wings Of Fury 2");
           
            windowHeight = window.Height;
            windowWidth = window.Width;
            
            Resize += new EventHandler(OgreForm_Resize);
            root.SaveConfig();
            return true;
           
        }


        public virtual void ChooseSceneManager()
        {
            // Get the SceneManager, in this case a generic one
            sceneMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "SceneMgr");

            if (EngineConfig.ShadowsQuality > 0)
            {            	
            	sceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_ADDITIVE; 
            	FocusedShadowCameraSetup cam = new FocusedShadowCameraSetup();                       	
            	sceneMgr.SetShadowCameraSetup(new ShadowCameraSetupPtr(cam));
            	
            //sceneMgr.SetShadowCameraSetup(new ShadowCameraSetupPtr(new DefaultShadowCameraSetup()));
           	//  LiSPSMShadowCameraSetup cam = new LiSPSMShadowCameraSetup();
            //	cam.OptimalAdjustFactor = 3.0f;
            //	sceneMgr.SetShadowCameraSetup(new ShadowCameraSetupPtr(new LiSPSMShadowCameraSetup()));
            	//sceneMgr.SetShadowCameraSetup(new ShadowCameraSetupPtr(new  PSSMShadowCameraSetup()));
            	
        
              
            	sceneMgr.SetShadowTextureCasterMaterial("Ogre/DepthShadowmap/Caster/Float");
         	//	sceneMgr.SetShadowTextureReceiverMaterial("Ogre/DepthShadowmap/Receiver/Float");
		
				sceneMgr.ShadowTextureSelfShadow = (true);	
			    sceneMgr.ShadowFarDistance = 140;
				switch(EngineConfig.ShadowsQuality)
				{
					case EngineConfig.ShadowsQualityTypes.Low:
						 sceneMgr.SetShadowTextureSettings(512, 1);
						 sceneMgr.ShadowFarDistance *= 0.8f;
					break;
					
					case EngineConfig.ShadowsQualityTypes.Medium:
						 sceneMgr.SetShadowTextureSettings(1024, 1);
					break;
					
					case EngineConfig.ShadowsQualityTypes.High:
						 sceneMgr.SetShadowTextureSettings(1024, 1);
						 sceneMgr.ShadowFarDistance *= 1.3f;
					break;					
						
				}				
				
            }


            minimapMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "MinimapMgr");
            overlayMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "OverlayMgr");
        }


        public void CreateMinimapCamera()
        {
            minimapCamera = minimapMgr.CreateCamera("minimapCamera");
            minimapCamera.Position = new Vector3(0, 90, 250);

            minimapCamera.LookAt(new Vector3(0, 75, 0));
            minimapCamera.NearClipDistance = 0.0001f;
            minimapCamera.FarClipDistance = 300.0f;
        }

        public void CreateMinimapNoseCamera()
        {
            minimapNoseCamera = sceneMgr.CreateCamera("minimapNoseCamera");
            minimapNoseCamera.Position = new Vector3(0, 0, -3.0f);
            minimapNoseCamera.UseIdentityProjection = true;

            minimapNoseCamera.LookAt(new Vector3(0, 0, -5));
            minimapNoseCamera.NearClipDistance = 0.0001f;
            minimapNoseCamera.FarClipDistance = 300.0f;
        }


        public void CreateOverlayCamera()
        {
            overlayCamera = overlayMgr.CreateCamera("overlayCamera");

            overlayCamera.Position = new Vector3(0, 0, 0);
            overlayCamera.LookAt(new Vector3(0, 0, -1));
            overlayCamera.NearClipDistance = 0.5f;
            overlayCamera.FarClipDistance = 100.0f;
        }

        protected static bool CreateSoundSystem(Camera camera, FreeSL.FSL_SOUND_SYSTEM ss)
        {
            return SoundManager3D.Instance.InitializeSound(camera, ss);
        }

        public virtual void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("mainCamera");

            camera.NearClipDistance = 1.0f;
            //camera.FarClipDistance = 500.0f;


            if (displayMinimap)
            {
                CreateMinimapCamera();
            }
            CreateMinimapNoseCamera();
            CreateOverlayCamera();
        }

       

       
       

        public virtual void CreateFrameListener()
        {
            modelWorker.DoWork += LoopModelWorker;
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
          
            root.RenderSystem.EventOccurred +=
                new RenderSystem.Listener.EventOccurredHandler(RenderSystem_EventOccurred);
            SoundManager3D.Instance.CreateFrameListener(root);
        }

        private void RenderSystem_EventOccurred(string eventName, Const_NameValuePairList parameters)
        {
            if (eventName.Equals("DeviceLost"))
                window.WindowMovedOrResized();
        }

       

        public virtual void LoopModelWorker(object sender, EventArgs args)
        {
            int frameTime = 20; // 20 ms -> 50 fps
            
            FrameEvent evt = new FrameEvent();
            Timer timer =new Timer();
            timer.Reset();          
            uint start, end;
            start = timer.Milliseconds;
             
            while(!modelWorker.CancellationPending)
            {            	
                evt.timeSinceLastFrame = (timer.Milliseconds - start) / 1000.0f; // w sekundach                       
                start = timer.Milliseconds;
                ModelFrameStarted(evt);
                end = timer.Milliseconds;  
                
                int duration = (int)(end - start);
                
                int sleepTime = frameTime - duration;
                if(sleepTime <0) sleepTime = 1;
                
                Thread.Sleep(sleepTime);
            }
        }



        public virtual void ModelFrameStarted(FrameEvent evt)
        {
            OnUpdateModel(evt);
        }

        public virtual bool FrameStarted(FrameEvent evt)
        {
            if (window.IsClosed)
                return false;


            return !shutDown;
        }

        public void HandleCameraInput(Keyboard inputKeyboard, Mouse inputMouse, JoyStick inputJoystick, FrameEvent evt, Camera camera,
                                      Camera minimapCamera, Plane playerPlane)
        {
            // Move about 100 units per second,
            float moveScale = camSpeed*evt.timeSinceLastFrame;
            // Take about 10 seconds for full rotation

            Vector3 translateVector = Vector3.ZERO;

            // set the scaling of camera motion

            MouseState_NativePtr mouseState = inputMouse.MouseState;


            if (EngineConfig.ManualCamera && camera != null)
            {
                if (inputKeyboard.IsKeyDown(KeyCode.KC_A))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                        translateVector.x = -3*moveScale;
                    else
                        translateVector.x = -moveScale;
                }

                if (inputKeyboard.IsKeyDown(KeyCode.KC_D))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                        translateVector.x = 3*moveScale;
                    else
                        translateVector.x = moveScale;
                }

                if (inputKeyboard.IsKeyDown(KeyCode.KC_W))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                        translateVector.z = - 3*moveScale;
                    else
                        translateVector.z = - moveScale;
                }

                if (inputKeyboard.IsKeyDown(KeyCode.KC_S))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                        translateVector.z = 3*moveScale;
                    else
                        translateVector.z = moveScale;
                }

                Degree cameraYaw = -mouseState.X.rel*.13f;
                Degree cameraPitch = -mouseState.Y.rel*.13f;

                camera.Yaw(cameraYaw);
                camera.Pitch(cameraPitch);
                camera.MoveRelative(translateVector);

                // ZOOM IN
                if (camera.Position.z > 20 && mouseState.Z.rel > 0)
                {
                    camera.Position += new Vector3(0, 0, -mouseState.Z.rel*.05f);
                }

                // ZOOM OUT
                if (camera.Position.z < 60 && mouseState.Z.rel < 0)
                {
                    camera.Position += new Vector3(0, 0, -mouseState.Z.rel*.05f);
                }
            }
            else
            {
                if (camera != null)
                {

                    // ZOOM IN
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_PGUP) || mouseState.Z.rel > 0)
                    {
                        if (cameraZoom < 25)
                        {
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_PGUP))
                            {
                                cameraZoom += 1.2f;
                            }
                            else
                            {
                                cameraZoom += mouseState.Z.rel * .02f;
                            }
                        }
                    }

                    // ZOOM IN
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_PGDOWN) || mouseState.Z.rel < 0)
                    {
                        if (cameraZoom > -25)
                        {
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_PGDOWN))
                            {
                                cameraZoom -= 1.2f;
                                
                            }
                            else
                            {
                                cameraZoom += mouseState.Z.rel * .02f;
                            }
                        }
                    }
                
                  

                    // ZOOM OUT
                  /*  if (camera.Position.z < 60 && mouseState.Z.rel < 0 && cameraZoom < 10)
                    {
                        cameraZoom -= mouseState.Z.rel * .02f;
                    }*/

                    CameraMotionManager.Manage(camera, playerPlane, evt, cameraZoom);
                }
             
                if (minimapCamera != null) CameraMotionManager.ManageMini(minimapCamera, playerPlane, evt);
            }


            // ustawienie kata
            /*if (playerPlane != null)
            {

                Vector2 target = UnitConverter.LogicToWorldUnits(playerPlane.Center);
                camera.LookAt(target.x, target.y, 0);
            }*/
        }


        protected virtual void OnUpdateModel(FrameEvent evt)
        {
          
           
            Degree scaleRotate = rotateSpeed*evt.timeSinceLastFrame;


            inputKeyboard.Capture();
            if(inputJoystick != null) inputJoystick.Capture();

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Escape) || GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEscape)) 
            {
                // stop rendering loop
                shutDown = true;
            }
            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Left))
            {
                camera.Yaw(scaleRotate);
            }

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Right))
            {
                camera.Yaw(-scaleRotate);
            }

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) )
            {
                camera.Pitch(scaleRotate);
            }

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down))
            {
                camera.Pitch(-scaleRotate);
            }

            if (inputJoystick !=null)
            {
                Vector2 joyVector = GetJoystickVector(inputJoystick);
                if (joyVector.x != 0) camera.Yaw(-joyVector.x * scaleRotate);
                if (joyVector.y != 0) camera.Pitch(joyVector.y * scaleRotate);
            }



            // subtract the time since last frame to delay specific key presses
            toggleDelay -= evt.timeSinceLastFrame;

            // toggle rendering mode
            if (inputKeyboard.IsKeyDown(KeyCode.KC_R) && toggleDelay < 0)
            {
                if (camera.PolygonMode == PolygonMode.PM_POINTS)
                {
                    camera.PolygonMode = PolygonMode.PM_SOLID;
                }
                else if (camera.PolygonMode == PolygonMode.PM_SOLID)
                {
                    camera.PolygonMode = PolygonMode.PM_WIREFRAME;
                }
                else
                {
                    camera.PolygonMode = PolygonMode.PM_POINTS;
                }

                Console.WriteLine("Rendering mode changed to '{0}'.", camera.PolygonMode);

                toggleDelay = 1;
            }

            if (inputKeyboard.IsKeyDown(KeyCode.KC_T) && toggleDelay < 0)
            {
                // toggle the texture settings
                switch (filtering)
                {
                    case TextureFilterOptions.TFO_BILINEAR:
                        filtering = TextureFilterOptions.TFO_TRILINEAR;
                        aniso = 1;
                        break;
                    case TextureFilterOptions.TFO_TRILINEAR:
                        filtering = TextureFilterOptions.TFO_ANISOTROPIC;
                        aniso = 8;
                        break;
                    case TextureFilterOptions.TFO_ANISOTROPIC:
                        filtering = TextureFilterOptions.TFO_BILINEAR;
                        aniso = 1;
                        break;
                }

                Console.WriteLine("Texture Filtering changed to '{0}'.", filtering);

                // set the new default
                MaterialManager.Singleton.SetDefaultTextureFiltering(filtering);
                MaterialManager.Singleton.DefaultAnisotropy = aniso;

                toggleDelay = 1;
            }

            if (inputKeyboard.IsKeyDown(KeyCode.KC_SYSRQ))
            {
                string[] temp = Directory.GetFiles(Environment.CurrentDirectory, "screenshot*.jpg");
                string fileName = string.Format("screenshot{0}.jpg", temp.Length + 1);

                TakeScreenshot(fileName);

                // show briefly on the screen
                mDebugText = string.Format("Wrote screenshot '{0}'.", fileName);

                // show for 2 seconds
                debugTextDelay = 2.0f;
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_B))
            {
                sceneMgr.ShowBoundingBoxes = !sceneMgr.ShowBoundingBoxes;
            }

            if (inputKeyboard.IsKeyDown(KeyCode.KC_F))
            {
                // hide all overlays, includes ones besides the debug overlay
                viewport.OverlaysEnabled = !viewport.OverlaysEnabled;
            }

            // update performance stats once per second
            if (statDelay < 0.0f && showDebugOverlay)
            {
                UpdateStats();
                statDelay = 1.0f;
            }
            else
            {
                statDelay -= evt.timeSinceLastFrame;
            }

            // turn off debug text when delay ends
            if (debugTextDelay < 0.0f)
            {
                debugTextDelay = 0.0f;
                mDebugText = "";
            }
            else if (debugTextDelay > 0.0f)
            {
                debugTextDelay -= evt.timeSinceLastFrame;
            }
            inputMouse.Capture();
            HandleCameraInput(inputKeyboard, inputMouse, inputJoystick, evt, camera, minimapCamera, null);
        }

        protected void TakeScreenshot()
        {
            string[] temp = Directory.GetFiles(Environment.CurrentDirectory, "screenshot*.jpg");
            string fileName = string.Format("screenshot{0}.jpg", temp.Length + 1);

            TakeScreenshot(fileName);
        }

        public void TakeScreenshot(string fileName)
        {
            window.WriteContentsToFile(fileName);
        }

        public string UpdateStats()
        {
            string fpsFormat = "{0}: {1} ";
            string avgFps = String.Format("{0}:", LanguageResources.GetString(LanguageKey.AverageFPS));
            string worstFps = String.Format("{0}:", LanguageResources.GetString(LanguageKey.WorstFPS));
            string tris = String.Format("{0}:", LanguageResources.GetString(LanguageKey.TriangleCount));
            // update stats when necessary
            try
            {
                OverlayElement guiAvg = OverlayManager.Singleton.GetOverlayElement("Core/AverageFps");
                OverlayElement guiCurr = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps");
                OverlayElement guiBest = OverlayManager.Singleton.GetOverlayElement("Core/BestFps");
                OverlayElement guiWorst = OverlayManager.Singleton.GetOverlayElement("Core/WorstFps");

                RenderTarget.FrameStats stats = window.GetStatistics();

                guiAvg.Caption = avgFps + stats.AvgFPS;
                guiCurr.Caption =
                    String.Format(fpsFormat, LanguageResources.GetString(LanguageKey.CurrentFPS), stats.LastFPS);
                guiBest.Caption =
                    String.Format(fpsFormat, LanguageResources.GetString(LanguageKey.BestFPS),
                                  stats.BestFPS + " " + stats.BestFrameTime + " ms");
                guiWorst.Caption = worstFps + stats.WorstFPS + " " + stats.WorstFrameTime + " ms";


                OverlayElement guiTris = OverlayManager.Singleton.GetOverlayElement("Core/NumTris");
                guiTris.Caption = tris + stats.TriangleCount;


                return
                    avgFps + " " + stats.AvgFPS + "|| " + worstFps + stats.WorstFPS + "|| " + tris + " " +
                    stats.TriangleCount;
            }

            catch
            {
                // ignore
            }
            return String.Empty;
        }

        public virtual bool UseBufferedInput
        {
            get { return false; }
        }

       

        public virtual void CreateInput()
        {
            LogManager.Singleton.LogMessage("*** Initializing OIS ***");

          

            ParamList pl = new ParamList();
            IntPtr windowHnd;
            window.GetCustomAttribute("WINDOW", out windowHnd);
            pl.Insert("WINDOW", windowHnd.ToString());

            inputManager = InputManager.CreateInputSystem(pl);

            //Create all devices (We only catch joystick exceptions here, as, most people have Key/Mouse)
            inputKeyboard = (Keyboard) inputManager.CreateInputObject(Type.OISKeyboard, UseBufferedInput);

            try
            {
                inputJoystick = (JoyStick)inputManager.CreateInputObject(Type.OISJoyStick, UseBufferedInput);
            }
            catch(Exception)
            {
                inputJoystick = null; 
            }
           

            inputMouse = (Mouse) inputManager.CreateInputObject(Type.OISMouse, UseBufferedInput);
        }

        public static bool GetJoystickButton(JoyStick j, int button)
        {
            if(j!=null)
            {
                if ((int)button - 1 < j.JoyStickState.ButtonCount)// indexed from 0
                {
                    try
                    {
                        return j.JoyStickState.GetButton((int)button - 1); 
                    }
                    catch (Exception ex)
                    {
                        LogManager.Singleton.LogMessage("Unable to read joystick button state (" + button +"), please check your joystick and Keymap.ini file");
                        return false;
                    }
                  
                }
            }
            return false;
        }

        public static Vector2 GetJoystickVector(JoyStick j)
        {
            if(j!=null)
            {
                if(j.JoyStickState.VectorCount > 0)
                {
                    MOIS.Vector3 v = j.JoyStickState.GetVector(0);
                    return new Vector2(v.x, v.y);
                } else 
                {
                   
                    int num = j.JoyStickState.AxisCount;
                    if(num >= 2)
                    {
                        int axisCount = j.JoyStickState.AxisCount;

                        if (KeyMap.Instance.JoystickVerticalAxisNo > axisCount - 1)
                        {
                            throw new Exception("JoystickVerticalAxisNo is greater than number of axes. Please change it in your KeyMap.ini");
                        }

                        if (KeyMap.Instance.JoystickHorizontalAxisNo > axisCount - 1)
                        {
                            throw new Exception("JoystickHorizontalAxisNo is greater than number of axes. Please change it in your KeyMap.ini");
                        }


                        Axis_NativePtr axisV = j.JoyStickState.GetAxis(KeyMap.Instance.JoystickVerticalAxisNo);
                        Axis_NativePtr axisH = j.JoyStickState.GetAxis(KeyMap.Instance.JoystickHorizontalAxisNo);
                     

                        double v = (1.0 * axisV.abs / JoyStick.MAX_AXIS);
                        double h = (1.0 * axisH.abs / JoyStick.MAX_AXIS);

                       // Console.WriteLine(h + " " + v);
                     
                      
                        if (System.Math.Abs(v) < KeyMap.Instance.JoystickDeadZone) v = 0;
                        else if (v > 1) v = 1;
                        else if (v < -1) v = -1;

                        if (System.Math.Abs(h) < KeyMap.Instance.JoystickDeadZone) h = 0;
                        else if (h > 1) h = 1;
                        else if (h < -1) h = -1;
                     
                      
                        return new Vector2((float)h, (float)-v);
                    } else
                    {
                        // no joys and no POVs 
                        return Vector2.ZERO;
                    }
                }
            } else
            {
                return Vector2.ZERO;
            }

        }


        public abstract void CreateScene(); // pure virtual - this has to be overridden

        public static void DestroyScenes()
        {

            if (sceneMgr != null)
            {
                sceneMgr.DestroyAllBillboardSets();
                sceneMgr.DestroyAllEntities();
                sceneMgr.DestroyAllManualObjects();
                sceneMgr.DestroyAllInstancedGeometry();
                sceneMgr.DestroyAllMovableObjects();
                sceneMgr.ClearScene();
                sceneMgr.DestroyAllCameras();
                sceneMgr.Dispose();
                Root.Singleton.DestroySceneManager(sceneMgr);
                sceneMgr = null;
            }
            if (minimapMgr != null)
            {
                minimapMgr.DestroyAllCameras();
                minimapMgr.DestroyAllEntities();
                minimapMgr.DestroyAllEntities();
                minimapMgr.ClearScene();
                Root.Singleton.DestroySceneManager(minimapMgr);
                minimapMgr = null;
            }

            if (overlayMgr != null)
            {
                overlayMgr.DestroyAllCameras();
                overlayMgr.ClearScene();
                Root.Singleton.DestroySceneManager(overlayMgr);
                overlayMgr = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        } // Optional to override this


        public void CreateMinimapViewport(int zOrder, float left, float top, float width, float height)
        {
            // MINIMAP
            if (minimapViewport != null)
            {
                minimapViewport.Dispose();
                minimapViewport = null;
            }

            minimapViewport = window.AddViewport(minimapCamera, zOrder, left, top, width, height);
            minimapViewport.BackgroundColour = new ColourValue(0, 0, 0);

            // Alter the camera aspect ratio to match the viewport
            minimapCamera.AspectRatio = ( minimapViewport.ActualWidth)/((float) minimapViewport.ActualHeight);
            minimapNoseCamera.AspectRatio = 14.0f/4.0f;

            minimapViewport.OverlaysEnabled = false;
         //   Overlay debugo = OverlayManager.Singleton.GetByName("Wof/Debug");
        }

        public void CreateMainViewport(int zOrder, float left, float top, float width, float height)
        {
            if (viewport != null)
            {
                viewport.Dispose();
                viewport = null;
            }

            viewport = window.AddViewport(camera, zOrder, left, top, width, height);
            viewport.BackgroundColour = new ColourValue(0, 0, 0);

            // Alter the camera aspect ratio to match the viewport
            camera.AspectRatio = ( viewport.ActualWidth)/((float) viewport.ActualHeight);
            viewport.OverlaysEnabled = false;
        }

        public void CreateOverlayViewport(int zOrder, float left, float top, float width, float height)
        {
            if (overlayViewport != null)
            {
                overlayViewport.Dispose();
                overlayViewport = null;
            }

            overlayViewport = window.AddViewport(overlayCamera, zOrder, left, top, width, height);
            overlayViewport.BackgroundColour = new ColourValue(0, 0, 0);
            overlayViewport.SetClearEveryFrame(false);
            // Alter the camera aspect ratio to match the viewport
            if (overlayCamera != null)
            {
                overlayCamera.AspectRatio = overlayViewport.ActualWidth/((float) overlayViewport.ActualHeight);
            }
            overlayViewport.OverlaysEnabled = true;
        }

        public virtual void CreateViewports()
        {
            // zwolnij zasoby
            if (viewport != null && CompositorManager.Singleton.HasCompositorChain(viewport)) CompositorManager.Singleton.RemoveCompositorChain(viewport);
            
            window.RemoveAllViewports();

            if (displayMinimap)
            {
                CreateMainViewport(1, 0, 0, 1.0f, 1 - minimapHeight);
                float left = 0;

                minimapWidth = 1.0f;
                CreateMinimapViewport(0, left, 1 - minimapHeight, minimapWidth, minimapHeight);
            }
            else
            {
                CreateMainViewport(1, 0, 0, 1, 1);
            }

            CreateOverlayViewport(2, 0, 0, 1, 1);
            // 
        }

        public virtual void SetupResources()
        {
        	SetupResources("resources.cfg");
        }
        
        /// Method which will define the source of resources (other than current folder)
        public virtual void SetupResources(string cfgFilename)
        {
            // Load resource paths from config file
            ConfigFile cf = new ConfigFile();
            cf.Load(cfgFilename, "\t:=", true);

            // Go through all sections & settings in the file
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();

            String secName, typeName, archName;

            // Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
        }

        /// Optional override method where you can create resource listeners (e.g. for loading screens)
        public virtual void CreateResourceListener()
        {
        }

        /// Optional override method where you can perform resource group loading
        /// Must at least do ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        public virtual void LoadResources()
        {
            // Initialise, parse scripts etc
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        }


        /// <summary>
        /// COMPOSITORS
        /// </summary>
        public enum CompositorTypes
        {
            OLDMOVIE,
            BLOOM,
            MOTION_BLUR,
            GAUSSIAN_BLUR,
            BW
        } ;

        public void SetCompositorEnabled(CompositorTypes type, bool enabled)
        {
        
            String name = null;
            switch (type)
            {
                case CompositorTypes.BLOOM:
                    name = "Bloom";
                    break;

                case CompositorTypes.OLDMOVIE:
                    name = "Old Movie";
                    break;
                    
                case CompositorTypes.MOTION_BLUR:
                    name = "Motion Blur";
                    break;
                    
                case CompositorTypes.GAUSSIAN_BLUR:
                    name = "Gaussian Blur";
                    break; 
                    
                case CompositorTypes.BW:
                    name = "B&W";
                    break;   
                    
                   
            }
            if (CompositorManager.Singleton.ResourceExists(name))
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, name, enabled);
            }
        }

        private void CreateMotionBlurCompositor(Viewport viewport)
        {
            CompositorPtr comp3 = CompositorManager.Singleton.Create("Motion Blur", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            {
                CompositionTechnique t = comp3.CreateTechnique();
                {
                    CompositionTechnique.TextureDefinition_NativePtr def = t.CreateTextureDefinition("scene");
                    def.width = 0;
                    def.height = 0;
                    def.format = PixelFormat.PF_R8G8B8;
                }
                {
                    CompositionTechnique.TextureDefinition_NativePtr def = t.CreateTextureDefinition("sum");
                    def.width = 0;
                    def.height = 0;
                    def.format = PixelFormat.PF_R8G8B8;
                }
                {
                    CompositionTechnique.TextureDefinition_NativePtr def = t.CreateTextureDefinition("temp");
                    def.width = 0;
                    def.height = 0;
                    def.format = PixelFormat.PF_R8G8B8;
                }
                /// Render scene
                {
                    CompositionTargetPass tp = t.CreateTargetPass();
                    tp.SetInputMode(CompositionTargetPass.InputMode.IM_PREVIOUS);
                    tp.OutputName = "scene";
                }
                /// Initialisation pass for sum texture
                {
                    CompositionTargetPass tp = t.CreateTargetPass();
                    tp.SetInputMode(CompositionTargetPass.InputMode.IM_PREVIOUS);
                    tp.OutputName = "sum";
                    tp.OnlyInitial = true;
                }
                /// Do the motion blur
                {
                    CompositionTargetPass tp = t.CreateTargetPass();
                    tp.SetInputMode(CompositionTargetPass.InputMode.IM_NONE);
                    tp.OutputName = "temp";
                    {
                        CompositionPass pass = tp.CreatePass();
                        pass.Type = CompositionPass.PassType.PT_RENDERQUAD;
                        pass.SetMaterialName("Ogre/Compositor/Combine");
                        pass.SetInput(0, "scene");
                        pass.SetInput(1, "sum");

                    }
                }
                /// Copy back sum texture
                {
                    CompositionTargetPass tp = t.CreateTargetPass();
                    tp.SetInputMode(CompositionTargetPass.InputMode.IM_NONE);
                    tp.OutputName = "sum";
                    {
                        CompositionPass pass = tp.CreatePass();
                        pass.Type = CompositionPass.PassType.PT_RENDERQUAD;
                        pass.SetMaterialName("Ogre/Compositor/Copyback");
                        pass.SetInput(0, "temp");

                    }
                }
                /// Display result
                {

                    CompositionTargetPass tp = t.OutputTargetPass;
                    tp.SetInputMode(CompositionTargetPass.InputMode.IM_NONE);
                    {
                        CompositionPass pass = tp.CreatePass();
                        pass.Type = CompositionPass.PassType.PT_RENDERQUAD;
                        pass.SetMaterialName("Ogre/Compositor/MotionBlur");
                        pass.SetInput(0, "sum");

                    }
                }

            }
        }
        public void AddCompositors()
        {
            CompositorInstance instance;
           
            instance = CompositorManager.Singleton.AddCompositor(viewport, "Old Movie");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "Old Movie", false);
            }

            
           // CompositorManager.Singleton.RemoveCompositor(viewport, "Bloom");
            instance = CompositorManager.Singleton.AddCompositor(viewport, "Bloom");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "Bloom", false);
            }
          
           // CompositorManager.Singleton.RemoveCompositor(viewport, "B&W");
           
            instance = CompositorManager.Singleton.AddCompositor(viewport, "B&W");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "B&W", false);
            }
            
            
            instance = CompositorManager.Singleton.AddCompositor(viewport, "Gaussian Blur");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "Gaussian Blur", false);
            }
            
           /* instance = CompositorManager.Singleton.AddCompositor(viewport, "Motion Blur");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "Motion Blur", false);
            }*/

          
            // COMPOSITOR
            /// Motion blur effect
            if(!CompositorManager.Singleton.ResourceExists("Motion Blur"))
            {
                CreateMotionBlurCompositor(viewport);
            }
            
       

            instance = CompositorManager.Singleton.AddCompositor(viewport, "Motion Blur");
            if (instance != null)
            {
                CompositorManager.Singleton.SetCompositorEnabled(viewport, "Motion Blur", false);
            }
        }
    }
}