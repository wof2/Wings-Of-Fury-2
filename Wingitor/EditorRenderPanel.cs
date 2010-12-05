using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wingitor;
using Wof.Controller;
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

        public EditorRenderPanel(string filename) : base()
        {
            this.filename = filename;
           
        }

        private string levelToLoad;

        public override void CreateScene()
        {
            if(filename != null)
            {
                currentLevel = new Level(filename, this, EngineConfig.CurrentPlayerPlaneType);
                levelView = new LevelView(this, this);
                levelView.OnRegisterLevel(currentLevel);
                levelView.SetVisible(true);
            }
            
        }
       
        protected override void OnUpdateModel(FrameEvent evt)
        {
            if(currentLevel != null)
            {
                int timeInterval = (int)System.Math.Round(evt.timeSinceLastFrame * 1000);
                currentLevel.Update(timeInterval);
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

        public void OnBunkerFire(BunkerTile bunker, Plane plane)
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

        public void OnRegisterTorpedo(Torpedo torpedo)
        {
            levelView.OnRegisterAmmunition(torpedo);
        }

        public void OnGearToggled(object plane)
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

        public void OnPlaneCrashed(Plane plane, TileKind tileKind)
        {
            
        }

        public void OnPlaneDestroyed(Plane plane)
        {
            
        }

        public void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition)
        {
            
        }

        public void OnWarCry(Plane plane)
        {
            
        }

        public void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            
        }

        public void OnReleasePlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            
        }

        public void OnUnregisterRocket(Rocket rocket)
        {
            levelView.OnUnregisterRocket(rocket);
        }

        public void OnUnregisterTorpedo(Torpedo torpedo)
        {
            levelView.OnUnregisterTorpedo(torpedo);
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

        public void OnTurnOnEngine(bool engineStartSound)
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
            
        }

        public void OnPrepareChangeDirectionEnd(object turnInfo)
        {
            
        }

        public void OnChangeDirectionEnd(object plane)
        {
            
        }

        public void OnSpinBegin(Plane plane)
        {
            
        }
        
        
        public void OnPotentialLanding(Plane p)
        {
        	
        }

        public void OnSecondaryFireOnCarrier()
        {
            
        }

        public void OnRocketHitPlane(Rocket rocket, Plane plane)
        {
            
        }

        #endregion
    }
}
