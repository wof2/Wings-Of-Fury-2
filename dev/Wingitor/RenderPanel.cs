using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Mogre;
using MOIS;
using Wof.Controller;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.Model.Level;
using Wof.View;
using Plane=Wof.Model.Level.Planes.Plane;
using Timer=Mogre.Timer;
using Vector3=Mogre.Vector3;

namespace wingitor
{
    public class RenderPanel : Panel, IFrameWork
    {

        public Camera Camera
        {
            get { return camera; }
        }

        public Camera MinimapCamera
        {
            get { return null; }
        }

        public Camera MinimapNoseCamera
        {
            get { return null; }
        }


        public Viewport Viewport
        {
            get { return viewport; }
        }

        public Viewport MinimapViewport
        {
            get { return null; }
        }

        public Viewport OverlayViewport
        {
            get { return null; }
        }

        public SceneManager OverlayMgr
        {
            get;
            set;
        }

        public SceneManager SceneMgr
        {
            get
            {
                return sceneMgr;
            }
            set
            {
                sceneMgr = value;
            }
        }

        public SceneManager MinimapMgr
        {
            get { return null; }
            set
            {

            }
        }

        protected System.Windows.Forms.Timer mouseTimer;

        RenderWindow window;
        protected Root root;
        protected static SceneManager sceneMgr;
        protected Camera camera;
        protected Viewport viewport;


        protected InputManager inputManager;
        protected Keyboard inputKeyboard;
        protected IList<JoyStick> inputJoysticks;
       // protected Mouse inputMouse;

        protected static float camSpeed = 50f;
        protected static Degree rotateSpeed = 16;
        protected float toggleDelay = 1.0f;
     
        protected Overlay debugOverlay;
        protected bool showDebugOverlay = true;
        protected float debugTextDelay = 0.0f;
        protected string mDebugText;
        private bool running = true;
        private bool isMouseOver = false;
        private Vector3 mousePos;
        private Vector3 mouseDiff;

        private Vector3 lastMousePos;
        private MouseButtons mouseButtons;
        public RenderPanel()
        {
            InitializeComponent();
            EngineConfig.DisplayingMinimap = false;
          
             //this.Size = new Size(800, 600);
           
        }

        protected  static object locker = new object();
    

        protected void OgreForm_Resize(object sender, EventArgs e)
         {
             lock (locker)
             {
                 window.Resize((uint)Width, (uint)Height);
            
                 /*if (window != null)
                     window.WindowMovedOrResized();*/
                // if (camera != null)
                 
             }
            
         }
 
       
         protected void OgreForm_Disposed(object sender, EventArgs e)
         {
             lock (locker)
             {
                 running = false;
                
             }
         }


       
     
      
        public virtual void Go()
        {
         
         
             while (!Disposing && root != null && running)
             {
                 lock (locker)
                 {
                  
                     try
                     {
                         if (root.RenderOneFrame())
                         {
                             //Application.DoEvents();
                         }
                         else
                         {
                             running = false;
                         }
                     }
                     catch (Exception ex)
                     {
                     	 MessageBox.Show(ex.Message + " " + ex.StackTrace ,"Error while rendering");
                         running = false;
                     }

                 }
                

             } 
             lock (locker)
             {
                 Destroy();
             }

         }

        private void mouseTimer_Tick(object sender, EventArgs e)
        {
            mousePos.z = 0;
            lastMousePos = mousePos;
        }

        public virtual void Destroy()
        {

            FrameWorkStaticHelper.DestroyScenes(this);
            try
            {
                root.Dispose();
                root = null;
            }
            catch (Exception ex)
            {
                
              //  throw;
            }
           // this.Dispose();
           
        }


        public CameraListenerBase CameraListener
        {
            get { return cameraListener; }
        }

        public void ChooseSceneManager()
        {
            // Get the SceneManager, in this case a generic one
            sceneMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "SceneMgr");
        }

        protected CameraListener cameraListener;

        protected void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("mainCamera");
            camera.NearClipDistance = 1.0f;
            camera.FarClipDistance = 8600.0f;
            camera.AutoAspectRatio = false;
            camera.SetPosition(0, 50, 100);
            cameraListener = new CameraListener(camera);
            camera.SetListener(cameraListener);
        }

        protected virtual void CreateFrameListeners()
        {
            Disposed += new EventHandler(OgreForm_Disposed);
            Resize += new EventHandler(OgreForm_Resize);
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            root.FrameEnded += new FrameListener.FrameEndedHandler(FrameEnded);
            root.RenderSystem.EventOccurred +=
                new RenderSystem.Listener.EventOccurredHandler(RenderSystem_EventOccurred);

        }
        protected virtual bool FrameEnded(FrameEvent evt)
        {
            return true;
        }
        protected virtual bool FrameStarted(FrameEvent evt)
        {
            if (window.IsClosed)
                return false;

            OnUpdateModel(evt);


            return true;
        }

        protected void RenderSystem_EventOccurred(string eventName, Const_NameValuePairList parameters)
        {

            if (eventName.Equals("DeviceLost"))
                window.WindowMovedOrResized();
        }
          
        protected virtual void OnUpdateModel(FrameEvent evt)
        {
         

            mouseDiff = mousePos - lastMousePos;
          
         
            Degree scaleRotate = rotateSpeed * evt.timeSinceLastFrame;


            inputKeyboard.Capture();
            if (inputJoysticks != null){
                foreach(JoyStick j in inputJoysticks) {		                
                	j.Capture();
                }
            }
         /*
            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Left))
            {
                camera.Yaw(scaleRotate);
            }

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Right))
            {
                camera.Yaw(-scaleRotate);
            }
            
            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up))
            {
                camera.Pitch(scaleRotate);
            }

            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down))
            {
                camera.Pitch(-scaleRotate);
            }*/

            if (inputJoysticks != null)
            {
                Vector2 joyVector = FrameWorkStaticHelper.GetJoystickVector(inputJoysticks,true);
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

         

            if (inputKeyboard.IsKeyDown(KeyCode.KC_SYSRQ))
            {
                string[] temp = Directory.GetFiles(Environment.CurrentDirectory, "wscreenshot*.jpg");
                string fileName = string.Format("wscreenshot{0}.jpg", temp.Length + 1);

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
         
          //  inputMouse.Capture();
            HandleCameraInput(inputKeyboard, null, inputJoysticks, evt, camera, null, null);
        }
    
        public void HandleCameraInput(Keyboard inputKeyboard, Mouse inputMouse, IList<JoyStick> inputJoysticks, FrameEvent evt, Camera camera, Camera minimapCamera, Plane playerPlane)
        {
           
            // Move about 100 units per second,
            float moveScale = camSpeed * evt.timeSinceLastFrame;
            // Take about 10 seconds for full rotation

            Vector3 translateVector = Vector3.ZERO;

            // set the scaling of camera motion




            if (camera != null)
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
                        translateVector.z = -3*moveScale;
                    else
                        translateVector.z = -moveScale;
                }

                if (inputKeyboard.IsKeyDown(KeyCode.KC_S))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                        translateVector.z = 3*moveScale;
                    else
                        translateVector.z = moveScale;
                }

                if( mouseButtons == MouseButtons.Left)
                {
                    Degree cameraYaw = -mouseDiff.x * .13f;
                    Degree cameraPitch = -mouseDiff.y * .13f;

                    camera.Yaw(cameraYaw);
                    camera.Pitch(cameraPitch);
                   

                }
               

                // ZOOM IN
                if (mousePos.z > 0)
                {
                    translateVector.z -= moveScale  * 0.1f * mousePos.z;
                    //camera.Position += new Vector3(0, 0, -mouseDiff.z * .05f);
                }

                // ZOOM OUT
                if (mousePos.z < 0)
                {
                    translateVector.z -= moveScale * 0.1f * mousePos.z;
                   // camera.Position += new Vector3(0, 0, -mouseDiff.z * .05f);
                }

                camera.MoveRelative(translateVector);
               
            }
        }

        public void TakeScreenshot(string fileName)
        {
         
        }

        public string UpdateStats()
        {
            return "";
            
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
            camera.AspectRatio = (viewport.ActualWidth) / ((float)viewport.ActualHeight);
            viewport.OverlaysEnabled = false;
        }


        public void CreateViewports()
        {
            // zwolnij zasoby
            if (viewport != null && CompositorManager.Singleton.HasCompositorChain(viewport)) CompositorManager.Singleton.RemoveCompositorChain(viewport);
            window.RemoveAllViewports();
            CreateMainViewport(1, 0, 0, 1, 1);
        }

       
      
        public void SetCompositorEnabled(FrameWorkForm.CompositorTypes type, bool enabled)
        {
            
        }


        public virtual bool Setup()
        {
           
            Splash splash = new Splash();
            splash.Show();
            splash.SetStepsCount(10);
            bool carryOn = true;
            string splashFormat = "{0}...";
            try
            {
                splash.Increment(
                    String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingTheRootObject)));
               

                root = new Root();
           
                LogManager.Singleton.LogMessage("Starting Wings of Fury 2 ver. ");


                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.SetupingResources)));
                FrameWorkForm.SetupResources();
                // Setup RenderSystem
                RenderSystem rs = root.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
                                                 // or use "OpenGL Rendering Subsystem"
                root.RenderSystem = rs;
                rs.SetConfigOption("Full Screen", "No");

                rs.SetConfigOption("Video Mode", Width + " x "+ Height + " @ 32-bit colour");
                // Create Render Window
                root.Initialise(false, "Wingitor RenderWindow");
                NameValuePairList misc = new NameValuePairList();
                misc["externalWindowHandle"] = Handle.ToString();
                window = root.CreateRenderWindow("Wingitor RenderWindow", (uint)ClientSize.Width, (uint)ClientSize.Height, false, misc.ReadOnlyInstance);
     
                    
                splash.Activate();
           
                // Load resources
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.LoadingResources)));
                FrameWorkForm.LoadResources();
              
                splash.Increment(
                    String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingGameObjects)));
                
                ChooseSceneManager();
                CreateCamera();
                CreateViewports();
               
                splash.Increment(4);

                SetupEngineConfig();
              
                // Create the scene
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingScene)));
                CreateScene();

            
                splash.Increment(String.Format(splashFormat, LanguageResources.GetString(LanguageKey.CreatingInput)));
                CreateInput();

                CreateFrameListeners();

             
            }
            finally
            {
                splash.Close();
                splash.Dispose();
                if (carryOn) 
                {
                    window.SetVisible(true);                	
                }
            }
            return true;
        }
        
        private void SetupEngineConfig()
        {
        	EngineConfig.LoadEngineConfig();            
            switch (EngineConfig.Difficulty)
            {
                case EngineConfig.DifficultyLevel.Easy:
                    Game.SetModelSettingsFromFile(0);
                    break;
                case EngineConfig.DifficultyLevel.Medium:
                    Game.SetModelSettingsFromFile(1);
                    break;
                case EngineConfig.DifficultyLevel.Hard:
                    Game.SetModelSettingsFromFile(2);
                    break;
            }
        }
     
        public virtual void CreateScene()
        {

        }

        public virtual void CreateInput()
        {

            mouseTimer = new System.Windows.Forms.Timer();
            mouseTimer.Tick += new EventHandler(mouseTimer_Tick);

            mouseTimer.Interval = 50;
            mouseTimer.Start();

            LogManager.Singleton.LogMessage("*** Initializing OIS ***");

          

            ParamList pl = new ParamList();
            IntPtr windowHnd;
            window.GetCustomAttribute("WINDOW", out windowHnd);
           

          //  pl.Insert("WINDOW", windowHnd.ToString());
            pl.Insert("WINDOW", Parent.Parent.Parent.Parent.Parent.Parent.Handle.ToString());

            inputManager = InputManager.CreateInputSystem(pl);

            //Create all devices (We only catch joystick exceptions here, as, most people have Key/Mouse)
            try
            {
                inputKeyboard = (Keyboard)inputManager.CreateInputObject(MOIS.Type.OISKeyboard, UseBufferedInput);

            }
            catch (Exception e)
            {
                
                throw e;
            }
         
           // inputMouse = (Mouse) inputManager.CreateInputObject(MOIS.Type.OISMouse, UseBufferedInput);
        }


        public virtual bool UseBufferedInput
        {
            get { return false; }
        }

        public bool Running
        {
            get { return running; }
            set { running = value; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RenderPanel
            // 
            this.MouseLeave += new System.EventHandler(this.RenderPanel_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseUp);
            this.MouseEnter += new System.EventHandler(this.RenderPanel_MouseEnter);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseWheel);
            this.ResumeLayout(false);

        }

       
        private void RenderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Vector3(e.Location.X, e.Location.Y, mousePos.z);

        }

        private void RenderPanel_MouseEnter(object sender, EventArgs e)
        {
            if (!this.Focused) this.Focus();
            isMouseOver = true;
           
        }

        private void RenderPanel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOver = false;
        }

        
        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseButtons =  e.Button;
        }

        private void RenderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseButtons = MouseButtons.None;
        }

        private void RenderPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            mousePos.z =  e.Delta;
         
        }


         
    }

}
