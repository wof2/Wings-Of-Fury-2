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
using Mogre;
using Wof.Languages;
using Wof.Misc;
using Wof.Model.Level.Weapon;
using FontManager=Wof.Languages.FontManager;
using Plane=Wof.Model.Level.Planes.Plane;
using Wof.Model.Level;

namespace Wof.Controller.Screens
{
    internal class IndicatorControl
    {
        private Viewport viewport;
        private Viewport minimapViewport;

        private Overlay hudOverlay;

        private GameScreen gameScreen;

        private bool wasDisplayed;

        public bool WasDisplayed
        {
            get { return wasDisplayed; }
        }


        private OverlayContainer ammoContainer;
        private OverlayElement ammoElement;

        private OverlayContainer ammoTypeContainer;
        private OverlayElement ammoTypeElement;

        private OverlayContainer livesContainer;
        private OverlayElement livesElement;

        private OverlayContainer scoreContainer;
        private OverlayElement scoreElement;

        private OverlayContainer hiscoreContainer;
        private OverlayElement hiscoreElement;

        private OverlayContainer infoContainer;
        private OverlayElement infoElement;

        private Boolean closing;

        private SceneManager sceneMgr;
        private Entity hud, fuelArrow, oilArrow;
        private SceneNode hudNode, fuelArrowNode, oilArrowNode;

        private Random random;

        /// <summary>
        /// Kiedy ostatnio zatrz¹s³ siê (graficznie) wskaŸnik paliwa (pomiar w sek.)
        /// </summary>
        private float lastFuelIndicatorDither;

        /// <summary>
        /// Kiedy ostatnio zatrz¹s³ siê (graficznie) wskaŸnik oleju (pomiar w sek.)
        /// </summary>
        private float lastOilIndicatorDither;



        public IndicatorControl(Viewport viewport,
                                Viewport minimapViewport, GameScreen gameScreen)
        {
            this.minimapViewport = minimapViewport;
            this.viewport = viewport;
            this.gameScreen = gameScreen;
            sceneMgr = FrameWork.OverlayMgr;
            random = new Random();

            wasDisplayed = false;
            closing = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuel">przedzia³ 0-1</param>
        private void RefreshFuel(float fuel)
        {
           

            fuelArrowNode.Orientation = new Quaternion(new Degree(fuel*330), Vector3.NEGATIVE_UNIT_Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oil">przedzia³ 0-1</param>
        private void RefreshOil(float oil)
        {
            oilArrowNode.Orientation = new Quaternion(new Degree(oil*330), Vector3.NEGATIVE_UNIT_Z);
        }


        public void ChangeAmmoType(WeaponType weaponType)
        {
            if(!EngineConfig.DisplayMinimap) return;

            switch (weaponType)
            {
                case WeaponType.Rocket:
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1", "Panels/Panel1_rocket");
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1_torpedo", "Panels/Panel1_rocket");
                    break;
                case WeaponType.Bomb:
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1_rocket", "Panels/Panel1");
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1_torpedo", "Panels/Panel1");
                    break;

                case WeaponType.Torpedo:
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1", "Panels/Panel1_torpedo");
                    ViewHelper.ReplaceMaterial(hud, "Panels/Panel1_rocket", "Panels/Panel1_torpedo");
                    break;
                    
            } 
           
                
            
        }

        public void DisplayIndicator()
        {
            wasDisplayed = true;
            if(!EngineConfig.DisplayMinimap) return;

            hudOverlay = OverlayManager.Singleton.GetByName("Wof/HUD");

            // HUD
            hud = sceneMgr.CreateEntity("HUD", "HUD.mesh");
            hud.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;

            hudNode = new SceneNode(sceneMgr);
            //float viewportAspect = (1.0f * viewport.ActualWidth / viewport.ActualHeight);
            //float hudAspect = 4.0f / 3.0f;
            //float scale = viewportAspect / hudAspect;


            hudNode.SetScale(0.01505f, 0.0150f, 0.01f);
            hudNode.AttachObject(hud);
            hudNode.Position = new Vector3(-0.01f, -0.405f, -1f);
            hudNode.GetMaterial().SetDepthCheckEnabled(false);

            // ARROW
            fuelArrow = sceneMgr.CreateEntity("FuelArrow", "Arrow.mesh");
            fuelArrow.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;

            fuelArrowNode = new SceneNode(sceneMgr);
            fuelArrowNode.SetScale(0.16f, 0.16f, 0.16f);
            fuelArrowNode.AttachObject(fuelArrow);
            fuelArrowNode.Position = new Vector3(-0.214f, -0.255f, -0.7f);
            fuelArrowNode.GetMaterial().SetDepthCheckEnabled(false);


            // ARROW
            oilArrow = sceneMgr.CreateEntity("OilArrow", "Arrow.mesh");
            oilArrow.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;

            oilArrowNode = new SceneNode(sceneMgr);
            oilArrowNode.SetScale(0.16f, 0.16f, 0.16f);
            oilArrowNode.AttachObject(oilArrow);
            oilArrowNode.Position = new Vector3(0.214f, -0.255f, -0.7f);
            oilArrowNode.GetMaterial().SetDepthCheckEnabled(false);


            hudOverlay.Add3D(fuelArrowNode);
            hudOverlay.Add3D(oilArrowNode);
            hudOverlay.Add3D(hudNode);
            hudOverlay.ZOrder = 1;
            hudOverlay.Show();


            CreateAmmoContainer();
            CreateAmmoTypeContainer();
            CreateLivesContainer();
            CreateScoreContainer();
            CreateHighscoreContainer();
            CreateInfoContainer();
        }

        public void UpdateGUI(float timeSinceLastFrame)
        {
            if (!EngineConfig.DisplayMinimap) return;
            if (gameScreen != null && gameScreen.CurrentLevel != null && !closing)
            {
                switch (gameScreen.CurrentLevel.UserPlane.Weapon.SelectWeapon)
                {
                    case WeaponType.Bomb:
                        ammoElement.Caption = gameScreen.CurrentLevel.UserPlane.Weapon.BombCount.ToString();
                        break;

                    case WeaponType.Rocket:
                        ammoElement.Caption = gameScreen.CurrentLevel.UserPlane.Weapon.RocketCount.ToString();
                        break;

                    case WeaponType.Torpedo:
                        ammoElement.Caption = gameScreen.CurrentLevel.UserPlane.Weapon.TorpedoCount.ToString();
                        break;
                }
              
                                       

                // ammoTypeElement.Caption = gameScreen.CurrentLevel.UserPlane.Weapon.SelectWeapon.ToString() + "s";


                livesElement.Caption = gameScreen.CurrentLevel.Lives.ToString();
                scoreElement.Caption = gameScreen.Score.ToString();
                hiscoreElement.Caption = gameScreen.GetHighscore().ToString();


                string difficulty = String.Format(@"{0}: ", LanguageResources.GetString(LanguageKey.Difficulty));
                switch (EngineConfig.Difficulty)
                {
                    case EngineConfig.DifficultyLevel.Easy:
                        difficulty += LanguageResources.GetString(LanguageKey.Low);
                        break;
                    case EngineConfig.DifficultyLevel.Medium:
                        difficulty += LanguageResources.GetString(LanguageKey.Medium);
                        break;
                    case EngineConfig.DifficultyLevel.Hard:
                        difficulty += LanguageResources.GetString(LanguageKey.High);
                        break;
                }

                if (EngineConfig.DebugInfo)
                {
                    if (gameScreen.CurrentLevel.MissionType == MissionType.BombingRun)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}",
                        LanguageResources.GetString(LanguageKey.Level), 
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.EnemySoldiersLeft),
                        gameScreen.CurrentLevel.SoldiersCount,
                        gameScreen.Framework.UpdateStats());
                    }
                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Dogfight)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}",
                        LanguageResources.GetString(LanguageKey.Level), 
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.EnemyPlanesLeft),
                        gameScreen.CurrentLevel.EnemyPlanesLeft,
                        gameScreen.Framework.UpdateStats());
                    }
                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Assassination)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.EnemyGenerals),
                        gameScreen.CurrentLevel.GeneralsCount,
                        gameScreen.Framework.UpdateStats());
                    }
                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Naval)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.EnemyShipsLeft),
                        gameScreen.CurrentLevel.ShipsLeft,
                        gameScreen.Framework.UpdateStats());
                    }

                }
                else
                {
                    if (gameScreen.CurrentLevel.MissionType == MissionType.BombingRun)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}: {6}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo, 
                        difficulty,
                        LanguageResources.GetString(LanguageKey.MissionType),
                        LanguageResources.GetString(LanguageKey.BombingRun),
                        LanguageResources.GetString(LanguageKey.EnemySoldiersLeft),
                        gameScreen.CurrentLevel.SoldiersCount);
                    }
                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Dogfight)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}: {6}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo, 
                        difficulty,
                        LanguageResources.GetString(LanguageKey.MissionType),
                        LanguageResources.GetString(LanguageKey.Dogfight),
                        LanguageResources.GetString(LanguageKey.EnemyPlanesLeft),
                        gameScreen.CurrentLevel.EnemyPlanesLeft);
                    }

                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Assassination)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}: {6}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.MissionType),
                        LanguageResources.GetString(LanguageKey.Assassination),
                        LanguageResources.GetString(LanguageKey.Target),
                        (gameScreen.CurrentLevel.GeneralsCount == 0) ?
                            LanguageResources.GetString(LanguageKey.Neutralized) :
                            LanguageResources.GetString(LanguageKey.Alive));
                    }
                    else if (gameScreen.CurrentLevel.MissionType == MissionType.Naval)
                    {
                        infoElement.Caption = String.Format(@"{0}: {1} | {2} | {3}: {4} | {5}: {6}",
                        LanguageResources.GetString(LanguageKey.Level),
                        gameScreen.LevelNo,
                        difficulty,
                        LanguageResources.GetString(LanguageKey.MissionType),
                        LanguageResources.GetString(LanguageKey.Naval),
                        LanguageResources.GetString(LanguageKey.EnemyShipsLeft),
                        gameScreen.CurrentLevel.ShipsLeft);
                    }

                }
                if(EngineConfig.C_IS_INTERNAL_TEST) 
                {
                     infoElement.Caption = EngineConfig.C_IS_INTERNAL_TEST_INFO + infoElement.Caption;
                }
               
                Plane p = gameScreen.CurrentLevel.UserPlane;

                float var, oil, fuel;
                
                // fuel
                fuel = p.Petrol / p.MaxPetrol;
                lastFuelIndicatorDither += timeSinceLastFrame;
                if (lastFuelIndicatorDither > 0.04)// 50ms
                {
                    if (p.IsEngineWorking)
                    {
                        var = (0.7f * p.AirscrewSpeed / 1300.0f) * (random.Next(-1, 2) / 100.0f);
                    }
                    else
                    {
                        var = 0;
                    }
                    if (fuel > 1.01f) fuel = 1.0f;
                    if (fuel < -0.01f) fuel = 0.0f; 
                    fuel += var;
                    lastFuelIndicatorDither = 0;
                }

                RefreshFuel(fuel);
               

                // oil
                oil = p.Oil / p.MaxOil;
                lastOilIndicatorDither += timeSinceLastFrame;
                if (lastOilIndicatorDither > 0.04) // 50ms
                {
                    if (p.IsEngineWorking)
                    {
                        var = (0.7f * p.AirscrewSpeed / 1300.0f) * (random.Next(-1, 2) / 100.0f);
                    }
                    else
                    {
                        var = 0;
                    } 
                    oil += var;
                    if (oil > 1.01f) oil = 1.0f; 
                    if (oil < -0.01f) oil = 0.0f; 
                   
                    lastOilIndicatorDither = 0;
                }
                RefreshOil(oil);



            }
        }


        private void CreateAmmoContainer()
        {
            ammoElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "ammoElement " + DateTime.Now.Ticks);
            ammoContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                   "Panel", "ammoContainer " + DateTime.Now.Ticks);

        	
        	int count = 0;
        	switch(gameScreen.CurrentLevel.UserPlane.Weapon.SelectWeapon)
        	{
        		case WeaponType.Bomb:
        			count = gameScreen.CurrentLevel.UserPlane.Weapon.BombCount;
        			break;
        		case WeaponType.Rocket:
        			count = gameScreen.CurrentLevel.UserPlane.Weapon.RocketCount;
        			break;
        		case WeaponType.Torpedo:
        			count = gameScreen.CurrentLevel.UserPlane.Weapon.TorpedoCount;
        			break;
        			
        	}
        	ConfigureElement(ammoElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight,count.ToString());

            ConfigureContainer(ammoContainer, ammoElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               UnitConverter.AspectDependentHorizontalProportion(0.057f, 0.03f, 0.134f, viewport),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, 61));
        }


        private void CreateAmmoTypeContainer()
        {
            ammoTypeElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "ammoTypeElement " + DateTime.Now.Ticks);
            ammoTypeContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                       "Panel", "ammoTypeContainer " + DateTime.Now.Ticks);

            ConfigureElement(ammoTypeElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight, "");
            /*gameScreen.CurrentLevel.UserPlane.Weapon.SelectWeapon.ToString() + "s");*/
            // nie pokazuje 'Rockets' ani 'Bombs'

            ConfigureContainer(ammoTypeContainer, ammoTypeElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               CountProportion(viewport.ActualWidth,
                                               833, 28),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, 43));
            if(gameScreen.CurrentLevel != null && gameScreen.CurrentLevel.UserPlane != null && gameScreen.CurrentLevel.UserPlane.Weapon != null)
            {
            	 ChangeAmmoType(gameScreen.CurrentLevel.UserPlane.Weapon.SelectWeapon);
            }
           
        }


        private void CreateLivesContainer()
        {
            livesElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "livesElement " + DateTime.Now.Ticks);
            livesContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                    "Panel", "livesContainer " + DateTime.Now.Ticks);

            ConfigureElement(livesElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                             gameScreen.CurrentLevel.Lives.ToString());

            ConfigureContainer(livesContainer, livesElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               UnitConverter.AspectDependentHorizontalProportion(0.125f, 0.10f, 0.189f, viewport),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, 61));
        }

        private void CreateScoreContainer()
        {
            scoreElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "scoreElement " + DateTime.Now.Ticks);
            scoreContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                    "Panel", "scoreContainer " + DateTime.Now.Ticks);

            ConfigureElement(scoreElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                             gameScreen.Score.ToString());

            ConfigureContainer(scoreContainer, scoreElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               UnitConverter.AspectDependentHorizontalProportion(0.85f, 0.875f, 0.790f, viewport),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, 58));
        }

        private void CreateInfoContainer()
        {
            infoElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "infoElement " + DateTime.Now.Ticks);
            infoContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                   "Panel", "infoContainer " + DateTime.Now.Ticks);

            ConfigureElement(infoElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                             "", FontManager.CurrentFont);

            ConfigureContainer(infoContainer, infoElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               CountProportion(viewport.ActualWidth,
                                               833, 5),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, -15));
        }


        private void CreateHighscoreContainer()
        {
            hiscoreElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "hiscoreElement " + DateTime.Now.Ticks);
            hiscoreContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                      "Panel", "hiscoreContainer " + DateTime.Now.Ticks);

            ConfigureElement(hiscoreElement, minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                             gameScreen.GetHighscore().ToString());

            ConfigureContainer(hiscoreContainer, hiscoreElement,
                               minimapViewport.ActualWidth, minimapViewport.ActualHeight,
                               UnitConverter.AspectDependentHorizontalProportion(0.85f, 0.875f, 0.790f, viewport),
                               viewport.ActualHeight - minimapViewport.ActualHeight +
                               CountProportion(minimapViewport.ActualHeight,
                                               84, 34));
        }
        
        public static void ConfigureElement(OverlayElement element, int width, int height, String caption)
        {
            ConfigureElement(element, width, height, caption, "BlueHighway");
        }

        public static void ConfigureElement(OverlayElement element, int width, int height, String caption, String fontName)
        {
            element.SetDimensions(100, 50);
            element.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            element.SetDimensions(width, height);
            element.SetPosition(0, 0);
            element.SetParameter("font_name", fontName);
            element.SetParameter("char_height", "12");
            element.SetParameter("colour_top", "0.0 0.0 0.0");
            element.SetParameter("colour_bottom", "0.0 0.0 0.0");
            element.Caption = caption;
        }

        private void ConfigureContainer(OverlayContainer container, OverlayElement element, int width, int height,
                                        int posX, int posY)
        {
            container.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            container.SetDimensions(minimapViewport.ActualWidth, minimapViewport.ActualHeight);
            container.SetPosition(posX,
                                  posY);

            container.AddChild(element);
            hudOverlay.Add2D(container);
            container.Show();
        }


        private int CountProportion(int vp, int s, int p)
        {
            return p*vp/s;
        }

        public void ClearGUI()
        {
           
            closing = true;
            
            if (hudOverlay != null)
            {
                hudOverlay.Hide();
                hudOverlay.Dispose();
            }
            if (!EngineConfig.DisplayMinimap) return;

            try
            {
                ammoContainer.Hide();
                livesContainer.Hide();
                scoreContainer.Hide();
                hiscoreContainer.Hide();
                infoContainer.Hide();

                OverlayManager.Singleton.DestroyOverlayElement(ammoElement);
                OverlayManager.Singleton.DestroyOverlayElement(livesElement);
                OverlayManager.Singleton.DestroyOverlayElement(scoreElement);
                OverlayManager.Singleton.DestroyOverlayElement(hiscoreElement);
                OverlayManager.Singleton.DestroyOverlayElement(infoElement);

                ammoContainer.Dispose();
                livesContainer.Dispose();
                hiscoreContainer.Dispose();
                scoreContainer.Dispose();
                infoContainer.Dispose();

                // sceneMgr.DestroyAllEntities();

                hudNode.Dispose();
                hudNode = null;
                fuelArrowNode.Dispose();
                fuelArrowNode = null;
                oilArrowNode.Dispose();
                oilArrowNode = null;

                /*
                ammoElement.Hide();
                ammoElement.Dispose();
                livesElement.Hide();
                livesElement.Dispose();
                scoreElement.Hide();
                scoreElement.Dispose();
                hiscoreElement.Hide();
                hiscoreElement.Dispose();*/
            }
            catch (Exception ex)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Exception while cleaning up indicator control: " + ex.StackTrace);   
             
            }
           
        }
    }
}