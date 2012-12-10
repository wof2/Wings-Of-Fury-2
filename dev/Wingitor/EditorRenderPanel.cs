using System;
using System.Collections.Generic;
using System.Text;
using BetaGUI;
using Mogre;
using MOIS;
using Wingitor;
using Wof.Controller;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Model.Level;
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
using Plane=Wof.Model.Level.Planes.Plane;

namespace wingitor
{
    public class EditorRenderPanel : RenderPanel, IController
    {
        private LevelView levelView;
        private Level currentLevel;

        private IGameTest gameTest;

        protected string filename;
        public LevelView LevelView
        {
            get { return levelView; }
            set { levelView = value; }
        }

        public Level CurrentLevel
        {
            get { return currentLevel; }
        }

        

        public override void Destroy()
        {
           
            if (levelView != null) levelView.Destroy();
            base.Destroy();

            
        }

        private MainWindow mainWindow;

        public EditorRenderPanel(MainWindow window)
            : base()
        {
            mainWindow = window;
            this.filename = "custom_levels/enhanced-4" + XmlLevelParser.C_LEVEL_POSTFIX;
        }

        public void SetGameTest(IGameTest gameTest)
        {
            this.gameTest = gameTest;
            this.filename = gameTest.LevelFilename;
        }
        /*
        public EditorRenderPanel(string filename) : base()
        {
            this.filename = filename;
           
        }*/

        private string levelToLoad;

        public override void CreateScene()
        {
            if(filename != null)
            {
                currentLevel = new Level(filename, this, EngineConfig.CurrentPlayerPlaneType);
                levelView = new LevelView(this, this);
                levelView.OnRegisterLevel(currentLevel);
                OnRegisterPlane(currentLevel.UserPlane);

                foreach (StoragePlane sp in currentLevel.StoragePlanes)
                {
                    OnRegisterPlane(sp);
                }     

                foreach (ISceneTest test in this.gameTest.SceneTests)
                {
                    test.Framework = this;
                    test.OnRegisterLevel(currentLevel);
                }
                levelView.SetVisible(true);
            }
            
        }

       
       
        protected override void OnUpdateModel(FrameEvent evt)
        {
            if(currentLevel != null)
            {
                int timeInterval = (int)System.Math.Round(evt.timeSinceLastFrame * 1000);
                currentLevel.Update(timeInterval);

                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Left) )
                {
                    currentLevel.OnSteerLeft();
                }
                else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Right))
                {
                    currentLevel.OnSteerRight();
                }
                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Up))
                {
                    currentLevel.OnSteerUp();
                }
                else if (inputKeyboard.IsKeyDown(KeyMap.Instance.Down))
                {
                    currentLevel.OnSteerDown();
                }
                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Engine))
                {
                    currentLevel.OnToggleEngineOn();
                }

                if (inputKeyboard.IsKeyDown(KeyMap.Instance.Gear) )
                {
                    currentLevel.OnToggleGear();
                }

                 if (inputKeyboard.IsKeyDown(KeyMap.Instance.GunFire) )
                 {
                     currentLevel.OnFireGun();
                 }

                 if (inputKeyboard.IsKeyDown((KeyCode)KeyMap.Instance.Spin))
                 {
                     currentLevel.OnSpinPressed();
                 }


                 if (inputKeyboard.IsKeyDown(KeyMap.Instance.PausePlane) &&
                    Button.CanChangeSelectedButton(3.0f))
                 {

                     currentLevel.UserPlane.PlanePaused = !currentLevel.UserPlane.PlanePaused;
                     currentLevel.EnemyPlanes.ForEach(delegate(Plane ep) { ep.PlanePaused = !ep.PlanePaused; });

                     Button.ResetButtonTimer();
                 }

            }

          
            base.OnUpdateModel(evt);
        }

        private bool reloadLevel = false;
        public void ReloadLevel(string filename)
        {
            levelToLoad = filename;
            reloadLevel = true;
        }
        public void ReloadLevel()
        {
            levelToLoad = null;
            reloadLevel = true;
        }
        public delegate void InvokeDelegate(XmlLevelParser parser);
        public delegate void InvokeDebugInfoDelegate(DebugInfo debugInfo);


        protected override bool FrameEnded(FrameEvent evt)
        {
            bool result = base.FrameEnded(evt);
            if(this.reloadAllReourcesNextFrame)
            {
                reloadAllReourcesNextFrame = false;

                FrameWorkStaticHelper.ReloadAllReources(this);
            }

            return result;
        }

        public void OnRegisterDebugInfo(DebugInfo debugInfo)
        {
            //mainWindow.BeginInvoke(new InvokeDebugInfoDelegate(mainWindow.UpdateDebugBox), debugInfo);
 			mainWindow.UpdateDebugBox( debugInfo);
        }

        protected override bool FrameStarted(Mogre.FrameEvent evt)
        {

            if (reloadLevel)
            {
                levelView.Destroy();
                if (levelToLoad != null)
                {
                    currentLevel.Dispose();
                }
                SceneMgr.ClearScene();
                EffectsManager.Singleton.Clear();
                if (levelToLoad != null)
                {
                    filename = levelToLoad;
                    currentLevel = new Level(filename, this, EngineConfig.CurrentPlayerPlaneType);
                }
               
                levelView = new LevelView(this, this);
                levelView.OnRegisterLevel(currentLevel);
                levelView.SetVisible(true);

                reloadLevel = false;
                levelToLoad = null;
                mainWindow.BeginInvoke(new InvokeDelegate(mainWindow.OnLevelLoaded),(currentLevel.LevelParser));

                foreach (ISceneTest test in this.gameTest.SceneTests)
                {
                    test.Framework = this;
                    test.OnRegisterLevel(currentLevel);
                }
                
            }


            if(base.FrameStarted(evt))
            {
                if (levelView != null)
                {

                    levelView.OnFrameStarted(evt);
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        #region Implementation of IController

       

        public void OnRegisterPlane(Plane plane)
        {
            levelView.OnRegisterPlane(plane);
        }

        public PlaneView FindPlaneView(Plane plane)
        {
            return levelView.FindPlaneView(plane);
        }

        public void OnBunkerFire(BunkerTile bunker, Plane plane)
        {
            
        }

        public void OnLevelFinished()
        {

        }
        public void OnPlanePass(Plane plane)
        {
            
        }

        public void OnUnregisterPlane(Plane plane)
        {
            levelView.OnUnregisterPlane(plane);
        }

        public void OnGunHitPlane(Plane plane)
        {
            
        }

        public void OnEnemyPlaneFromTheSide(bool left)
        {
            
        }

        public void OnPlaneForceChangeDirection()
        {
            
        }

        public void OnPlaneForceGoDown()
        {
            
        }

        public void OnEnemyAttacksCarrier()
        {
            
        }

        public void OnPlaneWrongDirectionStart()
        {
            
        }

        public void OnStartWaterBubblesSound()
        {
            
        }

        public void OnStopWaterBubblesSound()
        {
            
        }

        public void OnShipDamaged(ShipTile tile, ShipState state)
        {
            
        }

        public void OnShipBeginSinking(ShipTile tile)
        {
            
        }

        public void OnShipSinking(ShipTile tile)
        {
            
        }

        public void OnShipSunk(BeginShipTile tile)
        {
            
        }


        public void OnToggleGear(Plane plane)
        {
            
        }

        public void OnRegisterRocket(Rocket rocket)
        {
            levelView.OnRegisterAmmunition(rocket);
        }

        public void OnRegisterBunkerShellBullet(BunkerShellBullet shellBullet)
        {
            
        }

        public void OnRegisterTorpedo(Torpedo torpedo)
        {
            levelView.OnRegisterAmmunition(torpedo);
        }

        public void OnGearToggleEnd(object plane)
        {
            
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

        public void OnSoldierBeginDeath(Soldier soldier, bool gun, bool scream)
        {
            
        }

        public void OnSpinEnd(object plane)
        {
            ((Plane)plane).OnSpinEnd();
        }

        public void OnRegisterBomb(Bomb bomb)
        {
            levelView.OnRegisterAmmunition(bomb);
        }
        public void OnFireGun(IObject2D plane)
        {
          
        }

        public void OnGunHit(LevelTile tile, float posX, float posY)
        {
            levelView.OnGunHit(tile, posX, posY);
        }

        public void OnKillSoldier(Soldier soldier)
        {
            
        }

        public void OnReadyLevelEnd()
        {
            
        }

        public void OnChangeAmmunition()
        {
            
        }

       

        public void OnPlaneDestroyed(Plane plane)
        {
            levelView.OnPlaneDestroyed(plane);


        }

        public void OnPlaneCrashed(Plane plane, TileKind tileKind)
        {
            levelView.OnPlaneCrashed(plane, tileKind);
        }

        public void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition)
        {
            
        }

        public void OnWarCry(Plane plane)
        {
            
        }

        public void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            levelView.OnCatchPlane(plane, carrierTile);
            
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

        public void OnUnregisterTorpedo(Torpedo torpedo)
        {
            levelView.OnUnregisterTorpedo(torpedo);
        }

        public void OnUnregisterBunkerShellBullet(BunkerShellBullet shellBullet)
        {
            levelView.OnUnregisterBunkerShellBullet(shellBullet);
        }

        public void OnTileRestored(BunkerTile restoredBunker)
        {
            
        }

        public void OnTakeOff()
        {
            
        }

        public void OnTouchDown()
        {
            
        }

        public void OnPlaneWrecked(Plane Plane)
        {
            currentLevel.NextLife();
            levelView.NextLife();
        }


        public void OnSoldierPrepareToFire(Soldier soldier, float maxTime)
        {
            levelView.OnSoldierPrepareToFire(soldier, maxTime);
        }

        public void OnSoldierEndPrepareToFire(Soldier soldier)
        {
            levelView.OnSoldierEndPrepareToFire(soldier);
        }

        public void OnTurnOffEngine(Plane p)
        {
            
        }

        public void OnTurnOnEngine(bool engineStartSound, Plane userPlane)
        {
            
        }

        public void OnEngineFaulty(Plane p)
        {
            
        }

        public void OnEngineRepaired(Plane p)
        {
           
        }

        public void OnStartEngineFailed()
        {
            
        }

        public void OnEnemyPlaneBombed(Plane plane, Ammunition ammunition)
        {
            
        }

        public void OnTileBombed(LevelTile tile, Ammunition ammunition)
        {
            
        }

        public void OnTorpedoSunk(LevelTile tile, Torpedo ammunition, TorpedoFailure? torpedoFailure)
        {
            
        }

        public void OnTileDestroyed(LevelTile tile, Ammunition ammunition)
        {
            
        }

        public void OnTorpedoHitGroundOrWater(LevelTile tile, Torpedo torpedo, float posX, float posY)
        {
            
        }

        public void OnPrepareChangeDirection(Direction newDirection, Plane plane, TurnType turnType)
        {
            levelView.OnPrepareChangeDirection(newDirection, plane, turnType);
        }

        public void OnPrepareChangeDirectionEnd(object turnInfo)
        {
            ((TurnInfo)turnInfo).plane.OnPrepareChangeDirectionEnd(((TurnInfo)turnInfo).turnDurationInSeconds);
        }

        public void OnChangeDirectionEnd(object turnInfo)
        {
            TurnInfo t = ((TurnInfo)turnInfo);
            t.plane.OnChangeDirectionEnd(t.turnType);
        }

        public void OnSpinBegin(Plane plane)
        {
            levelView.OnBeginSpin(plane);
        }
        
        
        public void OnPotentialLanding(Plane p)
        {
        	
        }

        public void OnPotentialBadLanding(Plane p)
        {

        }

        public void OnShipBeginSubmerging(LevelTile tile)
        {
            
        }

        public void OnShipBeginEmerging(LevelTile tile)
        {
           
        }

        public void OnShipSubmerging(LevelTile tile)
        {
            
        }

        public void OnShipEmerging(LevelTile tile)
        {
           
        }

        public void OnShipEmerged(LevelTile tile)
        {
            
        }

        public void OnShipSubmerged(LevelTile tile)
        { 
        }

        public void OnSecondaryFireOnCarrier(Plane userPlane)
        {
            
        }

        public void OnRocketHitPlane(Rocket rocket, Plane plane)
        {
            
        }

        public void OnPlayFanfare()
        {
           
        }

        #endregion

        protected bool reloadAllReourcesNextFrame = false;
        public void ReloadAllResourcesNextFrame()
        {
            reloadAllReourcesNextFrame = true;
        }
    	
		public void OnBunkerFire(BunkerTile bunker, Plane plane, bool planeHit)
		{
		
		}
    	
		public void OnRegisterFlakBullet(FlakBullet flakBullet)
		{
			
		}
    	
		public void OnFlakFire(FlakBunkerTile bunker, Plane plane, Wof.Model.Level.Common.PointD pos, bool hit)
		{
			
		}
    	
		public void OnUnregisterAmmunition(Ammunition ammo)
		{
			
		}
    	
		public void OnRegisterGunBullet(GunBullet gunBullet)
		{
		
		}
    	
		public IFrameWork GetFramework()
		{
			return levelView.framework;
		}
    	
		public void OnAchievementFulFilled(Achievement a, bool playSound)
		{
			
		}
    	
		public void OnAchievementUpdated(Achievement a)
		{
		
		}
    }
}
