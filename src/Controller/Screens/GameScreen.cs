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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using AdManaged;
using BetaGUI;
using Microsoft.DirectX.DirectSound;
using Mogre;
using MOIS;
using Wof.Controller.AdAction;
using Wof.Controller.EffectBars;
using Wof.Controller.Indicators;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.Misc;
using Wof.Model.Configuration;
using Wof.Model.Exceptions;
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Effects;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.XmlParser;
using Wof.View;
using Wof.View.Effects;
using Button = BetaGUI.Button;
using FontManager = Wof.Languages.FontManager;
using Math = System.Math;
using Plane = Wof.Model.Level.Planes.Plane;
using Vector3=Mogre.Vector3;
using ViewEffectsManager = Wof.View.Effects.EffectsManager;

namespace Wof.Controller.Screens
{
    internal class GameScreen : MenuScreen, IController, BetaGUIListener
    {
        private const float C_LOADING_AD_PROBABILITY = 0.8f;
        private const float C_CHANGING_AMMO_AD_PROBABILITY = 0.65f;
        private const float C_CHANGING_AMMO_AD_MIN_TIME = 2.1f;
        private float changingAmmoTime = 0;
        private bool showingChangingAmmoAds = false;

        private WeaponType? changeAmmoToWhenCanClearRestoreAmmunitionScreen = null;

        private const int C_SOLDIER_SCORE = 10;
        private const int C_BARRACK_SCORE = 20;
        private const int C_WOODEN_BUNKER_SCORE = 30;
        private const int C_CONCRETE_BUNKER_SCORE = 50;
        private const int C_FORTRESS_BUNKER_SCORE = 150;
        private const int C_FLAK_BUNKER_SCORE = 60;  
        

        private const int C_SHIP_WOODEN_BUNKER_SCORE = 35;
        private const int C_SHIP_CONCRETE_BUNKER_SCORE = 55;

        
        private const int C_ENEMY_FIGHTER_SCORE = 35;
        private const int C_ENEMY_BOMBER_SCORE = 55;
        
        private const int C_LIFE_LEFT_SCORE = 50;

        public const float C_RESPONSE_DELAY = 0.16f;

        public const string C_AD_LOADING_ZONE = "pregame";
        public const string C_AD_GAME_ZONE = "ingame";
        public static string[] DEFAULT_AD_IMAGE_NAMES = new[] { "IngameAd1.jpg", "IngameAd2.jpg", "IngameAd3.jpg", "IngameAd4.jpg" };
       
        public const string C_ENGINE_HINT_ICON = "hint_engine.png";
        public const string C_BAD_LANDING_HINT_ICON = "hint_bad_landing.png";
		public const string C_LANDING_HINT_ICON = "hint_landing.png";

        
        public string getRandomDefaultIngameAdImageName()
        {

            return DEFAULT_AD_IMAGE_NAMES[UnitConverter.RandomGen.Next(0, DEFAULT_AD_IMAGE_NAMES.Length)];
        }

        private int changingAmmoAdId = 0;


        private int lastFireTick = 0;


        private bool showingLoadingAds = false;
        private AdManager.Ad loadingAd;
        private AdManager.Ad changingAmmoAd;
        private bool changingAmmoAdTried = false;

        /// <summary>
        /// Lista reklam dynamicznych ktore czekaja na rejestracje w warstwie view
        /// </summary>
        private Queue<AdManager.Ad> backgroundAdsToShow = new Queue<AdManager.Ad>();


        /// <summary>
        /// Indeks aktualnie zaznaczonej broni (w menu wyboru broni)
        /// </summary>
        private int ammoSelectedIndex;
        private int ammoSelectedIndexCount = 3;

        private int nextLevelMenuSelectedIndex;
        private int nextLevelMenuSelectedIndexCount = 3;

        private BulletTimeBar _bulletTimeBar;
        private AltitudeBar _altitudeBar;

        // obiekty kontroli sceny
        private readonly GameEventListener gameEventListener;
        private SceneManager sceneMgr;
        
        private Dictionary<Button, uint> soundButtonIds = new Dictionary<Button, uint>();
        private Dictionary<Button, uint> musicButtonIds = new Dictionary<Button, uint>();

        private Viewport viewport;
        private Camera camera;

        private int lives;

        public int Lives
        {
            get { return lives; }
        }


        protected uint fontSize;
        public void SetFontSize(uint fontSize)
        {
            this.fontSize = fontSize;
        }


      

        public uint GetFontSize()
        {
            return fontSize;
        }

        public uint GetTextVSpacing()
        {
            return (uint)(fontSize * 1.2f);
        }
        
        public Vector2 GetMargin()
        {
        	return new Vector2(viewport.ActualWidth * 0.01f, viewport.ActualHeight * 0.3f);
        }
        
         /// <summary>
        /// Pobiera kontrolke zawierajaca screen
        /// </summary>
        /// <returns></returns>
        public Control GetContainer()
        {
        	return (framework as Control);
        }
        
        private float XScale
        {
        	get { return 1.0f * viewport.ActualWidth / GetContainer().Width;}
        }
        
        private float YScale
        {
        	get { return 1.0f * viewport.ActualHeight / GetContainer().Height;}
        }
        
        public Vector2 ViewportToScreen(Vector2 screen)
        {
        	return new Vector2(screen.x / XScale, screen.y / YScale);
         	
        }
        
 		private bool displayed = false;
        public bool Displayed()
        {
        	return displayed;
        }
        // Obiekty kontroli GUI
        private GUI mGui;

        
        private Window guiWindow;

        private GUI missionTypeGui;
        private Window missionTypeWindow;
        
        private GUI achievementsGui;
        private Window achievementsWindow;
        
        public List<Achievement> CompletedAchievements 
        {      
        	get { return CurrentLevel.Achievements.FindAll(Predicates.GetCompletedAchievements()); }
        	
        }
        
        

        private GUI mGuiHint;
        /// <summary>
        /// Okienko dla hinta (wyspy po lewej / prawej / obu stronach)
        /// </summary>
        private Window hintWindow;
        private Button resumeButton = null, exitButton = null, gameOverButton = null, nextLevelButton = null, resumeFinishedLevelButton = null, rearmButton = null;
        private Button bombsButton, rocketsButton, torpedoesButton;
        private uint mousePosX, mousePosY;


        //private Button forceGameOver, forceWin;

        // Obiekty kontroli gry
        private Level currentLevel;

        public Level CurrentLevel
        {
            get { return currentLevel; }
        }

        private uint? levelNo;

        public uint? LevelNo
        {
            get { return levelNo; }
        }  
        
        private string levelFile;

        public string LevelFile
        {
            get { return levelFile; }
        }
        
        private LevelInfo levelInfo;

        public LevelInfo LevelInfo
        {
            get { return levelInfo; }
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


        private Boolean isInNextLevelMenu = false;

        public Boolean IsInNextLevelMenu
        {
            get { return isInNextLevelMenu; }
            set { isInNextLevelMenu = value; }
        }


        private Boolean isInPauseMenu = false;

        public Boolean IsInPauseMenu
        {
            get { return isInPauseMenu; }
            set { isInPauseMenu = value; }
        }

        private int hiscoreCache = -1;

        private Boolean changingAmmo;      
        private Boolean prevFuelTooLow = false;
        private Boolean prevOilTooLow = false;

        private Boolean readyForLevelEnd = false;
        private Boolean nextFrameGotoNextLevel = false;

        private IFrameWork framework;

        public IFrameWork Framework
        {
            get { return framework; }
        }

        private Boolean loading;
        private DateTime loadingStart;
        protected bool isFirstLoadingFrame;

      

        // zmienna okresla, czy w dalszym ciagu nalezy odtwarzac 
        // dzwiek dzialka
        // jezeli w ktores klatce okaze sie, ze nie odebrano komunikatu
        // OnFireGun, dzwiek dzialka bedzie wylaczany
        private Boolean isStillFireGun;

     
        private Boolean mayPlaySound;

        private readonly object loadingLock;
       // private Thread loaderThread;
        private Overlay loadingOverlay;
        private Overlay preloadingOverlay;
      
        private Boolean firstTakeOff = true;

        private IndicatorControl indicatorControl;
        private GameMessages gameMessages;
        private bool wasLeftMousePressed = false;
        
        protected bool isFirstFrame;

        private IController controller;

        protected PlaneType userPlaneType;

        private float survivalTime;
        private bool CountSurvivalTime
        {
            get
            {
                return currentLevel != null && currentLevel.UserPlane != null && currentLevel.MissionType == MissionType.Survival &&
                       !currentLevel.UserPlane.IsOnAircraftCarrier;
            }
            
        }


        public GameScreen(GameEventListener gameEventListener,
                          IFrameWork framework, Device directSound, int lives, LevelInfo levelInfo, PlaneType userPlaneType)
        {
        	
        	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Using keys from KeyMap.ini: \n" +  KeyMap.Instance.ToString()); // needed to init KeyMap instance
        	KeyMap.Instance.Value = KeyMap.Instance.Value;

            this.userPlaneType = userPlaneType;
        	isFirstFrame = false;
            this.gameEventListener = gameEventListener;
            sceneMgr = framework.SceneMgr;
            viewport = framework.Viewport;
            ammoSelectedIndex = ammoSelectedIndexCount; // wiêkszy ni¿ najwiêkszy mo¿liwy
            nextLevelMenuSelectedIndex = nextLevelMenuSelectedIndexCount;
            camera = framework.Camera;
            mousePosX = (uint) viewport.ActualWidth/2;
            mousePosY = (uint) viewport.ActualHeight/2;
            
            this.levelInfo = levelInfo;
            this.framework = framework;
            this.levelNo = levelInfo.GetLevelNo();            
            this.levelFile = LevelInfo.Filename;
            
            mayPlaySound = false;
            changingAmmo = false;
            changingAmmoTime = 0;
            showingChangingAmmoAds = false;
            changeAmmoToWhenCanClearRestoreAmmunitionScreen = null;
            loadingLock = new object();
            score = 0;
            survivalTime = 0;
            if(GameConsts.Game.LivesCheat) 
            {
            	this.lives = 98;	
            }
            else
            {
            	this.lives = 2;
            }
            
            this.fontSize = (uint)(EngineConfig.CurrentFontSize * viewport.ActualHeight);


            hiscoreCache = -1;
            //loading = true;
            wasLeftMousePressed = false;

            indicatorControl = new IndicatorControl(framework.OverlayMgr, framework.OverlayViewport, framework.MinimapViewport, this);
            gameMessages = new GameMessages(framework.Viewport);

           
        }
        
        
       
        
        private void UpdateHints(bool forceRefresh)
        {

           
        	
        	if(!currentLevel.CalculateFlyDirectionHint() && !forceRefresh)
        	{
        		return;
        	}
        	
        	// Game hints
        	if(mGuiHint == null)
        	{
	            mGuiHint = new GUI(FontManager.CurrentFont, fontSize);
        	}
        	if(hintWindow != null)
        	{
                mGuiHint.killWindow(hintWindow);
        	    hintWindow = null;
        	}
        	
	        hintWindow = mGuiHint.createWindow(new Vector4(0, 0.35f * viewport.ActualHeight, viewport.ActualWidth, 0.2f * viewport.ActualHeight), "", (int)wt.NONE, "");
	        
        	
            string hintLeftFilename = "hint_left.png";
            string hintRightFilename = "hint_right.png";
            if (readyForLevelEnd)
            {
                hintLeftFilename = "hint_left_landing.png";
                hintRightFilename = "hint_right_landing.png";
            }
            else
            {
                switch (this.currentLevel.MissionType)
                {
                    case MissionType.Assassination:
                        hintLeftFilename = "hint_left_assasination.png";
                        hintRightFilename = "hint_right_assasination.png";
                        break;

                    case MissionType.Naval:
                        hintLeftFilename = "hint_left_naval.png";
                        hintRightFilename = "hint_right_naval.png";
                        break;

                    case MissionType.Survival:
                    case MissionType.Dogfight:
                        hintLeftFilename = "hint_left_dogfight.png";
                        hintRightFilename = "hint_right_dogfight.png";
                        break;
                }
            }
           

           
           
            if(currentLevel.FlyDirectionHint == FlyDirectionHint.Left || currentLevel.FlyDirectionHint == FlyDirectionHint.Both)
            {
                hintWindow.createStaticImage(new Vector4(viewport.ActualWidth * 0.01f, 0, 0.9f * 0.15f * viewport.ActualWidth, 0.9f * 0.045f * viewport.ActualWidth), hintLeftFilename);
            } 
            
            if(currentLevel.FlyDirectionHint == FlyDirectionHint.Right || currentLevel.FlyDirectionHint == FlyDirectionHint.Both)
            {
                hintWindow.createStaticImage(new Vector4(viewport.ActualWidth * 0.84f, 0, 0.9f * 0.15f * viewport.ActualWidth, 0.9f * 0.045f * viewport.ActualWidth), hintRightFilename);
            }
            hintWindow.show();
        }
        
        
        private void createAchivementsGui(uint fontSize)
        {         
            achievementsGui = new GUI(FontManager.CurrentFont, fontSize, "AchievementsTypeGUI");    
            achievementsGui.SetZOrder(100);
            
            float dist = viewport.ActualWidth / 4.5f;
            float hsize = dist / 4.0f;
           
            achievementsWindow = achievementsGui.createWindow(new Vector4(0, viewport.ActualHeight - hsize - 2.5f*fontSize, dist, hsize + 2.5f*fontSize), "", (int)wt.NONE, "");
            
            List<Achievement> completedAchievementsBefore = LoadGameUtil.Singleton.GetCompletedAchievementsForLevel(levelInfo);
           
            for(int k =0; k < this.CurrentLevel.Achievements.Count; k++) {
            	                    	
            	Achievement a = CurrentLevel.Achievements[k];
            	if(completedAchievementsBefore != null && completedAchievementsBefore.Contains(a)) {
            		CurrentLevel.Achievements[k].CopyFrom(completedAchievementsBefore.Find(delegate(Achievement ach) { return ach.Equals(a); }));
            	                  		
                	OnAchievementFulFilled(CurrentLevel.Achievements[k], false);
            	} 
            	OnAchievementUpdated(CurrentLevel.Achievements[k]);
            }
            
            if(CurrentLevel.Achievements.Count > 0) {
            
	            string achievementsText = /*LanguageResources.GetString(LanguageKey.achi)*/  "Achievements";
	            float textWidth = ViewHelper.MeasureText(FontManager.CurrentFont, achievementsText, fontSize);
             	OverlayContainer c =  achievementsWindow.createStaticText(new Vector4(fontSize, 0,  textWidth,  achievementsWindow.h), achievementsText, new ColourValue(0.3f, 0.3f, 0.3f));
            }           	
           
            achievementsWindow.show();
                    
    	}

        private void createMissionTypeGui(uint fontSize)
        {
        	// missiontype gui        	
            float dist = viewport.ActualWidth / 4.5f;
            float hsize = dist / 4.0f;
                    
            int h = (int)GetTextVSpacing();
            missionTypeGui = new GUI(FontManager.CurrentFont, fontSize, "MissionTypeGUI");
        
            missionTypeWindow = missionTypeGui.createWindow(new Vector4(0, viewport.ActualHeight - hsize- 2.5f* fontSize, viewport.ActualWidth, hsize), "", (int)wt.NONE, "");

            string missionTypeText = LanguageResources.GetString(LanguageKey.MissionType) + ": " + LanguageResources.GetString(CurrentLevel.MissionType.ToString());
            float textWidth = ViewHelper.MeasureText(FontManager.CurrentFont, missionTypeText, fontSize) + fontSize;
           	
            OverlayContainer c =  missionTypeWindow.createStaticText(new Vector4(missionTypeWindow.w - textWidth, 0,  textWidth,  missionTypeWindow.h), missionTypeText, new ColourValue(0.3f, 0.3f, 0.3f));
        
           
            string filename = Level.GetMissionTypeTextureFile(CurrentLevel.MissionType);
            missionTypeWindow.createStaticImage(new Vector4(viewport.ActualWidth - hsize, fontSize, hsize, hsize), filename);
           
            missionTypeWindow.show();
        }
        
        
        private void StartLoading()
        {
            loading = true;
            lock (loadingLock)
            {
                try
                {     
           	
                    loadingStart = DateTime.Now;
                    
                    LogManager.Singleton.LogMessage("Initializing effects manager", LogMessageLevel.LML_CRITICAL);
                    ViewEffectsManager.Singleton.Init();

                  
                    LogManager.Singleton.LogMessage("About to load level view...", LogMessageLevel.LML_CRITICAL);
                    levelView = new LevelView(framework, this);
                
                    LogManager.Singleton.LogMessage("Preloading music (if streaming disabled)", LogMessageLevel.LML_CRITICAL);
                    SoundManager.Instance.PreloadRandomIngameMusic();

                    LogManager.Singleton.LogMessage("About to register level " + levelNo + " - " + LevelFile + " to view...", LogMessageLevel.LML_CRITICAL);
                    
                    levelView.OnRegisterLevel(currentLevel);
                    if (LevelView.IsNightScene)
                    {
                        MessageEntry.SetDefaultColours(new ColourValue(0.1f, 0.5f, 0.1f),
                                                       new ColourValue(0.1f, 0.3f, 0.1f));
                    }

                    LogManager.Singleton.LogMessage("About to register player plane", LogMessageLevel.LML_CRITICAL);
                    OnRegisterPlane(currentLevel.UserPlane);
                   
                    
                    LogManager.Singleton.LogMessage("Preloading meshes and textures", LogMessageLevel.LML_CRITICAL);
                    PlaneView upv = levelView.FindPlaneView(currentLevel.UserPlane);
                    ViewEffectsManager.Singleton.RegisterAdditionalPreloadedTextures(upv.PlaneEntity);


                    ViewEffectsManager.Singleton.PreloadGameResources();


                    LogManager.Singleton.LogMessage("About to register enemy planes", LogMessageLevel.LML_CRITICAL);
                    
                    
                    if (currentLevel.EnemyPlanes.Count > 0) //warunek dodany przez Emila
                    {
                        Plane p = currentLevel.EnemyPlanes[currentLevel.EnemyPlanes.Count - 1];
                        OnRegisterPlane(p);
                        PlaneView pv = levelView.FindPlaneView(p);
                        EffectsManager.Singleton.PreloadMesh(pv.GetMainMeshName());
                    }
                        

                    LogManager.Singleton.LogMessage("About to register storage planes", LogMessageLevel.LML_CRITICAL);
                    foreach (StoragePlane sp in currentLevel.StoragePlanes)
                    {
                        OnRegisterPlane(sp);
                    }               
                    
                    LogManager.Singleton.LogMessage("Finished loading level.", LogMessageLevel.LML_CRITICAL);
                   
                    UpdateHints(true);


					createAchivementsGui((uint)( fontSize* 0.6f));

                    
                    // missiontype gui                 
                    createMissionTypeGui((uint)( fontSize* 0.6f));
                    
                                       
                    _bulletTimeBar = new BulletTimeBar(fontSize, framework.Viewport, viewport.ActualWidth / 6.5f, viewport.ActualHeight / 75.0f);
                    _altitudeBar = new AltitudeBar(fontSize, framework.Viewport, viewport.ActualWidth / 6.5f, viewport.ActualHeight / 75.0f);
                    
                    if(currentLevel.MissionType == MissionType.Survival) {
                    	gameMessages.InstantBackground = true;
                    	
                    }
                    
                    if (ShowHintMessages && LevelNo == 1 && firstTakeOff)
                    {

                        MessageEntry message =
                              new CenteredMessageEntry(viewport, 5000, GetChangeAmmoMessage());
                      
                        gameMessages.AppendMessage(message);

                 		IconedMessageEntry message2 =
                            new IconedMessageEntry(new CenteredMessageEntry(viewport, GetHintMessage(), true, true), C_ENGINE_HINT_ICON);
                        message2.UseAutoDectetedIconDimesions(viewport);
                      //  message2.CenterIconOnScreen(viewport);
                    //    message2.IncreaseY(-message2.Y);
                     //   message2.IncreaseY(0.2f);
                        gameMessages.AppendMessage(message2);


                    }
                  //  OnLevelFinished();
        

                    loading = false;
                }
                catch (SEHException sex)
                {
                    // Check if it's an Ogre Exception
                    if (OgreException.IsThrown)
                    {
                        FrameWorkStaticHelper.ShowOgreException(sex);
                        throw;
                    }
                    else
                        throw;
                }
                catch (LevelCorruptedException exc)
                {
              
                    Game.getGame().Window.Destroy();
                    FrameWorkStaticHelper.ShowWofException(exc);
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, exc.Message + " " + exc.StackTrace);
                    Debug.WriteLine(exc.ToString());
                    Application.Exit();
                
                }
            }
        }

        

        private static String GetHintMessage()
        {
            return String.Format(LanguageResources.GetString(LanguageKey.PressEToTurnOnTheEngine), KeyMap.GetName(KeyMap.Instance.Engine));
        }

        private static String GetHintMessage2()
        {
            return String.Format(LanguageResources.GetString(LanguageKey.PressLeftUpToTakeOff),
                                                          KeyMap.GetName(KeyMap.Instance.Left),
                                                          KeyMap.GetName(KeyMap.Instance.Up));
        }
        private static String GetBadLandingHintMessage()
        {
            return "";
        }
        private static String GetLandingHintMessage()
        {
            return String.Format(LanguageResources.GetString(LanguageKey.PressUpToLand),
                                                          KeyMap.GetName(KeyMap.Instance.Up));
        }
        private static String GetChangeAmmoMessage()
        {
            return String.Format(LanguageResources.GetString(LanguageKey.PressXToChangeAmmo), KeyMap.GetName(KeyMap.Instance.AltFire));
        }


        /// <summary>
        /// Zwalnia screeny z pamiêci
        /// </summary>
        /// <returns>iloœæ screenóww</returns>
        public int FreeSplashScreens()
        {

            string baseName = "Tutorial";
            string lang = "_" + LanguageManager.ActualLanguageCode;
            int n = 0;
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
			displayed = true;
            LogManager.Singleton.LogMessage("About to load model...", LogMessageLevel.LML_CRITICAL);
            
            //controller = new DelayedControllerFacade(this);
            controller = this;
            
            if(levelNo.HasValue)
            {
                levelFile = XmlLevelParser.GetLevelFileName(levelNo.Value);
            }
            currentLevel = new Level(LevelFile, controller, lives, userPlaneType);
            if(!EngineConfig.IsEnhancedVersion && currentLevel.EnhancedOnly)
            {
                throw new LevelUnavailableException(levelFile);
            }


            string baseName;
            //Console.WriteLine("LOADING WAITING GUI");        

            baseName = "Tutorial";
            string lang = "_" + LanguageManager.ActualLanguageCode;
            int n = FreeSplashScreens();

           
            if (n >= 0)
            {
                n = (new Random().Next(0, n));
            }
            else
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL,
                                                "Error: No tutorial screens available for language:" +
                                                LanguageManager.ActualLanguageName);
            }
            showingLoadingAds = Mogre.Math.RangeRandom(0, 1) > (1 - C_LOADING_AD_PROBABILITY);


          

            string overlayAdScreenSplashName = "Wof/LoadingAdScreenSplash";
            string overlayAdScreenLogoName = "Wof/LoadingAdScreenLogo";
          
           
          
            String imageName = null;
            AdManager.AdStatus status;
            if (showingLoadingAds)
            {
                // pobierz i ustaw na bie¿ac¹
                 
                try
                {
                    status = AdManager.Singleton.GetAd(C_AD_LOADING_ZONE, 1.0f, out loadingAd);
                    if (status == AdManager.AdStatus.OK)
                    {
                        imageName = AdManager.Singleton.LoadAdTexture(loadingAd); // jesli sie nie uda bedzie null
                    }
                    else
                    {
                        loadingAd = null;
                    }
                    
                }
                catch (SEHException ex)
                {
                    loadingAd = null;
                }
                catch(Exception ex)
                {
                    loadingAd = null;
                }
               
            }
            
            // zlec zaladowanie reklamy ingame (przy przeladowaniu amunicji)
            if (!EngineConfig.IsEnhancedVersion)
            {
                if (EngineConfig.AdManagerDisabled)
                {
                    changingAmmoAdId = 110;
                }
                else
                {
                    AdManager.Singleton.GetAdAsync(C_AD_GAME_ZONE, 0.25f, out changingAmmoAdId);
                }
            }

            // zlec ladowanie reklam dynamicznych (3D)
            bool levelAd = this.levelInfo.GetLevelNo().HasValue && this.levelInfo.GetLevelNo().Value > 1;

            if (levelAd && !EngineConfig.IsEnhancedVersion && Mogre.Math.RangeRandom(0.0f,1.0f) > 0.6f)
            {
                BeginDynamicAdsDownload(LevelView.C_AD_DYNAMIC_ADS_COUNT);
            }
           


            // jesli nie ma byc reklamy lub jesli nie udalo sie zaladowac reklamy
            if(imageName == null)
            {
                showingLoadingAds = false;
                if(n >= 0)
                {
                    imageName = baseName + n + lang + ".jpg";
                    TextureManager.Singleton.Load(imageName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME).Load(false); // preload
                }

            }

            string overlayName = "Wof/Loading";
            string overlayLoadingScreenTextName = "Wof/LoadingScreenText";
            string overlayLoadingScreenMissionTypeName = "Wof/LoadingScreenMissionType";

            if(showingLoadingAds)
            {
                overlayName = "Wof/LoadingAd";
                overlayLoadingScreenTextName = "Wof/LoadingAdScreenText";
                overlayLoadingScreenMissionTypeName = "Wof/LoadingAdScreenMissionType";
            }

            loadingOverlay = OverlayManager.Singleton.GetByName(overlayName);
            MaterialPtr overlayMaterial = MaterialManager.Singleton.GetByName("SplashScreen");
            OverlayElement loadingText = OverlayManager.Singleton.GetOverlayElement(overlayLoadingScreenTextName);

            if (imageName != null)
            {
                overlayMaterial.Load();
                overlayMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(imageName);

                if (showingLoadingAds)
                {

                    TextureUnitState unit = overlayMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
                    OverlayElement adSurface = OverlayManager.Singleton.GetOverlayElement(overlayAdScreenSplashName);
                    const float adSurfaceWidth = 1.0f;
                    const float adSurfaceHeight = 0.95f;

                    adSurface.Width = adSurfaceWidth; adSurface.Height = adSurfaceHeight;

                    PointD scale = AdSizeUtils.ScaleAdToDisplay(unit.GetTextureDimensions(), new PointD(adSurface.Width * viewport.ActualWidth, adSurface.Height * viewport.ActualHeight), true);



                    adSurface.SetDimensions(0.5f * adSurface.Width * scale.X, 0.5f * adSurface.Height * scale.Y);
                    float xShift = (1 - adSurface.Width) / 2.0f;
                    float yShift = (1 - adSurface.Height) / 2.0f;
                    adSurface.SetPosition(xShift, yShift);

                    AdManager.Singleton.RegisterImpression(loadingAd);

                    OverlayManager.Singleton.GetOverlayElement(overlayAdScreenLogoName).Show();
                }


                overlayMaterial = null;
                string caption;
                if(levelNo == 0)
                {
                    caption = LevelInfo.GetCustomLevelName(LevelFile);
                }
                else
                {
                    caption = levelNo.ToString();
                }

                loadingText.Caption = LanguageResources.GetString(LanguageKey.Level) + ": " + caption + " " + LanguageResources.GetString(LanguageKey.MissionType) + ": " + LanguageResources.GetString(CurrentLevel.MissionType.ToString());
                loadingText.SetParameter("font_name", FontManager.CurrentFont);
                loadingText = null;
            }


            MaterialPtr missionTypeMaterial = MaterialManager.Singleton.GetByName("MissionType");
            missionTypeMaterial.Load();
            string texture = Level.GetMissionTypeTextureFile(CurrentLevel.MissionType);
            missionTypeMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(texture);

            OverlayManager.Singleton.GetOverlayElement(overlayLoadingScreenMissionTypeName).Show();
            
            loadingOverlay.Show();

          
            isFirstLoadingFrame = true;
            
      
            
        }

    
        public void CleanUp(Boolean justMenu)
        {
            SoundManager3D.Instance.UpdaterRunning = false;
            SoundManager.Instance.StopMusic();

            
            if(this.levelView != null)
            {
                levelView.Destroy();
                levelView = null;
            }

            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CleanUp");
            ViewEffectsManager.Singleton.Clear();
            FrameWorkStaticHelper.DestroyScenes(framework);
            

         //   controller.ClearJobs();
         //  controller = null;
            // test pamiêci
            
            
          /*  TextureManager.Singleton.UnloadAll();
            MaterialManager.Singleton.UnloadAll();
            MeshManager.Singleton.UnloadAll();
           */
           
           // CompositorManager.Singleton.RemoveAll();
           // CompositorManager.Singleton.UnloadAll();
            try
            { 	
            	_bulletTimeBar.Dispose();
            	_altitudeBar.Dispose();
            	
            
                if (mGui != null)
                {
               
                    mGui.killGUI();
                    mGui = null;
                }

                if(mGuiHint != null)
                {
                    mGuiHint.killGUI();
                    mGuiHint = null;
                }
                if (missionTypeGui != null)
                {
                    missionTypeGui.killGUI();
                    missionTypeGui = null;
                }                
				if(achievementsIcons!=null) {
	                foreach(AchievementIcon icon in achievementsIcons.Values) {
	            		icon.Dispose();
	            	}  
                }             
                if (achievementsGui != null)
                {
                    achievementsGui.killGUI();
                    achievementsGui = null;
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
            SoundManager3D.Instance.UpdaterRunning = true;
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CleanUpEnd");
        }

      
        public int GetHighscore()
        {
            if (hiscoreCache != -1) return hiscoreCache;
            HighscoreUtil util = new HighscoreUtil();
            hiscoreCache = util.FindHighHighscore();
            return hiscoreCache;
        }

        


        public void OnUpdateModel(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {
        	if(levelView == null || currentLevel == null) return; // moze sie zdarzyc kiedy level nie jest jeszcze zaladowany
          
          //  camera.Move(new Vector3(evt.timeSinceLastFrame * 5,0,0));
            if(nextFrameGotoNextLevel) return;

            // przeladuj efekty
            /*if (loadingOverlay != null)
            {
                EffectsManager.Singleton.UpdateTimeAndAnimateAll(evt.timeSinceLastFrame);
            }*/
            // wy³¹czone z uwagi na error z 'collection was modified'

        
            if (!loading && loadingOverlay == null)
            {
               // inputMouse.Capture();
                //inputKeyboard.Capture();
                //if(inputJoystick != null) inputJoystick.Capture();
                Vector2 joyVector = FrameWorkStaticHelper.GetJoystickVector(inputJoystick);

               

                // jezeli uzytkownik bedzie w stanie pauzy, to nie
                // moze miec mozliwosci ruszac samolotem
                if (!isGamePaused)
                {
                    if (!changingAmmo)
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
                            Vector2? inputVector = null;
                            if(!joyVector.IsZeroLength)
                            {
                                inputVector = joyVector;
                            }
                            currentLevel.UpdateInputVector(inputVector);

                            // nie chcemy, zeby uzytkownik ruszyl sie jednoczesnie
                            // w lewo i w prawo, dlatego pierwszenstwo ma
                            // ruch w lewo.
                            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Left) || joyVector.x < 0)
                            {
                                currentLevel.OnSteerLeft();
                            }
                            else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Right) || joyVector.x > 0)
                            {
                                currentLevel.OnSteerRight();
                            }

                            // góra / dó³
                            if ((!EngineConfig.InverseKeys && !EngineConfig.SpinKeys) ||
                                (EngineConfig.InverseKeys && EngineConfig.SpinKeys))
                            {
                                // podobnie, uzytkownik nie powinien miec mozliwosci
                                // ruszyc sie w tym samym momencie do gory i w dol.
                                // Pierwszenstwo posiada zatem gora
                                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) || joyVector.y > 0)
                                {
                                    currentLevel.OnSteerUp();
                                }
                                else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down) || joyVector.y < 0)
                                {
                                    currentLevel.OnSteerDown();
                                }
                                else if (EngineConfig.SpinKeys && EngineConfig.UseAlternativeSpinControl) EngineConfig.SpinKeys = false;
                            }
                            else
                            {
                                // klawisze zamienione miejscami.
                                // priorytet ma ruch do gory
                                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down) || joyVector.y < 0)
                                {
                                    currentLevel.OnSteerUp();
                                }
                                else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) || joyVector.y > 0)
                                {
                                    currentLevel.OnSteerDown();
                                }
                                else if (EngineConfig.SpinKeys && EngineConfig.UseAlternativeSpinControl) EngineConfig.SpinKeys = false;
                            }


                            // uzytkownik moze wlaczyc i wylaczyc silnik w kazdym 
                            // momencie lotu w polaczeniu z kazda kombinacja klawiszy
                            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Engine) ||
                                FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEngine))
                            {
                                currentLevel.OnToggleEngineOn();
                            }

                            // uzytkownik moze starac sie schowac lub otworzyc
                            // podwozie w kazdym momencie lotu, o mozliwosc zajscia
                            //zdarzenia decyduje model
                            if (inputKeyboard.IsKeyDown(KeyMap.Instance.Gear) ||
                                FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickGear))
                            {
                                currentLevel.OnToggleGear();
                            }

                          
                            // strzal z rakiety
                            if (inputKeyboard.IsKeyDown(KeyMap.Instance.AltFire) ||
                                FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickRocket))
                            {
                                currentLevel.OnFireSecondaryWeapon();
                            }
                            
                            isStillFireGun = false;
                            // strzal z dzialka
                            if (inputKeyboard.IsKeyDown(KeyMap.Instance.GunFire) ||
                                FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickGun))
                            {
                            	//this.levelView.FindPlaneView(this.currentLevel.UserPlane).PlayPlanePass();

                            	if(currentLevel.OnFireGun())
                            	{
                            		isStillFireGun = true;
                            	}
                                
                            }

                            // obrót z 'pleców na brzuch' samolotu
                            // tymczasowo wy³¹czone - problematyczny obrót samolotu w modelu

                            if (KeyMap.Instance.Spin is MOIS.Keyboard.Modifier)
                            {
                                if (inputKeyboard.IsModifierDown((MOIS.Keyboard.Modifier) KeyMap.Instance.Spin))
                                {
                                    currentLevel.OnSpinPressed();
                                }
                            }
                            else
                            {
                                if (inputKeyboard.IsKeyDown((KeyCode) KeyMap.Instance.Spin))
                                {
                                    currentLevel.OnSpinPressed();
                                }

                            }

                            // bullet time
                            bool backspaceKeyDown = inputKeyboard.IsKeyDown(KeyMap.Instance.BulletTimeEffect) ||
                                                    FrameWorkStaticHelper.GetJoystickButton(inputJoystick,
                                                                                KeyMap.Instance.
                                                                                    JoystickBulletiTimeEffect);

                            if (backspaceKeyDown)
                                ModelEffectsManager.Instance.StartConsumptionEffect(EffectType.BulletTimeEffect);
                            else ModelEffectsManager.Instance.StartLoadEffect(EffectType.BulletTimeEffect);
                            if (backspaceKeyDown &&
                                ModelEffectsManager.Instance.GetEffectLevel(EffectType.BulletTimeEffect) > 0.0f)
                            {
                                if (EngineConfig.CurrentGameSpeedMultiplier ==
                                    EngineConfig.GameSpeedMultiplierNormal)
                                {
                                    this.framework.SetCompositorEnabled(FrameWorkForm.CompositorTypes.MOTION_BLUR, true);
                                }
                                if (gameMessages.IsMessageQueueEmpty())
                                    gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.BulletTime));
                                EngineConfig.CurrentGameSpeedMultiplier = EngineConfig.GameSpeedMultiplierSlow;
                            }
                            else
                            {
                                if (EngineConfig.CurrentGameSpeedMultiplier == EngineConfig.GameSpeedMultiplierSlow)
                                {
                                    this.framework.SetCompositorEnabled(FrameWorkForm.CompositorTypes.MOTION_BLUR, false);
                                }
                                EngineConfig.CurrentGameSpeedMultiplier = EngineConfig.GameSpeedMultiplierNormal;
                            }
                        }

                      

                    }



                    if (!isGamePaused)
                    {
                        if (!changingAmmo)
                        {
                            // odswiezam model przed wyrenderowaniem klatki
                            // czas od ostatniej klatki przekazywany jest w sekundach
                            // natomiast model potrzebuje wartosci w milisekundach
                            // dlatego mnoze przez 1000 i zaokraglam     
                            int timeInterval = (int) Math.Round(evt.timeSinceLastFrame*1000);
                            if (isFirstFrame)
                            {
                                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "!!! Game started !!!");
                            	if(currentLevel.UserPlane.IsEngineWorking) 
				                {
                                    OnTurnOnEngine(false, currentLevel.UserPlane);				                	
				                }
            	
                                // w pierwszej klatce chcemy wyzerowaæ czas
                                timeInterval = 0;
                                isFirstFrame = false;
                            }


                            if (currentLevel.MissionType == MissionType.Survival)
                            {
                                gameMessages.ClearMessages();
                                IconedMessageEntry message = new IconedMessageEntry(0, 0, String.Format(LanguageResources.GetString(LanguageKey.SurvivalTime) + " {0:f}s.", survivalTime));
                                message.Icon = "survival.png";

                                gameMessages.AppendMessage(message);

                                UpdateSurvivalTime(evt);
                            }

                            


                            currentLevel.Update(timeInterval);
                            _bulletTimeBar.Update(timeInterval);
							_altitudeBar.Update(timeInterval);
                            
                            if (!readyForLevelEnd)
                            {
                                // sprawdzam, czy to juz ten moment
                                currentLevel.OnCheckVictoryConditions();
                            }
							
                            
                        }
                    }
                }


             
               // ControlChangeAmmunition();
                ControlFuelState();
                ControlOilState();

                if (!isGamePaused && !changingAmmo)
                {
                    ControlPlaneEngineSounds();
                    
                }
                
            }
            else
            {
            	
            }
        }

        protected void UpdateSurvivalTime(FrameEvent evt)
        {
            if (CountSurvivalTime)
            {
                switch (EngineConfig.Difficulty)
                {
                    case EngineConfig.DifficultyLevel.Easy:
                        survivalTime += evt.timeSinceLastFrame;
                        break;

                    case EngineConfig.DifficultyLevel.Medium:
                        survivalTime += 1.2f* evt.timeSinceLastFrame;
                        break;

                    case EngineConfig.DifficultyLevel.Hard:
                        survivalTime += 1.4f * evt.timeSinceLastFrame;
                        break;
                }
                                   
            }
        }


        private void UpdateMenusGui(Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {

            if (mGui != null)
            {
                MouseState_NativePtr mouseState = inputMouse.MouseState;

                mousePosX = (uint)(mousePosX + mouseState.X.rel * 2);
                mousePosY = (uint)(mousePosY + mouseState.Y.rel * 2);

                if ((int)mousePosX + mouseState.X.rel * 2 < 0)
                {
                    mousePosX = 0;
                }
                if ((int)mousePosY + mouseState.Y.rel * 2 < 0)
                {
                    mousePosY = 0;
                }

                if (mousePosX > framework.OverlayViewport.ActualWidth)
                {
                    mousePosX = (uint)framework.OverlayViewport.ActualWidth;
                }
                if (mousePosY > framework.OverlayViewport.ActualHeight)
                {
                    mousePosY = (uint)framework.OverlayViewport.ActualHeight;
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
                    int id = mGui.injectMouse(mousePosX, mousePosY, true);


                    wasLeftMousePressed = false;
                }


                Vector2 joyVector = FrameWorkStaticHelper.GetJoystickVector(inputJoystick);

                // przyciski - pauza
                if (isInPauseMenu)
                {

                    if (Button.CanChangeSelectedButton())
                    {
                        if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) || joyVector.y > 0)
                        {
                            resumeButton.activate(true);
                            exitButton.activate(false);
                            Button.ResetButtonTimer();
                        }
                        else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down) || joyVector.y < 0)
                        {
                            resumeButton.activate(false);
                            exitButton.activate(true);
                            Button.ResetButtonTimer();
                        }
                    }

                    if (inputKeyboard.IsKeyDown(KeyMap.Instance.Enter) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEnter))
                    {
                        if (exitButton.activated) Button.TryToPressButton(exitButton, 0.1f);
                        else
                        {
                            if (resumeButton.activated) Button.TryToPressButton(resumeButton, 0.4f);
                        }
                    }
                }



                // przyciski - game over
                if (isInGameOverMenu && (inputKeyboard.IsKeyDown(KeyMap.Instance.Enter) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEnter)))
                {
                    Button.TryToPressButton(gameOverButton);

                }

                // przyciski - next level
                if (isInNextLevelMenu && (Button.CanChangeSelectedButton(1.5f))) // mozna nacisnac guzik 
                { 
                    bool pressed = false;

                    if (nextLevelMenuSelectedIndex != nextLevelMenuSelectedIndexCount)
                    {
                        if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) || joyVector.y > 0)
                        {
                            nextLevelMenuSelectedIndex -= 1;
                            pressed = true;
                        }

                        if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down) || joyVector.y < 0)
                        {
                            nextLevelMenuSelectedIndex += 1;
                            pressed = true;
                        }

                        if (pressed) Button.ResetButtonTimer();


                        if (nextLevelMenuSelectedIndex >= nextLevelMenuSelectedIndexCount)
                        {
                            nextLevelMenuSelectedIndex = nextLevelMenuSelectedIndexCount - 1;
                        }

                        if (nextLevelMenuSelectedIndex < 0)
                        {
                            nextLevelMenuSelectedIndex = 0;
                        }

                    }
                    else
                    {
                        nextLevelMenuSelectedIndex = 0; // domyslnie
                        pressed = true;
                    }
  

                    if (pressed)
                    {
                        switch (nextLevelMenuSelectedIndex)
                        {
                            case 0:
                                resumeFinishedLevelButton.activate(true);
                                rearmButton.activate(false);
                                nextLevelButton.activate(false);
                                break;

                            case 1:
                                resumeFinishedLevelButton.activate(false);
                                rearmButton.activate(true);
                                nextLevelButton.activate(false);
                                break;

                            case 2:
                                resumeFinishedLevelButton.activate(false);
                                rearmButton.activate(false);
                                nextLevelButton.activate(true);
                                break;
                        }
                    }

                    if (inputKeyboard.IsKeyDown(KeyMap.Instance.Enter) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEnter))
                    {
                        Button buttonToPress = null;
                        switch (nextLevelMenuSelectedIndex)
                        {
                            case 0:
                                buttonToPress = resumeFinishedLevelButton;
                                break;

                            case 1:
                                buttonToPress = rearmButton;
                                break;

                            case 2:
                                buttonToPress = nextLevelButton;
                                break;

                        }
                       
                        if (buttonToPress != null)
                        {
                            onButtonPress(buttonToPress);
                            ammoSelectedIndex = ammoSelectedIndexCount;
                            Button.ResetButtonTimer();
                        }

                    }
            
                    if (inputKeyboard.IsKeyDown(KeyMap.Instance.Escape) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEscape))
                    {
                        onButtonPress(resumeFinishedLevelButton);
                        Button.ResetButtonTimer();
                        nextLevelMenuSelectedIndex = nextLevelMenuSelectedIndexCount;
                    } 

                }







                // zmiana amunicji za pomoc¹ klawiatury                       
                if (changingAmmo && (Button.CanChangeSelectedButton(1.5f))) // mozna nacisnac guzik 
                {
                    bool pressed = false;
                    if (ammoSelectedIndex != ammoSelectedIndexCount)
                    {
                        if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up) || joyVector.y > 0)
                        {
                            ammoSelectedIndex -= 1;
                            pressed = true;
                        }

                        if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down) || joyVector.y < 0)
                        {
                            ammoSelectedIndex += 1;
                            pressed = true;
                        }

                        if (inputKeyboard.IsKeyDown(KeyCode.KC_B))
                        {
                            ammoSelectedIndex = 0;
                            pressed = true;
                        }

                        if (inputKeyboard.IsKeyDown(KeyCode.KC_R))
                        {
                            ammoSelectedIndex = 1;
                            pressed = true;
                        }

                        if (inputKeyboard.IsKeyDown(KeyCode.KC_T))
                        {
                            ammoSelectedIndex = 2;
                            pressed = true;
                        }

                        if (pressed) Button.ResetButtonTimer();


                        if (ammoSelectedIndex >= ammoSelectedIndexCount)
                        {
                            ammoSelectedIndex = ammoSelectedIndexCount - 1;
                        }

                        if (ammoSelectedIndex < 0)
                        {
                            ammoSelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ammoSelectedIndex = 0; // domyslnie bomby
                        pressed = true;
                    }


                    if (pressed)
                    {
                        switch (ammoSelectedIndex)
                        {
                            case 0:
                                bombsButton.activate(true);
                                rocketsButton.activate(false);
                                torpedoesButton.activate(false);
                                break;

                            case 1:
                                bombsButton.activate(false);
                                rocketsButton.activate(true);
                                torpedoesButton.activate(false);
                                break;

                            case 2:
                                bombsButton.activate(false);
                                rocketsButton.activate(false);
                                torpedoesButton.activate(true);
                                break;
                        }
                    }


                    if (inputKeyboard.IsKeyDown(KeyMap.Instance.Enter) || inputKeyboard.IsKeyDown(KeyCode.KC_B) || inputKeyboard.IsKeyDown(KeyCode.KC_R) || inputKeyboard.IsKeyDown(KeyCode.KC_T) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEnter))
                    {

                        Button buttonToPress = null;
                        WeaponType type = WeaponType.None;
                        switch (ammoSelectedIndex)
                        {
                            case 0:
                                // bomby
                                buttonToPress = bombsButton;
                                type = WeaponType.Bomb;
                               
                                break;

                            case 1:
                                // rakiety
                                buttonToPress = rocketsButton;
                                type = WeaponType.Rocket;
                                break;

                            case 2:
                                // torpedy
                                buttonToPress = torpedoesButton;
                                type = WeaponType.Torpedo;
                                break;

                        }
                        if(CanClearRestoreAmmunitionScreen)
                        {
                            if(buttonToPress != null)
                            {
                                onButtonPress(buttonToPress);
                                ammoSelectedIndex = ammoSelectedIndexCount;

                            }
                        }
                        else
                        {
                            changeAmmoToWhenCanClearRestoreAmmunitionScreen = type;
                        }
                        
                    }

                    if (inputKeyboard.IsKeyDown(KeyMap.Instance.Escape) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEscape))
                    {
                        if (CanClearRestoreAmmunitionScreen)
                        {
                            ClearRestoreAmmunitionScreen();
                            Button.ResetButtonTimer();
                            ammoSelectedIndex = ammoSelectedIndexCount;
                        }
                        else
                        {
                            changeAmmoToWhenCanClearRestoreAmmunitionScreen = WeaponType.None;
                        }
                    } 
                   
                   
                }

                // mouseState = null;
            }
        }


        public void OnHandleViewUpdateEnded(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            if(levelView != null)
            {
                levelView.OnFrameEnded(evt);
            }

        }

        public static void ShuffleArray<T>(T[] list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Length;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
      
        private void BeginDynamicAdsDownload(int count)
        {
            int failed = 0;
            for(int i = 0; i < count ; i++)
            {
                int temp;
                AdManager.AdStatus status = AdManager.Singleton.GetAdAsync(GameScreen.C_AD_GAME_ZONE, 1.0f, out temp,
                                                                           backgroundAdDownloadedAsyncCallback);
                if(status == AdManager.AdStatus.DOWNLOAD_FAILED || status == AdManager.AdStatus.ADS_DISABLED)
                {
                    failed++;
                }
            }

            int max = count < DEFAULT_AD_IMAGE_NAMES.Length ? count : DEFAULT_AD_IMAGE_NAMES.Length;
            SHA1_Hash.InitHashOfImage();

            ShuffleArray(DEFAULT_AD_IMAGE_NAMES);

            for (int i = 0; i < max; i++)
            {
                if (!SHA1_Hash.ValidateImage(DEFAULT_AD_IMAGE_NAMES[i]))
                {
                    throw new Exception("Image " + DEFAULT_AD_IMAGE_NAMES[i] + " has been tampered with!");
                }

                AdManager.Ad ad = new AdManager.Ad(-i - 100, DEFAULT_AD_IMAGE_NAMES[i], false);
                AdManager.Singleton.AdDownloaded(ad);
                backgroundAdDownloadedAsyncCallback(ad);
            }
        }

    
     
        /// <summary>
        /// Callback wywolywany kiedy zakonczone jest sciagniecie reklamy dynamicznej
        /// </summary>
        /// <param name="ad"></param>
        private void backgroundAdDownloadedAsyncCallback(AdManager.Ad ad)
        {
            lock (backgroundAdsToShow)
            {
                backgroundAdsToShow.Enqueue(ad);
            }
            
        }

        /// <summary>
        /// Rejestruje czekajace reklamy dynamiczne w warstwie view
        /// </summary>
        private void RegisterDynamicAds()
        {
            lock (backgroundAdsToShow)
            {
                while (backgroundAdsToShow.Count > 0)
                {
                    levelView.OnRegisterBackgroundDynamicAd(backgroundAdsToShow.Dequeue());
                }
            }
        }
      

        public void OnHandleViewUpdate(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            if (levelView == null && !isFirstLoadingFrame)
            {
               // Thread.Sleep(10000);
                StartLoading();
            }

            AdManager.Singleton.Work(camera);
            

            if(isFirstLoadingFrame)
            {
                isFirstLoadingFrame = false;
                // preloader tekstur
                if (EngineConfig.UseHardwareTexturePreloader)
                {
                    MaterialPtr preloadingMaterial = ViewHelper.BuildPreloaderMaterial(EngineConfig.HardwareTexturePreloaderTextureLimit);
                    if (preloadingMaterial != null)
                    {
                        preloadingOverlay = OverlayManager.Singleton.GetByName("Wof/Preloader");
                        OverlayElement preloaderScreen = OverlayManager.Singleton.GetOverlayElement("Wof/PreloaderScreen");
                        preloaderScreen.MaterialName = preloadingMaterial.Name;
                        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Presenting hardware preloader overlay.");
                        preloaderScreen.Show();
                        preloadingOverlay.Show();
                      
                        
                    }
                }
            } else
            {
            	
            }

            try
            {

              
                lock (loadingLock)
                {
                    if (nextFrameGotoNextLevel)
                    {
                        isGamePaused = true;
                        nextFrameGotoNextLevel = false;
                        SoundManager.Instance.HaltEngineSound(currentLevel.UserPlane);
                        SoundManager.Instance.HaltGunFireSound();
                        SoundManager.Instance.HaltWaterBubblesSound();
                        levelView.OnStopPlayingEnemyPlaneEngineSounds();
                        SoundManager.Instance.HaltOceanSound();
                        increaseScore(this.lives * C_LIFE_LEFT_SCORE);

                        // synchronizuj z levelview
                       
                        levelView.Destroy();
                        levelView = null;
                        if (mGui != null)
                        {
                            mGui.killGUI();
                            mGui = null;
                        }
                        if (mGuiHint != null)
                        {
                            mGuiHint.killGUI();
                            mGuiHint = null;
                        }
                        if (missionTypeGui != null)
                        {
                            missionTypeGui.killGUI();
                            missionTypeGui = null;
                        }
						if(achievementsIcons!=null) {
			                foreach(AchievementIcon icon in achievementsIcons.Values) {
			            		icon.Dispose();
			            	}  
		                }                        
                        if (achievementsGui != null)
		                {
		                    achievementsGui.killGUI();
		                    achievementsGui = null;
		                }
		                        
                        AdManager.Singleton.ClearDynamicAds();


                        if(levelInfo.IsCustom)
                        { 
                        	// customowy poziom - koniec
							LoadGameUtil.NewLevelCompleted(levelInfo, CompletedAchievements);
                            gameEventListener.GotoStartScreen();
                        }
                        else
                        {
                            gameEventListener.GotoNextLevel();
                        }
                        
                        
                    }



                    if (!loading && loadingOverlay == null)
                    {

                        inputMouse.Capture();
                        inputKeyboard.Capture();
                        if (inputJoystick != null) inputJoystick.Capture();
                        Vector2 joyVector = FrameWorkStaticHelper.GetJoystickVector(inputJoystick);

                        if(changingAmmo)
                        {
                            changingAmmoTime += evt.timeSinceLastFrame;
                            // jesli zmieniamy amunicje i screen moze zostac zamkniety ORAZ user juz w cos klikn¹³ nalezy teraz akcje ktora wybral user i wreszcie zamknac screen zmiany amunicji
                            DelayedChangeAmmoAndCloseScreen();
                        }

                        UpdateMenusGui(inputMouse, inputKeyboard, inputJoystick);

                       
                        if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Escape) || FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickEscape)) && Button.CanChangeSelectedButton(3.5f) &&
                           !changingAmmo)
                        {
                            if (!isGamePaused)
                            {
                                DisplayPauseScreen();
                                SoundManager.Instance.HaltEngineSound(currentLevel.UserPlane);
                                SoundManager.Instance.HaltWaterBubblesSound();
                                SoundManager.Instance.HaltOceanSound();
                            }
                            else
                            {
                                ClearPauseScreen();
                                SoundManager.Instance.LoopOceanSound();
                                if (mayPlaySound)
                                {
                                    SoundManager.Instance.LoopEngineSound(currentLevel.UserPlane);
                                }
                            }
                            Button.ResetButtonTimer();
                        }

                       
                        if (levelView != null && levelView.IsHangaringFinished())
                        {
                            levelView.OnHangaringFinished();
                        }

                      
                        if (!isGamePaused)
                        {
                            if (!changingAmmo)
                            {

                                // przyczep / odczep kamere
                                if (inputKeyboard.IsKeyDown(KeyMap.Instance.ResetCamera) && 
                                    Button.CanChangeSelectedButton(3.0f))
                                {
                                    if (EngineConfig.FreeLook)
                                    {
                                        EngineConfig.ManualCamera = !EngineConfig.ManualCamera;
                                    }
                                    
                                    if (!EngineConfig.ManualCamera) 
                                    {
                                    	levelView.OnResetCamera();
                                    }
                                    Button.ResetButtonTimer();
                                }



                                if (EngineConfig.DebugInfo && inputKeyboard.IsKeyDown(KeyMap.Instance.PausePlane) &&
                                   Button.CanChangeSelectedButton(3.0f))
                                {

                                    currentLevel.UserPlane.PlanePaused = !currentLevel.UserPlane.PlanePaused;
                                  	currentLevel.EnemyPlanes.ForEach(delegate(Plane ep) { ep.PlanePaused = !ep.PlanePaused; });
                                  
                                    Button.ResetButtonTimer();
                                }


                                // przeladuj hydrax jesli nacisnieto H
                                if (EngineConfig.DebugInfo && inputKeyboard.IsKeyDown(KeyCode.KC_H) && Button.CanChangeSelectedButton(3.0f))
                                {
                                    levelView.OnReCreateHydrax();
                                    Button.ResetButtonTimer();
                                }


                                // zmiana kamery
                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam1)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(0);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam2)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(1);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam3)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(2);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam4)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(3);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam5)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(4);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Cam6)) && Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera(5);
                                }

                                if ((inputKeyboard.IsKeyDown(KeyMap.Instance.Camera) ||
                                     FrameWorkStaticHelper.GetJoystickButton(inputJoystick, KeyMap.Instance.JoystickCamera)) &&
                                    Button.CanChangeSelectedButton(2.5f))
                                {
                                    SwitchCamera();
                                }

                                // screenshots
                                if (inputKeyboard.IsKeyDown(KeyCode.KC_SYSRQ) && Button.CanChangeSelectedButton())
                                {
                                    string[] temp = Directory.GetFiles(Environment.CurrentDirectory, "screenshot*.jpg");
                                    string fileName = string.Format("screenshot{0}.jpg", temp.Length + 1);
                                    framework.TakeScreenshot(fileName);
                                    gameMessages.AppendMessage(String.Format("{0} '{1}'",
                                                                             LanguageResources.GetString(
                                                                                 LanguageKey.ScreenshotWrittenTo),
                                                                             fileName));
                                    Button.ResetButtonTimer();
                                }

                                if (levelView.CurrentCameraHolderIndex == 0)
                                {
                                    framework.HandleCameraInput(inputKeyboard, inputMouse, inputJoystick, evt,
                                                                framework.Camera,
                                                                framework.MinimapCamera, currentLevel.UserPlane);
                                }
                                else
                                {
                                    framework.HandleCameraInput(inputKeyboard, inputMouse, inputJoystick, evt, null,
                                                                framework.MinimapCamera, currentLevel.UserPlane);
                                }

                            }

                            RegisterDynamicAds();
                            levelView.OnFrameStarted(evt);


                            if (SoundManager.Instance.ShouldLoadNextMusic)
                            {
                                
                                SoundManager.Instance.ShouldLoadNextMusic = false;
                                SoundManager.Instance.PreloadRandomIngameMusic();
                                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Starting next track: " + SoundManager.Instance.LastRandomMusicTrackNo);
                                SoundManager.Instance.PlayRandomIngameMusic(EngineConfig.MusicVolume);
                            }
                            //    controller.DoJobs();

                            // odswiez raz na jakis czas
                            // TODO: timer
                            if (!changingAmmo && Mogre.Math.RangeRandom(0, 1) > 0.8f)
                            {
                                UpdateHints(false);
                            }
                        }
                        if (indicatorControl.WasDisplayed == false)
                        {
                            indicatorControl.DisplayIndicator();
                        }
                        else
                        {
                            indicatorControl.UpdateGUI(evt.timeSinceLastFrame);
                            gameMessages.UpdateControl(evt.timeSinceLastFrame);
                        }

                        ControlGunFireSound();

                    }
                    else if (loading == false && levelView != null)
                    {
                        isFirstFrame = true;
                        TimeSpan diff = DateTime.Now.Subtract(loadingStart);
                        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Level loaded in "+ (int)diff.TotalMilliseconds + "[ms]");



                        // reklama w czasie gry
                        if (changingAmmoAd == null && !changingAmmoAdTried)
                        {
                            changingAmmoAdTried = true;
                            // pobierz i ustaw na bie¿ac¹
                            if(changingAmmoAdId > 0)
                            {
                                AdManager.AdStatus status;
                                if(EngineConfig.AdManagerDisabled)
                                {
                                    changingAmmoAd = new AdManager.Ad(100, getRandomDefaultIngameAdImageName(), false);
                                    AdManager.Singleton.AdDownloaded(changingAmmoAd);
                                    status = AdManager.AdStatus.OK;
                                }else
                                {
                                    status = AdManager.Singleton.GatherAsyncResult(changingAmmoAdId, AdManager.C_AD_DOWNLOAD_TIMEOUT, out changingAmmoAd);
                                }
                            
                                
	                           // status = AdManager.Singleton.GetAd(C_AD_GAME_ZONE, 0.25f, out changingAmmoAd);
	                            if (status == AdManager.AdStatus.OK)
	                            {
	                                if (AdManager.Singleton.LoadAdTexture(changingAmmoAd) == null)
	                                {
	                                    changingAmmoAd = null;
	                                    //levelView.OnRegisterAd(AdManager.Singleton.CurrentAd);
	                                }
	                                
	                              
	
	                            }
                            }
                            
                        }




                        if (EngineConfig.DebugStart || (!showingLoadingAds || diff.TotalMilliseconds > EngineConfig.C_LOADING_DELAY_AD))
                        {
                            levelView.BuildCameraHolders();
                            if (!EngineConfig.FreeLook)
                            {
                                levelView.OnResetCamera();
                            }
                            
                            //mGui = new GUI(FontManager.CurrentFont, fontSize);
            				//mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");

                            if (EngineConfig.MinimapNoseCamera)
                            {
                                framework.MinimapViewport.Camera = framework.MinimapNoseCamera;
                                levelView.FindPlaneView(currentLevel.UserPlane).InnerNode.AttachObject(
                                    framework.MinimapNoseCamera);
                            }

                            //         levelView.InitOceanSurface();

                            levelView.SetVisible(true);

                            if (EngineConfig.UseHardwareTexturePreloader && preloadingOverlay != null)
                            {
                                preloadingOverlay.Hide();
                                preloadingOverlay.Dispose();
                                preloadingOverlay = null;
                            }
                            if(showingLoadingAds)
                            {
                                AdManager.Singleton.CloseAd(loadingAd);
                            }

                            loadingOverlay.Hide();
                            loadingOverlay.Dispose();
                            loadingOverlay = null;

                            FreeSplashScreens();
                            SoundManager3D.Instance.UpdaterRunning = true;
                            SoundManager.Instance.LoopOceanSound();
                                        
                        }
                    }
                }
            }
            finally
            {
               

            }

        }

     


        private void SwitchCamera()
        {
            SwitchCamera(levelView.CurrentCameraHolderIndex + 1);
        }

        private void SwitchCamera(int index)
        {
            if (gameMessages.IsMessageQueueEmpty())
            {
                gameMessages.AppendMessage(String.Format(@"{0}...",
                                                         LanguageResources.GetString(
                                                             LanguageKey.CameraChanged)));
            }
            levelView.OnChangeCamera(index);
            Button.ResetButtonTimer();
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

       /* public void ControlChangeAmmunition()
        {
            if (currentLevel.UserPlane.LocationState == LocationState.AircraftCarrier)
            {
                if (currentLevel.UserPlane.CanChangeAmmunition)
                {
                    if (!prevCouldChangeAmmo)
                    {
                        gameMessages.AppendMessage(GetChangeAmmoMessage());
                            // GameMessages.C_PRESS_X);
                        prevCouldChangeAmmo = true;
                    }
                    return;
                }
            }
            prevCouldChangeAmmo = false;
        }
        */
        public void ControlFuelState()
        {
            if (currentLevel.UserPlane.Petrol < 10)
            {
                if (!prevFuelTooLow)
                {
                    string message = String.Format("{0}!", LanguageResources.GetString(LanguageKey.LowFuelWarning)) + " " +  String.Format(LanguageResources.GetString(LanguageKey.LandOnTheCarrierAndPressXToRefuel), KeyMap.GetName(KeyMap.Instance.AltFire));
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
                    string message = String.Format("{0}!", LanguageResources.GetString(LanguageKey.LowOilWarning)) + " " + String.Format(LanguageResources.GetString(LanguageKey.LandOnTheCarrierAndPressXToRepair), KeyMap.GetName(KeyMap.Instance.AltFire));
                    gameMessages.AppendMessage(message);
                    prevOilTooLow = true;
                }
                return;
            }
            prevOilTooLow = false;
        }

        private void ControlPlaneEngineSounds()
        {
            float distance;

            if(currentLevel.UserPlane != null)
            {
                SoundManager.Instance.SetEngineFrequency(currentLevel.UserPlane);  
            }
           

            foreach (EnemyPlaneBase ep in currentLevel.EnemyPlanes)
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

        
        
        #region BetaGUIListener Members

        public void onButtonPress(BetaGUI.Button referer)
        {
        	
        	//volumes    
        
            string styleOn = "bgui.selected.button";
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Button : '" + referer.text + "' clicked");

        	foreach(KeyValuePair<Button,uint> pair in soundButtonIds)
        	{        
        		
        		if(pair.Key.Equals(referer))
        		{
                    // change sound volume
                    SoundManager3D.SetSoundVolume(pair.Value);
                    SoundManager.Instance.PlayBombSound();
                    ClearPauseScreen();
        			DisplayPauseScreen();
        			return;
        		} 
        	}
        	
        	foreach(KeyValuePair<Button,uint> pair in musicButtonIds)
        	{
        		if(pair.Key.Equals(referer))
        		{	        			
        			// change music volume
                    referer.mO.MaterialName = styleOn;
        			SoundManager3D.Instance.SetMusicVolume(pair.Value);
                    ClearPauseScreen();
                    DisplayPauseScreen();
        			
        		
        			return;
        		}
        	}
        	
        	// other buttons
        	
            if (referer == resumeButton)
            {
                ClearPauseScreen();
                SoundManager.Instance.LoopOceanSound();
                if (mayPlaySound)
                {
                    SoundManager.Instance.LoopEngineSound(currentLevel.UserPlane);
                }
            }
            if (referer == resumeFinishedLevelButton)
            {
                levelView.OnChangeCamera(0);
                ClearNextLevelScreen();
            }
            if (referer == rearmButton)
            {
                ClearNextLevelScreen();
                levelView.OnChangeCamera(0);
                OnChangeAmmunition();
            }
            

            if (referer == exitButton)
            {
                if (mGui != null)
                {
                    mGui.killGUI();
                    mGui = null;
                }
                if (mGuiHint != null)
                {
                    mGuiHint.killGUI();
                    mGuiHint = null;
                }
                if (missionTypeGui != null)
                {
                    missionTypeGui.killGUI();
                    missionTypeGui = null;
                }
                if(achievementsIcons!=null) {
	                foreach(AchievementIcon icon in achievementsIcons.Values) {
	            		icon.Dispose();
	            	}  
                }
                if (achievementsGui != null)
                {
                    achievementsGui.killGUI();
                    achievementsGui = null;
                }
                
                if (levelView != null)
                {
                    levelView.Destroy();
                    levelView = null;
                }
                
                
                
                gameEventListener.GotoStartScreen();
            }
            if (referer == gameOverButton)
            {
                if (mGui != null)
                {
                    mGui.killGUI();
                    mGui = null;
                }
                if (mGuiHint != null)
                {
                    mGuiHint.killGUI();
                    mGuiHint = null;
                }
                if (missionTypeGui != null)
                {
                    missionTypeGui.killGUI();
                    missionTypeGui = null;
                }
                if(achievementsIcons!=null) {
	                foreach(AchievementIcon icon in achievementsIcons.Values) {
	            		icon.Dispose();
	            	}  
                }
                if (achievementsGui != null)
                {
                    achievementsGui.killGUI();
                    achievementsGui = null;
                }
                
                
                if (levelView != null)
                {
                    levelView.Destroy();
                    levelView = null;
                }

                HighscoreUtil util = new HighscoreUtil();
                int leastScore = util.FindLeastHighscore();
                float maxSurvivalTime = util.GetSurvivalTime();
                
                if(this.CurrentLevel.MissionType == MissionType.Survival) {
                	LoadGameUtil.NewLevelCompleted(levelInfo, CompletedAchievements);
                }

              
                if (score >= leastScore || survivalTime > maxSurvivalTime)
                {
                    gameEventListener.GotoEnterScoreScreen(score, survivalTime);
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
            if (referer == nextLevelButton)
            {
                if (hintWindow != null)
                {
                    hintWindow.hide();
                }
                nextFrameGotoNextLevel = true;
                isInNextLevelMenu = false;
            }


            if (referer == rocketsButton)
            {
                // zmieniam bron na rakiety
                if (CanClearRestoreAmmunitionScreen)
                {
                    currentLevel.OnRestoreAmmunition(WeaponType.Rocket);
                    ClearRestoreAmmunitionScreen();
                    SoundManager.Instance.PlayReloadSound();
                    indicatorControl.ChangeAmmoType(WeaponType.Rocket);
                }
                else
                {
                    changeAmmoToWhenCanClearRestoreAmmunitionScreen = WeaponType.Rocket;
                }
                return;
            }

            if (referer == torpedoesButton)
            {
                // zmieniam bron na torpedy
                if (CanClearRestoreAmmunitionScreen)
                {
                    currentLevel.OnRestoreAmmunition(WeaponType.Torpedo);
                    ClearRestoreAmmunitionScreen();
                    SoundManager.Instance.PlayReloadSound();
                    indicatorControl.ChangeAmmoType(WeaponType.Torpedo);
                }
                else
                {
                    changeAmmoToWhenCanClearRestoreAmmunitionScreen = WeaponType.Torpedo;
                }

                return;
            }
            
            if (referer == bombsButton)
            {
                // zmieniam bron na bomby
                if (CanClearRestoreAmmunitionScreen)
                {
                    currentLevel.OnRestoreAmmunition(WeaponType.Bomb);
                    ClearRestoreAmmunitionScreen();
                    SoundManager.Instance.PlayReloadSound();
                    indicatorControl.ChangeAmmoType(WeaponType.Bomb);
                }
                else
                {
                    changeAmmoToWhenCanClearRestoreAmmunitionScreen = WeaponType.Bomb;
                }

                return;
            }
        }

        #endregion
        

       
        private void DelayedPressButton()
        {
            
        }
        
        
        private void DisplayPauseScreen()
        {
        	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Showing pause screen");
            
            isGamePaused = true;
            isInPauseMenu = true;
			OverlayContainer c;
            int left = 20;
            int top = 10;
            int width = -10 + (int)(viewport.ActualWidth * 0.7f);
            int y = 0;
            int h = (int)GetTextVSpacing();

            levelView.OnStopPlayingEnemyPlaneEngineSounds();

            mGui = new GUI(FontManager.CurrentFont, fontSize);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
            guiWindow = mGui.createWindow(new Vector4(viewport.ActualWidth * 0.15f - 10,
                                                      viewport.ActualHeight / 8 - 10, viewport.ActualWidth * 0.7f + 10, 19.0f * h),
                                          "bgui.window", (int) wt.NONE, LanguageResources.GetString(LanguageKey.Pause));
            Callback cc = new Callback(this);

          
            y += h;
            resumeButton =
                guiWindow.createButton(
                    new Vector4(left - 10, top + y, width / 2.0f, h),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Resume), cc);

            y += (int)h;
            exitButton =
                guiWindow.createButton(
                    new Vector4(left - 10, top + y, width / 2.0f, h),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.ExitToMenu), cc);
            resumeButton.activate(true);
            

            
            y = AbstractOptionsScreen.AddControlsInfoToGui(guiWindow, mGui, left, top, y, width, h, (uint)(fontSize * 0.67f));
            

            y += (int)(2*h);
            
            // Opcje dzwieku
            float soundWidth = width / 13.0f;
         	float iconSize = h;
            
         	// myzuka
            c = guiWindow.createStaticImage(new Vector4(left, top + y, iconSize, iconSize), "music_icon.png");           
            uint j = 1;
            uint startId = 1000;
            
            string styleOff = "bgui.button";
            string styleOn = "bgui.selected.button";
            
            for (uint i = 0; i <= 100; i += 10)
            {
            	
            	
                 Button button = guiWindow.createButton(
                    new Vector4(
                        left + j * soundWidth, top + y, soundWidth, h),
            		    EngineConfig.MusicVolume == i ? styleOn : styleOff, i.ToString(), cc,  startId++);
                 musicButtonIds[button] = i;
            	 j++;
            	
            }
            
            y += (int)(2*h);
            
            
            // dzwiek
            c = guiWindow.createStaticImage(new Vector4(left, top + y, iconSize, iconSize), "sound_icon.png");           
            j = 1;
            
            for (uint i = 0; i <= 100; i += 10)
            {
                 Button button = guiWindow.createButton(
                    new Vector4(
                        left + j * soundWidth, top + y, soundWidth, h),
            		EngineConfig.SoundVolume == i ? styleOn : styleOff, i.ToString(), cc,  startId++);
            	 soundButtonIds[button]= i;
            	 j++;
            }
            
            y += (int)(h);            
            guiWindow.show();
        }

        
        
        private void ClearPauseScreen()
        {
            isGamePaused = false;
            isInPauseMenu = false;
            mGui.killGUI();
            mGui = null;
        }

        private void BuildStatsScreen(Window window)
        {
           // uint oldSize = mGui.mFontSize;
           // mGui.mFontSize = (uint)(oldSize * 0.83f);

            int left = 20;
            int top = 10;

            OverlayContainer c;
            // Level stats
            c = window.createStaticText(
              new Vector4(left, top + 3 * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
               LanguageResources.GetString(LanguageKey.LevelStats));

            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f, 0.8f, 0.0f), new ColourValue(0.9f, 0.7f, 0.0f));

            float i = 4;
            window.createStaticText(
               new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.BombsDropped) + " " + this.currentLevel.Statistics.BombCount);

            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.BombsAccuracy) + " " + this.currentLevel.Statistics.BombStats + "%");

            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
               LanguageResources.GetString(LanguageKey.RocketsFired) + " " + this.currentLevel.Statistics.RocketCount);
            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.RocketsAccuracy) + " " + this.currentLevel.Statistics.RocketStats + "%");
            
            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
               LanguageResources.GetString(LanguageKey.TorpedoesFired) + " " + this.currentLevel.Statistics.TorpedoCount);
            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.TorpedoesAccuracy) + " " + this.currentLevel.Statistics.TorpedoStats + "%");


            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.GunShellsFired) + " " + this.currentLevel.Statistics.GunCount);
            i++;
            window.createStaticText(
              new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
               LanguageResources.GetString(LanguageKey.GunAccuracy) + " " + this.currentLevel.Statistics.GunStats + "%");

          
            i+=1.5f;

            window.createStaticText(
                new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                 LanguageResources.GetString(LanguageKey.PlanesDestroyed) + " " + this.currentLevel.Statistics.PlanesShotDown);


            if (currentLevel.MissionType == MissionType.Survival)
            {
                i += 1.5f;
                window.createStaticText(
                  new Vector4(left, top + i * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                   String.Format(LanguageResources.GetString(LanguageKey.SurvivalTime) + ": {0:f}s.", survivalTime));

            }
           


           // mGui.mFontSize = oldSize;

        }


        private void ClearNextLevelScreen()
        {
            isGamePaused = false;
            isInNextLevelMenu = false;
            mGui.killGUI();
            mGui = null;
        }


        private void DisplayNextLevelScreen()
        {
			LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Showing 'next level screen'");
            
            levelView.OnChangeCamera(1);
            mGui = new GUI(FontManager.CurrentFont, fontSize);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");

            guiWindow = mGui.createWindow(new Vector4(viewport.ActualWidth * 0.15f - 10,
                                                    viewport.ActualHeight / 8 - 10, viewport.ActualWidth * 0.7f + 10, 20.5f * GetTextVSpacing()),
                                                    "bgui.window", (int)wt.NONE,LanguageResources.GetString(LanguageKey.LevelCompleted));
         

            BuildStatsScreen(guiWindow);
           

            Callback cc = new Callback(this);
         
            resumeFinishedLevelButton =
                guiWindow.createButton(
                      new Vector4(5 + viewport.ActualWidth * 0.1f, 16.50f * GetTextVSpacing(), viewport.ActualWidth / 2.0f, GetTextVSpacing()),
                      "bgui.button",
                      LanguageResources.GetString(LanguageKey.Resume), cc);
               
            rearmButton =
                guiWindow.createButton(
                    new Vector4(5 + viewport.ActualWidth * 0.1f, 17.50f * GetTextVSpacing(), viewport.ActualWidth / 2.0f, GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Rearm), cc);

            nextLevelButton =
                guiWindow.createButton(
                   new Vector4(5 + viewport.ActualWidth * 0.1f, 18.50f * GetTextVSpacing(), viewport.ActualWidth / 2.0f, GetTextVSpacing()),
                   "bgui.button",
                   LanguageResources.GetString(LanguageKey.EndMission), cc);

            isInNextLevelMenu = true;
        }


        private void DisplayGameoverScreen()
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Showing game over screen");
            isGamePaused = true;
            levelView.OnStopPlayingEnemyPlaneEngineSounds();
            SoundManager.Instance.HaltOceanSound();
            mGui = new GUI(FontManager.CurrentFont, fontSize);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
            //mGui.injectMouse(0, 0, false);
            /*guiWindow = mGui.createWindow(new Vector4(0,
                                                      0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int) wt.NONE,
                                          LanguageResources.GetString(LanguageKey.GameOver));
            */
            int left = 20;
            int top = 10;
            guiWindow = mGui.createWindow(new Vector4(viewport.ActualWidth * 0.15f - 10,
                                                      viewport.ActualHeight / 8 - 10, viewport.ActualWidth * 0.7f + 10, 17.5f * GetTextVSpacing()), 
                                                      "bgui.window", (int)wt.NONE,"");


            Callback cc = new Callback(this);

            OverlayContainer c;
            c = guiWindow.createStaticText(
                new Vector4(left, top + GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                LanguageResources.GetString(LanguageKey.GameOver));
            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f, 0.0f, 0.0f), new ColourValue(0.9f, 0.1f, 0.0f));
            
            BuildStatsScreen(guiWindow);

            gameOverButton =
                guiWindow.createButton(
                    new Vector4(left, top + 15.33f * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.OK), cc);


            guiWindow.show();
            isInGameOverMenu = true;
        }

        /// <summary>
        /// Metoda ma na celu zmiane broni jesli user wybral bron ale ekran zmiany broni nie zamknal siê z uwagi min. okres wyœwietlania reklamy
        /// </summary>
        private void DelayedChangeAmmoAndCloseScreen()
        {
            if (CanClearRestoreAmmunitionScreen &&
                changeAmmoToWhenCanClearRestoreAmmunitionScreen != null)
            {

                WeaponType wt = changeAmmoToWhenCanClearRestoreAmmunitionScreen.GetValueOrDefault();
                switch (wt)
                {
                    case WeaponType.None:
                        ClearRestoreAmmunitionScreen();
                        ammoSelectedIndex = ammoSelectedIndexCount;
                        break;
                    case WeaponType.Bomb:
                        onButtonPress(bombsButton);
                        break;
                    case WeaponType.Rocket:
                        onButtonPress(rocketsButton);
                        break;
                    case WeaponType.Torpedo:
                        onButtonPress(torpedoesButton);
                        break;
                }
                changeAmmoToWhenCanClearRestoreAmmunitionScreen = null;
            }
        }

        private void DisplayChangeAmmoScreen()
        {
            try
            {
                mGui = new GUI(FontManager.CurrentFont, (uint)(fontSize * 1.05f));
                mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
                //  mGui.injectMouse(0, 0, false);
                guiWindow = mGui.createWindow(new Vector4(0,
                                                          0, viewport.ActualWidth, viewport.ActualHeight),
                                              "bgui.window", (int)wt.NONE,
                                              LanguageResources.GetString(LanguageKey.SelectAmmunition));
                Callback cc = new Callback(this);
                bombsButton = guiWindow.createButton(new Vector4(viewport.ActualWidth / 4,
                                                                 viewport.ActualHeight / 4 + 2 * GetTextVSpacing(),
                                                                 viewport.ActualWidth / 2, GetTextVSpacing()),
                                                     "bgui.button",
                                                     LanguageResources.GetString(LanguageKey.Bombs), cc);
                bombsButton.activate(true);
                rocketsButton = guiWindow.createButton(new Vector4(viewport.ActualWidth / 4,
                                                                   viewport.ActualHeight / 4 + 3 * GetTextVSpacing(),
                                                                   viewport.ActualWidth / 2, GetTextVSpacing()),
                                                       "bgui.button",
                                                       LanguageResources.GetString(LanguageKey.Rockets), cc);


                torpedoesButton = guiWindow.createButton(new Vector4(viewport.ActualWidth / 4,
                                                                   viewport.ActualHeight / 4 + 4 * GetTextVSpacing(),
                                                                   viewport.ActualWidth / 2, GetTextVSpacing()),
                                                       "bgui.button",
                                                       LanguageResources.GetString(LanguageKey.Torpedoes), cc);
                
                
               

                guiWindow.show();
                if (changingAmmoAd != null)
                {
                    showingChangingAmmoAds = Mogre.Math.RangeRandom(0, 1) > (1 - C_CHANGING_AMMO_AD_PROBABILITY);
                }
                else
                {
                    showingChangingAmmoAds = false;
                }

                if (showingChangingAmmoAds)
                {


                    OverlayContainer adContainer = guiWindow.createStaticImage(new Vector2(0,0),
                                                changingAmmoAd.path);

                    TexturePtr adTexture = TextureManager.Singleton.GetByName(changingAmmoAd.path);
                    
                    Mogre.Pair<uint, uint> pair = new Mogre.Pair<uint, uint>(adTexture.SrcWidth, adTexture.SrcHeight);
                    float targetWidth = guiWindow.w;
                    float targetHeight = guiWindow.h - GetTextVSpacing() - torpedoesButton.Y;

                    PointD adSurface = new PointD(targetWidth, targetHeight);

                    PointD scale = AdSizeUtils.ScaleAdToDisplay(pair, adSurface, false);

                    adContainer.SetDimensions(0.65f * adContainer.Width * scale.X, 0.65f * adContainer.Height * scale.Y);

                    float xPos = (adSurface.X - adContainer.Width) / 2.0f;
                    float yPos = guiWindow.h - 0.9f * adSurface.Y; //(adSurface.Y - adContainer.Height) / 2.0f;
                    adContainer.SetPosition(xPos, yPos);


                }


            }
            catch (Exception ex)
            {
                
                throw ex;
            }
       
        }
        
        private bool CanClearRestoreAmmunitionScreen
        {
            get { return !showingChangingAmmoAds || changingAmmoTime > C_CHANGING_AMMO_AD_MIN_TIME; }
        }

      
        private void ClearRestoreAmmunitionScreen()
        {

            if (showingChangingAmmoAds)
            {
                // todo: czas
                //if()
                {
                    AdManager.Singleton.RegisterImpression(changingAmmoAd);
                    AdManager.Singleton.CloseAd(changingAmmoAd);

                }
                AdManager.Singleton.Work(camera);
                showingChangingAmmoAds = false;
                changingAmmoAd = null;
                changingAmmoAdTried = false;
            }

            changingAmmo = false;
            changingAmmoTime = 0;
            mGui.killGUI();
            mGui = null;
            /*SoundManager.Instance.LoopOceanSound();
            if (mayPlaySound)
            {
                SoundManager.Instance.LoopEngineSound();
            }*/
            levelView.OnStartHangaring(1, false); // powrot platformy
            
        }


        private void increaseScore(int baseScore)
        {
            if (GameConsts.GenericPlane.CurrentUserPlane.GodMode || GameConsts.Game.AllLevelsCheat || GameConsts.GenericPlane.CurrentUserPlane.PlaneCheat || GameConsts.Game.LivesCheat || EngineConfig.DebugStart) return;

            switch (EngineConfig.Difficulty)
            {
                case EngineConfig.DifficultyLevel.Easy:
                    score += (int)(baseScore * 0.6f);
                    break;
                case EngineConfig.DifficultyLevel.Hard:
                    score += (int)(baseScore * 1.4f);
                    break;
                default:
                    score += baseScore;
                    break;
            }
        }



        #region IController Members

	
	 
		
        /// <summary>
        /// Funkcja rejestruje strzal do samolotu.
        /// </summary>
        /// <param name="bunker">Bunkier ktory strzelil.</param>
        /// <param name="plane">Samolot gracza.</param>
        public void OnBunkerFire(BunkerTile bunker, Plane plane, bool planeHit)
        {
        	if(bunker is FortressBunkerTile)
        	{
        		SoundManager.Instance.PlayFortressFireSound();
        	} else if(bunker is ShipBunkerTile && Mogre.Math.RangeRandom(0,1) > 0.5f)
        	{
        		SoundManager.Instance.PlayShipFireSound();
        	} else {
        		if(Mogre.Math.RangeRandom(0,1) > 0.5f)
        		{
            		SoundManager.Instance.PlayBunkerFireSound();
        		} else
        		{
        			SoundManager.Instance.PlayBunkerFireSound2();
        		}
        	}
            levelView.OnBunkerFire(bunker, plane, planeHit);
            // Console.WriteLine("OnBunkerFire " + " BunkerTile bunker " + " Plane plane");
        }

        /// <summary>
        /// Funkcja rejestruje na planszy nowego zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz do zarejestrowania.</param>
        public void OnRegisterSoldier(Soldier soldier, MissionType missionType)
        {
            levelView.OnRegisterSoldier(soldier, missionType);
        }


        /// <summary>
        /// Funkcja usuwa zolnierza z widoku.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostanie odrejestrowany.</param>
        public void UnregisterSoldier(Soldier soldier)
        {
            levelView.OnUnregisterSoldier(soldier);
        }



        /// <summary>
        /// Funkcja rozpoczyna animacje usmiercania zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal trafiony.</param>
        /// <param name="gun"></param>
        /// <param name="scream"></param>
        public void OnSoldierBeginDeath(Soldier soldier, bool gun, bool scream)
        {
            levelView.OnKillSoldier(soldier, !gun, scream);
            increaseScore(C_SOLDIER_SCORE);
            
            Achievement a;
            AchievementType type;
            if(soldier is General) {    			
    			 type = AchievementType.Generals;    			 
    		}else {   
			     type = AchievementType.Soldiers;     			
    		}
            
            a = CurrentLevel.GetAchievementByType(type);
            if(a!=null) {
    			a.AmountDone++;        			 	
    	    }
           // currentLevel.OnCheckVictoryConditions();
        }
        
       
        
       private Dictionary<Achievement, AchievementIcon> achievementsIcons = new Dictionary<Achievement, AchievementIcon>();
        
        
        
        public void OnAchievementFulFilled(Achievement a, bool playSound)
		{  
        	// LoadGameUtil.NewLevelCompleted(this.LevelInfo, completedAchievements.FindAll(Predicates.GetCompletedAchievements()));   
			// save tylko na koniec levelu    
			if(playSound) {
				LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Playing PlayAchievementFulFilled sound");
        		SoundManager.Instance.PlayAchievementFulFilled(); 
			}
		}
        
        
       
        public void OnAchievementUpdated(Achievement a)
		{  
        	AchievementIcon icon;
        	if(achievementsIcons.ContainsKey(a)) {
        		icon = achievementsIcons[a];        		
        	}else {
        		achievementsIcons[a] = icon =  new AchievementIcon(a, achievementsWindow);
        		
        	}        	        	        	
        	int index = CurrentLevel.Achievements.FindIndex(delegate(Achievement ach) { return ach == a; });
        
        	        	
			icon.Update(index);        
        	        
        	
        }

        public void OnSoldierPrepareToFire(Soldier soldier, float maxTime)
        {
            levelView.OnSoldierPrepareToFire(soldier, maxTime);
        }

        public void OnSoldierEndPrepareToFire(Soldier soldier)
        {
            levelView.OnSoldierEndPrepareToFire(soldier);
        }

        public void OnRegisterDebugInfo(DebugInfo debugInfo)
        {
          
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
        public void OnTurnOffEngine(Plane p)
        {
            mayPlaySound = false;
            SoundManager.Instance.HaltEngineSound(p);
           
            // pokaz komunikat ze silnik mozna ponownie odpalic
            if (p.IsEngineFaulty && p.CanTryToStartEngine)
            {
                gameMessages.ClearMessages();
                MessageEntry message = new IconedMessageEntry(new CenteredMessageEntry(viewport, GetHintMessage(), true, true), C_ENGINE_HINT_ICON);
                gameMessages.AppendMessage(message);
            }
            SoundManager.Instance.PlayStopEngineSound();
        }

        /// <summary>
        /// Funkcja zglasza o rozpoczeciu pracy silnika.
        /// </summary>
        public void OnTurnOnEngine(bool engineStartSound, Plane userPlane)
        {
            mayPlaySound = true;
            if (engineStartSound)
            {
                SoundManager.Instance.PlayStartEngineSound(currentLevel.UserPlane, startEngineSound_Ending);
            } else
            {
            	
                startEngineSound_Ending(this, null); // this will loop regular engine sound
            }
            
            SoundManager.Instance.PlayRandomIngameMusic(EngineConfig.MusicVolume);

            // wy³¹cz komunikat 
            gameMessages.ClearMessages(GetHintMessage());
          

            if (ShowHintMessages && LevelNo == 1 && firstTakeOff && userPlane.LocationState == LocationState.AircraftCarrier)
            {
                MessageEntry message = new CenteredMessageEntry(viewport, GetHintMessage2(), true, true);
                gameMessages.AppendMessage(message);
                
            }


        }
        public void OnEngineFaulty(Plane p)
        {
            MessageEntry message = new IconedMessageEntry(new CenteredMessageEntry(viewport, GetHintMessage(), true, true), C_ENGINE_HINT_ICON);
            gameMessages.AppendMessage(message);
            SoundManager.Instance.OnEngineFaulty(p);

        }

        public void OnEngineRepaired(Plane p)
        {
            SoundManager.Instance.OnEngineRepaired(p);
        }

        private void startEngineSound_Ending(object sender, EventArgs e)
        {
            if (mayPlaySound)
            {
            	if(currentLevel.UserPlane != null)
            	{
            		SoundManager.Instance.SetEngineFrequency(currentLevel.UserPlane);
            	}
               
                if (!isGamePaused && !changingAmmo)
                {
                    SoundManager.Instance.LoopEngineSound(currentLevel.UserPlane);
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

        public void OnTorpedoSunk(LevelTile tile, Torpedo ammunition, TorpedoFailure? torpedoFailure)
        {
            if (tile is OceanTile || tile is ShipTile)
            {
                levelView.OnAmmunitionVanish(tile, ammunition);
                SoundManager.Instance.PlayIncorrectStart();
                OnWaterBubblesSound();
                if (torpedoFailure != null)
                {
                    if (torpedoFailure == TorpedoFailure.TooHigh)
                        gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.TorpedoTooHigh));
                    else if (torpedoFailure == TorpedoFailure.LongTravelDistance)
                        gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.TorpedoTooLongTravelDistance));
                }
               
            }
        }
		public void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition)
        {
			gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.EnemyInstallationDamaged));            
			OnTileBombed(tile, ammunition);			
			levelView.OnFortressHit(tile, ammunition);
			
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
            	if(ammunition is GunBullet) {
            		if(Mogre.Math.RangeRandom(0,1) > 0.90f) {
            		 	SoundManager.Instance.PlayRicochetSound();
            		}
            	}else {
            		 SoundManager.Instance.PlayExposionSound();
            	}
                levelView.OnAmmunitionExplode(tile, ammunition);
               
            }
        }

        public void OnTorpedoHitGroundOrWater(LevelTile tile, Torpedo torpedo, float posX, float posY)
        {
            if(tile is OceanTile)
            {
                SoundManager.Instance.LoopTorpedoRunSound();
            }
            else
            {
                SoundManager.Instance.HaltTorpedoRunSound();
            }
            
            levelView.OnTorpedoHitGroundOrWater(tile, torpedo, posX, posY);
        }

        public void OnWaterBubblesSound()
        {
            SoundManager.Instance.SingleWaterBubblesSound();
        }

        public void OnStartWaterBubblesSound()
        {
            SoundManager.Instance.LoopWaterBubblesSound();
        }

        public void OnStopWaterBubblesSound()
        {
            SoundManager.Instance.HaltWaterBubblesSound();
        }


        public void OnShipBeginSubmerging(LevelTile tile)
        {
            OnStartWaterBubblesSound();
            SoundManager.Instance.PlayStartSubmergingSound();
            levelView.OnShipBeginSubmerging(tile);
        }

        public void OnShipBeginEmerging(LevelTile tile)
        {
            OnStartWaterBubblesSound();
            levelView.OnShipBeginEmerging(tile);
        }

        public void OnShipSubmerging(LevelTile tile)
        {
            levelView.OnShipSubmerging(tile);
        }

        public void OnShipEmerging(LevelTile tile)
        {
            levelView.OnShipEmerging(tile);
        }

        public void OnShipEmerged(LevelTile tile)
        {
        	 OnStopWaterBubblesSound();
            levelView.OnShipEmerged(tile); 
        }

        public void OnShipSubmerged(LevelTile tile)
        {
        	OnStopWaterBubblesSound();
            levelView.OnShipSubmerged(tile);
        }

        public void OnShipBeginSinking(ShipTile tile)
        {
        	if(tile is BeginShipTile) {
        		Achievement a = CurrentLevel.GetAchievementByShipType((tile as BeginShipTile).TypeOfEnemyShip);
             	if(a!=null) {
    		 		a.AmountDone++;        			 	
             	}
        	}
        	  
             OnStartWaterBubblesSound();
             levelView.OnShipBeginSinking(tile);                         
        }

        public void OnShipSinking(ShipTile tile)
        {
            levelView.OnShipSinking(tile);


            if(tile.SinkingTimeElapsed > 5000) // maksymalny czas trwania dzwieku toniecia
            {
                OnStopWaterBubblesSound();
            }
        }
        
       /* public void OnShipUnderWater(ShipTile tile)
        {
            levelView.OnShipUnderWater(tile);
        }*/
        

        public void OnShipSunk(BeginShipTile tile)
        {
            levelView.OnShipSunk(tile);
            OnStopWaterBubblesSound();
           
          
        
            //TODO: remove the ship from the level.
            
         //  currentLevel.
          //  this.lev
         //   this.ShipsList.Remove
          //  tile = null;
        }

        public void OnShipDamaged(ShipTile tile, ShipState state)
        {
        	levelView.OnShipDamaged(tile, state);        
        }

        public void OnTileDestroyed(LevelTile tile, Ammunition ammunition)
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.EnemyInstallationDestroyed));
                // GameMessages.C_TILE_DESTROYED);
        
            levelView.OnAmmunitionExplode(tile, ammunition);
            
            levelView.OnTileDestroyed(tile);
            
            Achievement a;
            AchievementType? type = null;
         	

            if (tile is BarrackTile)
            {
            	type = AchievementType.Barracks;    
                increaseScore(C_BARRACK_SCORE);
            }
            else if (tile is WoodBunkerTile)
            {
            	type = AchievementType.WoodBunkers;    
                increaseScore(C_WOODEN_BUNKER_SCORE);
            }
            else if (tile is ConcreteBunkerTile)
            {
            	type = AchievementType.ConcreteBunkers;
                increaseScore(C_CONCRETE_BUNKER_SCORE);
            } if (tile is FlakBunkerTile)
            {
            	type = AchievementType.FlakBunkers;
                increaseScore(C_FLAK_BUNKER_SCORE);
            }
            else if (tile is FortressBunkerTile)
            {
            	type = AchievementType.Fortresses;
                increaseScore(C_FORTRESS_BUNKER_SCORE);
            }
            else if (tile is ShipWoodBunkerTile)
            {          
				type = AchievementType.WoodBunkers;            	
                increaseScore(C_SHIP_WOODEN_BUNKER_SCORE);
            }
            else if (tile is ShipConcreteBunkerTile)
            {
            	type = AchievementType.ConcreteBunkers;
                increaseScore(C_SHIP_CONCRETE_BUNKER_SCORE);
            }
            if(Mogre.Math.RangeRandom(0,1) > 0.5f)
            {
                SoundManager.Instance.PlayHeavyExposionSound();
            }
            else
            {
                SoundManager.Instance.PlayExposionSound();
            }
            if(type != null && type.HasValue) {
	            a = CurrentLevel.GetAchievementByType(type.Value);
	            if(a!=null) {
	    			a.AmountDone++;        			 	
	    	    }
            }
           
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
            if(rocket.Owner is BunkerTile)
            {
                SoundManager.Instance.PlaySmallMissleSound();
            }
            else
            {
                SoundManager.Instance.PlayMissleSound();
            }
            
            levelView.OnRegisterAmmunition(rocket);
        }

        public void OnRegisterBunkerShellBullet(BunkerShellBullet shellBullet)
        {
            levelView.OnRegisterAmmunition(shellBullet);
        }

        public void OnRegisterTorpedo(Torpedo torpedo)
        {
            SoundManager.Instance.PlayTorpedoSound();
            levelView.OnRegisterAmmunition(torpedo);
        }

        public void OnRegisterFlakBullet(FlakBullet flakBullet)
		{
        	SoundManager.Instance.PlayFlakBunkerFireSound();
			levelView.OnRegisterAmmunition(flakBullet);
		}
        
        public void OnRegisterGunBullet(GunBullet gunBullet)
		{
			levelView.OnRegisterAmmunition(gunBullet);
		}
       
        public void OnGearToggleEnd(object plane)
        {
            currentLevel.OnGearToggled((Plane) plane);
            
        }

        public PlaneView FindPlaneView(Plane p)
        {
            return levelView.FindPlaneView(p);
        }
        public void OnFireGun(IObject2D plane)
        {
            if (Environment.TickCount - lastFireTick >= GameConsts.Gun.FireInterval)
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
        /// Uruchamiane, gdy jakiœ samolot trafi drugi z dzia³ka lotoniczego
        /// <author>Adam Witczak</author>
        /// </summary>
        /// <param name="plane"></param>
        public void OnGunHitPlane(Plane plane)
        {
        	if(
        		!SoundManager.Instance.IsRicochetBeingPlayed() &&  Mogre.Math.RangeRandom(0,1) > 0.92f ||
        	  	Mogre.Math.RangeRandom(0,1) > 0.99f
        	  )
        	{
        		SoundManager.Instance.PlayRicochetSound();
        	}
            levelView.OnGunHitPlane(plane);
        }

        public void OnKillSoldier(Soldier soldier)
        {
            //Console.WriteLine("OnKillSoldier  Soldier soldier");
        }

        public void OnPlayFanfare()
        {
            SoundManager.Instance.PlayFanfare();
        }
        public void OnReadyLevelEnd()
        {
        	if(readyForLevelEnd) return; // nie ma co sprawdzac, i tak juz zostal odtrabiony koniec misji
        	
            readyForLevelEnd = true;

            string message = "";

            if (currentLevel.MissionType == MissionType.BombingRun)
            {
                message =
                    String.Format(
                        LanguageResources.GetString(LanguageKey.AllEnemySoldiersEliminatedLandOnTheCarrierAndPressX),
                        KeyMap.GetName(KeyMap.Instance.AltFire));
                         
            }
            else if (currentLevel.MissionType == MissionType.Dogfight)
            {
                message =
                   String.Format(
                       LanguageResources.GetString(LanguageKey.AllEnemyPlanesDestroyedLandOnCarrierAndPressX),
                       KeyMap.GetName(KeyMap.Instance.AltFire));
             
            }
            else if (currentLevel.MissionType == MissionType.Survival)
            {
                message =
                   String.Format(
                       LanguageResources.GetString(LanguageKey.AllEnemyPlanesDestroyedLandOnCarrierAndPressX),
                       KeyMap.GetName(KeyMap.Instance.AltFire));

            }
            else if (currentLevel.MissionType == MissionType.Assassination)
            {
                message =
                   String.Format(
                       LanguageResources.GetString(LanguageKey.TargetNeutralizedLandOnCarrierAndPressX),
                       KeyMap.GetName(KeyMap.Instance.AltFire));
             
            }
            else if (currentLevel.MissionType == MissionType.Naval)
            {
                message =
                   String.Format(
                       LanguageResources.GetString(LanguageKey.AllEnemyShipsDestroyedLandOnCarrierAndPressX),
                       KeyMap.GetName(KeyMap.Instance.AltFire));
              
            }
            gameMessages.ClearMessages();
            gameMessages.AppendMessage(new MessageEntry(0, 0, message, true, true ));
            UpdateHints(true);
            OnPlayFanfare();
        }

        public void OnLevelFinished()
        {
            OnReadyLevelEnd();
            isGamePaused = true;


            // mission is fulfilled - make next level available
            if (!this.LevelInfo.IsCustom && File.Exists(XmlLevelParser.GetLevelFileName(levelNo.Value + 1)))
            {                    
            	LevelInfo nextLevelInfo = new LevelInfo(levelNo.Value + 1);
                if (!LoadGameUtil.Singleton.HasCompletedLevel(nextLevelInfo))
                {
                    LoadGameUtil.NewLevelCompleted(nextLevelInfo, new List<Achievement>());
                }
            }
                

            DisplayNextLevelScreen();
        }
        

        public void OnSecondaryFireOnCarrier(Plane userPlane)
        {
            if (readyForLevelEnd && !isInNextLevelMenu)
            {
                OnLevelFinished();
            }
            else
            {

                if (userPlane.CanChangeAmmunition)
                {
                    userPlane.MovementVector.X = 0;
                    OnChangeAmmunition();
                }
            }
        }

        public void OnRocketHitPlane(Rocket rocket, Plane plane)
        {
            SoundManager.Instance.PlayExposionSound();
        }

        public void OnChangeAmmunition()
        {
            //currentLevel.OnCheckVictoryConditions();
            levelView.OnStopPlayingEnemyPlaneEngineSounds();
        //  readyForLevelEnd = true;
          //  if (!readyForLevelEnd)
            {

                if(!changingAmmo)
                {
                    changingAmmo = true;            	
                    DisplayChangeAmmoScreen();
                   
                   // SoundManager.Instance.HaltEngineSound();
                   // SoundManager.Instance.HaltOceanSound();
                    levelView.OnStartHangaring(-1, true);
                    
                }

                //DisplayNextLevelScreen();
                //DisplayGameoverScreen();
            }
            /*else if(!isInNextLevelMenu)
            {
                OnReadyLevelEnd();
                isGamePaused = true;
                DisplayNextLevelScreen();
            }*/
        }

        public void OnWarCry(Plane plane)
        {
            levelView.OnWarCry(plane);
        }

        public void OnPlanePass(Plane plane)
        {
            levelView.OnPlanePass(plane);
        }

        public void OnPlaneDestroyed(Plane plane)
        {
            if (plane.IsEnemy)
            {
                gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.EnemyPlaneDownIRepeat));
             
                increaseScore(plane is EnemyFighter ? C_ENEMY_FIGHTER_SCORE : C_ENEMY_BOMBER_SCORE);
                
                Achievement a = CurrentLevel.GetAchievementByEnemyPlaneType(plane.PlaneType);
	            if(a!=null) {
	    			a.AmountDone++;        			 	
	    	    }

            }
            levelView.OnPlaneDestroyed(plane);
          
            
            if(plane is StoragePlane)
            {
                SoundManager3D.Instance.PlayAmbientMusic(SoundManager3D.C_STORAGE_PLANE_DESTROYED, false);
            }
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
                if(!GameConsts.GenericPlane.CurrentUserPlane.GodMode) lives--;
                if (lives < 0 || currentLevel.Lives - 1 < 0)
                {
                    isGamePaused = true;
                    DisplayGameoverScreen();
                }
                else
                {
                	
                	
                    isGamePaused = true;
                    currentLevel.NextLife();
                    levelView.NextLife();
                    isGamePaused = false;
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Player killed - lives left:"+ currentLevel.Lives);
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
        
  		public void OnUnregisterAmmunition(Ammunition ammo)
        {
  			if(ammo is Rocket) {
  				OnUnregisterRocket(ammo as Rocket);
  			}else
            if(ammo is Torpedo) {
  				OnUnregisterTorpedo(ammo as Torpedo);
  			}else
  			if(ammo is FlakBullet) {
  				SoundManager.Instance.PlayFlakBunkerFireSound();
  				OnUnregisterFlakBullet(ammo as FlakBullet);
            }
            else
            if (ammo is BunkerShellBullet)
            {
                OnUnregisterBunkerShellBullet(ammo as BunkerShellBullet);
            } else
  			if(ammo is GunBullet) {  				
  				OnUnregisterGunBullet(ammo as GunBullet);
  			}
        }
  		
  		
        public void OnUnregisterRocket(Rocket rocket)
        {
            levelView.OnUnregisterRocket(rocket);
        }

        public void OnUnregisterTorpedo(Torpedo torpedo)
        {
            levelView.OnUnregisterTorpedo(torpedo);
        }
        
        public void OnUnregisterFlakBullet(FlakBullet flak)
        {
            levelView.OnUnregisterFlakBullet(flak);
        }
        public void OnUnregisterBunkerShellBullet(BunkerShellBullet shellBullet)
        {
            levelView.OnUnregisterBunkerShellBullet(shellBullet);
        }

        
        
        public void OnUnregisterGunBullet(GunBullet gun)
        {
            levelView.OnUnregisterGunBullet(gun);
        }


        public void OnTileRestored(BunkerTile restoredBunker)
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.OhNoEnemySoldiersAreRebuildingBunker));
            SoundManager.Instance.PlayBunkerRebuild();
            levelView.OnTileRestored(restoredBunker);
        }

        private bool ShowHintMessages
        {
        	get { return EngineConfig.Difficulty <= EngineConfig.DifficultyLevel.Easy; }
        }

        public float SurvivalTime
        {
            get { return survivalTime; }
         
          
        }


        public void OnPotentialBadLanding(Plane p)
        {
            if (ShowHintMessages)
            {
                if (!GetBadLandingHintMessage().Equals(gameMessages.PeekMessage()))
                {
                    IconedMessageEntry message =
                        new IconedMessageEntry(new CenteredMessageEntry(viewport, GetBadLandingHintMessage(), false, false), C_BAD_LANDING_HINT_ICON);
                   
                    message.UseAutoDectetedIconDimesions(viewport);
                    message.CenterIconOnScreen(viewport);
                    message.IncreaseY(-message.Y);
                    message.IncreaseY(0.2f);
                    gameMessages.ClearMessages();
                    gameMessages.AppendMessage(message);
                }

            }
        }

      

        public void OnPotentialLanding(Plane p)
        {
        	if (ShowHintMessages)        
            {
                if(!GetLandingHintMessage().Equals(gameMessages.PeekMessage()))
                {
                	
                    MessageEntry m = new CenteredMessageEntry(viewport, GetLandingHintMessage(), true, false);
                    IconedMessageEntry message = new IconedMessageEntry(m, C_LANDING_HINT_ICON);
                    
                    message.UseAutoDectetedIconDimesions(viewport);
                  //  message.CenterIconOnScreen(viewport);
                  //  message.IncreaseY(-message.Y);
                  //  message.IncreaseY(0.2f);
                  //  message.IncreaseX(-message.CustomIconDimensions.x);
                    
                 //   message.IncreaseY(-message.CharHeight / 2.0f);
                    gameMessages.ClearMessages();
                    gameMessages.AppendMessage(message);     
                }
        		   		
        	}
        }
        
        public void OnTakeOff()
        {
           
            if (firstTakeOff && levelNo == 1)
            {
                gameMessages.ClearMessages(GetHintMessage2());

                if(EngineConfig.Difficulty > EngineConfig.DifficultyLevel.Easy)
                {
                    gameMessages.AppendMessage(String.Format(LanguageResources.GetString(LanguageKey.RetractYourLandingGearWithG), KeyMap.GetName(KeyMap.Instance.Gear)));
                }
                if (EngineConfig.Difficulty <= EngineConfig.DifficultyLevel.Easy)
                {
                    gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.KillAllEnemySoldiers));
                }
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

           /* if(mGuiHint!=null)
            {
                mGuiHint.killGUI();
                mGuiHint = null;
            }*/
          

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

        float timeSinceLastBuzzerSound = Environment.TickCount;
        float minBuzzerSoundRepeatTime = 10000; // 10 sek
        public void OnEnemyAttacksCarrier()
        {
            gameMessages.AppendMessage(LanguageResources.GetString(LanguageKey.WARNINGProtectTheCarrierFromEnemyPlane));
            if(Environment.TickCount > timeSinceLastBuzzerSound + minBuzzerSoundRepeatTime) {
            	SoundManager.Instance.PlayBuzzerSound();
            	timeSinceLastBuzzerSound = Environment.TickCount;
            }
        }

        public void OnPlaneWrongDirectionStart()
        {
            SoundManager.Instance.PlayIncorrectStart();
        }
        
      

        #endregion
    	
		
    	
		
    	
		public IFrameWork GetFramework()
		{
			return framework;
		}
    	
		
    }

   
}