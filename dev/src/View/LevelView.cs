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
using System.Reflection;

using Mogre;
using Wof.Controller;
using Wof.Controller.AdAction;
using Wof.Misc;
using Wof.Model.Configuration;
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.View.AmmunitionViews;
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using Wof.View.TileViews;
using Wof.View.VertexAnimation;
using Math = Mogre.Math;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    /// <summary>
    /// Klasa reprezentuj¹ca poziom gry w warstwie View
    /// <author>Adam Witczak, Kamil S³awiñski</author>
    /// </summary>
    public class LevelView
    {
        private const int C_AD_BASE_X = 200;
        private const int C_AD_Z_DIST = 125;
        private const int C_AD_X_DIST = 450;
        private const int C_AD_Y_DIST = 20;

        private const int C_AD_SIZE = 32;

        public const int C_AD_DYNAMIC_ADS_COUNT = 3;
        private float[] C_AD_MAX_DISPLAY_TIMES = new float[C_AD_DYNAMIC_ADS_COUNT];
        

        /*
        private AdManager.Ad ad = null;
        public AdManager.Ad Ad
        {
            get { return ad; }
        }
        */

        private int cameraIndexBeforeHangaring;
        public int CurrentCameraHolderIndex
        {
            get { return currentCameraHolderIndex; }
        }

        private int currentCameraHolderIndex = 0;

       

        public SceneNode CurrentCameraHolder
        {
            get
            {
                if (currentCameraHolderIndex >= 0 && currentCameraHolderIndex < cameraHolders.Count)
                {
                    return cameraHolders[currentCameraHolderIndex];
                }
                return null;
            }
        }

        private List<SceneNode> cameraHolders;

        public List<SceneNode> CameraHolders
        {
            get { return cameraHolders; }
        }

        public const int oceanSize = 10000;

        private static readonly float tileWidth = 10.0f;

        public static float TileWidth
        {
            get { return tileWidth; }
        }

        //Fields
        /// <summary>
        /// Licznik obiektow dodatkowych - drzew, flag
        /// uzywany do nadawania unikalnych nazw w Ogre
        /// </summary>
        private static int propCounter = 1;

        private static bool isNightScene = false;

        public static bool IsNightScene
        {
            get { return isNightScene; }
        }

        public static int PropCounter
        {
            get { return propCounter++; }
        }

        private static float modelToViewAdjust;

        public static float ModelToViewAdjust
        {
            get { return modelToViewAdjust; }
        }

        //Zbiera Tile dla danego CompositeModelView
        private List<TileView> tempTileViews;

        private static Stack<SceneNode> availableSplashNodesPool;
        private static Queue<SceneNode> usedSplashNodesPool;

        public void Destroy()
        {
            SoundManager3D.Instance.UpdaterRunning = false;
            if(dynamicAds != null)
            {
                dynamicAds.Clear();
                dynamicAds = null;
            }
            if (availableSplashNodesPool != null)
            {
                availableSplashNodesPool.Clear();
                availableSplashNodesPool = null;
            }

            if (usedSplashNodesPool != null)
            {
                usedSplashNodesPool.Clear();
                usedSplashNodesPool = null;
            }

            if (ammunitionViews != null)
            {
                ammunitionViews.Clear();
                ammunitionViews = null;
            }
            RocketView.DestroyPool(); 
            TorpedoView.DestroyPool();
            BombView.DestroyPool();
            FlakBulletView.DestroyPool();
            BunkerShellBulletView.DestroyPool();
			GunBulletView.DestroyPool();

            if (backgroundViews != null)
            {
                backgroundViews.Clear();
                backgroundViews = null;
            }

            carrierView = null;

            if (compositeModelViews != null)
            {
            	foreach(CompositeModelView cmv in compositeModelViews)
            	{
            		if(cmv is IDisposable) (cmv as IDisposable).Dispose();
            	}
                compositeModelViews.Clear();
                compositeModelViews = null;
            }
          
            cameraHolders = null;
            if(playerPlaneView != null)
            {
                playerPlaneView.Destroy();
                playerPlaneView = null;
            }
           

            if (planeViews != null)
            {
                foreach (var p in planeViews)
                {
                    p.Destroy();
                }
                planeViews.Clear();
                planeViews = null;
            }
            playerPlaneView = null;


            if (dyingSoldierViews != null)
            {
                dyingSoldierViews.Clear();
                dyingSoldierViews = null;
            }
            if (soldierViews != null)
            {
                soldierViews.Clear();
                soldierViews = null;
            }
            SoldierView.DestroyPool();
        
            if (tempTileViews != null)
            {
                tempTileViews.Clear();
                tempTileViews = null;
            }
            if (tileViews != null)
            {
                tileViews.Clear();
                tileViews = null;
            }

            if (EngineConfig.UseHydrax)
            {
            	HydraxManager.Singleton.RemoveHydraxDepthTechniques();
                HydraxManager.Singleton.DisposeHydrax();
                
            }

            //SoundManager3D.Instance.UpdaterRunning = true;
            ViewHelper.DetachQuadrangles(sceneMgr);
        }

        private void InitSplashPool(int size)
        {
            availableSplashNodesPool = new Stack<SceneNode>(size);
            usedSplashNodesPool = new Queue<SceneNode>(size);

            for (int i = 0; i < size; i++)
            {
                SceneNode s = sceneMgr.RootSceneNode.CreateChildSceneNode("SplashNode" + PropCounter, new Vector3(-15000, 10000, 5000));
                availableSplashNodesPool.Push(s);
            }
        }

        public SceneNode getSplashNode()
        {
            if (availableSplashNodesPool.Count == 0)
            {
                return null;
            }
            SceneNode s = availableSplashNodesPool.Pop();
            s.SetPosition(0,0,0);
            usedSplashNodesPool.Enqueue(s);
            return s;
        }

        public void freeSplashNode()
        {
            if (usedSplashNodesPool.Count == 0) return;
            SceneNode s = usedSplashNodesPool.Dequeue();
            availableSplashNodesPool.Push(s);
        }

        //Logic
        private Level level;
        private List<PlaneView> planeViews;

        private List<SoldierView> soldierViews;
        private List<SoldierView> dyingSoldierViews;
        private List<AmmunitionView> ammunitionViews;
        private List<TileView> tileViews;
        private List<CompositeModelView> compositeModelViews;

        private List<AdQuadrangle3D> dynamicAds;
        private CarrierView carrierView;

        private List<CompositeModelView> backgroundViews;

        private PlayerPlaneView playerPlaneView = null;

        public IFrameWork framework;
        protected SceneManager sceneMgr, minimapMgr;

        public SceneManager SceneMgr
        {
            get { return sceneMgr; }
        }

        public SceneManager MinimapMgr
        {
            get { return minimapMgr; }
        }

        protected IController controller;

        private readonly uint defaultVisibilityMask;

        public void SetVisible(bool visible)
        {
            if (visible)
            {
                sceneMgr.VisibilityMask = defaultVisibilityMask;
                if (EngineConfig.DisplayingMinimap)
                {
                    minimapMgr.VisibilityMask = defaultVisibilityMask;
                }
            }
            else
            {
                sceneMgr.VisibilityMask = 0;
                if (EngineConfig.DisplayingMinimap)
                {
                    minimapMgr.VisibilityMask = 0;
                }
            }
        }

        public LevelView(IFrameWork framework, IController controller)
        {
            isNightScene = false;

            this.framework = framework;
            this.controller = controller;

            sceneMgr = framework.SceneMgr;
            minimapMgr = framework.MinimapMgr;

            defaultVisibilityMask = sceneMgr.VisibilityMask;
            // ukryj cala scene na czas ladowania

            SetVisible(false);

            tileViews = new List<TileView>();
            compositeModelViews = new List<CompositeModelView>();
            tempTileViews = new List<TileView>();

            planeViews = new List<PlaneView>();
            soldierViews = new List<SoldierView>();
            dyingSoldierViews = new List<SoldierView>();
            ammunitionViews = new List<AmmunitionView>();
            backgroundViews = new List<CompositeModelView>();
            dynamicAds = new List<AdQuadrangle3D>();
        }

        public PlaneView FindPlaneView(Plane p)
        {
            if (playerPlaneView != null && playerPlaneView.Plane == p) return playerPlaneView;

            if (p is StoragePlane)
            {
                if (carrierView != null)
                {
                    return carrierView.FindStoragePlaneView(p as StoragePlane);
                }
            }

            return planeViews.Find(delegate(PlaneView pv) { return pv.Plane == p; });
        }

        public TileView FindTileView(LevelTile l)
        {
            return tileViews.Find(delegate(TileView tv) { return tv.LevelTile == l; });
        }

        public ShipView FindShipView(LevelTile t)
        {
            return compositeModelViews.Find(delegate(CompositeModelView c) { 
                if(c is ShipView)
                {
                    foreach (TileView tv in (c as ShipView).TileViews)
                    {
                        if (tv.LevelTile == t) return true;
                    }
                }
               
                return false;
            }) as ShipView;
            
        }

      


        /// <summary>
        /// Rejestruje w view reklamê dynamiczn¹
        /// </summary>
        /// <param name="ad"></param>
        public void OnRegisterBackgroundDynamicAd(AdManager.Ad ad)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
            int count = dynamicAds.Count;
            Vector3 position = Vector3.ZERO;
            Vector2 size;
            SceneNode adNodeParent;
            SceneNode adNodeSuper = null;
            bool isPersistent = false;

            size = new Vector2(C_AD_SIZE, C_AD_SIZE);
            TexturePtr ptr = TextureManager.Singleton.GetByName(ad.path);
            float ratio = 1.0f;
            if (ptr != null)
            {
                ratio = 1.0f * ptr.SrcWidth / ptr.SrcHeight;
            }
            size.x *= ratio;

            float newWidth = 1.0f;
            float newHeight = 1.0f;

            Entity adHolder = sceneMgr.CreateEntity("AdHolder" + count, "AdHolder.mesh");
            adHolder.CastShadows = false;
            if(count == 0)
            {
                size *= 0.3f;
                position = carrierView.MainNode._getDerivedPosition() + new Vector3(68, 1, -8);
                isPersistent = true;

                
                SceneNode adHolderNode = sceneMgr.RootSceneNode.CreateChildSceneNode(adHolder.Name + "Node", position + new Vector3(size.x * 0.5f, 0, 0));
                adHolderNode.AttachObject(adHolder);
             

                AxisAlignedBox bb = adHolder.BoundingBox;
                newWidth = size.x / (bb.Size.x * 0.87f);
                newHeight = size.y / (bb.Size.y * 0.86f);
                adHolderNode.Scale(newWidth, newHeight, 1.0f);
              
                
                adNodeParent = sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(0.0f, 0.26f, 0));
                adNodeSuper = adHolderNode;
            }
            else
            {
                // zaczep o ktoras wyspe
                int j = 0;
                for (int i = 0; i < compositeModelViews.Count; i++)
                {

                    CompositeModelView iv = compositeModelViews[i];
                    if(!(iv is CarrierView)) 
                    {
                        // hardcode :/
                        if(iv is IslandView && iv.TileViews[0].LevelTile.Variant == 7) continue; // to jest variant w ktorym wyspa ma w tle radar

                        j++;
                        if(j < count)
                        {
                            continue;
                        }

                        iv = compositeModelViews[i];

                        position = new Vector3(UnitConverter.LogicToWorldUnits(iv.TileViews[0].LevelTile.TileIndex) + Math.RangeRandom(-15 * LevelTile.TileWidth, -1 * LevelTile.TileWidth), C_AD_Y_DIST, -C_AD_Z_DIST);
                  
                        break;
                    }
                }

                // jak nie ma wyspy to losuj 
                if(position == Vector3.ZERO)
                {
                    int dir = count % 2 == 1 ? 1 : -1;
                    position = new Vector3(C_AD_BASE_X + count * C_AD_X_DIST * dir, C_AD_Y_DIST, -C_AD_Z_DIST);
                }


                Entity radarDome = sceneMgr.CreateEntity("AdRadarDome" + count, "RadarDome.mesh");
                radarDome.CastShadows = false;
                SceneNode radarNode = sceneMgr.RootSceneNode.CreateChildSceneNode(radarDome.Name + "Node", position + new Vector3(size.x * 0.5f, 0, 0));
                radarNode.AttachObject(radarDome);
                SceneNode innerHolderNode = radarNode.CreateChildSceneNode(new Vector3(0,0,1.5f));

                innerHolderNode.AttachObject(adHolder);

                AxisAlignedBox bb = adHolder.BoundingBox;
                newWidth = size.x / (bb.Size.x * 0.87f);
                newHeight = size.y / (bb.Size.y * 0.86f);
                innerHolderNode.Scale(newWidth, newHeight, 1.0f);


                IslandView.initPalm(sceneMgr, radarNode, new Vector3(-9, -21, 12 - 5), true).Roll(Math.HALF_PI);
                IslandView.initPalm2(sceneMgr, radarNode, new Vector3(-5, -20, 11 - 5), true).Roll(Math.HALF_PI);
                IslandView.initPalm2(sceneMgr, radarNode, new Vector3(2, -18, 11 - 5), true).Roll(Math.HALF_PI);
                IslandView.initPalm2(sceneMgr, radarNode, new Vector3(7, -20, 12 - 5), true).Roll(Math.HALF_PI);
                IslandView.initPalm(sceneMgr, radarNode, new Vector3(10, -17, 11 - 5), true).Roll(Math.HALF_PI);
               
                adNodeParent = radarNode.CreateChildSceneNode(-position - new Vector3(size.x * 0.5f, 0, -1.5f));
                adNodeSuper = radarNode;
            }

            AdQuadrangle3D q3d = AdManager.Singleton.AddDynamicAd(sceneMgr, ad.id, position, size, isPersistent);
                
          //  SceneNode adNode = adNodeParent.CreateChildSceneNode();
            
          //  adNode.AttachObject(q3d.ManualObject);
            adNodeParent.AttachObject(q3d.ManualObject);
           
            q3d.SetSceneNodes(adNodeSuper, adNodeParent);
            dynamicAds.Add(q3d);
            C_AD_MAX_DISPLAY_TIMES[dynamicAds.Count - 1] = Mogre.Math.RangeRandom(45.0f, 65.0f);
            

        }



        protected void OnRegisterTile(LevelTile levelTile)
        {
            if (EngineConfig.DisplayBoundingQuadrangles && !(levelTile is OceanTile))
            {
                OnRegisterBoundingQuadrangle(levelTile, sceneMgr);
            }

            if (levelTile is OceanTile)
            {
                if (tempTileViews.Count > 0)
                {
                    LevelTile lastTileView = tempTileViews[tempTileViews.Count - 1].LevelTile;
                    CompositeModelView cmv = null;

                    if (lastTileView is EndIslandTile)
                    {
                        cmv = new IslandView(tempTileViews, framework, sceneMgr.RootSceneNode);
                        BeginIslandTileView beginTileView = (BeginIslandTileView) tempTileViews[0];

                        int count = beginTileView.BackgroundViews.Count;
                        for (int i = 0; i < count; i++)
                        {
                            CompositeModelView bmv = beginTileView.BackgroundViews[i];
                            backgroundViews.Add(bmv);
                        }
                    }
                    else if (lastTileView is EndAircraftCarrierTile)
                    {
                        carrierView = new CarrierView(tempTileViews, framework, sceneMgr.RootSceneNode);
                        cmv = carrierView;
                    }
                    else if (lastTileView is EndShipTile)
                    {
                        cmv = new ShipView(tempTileViews, framework, sceneMgr.RootSceneNode);
                    }

                    if (cmv != null) compositeModelViews.Add(cmv);

                    tempTileViews = new List<TileView>();
                } //else
                {
                    OceanTileView otv = new OceanTileView(levelTile, framework);
                    tileViews.Add(otv);
                    otv.initOnScene(this.sceneMgr.RootSceneNode, levelTile.TileIndex, 1);
                }
            }
            else if (levelTile is IslandTile || levelTile is AircraftCarrierTile || levelTile is ShipTile)
            {
                TileView tileView = null;
                if (levelTile is BeginIslandTile)
                {
                    tileView = new BeginIslandTileView(levelTile, framework);
                }
                else if (levelTile is BeginShipTile)
                {
                    tileView = new BeginShipTileView(levelTile, framework);
                }
                else if (levelTile is MiddleIslandTile)
                {
                    tileView = new MiddleIslandTileView(levelTile, framework);
                }
                else if (levelTile is EndIslandTile)
                {
                    tileView = new EndIslandTileView(levelTile, framework);
                }
                else if (levelTile is MiddleShipTile)
                {
                     tileView = new MiddleShipTileView(levelTile, framework);
                }
                else if (levelTile is EndShipTile)
                {
                    tileView = new EndShipTileView(levelTile, framework);
                }
                else if (levelTile is BarrackTile)
                {
                    tileView = new BarrackTileView(levelTile, framework);
                }
                else if (levelTile is ShipBunkerTile)
                {
                    tileView = new ShipBunkerTileView(levelTile, framework);
                }
                else if (levelTile is BunkerTile)
                {
                    tileView = new BunkerTileView(levelTile, framework);
                }
                
                else if (levelTile is BarrelTile)
                {
                    tileView = new BarrelTileView(levelTile, framework);
                }
                else if (levelTile is AircraftCarrierTile)
                {
                    //tileView = new OceanTileView(levelTile, framework);
                    tileView = new AircraftCarrierTileView(levelTile, framework);

                }

                //Na uzytek LevelView
                tileViews.Add(tileView);

                //Na uzytek CompositeModelView
                tempTileViews.Add(tileView);
            }
        }

        public void OnRegisterSoldier(Soldier soldier, MissionType missionType)
        {
            SoldierView	sv = SoldierView.GetInstance(soldier);
            if(sv == null)
            {
                return; // blad, pula jest pusta!
            }
            
            // strzalka nad glowna genera³a
            if(missionType == MissionType.Assassination)
            {
                if (soldier is General) sv.showArrow(EffectsManager.EffectType.HINT_ARROW);
            }
            soldierViews.Add(sv);
        }

        private int FindSoldierViewIndex(Soldier soldier)
        {
            // TODO: predykaty
            int count = soldierViews.Count;
            for (int i = 0; i < count; i++)
            {
                if (soldierViews[i].Soldier.Equals(soldier)) return i;
            }
            return -1;
        }

        public void OnUnregisterSoldier(Soldier soldier)
        {
            int index = FindSoldierViewIndex(soldier);
            if (index != -1)
            {
                SoldierView soldierView = soldierViews[index];

                soldierViews.Remove(soldierView);
                SoldierView.FreeInstance(soldier, true);
            }
        }

    
        public void OnSoldierPrepareToFire(Soldier soldier, float time)
        {
            int index = FindSoldierViewIndex(soldier);
            if (index == -1) return;
            SoldierView soldierView = soldierViews[index];
            soldierView.PrepareToFire();
        }

        public void OnSoldierEndPrepareToFire(Soldier soldier)
        {
         
           
            int index = FindSoldierViewIndex(soldier);
            if (index == -1) return;
            SoldierView soldierView = soldierViews[index];
            soldierView.Run();
        }

        public void OnKillSoldier(Soldier soldier, Boolean dieFromExplosion, bool scream)
        {
            int index = FindSoldierViewIndex(soldier);
            if (index == -1) return;

            SoldierView soldierView = soldierViews[index];
            if(scream) soldierView.PlaySoldierDeathSound();

            soldierViews.Remove(soldierView);
            dyingSoldierViews.Add(soldierView);

            if (dieFromExplosion)
            {
                if (Math.RangeRandom(0.0f, 1.0f) < 0.7f)
                {
                    soldierView.DieFromExplosion(false);
                }
                else
                {
                    soldierView.DieFromGun(false);
                }
            }
            else
            {
                if (Math.RangeRandom(0.0f, 1.0f) < 0.7f)
                {
                    soldierView.DieFromGun(Math.RangeRandom(0.0f,1.0f) > 0.5f);
                }
                else
                {
                    soldierView.DieFromExplosion(Math.RangeRandom(0.0f, 1.0f) > 0.5f);
                }
            }
        }

        public void OnRegisterAmmunition(Ammunition ammunition)
        {
            if (ammunition is Bomb)
            {
                ammunitionViews.Add(BombView.GetInstance(ammunition));
            }
            else if (ammunition is Rocket)
            {
                ammunitionViews.Add(RocketView.GetInstance(ammunition, framework));
            }

            else if (ammunition is Torpedo)
            {
                ammunitionViews.Add(TorpedoView.GetInstance(ammunition, this));
            }
            else if (ammunition is FlakBullet)
            {
                ammunitionViews.Add(FlakBulletView.GetInstance(ammunition, framework));
            }
            else if (ammunition is BunkerShellBullet)
            {
                ammunitionViews.Add(BunkerShellBulletView.GetInstance(ammunition, framework));
            }
            else if (ammunition is GunBullet)
            {
                AmmunitionView av = GunBulletView.GetInstance(ammunition, framework);
                ammunitionViews.Add(av);
          
                if (EngineConfig.DisplayBoundingQuadrangles)
	            {
	                OnRegisterBoundingQuadrangle(ammunition, sceneMgr);
	            }
                
          		     
            }
            
          
            
            
        }

        public void OnLoopEnemyPlaneEngineSound(EnemyPlaneBase plane)
        {
            EnemyPlaneViewBase pv = (EnemyPlaneViewBase)FindPlaneView(plane);
            if(pv == null) return; // byc moze w levelview jeszcze nie ma tego samolotu
            pv.LoopEngineSound();
        }

        public void OnLoopEnemyPlaneEngineSounds()
        {
            for (int i = 0; i < planeViews.Count; i++)
            {
                if (planeViews[i] is EnemyPlaneViewBase)
                    ((EnemyPlaneViewBase)planeViews[i]).LoopEngineSound();
            }
        }

        public void OnStopPlayingEnemyPlaneEngineSound(EnemyPlaneBase plane)
        {
            EnemyPlaneViewBase pv = (EnemyPlaneViewBase)FindPlaneView(plane);
            if (pv == null) return;
            pv.StopEngineSound();
        }

        public void OnStopPlayingEnemyPlaneEngineSounds()
        {
            for (int i = 0; i < planeViews.Count; i++)
            {
                if (planeViews[i] is EnemyPlaneViewBase)
                    ((EnemyPlaneViewBase)planeViews[i]).StopEngineSound();
            }
        }

        public void OnRegisterPlane(Plane plane)
        {
            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                OnRegisterBoundingQuadrangle(plane, sceneMgr);
            }
            

            if (plane is StoragePlane)
            {
                if (carrierView != null)
                {
                    carrierView.InitStoragePlaneOnCarrier(plane as StoragePlane);
                }
                else
                {
                    // error
                }
            }
            else
            {
                if (plane.IsEnemy)
                {
                    if(plane is EnemyFighter)
                    { 
                        planeViews.Add(new EnemyFighterView(plane, framework, sceneMgr.RootSceneNode));
                    }
                    else 
                    if (plane is EnemyBomber)
                    {
                        planeViews.Add(new EnemyBomberView(plane, framework, sceneMgr.RootSceneNode));
                    }


                }
                else
                {
                    switch (level.UserPlane.PlaneType)
                    {
                       
                        case PlaneType.F4U:
                            playerPlaneView = new F4UPlaneView(plane, framework, sceneMgr.RootSceneNode);
                            break;

                        case PlaneType.B25:
                            playerPlaneView = new B25PlaneView(plane, framework, sceneMgr.RootSceneNode);
                            break;

                        default:
                        case PlaneType.P47:
                            playerPlaneView = new P47PlaneView(plane, framework, sceneMgr.RootSceneNode);
                            break;
                       
                    }
                }
                
                BuildCameraHolders();
            }
        }

        public void OnUnregisterPlane(Plane plane)
        {
          
            // TODO: na razie samoloty sa ukrywane
            if (plane is StoragePlane)
            {
                if (carrierView != null)
                {
                    carrierView.DestoryStoragePlane((plane as StoragePlane).Tile);
                }
                else
                {
                    // error
                }
                return;
            }

            PlaneView p = FindPlaneView(plane);
            if (p != null)
            {
                p.PlaneNode.SetVisible(false);
                if (p is P47PlaneView)
                {
                    (p as P47PlaneView).StopSmokeTrails();
                }
                if(p.MinimapItem !=null) p.MinimapItem.Hide();
            }
            else
            {
                // error
            }
        }
        
		protected  NodeAnimation.NodeAnimation DoFlakExplosion(FlakBullet flak)
        {
	     
			Vector3 pos =
				new Vector3(flak.Position.X + ModelToViewAdjust, flak.Position.Y, Mogre.Math.RangeRandom(-5,5));
					
        
            return EffectsManager.Singleton.Sprite(
               sceneMgr,
               sceneMgr.RootSceneNode,
               pos,               
               new Vector2(GameConsts.FlakBunker.DamageRange, GameConsts.FlakBunker.DamageRange) + ViewHelper.RandomVector2(4),
               EffectsManager.EffectType.FLAK,
               false,
               flak.GetHashCode().ToString()+" "+pos.ToString()
               );

           

		}
		
		/// <summary>
		/// @deprecated
		/// </summary>
		/// <param name="bunker"></param>
		/// <param name="plane"></param>
		/// <param name="planeHit"></param>
        public void OnBunkerFire(BunkerTile bunker, Plane plane, bool planeHit)
        {
            /*PlaneView p = FindPlaneView(plane);
            if (p != null && planeHit)
            {
                // wybuch wokol samolotu
                EffectsManager.Singleton.Sprite(
                   sceneMgr,
                   p.OuterNode,
                   ViewHelper.RandomVector3(5),
                   new Vector2(2, 2) + ViewHelper.UnsignedRandomVector2(5),
                   EffectsManager.EffectType.EXPLOSION1,
                   false,
                   bunker.GetHashCode().ToString()
                   );

            }*/

           

            TileView t = FindTileView(bunker);
            if (t is EnemyInstallationTileView)
            {
                EnemyInstallationTileView tv = (t as EnemyInstallationTileView);
                tv.GunFire();

                // TODO: obracac wybuch razem z dzialkiem
             
            }
        }

        private int FindAmmunitionViewIndex(Ammunition ammunition)
        {
            int count = ammunitionViews.Count;
            for (int i = 0; i < count; i++)
            {
                if (ammunitionViews[i].Ammunition.Equals(ammunition)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Oblicza lokalny wektor (wzgledem outernode) skierowany do gory
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector3 SmokeUpVector(PlaneView p)
        {
            // niestety coœ nie dzia³a w przypadku storage planes. 'Rêcznie' bêd¹ dymiæ do góry.
            if(p.Plane != null && p.Plane is StoragePlane)
            {
                return new Vector3(0,1,0);
            }


            Vector3 smokeUp = Vector3.NEGATIVE_UNIT_Z;
            Quaternion q = smokeUp.GetRotationTo(p.OuterNode._getDerivedOrientation()*Vector3.NEGATIVE_UNIT_Z);
            smokeUp = q*smokeUp;
            smokeUp = new Quaternion(new Degree(90), Vector3.NEGATIVE_UNIT_Z)*smokeUp;
            if (p.Plane.Direction == Direction.Left)
            {
                smokeUp = new Vector3(smokeUp.z, smokeUp.y, -smokeUp.x);
            }
            else
            {
                smokeUp = new Vector3(smokeUp.z, -smokeUp.y, -smokeUp.x);
            }
            return smokeUp;
        }

        public void OnPlaneDestroyed(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if(p == null) return; //error

            EffectsManager.Singleton.Sprite(sceneMgr, p.PlaneNode, new Vector3(0, 0, 0), new Vector2(20, 20),
                                            EffectsManager.EffectType.PLANECRASH, false);

            if (p.IsSmokingSlightly)
            {
                EffectsManager.Singleton.NoSmoke(sceneMgr, p.OuterNode, EffectsManager.SmokeType.LIGHTSMOKE);
                p.IsSmokingSlightly = false;
            }

            p.IsSmokingHeavily = false; // aby updateplaneview nie wylaczyl zaraz dymu
            EffectsManager.Singleton.Smoke(sceneMgr, p.OuterNode, Vector3.ZERO, SmokeUpVector(p));


            // korkoci¹g
            if (p.AnimationMgr.CanStopAnimation && Math.RangeRandom(0.0f, 1.0f) > 0.5)
            {
                p.AnimationMgr.switchToDeathSpin(true, null, null);
            }

            if (p is PlayerPlaneView) 
                EngineConfig.SpinKeys = false;


            p.Smash(); // vertex animation
        }




        public void OnPlaneCrashed(Plane plane, TileKind tileKind)
        {
            PlaneView p = FindPlaneView(plane);
            if(p==null) return; //error
            switch (tileKind)
            {
                case TileKind.Ocean:
                {
                    float adjustSpeedFactor = plane.Speed - 24; //24 predkosc minimalna samolotu
                    float cos = Math.Cos(plane.Angle);
                    float cos2 = Math.Cos(plane.Angle + 0.1f);
                    float adjust = (1.4f + cos*3.8f) + adjustSpeedFactor*cos2*0.2f;
                    Vector3 posView =
                        new Vector3(
                            UnitConverter.LogicToWorldUnits(plane.Position).x +
                            ((plane.Direction == Direction.Left) ? -adjust : adjust), 0.5f, 0);

                    if (!EngineConfig.LowDetails)
                        EffectsManager.Singleton.RectangularEffect(sceneMgr, sceneMgr.RootSceneNode,
                                                                   "Submerge" + plane.GetHashCode(),
                                                                   EffectsManager.EffectType.SUBMERGE, posView,
                                                                   new Vector2(25, 25), Quaternion.IDENTITY, false);
                    EffectsManager.Singleton.WaterImpact(sceneMgr, sceneMgr.RootSceneNode, posView,
                                                         new Vector2(20, 32), false, "");
                }
                break;
                case TileKind.Ship:
                case TileKind.Island:
                case TileKind.AircraftCarrier:
                {
                  
                }
                break;
            }

            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                OnUnregisterBoundingQuadrangle(plane);
            }

            if (p is P47PlaneView)
            {
                (p as P47PlaneView).StopSmokeTrails();
            }

            // jesli model zglosil prosbe o zawracanie to nie nalezy przerywac animacji. UpdatePlaneView musi odpalic animacje zawracania i wyslac komunikat do modelu ze zawracanie rozpoczete
            if (p.AnimationMgr.CanStopAnimation)
            {
                p.AnimationMgr.disableAll();
            }

            if (p is PlayerPlaneView) 
            {
            	 EngineConfig.SpinKeys = false;
            	 OnChangeCamera(0); // przelacz kamere
            }
               


            EffectsManager.Singleton.Smoke(sceneMgr, p.OuterNode, Vector3.ZERO, SmokeUpVector(p));
            p.IsSmokingHeavily = false; // aby updateplaneview nie wylaczylo dymu
        }

        public void OnUnregisterRocket(Rocket rocket)
        {
            OnAmmunitionExplode(null, rocket);
        }

        public void OnUnregisterTorpedo(Torpedo torpedo)
        {
            OnAmmunitionExplode(null, torpedo);
        }
        public void OnUnregisterFlakBullet(FlakBullet flak)
        {
            OnAmmunitionExplode(null, flak);
        }

        public void OnUnregisterBunkerShellBullet(BunkerShellBullet bullet)
        {
            OnAmmunitionExplode(null, bullet);
        }
        
        public void OnUnregisterGunBullet(GunBullet gun)
        {
            OnAmmunitionExplode(null, gun);
        }
        
        

        public void OnEnemyPlaneBombed(Plane plane, Ammunition ammunition)
        {
            // PlaneView p = FindPlaneView(plane);
            OnAmmunitionExplode(null, ammunition);
        }

        /// <summary>
        /// Torpeda trafia w wodê
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="torpedo"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public void OnTorpedoHitGroundOrWater(LevelTile tile, Torpedo torpedo, float posX, float posY)
        {
            int index = FindAmmunitionViewIndex(torpedo);
            if (index == -1) return;
            AmmunitionView av = ammunitionViews[index];
            uint hash = (uint) tile.GetHashCode();
            
            SceneNode splashNode = getSplashNode();
            if (splashNode == null) return; // koniec poola

            // plusk
            NodeAnimation.NodeAnimation na;
           
            bool ocean = false;
            if (tile is OceanTile)
            {
                ocean = true;
            }
            EffectsManager.EffectType type;
            if (ocean)
            {
                type = EffectsManager.EffectType.WATERIMPACT2;
            }
            else
            {
                type = EffectsManager.EffectType.DIRTIMPACT1;
            }
           
            Vector3 position =
                new Vector3(posX + ModelToViewAdjust, posY, 0);

            na =
               EffectsManager.Singleton.RectangularEffect(sceneMgr, splashNode, type.ToString(), type, position,
                                                          new Vector2(4, 4), Quaternion.IDENTITY, false);
            na.FirstNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            na.FirstNode.Orientation *= new Quaternion(Math.RangeRandom(-0.1f, 0.1f) * Math.HALF_PI, Vector3.UNIT_Y);
 
            na.onFinishInfo = na.Nodes;
            na.onFinish = onFreeSplashNode;    
        }

        public void OnAmmunitionExplode(LevelTile tile)
        {
            SceneNode splashNode = getSplashNode();
            if (splashNode == null) return; // koniec poola
            
            Vector3 position =
               new Vector3(Mathematics.IndexToPosition(tile.TileIndex) + ModelToViewAdjust, (tile.YBegin + tile.YEnd) / 2.0f, 0);

            NodeAnimation.NodeAnimation na;
            na = EffectsManager.Singleton.Sprite(
                   sceneMgr,
                   splashNode,
                   position,
                   new Vector2(4, 4) + ViewHelper.UnsignedRandomVector2(5),
                   EffectsManager.EffectType.EXPLOSION2,
                   false,
                   splashNode.GetHashCode().ToString()
                   );

            na.onFinishInfo = na.Nodes;
            na.onFinish = onFreeSplashNode;

          
        }

        public void OnAmmunitionVanish(LevelTile tile, Ammunition ammunition)
        {
            int index = FindAmmunitionViewIndex(ammunition);
            if (index == -1)
            {
                return;
            }
            AmmunitionView av = ammunitionViews[index];
            ammunitionViews.RemoveAt(index);
            av.Hide();

            if (EngineConfig.DisplayBoundingQuadrangles && ammunition != null)
            {
                OnUnregisterBoundingQuadrangle(ammunition);
            }

            if (av is RocketView)
            {
                RocketView.FreeInstance(ammunition);
            }
            else if (av is TorpedoView)
            {
                TorpedoView.FreeInstance(ammunition);
            } else if (av is FlakBulletView)
            {
                FlakBulletView.FreeInstance(ammunition);
            }
            else if (av is BunkerShellBulletView)
            {
                BunkerShellBulletView.FreeInstance(ammunition);
            } 
            else if (av is GunBulletView)
            {
                GunBulletView.FreeInstance(ammunition);
            } else
            {
                BombView.FreeInstance(ammunition);
            }
            if (EngineConfig.ExplosionLights && IsNightScene) 
            {
            
            	av.ExplosionFlash.Visible = false;
            }
        }
        
      
		
		
        /// <summary>
        /// Wybuch pocisku.
        /// </summary>
        /// <param name="tile">Tile na ktorych nastepuje wybuch. Wartosc moze byc null'em</param>
        /// <param name="ammunition"></param>
        public void OnAmmunitionExplode(LevelTile tile, Ammunition ammunition)
        {
            if (EngineConfig.DisplayBoundingQuadrangles && ammunition != null)
            {
                OnUnregisterBoundingQuadrangle(ammunition);
            }
            if (ammunition == null)
            {
                OnAmmunitionExplode(tile);
                return;
            }

            int index = FindAmmunitionViewIndex(ammunition);
           
            if (index == -1)
            {
              //  Console.WriteLine("HOOOLAA!");
                return;
            }
            AmmunitionView av = ammunitionViews[index];

            uint hash;
            bool ocean = false;

            if (tile != null)
            {
                hash = (uint) tile.GetHashCode();
                if (tile is OceanTile) ocean = true;
            }
            else
            {
                hash = 1;
            }

            NodeAnimation.NodeAnimation na;
            EffectsManager.EffectType effectType;

            if (ocean)
            {
                effectType = EffectsManager.EffectType.WATERIMPACT2;
                na = EffectsManager.Singleton.Sprite(
                    sceneMgr,
                    av.AmmunitionNode,
                    new Vector3(0, 0.5f, (ammunition.Direction == Direction.Left) ? -1.55f : -1.65f),
                    new Vector2(3, 3) + ViewHelper.UnsignedRandomVector2(5),
                    effectType,
                    false,
                    hash.ToString()
                    );
            }
            else
            {
            	if(ammunition is FlakBullet) {
                    effectType = EffectsManager.EffectType.FLAK;
            		na =  DoFlakExplosion(ammunition as FlakBullet);
            		
            	} else if(ammunition is GunBullet) {

                    effectType = EffectsManager.EffectType.DEBRIS;
            		na = EffectsManager.Singleton.Sprite(
                    sceneMgr,
                    av.AmmunitionNode,
                    new Vector3(0, 0.0f, 0),
                    new Vector2(1, 1),
                    effectType,
                    false,
                    hash.ToString()
                    );
            		
            	} else            	
            	{

                    if (tile == null)
                    {
                        // powietrze
                        effectType = (IsNightScene)
                                         ? EffectsManager.EffectType.EXPLOSION3_NIGHT
                                         : EffectsManager.EffectType.EXPLOSION3;

                        na = EffectsManager.Singleton.Sprite(
                        sceneMgr,
                        av.AmmunitionNode,
                        new Vector3(0, 0.5f, 0),
                        new Vector2(4, 4) + ViewHelper.UnsignedRandomVector2(5),
                        effectType,
                        false,
                        hash.ToString()
                        );
                       

                    }else
            	    if(tile != null && tile is EnemyInstallationTile)
                    {
                        // budynki
                       effectType = EffectsManager.EffectType.EXPLOSION2;
                       na = EffectsManager.Singleton.Sprite(
                       sceneMgr,
                       av.AmmunitionNode,
                       new Vector3(0, 0.5f, 0),
                       new Vector2(8, 8) + ViewHelper.UnsignedRandomVector2(5),
                       effectType,
                       false,
                       hash.ToString()
                       );
                    } else
                    {
                        // ziemia
                      effectType = EffectsManager.EffectType.EXPLOSION4;
                      na = EffectsManager.Singleton.Sprite(
                      sceneMgr,
                      av.AmmunitionNode,
                      new Vector3(0, 0.5f, 0),
                      new Vector2(6, 6) + ViewHelper.UnsignedRandomVector2(5),
                      effectType,
                      false,
                      hash.ToString()
                      );
                    }
                    
                   
            	}
            }
          //  Console.WriteLine("Effect: " + effectType);
            ammunitionViews.RemoveAt(index);
            av.Hide();

            if (!ocean && tile != null && EngineConfig.ExplosionLights && IsNightScene && av.ExplosionFlash != null)
            {
                // TODO: czas œwiecenia taki jak d³ugoœæ efektu EffectsManager.EffectType.EXPLOSION3
                NodeAnimation.NodeAnimation ani =
                    EffectsManager.Singleton.AddCustomEffect(
                        new SinLightAttenuationAnimation(av.ExplosionFlash, 1.0f, 1.0f, Math.PI,
                                                         "LightAnimation" + av.GetHashCode()));
                ani.Enabled = true;
                ani.rewind();
                ani.Looped = false;
                av.ExplosionFlash.Visible = true;
            }
            
            if(na!= null) 
            {

	            na.onFinishInfo = new Object[5];
	            Object[] args = (Object[]) na.onFinishInfo;
	            args[0] = av;
	            args[1] = ocean;
	            args[2] = hash;
	            args[3] = ammunition;
                args[4] = effectType;
	            na.onFinish = onAmmunitionExplodeFinish;
            }
            
            
        }


        private void onShakeFinish(Object o)
        {
            NodeAnimation.NodeAnimation ani = (o as NodeAnimation.NodeAnimation);
            EffectsManager.Singleton.RemoveAnimationSafe(ani);
            ani = null;
        }

        private void onAmmunitionExplodeFinish(Object o)
        {
            Object[] args = (Object[]) o;
            AmmunitionView av = (AmmunitionView) args[0];
            Boolean ocean = (Boolean) args[1];
            uint hash = (uint) args[2];
            Ammunition ammunition = (Ammunition) args[3];


            EffectsManager.EffectType effectType = (EffectsManager.EffectType) args[4];
			
            EffectsManager.Singleton.HideSprite(
                sceneMgr,
                av.AmmunitionNode,
                effectType,
                hash
                );

            if (av is RocketView)
            {
                RocketView.FreeInstance(ammunition);
            }else if (av is TorpedoView)
            {
                TorpedoView.FreeInstance(ammunition);
            }else if (av is FlakBulletView)
            {
                FlakBulletView.FreeInstance(ammunition);
            }
            else if (av is BunkerShellBulletView)
            {
                BunkerShellBulletView.FreeInstance(ammunition);
            }
            else if (av is GunBulletView)
            {
                GunBulletView.FreeInstance(ammunition);
            }
            else
            {
                BombView.FreeInstance(ammunition);
            }
            if (EngineConfig.ExplosionLights && IsNightScene) 
            {
            	NodeAnimation.NodeAnimation ani = EffectsManager.Singleton.GetEffect("LightAnimation" + av.GetHashCode());
            	//EffectsManager.Singleton.RemoveAnimation(ani);
                if(ani!= null)
                {
                    ani.Enabled = false;
                    (ani as LightAttenuationAnimation).Light = null;
                    av.DestroyExplosionFlash();     
                }
            	       	
            }
        }

        public void OnTileRestored(BunkerTile restoredBunker)
        {
            TileView v = FindTileView(restoredBunker);
            if (!(v is BunkerTileView)) return; // error
            (v as BunkerTileView).Restore();
        }

        protected void onUnregisterShip(BeginShipTile tile)
        {
            ShipView sv = FindShipView(tile);
            if (sv == null) return;

            sv.MainNode.SetVisible(false);
            if (sv.MinimapItem != null) sv.MinimapItem.Hide();
        }

        public void OnShipDamaged(ShipTile tile, ShipState state)
        {
            ShipView sv = FindShipView(tile);
            if(sv == null) return;
            sv.OnShipDamaged(state);
        }

        public void OnShipSunk(BeginShipTile tile)
        {
            ShipView sv = FindShipView(tile);
            if (sv == null) return;
            sv.OnShipSunk();
            onUnregisterShip(tile);
        }

		public void OnShipBeginSinking(ShipTile tile)
		{ 
			ShipView sv  = FindShipView(tile);
            
            if(sv == null) return;
            sv.OnShipBeginSinking(tile);
		}
		
        public void OnShipSinking(ShipTile tile)
        {
            ShipView sv  = FindShipView(tile);
            
            if(sv == null) return;
            sv.OnShipSinking(tile);
        }

        public void OnShipBeginSubmerging(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipBeginSubmerging(tile);
        }

        public void OnShipBeginEmerging(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipBeginEmerging(tile);

        }

        public void OnShipSubmerging(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipSubmerging(tile);
        }

        public void OnShipEmerging(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipEmerging(tile);
        }


        public void OnShipEmerged(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipEmerged(tile);
        }

        public void OnShipSubmerged(LevelTile tile)
        {
            ShipView sv = FindShipView(tile);

            if (sv == null) return;
            sv.OnShipSubmerged(tile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="ammunition"></param>
		public void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition)
        {			
			BunkerTileView bunker = (BunkerTileView)FindTileView(tile);
			if(bunker == null)
			{
				return;
			}
			
			bunker.Damage(true);
        }
		

        public void OnTileDestroyed(LevelTile tile)
        {
        	
            if (tile is ConcreteBunkerTile || tile is WoodBunkerTile || tile is ShipBunkerTile || tile is FortressBunkerTile || tile is FlakBunkerTile)
            {
                EnemyInstallationTileView bunker = (EnemyInstallationTileView)FindTileView(tile);
                if (bunker == null) return; // error
                bunker.Destroy();
                
                // hiding of minimap items in case of ship sinking moved to shipview
                /*if(tile is ShipWoodBunkerTile || tile is ShipConcreteBunkerTile)
                {
                	ShipView sv = FindShipView(tile as ShipTile);
                	
                	if(sv != null && sv.TileViews[0] != null &&sv.TileViews[0].LevelTile.IsSinking) bunker.MinimapItem.Hide();
                }*/
            }
            else if (tile is BarrackTile)
            {
                BarrackTileView barrack = (BarrackTileView)FindTileView(tile);
                if (barrack == null) return; // error
                barrack.Destroy();
            }
            else if (tile is BarrelTile)
            {
                BarrelTileView barrel = (BarrelTileView)FindTileView(tile);
                if (barrel == null) return; // error
                barrel.Destroy();
            }

            // shake animation
            if (Math.RangeRandom(0, 1) < 0.2f)
            {
                ShakeNodeAnimation animation = new ShakeNodeAnimation(framework.Camera.ParentSceneNode, 0.25f,
                                                                      new Radian(Math.PI), 0.75f,
                                                                      tile.Name + "Shake");
                animation.Enabled = true;
                animation.rewind();
                animation.Looped = false;
                EffectsManager.Singleton.AddCustomEffect(animation);
                animation.onFinish = onShakeFinish;
                animation.onFinishInfo = animation;
            }
        }

        public void OnWarCry(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null) return;
            if (p is EnemyPlaneViewBase)
            {
                (p as EnemyPlaneViewBase).PlayWarcry();
            }
        }

        public void OnPlanePass(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null) return;
            if (p is PlayerPlaneView)
            {
                (p as PlayerPlaneView).PlayPlanePass();
            }
        }

        public void OnFireGun(IObject2D obj)
        {
        	if(!(obj is Plane)) return;
        	Plane plane = (Plane)obj;

            PlaneView p = FindPlaneView(plane);
            

            if (p == null) return;
            p.OnFireGun();
            
        }

        public void OnGunHitPlane(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null) return; //errro
            for (uint i = 1; i <= 3; i++)
            {
            	String name = EffectsManager.BuildSpriteEffectName(p.OuterNode, EffectsManager.EffectType.DEBRIS, (100 + i).ToString());
                if(name != null)
                {
                    NodeAnimation.NodeAnimation anim = EffectsManager.Singleton.GetEffect(name);
                    if(anim != null && !anim.Ended)
                    {
                        continue; // poprzedni efekt jeszcze jest odtwarzany
                    }
                }

                EffectsManager.Singleton.Sprite(sceneMgr, p.OuterNode,
                                                new Vector3(-5f, -1.5f, -2) +
                                                ViewHelper.UnsignedRandomVector3(5, 1.5f, 4), new Vector2(15, 15),
                                                EffectsManager.EffectType.DEBRIS, false, (100 + i).ToString());
            }
        }

        public void OnBeginSpin(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null)
            {
                // ERROR 
                return;
            }
            p.AnimationMgr.PrepareToSpin = true;

            if (p.AnimationMgr.CurrentAnimation != null)
            {
                p.AnimationMgr.CurrentAnimation.Looped = false;
            }
        }

        public void OnPrepareChangeDirection(Direction newDirection, Plane plane, TurnType turnType)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null)
            {
                // ERROR 
                return;
            }

            if (turnType == TurnType.Airborne)
            {
                p.AnimationMgr.PrepareToChangeDirection = true;
            }
            if (turnType == TurnType.Carrier)
            {
                p.AnimationMgr.PrepareToChangeDirectionOnCarrier = true;
            }

            if (p.AnimationMgr.CurrentAnimation != null)
            {
                p.AnimationMgr.CurrentAnimation.Looped = false;
            }
        }

        private void UpdatePlaneView(PlaneView p, float timeSinceLastFrame)
        {
            p.AnimationMgr.updateTimeAll(timeSinceLastFrame);
            p.AnimationMgr.animateAll();
            p.refreshPosition();

            (p as VertexAnimable).updateTime(timeSinceLastFrame);

            if(p is PlayerPlaneView) p.UpdateCrossHairVisibility(framework.Camera);

            if (p.Plane != null)
            {
           
                // powieszona torpeda
                if (p.Plane.Weapon.SelectWeapon == WeaponType.Torpedo && p.Plane.Weapon.IsTorpedoAvailable && p.Plane.IsNextTorpedoAvailable)
                {
                    p.ShowTorpedo();
                }
                else
                {
                    p.HideTorpedo();
                }
                
                // slad na wodzie
                if (p.IsReadyForLastWaterTrail)
                {
                    SceneNode splashNode = getSplashNode();
                    if (splashNode != null) // koniec poola
                    {
                        NodeAnimation.NodeAnimation na;
                        Vector3 position = new Vector3(p.PlaneNode.Position.x, 0.25f, 0);

                        na =
                            EffectsManager.Singleton.RectangularEffect(sceneMgr, splashNode, "PlaneWaterTrail",
                                                                       EffectsManager.EffectType.PLANEWATERTRAIL,
                                                                       position, new Vector2(5, 3), Quaternion.IDENTITY,
                                                                       false);
                        //na.Node.Orientation = new Quaternion(Mogre.Math.HALF_PI, Vector3.UNIT_X);
                        //na.Node.Orientation *= new Quaternion(Mogre.Math.RangeRandom(-0.2f, 0.2f) * Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                        na.onFinishInfo = na.Nodes;
                        na.onFinish = onFreeSplashNode;
                        p.LastWaterTrailTime = Environment.TickCount;
                    }
                }
               
                // Dym
                if (p.Plane.Oil < p.Plane.MaxOil && p.PlaneNode._getDerivedPosition().y >= 0)
                {
                    // slaby dym
                    if (p.Plane.Oil < (p.Plane.MaxOil*0.9f))
                    {
                        if (!p.IsSmokingSlightly && !p.IsSmokingHeavily)
                        {
                            EffectsManager.Singleton.Smoke(sceneMgr, p.OuterNode, EffectsManager.SmokeType.LIGHTSMOKE,
                                                           Vector3.ZERO, Vector3.UNIT_Z, Vector2.ZERO);
                            p.IsSmokingSlightly = true;
                        }
                    }
                   
                    // mocny dym
                    if (p.Plane.Oil < (p.Plane.MaxOil*0.25f))
                    {
                        if (!p.IsSmokingHeavily) // optymalizacja zeby za kazdym razem nie wywolywac
                        {
                            EffectsManager.Singleton.NoSmoke(sceneMgr, p.OuterNode, EffectsManager.SmokeType.LIGHTSMOKE);
                            EffectsManager.Singleton.Smoke(sceneMgr, p.OuterNode, Vector3.ZERO, Vector3.UNIT_Z);
                            p.SmashPaint();
                         
                            p.IsSmokingHeavily = true;
                            p.IsSmokingSlightly = false;
                        }
                    }
                }
                else
                {
                    // brak dymu
                    if (p.IsSmokingHeavily || p.IsSmokingSlightly)
                    {
                        EffectsManager.Singleton.NoSmoke(sceneMgr, p.OuterNode, EffectsManager.SmokeType.NORMAL);
                        EffectsManager.Singleton.NoSmoke(sceneMgr, p.OuterNode, EffectsManager.SmokeType.LIGHTSMOKE);
                        p.RestorePaint();
                     //   if (!EngineConfig.LowDetails)
                     //       ViewHelper.ReplaceMaterial(p.PlaneEntity, "P47/DestroyedBody", "P47/Body");
                        p.IsSmokingHeavily = false;
                        p.IsSmokingSlightly = false;
                    }
                }
                // smiglo              
                p.AnimationMgr.changeBladeSpeed(p.Plane.AirscrewSpeed);
                if (p is P47PlaneView)
                {
                    if (p.Plane.AirscrewSpeed < 100)
                    {
                        (p as P47PlaneView).SwitchToSlowEngine();
                    }
                    else
                    {
                        (p as P47PlaneView).SwitchToFastEngine();
                    }
                }

                // smuga dymu
                if (p is P47PlaneView)
                {
                    P47PlaneView p47 = (p as P47PlaneView);
                    if (!p47.HasSmokeTrail && p47.ShouldHaveSmokeTrail)
                    {
                        p47.StartSmokeTrails();
                    }
                    if (p47.HasSmokeTrail && !p47.ShouldHaveSmokeTrail)
                    {
                        p47.StopSmokeTrails();
                    }
                }
            }

            // rozpocznij obracanie z 'pleców na brzuch' jeœli zakolejkowany i zakoñczy³a siê poprzednia animacja
            if (p.AnimationMgr.PrepareToSpin)
            {
                // jesli samolot jest juz rozbity to nie ma co go obracaæ
                if (p.Plane.PlaneState == PlaneState.Crashed)
                {
                    p.AnimationMgr.PrepareToSpin = false;
                }
                else if (p.AnimationMgr.CurrentAnimation.Ended) // zacznij obracaæ jak zakoñczysz stara animacje
                {
                    p.AnimationMgr.switchToSpin(true, null, controller.OnSpinEnd, p.Plane , true);
                    p.AnimationMgr.PrepareToSpin = false;
                } else
                {
                	// animacja sie nie zakonczyla, chcemy ja przyspieszyc
                	p.AnimationMgr.CurrentAnimation.TimeScale = 3.5f;
                }
            }
          

            // jeœli zakoñczy³ siê obrót powróæ do IDLE
            if (p.AnimationMgr.isCurrentAnimation(PlaneNodeAnimationManager.AnimationType.SPIN_PHASE_TWO))
            {
                if (p.AnimationMgr.CurrentAnimation == null || p.AnimationMgr.CurrentAnimation.Ended ||
                    !p.AnimationMgr.CurrentAnimation.Enabled)
                {
                    if(EngineConfig.UseAlternativeSpinControl)
                    {
                        EngineConfig.SpinKeys = true;
                    }
                    p.AnimationMgr.switchToIdle(true);
                }
            }

            // rozpocznij zakrêcanie jeœli zakolejkowano i zakoñczy³a siê poprzednia animacja.
            if (p.AnimationMgr.PrepareToChangeDirection)
            {
                // jesli samolot jest juz rozbity to nie ma co go zawracac
                if (p.Plane.PlaneState == PlaneState.Crashed)
                {
                    p.AnimationMgr.PrepareToChangeDirection = false;
                }
                else if (p.AnimationMgr.CurrentAnimation == null || p.AnimationMgr.CurrentAnimation.Ended) // zacznij zawracac jak zakonczysz stara animacje
                {
                    p.AnimationMgr.switchToTurn(true, controller.OnPrepareChangeDirectionEnd,
                                                controller.OnChangeDirectionEnd);
                    p.AnimationMgr.PrepareToChangeDirection = false;
                } else
                {
                	// animacja sie nie zakonczyla, chcemy ja przyspieszyc                	
                	p.AnimationMgr.CurrentAnimation.TimeScale = 3.5f;
                	
                }
            }
            // jeœli zakoñczy³ siê obrót powróæ do IDLE
            if (p.AnimationMgr.isCurrentAnimation(PlaneNodeAnimationManager.AnimationType.OUTERTURN))
            {
            	
            	// bajer - wirtualny ruch samolotu prostopadle do toru lotu podczas zawracania
            	if(p.AnimationMgr.CurrentAnimation != null) {
	            	float amount = p.AnimationMgr.CurrentAnimation.getPercent();
	            	float translateAmount = (float)Math.Sin( amount * 2 * Math.PI) * timeSinceLastFrame * 10.0f;
	            	translateAmount /= p.AnimationMgr.CurrentAnimation.Duration; // normalizacja - inaczej im dluzszy obrot tym wieksze wychylenie	            	
	            	int dir =  p.Plane.Direction == Direction.Right ? -1 : 1;	            	
	            	p.OuterSteeringNode.Translate(0, 0, translateAmount * dir);
	               // Console.WriteLine("Translate: "+translateAmount);
	                
            	}
            	
                if (p.AnimationMgr.CurrentAnimation == null || p.AnimationMgr.CurrentAnimation.Ended ||
                    !p.AnimationMgr.CurrentAnimation.Enabled)
                {
                    p.AnimationMgr.switchToIdle(true);
                    p.OuterSteeringNode.Position = new Vector3(p.OuterSteeringNode.Position.x, p.OuterSteeringNode.Position.y, 0); // wyzeruj po powyzszej animacji
                }
            }

            // analogicznie do p.AnimationMgr.PrepareToChangeDirection
            if (p.AnimationMgr.PrepareToChangeDirectionOnCarrier)
            {
                if (p.Plane.PlaneState == PlaneState.Crashed)
                {
                    p.AnimationMgr.PrepareToChangeDirectionOnCarrier = false;
                }
                else if (p.AnimationMgr.CurrentAnimation == null || p.AnimationMgr.CurrentAnimation.Ended ||
                         !p.AnimationMgr.CurrentAnimation.Enabled)
                {
                    p.AnimationMgr.switchToTurnOnCarrier(true, controller.OnPrepareChangeDirectionEnd,
                                                         controller.OnChangeDirectionEnd);
                    p.AnimationMgr.PrepareToChangeDirectionOnCarrier = false;
                }
            }
            // jeœli zakoñczy³ siê obrót powróæ do IDLE
            if (p.AnimationMgr.isCurrentAnimation(PlaneNodeAnimationManager.AnimationType.TURN_ON_CARRIER))
            {
                if (p.AnimationMgr.CurrentAnimation.Ended)
                {
                    if (p.Plane != null && !p.Plane.IsOnAircraftCarrier) p.AnimationMgr.switchToIdle(true);
                }
            }
        }

        public void NextLife()
        {
        	if(playerPlaneView == null ) return; // wingitor
        	
            if (playerPlaneView.AnimationMgr.PrepareToChangeDirection)
            {
                Console.WriteLine("BUG - view nie odes³a³ komunikatu");
            }
            if(!GameConsts.GenericPlane.CurrentUserPlane.GodMode) carrierView.RemoveNextStoragePlane();
            carrierView.CrewStatePlaneOnCarrier();
            playerPlaneView.Restore();

            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                OnRegisterBoundingQuadrangle(playerPlaneView.Plane, sceneMgr);
            }
        }

        /// <summary>
        /// Rejestruje czworokat ktory rysowany jest w view (HELPER)
        /// </summary>
        /// <param name="q">obiekt implementuj¹cy IRenderableQuadrangles</param>
        public static void OnRegisterBoundingQuadrangle(IRenderableQuadrangles q, SceneManager sceneMgr)
        {
            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                ViewHelper.AttachQuadrangles(sceneMgr, q);
            }
        }

        /// <summary>
        /// Odrejestruje czworokat ktory rysowany jest w view (HELPER)
        /// </summary>
        /// <param name="q">obiekt implementuj¹cy IRenderableQuadrangles</param>
        public void OnUnregisterBoundingQuadrangle(IRenderableQuadrangles q)
        {
            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                ViewHelper.DetachQuadrangles(sceneMgr, q);
            }
        }
        public void OnFrameEnded(FrameEvent evt)
        {
          
        }

        private float dynamicAdsTimer = 0;


        private void HandleDynamicAds(FrameEvent evt)
        {
            dynamicAdsTimer += evt.timeSinceLastFrame;


            // reklamy ktore juz zostaly pokazane (i uplynal odpowiedni czas) sa chowane
            for (int i =0; i < dynamicAds.Count; i++) 
            {
                
                AdQuadrangle3D ad = dynamicAds[i];
                if (ad.IsPersistent) continue;
               
                if (ad.WasShown)
                {
                    if (dynamicAdsTimer > C_AD_MAX_DISPLAY_TIMES[i])
                    {
                        SceneNode holder = ad.GetParent();
                        Vector3 pos = Vector3.ZERO;
                        if (holder != null)
                        {
                            pos = holder._getDerivedPosition();
                        }
                       
                        // zatop

                        if (holder != null)
                        {

                            if (pos.y > -60)
                            {
                                if (!EngineConfig.LowDetails)
                                {
                                    
                                    // ponad woda
                                    string name;
                                    EffectsManager.EffectType type;
                                    if (((uint) ad.GetHashCode() + i)%2 == 0)
                                    {
                                        type = EffectsManager.EffectType.EXPLOSION2_SLOW;
                                    }
                                    else
                                    {
                                        type = EffectsManager.EffectType.EXPLOSION1_SLOW;
                                    }
                                    for (uint j = 0; j < 3; j++)
                                    {

                                        name = EffectsManager.BuildSpriteEffectName(sceneMgr.RootSceneNode, type,
                                                                                    (ad.GetHashCode() + j).ToString());
                                        if (!EffectsManager.Singleton.EffectExists(name))
                                        {
                                            if (Math.RangeRandom(0, 1) > 0.8f)
                                            {
                                                EffectsManager.Singleton.Sprite(sceneMgr, sceneMgr.RootSceneNode,
                                                                                pos +
                                                                                ViewHelper.RandomVector3(15, 0, 5),
                                                                                new Vector2(25, 25) +
                                                                                ViewHelper.RandomVector2(5, 5),
                                                                                type, false,
                                                                                (ad.GetHashCode() + j).ToString());

                                            }

                                        }
                                    }
                                    pos.y = 0;
                                    ShipView.SinkingWaterAnimation(sceneMgr, pos, "AdWave" + ad.GetBillboardId(), 4,
                                                                   new Vector2(25, 25), new Vector2(40, 40));

                                }
                                holder.Translate(0, -evt.timeSinceLastFrame*6.0f, 0);
                            }

                            if (!ad.DecreaseOpacity(evt.timeSinceLastFrame*0.15f) && pos.y < -60)
                            {
                                //AdManager.Singleton.RemoveDynamicAd(ad);
                                holder.SetVisible(false);
                                ad.ManualObject.Visible = false;
                                dynamicAds.Remove(ad);

                            
                            }


                        }
                    }
                }
                else
                {
                    if (AdManager.Singleton.IsDynamicAdVisible(ad))
                    {
                        ad.WasShown = true;
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt"></param>
        public void OnFrameStarted(FrameEvent evt)
        {
         

            HandleDynamicAds(evt);
            
            if (EngineConfig.UseHydrax)
            {
                HydraxManager.Singleton.Update(evt);
            }

            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                ViewHelper.RefreshBoundingQuandrangles();
            }

            EffectsManager.Singleton.UpdateTimeAndAnimateAll(evt.timeSinceLastFrame);

            PlayerPlaneView p = playerPlaneView;
            int count = planeViews.Count;
            for (int i = 0; i < count; i++)
            {
                UpdatePlaneView(planeViews[i], evt.timeSinceLastFrame);
            }
            if (p != null) UpdatePlaneView(p, evt.timeSinceLastFrame);

            // tileviews
            Animable va;
            count = tileViews.Count;
            for (int i = 0; i < count; i++)
            {
                if (tileViews[i] is Animable)
                {
                    va = tileViews[i] as Animable;
                    va.updateTime(evt.timeSinceLastFrame);
                }
            }

            // soldiers
            SoldierView sv;
            count = soldierViews.Count;
            for (int i = 0; i < count; i++)
            {
                sv = soldierViews[i];
                sv.updateTime(evt.timeSinceLastFrame);
                sv.refreshPosition();
            }

            count = dyingSoldierViews.Count;
            for (int i = 0; i < count; i++)
            {
                sv = dyingSoldierViews[i];
                sv.updateTime(evt.timeSinceLastFrame);
                sv.refreshPosition();

                if (sv.IsAnimationFinished() && !EngineConfig.BodiesStay)
                {
                    dyingSoldierViews.Remove(sv);
                    i--;
                    count--;
                    SoldierView.FreeInstance(sv.Soldier, !EngineConfig.BodiesStay);
                }
            }

            // ammo
            AmmunitionView ammunitionView;
            count = ammunitionViews.Count;
            for (int i = 0; i < count; i++)
            {
                ammunitionView = ammunitionViews[i];
                ammunitionView.updateTime(evt.timeSinceLastFrame);
                ammunitionView.refreshPosition();
            }

            //Carrier
          //  if (carrierView != null)
          //  {
          //      carrierView.updateTime(evt.timeSinceLastFrame);
          //  }

            foreach(CompositeModelView cv in this.compositeModelViews)
            {
                if(cv is ShipView)
                {
                    (cv as ShipView).refreshPosition();
                }

                if(cv is VertexAnimable)
                {
                    (cv as VertexAnimable).updateTime(evt.timeSinceLastFrame);
                }
            }

            count = backgroundViews.Count;
            for (int i = 0; i < count; i++)
            {
                CompositeModelView cmv = backgroundViews[i];
                if (cmv is CarrierView)
                {
                    CarrierView cv = (CarrierView) cmv;
                    cv.updateTime(evt.timeSinceLastFrame);
                }
            }
        }

        public void OnStartHangaring(int hangaringDirection, bool changeCam)
        {
            if (changeCam)
            {
                cameraIndexBeforeHangaring = OnChangeCamera(this.playerPlaneView.HangaringCameraHolder);
            }
           
            this.carrierView.StartHangaringPlane(this.playerPlaneView, hangaringDirection);
        }

        public bool IsHangaringFinished()
        {
            return this.carrierView.IsHangaringFinished();
        }

        public void OnHangaringFinished()
        {
            OnChangeCamera(cameraIndexBeforeHangaring);    
            this.carrierView.ResetHangaringFinished();
        }

        public void OnRegisterLevel(Level level)
        {
            this.level = level;
			InitOceanSurface();
            InitSkies();
            

            List<LevelTile> lvlTiles = level.LevelTiles;

            modelToViewAdjust = -(lvlTiles.Count/2)*TileWidth;
            int totalTilesNumber = lvlTiles.Count;

            for (int i = 0; i < totalTilesNumber; i++)
            {
                OnRegisterTile(lvlTiles[i]);
            }

            tileViews.ToString();

            BombView.InitPool(40, framework);
            RocketView.InitPool(80, framework);
            FlakBulletView.InitPool(40, framework);
            BunkerShellBulletView.InitPool(40, framework);
            GunBulletView.InitPool(200, framework);
            TorpedoView.InitPool(10, framework);
            SoldierView.InitPool(80, framework);

            InitSplashPool(350);
            // Quaternion q = new Quaternion();
            // q.FromAngleAxis(new Degree(90), Vector3.UNIT_Y);
            // EffectsManager.Singleton.RectangularEffect(sceneMgr, sceneMgr.RootSceneNode, "asd", EffectsManager.EffectType.WATERTRAIL, new Vector3(-100,0,0), new Vector2(20,20), q, true);
            //Console.WriteLine("Level loaded");
        }

        public void OnEndLoadingLevel()
        {
        }

        public void InitOceanSurface()
        {
            // OCEAN 
           
            // minimap
            if (EngineConfig.DisplayingMinimap)
            {
                SceneNode mOceanNode = minimapMgr.RootSceneNode.CreateChildSceneNode("MinimapOceanNode");
                Entity mOcean = sceneMgr.CreateEntity("MinimapOcean", "TwoSidedPlane.mesh");
                mOcean.SetMaterialName("Minimap/Ocean");
                mOcean.CastShadows = false;
                mOcean.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_WORLD_GEOMETRY_1;
                mOceanNode.AttachObject(mOcean);
                mOceanNode.Position = new Vector3(0, -10, 0);
                mOceanNode.SetScale(oceanSize, 1, 5);
                mOceanNode.Pitch(new Degree(90));
            }

            if(EngineConfig.UseHydrax)
            {
             
                string config = "";
                switch (level.DayTime)
                {
                    case DayTime.Foggy:
                        config = "Foggy.hdx";
                    break;

                    case DayTime.Night:
                    config = "Night.hdx";
                    break;

                    default:
                        config = "Tropical.hdx";
                    break;
                        
                }
				try
				{
					HydraxManager.Singleton.CreateHydrax(config, sceneMgr, framework.Camera, framework.Viewport);
                	HydraxManager.Singleton.AddHydraxDepthTechniques();
                	//  MHydrax.MDecal d = hydrax.DecalsManager.Add("Rosette.png");
          			//  d.Position = new Vector2(120, 120);
          			
                	return;
				}
				catch(Exception ex)
            	{
            		LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Exception while creating hydrax, using old style water. " + ex.Message + " "+ ex.StackTrace);
            	}
  
            }
          

            Entity ocean2 = sceneMgr.CreateEntity("Ocean2", "OceanPlane.mesh");
            ocean2.CastShadows = false;
            if(EngineConfig.ShadowsQuality > 0) 
            {
            	ocean2.SetMaterialName("Ocean2_HLSL");
            }
            else 
            {
            	ocean2.SetMaterialName("Ocean2_HLSL_NoShadows");
            }
            
            // ocean jest uniesiony z poziomu shadera (aby dzia³a³y cienie), teraz trzeba go opuscic
            sceneMgr.RootSceneNode.CreateChildSceneNode("OceanNode", new Vector3(0,0,0)).AttachObject(ocean2);
            
            //sceneMgr.RootSceneNode.AttachObject( ocean2);
            // OCEAN
        }

        private void InitSkies()
        {
            // Set the material
            ColourValue ambient = new ColourValue(0.5f, 0.5f, 0.5f);

            Mogre.Plane skyPlane;
            skyPlane.normal = Vector3.UNIT_Z;
            skyPlane.d = 210;

            if (EngineConfig.DisplayingMinimap)
            {
                minimapMgr.SetSkyPlane(true, skyPlane, "MiniMap/Sky", 10.0f, 7 );

            }


            // zmienne odbicie w wodzie
            string texture = "morning.jpg";
            string texture_low = "morning_low.jpg";
            string material = "Skyplane/Morning";
            material = "Skybox/Morning";

            switch (level.DayTime)
            {
                case DayTime.Noon:
                    material = "Skyplane/Noon";
                    material = "Skybox/Noon";
                    texture = "cloudy_noon.jpg";
                    texture_low = "cloudy_noon_low.jpg";
                    InitLight();
                    break;
                    
               
                case DayTime.Foggy:
                    material = "Skyplane/Foggy";
                    material = "Skybox/Foggy";
                    texture = "foggy.jpg";
                    texture_low = "foggy_low.jpg";
                    InitLight();
                    break;

                case DayTime.Dawn:
                    material = "Skyplane/Morning";
                    material = "Skybox/Morning";
                    texture = "morning.jpg";
                    texture_low = "morning_low.jpg";
                    InitDawnLight();
                    break;
                    
                 case DayTime.Dawn2:
                    material = "Skyplane/Morning2";
                    material = "Skybox/Morning2";
                    texture = "morning2.jpg";
                    texture_low = "morning2_low.jpg";
                    InitLight();
                    break;
                    
 				 case DayTime.Dusk:                   
                    material = "Skybox/Dusk";
                    texture = "dusk.jpg";
                    texture_low = "dusk_low.jpg";
                    InitLight();
                    break;

 				case DayTime.Dusk2:                   
                    material = "Skybox/Dusk2";
                    texture = "dusk2.jpg";
                    texture_low = "dusk2_low.jpg";
                    InitLight();
                    break;
                    
                case DayTime.Night:
                    material = "Skyplane/Night";
                    material = "Skybox/Night";
                    texture = "night.jpg";
                    texture_low = "night_low.jpg";
                    ambient = new ColourValue(0.20f, 0.20f, 0.27f);
                    InitNightLight();
                    isNightScene = true;
                    break;
            }
            MaterialPtr m;
            if(EngineConfig.ShadowsQuality > 0) 
            {
            	m = MaterialManager.Singleton.GetByName("Ocean2_HLSL");
            }
            else 
            {
            	m = MaterialManager.Singleton.GetByName("Ocean2_HLSL_NoShadows");
            }
            m.Load(false);
            try
            {
	            Pass p = m.GetBestTechnique().GetPass("Decal");
	            TextureUnitState tu = null;
	            if(p!= null)
	            {
	            	 tu = p.GetTextureUnitState("Reflection");
	            }
	           
	            
	            if (tu != null)
	            {
	                tu.SetCubicTextureName(texture, true);
	            }
	            if (p.HasFragmentProgram)
	            {
	                GpuProgramParametersSharedPtr param = p.GetVertexProgramParameters();
	                param.SetNamedConstant("bumpSpeed", new Vector3(0.02f, -0.03f, 0));
	                p.SetVertexProgramParameters(param);
	              
	            }
            }
            catch(Exception ex)
            {
            	
            }


            // environment mapping materials

            try
            {

                foreach (String name in new String[] { "B25/Body", "B25/BodyDestroyed", "SteelEnv" })
                {
                    //set_texture_alias
                    m = MaterialManager.Singleton.GetByName(name);
                    m.Load(false);

                    Pass p = m.GetBestTechnique().GetPass(0);
                    TextureUnitState tu = null;
                    if (p != null)
                    {
                        tu = p.GetTextureUnitState(1);
                        tu.SetCubicTextureName(texture_low);
                    }
                }
                
            }
            catch (Exception)
            {
                
               
            }
            
            m = null;
            ColourValue fogColor = new ColourValue(0.9f, 0.9f, 0.9f);
            // fog
            if (level.DayTime == DayTime.Foggy)
            {
                fogColor = new ColourValue(0.8f, 0.8f, 0.8f);
                sceneMgr.SetFog(FogMode.FOG_LINEAR, fogColor, 0.00f, 480, 1800);
            }
            else if (level.DayTime == DayTime.Night)
            {
                fogColor = new ColourValue(0.4f, 0.4f, 0.4f);
                sceneMgr.SetFog(FogMode.FOG_LINEAR, fogColor - new ColourValue(0.2f,0.2f,0.2f), 0.000f, 600, 3600);

            } else
            {
                sceneMgr.SetFog(FogMode.FOG_LINEAR, fogColor, 0.000f, 300, 3600);

            }

          
            sceneMgr.SetSkyBox(true, material, 3000, true);
            sceneMgr.AmbientLight = ambient;
          //  sceneMgr.SetFog(FogMode.FOG_NONE);

          
            // mewy
            if (!EngineConfig.LowDetails)
            {
                EffectsManager.Singleton.AddSeagulls(sceneMgr, new Vector3(0, 150, -1500), new Vector2(20, 25),
                                                     new Degree(10), 20, 10);
            }
            bool lighterClouds = (level.DayTime == DayTime.Dawn2);
            float visibility = 1.0f;
            
            ColourValue colour = ColourValue.White;
            if(level.DayTime == DayTime.Night)
            {
            	//visibility = 0.3f;
            	colour = new ColourValue(0.247f, 0.211f, 0.145f);
            }
	
            // chmury
            int cloudsets = 8;
            int currentX = (int) (-oceanSize/1.8f);
            int currentZ = (int) (-oceanSize/1.8f);
             
            for (int i = -cloudsets; i < cloudsets; i += 2)
            {
                currentX += (oceanSize/cloudsets);
                currentZ += (oceanSize/cloudsets);

                if (!EngineConfig.LowDetails)
                    EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, 100, -600), new Vector2(150, 50),
                                                       new Degree(5), 10, lighterClouds, Quaternion.IDENTITY, visibility * 0.75f, colour);

                float cloudDist;

                cloudDist = -4200;
                EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, -100, cloudDist),
                                           new Vector2(5500, 400) + ViewHelper.RandomVector2(1000, 100),
                                           new Degree(5), 5, lighterClouds, Quaternion.IDENTITY, visibility, colour);
                
                
                
                Quaternion q = new Quaternion(new Radian(Math.HALF_PI), Vector3.UNIT_Y);
                
                EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, -100, cloudDist),
                                           new Vector2(5500, 400) + ViewHelper.RandomVector2(1000, 100),
                                           new Degree(5), 5, lighterClouds, q, visibility, colour);
                
                Quaternion q2 = new Quaternion(new Radian(-Math.HALF_PI), Vector3.UNIT_Y);
                
                EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, -100, cloudDist),
                                           new Vector2(5500, 400) + ViewHelper.RandomVector2(1000, 100),
                                           new Degree(5), 5, lighterClouds, q2, visibility, colour);
                

                if (level.DayTime == DayTime.Foggy && !EngineConfig.LowDetails)
                {

                   
                    Quaternion q3 = new Quaternion(new Radian(0.0001f), Vector3.UNIT_Y);
                    cloudDist = -700; // heavy clouds
                    EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, 10, cloudDist),
                                               new Vector2(5000, 400) + ViewHelper.RandomVector2(1000, 100),
                                               new Degree(1), 5, lighterClouds, q3, visibility * 0.2f, colour);

                }
                

              
                // nad samolotem (niebo)
                EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(currentX, 170, -150), new Vector2(500, 200), 0, 1, lighterClouds, Quaternion.IDENTITY, visibility, colour);
            }
        }

        private void InitDawnLight()
        {
            // create a default point light
            Light light = sceneMgr.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
           
            light.Direction = new Vector3(3, -1, -0.5f);
            light.Direction.Normalise();
            light.DiffuseColour = new ColourValue(0.95f, 0.90f, 0.90f);
            light.SpecularColour = new ColourValue(0.05f, 0.05f, 0.05f);
          
            sceneMgr.ShadowColour = new ColourValue(0.8f, 0.8f, 0.8f);
        }

        private void InitLight()
        {
        
            // create a default point light
            Light light = sceneMgr.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
        //    light.Type = Light.LightTypes.LT_SPOTLIGHT;
         //   light.SetSpotlightRange(new Degree(60),new Degree(90));		  
            
         
            Vector3 dir = new Vector3(2.8f, -1.8f, 3.0f);
            dir.Normalise();
            light.SetDirection(dir.x, dir.y, dir.z);
            light.DiffuseColour = new ColourValue(0.50f, 0.50f, 0.52f);
            light.SpecularColour = new ColourValue(0.1f, 0.1f, 0.1f);
            
            light.CastShadows = true;
            
             /*
            BillboardSet lightbillboardset =
                sceneMgr.CreateBillboardSet("_lights" , 1);
            lightbillboardset.MaterialName = "Examples/Flare";
            Billboard lightbillboard = lightbillboardset.CreateBillboard(lpos);
            lightbillboard.SetDimensions(39, 39);
            sceneMgr.RootSceneNode.AttachObject(lightbillboardset);
            sceneMgr.RootSceneNode.AttachObject(light);
			*/
/*
            Camera texCamera = new Camera("TexCamera", sceneMgr);
            LiSPSMShadowCameraSetup c = new LiSPSMShadowCameraSetup();
            c.GetShadowCamera(sceneMgr, framework.Camera, framework.Viewport, light, texCamera, 2);
            ShadowCameraSetupPtr p = new ShadowCameraSetupPtr(c);
            sceneMgr.SetShadowCameraSetup(p);
  */         
             sceneMgr.ShadowColour = new ColourValue(0.5f, 0.5f, 0.5f);
        }

        private void InitNightLight()
        {
            // create a default point light
            Light light = sceneMgr.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
        //    light.Position = new Vector3(0, 0, 0);
            light.Direction = new Vector3(2, -5, 3);
            light.Direction.Normalise();
            light.DiffuseColour = new ColourValue(0.6f, 0.6f, 0.7f);
            light.SpecularColour = new ColourValue(0.00f, 0.00f, 0.00f);

          //  sceneMgr.ShadowColour = new ColourValue(0.65f, 0.65f, 0.75f);
        }

        /// <summary>
        /// Zdarzenie jest wyzwalane przez kontroler w celu zmiany
        /// stanu podwozia samolotu przekazanego jako parametr metody.
        /// Zdarzenie moze oznaczac tak otwieranie, jak i skladanie podwozia.
        /// Rodzaj animacji jest wybierany na podstawie zmiennej opisujacej
        /// aktualny stan podwozia (mozliwe stany podwozia ToggleOut - nalezy
        /// otworzyc podwozie, ToggleIn - nalezy zwinac podwozie)
        /// 
        /// Po zakonczeniu animacji zmiany stanu podwozia, przekazywany jest
        /// odpowiedni event do kontrolera.
        /// </summary>
        /// <param name="plane">
        ///     Samolot, w ktorym nalezy zmienic stan podwozia.
        /// </param>
        /// <author>Jakub Tezycki</author>
        public void OnToggleGear(Plane plane)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null) return;
            p.AnimationMgr.switchToGearUpDown(false, null, controller.OnGearToggleEnd);
            p.AnimationMgr.CurrentAnimation.onFinishInfo = plane;
        }

        /// <summary>
        /// Metody wywolywana gdy pocisk z dzialka samolotu trafia z ziemie / ocean
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public void OnGunHit(LevelTile tile, float posX, float posY)
        {
			
            if(playerPlaneView  != null && this.playerPlaneView.Plane.LocationState == LocationState.AirTurningRound) return; // przypadek kiedy zawracamy i strzelamy - nie pokazuj sladow na wodzie bo kierunek zmienia sie na koniec obrotu wiec nie wyswietlalo by sie to poprawnie

            SceneNode splashNode = getSplashNode();
            if (splashNode == null) return; // koniec poola
            Boolean ocean = false;
            NodeAnimation.NodeAnimation na, na2;

            if (tile is OceanTile)
            {
                ocean = true;
            }
            EffectsManager.EffectType type;
            if (ocean)
            {
                type = EffectsManager.EffectType.WATERIMPACT2;
            }
            else
            {
                type = EffectsManager.EffectType.DIRTIMPACT1;
            }

            Vector3 position =
                new Vector3(posX + ModelToViewAdjust, ocean ? (float) posY : (float) (posY + 1.0f), 0);

            na =
                EffectsManager.Singleton.RectangularEffect(sceneMgr, splashNode, type.ToString(), type, position,
                                                           new Vector2(4, 4), Quaternion.IDENTITY, false);
            na.FirstNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            na.FirstNode.Orientation *= new Quaternion(Math.RangeRandom(-0.1f, 0.1f)*Math.HALF_PI, Vector3.UNIT_Y);
            na.onFinishInfo = na.Nodes;
            na.onFinish = onFreeSplashNode;


            // zeby bylo widac z gory
            na2 =
               EffectsManager.Singleton.RectangularEffect(sceneMgr, splashNode, type + "top", type, position,
                                                          new Vector2(4, 4), Quaternion.IDENTITY, false);

            na2.FirstNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            na2.FirstNode.Orientation *= new Quaternion(Math.RangeRandom(-0.1f, 0.1f) * Math.HALF_PI, Vector3.UNIT_Y);
            na2.FirstNode.Orientation *= new Quaternion(Math.HALF_PI, Vector3.UNIT_Z);

            na2.onFinishInfo = na2.Nodes;
            na2.onFinish = onFreeSplashNode;
        }


        public void onFreeSplashNode(object o)
        {
            List<SceneNode> animationNodes = (List<SceneNode>)o;
            foreach (SceneNode node in animationNodes)
            {
                node.SetVisible(false);
            }
            
            freeSplashNode();
        }

        public void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            PlaneView p = FindPlaneView(plane);
            if (p == null) return;
            carrierView.StartCatchingPlane(p, carrierTile);
        }

        public void OnReleasePlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            carrierView.StartReleasingPlane();
        }



        public void OnPlaneEnterRestoreAmmunitionTile(Plane plane)
        {
            if (EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
            {
                carrierView.showChangeAmmoArrow();
            }
        }

        public void OnPlaneLeaveRestoreAmmunitionTile(Plane plane)
        {
            if (EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
            {
                carrierView.showHangaringArrow();
            }
        }

        public void OnTakeOff()
        {
            playerPlaneView.AnimationMgr.switchToIdle(true);
            carrierView.CrewStatePlaneAirborne();
            if(EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
            {
            	carrierView.hideHangaringArrow();
            }
        }
        
        public void OnTouchDown()
        {
          //  playerPlaneView.AnimationMgr[PlaneNodeAnimationManager.AnimationType.IDLE].rewind();
            playerPlaneView.AnimationMgr[PlaneNodeAnimationManager.AnimationType.IDLE].Enabled = false;
            carrierView.CrewStatePlaneOnCarrier();
            if(EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
            {
            	carrierView.showHangaringArrow();
            }
            
        }

        public void BuildCameraHolders()
        {
            if (playerPlaneView != null)
            {
                cameraHolders = playerPlaneView.GetCameraHolders();
            }
            if(EngineConfig.DebugStart) 
            {
	            foreach(PlaneView pv in planeViews) 
                {
	            	cameraHolders.AddRange(pv.GetCameraHolders());
	            	
	            }
            	            	
            	
            }
        }

        public void OnChangeCamera()
        {
            OnChangeCamera(currentCameraHolderIndex + 1);
        }

        public int OnChangeCamera(SceneNode holder)
        {

            HydraxManager.Singleton.ForceUpdate(); // jako ze hydrax nie updateuje sie zawsze, nalezy wymusic odswiezenie
            int lastIndex = currentCameraHolderIndex;
      
            framework.Camera.Position = Vector3.ZERO;
            framework.Camera.Orientation = Quaternion.IDENTITY;
     
            for (int i = 0; i < cameraHolders.Count; i++)
            {
                CurrentCameraHolder.DetachObject(framework.Camera);
            }


            holder.AttachObject(framework.Camera);
            playerPlaneView.HideCrossHair();
      
            //  EffectsManager.Singleton.NoSprite();
            SoundManager3D.Instance.SetListener(framework.CameraListener);
            return lastIndex;
        }


        public int OnChangeCamera(int camIndex)
        {
            int lastIndex = currentCameraHolderIndex;
            
            //framework.CameraZoom = 0;
            framework.Camera.Position = Vector3.ZERO;
            framework.Camera.Orientation = Quaternion.IDENTITY;

            currentCameraHolderIndex = (camIndex % cameraHolders.Count);
            for (int i = 0; i < cameraHolders.Count; i++)
            {
                CurrentCameraHolder.DetachObject(framework.Camera);
            }
           // Console.WriteLine("Attaching camera holder: "+cameraHolders[currentCameraHolderIndex].Name);
            cameraHolders[currentCameraHolderIndex].AttachObject(framework.Camera);

            // crosshair
            if (playerPlaneView.GetCrossHairCameraIndexes().Contains(camIndex))
            {
                float distance = 35;
                if(camIndex == 4)
                {
                    distance = -40;
                }
                playerPlaneView.ShowCrossHair(distance);
            }
            else
            {
                playerPlaneView.HideCrossHair();
            }

           

            


          //  EffectsManager.Singleton.NoSprite();
            SoundManager3D.Instance.SetListener(framework.CameraListener);
            return lastIndex;
        }

        public void OnResetCamera()
        {
            currentCameraHolderIndex = -1;
            OnChangeCamera();
        }


        public void OnReCreateHydrax()
        {
            HydraxManager.Singleton.ReCreateHydrax(framework.SceneMgr, framework.Camera, framework.Viewport);
        }




       
    }
}