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
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.Common;
using Wof.View.Effects;
using Math=Mogre.Math;
using Wof.View.TileViews;
using Wof.Model.Level.LevelTiles.IslandTiles;

namespace Wof.View
{
    internal class IslandView : CompositeModelView
    {

        private List<PlaneView> parkedPlanes; 
        private static int islandCounter = 0;
        protected SceneNode staticNode;

        public SceneNode StaticNode
        {
            get { return staticNode; }
        }


        protected string meshName;


        protected int count;

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
        /// <param name="tileViews">Lista pól Views wyspy</param>
        /// <param name="framework">Standardowy framework Ogre'a</param>
        /// <param name="parentNode">SceneNode który bêdzie zawiera³ w sobie Node'a wyspy</param>
        /// <author>Kamil S³awiñski</author>
        public IslandView(List<TileView> tileViews, IFrameWork framework, SceneNode parentNode)
            : base(tileViews, framework, parentNode, "Island" + (++islandCounter))
        {
            parkedPlanes =  new List<PlaneView>();
            initOnScene();
        }

        /// <summary>
        /// Konstruktor dla wysp na drugim planie / nie biora udzialu w grze
        /// </summary>
        /// <param name="indexTile"></param>
        /// <param name="length">D³ugoœæ wyspy</param>
        /// <param name="framework">Standardowy framework Ogre'a</param>
        /// <param name="parentNode">SceneNode który bêdzie zawiera³ w sobie Node'a wyspy</param>
        /// <author>Kamil S³awiñski</author>
        public IslandView(int indexTile, IFrameWork framework, SceneNode parentNode)
            : base(indexTile, framework, parentNode, "Island" + (++islandCounter))
        {
            parkedPlanes = new List<PlaneView>();
            //count = length;
            initOnScene();
        }

        protected override void initOnScene()
        {
            BeginIslandTile tile;
            if (tileViews != null && tileViews.Count > 0 && tileViews[0].LevelTile is BeginIslandTile)
            {
                tile = (tileViews[0].LevelTile as BeginIslandTile);
                meshName = tile.MeshName;
            }
            if (meshName == null) return;
            



            float margin;
            staticNode = sceneMgr.CreateSceneNode(mainNode.Name + "Static");

            if (!backgroundDummy)
            {
                count =  tileViews.Count;
                margin = 4.5f;
            }
            else
            {
                margin = 0.3f;
            }
            float maxX = -((Math.Abs(count) - 1) * LevelView.TileWidth);
           
           

          
           
            string meshFilename;
            switch (meshName)
            {
                case "Island1": //5
                    //ISLAND1
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, 0.7f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, 0.7f);
                    break;

                case "Island1a": //5
                    //ISLAND1
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, 0.7f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, 0.7f);
                    break;
                case "IslandRound": //4
                    //ISLAND ROUND
                    initNonCollisionTreesCircle(sceneMgr, staticNode, 15.0f, 1.3f);
                    break;

                case "Island2": //6
                    //ISLAND2
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, 0.7f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, 0.7f);
                    break;
                case "Laguna": //7
                    //LAGUNA
                    break;


                case "DoubleLaguna": //8
                    // DOUBLE LAGUNA
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -5.5f, 0.0f, maxX * 0.1f, maxX, 0.6f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -1.5f, 30.0f, maxX * 0.00f, maxX * 0.15f, 0.6f);

                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -1.5f, 30.0f, maxX * 0.85f, maxX * 1.00f, 0.6f);
                   
                    break;

                case "Island3": //9
                    //ISLAND3
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, 0.7f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, 0.7f);
                    break;
                case "Island12u": //12
                    //ISLAND12u
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, maxX / 4.0f, maxX / 2.0f, 0.5f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, maxX / 4.0f, maxX / 2.0f, 0.5f);
                    break;

                case "Island4": //13
                    //ISLAND4
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 5, 0.7f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -5, 0.7f);
                    break;

                case "Island18u": //18
                    //ISLAND18u
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, 10, 14, maxX / 2.5f, maxX, 0.5f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -11, -13, maxX / 2.5f, maxX, 0.5f);

                    // zwolnic pamiec
                    EnemyPlaneView epv = new EnemyPlaneView(null, framework, staticNode);
                    epv.PlaneNode.SetPosition(-3, 0.8f, -18 - 90);
                    epv.PlaneNode.Yaw(new Radian(new Degree(60)));
                    if(EngineConfig.DisplayingMinimap)
                    {
                    	epv.MinimapItem.Hide();
                    }
                    parkedPlanes.Add(epv);

                    EnemyPlaneView epv2 = new EnemyPlaneView(null, framework, staticNode);
                    epv2.PlaneNode.SetPosition(-3, 0.8f, -25 - 90);
                    epv2.PlaneNode.Yaw(new Radian(new Degree(62)));
                    if(EngineConfig.DisplayingMinimap)
                    {
                    	epv2.MinimapItem.Hide();
                    }
                   
                    parkedPlanes.Add(epv2);

                    initLampPosts(staticNode, -7, maxX * 0.55f, maxX * 0.95f, 12, new Radian(new Degree(0)));
                    break;


                case "Island5": //24
                    //ISLAND5
                   
                    if (backgroundDummy)
                    {
                        initNonCollisionTreesDiamond(sceneMgr, staticNode, 1, 15, 0.7f);
                        initNonCollisionTreesDiamond(sceneMgr, staticNode, -1, -15, 0.7f);
                    }
                    else
                    {
                        initNonCollisionTreesDiamond(sceneMgr, staticNode, margin, 15, 0.7f);
                        initNonCollisionTreesDiamond(sceneMgr, staticNode, -margin, -15, 0.7f);
                    }
                    break;

                case "Island6": //42
                    //ISLAND6
                    if (EngineConfig.LowDetails)
                    {
                        initNonCollisionTreesDiamond(sceneMgr, staticNode, 4.5f, 33, 0.5f);
                    }
                    else initNonCollisionTreesDiamond(sceneMgr, staticNode, 4.5f, 33, 0.9f);

                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -4.5f, -60, 0.5f);
                    initNonCollisionTreesDiamond(sceneMgr, staticNode, -4.5f, -30, 0.5f);
                    break;
                default:
                    return;
            }

            if (EngineConfig.LowDetails && MeshManager.Singleton.ResourceExists(meshName + "_low" + ViewHelper.C_MESH_EXT))
            {
                meshFilename = "_low" + ViewHelper.C_MESH_EXT;
            }
            else
            {
                meshFilename = meshName + ViewHelper.C_MESH_EXT;
            }


            compositeModel = sceneMgr.CreateEntity(name, meshFilename);
            compositeModel.CastShadows = false;// EngineConfig.ShadowsQuality > 0;
           
          //  compositeModel.SetMaterialName("Carrier");
      //      compositeModel.SetMaterialName("Carrier/Panels");
           /* 
            Mogre.Plane mPlane = new Mogre.Plane(Vector3.UNIT_Y, new Vector3(0,Mogre.Math.RangeRandom(0,1) ,0));
	     
            MeshManager.Singleton.CreatePlane("Myplane"+this.GetHashCode(),
	            ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, mPlane, 1000,1000,50,50,true,1,5,5,Vector3.UNIT_Z);
	        
            Entity pPlaneEnt = sceneMgr.CreateEntity( "plane"+this.GetHashCode(), "Myplane"+this.GetHashCode() );
            
            compositeModel = pPlaneEnt;
            compositeModel.SetMaterialName("Concrete");
*/

            if (!backgroundDummy)
            {
                staticNode.Translate(new Vector3(UnitConverter.LogicToWorldUnits(firstTileIndex), 1.00f, 0));
                staticNode.SetDirection(Vector3.UNIT_X);
            }
            else
            {
                float angle;

                if (meshName.Equals("Island6"))
                {
                    float X = (islandCounter%5*250) - 650;
                    float Z = Math.RangeRandom(-4, 0)*70;
                    staticNode.Translate(new Vector3(-250 + Z, 0, Math.RangeRandom(-120, 120) + X));
                    staticNode.SetDirection(Vector3.UNIT_X);
                    angle = Math.RangeRandom(0, 2*Math.PI);
                }
                else
                {

                    staticNode.Translate(new Vector3(-450 + Math.RangeRandom(-150, 150), 0, Math.RangeRandom(-200, 200)));
                    staticNode.SetDirection(Vector3.UNIT_X);
                    angle = Math.HALF_PI;
                }

                staticNode.Yaw(angle);
            }
            //  StaticGeometry sg;
            staticNode.AttachObject(compositeModel);

            /* 
            if (sceneMgr.HasStaticGeometry(this.name + "_StaticGeometry"))
            {
                sceneMgr.DestroyStaticGeometry(this.name + "_StaticGeometry");               
            }
            sg = new StaticGeometry(sceneMgr, this.name + "_StaticGeometry");
            sg.Reset();
            Vector3 sgSize = compositeModel.BoundingBox.Size;
            sg.RegionDimensions = sgSize; 
            sg.AddSceneNode(staticNode);
            sg.Build();
          */

            //  mainNode.SetDirection(Vector3.UNIT_X);
            //  mainNode.Position = staticNode.Position;
            mainNode.AddChild(staticNode);


            //   mainNode.AttachObject(compositeModel);
            // elementy na wyspie sa animowalne wiec nie beda w static geometry
            if (!backgroundDummy)
            {
                for (int i = 0; i < count; i++)
                {
                    tileViews[i].initOnScene(staticNode, i + 1, count);
                }

                // minimapa
                if (EngineConfig.DisplayingMinimap)
                {
                    minimapItem =
                        new MinimapItem(staticNode, framework.MinimapMgr, "Cube.mesh",
                                        new ColourValue(1, 0.9137f, 0.29f), compositeModel);
                    minimapItem.Entity.SetMaterialName("Minimap/Island");
                    minimapItem.ScaleOverride = new Vector2(count * 10.0f, 3); // stala wysokosc wyspy, niezale¿na od bounding box
                 //   minimapItem.MinimapNode.Translate(0,0,50);
                    minimapItem.Refresh();
                }
            }
        }
        protected void initPalm(SceneManager sceneMgr1, SceneNode parent, Vector3 position)
        {
            initPalm(sceneMgr1, parent, position, false, true);   

        }
        public static SceneNode initPalm(SceneManager sceneMgr1, SceneNode parent, Vector3 position, bool forceLowDetails)
        {
           return initPalm( sceneMgr1, parent, position, forceLowDetails, true);   

        }
        public static SceneNode initPalm(SceneManager sceneMgr, SceneNode parent, Vector3 position, bool forceLowDetails, bool rayBasedY)
        {
            Entity palm;
            SceneNode palmNode;
            
           /* if(rayBasedY)
        	{
        		position = ViewHelper.GetVerticalRayIntersection(sceneMgr, parent, position);
        	}*/

            int id = LevelView.PropCounter;
            if (EngineConfig.LowDetails || forceLowDetails)
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "TwoSidedPlane.mesh");
                palm.SetMaterialName("FakePalmTree");
                
            }
            else
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "PalmTree.mesh");
            }

            palm.CastShadows = EngineConfig.ShadowsQuality > 0; 

            palmNode = parent.CreateChildSceneNode("PalmNode" + LevelView.PropCounter, position);
          
            if (EngineConfig.LowDetails || forceLowDetails)
            {

                float angle = Math.RangeRandom(-Math.PI/5, Math.PI/5);

                palmNode.Yaw(Math.HALF_PI + angle);
                palmNode.Scale(0.5f, 1, 1);
                palmNode.Translate(new Vector3(0, 2.5f, 0));
                palmNode.Pitch(-Math.HALF_PI);
                //palmNode.Pitch(Math.PI);
                Quaternion q = new Quaternion(new Radian(new Degree(20)), Vector3.UNIT_X);
                Quaternion q2 = new Quaternion(new Radian(new Degree(-20)), Vector3.UNIT_X);

                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "0" , EffectsManager.EffectType.PALMTOP1, position + new Vector3(0f, 4.0f, -0.0f), new Vector2(1.4f, 1.4f),
                                         q, true).Node.Yaw(angle);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "1", EffectsManager.EffectType.PALMTOP1, position + new Vector3(0f, 4.0f, -0.0f), new Vector2(1.4f, 1.4f),
                                         q2, true).Node.Yaw(angle);
            }
            else
            {
                palmNode.Rotate(Vector3.UNIT_Y, Math.RangeRandom(0.0f, Math.PI));
                palmNode.Scale(1, Math.RangeRandom(0.9f, 1.1f), 1);
            }
            palmNode.AttachObject(palm);

            return palmNode;

           
        }

        private void initPalm2(SceneManager sceneMgr, SceneNode parent, Vector3 position)
        {
            initPalm2(sceneMgr, parent, position, backgroundDummy);
        }
        
        
        
        /// <summary>
        ///  Uwaga, wspolrzedna Y wektora pozycji jest pomijana i obliczany jest promien kolizji. Na jego podstawie liczony jest wlasciwy Y
        ///  [UWAGA: na razie to nie jest zaimplementowane z uwagi na problemy z wydajnoœci¹ tego rozwi¹zania
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="forceLowDetails"></param>
        /// <param name="rayBasedY"></param>
        public static SceneNode initPalm2(SceneManager sceneMgr, SceneNode parent, Vector3 position, bool forceLowDetails, bool rayBasedY)
        {
        	/*if(rayBasedY)
        	{
        		position = ViewHelper.GetVerticalRayIntersection(sceneMgr, parent, position);
        	}*/
			
			Entity palm;
            SceneNode palmNode;


            float id = LevelView.PropCounter;

            if (EngineConfig.LowDetails || forceLowDetails)
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "TwoSidedPlane.mesh");
                palm.SetMaterialName("FakePalmTree2");
            }
            else
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "PalmTree2.mesh");
            }

            palm.CastShadows = EngineConfig.ShadowsQuality > 0; 

            palmNode = parent.CreateChildSceneNode("PalmNode" + LevelView.PropCounter, position);

            if (EngineConfig.LowDetails || forceLowDetails)
            {
                float angle = Math.RangeRandom(-Math.PI/5, Math.PI/5);

                palmNode.Yaw(Math.HALF_PI + angle);
                palmNode.Scale(0.5f, 1, 1);
                palmNode.Translate(new Vector3(0, 2.5f, 0));
                palmNode.Pitch(-Math.HALF_PI);
                Quaternion q = new Quaternion(new Radian(new Degree(30)), Vector3.UNIT_X);
                Quaternion q2 = new Quaternion(new Radian(new Degree(-30)), Vector3.UNIT_X);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "0", EffectsManager.EffectType.PALMTOP2, position + new Vector3(0.0f, 4.0f, -0.2f), new Vector2(2.5f, 2.5f),
                                    q, true).Node.Yaw(angle);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "1", EffectsManager.EffectType.PALMTOP2, position + new Vector3(0.0f, 4.0f, -0.2f), new Vector2(2.5f, 2.5f),
                                  q2, true).Node.Yaw(angle);
            }
            else
            {
                palmNode.Rotate(Vector3.UNIT_Y, Math.RangeRandom(0.0f, Math.PI));
                palmNode.Scale(1, Math.RangeRandom(0.9f, 1.1f), 1);
                palmNode.Translate(new Vector3(0, -0.0f, 0));
            }
            palmNode.AttachObject(palm);
            return palmNode;
        	
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="forceLowDetails"></param>
        public static SceneNode initPalm2(SceneManager sceneMgr, SceneNode parent, Vector3 position, bool forceLowDetails)
        {
        	return initPalm2(sceneMgr, parent, position, forceLowDetails, true);

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

        private void initNonCollisionTreesDiamond(SceneManager sceneMgr, SceneNode parent, float zMin, float zMax)
        {
            initNonCollisionTreesDiamond(sceneMgr, parent, zMin, zMax, 1);
        }

        private void initLampPosts(SceneNode parent, float z, float xMin, float xMax, float num, Radian direction)
        {
            float dist = xMin; 
            for (int i = 0; i < num; i++)
            {
                initLampPost(parent, new Vector3(z, -0.5f, dist), direction);
                dist += (xMax - xMin) / num;
               
            }
        }
        public void initNonCollisionTreesDiamond(SceneManager sceneMgr1, SceneNode parent, float zMin, float zMax, float xMin, float xMax, float intensity)
        {
            initNonCollisionTreesDiamond(sceneMgr1, parent, zMin, zMax, xMin, xMax, intensity, backgroundDummy);
        }

        private void initNonCollisionTreesDiamond(SceneManager sceneMgr1, SceneNode parent, float zMin, float zMax, float xMin, float xMax, float intensity, bool forceLowDetails)
        {
            int c = (int)Math.Abs(count);
            int count_l = (int)(c * 2 * intensity);

            for (int i = 0; i < count_l; i++)
            {
                float z = Math.RangeRandom(zMin, zMax);

                if (i % 10 == 1) //Co dziesiata palma jest z wieksza iloscia trojkatow
                {
                    initPalm(sceneMgr1, parent, new Vector3(z, -0.5f, Math.RangeRandom(xMin, xMax)), forceLowDetails);
                }
                else
                {
                    initPalm2(sceneMgr1, parent, new Vector3(z, -0.5f, Math.RangeRandom(xMin, xMax)), forceLowDetails);
                }
            }
        }

        private void initNonCollisionTreesCircle(SceneManager sceneMgr, SceneNode parent, float radius, float intensity)
        {
            initNonCollisionTreesCircle(sceneMgr, parent, radius, intensity, backgroundDummy);
        }

        private void initNonCollisionTreesCircle(SceneManager sceneMgr, SceneNode parent, float radius, float intensity, bool forceLowDetails)
        {
            int c = (int)Math.Abs(count);
            int count_l = (int)(c * 2 * intensity);

            for (int i = 0; i < count_l; i++)
            {
                initPalm2(sceneMgr, parent, new Vector3(Math.RangeRandom(-radius, radius), -0.5f, Math.RangeRandom(-radius, radius) - 25), forceLowDetails);
            }
        }

        private void initNonCollisionTreesDiamond(SceneManager sceneMgr, SceneNode parent, float zMin, float zMax, float intensity)
        {
            initNonCollisionTreesDiamond(sceneMgr, parent, zMin, zMax, intensity, backgroundDummy);
        }

        private void initNonCollisionTreesDiamond(SceneManager sceneMgr, SceneNode parent, float zMin, float zMax, float intensity, bool forceLowDetails)
        {
            int c = (int)Math.Abs(count);
            float max = - c * LevelView.TileWidth;
            initNonCollisionTreesDiamond(sceneMgr, parent, zMin, zMax, 0.1f * max, 0.9f * max, intensity, forceLowDetails);
        }


        ~IslandView()
        {
            parkedPlanes.Clear();
            parkedPlanes = null;
            /*for (int i=0; i < parkedPlanes.Count; i++)
            {
                parkedPlanes[i].
            }*/
        }
    }
}