/*
 * Copyright 2008 Adam Witczak, Jakub Tê¿ycki, Kamil S³awiñski, Tomasz Bilski, Emil Hornung, Micha³ Ziober
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using FSLOgreCS;
using Microsoft.Win32;
using Mogre;
using MOIS;
using Wof.Controller.Screens;
using Wof.Model.Configuration;
using Wof.View;
using Wof.View.Effects;

using LONG = System.Int32;
using LONG_PTR = System.IntPtr;
using HWND = System.IntPtr;

	    
	    
namespace Wof.Controller
{
    /// <summary>
    /// Klasa odpowiedzialna za start aplikacji, przechodzenie miêdzy screenami menu i implementuj¹ca Framework.
    /// <author>Jakub Tê¿yki, Adam Witczak, Micha³ Ziober</author>
    /// </summary>
    internal class Game : FrameWorkForm, GameEventListener
    {
       

        public delegate void DelegateVoidVoid();

        protected float time = 0;

        private static Game game;
        private static PerformanceTestFramework performanceTest;

        private MenuScreen currentScreen;

        public MenuScreen CurrentScreen
        {
            get { return currentScreen;  }
        }

        public static bool ShouldReload
        {
            get { return shouldReload; }
            set { shouldReload = value; }           
            
        }
        
         private Browser browser;

        private static Boolean shouldReload = false;
        private DelegateVoidVoid afterExit = null;

        [DllImport("shell32.dll")]
        private static extern long ShellExecute(Int32 hWnd, string lpOperation,
            string lpFile, string lpParameters, string lpDirectory, long nShowCmd);

        /// <summary>
        /// Buduje scenê
        /// </summary>
        public override void CreateScene()
        {
            if (currentScreen == null)
            {
                if(EngineConfig.DebugStart)
                {
                    StartGame(EngineConfig.DebugStartLevel);
                    return;

                } else
                {
                	
                    if (EngineConfig.ShowIntro)
                    {
                        currentScreen = new IntroScreen(this, this, viewport, camera);
                        StartBrowser();
                    }
                    else
                    {
                        SoundManager.Instance.PlayMainTheme();
                          
                        currentScreen = new StartScreen(this, this, viewport, camera);
                        StartBrowser();
                        ShowBrowser();    
                    }  
                }
             
         
     
            }
          
           currentScreen.DisplayGUI(false);
        }


        protected override void WireEventListeners()
        {

            base.WireEventListeners();
            this.Activated += new EventHandler(Game_Activated);
            this.Deactivate += new EventHandler(Game_Deactivate);
            this.Closing += new System.ComponentModel.CancelEventHandler(Game_Closing);

        }

       

        void Game_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        void Game_Activated(object sender, EventArgs e)
        {
            // przegladarka reklam wraca na swoje miejsce
            if(browser != null)
            {
                browser.ReturnToInitialState();
                browser.IsActivated = false;
            }
            isActivated = true;
        }

        private void Game_Deactivate(object sender, EventArgs e)
        {
           // isActivated = false;
            /*if (browser != null && !browser.IsActivated)
            {
                browser.Hide();
            }*/
        }


        public void OnFrameStarted(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {

        }

        protected bool isActivated = false;

       


        protected override bool FrameEnded(FrameEvent evt)
        {
            if (currentScreen != null)
            {
                currentScreen.OnHandleViewUpdateEnded(evt, inputMouse, inputKeyboard, inputJoystick);
              
                
                if(browser != null && browser.Visible && browser.IsInitialState)
                {   
                	
                    Point screenPos = (currentScreen as AbstractScreen).MousePosScreen;                   
                    if (screenPos.X >= 0 && screenPos.Y >= 0)
                    {
                        bool activateMain = false;
                        if (browser.IsActivated)
                        {
                            // uzywamy wspolrzednych myszy z browsera
                            if (!browser.IsMouseOver())
                            {
                                // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "!browser.IsMouseOver() - > activateMain");
                                // mysz wyszla na zewnatrz browsera
                                activateMain = true;
                            }
                        }
                        else if (isActivated)
                        {                            
                            // okno glowne
                            if (browser.IsMouseOver(screenPos))
                            {
                                activateMain = false;
                            }
                            else
                            {                            
                                activateMain = true;
                            }
                        }
                        else
                        {
                            // zadne nie jest aktywne?
                            activateMain = true;
                        }
                        
                        // faktyczna aktywacja
                        if (!activateMain)
                        {
                            if (!browser.IsActivated)
                            {
                            	//(currentScreen as AbstractScreen).MGui.injectMouse((uint)( viewport.ActualWidth + 1),(uint)(viewport.ActualHeight + 1), false);
                                isActivated = false;
                                browser.IsActivated = true;
                                browser.Activate();
                                //Console.WriteLine("Browser");
                            }
                        }
                        else
                        {                     
                        	                        	
                            if (!isActivated)
                            {                                                     
                                isActivated = true;
                                browser.IsActivated = false;                           
                                Activate();                                 
                            }

                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Handler zdarzenia FrameStarted: animacja
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        protected override bool FrameStarted(FrameEvent evt)
        {

            
            if(IsDisposed)
            {
                return false;
            }
        	evt.timeSinceLastFrame *= EngineConfig.CurrentGameSpeedMultiplier;
            time += evt.timeSinceLastFrame;

            if (currentScreen != null)
            {
                if(HydraxManager.Singleton.USE_UPDATER_THREAD)  Monitor.Enter(HydraxManager.Singleton);
                currentScreen.OnHandleViewUpdate(evt, inputMouse, inputKeyboard, inputJoystick);
                if (HydraxManager.Singleton.USE_UPDATER_THREAD) Monitor.Exit(HydraxManager.Singleton);
            }

            if (window.IsClosed)
                return false;
       
           
          
            return !shutDown;
        }

        protected override void ModelFrameStarted(FrameEvent evt)
        {
            evt.timeSinceLastFrame *= EngineConfig.CurrentGameSpeedMultiplier;
            OnUpdateModel(evt);
        }

        protected override void OnUpdateModel(FrameEvent evt)
        {
            if (currentScreen != null)
            {
                currentScreen.OnUpdateModel(evt, inputMouse, inputKeyboard, inputJoystick);
            }
        }

        #region Main Method

        [STAThread]   
        private static void Main(string[] args)
        {

        	try
        	{
        		Firewall.AddException();
        	}
        	catch(Exception ex)
        	{
        		LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Error while opening firewall for WOF: "+ ex.Message);
        	}
        	
        	
        	//  MessageBox.Show("Params"+string.Join(",",args));
     
        	
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-FreeLook"))
                    {
                        EngineConfig.FreeLook = true;
                        EngineConfig.AttachCameraToPlayerPlane = false;
                        EngineConfig.ManualCamera = true;
                    }
                    else if (args[i].Equals("-SkipIntro"))
                    {
                        EngineConfig.ShowIntro = false;                        
                    } 
                    else if (args[i].Equals("-DebugInfo"))
                    {
                        EngineConfig.DebugInfo = true;
                    }                    
                    else if (args[i].Equals("-DebugStart"))
                    {
                        EngineConfig.DebugStart = true;
                        if(i + 1 < args.Length )
                        {
                            int levelNo;
                            if(int.TryParse(args[i + 1], out levelNo))
                            {
                                EngineConfig.DebugStartLevel = levelNo;
                                i++;
                            }
                            
                            
                        }

                    } 
                        
                }
            }
            bool firstRun = !File.Exists(EngineConfig.C_ENGINE_CONFIG) || !File.Exists(EngineConfig.C_OGRE_CFG);
          
            // przeprowadz test wydajnosci
            if(firstRun)
            {
            	StartPerformanceTest();
            } 

            StartWOFApplication();	            
          
	        if (getGame().afterExit != null) getGame().afterExit();
            
           
        }

        /// <summary>
        /// Sprawdza czy juz zostala uruchomiana apliakcja.
        /// Jesli nie to ja uruchamia.
        /// </summary>
        private static void StartWOFApplication()
        {
            try
            {
            	
                bool firstInstance;
                Mutex mutex = new Mutex(false, @"Wings_Of_Fury", out firstInstance);
             //   if (firstInstance)
                {
                    StartFirstInstance();
                }
           //     else
                {
                   
                }  
                SoundManager3D.Instance.Dispose();
                mutex.Close();
              
            }
            catch (Exception exc)
            {
             	
	            try{
            		LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, exc.Message + " " + exc.StackTrace);
	                getGame().window.Destroy();
	                FrameWorkStaticHelper.ShowWofException(exc);	                
	                Debug.WriteLine(exc.ToString());
	            }
            	catch
            	{
            		
            	}

            }
        }

        public static Game getGame()
        {
            return Game.game;
        }
        
        /// <summary>
        /// Uruchamia pierwsza instancje na tym komputerze
        /// </summary>
        private static void StartPerformanceTest()
        {
            try
            {
                performanceTest = new PerformanceTestFramework();
                performanceTest.Go();

             
            }
            catch (SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    FrameWorkStaticHelper.ShowOgreException();
                else
                    throw;
            }
                       
        }
        
        
		public override void Go()
		{			
			base.Go();
			
		}
        /// <summary>
        /// Uruchamia pierwsza instancje na tym komputerze
        /// </summary>
        private static void StartFirstInstance()
        {
            try
            {
                game = new Game();

                // jesli przeprowadzono test wydajnosci, przekaz go dalej
                if (performanceTest != null && performanceTest.HasResults)
                {
                    game.InjectPerformanceTestResults(performanceTest);
                }
                EngineConfig.DisplayingMinimap = false;
                
                game.Go();

                if(game.browser != null)
                {
                    game.DisposeBrowser();
                }
            }
            catch (SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    FrameWorkStaticHelper.ShowOgreException();
                else
                    throw;
            }
            catch(RootInitializationException)
            {
                // i tak bêdzie reload
                shouldReload = true;
            }

            if (shouldReload)
            {
               /* string filename = Process.GetCurrentProcess().MainModule.FileName;
                int index = filename.IndexOf("bin");
                string dir = filename.Substring(0, index);
                filename = dir + "Wof.exe";
                */				
               // ShellExecute(0, "open", filename, param, dir, 1);
               
  				string param="-SkipIntro"; // nie bedziemy meczyc intro jesli ktos chce tylko zmienic rozdzielczosc
                ProcessStartInfo startInfo = new ProcessStartInfo();
		        startInfo.CreateNoWindow = false;
		        startInfo.UseShellExecute = false;
		        startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
		        startInfo.WindowStyle = ProcessWindowStyle.Normal;
		        startInfo.Arguments = param;
				Process exeProcess = Process.Start(startInfo);
				
				//User32.SetForegroundWindow(exeProcess.Handle);
				//SetFocusToPreviousInstance();
				game.Hide();
			
            }
        }

        public static void SetModelSettingsFromFile(int index)
        {
            string[] configFiles = ConfigurationManager.GetAvailableConfigurationFiles();
            if (configFiles != null && configFiles.Length > 0)
            {
                //Array.Sort(configFiles);
                if (index < configFiles.Length)
                    ConfigurationManager.SetConstsFromFile(configFiles[index]);
                else
                    ConfigurationManager.SetConstsFromFile(configFiles[0]);
            }
        }

        /// <summary>
        /// Jesli juz jedna instancja jest juz
        /// stworzona.
        /// </summary>
        private static void SetFocusToPreviousInstance()
        {
            try
            {
                Process[] proces = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                if (proces != null && proces.Length > 0)
                {
                    IntPtr hWnd = IntPtr.Zero;
                    foreach (Process proc in proces)
                    {
                        if (proc.Id != Process.GetCurrentProcess().Id)
                        {
                            hWnd = Process.GetProcessById(proc.Id).MainWindowHandle;
                        }
                    }
                    // jesli znalazlem poprzednia wersje.
                    if (hWnd != IntPtr.Zero)
                    {
                        // Jesli jakies okno jest wyswietlone.
                        IntPtr hPopupWnd = User32.GetLastActivePopup(hWnd);
                        if (User32.IsWindowEnabled(hPopupWnd))
                        {
                            hWnd = hPopupWnd;
                        }
                        User32.SetForegroundWindow(hWnd);
                        //Jesli program jest zminimalizowany, przywracam go.
                        if (User32.IsIconic(hWnd))
                        {
                            User32.ShowWindow(hWnd, User32.Restore);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
            }
        }

        #endregion

        #region GameEventListener Members

        public void StartGame()
        {
            StartGame(1);
        }


        public void StartGame(string levelFile)
        {
            StartGame(0, levelFile);
          
        }
        public void StartGame(int levelNo)
        {
             StartGame(levelNo, null);
        }
        public void StartGame(int levelNo, string levelFile)
        {
        	HideBrowser();
            switch (EngineConfig.Difficulty)
            {
                case EngineConfig.DifficultyLevel.Easy:
                    SetModelSettingsFromFile(0);
                    break;
                case EngineConfig.DifficultyLevel.Medium:
                    SetModelSettingsFromFile(1);
                    break;
                case EngineConfig.DifficultyLevel.Hard:
                    SetModelSettingsFromFile(2);
                    break;
            }

            SoundManager.Instance.StopMusic();
            if(currentScreen !=null)
            {
                currentScreen.CleanUp(false);
            }


            EngineConfig.DisplayingMinimap = EngineConfig.DisplayMinimap; ;
            ChooseSceneManager();
            CreateCamera();

            CreateViewports();
            AddCompositors();
       

           
            if (!FrameWorkStaticHelper.CreateSoundSystem(camera, EngineConfig.SoundSystem))
                EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;


            SetCompositorEnabled(CompositorTypes.BLOOM, EngineConfig.BloomEnabled);
            currentScreen = new GameScreen(this, this, directSound, 2, levelNo, levelFile);
            currentScreen.DisplayGUI(false);
        }

      

        public void GotoNextLevel()
        {
            int lives = ((GameScreen) currentScreen).Lives;
           

            int score = ((GameScreen) currentScreen).Score;
            int level = ((GameScreen) currentScreen).LevelNo;
          
            if (File.Exists(GameScreen.GetLevelFileName(level + 1)))
            {
                LoadGameUtil.NewLevelCompleted((uint) (level + 1));

                MenuScreen screen = currentScreen;
                screen.CleanUp(false);

                EngineConfig.DisplayingMinimap = EngineConfig.DisplayMinimap;
                

                ChooseSceneManager();
                CreateCamera();
                if (!FrameWorkStaticHelper.CreateSoundSystem(camera, EngineConfig.SoundSystem))
                    EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;

                CreateViewports();
                AddCompositors();

                SetCompositorEnabled(CompositorTypes.BLOOM, EngineConfig.BloomEnabled);

                currentScreen = new GameScreen(this, this, directSound, lives, level + 1, null);
                ((GameScreen) currentScreen).Score = score;

                currentScreen.DisplayGUI(false);
            }
            else
            {
                GotoEndingScreen(score);
            }
        }

        public void ExitGame()
        {
        	DisposeBrowser();
            shutDown = true;
            afterExit = null;
        }

        public void ExitGame(DelegateVoidVoid d)
        {
        	DisposeBrowser();
            shutDown = true;
            this.afterExit = d;
        }

        /// <summary>
        /// Jeœli przy przejœciu do tego screena potrzebna jest ponowna inicjalizacja kamery, scenemanager'a, viewportów, compositorów oraz dŸwiêku, metoda przeprowadzi j¹.
        /// </summary>
        private void initScreenAfter(MenuScreen screen)
        {

            EngineConfig.DisplayingMinimap = false;

            Boolean justMenu = IsMenuScreen(screen);
            if(screen != null)    screen.CleanUp(justMenu);
           
            if (!justMenu)
            {
               
                ChooseSceneManager();
                CreateCamera();
                if (!FrameWorkStaticHelper.CreateSoundSystem(camera, EngineConfig.SoundSystem))
                    EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;
                
                CreateViewports();
                AddCompositors();
             
            } else
            {

            
				
            }
        }
        
       

        public void StartBrowser()
        { 
        	browser = new Browser(this, currentScreen as AbstractScreen);
            browser.SetPosition();
        	HideBrowser();
        }
        
        public void ShowBrowser()
        {
        	if(browser == null) StartBrowser();
        	browser.TopMost = true;
        	if(!browser.Visible) browser.Show();
        }
        public void HideBrowser()
        {
        	 if(browser == null) return;
        	 browser.Visible = false;
        }
        public void DisposeBrowser()
        {
        	if(browser != null)
        	{	   
        		browser.Hide();
            	browser.Close();
        		browser.Dispose();
        		browser = null;
        	}
        }

        public void GotoIntroScreen()
        {
        	
        	if (OptionsScreen.restartRequired)
            {
                OptionsScreen.restartRequired = false;
                shouldReload = true;
                shutDown = true;
            }
            if(OptionsScreen.shutdownRequired)
            {
            	OptionsScreen.shutdownRequired = false;
            	shouldReload = false;
            	shutDown = true;
            }
           
            ScreenState ss = null;
            if (currentScreen != null && currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
          
            Boolean justMenu = IsMenuScreen(currentScreen);
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();
           

            currentScreen = new IntroScreen(this, this, viewport, camera);
         

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
            if (ss != null)
            {
            	(currentScreen as AbstractScreen).SetMousePosition(ss);
        
            }
        }
        public void GotoStartScreen()
        {
        	
        	
            if (OptionsScreen.restartRequired)
            {
                OptionsScreen.restartRequired = false;
                shouldReload = true;
                shutDown = true;
            }
            if(OptionsScreen.shutdownRequired)
            {
            	OptionsScreen.shutdownRequired = false;
            	shouldReload = false;
            	shutDown = true;
            }
          
            
            ScreenState ss = null;
            if (currentScreen != null && currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                if((browser == null && (currentScreen as AbstractScreen).Viewport != null))
                { 
                    StartBrowser();
                }
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
          
            Boolean justMenu = IsMenuScreen(currentScreen);
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();
           

            currentScreen = new StartScreen(this, this, viewport, camera);
            if (browser == null) StartBrowser();

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
            if (ss != null)
            {
            	(currentScreen as AbstractScreen).SetMousePosition(ss);             
            }

            if (!shutDown && !shouldReload)
            {
                ShowBrowser();
            }
        }


        public void GotoLoadGameScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();

            currentScreen = new LoadGameScreen(this, this, viewport, camera);
            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoCustomLevelsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();

            currentScreen = new CustomLevelsScreen(this, this, viewport, camera);
            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoHighscoresScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            SoundManager.Instance.PlayMainTheme();

            currentScreen = new HighscoresScreen(this, this, viewport, camera);
            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoEnterScoreScreen(int score)
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }

            initScreenAfter(currentScreen);
           
            SoundManager.Instance.PlayMainTheme();
            currentScreen = new ScoreEnterScreen(this, this, viewport, camera, score);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }


        public void GotoDonateScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            SoundManager.Instance.PlayMainTheme();

            currentScreen = new DonateScreen(this, this, viewport, camera);
            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoQuitScreen()
        {
        	DisposeBrowser();
            if(currentScreen is AbstractScreen)
            {
                (currentScreen as AbstractScreen).ForceRebuild = true;
            }
            
            Boolean justMenu = IsMenuScreen(currentScreen);
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            currentScreen = new QuitScreen(this, this, viewport, camera);
            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
            
        }


        public void GotoEndingScreen(int highscore)
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            SoundManager.Instance.PlayMainTheme();

            currentScreen = new EndingScreen(this, this, viewport, camera, true, 25, highscore);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoCreditsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
			HideBrowser();
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();
            currentScreen = new CreditsScreen(this, this, viewport, camera, true, 65);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
 		
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            currentScreen = new OptionsScreen(this, this, viewport, camera);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoTutorialScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            HideBrowser();
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new TutorialScreen(this, this, viewport, camera);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoVideoModeScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new VideoModeScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoAntialiasingOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new AntialiasingOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoVSyncOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }

            initScreenAfter(currentScreen);
            currentScreen = new VSyncOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoDifficultyOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new DifficultyScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoBloomOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new BloomOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }
        
  		public void GotoHydraxOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new HydraxOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoBloodOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new BloodOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

  		
  		
        public void GotoLodOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new LODOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }
        
        public void GotoShadowsOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new ShadowsOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }
        

        public void GotoControlsOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new ControlsOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoLanguagesOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new LanguagesOptionsScreen(this, this, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoSoundOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof (AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new SoundOptionsScreen(this, this, viewport, camera, root);

            if(ss!=null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            

            currentScreen.DisplayGUI(justMenu);
        }

        public void MinimizeWindow()
        {
        	if(browser != null && browser.Visible) 
        	{
        	//	browser.IsInitialState = false;
        		browser.Hide();
        	}
        	this.WindowState = FormWindowState.Minimized;
           // window.SetVisible(false);
         //   window.SetFullscreen(false, 0, 0);
          //  window.Resize(0, 0);
          //  window.Update(true);
        }

   


       

        /// <summary>
        /// Reads path of default browser from registry
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultBrowserPath()
        {

            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                //trim off quotes
                browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (key != null) key.Close();
            }
            return browser;

        }

        public void GotoDonateWebPage()
        {
        	MinimizeWindow();
        	GotoDonateWebPageDo();
        	
          // ExitGame(GotoDonateWebPageDo);
        }
        public void GotoDonateWebPageDo()
        {
            string url = EngineConfig.C_WOF_HOME_PAGE + "/page/donate?v="+EngineConfig.C_WOF_VERSION+"_"+EngineConfig.C_IS_DEMO.ToString();
            try
            {
                // launch default browser
                Process.Start(GetDefaultBrowserPath(), url);
            }
            catch (Exception)
            { }
        }


        public void GotoUpdateWebPage()
        {
        	MinimizeWindow();
        	GotoUpdateWebPageDo();
        	
           // ExitGame(GotoUpdateWebPageDo);
        }


        public void GotoUpdateWebPageDo()
        {
            string url = EngineConfig.C_WOF_HOME_PAGE + "/update.php?v="+EngineConfig.C_WOF_VERSION+"&d="+EngineConfig.C_IS_DEMO.ToString();
            try
            {
                // launch default browser
                Process.Start(GetDefaultBrowserPath(), url);
            }
            catch (Exception)
            { }
        }


        

        private Boolean SceneNeedsRebuilding(MenuScreen screen)
        {
        	if (screen ==  null) return true;
            if ((screen is EndingScreen)) return true;
            if ((screen is GameScreen)) return true;
            if ((screen is IntroScreen)) return true;
            if ((screen is AbstractScreen) && (screen as AbstractScreen).ForceRebuild) return true;
            return false;
        }

        private Boolean IsMenuScreen(MenuScreen screen)
        {
            return !SceneNeedsRebuilding(screen);
        }

        #endregion

       
    }

    #region Import function - user32.dll

    /// <summary>
    /// Importuje funkcje z user32.dll
    /// </summary>
    internal static class User32
    {
        private static readonly short SW_RESTORE = 9;

        public static short Restore
        {
            get { return SW_RESTORE; }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetLastActivePopup(IntPtr hWnd);
        
        
	    [DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
	   
		[DllImport("user32.dll", SetLastError=true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		
		
		
		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
	    public static extern int GetSystemMetrics(int which);
	
	    [DllImport("user32.dll")]
	    public static extern void
	        SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
	                     int X, int Y, int width, int height, uint flags);        
	
	    private const int SM_CXSCREEN = 0;
	    private const int SM_CYSCREEN = 1;
	    private static IntPtr HWND_TOP = IntPtr.Zero;
	    private const int SWP_SHOWWINDOW = 64; // 0×0040
	
	    public static int ScreenX
	    {
	        get { return GetSystemMetrics(SM_CXSCREEN);}
	    }
	
	    public static int ScreenY
	    {
	        get { return GetSystemMetrics(SM_CYSCREEN);}
	    }
	
	    public static void SetWinFullScreen(IntPtr hwnd)
	    {
	        SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
	    }


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);


        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
       
    }
    
    public abstract class WindowStyles
    {
        public static int WS_OVERLAPPED       = 0x00000000;       
   		public static int WS_POPUP = unchecked((int)0x80000000);
        public static int WS_CHILD        = 0x40000000;
        public static int WS_MINIMIZE     = 0x20000000;
        public static int WS_VISIBLE      = 0x10000000;
        public static int WS_DISABLED     = 0x08000000;
        public static int WS_CLIPSIBLINGS     = 0x04000000;
        public static int WS_CLIPCHILDREN     = 0x02000000;
        public static int WS_MAXIMIZE     = 0x01000000;
        public static int WS_CAPTION      = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public static int WS_BORDER       = 0x00800000;
        public static int WS_DLGFRAME     = 0x00400000;
        public static int WS_VSCROLL      = 0x00200000;
        public static int WS_HSCROLL      = 0x00100000;
        public static int WS_SYSMENU      = 0x00080000;
        public static int WS_THICKFRAME       = 0x00040000;
        public static int WS_GROUP        = 0x00020000;
        public static int WS_TABSTOP      = 0x00010000;

        public static int WS_MINIMIZEBOX      = 0x00020000;
        public static int WS_MAXIMIZEBOX      = 0x00010000;

        public static int WS_TILED        = WS_OVERLAPPED;
        public static int WS_ICONIC       = WS_MINIMIZE;
        public static int WS_SIZEBOX      = WS_THICKFRAME;
        public static int WS_TILEDWINDOW      = WS_OVERLAPPEDWINDOW;

        // Common Window Styles

        public static int WS_OVERLAPPEDWINDOW =
            ( WS_OVERLAPPED  |
              WS_CAPTION     |
              WS_SYSMENU     |
              WS_THICKFRAME  |
              WS_MINIMIZEBOX |
              WS_MAXIMIZEBOX );

        public static int WS_POPUPWINDOW =
            ( WS_POPUP   |
              WS_BORDER  |
              WS_SYSMENU );

        public static int WS_CHILDWINDOW = WS_CHILD;

        //Extended Window Styles

        public static int WS_EX_DLGMODALFRAME     = 0x00000001;
        public static int WS_EX_NOPARENTNOTIFY    = 0x00000004;
        public static int WS_EX_TOPMOST       = 0x00000008;
        public static int WS_EX_ACCEPTFILES       = 0x00000010;
        public static int WS_EX_TRANSPARENT       = 0x00000020;

//#if(WINVER >= 0x0400)
        public static int WS_EX_MDICHILD      = 0x00000040;
        public static int WS_EX_TOOLWINDOW    = 0x00000080;
        public static int WS_EX_WINDOWEDGE    = 0x00000100;
        public static int WS_EX_CLIENTEDGE    = 0x00000200;
        public static int WS_EX_CONTEXTHELP       = 0x00000400;

        public static int WS_EX_RIGHT         = 0x00001000;
        public static int WS_EX_LEFT          = 0x00000000;
        public static int WS_EX_RTLREADING    = 0x00002000;
        public static int WS_EX_LTRREADING    = 0x00000000;
        public static int WS_EX_LEFTSCROLLBAR     = 0x00004000;
        public static int WS_EX_RIGHTSCROLLBAR    = 0x00000000;

        public static int WS_EX_CONTROLPARENT     = 0x00010000;
        public static int WS_EX_STATICEDGE    = 0x00020000;
        public static int WS_EX_APPWINDOW     = 0x00040000;

        public static int WS_EX_OVERLAPPEDWINDOW  = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public static int WS_EX_PALETTEWINDOW     = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
//#endif /* WINVER >= 0x0400 */

//#if(_WIN32_WINNT >= 0x0500)
        public static int WS_EX_LAYERED       = 0x00080000;
//#endif /* _WIN32_WINNT >= 0x0500 */

//#if(WINVER >= 0x0500)
        public static int WS_EX_NOINHERITLAYOUT   = 0x00100000; // Disable inheritence of mirroring by children
        public static int WS_EX_LAYOUTRTL     = 0x00400000; // Right to left mirroring
//#endif /* WINVER >= 0x0500 */

//#if(_WIN32_WINNT >= 0x0500)
        public static int WS_EX_COMPOSITED    = 0x02000000;
        public static int WS_EX_NOACTIVATE    = 0x08000000;
//#endif /* _WIN32_WINNT >= 0x0500 */
    }


    #endregion
}