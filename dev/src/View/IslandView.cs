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
using Wof.View.Effects;
using Math=Mogre.Math;
using Wof.View.TileViews;

namespace Wof.View
{
    internal class IslandView : CompositeModelView
    {
        private static int islandCounter = 0;
        protected SceneNode staticNode;

        public SceneNode StaticNode
        {
            get { return staticNode; }
        }

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
        /// <param name="tileViews">Lista pól Views wyspy</param>
        /// <param name="framework">Standardowy framework Ogre'a</param>
        /// <param name="parentNode">SceneNode który bêdzie zawiera³ w sobie Node'a wyspy</param>
        /// <author>Kamil S³awiñski</author>
        public IslandView(List<TileView> tileViews, FrameWork framework, SceneNode parentNode)
            : base(tileViews, framework, parentNode, "Island" + (++islandCounter))
        {
            initOnScene();
        }

        /// <summary>
        /// Konstruktor dla wysp na drugim planie / nie biora udzialu w grze
        /// </summary>
        /// <param name="length">D³ugoœæ wyspy</param>
        /// <param name="framework">Standardowy framework Ogre'a</param>
        /// <param name="parentNode">SceneNode który bêdzie zawiera³ w sobie Node'a wyspy</param>
        /// <author>Kamil S³awiñski</author>
        public IslandView(int length, FrameWork framework, SceneNode parentNode)
            : base(null, framework, parentNode, "Island" + (++islandCounter))
        {
            count = length;
            initOnScene();
        }

        protected override void initOnScene()
        {
            String islandMeshName; //Nazwa modelu  

            float adjust, margin;
            staticNode = sceneMgr.CreateSceneNode(mainNode.Name + "Static");

            if (!backgroundDummy)
            {
                count = tileViews.Count;
                margin = 4.5f;
            }
            else
            {
                margin = 0.3f;
            }

            switch (count)
            {
                case 5: //5
                    //ISLAND1
                    islandMeshName = "Island1.mesh";
                    adjust = 0;
                    initNonColissionTrees(staticNode, margin, 5, 0.7f);
                    initNonColissionTrees(staticNode, -margin, -5, 0.7f);
                    break;
                case -6: //4
                    //ISLAND ROUND
                    islandMeshName = "IslandRound.mesh";
                    adjust = 0;
                    initNonColissionTrees(staticNode, margin, 10, 0.7f);
                    initNonColissionTrees(staticNode, -margin, -10, 0.7f);
                    break;

                case 6: //6
                    //ISLAND2
                    islandMeshName = "Island2.mesh";
                    adjust = LevelView.TileWidth*0.5f;
                    initNonColissionTrees(staticNode, margin, 5, 0.7f);
                    initNonColissionTrees(staticNode, -margin, -5, 0.7f);
                    break;
                case 9: //9
                    //ISLAND3
                    islandMeshName = "Island3.mesh";
                    adjust = LevelView.TileWidth*2;
                    initNonColissionTrees(staticNode, margin, 5, 0.7f);
                    initNonColissionTrees(staticNode, -margin, -5, 0.7f);
                    break;
                case 13: //13
                    //ISLAND4
                    islandMeshName = "Island4.mesh";
                    adjust = LevelView.TileWidth*4;
                    initNonColissionTrees(staticNode, margin, 5, 0.7f);
                    initNonColissionTrees(staticNode, -margin, -5, 0.7f);
                    break;
                case 24:
                    //ISLAND5
                    if (EngineConfig.LowDetails)
                    {
                        islandMeshName = "Island5_low.mesh";
                    }
                    else
                    {
                        islandMeshName = "Island5.mesh";
                    }

                    adjust = LevelView.TileWidth*9.5f;

                    if (backgroundDummy)
                    {
                        initNonColissionTrees(staticNode, 1, 15, 0.7f);
                        initNonColissionTrees(staticNode, -1, -15, 0.7f);
                    }
                    else
                    {
                        initNonColissionTrees(staticNode, margin, 15, 0.7f);
                        initNonColissionTrees(staticNode, -margin, -15, 0.7f);
                    }
                    break;
                case 42:
                    //ISLAND6
                    if (EngineConfig.LowDetails)
                    {
                        islandMeshName = "Island6_low.mesh";
                    }
                    else
                    {
                        islandMeshName = "Island6.mesh";
                    }

                    adjust = LevelView.TileWidth*18.5f;
                    if (EngineConfig.LowDetails)
                    {
                        initNonColissionTrees(staticNode, 4.5f, 33, 0.5f);
                    }
                    else initNonColissionTrees(staticNode, 4.5f, 33, 0.9f);

                    initNonColissionTrees(staticNode, -4.5f, -60, 0.5f);
                    initNonColissionTrees(staticNode, -4.5f, -30, 0.5f);
                    break;
                default:
                    return;
            }

            compositeModel = sceneMgr.CreateEntity(name, islandMeshName);
            compositeModel.CastShadows = EngineConfig.Shadows;

            if (!backgroundDummy)
            {
                staticNode.Translate(new Vector3(LevelView.CurrentTilePositionOnScene - adjust, 1.00f, 0));
                staticNode.SetDirection(Vector3.UNIT_X);
            }
            else
            {
                float angle;

                if (count != 42)
                {
                    float X = (islandCounter%5*250) - 650;
                    float Z = Math.RangeRandom(-4, 0)*50;
                    staticNode.Translate(new Vector3(-250 + Z, 1, LevelView.CurrentTilePositionOnScene - adjust + X));
                    staticNode.SetDirection(Vector3.UNIT_X);
                    angle = Math.RangeRandom(0, 2*Math.PI);
                }
                else
                {
                    staticNode.Translate(new Vector3(-350, 1, LevelView.CurrentTilePositionOnScene - adjust + 100));
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
                    tileViews[i].initOnScene(staticNode, i + 1, tileViews.Count);
                }

                // minimapa
                if (FrameWork.DisplayMinimap)
                {
                    minimapItem =
                        new MinimapItem(staticNode, FrameWork.MinimapMgr, "Cube.mesh",
                                        new ColourValue(1, 0.9137f, 0.29f), compositeModel);
                    minimapItem.ScaleOverride = new Vector2(0, 3); // stala wysokosc wyspy, niezale¿na od bounding box
                    minimapItem.Refresh();
                }
            }
        }

        protected void initPalm(SceneNode parent, Vector3 position)
        {
            Entity palm;
            SceneNode palmNode;

            int id = LevelView.PropCounter;
            if (EngineConfig.LowDetails)
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "TwoSidedPlane.mesh");

                palm.SetMaterialName("FakePalmTree");
            }
            else
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "PalmTree.mesh");
            }

            palm.CastShadows = EngineConfig.Shadows;

            palmNode = parent.CreateChildSceneNode("PalmNode" + LevelView.PropCounter, position);

            if (EngineConfig.LowDetails)
            {

                float angle = Math.RangeRandom(-Math.PI/5, Math.PI/5);

                palmNode.Yaw(Math.HALF_PI + angle);
                palmNode.Scale(0.5f, 1, 1);
                palmNode.Translate(new Vector3(0, 3, 0));
                palmNode.Pitch(Math.HALF_PI);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "PalmTop" + id, EffectsManager.EffectType.PALMTOP1, position + new Vector3(0f, 4.5f, -0.0f), new Vector2(1.8f, 1.8f),
                                         Quaternion.IDENTITY, true).Node.Yaw(angle);
            }
            else
            {
                palmNode.Rotate(Vector3.UNIT_Y, Math.RangeRandom(0.0f, Math.PI));
                palmNode.Scale(1, Math.RangeRandom(0.9f, 1.1f), 1);
            }
            palmNode.AttachObject(palm);


           
        }

        protected void initPalm2(SceneNode parent, Vector3 position)
        {
            Entity palm;
            SceneNode palmNode;


            float id = LevelView.PropCounter;

            if (EngineConfig.LowDetails)
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "TwoSidedPlane.mesh");
                palm.SetMaterialName("FakePalmTree2");
            }
            else
            {
                palm = sceneMgr.CreateEntity("Palm" + id, "PalmTree2.mesh");
            }

            palm.CastShadows = EngineConfig.Shadows;

            palmNode = parent.CreateChildSceneNode("PalmNode" + LevelView.PropCounter, position);

            if (EngineConfig.LowDetails)
            {
                float angle = Math.RangeRandom(-Math.PI/5, Math.PI/5);

                palmNode.Yaw(Math.HALF_PI + angle);
                palmNode.Scale(0.5f, 1, 1);
                palmNode.Translate(new Vector3(0, 3, 0));
                palmNode.Pitch(Math.HALF_PI);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, parent, "PalmTop" + id, EffectsManager.EffectType.PALMTOP2, position + new Vector3(0.0f, 4.5f, -0.2f), new Vector2(2.5f, 2.5f),
                                    Quaternion.IDENTITY, true).Node.Yaw(angle);
            }
            else
            {
                palmNode.Rotate(Vector3.UNIT_Y, Math.RangeRandom(0.0f, Math.PI));
                palmNode.Scale(1, Math.RangeRandom(0.9f, 1.1f), 1);
                palmNode.Translate(new Vector3(0, -0.3f, 0));
            }
            palmNode.AttachObject(palm);


        }

        private void initNonColissionTrees(SceneNode parent, float zMin, float zMax)
        {
            initNonColissionTrees(parent, zMin, zMax, 1);
        }

        private void initNonColissionTrees(SceneNode parent, float zMin, float zMax, float intensity)
        {
            int c = (int) Math.Abs(count);
            float max = (c - 1)*LevelView.TileWidth/16;

            int count_l;

            /* if (EngineConfig.LowDetails)
            {
                count_l = c / 10;
            }
            else
            {*/
            count_l = (int) (c*2*intensity);
            //}
            for (int i = 0; i < count_l; i++)
            {
                float z = Math.RangeRandom(zMin, zMax);
                float adjust = Math.Abs((z - zMin)/(4*zMin));

                if (i%10 == 1) //Co dziesiata palma jest z wieksza iloscia trojkatow
                {
                    initPalm(parent, new Vector3(z, -0.5f, Math.RangeRandom(-max, max)*(8 - adjust)));
                }
                else
                {
                    initPalm2(parent, new Vector3(z, -0.5f, Math.RangeRandom(-max, max)*(8 - adjust)));
                }
            }
        }
    }
}