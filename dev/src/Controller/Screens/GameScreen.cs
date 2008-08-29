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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BetaGUI;
using Microsoft.DirectX.DirectSound;
using Mogre;
using MOIS;
using Wof.Controller.Indicators;
using Wof.Languages;
using Wof.Model.Exceptions;
using Wof.Model.Level;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Troops;
using Wof.Model.Level.Weapon;
using Wof.View;
using Wof.View.Effects;
using Button=BetaGUI.Button;
using FontManager=Wof.Languages.FontManager;
using Math=System.Math;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.Controller.Screens
{
    internal class GameScreen : MenuScreen, IController, BetaGUIListener
    {
        private const String C_LEVEL_FOLDER = "levels";
        private const String C_LEVEL_PREFIX = "level-";
        private const String C_LEVEL_POSTFIX = ".dat";

        private const int C_SOLDIER_SCORE = 10;
        private const int C_BARRACK_SCORE = 20;
        private const int C_WOODEN_BUNKER_SCORE = 30;
        private const int C_CONCRETE_BUNKER_SCORE = 50;
        private const int C_ENEMY_PLANE_SCORE = 35;
        private const int C_LIFE_LEFT = 50;


        public const float C_RESPONSE_DELAY = 0.16f;

        private int lastFireTick = 0;

        // obiekty kontroli sceny
        private readonly GameEventListener gameEventListener;
        private SceneManager sceneMgr;

        private Viewport viewport;
        private Camera camera;

        private int lives;

        public int Lives
        {
            get { return lives; }
        }


        // Obiekty kontroli GUI
        private GUI mGui;
        private Window guiWindow;
        private Button resumeButton = null, exitButton = null, gameOverButton = null;
        private Button bombsButton, rocketsButton;
        private uint mousePosX, mousePosY;


        //private Button forceGameOver, forceWin;

        // Obiekty kontroli gry
        private Level currentLevel;

        public Level CurrentLevel
        {
            get { return currentLevel; }
        }

        private int level;

        public int Level
        {
            get { return level; }
        }

        private LevelView levelView;

        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }


        private Boolean isGamePaused;

        public Boolean IsGamePaused
        {
            get { return isGamePaused; }
            set { isGamePaused = value; }
        }

        private Boolean isInGameOverMenu = false;

        public Boolean IsInGameOverMenu
        {
            get { return isInGameOverMenu; }
            set { isInGameOverMenu = value; }
        }

        private Boolean isInPauseMenu = false;

        public Boolean IsInPauseMenu
        {
            get { return isInPauseMenu; }
            set { isInPauseMenu = value; }
        }


        private Boolean changingAmmo;
        private Boolean prevCouldChangeAmmo = false;
        private Boolean prevFuelTooLow = false;
        private Boolean prevOilTooLow = false;

        private Boolean readyForLevelEnd = false;
        private Boolean nextFrameGotoNextLevel = false;

        private FrameWork framework;

        public FrameWork Framework
        {
            get { return framework; }
        }

        private Boolean loading;
        private DateTime loadingStart;

        // zmienna okresla, czy w dalszym ciagu nalezy odtwarzac 
        // dzwiek dzialka
        // jezeli w ktores klatce okaze sie, ze nie odebrano komunikatu
        // OnFireGun, dzwiek dzialka bedzie wylaczany
        private Boolean isStillFireGun;

     
        private Boolean mayPlaySound;

        private readonly object loadingLock;
        private Thread loaderThread;
        private Overlay loadingOverlay;
      
        private Boolean firstTakeOff = true;

        private IndicatorControl indicatorControl;
        private GameMessages gameMessages;
        private bool wasLeftMousePressed = false;

        public GameScreen(GameEventListener gameEventListener,
                          FrameWork framework, Device directSound, int lives, int levelNo)
        {
            this.gameEventListener = gameEventListener;
            sceneMgr = FrameWork.SceneMgr;
            viewport = framework.Viewport;

            camera = framework.Camera;
            mousePosX = (uint) viewport.ActualWidth/2;
            mousePosY = (uint) viewport.ActualHeight/2;
            this.framework = framework;
            level = levelNo;
            mayPlaySound = false;
            changingAmmo = false;
            loadingLock = new object();
            score = 0;
            this.lives = 2;

            loading = true;
            wasLeftMousePressed = false;

            indicatorControl = new IndicatorControl(framework.OverlayViewport, framework.MinimapViewport, this);
            gameMessages = new GameMessages(framework.Viewport);
        }

        private void StartLoading()
        {
            loading = true;
            lock (loadingLock)
            {
                try
                {
                    loadingStart = DateTime.Now;
                    LogManager.Singleton.LogMessage("About to load level view...", LogMessageLevel.LML_CRITICAL);
                    levelView = new LevelView(framework, this);
                    LogManager.Singleton.LogMessage("About to load model...", LogMessageLevel.LML_CRITICAL);
                    currentLevel = new Level(GetLevelName(level), this);
                    LogManager.Singleton.LogMessage("About to register level " + level + " to view...", LogMessageLevel.LML_CRITICAL);

                    levelView.OnRegisterLevel(currentLevel);

                    LogManager.Singleton.LogMessage("About to register player plane", LogMessageLevel.LML_CRITICAL);
                    OnRegisterPlane(currentLevel.UserPlane);


                    LogManager.Singleton.LogMessage("About to register enemy planes", LogMessageLevel.LML_CRITICAL);
                    if (currentLevel.EnemyPlanes.Count > 0) //warunek dodany przez Emila
                        OnRegisterPlane(currentLevel.EnemyPlanes[currentLevel.EnemyPlanes.Count - 1]);

                    LogManager.Singleton.LogMessage("About to register storage planes", LogMessageLevel.LML_CRITICAL);
                    foreach (StoragePlane sp in currentLevel.StoragePlanes)
                    {
                        OnRegisterPlane(sp);
                    }
                    LogManager.Singleton.LogMessage("Finished loading level.", LogMessageLevel.LML_CRITICAL);
                    SoundManager.Instance.PreloadRandomIngameMusic();

                    if (Level == 1 && firstTakeOff)
                    {
                        MessageEntry message =
                            new MessageEntry(0.2f, 0.4f,
                                             LanguageResources.GetString(LanguageKey.PressEToTurnOnTheEngine), true,
                                             true);
                        message.IncreaseY(-message.CharHeight/2.0f);
                        gameMessages.AppendMessage(message);
                    }

                    loading = false;
                }
                catch (SEHException)
                {
                    // Check if it's an Ogre Exception
                    if (OgreException.IsThrown)
                    {
                        Game.ShowOgreException();
                        throw;
                    }
                    else
                        throw;
                }
                catch (LevelCorruptedException exc)
                {
              
                    Game.getGame().Window.Destroy();
                    Wof.Controller.FrameWork.ShowWofException(exc);
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, exc.Message + " " + exc.StackTrace);
                    Debug.WriteLine(exc.ToString());
                    Application.Exit();
                
                }
            }
        }

       
        public static String GetLevelName(int levelNo)
        {
            return C_LEVEL_FOLDER + "\\"
                   + C_LEVEL_PREFIX + levelNo + C_LEVEL_POSTFIX;
        }

        /// <summary>
        /// Zwalnia screeny z pamiêci
        /// </summary>
        /// <returns>iloœæ screenóww</returns>
        public int FreeSplashScreens()
        {

            string baseName = "Tutorial";
            string lang = "_" + LanguageManager.ActualLanguageCode;
            int n = 1;
            while (
                ResourceGroupManager.Singleton.ResourceExists(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                                                              baseName + n + lang + ".jpg"))
            {
                n++;
                TextureManager.Singleton.Unload(baseName + n + lang + ".jpg");
            }
            LogManager.Singleton.LogMessage("Finished freeing splash screens", LogMessageLevel.LML_CRITICAL);
            return n - 1;

        }


        public void DisplayGUI(Boolean justMenu)
        {
            string baseName;
            //Console.WriteLine("LOADING WAITING GUI");        

            baseName = "Tutorial";
            string lang = "_" + LanguageManager.ActualLanguageCode;
            int n = FreeSplashScreens();
            if (n > 0)
            {
                n = (new Random().Next(1, n));
            }
            else
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL,
                                                "Error: No tutorial screens available for language:" +
                                                LanguageManager.ActualLanguageName);
            }

            loadingOverlay = OverlayManager.Singleton.GetByName("Wof/Loading");
            MaterialPtr overlayMaterial = MaterialManager.Singleton.GetByName("SplashScreen");

            OverlayElement loadingText = OverlayManager.Singleton.GetOverlayElement("Wof/LoadingScreenText");
            if (n > 0)
            {
                TextureManager.Singleton.Load(baseName + n + lang + ".jpg", "General").Load(false); // preload

                overlayMaterial.Load();
                overlayMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(baseName + n + lang +
                                                                                                   ".jpg");
                overlayMaterial = null;
                loadingText.Caption = LanguageResources.GetString(LanguageKey.Level) + ": "+ level;
                loadingText.SetParameter("font_name", FontManager.CurrentFont);
                loadingText = null;
            }
            loadingOverlay.Show();

            EffectsManager.Singleton.Load();

            Console.WriteLine("Starting loading thread...");
            // start loading
            loaderThread = new Thread(StartLoading);
            loaderThread.Start();
        }

    
        public void CleanUp(Boolean justMenu)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CleanUp");
            EffectsManager.Singleton.Clear();
            FrameWork.DestroyScenes();
            SoundManager.Instance.StopMusic();


            // test pamiêci
            TextureManager.Singleton.UnloadAll();
            MaterialManager.Singleton.UnloadAll();
            MeshManager.Singleton.UnloadAll();


            try
            {
                if (mGui != null)
                {
                    mGui.killGUI();
                }
            }
            catch 
            {
                // silent
            }
            //  LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Clearing meshes");
            // MeshManager.Singleton.UnloadAll();
            // MeshManager.Singleton.RemoveAll();


            indicatorControl.ClearGUI();
            gameMessages.DestroyMessageContainer();

            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CleanUpEnd");
        }


        public int GetHighscore()
        {
            HighscoreUtil util = new HighscoreUtil();
            return util.FindHighHighscore();
        }

        public void HandleInput(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            if (nextFrameGotoNextLevel)
            {
                isGamePaused = true;
                nextFrameGotoNextLevel = false;
                SoundManager.Instance.HaltEngineSound();
                SoundManager.Instance.HaltGunFireSound();
                SoundManager.Instance.HaltWaterBubblesSound();
                levelView.OnStopPlayingEnemyPlaneEngineSounds();
                SoundManager.Instance.HaltOceanSound();
                increaseScore(this.lives * C_LIFE_LEFT);
                levelView.Destroy();
                gameEventListener.GotoNextLevel();
            }

            // przeladuj efekty
            /*if (loadingOverlay != null)
            {
                EffectsManager.Singleton.UpdateTimeAndAnimateAll(evt.timeSinceLastFrame);
            }*/
            // wy³¹czone z uwagi na error z 'collection was modified'

            lock (loadingLock)
            {


                if (!loading && loadingOverlay == null)
                {
                    isStillFireGun = false;
                    inputMouse.Capture();
                    inputKeyboard.Capture();
                    if(inputJoystick != null) inputJoystick.Capture();
                    Vector2 joyVector = FrameWork.GetJoystickVector(inputJoystick);


                    if ((inputKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Escape))  && Button.CanChangeSelectedButton(3.5f) &&
                        !changingAmmo)
                    {
                        if (!isGamePaused)
                        {
                            DisplayPause();
                            SoundManager.Instance.HaltEngineSound();
                            SoundManager.Instance.HaltWaterBubblesSound();
                            SoundManager.Instance.HaltOceanSound();
                        }
                        else
                        {
                            ClearPause();
                            SoundManager.Instance.LoopOceanSound();
                            if (mayPlaySound)
                            {
                                SoundManager.Instance.LoopEngineSound();
                            }
                        }
                        Button.ResetButtonTimer();
                    }

                    if (mGui != null)
                    {
                        MouseState_NativePtr mouseState = inputMouse.MouseState;

                        mousePosX = (uint) (mousePosX + mouseState.X.rel*2);
                        mousePosY = (uint) (mousePosY + mouseState.Y.rel*2);

                        if ((int) mousePosX + mouseState.X.rel*2 < 0)
                        {
                            mousePosX = 0;
                        }
                        if ((int) mousePosY + mouseState.Y.rel*2 < 0)
                        {
                            mousePosY = 0;
                        }

                        if (mousePosX > framework.OverlayViewport.ActualWidth)
                        {
                            mousePosX = (uint) framework.OverlayViewport.ActualWidth;
                        }
                        if (mousePosY > framework.OverlayViewport.ActualHeight)
                        {
                            mousePosY = (uint) framework.OverlayViewport.ActualHeight;
                        }

                        mGui.injectMouse(mousePosX, mousePosY, false);

                        if (mouseState.ButtonDown(MouseButtonID.MB_Left))
                        {
                          wasLeftMousePressed = true;
                        }
                        else if (wasLeftMousePressed)
                        {
                          //w poprzedniej klatce uzytkownik
                          // trzymal wcisniety przycisk myszki
                          // a teraz go zwolnil
                          mGui.injectMouse(mousePosX, mousePosY, true);
                          wasLeftMousePressed = false;
                        }

                        


                        // przyciski - pauza
                        if (isInPauseMenu)
                        {
                            if (Button.CanChangeSelectedButton())
                            {
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_UP) || joyVector.y > 0)
                                {
                                    resumeButton.activate(true);
                                    exitButton.activate(false);
                                    Button.ResetButtonTimer();
                                }
                                else if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN) || joyVector.y < 0)
                                {
                                    resumeButton.activate(false);
                                    exitButton.activate(true);
                                    Button.ResetButtonTimer();
                                }
                            }

                            if (inputKeyboard.IsKeyDown(KeyCode.KC_RETURN) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Enter)) 
                            {
                                if (exitButton.activated) Button.TryToPressButton(exitButton, 0.1f);
                                else
                                {
                                    if (resumeButton.activated) Button.TryToPressButton(resumeButton, 0.4f);
                                }
                            }
                        }


                        // przyciski - game over
                        if (isInGameOverMenu && (inputKeyboard.IsKeyDown(KeyCode.KC_RETURN) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Enter))) 
                        {
                            Button.TryToPressButton(gameOverButton);
                            
                        }

                        // zmiana amunicji za pomoc¹ klawiatury
                        if (changingAmmo && Button.CanChangeSelectedButton(3.0f))
                        {
                            // wybierz bomby
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_UP) || inputKeyboard.IsKeyDown(KeyCode.KC_B) || joyVector.y > 0)
                            {
                                bombsButton.callback.LS.onButtonPress(bombsButton);
                                Button.ResetButtonTimer();
                            }
                            else
                                // wybierz rakiety
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN) || inputKeyboard.IsKeyDown(KeyCode.KC_R) || joyVector.y < 0)
                                {
                                    rocketsButton.callback.LS.onButtonPress(rocketsButton);
                                    Button.ResetButtonTimer();
                                }


                            if (inputKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Escape))
                            {
                                ClearRestoreAmmunition();
                                Button.ResetButtonTimer();
                            }
                        }

                       // mouseState = null;
                    }


                    // jezeli uzytkownik bedzie w stanie pauzy, to nie
                    // moze miec mozliwosci ruszac samolotem
                    if (!isGamePaused && !changingAmmo)
                    {
                        //  if (timeCounter > 0.5f)
                        {
                            if (EngineConfig.DebugInfo)
                            {
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_B) && Button.CanChangeSelectedButton())
                                {
                                    levelView.SceneMgr.ShowBoundingBoxes = !levelView.SceneMgr.ShowBoundingBoxes;
                                    Button.ResetButtonTimer();
                                }
                            }
                          
                            // nie chcemy, zeby uzytkownik ruszyl sie jednoczesnie
                            // w lewo i w prawo, dlatego pierwszenstwo ma
                            // ruch w lewo.
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_LEFT) || joyVector.x < 0)
                            {
                                currentLevel.OnSteerLeft();
                            }
                            else if (inputKeyboard.IsKeyDown(KeyCode.KC_RIGHT) || joyVector.x > 0)
                            {
                                currentLevel.OnSteerRight();
                            }
                           


                          

                            // góra / dó³
                            if (!EngineConfig.InverseKeys)
                            {
                                // podobnie, uzytkownik nie powinien miec mozliwosci
                                // ruszyc sie w tym samym momencie do gory i w dol.
                                // Pierwszenstwo posiada zatem gora
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_UP) || joyVector.y > 0)
                                {
                                    currentLevel.OnSteerUp();
                                }
                                else if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN) || joyVector.y < 0)
                                {
                                    currentLevel.OnSteerDown();
                                }
                            }
                            else
                            {
                                // klawisze zamienione miejscami.
                                // priorytet ma ruch do gory
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN) || joyVector.y < 0)
                                {
                                    currentLevel.OnSteerUp();
                                }
                                else if (inputKeyboard.IsKeyDown(KeyCode.KC_UP) || joyVector.y > 0)
                                {
                                    currentLevel.OnSteerDown();
                                }

                            }


                            // uzytkownik moze wlaczyc i wylaczyc silnik w kazdym 
                            // momencie lotu w polaczeniu z kazda kombinacja klawiszy
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_E) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Engine))
                            {
                                currentLevel.OnToggleEngineOn();
                            }

                            // uzytkownik moze starac sie schowac lub otworzyc
                            // podwozie w kazdym momencie lotu, o mozliwosc zajscia
                            //zdarzenia decyduje model
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_G) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Gear))
                            {
                                currentLevel.OnToggleGear();
                            }

                            // przyczep / odczep kamere
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_V) && EngineConfig.FreeLook &&
                                Button.CanChangeSelectedButton(3.0f))
                            {
                                EngineConfig.ManualCamera = !EngineConfig.ManualCamera;
                                if (!EngineConfig.ManualCamera) levelView.OnResetCamera();
                                Button.ResetButtonTimer();
                            }

                            // strzal z rakiety
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_X) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Rocket))
                            {
                                currentLevel.OnFireRocket();
                            }

                            // strzal z dzialka
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_Z) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Gun))
                            {
                                currentLevel.OnFireGun();
                            }

                            // obrót z 'pleców na brzuch' samolotu
                            // tymczasowo wy³¹czone - problematyczny obrót samolotu w modelu
                            if (inputKeyboard.IsModifierDown(Keyboard.Modifier.Ctrl))
                            {
                                currentLevel.OnSpinPressed();
                            }
                          


                            // zmiana kamery
                            if ((inputKeyboard.IsKeyDown(KeyCode.KC_C) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Camera)) && Button.CanChangeSelectedButton(3.0f))
                            {
                                if (gameMessages.IsMessageQueueEmpty())
                                {
                                    gameMessages.AppendMessage(String.Format(@"{0}...", LanguageResources.GetString(LanguageKey.CameraChanged)));
                                }
                                levelView.OnChangeCamera();
                                Button.ResetButtonTimer();
                            }

                            // screenshots
                            if (inputKeyboard.IsKeyDown(KeyCode.KC_SYSRQ) && Button.CanChangeSelectedButton())
                            {
                                string[] temp = Directory.GetFiles(Environment.CurrentDirectory, "screenshot*.jpg");
                                string fileName = string.Format("screenshot{0}.jpg", temp.Length + 1);
                                framework.TakeScreenshot(fileName);
                                gameMessages.AppendMessage(String.Format("{0} '{1}'",
                                                                         LanguageResources.GetString(
                                                                             LanguageKey.ScreenshotWrittenTo), fileName));
                                Button.ResetButtonTimer();
                            }
                        }


                        if (!isGamePaused)
                        {
                            // odswiezam model przed wyrenderowaniem klatki
                            // czas od ostatniej klatki przekazywany jest w sekundach
                            // natomiast model potrzebuje wartosci w milisekundach
                            // dlatego mnoze przez 1000 i zaokraglam     


                            currentLevel.Update((int) Math.Round(evt.timeSinceLastFrame*1000));

                            if (!readyForLevelEnd)
                            {
                                // sprawdzam, czy to juz ten moment
                                currentLevel.OnSoldierEndDeath();
                            }

                            SoundManager.Instance.SetEngineFrequency((int) currentLevel.UserPlane.AirscrewSpeed*7);

                            if (levelView.CurrentCameraHolderIndex == 0)
                            {
                                framework.HandleCameraInput(inputKeyboard, inputMouse, inputJoystick, evt, framework.Camera,
                                                            framework.MinimapCamera, currentLevel.UserPlane);
                            }
                            else
                            {
                                framework.HandleCameraInput(inputKeyboard, inputMouse, inputJoystick, evt, null,
                                                            framework.MinimapCamera, currentLevel.UserPlane);
                            }


                            // try
                            //  {
                            levelView.OnFrameStarted(evt);
                            // }
                            // catch (Exception ex)
                            //  {
                            //      Console.WriteLine("EXCEPTION VIEW!!" + ex.ToString());
                            //
                            // }
                        }
                    }

                    if (indicatorControl.WasDisplayed == false)
                    {
                        indicatorControl.DisplayIndicator();
                    }
                    else
                    {
                        indicatorControl.UpdateGUI(evt.timeSinceLastFrame);
                        gameMessages.UpdateControl();
                    }

                    ControlGunFireSound();
                    ControlChangeAmmunition();
                    ControlFuelState();
                    ControlOilState();

                    if (!isGamePaused && !changingAmmo)
                    {
                        ControlEnemyEngineSounds();
                    }
                }
                else
                {
                    if (loading == false)
                    {
                        TimeSpan diff = DateTime.Now.Subtract(loadingStart);
                        if (EngineConfig.DebugStart || diff.TotalMilliseconds > EngineConfig.LOADING_DELAY)
                        {
                            levelView.BuildCameraHolders();
                            if (!EngineConfig.FreeLook)
                            {
                                levelView.OnResetCamera();
                            }

                            if (EngineConfig.MinimapNoseCamera)
                            {
                                framework.MinimapViewport.Camera = framework.MinimapNoseCamera;
                                levelView.FindPlaneView(currentLevel.UserPlane).InnerNode.AttachObject(
                                    framework.MinimapNoseCamera);
                            }


                            levelView.SetVisible(true);
                            loadingOverlay.Hide();
                            loadingOverlay.Dispose();
                            loadingOverlay = null;

                            FreeSplashScreens();
                            SoundManager.Instance.LoopOceanSound();
                        }
                    }
                }
            }
        }

        private void ControlGunFireSound()
        {
            if (isStillFireGun)
            {
                SoundManager.Instance.LoopGunFireSoundIfCan();
            }
            else
            {
                SoundManager.Instance.HaltGunFireSound();
            }
        }

        public void ControlChangeAmmunition()
        {
            if (currentLevel.UserPlane.LocationState == LocationState.AircraftCarrier)
            {
                if (currentLevel.UserPlane.CanChangeAmmunition)
                {
                    if (!prevCouldChangeAmmo)
                    {
                        gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.PressXToChangeAmmo));
                            // GameMessages.C_PRESS_X);
                        prevCouldChangeAmmo = true;
                    }
                    return;
                }
            }
            prevCouldChangeAmmo = false;
        }

        public void ControlFuelState()
        {
            if (currentLevel.UserPlane.Petrol < 10)
            {
                if (!prevFuelTooLow)
                {
                    string message =
                        String.Format("{0} {1}",
                                      String.Format("{0}!", LanguageResources.GetString(LanguageKey.LowFuelWarning)),
                                      LanguageResources.GetString(LanguageKey.LandOnTheCarrierAndPressXToRefuel));
                    gameMessages.AppendMessage(message);
                    prevFuelTooLow = true;
                }
                return;
            }
            prevFuelTooLow = false;
        }

        public void ControlOilState()
        {
            if (currentLevel.UserPlane.Oil < 10)
            {
                if (!prevOilTooLow)
                {
                    string message = String.Format("{0} {1}",
                                                   String.Format("{0}!",
                                                                 LanguageResources.GetString(LanguageKey.LowOilWarning)),
                                                   LanguageResources.GetString(
                                                       LanguageKey.LandOnTheCarrierAndPressXToRepair));
                    gameMessages.AppendMessage(message);
                    prevOilTooLow = true;
                }
                return;
            }
            prevOilTooLow = false;
        }

        private void ControlEnemyEngineSounds()
        {
            float distance;
            foreach (EnemyPlane ep in currentLevel.EnemyPlanes)
            {
                distance = currentLevel.UserPlane.XDistanceToPlane(ep);

                if (distance == -1)
                {
                    // w tej sytuacji samolot przeciwnika zniknal z planszy 
                    // i trzeba wylaczyc dzwiek
                
                   // SoundManager.Instance.HaltEnemyEngineSound();
                    levelView.OnStopPlayingEnemyPlaneEngineSound(ep);
                }
                else if (distance != -1)
                {
                   // glosnosc sterowana przez FreeSL
                  
                   /* SoundManager.Instance.SetEnemyEngineVolume((int) distance,
                                                               (int) ep.AirscrewSpeed*5);
                    SoundManager.Instance.LoopEnemyEngineSound();*/

                    levelView.OnLoopEnemyPlaneEngineSound(ep);
                }
                else
                {
                    // nie robimy nic
                }
            }
        }


        private void DisplayPause()
        {
            isGamePaused = true;
            isInPauseMenu = true;   
            levelView.OnStopPlayingEnemyPlaneEngineSounds();


            mGui = new GUI(FontManager.CurrentFont, 24);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
          //  mGui.injectMouse(0, 0, false);
            guiWindow = mGui.createWindow(new Vector4(0,
                                                      0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int) wt.NONE, LanguageResources.GetString(LanguageKey.Pause));
            Callback cc = new Callback(this);
            resumeButton =
                guiWindow.createButton(
                    new Vector4(viewport.ActualWidth/4, viewport.ActualHeight/4 + 30, viewport.ActualWidth/2, 30),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Resume), cc);
            exitButton =
                guiWindow.createButton(
                    new Vector4(viewport.ActualWidth/4, viewport.ActualHeight/4 + 60, viewport.ActualWidth/2, 30),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.ExitToMenu), cc);
            resumeButton.activate(true);
            guiWindow.show();
        }

        private void ClearPause()
        {
            isGamePaused = false;
            isInPauseMenu = false;
            mGui.killGUI();
        }

        private void DisplayGameover()
        {
            OverlayContainer c;
            isGamePaused = true;
            levelView.OnStopPlayingEnemyPlaneEngineSounds();
            SoundManager.Instance.HaltOceanSound();
            mGui = new GUI(FontManager.CurrentFont, 24);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
            //mGui.injectMouse(0, 0, false);
            guiWindow = mGui.createWindow(new Vector4(0,
                                                      0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int) wt.NONE,
                                          LanguageResources.GetString(LanguageKey.Pause));
            Callback cc = new Callback(this);
            c = guiWindow.createStaticText(
                new Vector4(viewport.ActualWidth/4, viewport.ActualHeight/4 + 30, viewport.ActualWidth/2, 30),
                LanguageResources.GetString(LanguageKey.GameOver));
            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f, 0.0f, 0.0f), new ColourValue(0.9f, 0.1f, 0.0f));

            mGui.mFontSize = 20;
           


            // game stats
            c = guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 90, viewport.ActualWidth / 2, 30),
               LanguageResources.GetString(LanguageKey.GameStats));

            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f,0.8f,0.0f), new ColourValue(0.9f,0.7f,0.0f));
         

            guiWindow.createStaticText(
               new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 120, viewport.ActualWidth / 2, 30),
                LanguageResources.GetString(LanguageKey.BombsDropped) + " "+ this.currentLevel.Statistics.BombCount);

            guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 150, viewport.ActualWidth / 2, 30),
                LanguageResources.GetString(LanguageKey.BombsAccuracy) + " " + this.currentLevel.Statistics.BombStats + "%");


            guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 180, viewport.ActualWidth / 2, 30),
               LanguageResources.GetString(LanguageKey.RocketsFired) + " " + this.currentLevel.Statistics.RocketCount);
            
            guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 210, viewport.ActualWidth / 2, 30),
                LanguageResources.GetString(LanguageKey.RocketsAccuracy) + " " + this.currentLevel.Statistics.RocketStats + "%");

           
            guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 240, viewport.ActualWidth / 2, 30),
                LanguageResources.GetString(LanguageKey.GunShellsFired) + " " + this.currentLevel.Statistics.GunCount);

            guiWindow.createStaticText(
              new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 270, viewport.ActualWidth / 2, 30),
               LanguageResources.GetString(LanguageKey.GunAccuracy) + " " + this.currentLevel.Statistics.GunStats + "%");


            guiWindow.createStaticText(
                new Vector4(10 + viewport.ActualWidth / 4, viewport.ActualHeight / 4 + 310, viewport.ActualWidth / 2, 30),
                 LanguageResources.GetString(LanguageKey.PlanesDestroyed) + " " + this.currentLevel.Statistics.PlanesShotDown);


            mGui.mFontSize = 24;

            gameOverButton =
                guiWindow.createButton(
                    new Vector4(viewport.ActualWidth/4, viewport.ActualHeight/4 + 370, viewport.ActualWidth/2, 30),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.OK), cc);


            guiWindow.show();
            isInGameOverMenu = true;
        }

        private void DisplayChangeAmmo()
        {
            mGui = new GUI(FontManager.CurrentFont, 24);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
          //  mGui.injectMouse(0, 0, false);
            guiWindow = mGui.createWindow(new Vector4(0,
                                                      0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int) wt.NONE,
                                          LanguageResources.GetString(LanguageKey.SelectAmmunition));
            Callback cc = new Callback(this);
            bombsButton = guiWindow.createButton(new Vector4(viewport.ActualWidth/4,
                                                             viewport.ActualHeight/4 + 60,
                                                             viewport.ActualWidth/2, 30),
                                                 "bgui.button",
                                                 LanguageResources.GetString(LanguageKey.Bombs), cc);
            rocketsButton = guiWindow.createButton(new Vector4(viewport.ActualWidth/4,
                                                               viewport.ActualHeight/4 + 90,
                                                               viewport.ActualWidth/2, 30),
                                                   "bgui.button",
                                                   LanguageResources.GetString(LanguageKey.Rockets), cc);
            guiWindow.show();
        }

        private void ClearRestoreAmmunition()
        {
            changingAmmo = false;
            mGui.killGUI();
            SoundManager.Instance.LoopOceanSound();
            if (mayPlaySound)
            {
                SoundManager.Instance.LoopEngineSound();
            }
        }

        #region BetaGUIListener Members

        public void onButtonPress(Button referer)
        {
            if (referer == resumeButton)
            {
                ClearPause();
                SoundManager.Instance.LoopOceanSound();
                if (mayPlaySound)
                {
                    SoundManager.Instance.LoopEngineSound();
                }
            }
            if (referer == exitButton)
            {
                if (levelView != null)
                {
                    levelView.Destroy();
                }
                gameEventListener.GotoStartScreen();
            }
            if (referer == gameOverButton)
            {
                if (levelView != null)
                {
                    levelView.Destroy();
                }

                HighscoreUtil util = new HighscoreUtil();
                int leastScore = util.FindLeastHighscore();

                if (score >= leastScore)
                {
                    gameEventListener.GotoEnterScoreScreen(score);
                    isInGameOverMenu = false;
                    return;
                }
                else
                {
                    gameEventListener.GotoHighscoresScreen();
                    isInGameOverMenu = false;
                    return;
                }
            }
            if (referer == rocketsButton)
            {
                // zmieniam bron na rakiety
                currentLevel.OnRestoreAmmunition(WeaponType.Rocket);
                ClearRestoreAmmunition();
                SoundManager.Instance.PlayReloadSound();
                indicatorControl.ChangeAmmoType(WeaponType.Rocket);

                return;
            }
            if (referer == bombsButton)
            {
                // zmieniam bron na bomby
                currentLevel.OnRestoreAmmunition(WeaponType.Bomb);
                ClearRestoreAmmunition();
                SoundManager.Instance.PlayReloadSound();
                indicatorControl.ChangeAmmoType(WeaponType.Bomb);
                return;
            }
        }

        #endregion

        #region IController Members

        /// <summary>
        /// Funkcja rejestruje strzal do samolotu.
        /// </summary>
        /// <param name="bunker">Bunkier ktory strzelil.</param>
        /// <param name="plane">Samolot gracza.</param>
        public void OnBunkerFire(BunkerTile bunker, Plane plane)
        {
            SoundManager.Instance.PlayBunkerFireSound();
            levelView.OnBunkerFire(bunker, plane);
            // Console.WriteLine("OnBunkerFire " + " BunkerTile bunker " + " Plane plane");
        }

        /// <summary>
        /// Funkcja rejestruje na planszy nowego zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz do zarejestrowania.</param>
        public void OnRegisterSoldier(Soldier soldier)
        {
            levelView.OnRegisterSoldier(soldier);
        }


        /// <summary>
        /// Funkcja usuwa zolnierza z widoku.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostanie odrejestrowany.</param>
        public void UnregisterSoldier(Soldier soldier)
        {
            levelView.OnUnregisterSoldier(soldier);
        }

        private void increaseScore(int baseScore)
        {
            switch (EngineConfig.Difficulty)
            {
                case EngineConfig.DifficultyLevel.Easy:
                    score += (int) (baseScore*0.6f);
                    break;
                case EngineConfig.DifficultyLevel.Hard:
                    score += (int) (baseScore*1.4f);
                    break;
                default:
                    score += baseScore;
                    break;
            }
        }


        /// <summary>
        /// Funkcja rozpoczyna animacje usmiercania zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal trafiony.</param>
        /// <param name="gun"></param>
        public void OnSoldierBeginDeath(Soldier soldier, bool gun)
        {
            levelView.OnKillSoldier(soldier, !gun);
            increaseScore(C_SOLDIER_SCORE);
            currentLevel.OnSoldierEndDeath();
        }

        public void OnRegisterPlane(Plane plane)
        {
            levelView.OnRegisterPlane(plane);
        }

        public void OnUnregisterPlane(Plane plane)
        {
            levelView.OnUnregisterPlane(plane);
        }

        /// <summary>
        /// Funkcja zglasza o zatrzymaniu pracy silnika.
        /// </summary>
        public void OnTurnOffEngine()
        {
            mayPlaySound = false;
            SoundManager.Instance.HaltEngineSound();
            SoundManager.Instance.PlayStopEngineSound();
        }

        /// <summary>
        /// Funkcja zglasza o rozpoczeciu pracy silnika.
        /// </summary>
        public void OnTurnOnEngine()
        {
            mayPlaySound = true;
            SoundManager.Instance.PlayStartEngineSound(startEngineSound_Ending);

            SoundManager.Instance.PlayRandomIngameMusic(EngineConfig.MusicVolume);

            // wy³¹cz komunikat 
            if (gameMessages.PeekMessage().Equals(LanguageResources.GetString(LanguageKey.PressEToTurnOnTheEngine)))
            {
                gameMessages.ClearMessage();
            }
        }

        private void startEngineSound_Ending(object sender, EventArgs e)
        {
            if (mayPlaySound)
            {
                SoundManager.Instance.SetEngineFrequency(10000);

                if (!isGamePaused && !changingAmmo)
                {
                    SoundManager.Instance.LoopEngineSound();
                }
            }
        }

        /// <summary>
        /// Funkcja zglasz, ze proba uruchomienia silnika.
        /// samolotu sie niepowiodla.
        /// </summary>
        public void OnStartEngineFailed()
        {
            SoundManager.Instance.PlayFailedSoundIfCan();
        }

        /// <summary>
        /// Funkcja zglasza o trafieniu wrogiego samolotu bomba lub rakieta.
        /// </summary>
        /// <param name="plane">Samolot przeciwnika, ktory zostal trafiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w samolot.</param>
        public void OnEnemyPlaneBombed(Plane plane, Ammunition ammunition)
        {
            levelView.OnEnemyPlaneBombed(plane, ammunition);

            //Console.WriteLine("OnEnemyPlaneBombed " + " Plane plane " + " Ammunition ammunition ");
        }

        public void OnTileBombed(LevelTile tile, Ammunition ammunition)
        {
            if (tile is OceanTile)
            {
                levelView.OnAmmunitionExplode(tile, ammunition);
                SoundManager.Instance.PlayWaterExplosionSound();
            }
            else
            {
                levelView.OnAmmunitionExplode(tile, ammunition);
                SoundManager.Instance.PlayExposionSound();
            }
        }

        public void OnTileDestroyed(LevelTile tile, Ammunition ammunition)
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.EnemyInstallationDestroyed));
                // GameMessages.C_TILE_DESTROYED);
            if(ammunition != null)
                levelView.OnAmmunitionExplode(tile, ammunition);
            
            levelView.OnTileDestroyed(tile);

            if (tile is BarrackTile)
            {
                increaseScore(C_BARRACK_SCORE);
            }
            else if (tile is WoodBunkerTile)
            {
                increaseScore(C_WOODEN_BUNKER_SCORE);
            }
            else if (tile is ConcreteBunkerTile)
            {
                increaseScore(C_CONCRETE_BUNKER_SCORE);
            }

            SoundManager.Instance.PlayExposionSound();
        }


        public void OnSpinBegin(Plane plane)
        {
            levelView.OnBeginSpin(plane);
        }

        public void OnSpinEnd(object plane)
        {
            ((Plane) plane).OnSpinEnd();
        }

        public void OnPrepareChangeDirection(Direction newDirection, Plane plane, TurnType turnType)
        {
            levelView.OnPrepareChangeDirection(newDirection, plane, turnType);
        }


        public void OnPrepareChangeDirectionEnd(object turnInfo)
        {
            ((TurnInfo) turnInfo).plane.OnPrepareChangeDirectionEnd(((TurnInfo) turnInfo).turnDurationInSeconds);
        }

        public void OnChangeDirectionEnd(object turnInfo)
        {
            TurnInfo t = ((TurnInfo) turnInfo);
            t.plane.OnChangeDirectionEnd(t.turnType);
        }


        public void OnRegisterBomb(Bomb bomb)
        {
            SoundManager.Instance.PlayBombSound();
            levelView.OnRegisterAmmunition(bomb);
        }

        public void OnToggleGear(Plane plane)
        {
            if (plane.WheelsState == WheelsState.TogglingIn)
            {
                SoundManager.Instance.PlayGearUpSound();
            }
            else
            {
                SoundManager.Instance.PlayGearDownSound();
            }
            levelView.OnToggleGear(plane);
        }

        public void OnRegisterRocket(Rocket rocket)
        {
            SoundManager.Instance.PlayMissleSound();
            levelView.OnRegisterAmmunition(rocket);
        }

        public void OnGearToggled(object plane)
        {
            currentLevel.OnGearToggled((Plane) plane);
        }


        public void OnFireGun(Plane plane)
        {
            isStillFireGun = true;

            if (Environment.TickCount - lastFireTick >= Gun.FireInterval)
            {
                levelView.OnFireGun(plane);
                lastFireTick = Environment.TickCount;
            }
        }

        public void OnGunHit(LevelTile tile, float posX, float posY)
        {
            levelView.OnGunHit(tile, posX, posY);
        }

        /// <summary>
        /// Uruchamiane, gdy jakiœ samolot trafi drugi z dzia³ka lotniczego
        /// <author>Adam Witczak</author>
        /// </summary>
        /// <param name="plane"></param>
        public void OnGunHitPlane(Plane plane)
        {
            levelView.OnGunHitPlane(plane);
        }

        public void OnKillSoldier(Soldier soldier)
        {
            //Console.WriteLine("OnKillSoldier  Soldier soldier");
        }

        public void OnReadyLevelEnd()
        {
            readyForLevelEnd = true;
            string message =
                LanguageResources.GetString(LanguageKey.AllEnemySoldiersEliminatedLandOnTheCarrierAndPressX);
            gameMessages.AppendMessage(message);

            SoundManager.Instance.PlayFanfare();
        }

        public void OnChangeAmmunition()
        {
            currentLevel.OnSoldierEndDeath();

            levelView.OnStopPlayingEnemyPlaneEngineSounds();

            if (!readyForLevelEnd)
            {
                changingAmmo = true;
                SoundManager.Instance.HaltEngineSound();
                SoundManager.Instance.HaltOceanSound();
              //  DisplayChangeAmmo();
                DisplayGameover();
            }
            else
            {
                isGamePaused = true;
                nextFrameGotoNextLevel = true;
            }
        }


        public void OnPlaneDestroyed(Plane plane)
        {
            if (plane.IsEnemy)
            {
                gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.EnemyPlaneDownIRepeat));
                    // GameMessages.C_ENEMY_PLANE_DOWN);
                increaseScore(C_ENEMY_PLANE_SCORE);

            }
            levelView.OnPlaneDestroyed(plane);
            SoundManager.Instance.PlayExposionSound();
        }

        public void OnPlaneCrashed(Plane plane, TileKind tileKind)
        {
            if (tileKind == TileKind.Ocean)
            {
                SoundManager.Instance.PlayWaterExplosionSound();
                SoundManager.Instance.LoopWaterBubblesSound();
            }
            else
            {
                SoundManager.Instance.PlayExposionSound();
            }
            levelView.OnPlaneCrashed(plane, tileKind);


            //Console.WriteLine("OnPlaneCrashed");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plane">Wof.Model.Level.Planes.Plane</param>
        public void OnPlaneWrecked(Plane Plane)
        {
            SoundManager.Instance.HaltWaterBubblesSound();

            if (!Plane.IsEnemy)
            {
                lives--;
                if (lives < 0 || currentLevel.Lives - 1 < 0)
                {
                    isGamePaused = true;
                    DisplayGameover();
                }
                else
                {
                    isGamePaused = true;
                    currentLevel.NextLife();
                    levelView.NextLife();
                    isGamePaused = false;
                }
            }
        }


        public void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            levelView.OnCatchPlane(plane, carrierTile);
            SoundManager.Instance.PlayCatchPlaneSound();
        }

        public void OnReleasePlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            levelView.OnReleasePlane(plane, carrierTile);
            currentLevel.OnReleaseLine(carrierTile);
        }

        public void OnUnregisterRocket(Rocket rocket)
        {
            levelView.OnUnregisterRocket(rocket);
        }


        public void OnTileRestored(BunkerTile restoredBunker)
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.OhNoEnemySoldiersAreRebuildingBunker));
            SoundManager.Instance.PlayBunkerRebuild();
            levelView.OnTileRestored(restoredBunker);
        }

        public void OnTakeOff()
        {
            if (firstTakeOff && level == 1)
            {
                gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.RetractYourLandingGearWithG));
                gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.KillAllEnemySoldiers));
            }

            string message;
            switch (currentLevel.FlyDirectionHint)
            {
                case FlyDirectionHint.Left:
                    message = LanguageResources.GetString(LanguageKey.EnemyInstallationsAreLeftCaptain);
                    gameMessages.AppendMessage(message);
                    gameMessages.AppendMessage(message);
                    break;
                case FlyDirectionHint.Right:
                    message = LanguageResources.GetString(LanguageKey.EnemyInstallationsAreRightCaptain);
                    gameMessages.AppendMessage(message); 
                    gameMessages.AppendMessage(message); 
                    break;
                case FlyDirectionHint.Both:
                    message = LanguageResources.GetString(LanguageKey.EnemyInstallationsAreBothSideCaptain);
                    gameMessages.AppendMessage(message); 
                    gameMessages.AppendMessage(message);
                    break;
            }

            firstTakeOff = false;
            levelView.OnTakeOff();
        }

        public void OnTouchDown()
        {
            levelView.OnTouchDown();
        }

        public void OnEnemyPlaneFromTheSide(Boolean left)
        {
            gameMessages.AppendMessage(left
                                           ?
                                               LanguageResources.GetString(LanguageKey.EnemyPlaneApproachingFromTheLeft)
                                           :
                                               LanguageResources.GetString(LanguageKey.EnemyPlaneApproachingFromTheRight));
        }

        public void OnPlaneForceChangeDirection()
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.WeAreNotLeavingYetCaptain));
            
        }

        public void OnPlaneForceGoDown()
        {
         
        }

        public void OnEnemyAttacksCarrier()
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.WARNINGProtectTheCarrierFromEnemyPlane));
            SoundManager.Instance.PlayBuzzerSound();
        }

        public void OnPlaneWrongDirectionStart()
        {
            SoundManager.Instance.PlayIncorrectStart();
        }

        #endregion
    }
}