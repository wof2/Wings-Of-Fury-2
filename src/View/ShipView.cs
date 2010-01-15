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


using FSLOgreCS;
using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.View.Effects;
using Wof.View.TileViews;
using Math = Mogre.Math;

namespace Wof.View
{
    public class ShipView : CompositeModelView, IDisposable
    {
       
        private static int shipCounter = 0;
        protected SceneNode staticNode;

        protected float adjust;

        public SceneNode StaticNode
        {
            get { return staticNode; }
        }

        
        private FSLSoundEntity dieSound = null;
        private int count;

        #region Minimap representation

        protected MinimapItem minimapItem;

        public MinimapItem MinimapItem
        {
            get { return minimapItem; }
        }

        #endregion

        /// <summary>
        /// Konstruktor dla wysp na pierwszym planie / biora udzial w grze
        /// </summary>
        /// <param name="tileViews">Lista pól Views statku</param>
        /// <param name="framework">Standardowy framework Ogre'a</param>
        /// <param name="parentNode">SceneNode który bêdzie zawiera³ w sobie Node'a statku</param>
        /// <author>Adam Witczak</author>
        public ShipView(List<TileView> tileViews, IFrameWork framework, SceneNode parentNode)
            : base(tileViews, framework, parentNode, "Ship" + (++shipCounter))
        {
            initOnScene();
        }


      
        
        public void OnShipSunk()
        {
            EffectsManager.Singleton.NoSprite(sceneMgr, staticNode, EffectsManager.EffectType.FIRE, 0);
            EffectsManager.Singleton.NoSmoke(sceneMgr, staticNode);
         
            for (uint i = 0; i < 6; i++ )
            {
  				EffectsManager.Singleton.NoSprite(sceneMgr, staticNode, EffectsManager.EffectType.FIRE, i);
            }
        }
        
        
        public void OnShipDamaged(ShipState state)
        {
        	
         	float length = this.tileViews.Count * LevelTile.TileWidth;
         	
         	
            switch (state)
            {
                case ShipState.LightDamaged:
        		{
		               EffectsManager.Singleton.Smoke(sceneMgr, staticNode,Effects.EffectsManager.SmokeType.NORMAL, new Vector3(0, 6, -Mogre.Math.RangeRandom(0, length)), Vector3.UNIT_Y, new Vector2(15,15));
	        		   EffectsManager.Singleton.Smoke(sceneMgr, staticNode,Effects.EffectsManager.SmokeType.LIGHTSMOKE, new Vector3(0, 6, -Mogre.Math.RangeRandom(0, length)), Vector3.UNIT_Y, new Vector2(3,3));
	        		
		               
            	}
                break;

                case ShipState.HeavyDamage:
                {
                	for (uint i = 0; i < 6; i++ )
	                {
                		EffectsManager.Singleton.Sprite(sceneMgr, staticNode,
                		                                new Vector3(Math.RangeRandom(-3, 3), 9.0f, -Mogre.Math.RangeRandom(0, length)),  new Vector2(Math.RangeRandom(5, 10), Math.RangeRandom(5, 10)),
                                               EffectsManager.EffectType.FIRE, true, i.ToString());
                	}
                }
                break;

            }
        }

        public void OnShipBeginSinking(LevelTile shipTile)
        {
            if (EngineConfig.SoundEnabled)
            {
                dieSound.Play();                
            }
            
        	foreach(TileView tv in TileViews)
            {
            	if(tv is ShipBunkerTileView)
            	{
            		tv.MinimapItem.Hide();
            	}
            }
        	LevelTile tile = shipTile;
        	Vector2 v = UnitConverter.LogicToWorldUnits(new PointD(Mathematics.IndexToPosition(tile.TileIndex), 1.5f));
            string name;
            
			if (!EngineConfig.LowDetails)
            {
	            for (uint i = 0; i < 3; i++ )
	            {
	
	                Vector2 rand = ViewHelper.RandomVector2(8, 8);
	                Vector3 posView = new Vector3(v.x + rand.x, v.y, 0 + rand.y);
	                name = EffectsManager.BuildSpriteEffectName(sceneMgr.RootSceneNode, EffectsManager.EffectType.SUBMERGE, tile.GetHashCode() + "_" + i);
	                if (!EffectsManager.Singleton.EffectExists(name) || EffectsManager.Singleton.EffectEnded(name))
	                {
	                    EffectsManager.Singleton.RectangularEffect(sceneMgr, sceneMgr.RootSceneNode,
	                                                               tile.GetHashCode() + "_" + i,
	                                                               EffectsManager.EffectType.SUBMERGE, posView,
	                                                               new Vector2(25, 25), Quaternion.IDENTITY, false);
	                }
	
	                name = EffectsManager.BuildSpriteEffectName(sceneMgr.RootSceneNode, EffectsManager.EffectType.WATERIMPACT1,"WaterImpact1_" + tile.GetHashCode() + "_" + i);
	                if (!EffectsManager.Singleton.EffectExists(name))
	                {
	                    EffectsManager.Singleton.WaterImpact(sceneMgr, sceneMgr.RootSceneNode, posView, new Vector2(20, 32), false, tile.GetHashCode() + "_" + i);
	                }
	
	
	                EffectsManager.EffectType type;
	                if (((uint)tile.GetHashCode() + i) % 2 == 0)
	                {
	                    type = EffectsManager.EffectType.EXPLOSION2_SLOW;
	                } else
	                {
	                    type = EffectsManager.EffectType.EXPLOSION1_SLOW;
	                }
	
	                name = EffectsManager.BuildSpriteEffectName(sceneMgr.RootSceneNode, type, (tile.GetHashCode() + i).ToString());
	                if (!EffectsManager.Singleton.EffectExists(name))
	                {
	                    if(Math.RangeRandom(0,1) > 0.8f)
	                    {
	                         EffectsManager.Singleton.Sprite(sceneMgr, sceneMgr.RootSceneNode, posView + ViewHelper.UnsignedRandomVector3(0,10,0), new Vector2(15, 15) + ViewHelper.RandomVector2(5,5),
	                                                    type, false,
	                                                    (tile.GetHashCode() + i).ToString());
	
	                    }
	                   
	                }
	            }

            }
        
        	
        }
        
       
        public void OnShipSinking(LevelTile shipTile) 
        {
        	LevelTile tile = shipTile;
        	Vector2 v = UnitConverter.LogicToWorldUnits(new PointD(Mathematics.IndexToPosition(tile.TileIndex), 1.5f));
            string name;
           

            for (uint i = 0; i < 3; i++ )
            {
                Vector2 rand = ViewHelper.RandomVector2(6, 6);
                Vector3 posView = new Vector3(v.x + rand.x, v.y, 0 + rand.y);
                name = EffectsManager.BuildSpriteEffectName(sceneMgr.RootSceneNode, EffectsManager.EffectType.SUBMERGE, tile.GetHashCode() + "_" + i);
                NodeAnimation.NodeAnimation node = EffectsManager.Singleton.GetEffect(name);
                if (!EffectsManager.Singleton.EffectExists(name) || (node!=null && node.getPercent() > 0.6f))
                {
                    EffectsManager.Singleton.RectangularEffect(sceneMgr, sceneMgr.RootSceneNode,
                                                               tile.GetHashCode() + "_" + i,
                                                               EffectsManager.EffectType.SUBMERGE, posView,
                                                               new Vector2(25, 25), Quaternion.IDENTITY, false);
                }                    

            }
            
           
            // zgaœ ogieñ - pierwotnie mia³o byæ realizowane przez LevelView.OnShipUnderWater. Z przyczyn technicznych realizowane jest tutaj
            for (uint i = 0; i < 6; i++ )
            {
            	name = EffectsManager.BuildSpriteEffectName(staticNode, EffectsManager.EffectType.FIRE, i.ToString());
            	NodeAnimation.NodeAnimation node = EffectsManager.Singleton.GetEffect(name);
            	if(node != null && node.Node._getDerivedPosition().y < 0.0f)
            	{
            	   	EffectsManager.Singleton.NoSprite(sceneMgr, staticNode, EffectsManager.EffectType.FIRE, i);
            	}
        		
        	}
        }
        
        

        protected override void initOnScene()
        {
            String meshName; //Nazwa modelu  

           
            staticNode = sceneMgr.CreateSceneNode(mainNode.Name + "Static");
            
            count = tileViews.Count;
          
           
            float maxX = (Math.Abs(count) - 1) * LevelView.TileWidth / 16;
            BeginShipTile begin = tileViews[0].LevelTile as BeginShipTile;

            switch (begin.TypeOfEnemyShip)
            {
                case TypeOfEnemyShip.PatrolBoat: 
                    meshName = "PatrolBoat.mesh";
                   
                    break;

                case TypeOfEnemyShip.WarShip:
                    meshName = "Warship.mesh";
                 
                    break;


                default:
                    return;
            }

            compositeModel = sceneMgr.CreateEntity(name, meshName);
            compositeModel.CastShadows = EngineConfig.ShadowsQuality > 0; 


            staticNode.Translate(new Vector3(UnitConverter.LogicToWorldUnits(tileViews[0].LevelTile.TileIndex), -(tileViews[0].LevelTile as ShipTile).Depth, 0));
            staticNode.SetDirection(Vector3.UNIT_X);
       
           
            //  StaticGeometry sg;
            staticNode.AttachObject(compositeModel);

            mainNode.AddChild(staticNode);
            
            // elementy na statku sa animowalne wiec nie beda w static geometry
            for (int i = 0; i < count; i++)
            {
                tileViews[i].initOnScene(staticNode, i + 1, tileViews.Count);
            }
             
            string soundFile = SoundManager3D.C_SHIP_SINKING;
            if(Mogre.Math.RangeRandom(0,1) > 0.5f)
            {
            	soundFile = SoundManager3D.C_SHIP_SINKING_2;
            }
            dieSound = SoundManager3D.Instance.CreateSoundEntity(soundFile, mainNode, false, false);
            dieSound.SetBaseGain(0.25f);

            // minimapa
            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem =
                    new MinimapItem(staticNode, framework.MinimapMgr, "ShipMinimap.mesh",
                                     new ColourValue(0.092f, 0.262f, 0.49f), compositeModel);

                minimapItem.ScaleOverride = new Vector2(0, 15); // stala wysokosc, niezale¿na od bounding box
                minimapItem.Refresh();
            }
           
        }
      
        public virtual void refreshPosition()
        {

            if (tileViews.Count > 0)
            {
                LevelTile t = tileViews[0].LevelTile;
                Vector2 v = UnitConverter.LogicToWorldUnits(new PointD(Mathematics.IndexToPosition(t.TileIndex), -(t as ShipTile).Depth));

                staticNode.SetPosition(v.x, v.y, 0.0f);
            }
        }

        protected void initLampPost(SceneNode parent, Vector3 position, Radian direction)
        {
            Entity lamp;
            SceneNode lampNode;
            int id = LevelView.PropCounter;
            lamp = sceneMgr.CreateEntity("Lamp" + id, "LampPost.mesh");
            lamp.CastShadows = EngineConfig.ShadowsQuality > 0; 

            lampNode = parent.CreateChildSceneNode("LampNode" + LevelView.PropCounter, position);
            lampNode.Yaw(direction);
            lampNode.AttachObject(lamp);

        }
        

        ~ShipView()
        {
        
            /*for (int i=0; i < parkedPlanes.Count; i++)
            {
                parkedPlanes[i].
            }*/
        }
    	
		public void Dispose()
		{
		    if (dieSound != null)
            {
                 SoundManager3D.Instance.RemoveSound(dieSound.Name); 
            }
		}
    }
}
