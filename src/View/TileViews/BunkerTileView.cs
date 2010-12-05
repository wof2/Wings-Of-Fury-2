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
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.View.Effects;

namespace Wof.View.TileViews
{
    public class BunkerTileView : EnemyInstallationTileView
    {
        protected SceneNode gunPlaceNode;

        public SceneNode GunPlaceNode
        {
            get { return gunPlaceNode; }
        }

       

        protected bool isConcrete;

        public bool IsConcrete
        {
            get { return isConcrete; }
        }


      

        /// <summary>
        /// Czy posiada wyrzutnie rakiet
        /// </summary>
        public bool HasRockets
        {
            get { return (levelTile as BunkerTile).HasRockets; }
        }

        protected AnimationState barrelState;
        protected Entity flakBarrel;


        public BunkerTileView(LevelTile levelTile, IFrameWork framework)
            : base(levelTile, framework)
        {
            animationState = null;
        }

        protected void rotateGun(float radians)
        {
            gunNode.Orientation = new Quaternion(radians, Vector3.UNIT_X);
        }
        
        


        private void initBunker(SceneNode islandNode, float positionOnIsland)
        {
            String nameSuffix = tileID.ToString();

            gunPlaceNode = islandNode.CreateChildSceneNode("GunEmplacement" + nameSuffix, new Vector3(0.0f, 0.1f, -7.0f));
            //gunPlaceNode.Translate(new Vector3(0.0f, 0.0f, positionOnIsland));
            gunPlaceNode.Translate(new Vector3(0.0f, levelTile.HitBound.LowestY, positionOnIsland));

            if(!(LevelTile is FortressBunkerTile))
            {
                 Entity sandbags = sceneMgr.CreateEntity("Sandbags" + nameSuffix, "Sandbags.mesh");
                gunPlaceNode.AttachObject(sandbags);
            }
           
           

            installationNode =
                gunPlaceNode.CreateChildSceneNode("BunkerNode" + nameSuffix, new Vector3(0.0f, 0.0f, 4.5f));
            if (!(LevelTile is FortressBunkerTile))
            {
                installationEntity = sceneMgr.CreateEntity("Bunker" + nameSuffix, "Bunker.mesh");
            }
            else
            {
                installationEntity = sceneMgr.CreateEntity("Fortress" + nameSuffix, "Fortress.mesh");
            }
           
            isConcrete = false;
            if (LevelTile is ConcreteBunkerTile || LevelTile is FortressBunkerTile)
            {
                isConcrete = true;
               
            }

            if (LevelTile is ConcreteBunkerTile && !(LevelTile is FortressBunkerTile)) installationEntity.SetMaterialName("Concrete"); // aby by³ betonowy

            installationNode.AttachObject(installationEntity);

            Entity flakBase = sceneMgr.CreateEntity("FlakBase" + nameSuffix, "FlakBase.mesh");
            gunPlaceNode.AttachObject(flakBase);

            flakBarrel = sceneMgr.CreateEntity("FlakBarrel" + nameSuffix, "FlakBarrel.mesh");
            gunNode = gunPlaceNode.CreateChildSceneNode("FlakBarrelNode" + nameSuffix, new Vector3(0.0f, 0.5f, 0.0f));
            gunNode.AttachObject(flakBarrel);

          


            if (EngineConfig.DisplayingMinimap)
            {
            	ColourValue col = ColourValue.Red;
            	if(isConcrete)
            	{
                    col = new ColourValue(0, 0, 1);
            	}
                
                minimapItem =
                    new MinimapItem(gunPlaceNode, framework.MinimapMgr, "Cube.mesh", col, installationEntity);
                minimapItem.ScaleOverride = new Vector2(0, 13); // stala wysokosc dziala, niezale¿na od bounding box
                minimapItem.Refresh();
            }

            barrelState = flakBarrel.GetAnimationState("manual");
            barrelState.Enabled = true;
            barrelState.Loop = true;
            animableElements.Add(barrelState);
        }

        public override void GunFire()
        {
            int i = animableElements.IndexOf(barrelState);
            if (i != -1)
            {
                barrelState = animableElements[i] = flakBarrel.GetAnimationState("fire");
                animableElements[i].TimePosition = 0.0f;
                animableElements[i].Enabled = true;
                animableElements[i].Loop = false;
            } 
            EffectsManager.Singleton.Sprite(
              sceneMgr,
              GunNode,
              new Vector3(0, 0, -3),
              new Vector2(1, 1) + ViewHelper.UnsignedRandomVector2(0.2f, 0.2f),
              EffectsManager.EffectType.EXPLOSION1,
              false,
              0
            ).TimeScale=2.0f;
        }

        public override void Restore()
        {
            animationState = installationEntity.GetAnimationState("manual");
            animationState.Enabled = true;
            animationState.Loop = false;

            if (isConcrete)
            {
                ViewHelper.ReplaceMaterial(installationEntity, "DestroyedConcrete", "Concrete");
            }
            else
            {
                ViewHelper.ReplaceMaterial(installationEntity, "DestroyedWood", "Wood");
            }
            EffectsManager.Singleton.NoSmoke(sceneMgr, installationNode);
            EffectsManager.Singleton.HideSprite(sceneMgr, installationNode, EffectsManager.EffectType.FIRE, 0);

            if (EngineConfig.DisplayingMinimap)
            {
                ColourValue col = ColourValue.Red;
                if (isConcrete)
                {
                    col = new ColourValue(0, 0, 1);
                }
                minimapItem.Colour = col;
                minimapItem.Refresh();
            }
        }
        
        public void Damage(bool firePossibility)
        {
        	
           // smokeParticleSystem = EffectsManager.Singleton.Smoke(sceneMgr, installationNode, new Vector3(0, -1, 0), Vector3.UNIT_Y);
          

            if (firePossibility && Mogre.Math.RangeRandom(0.0f, 1.0f) > 0.8f)
            {
                EffectsManager.Singleton.Sprite(sceneMgr, InstallationNode,
                                                new Vector3(2, 2.4f, Mogre.Math.RangeRandom(-4, 4)), new Vector2(5, 5),
                                                EffectsManager.EffectType.FIRE, true, 0);
            }
            
            if (isConcrete)
            {
                ViewHelper.ReplaceMaterial(installationEntity, "Concrete", "DestroyedConcrete");
            }
        }
        

        public override void Destroy()
        {
            gunNode.Orientation = Quaternion.IDENTITY;
            base.Destroy();
            if (isConcrete)
            {
                ViewHelper.ReplaceMaterial(installationEntity, "Concrete", "DestroyedConcrete");
            }
            else
            {
                ViewHelper.ReplaceMaterial(installationEntity, "Wood", "DestroyedWood");
            }

            if (EngineConfig.DisplayingMinimap)
            {
                //Kolor szary
                minimapItem.Colour = new ColourValue(0.752f, 0.752f, 0.752f);
            }
        }

        public override void initOnScene(SceneNode parentNode, int tileCMVIndex, int compositeModelTilesNumber)
        {
            base.initOnScene(parentNode, tileCMVIndex, compositeModelTilesNumber);

            if (levelTile is BunkerTile)
            {

                initBunker(parentNode, -getRelativePosition(parentNode, LevelTile));

                int variant = ((IslandTile) LevelTile).Variant;

                switch (variant)
                {
                        //Bez drzew
                    case 0:
                        break;
                    case 1:
                        initPalm(new Vector3(-0.6f, 0, -5));
                        initPalm(new Vector3(0.9f, 0, -3));
                        break;
                    case 2:
                        initPalm(new Vector3(0.6f, 0, -5));
                        initPalm(new Vector3(-0.9f, 0, -3));
                        break;
                        //Flaga na dachu
                    case 3:
                        initFlag(new Vector3(0, 1.6f, 0));
                        break;
                        //Flaga pomiedzy bunkrem i dzialem
                    case 4:
                        initFlag(new Vector3(0, -0.1f, -2.9f));
                        break;
                }
            }
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {
            base.updateTime(timeSinceLastFrameUpdate);

            BunkerTile bunkerTile = (BunkerTile) levelTile;
            rotateGun((float) bunkerTile.Angle);
        }
    }
}