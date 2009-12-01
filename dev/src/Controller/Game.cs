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

namespace Wof.Controller
{
    /// <summary>
    /// Klasa odpowiedzialna za start aplikacji, przechodzenie miêdzy screenami menu i implementuj¹ca Framework.
    /// <author>Jakub Tê¿yki, Adam Witczak, Micha³ Ziober</author>
    /// </summary>
    internal class Game : FrameWork, GameEventListener
    {
        /// <summary>
        /// Nazwa gry: Wings of Fury 2
        /// </summary>
        public new const string Name = @"Wings of Fury 2";

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
                        currentScreen = new IntroScreen(this, sceneMgr, viewport, camera);
                    }
                    else
                    {
                        SoundManager.Instance.PlayMainTheme();
                        currentScreen = new StartScreen(this, sceneMgr, viewport, camera);
                    }  
                }
             
         
     
            }
          
            currentScreen.DisplayGUI(false);
        }

       

        public void OnFrameStarted(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {

        }

        public override bool FrameEnded(FrameEvent evt)
        {
            if (currentScreen != null)
            {
                currentScreen.OnHandleViewUpdateEnded(evt, inputMouse, inputKeyboard, inputJoystick);
            }
            return true;
        }

        /// <summary>
        /// Handler zdarzenia FrameStarted: animacja
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public override bool FrameStarted(FrameEvent evt)
        {

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

        public override void ModelFrameStarted(FrameEvent evt)
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

        private static void Main(string[] args)
        {
         
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
	                ShowWofException(exc);	                
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
                    ShowOgreException();
                else
                    throw;
            }
                       
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
                game.SetDisplayMinimap(false);
                game.Go();
            }
            catch (SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    ShowOgreException();
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

        private static void SetModelSettingsFromFile(int index)
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

        public void StartGame(int levelNo)
        {
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

            if (EngineConfig.DisplayMinimap)
            {
                SetDisplayMinimap(true);
            }
            ChooseSceneManager();
            CreateCamera();

            CreateViewports();
            AddCompositors();
       

           
            if (!CreateSoundSystem(camera, EngineConfig.SoundSystem))
                EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;


            SetCompositorEnabled(CompositorTypes.BLOOM, EngineConfig.BloomEnabled);
            currentScreen = new GameScreen(this, this, directSound, 2, levelNo);
            currentScreen.DisplayGUI(false);
        }

        public void GotoNextLevel()
        {
            int lives = ((GameScreen) currentScreen).Lives;
           

            int score = ((GameScreen) currentScreen).Score;
            int level = ((GameScreen) currentScreen).LevelNo;
          
            if (File.Exists(GameScreen.GetLevelName(level + 1)))
            {
                LoadGameUtil.NewLevelCompleted((uint) (level + 1));

                MenuScreen screen = currentScreen;
                screen.CleanUp(false);

                if (EngineConfig.DisplayMinimap)
                {
                    SetDisplayMinimap(true);
                }

                ChooseSceneManager();
                CreateCamera();
                if (!CreateSoundSystem(camera, EngineConfig.SoundSystem))
                    EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;

                CreateViewports();
                AddCompositors();

                SetCompositorEnabled(CompositorTypes.BLOOM, EngineConfig.BloomEnabled);

                currentScreen = new GameScreen(this, this, directSound, lives, level + 1);
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
            shutDown = true;
            afterExit = null;
        }

        public void ExitGame(DelegateVoidVoid d)
        {
            shutDown = true;
            this.afterExit = d;
        }

        /// <summary>
        /// Jeœli przy przejœciu do tego screena potrzebna jest ponowna inicjalizacja kamery, scenemanager'a, viewportów, compositorów oraz dŸwiêku, metoda przeprowadzi j¹.
        /// </summary>
        private void initScreenAfter(MenuScreen screen)
        {
            SetDisplayMinimap(false);

            Boolean justMenu = IsMenuScreen(screen);
            screen.CleanUp(justMenu);

            if (!justMenu)
            {
                ChooseSceneManager();
                CreateCamera();
                if (!CreateSoundSystem(camera, EngineConfig.SoundSystem))
                    EngineConfig.SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM;
                
                CreateViewports();
                AddCompositors();
             
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
          
            Boolean justMenu = IsMenuScreen(currentScreen);
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();
            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
          
            currentScreen = new StartScreen(this, sceneMgr, viewport, camera);
         

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            currentScreen.DisplayGUI(justMenu);
            if (ss != null)
            {
                (currentScreen as AbstractScreen).MousePosX = ss.MousePosX;
                (currentScreen as AbstractScreen).MousePosY = ss.MousePosY;
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

            currentScreen = new LoadGameScreen(this, sceneMgr, viewport, camera);
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
          
            currentScreen = new HighscoresScreen(this, sceneMgr, viewport, camera);
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
            currentScreen = new ScoreEnterScreen(this, sceneMgr, viewport, camera, score);

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

            currentScreen = new DonateScreen(this, sceneMgr, viewport, camera);
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

            currentScreen = new EndingScreen(this, sceneMgr, viewport, camera, true, 25, highscore);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoCreditsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);

            SoundManager.Instance.PlayMainTheme();
            currentScreen = new CreditsScreen(this, sceneMgr, viewport, camera, true, 65);

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

            currentScreen = new OptionsScreen(this, sceneMgr, viewport, camera);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

        public void GotoTutorialScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new TutorialScreen(this, sceneMgr, viewport, camera);

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
            currentScreen = new VideoModeScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new AntialiasingOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new VSyncOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new DifficultyScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new BloomOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new HydraxOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new BloodOptionsScreen(this, sceneMgr, viewport, camera, root);

            if (ss != null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }

            currentScreen.DisplayGUI(justMenu);
        }

  		
  		
        public void GotoLODOptionsScreen()
        {
            Boolean justMenu = IsMenuScreen(currentScreen);

            ScreenState ss = null;
            if (currentScreen.GetType().IsSubclassOf(typeof(AbstractScreen)))
            {
                ss = (currentScreen as AbstractScreen).GetScreenState();
            }
            initScreenAfter(currentScreen);
            currentScreen = new LODOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new ShadowsOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new ControlsOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new LanguagesOptionsScreen(this, sceneMgr, viewport, camera, root);

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
            currentScreen = new SoundOptionsScreen(this, sceneMgr, viewport, camera, root);

            if(ss!=null)
            {
                ((AbstractScreen)currentScreen).SetScreenState(ss);
            }
            

            currentScreen.DisplayGUI(justMenu);
        }

        public void MinimizeWindow()
        {
            window.SetVisible(false);
            window.SetFullscreen(false, 0, 0);
            window.Resize(0, 0);
            window.Update(true);
        }

        public void MaximizeWindow()
        {
            window.SetFullscreen(true, windowWidth, windowHeight);
            window.SetVisible(true);
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
            ExitGame(GotoDonateWebPageDo);
        }
        public void GotoDonateWebPageDo()
        {
            string url = "http://wingsoffury2.com/page/donate?v="+EngineConfig.C_WOF_VERSION+"_"+EngineConfig.C_IS_DEMO.ToString();
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
            ExitGame(GotoUpdateWebPageDo);
        }


        public void GotoUpdateWebPageDo()
        {
            string url = "http://wingsoffury2.com/update.php?v="+EngineConfig.C_WOF_VERSION+"&d="+EngineConfig.C_IS_DEMO.ToString();
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
            if ((screen is EndingScreen)) return true;
            if ((screen is GameScreen)) return true;
            if ((screen is IntroScreen)) return true;
            if ((screen is AbstractOptionsScreen) && (screen as AbstractOptionsScreen).ForceRebuild) return true;
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
    }

    #endregion
}