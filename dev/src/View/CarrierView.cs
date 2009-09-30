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
using Wof.Model.Level;
using Wof.Model.Level.Carriers;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.Planes;
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using Wof.View.TileViews;
using Math=Mogre.Math;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    /// <summary>
    /// Widok lotniskowca 
    /// <author>Kamil S³awiñski, Adam Witczak</author>
    /// </summary>
    internal class CarrierView : CompositeModelView
    {
        private static int carrierCounter = 0;

        private static int hangarDepth = 3;

        protected Entity carrierAerial1;
        protected Entity carrierAerial2;
        protected Entity carrierHangar;


        protected bool isHangaringPlane = false;
        protected PlaneView hangarPlane;

       
        private int hangaringDirection = 1;
        private bool isHangaringFinished = false;
     
        protected SceneNode carrierAerial1Node;
        protected SceneNode carrierAerial2Node;
        protected SceneNode carrierHangarNode;
      
        private ConstRotateNodeAnimation aerialAnimation1;
        private ConstRotateNodeAnimation aerialAnimation2;

        protected List<PlaneView> storagePlanes;

        protected List<SceneNode> arrestingWires;
        protected List<SceneNode> activeArrestingWires;

        /// <summary>
        /// wyskosc trojkata rozpietego przez liny hamuj¹ce. 
        /// Kazda wartosc to ostatnia odleg³oœæ tylnego ko³a samolotu od bazy liny hamuj¹cej.
        /// Przy okazji jest to wysokoœæ trójk¹t rozpiêtego miêdzy linami hamuj¹cymi w po³o¿eniu pocz¹tkowym a linami w po³o¿eniu hamuj¹cym
        /// Potrzebne do eleganckiego powrotu lin na swoje inicjalne po³o¿enie
        /// </summary>
        protected float[] arrestingWiresH;

        protected PlaneView planeBeingCaught;

        protected AnimationState[] crewAnimationStates = null;

        protected AnimationState japanFlagState;
        protected Entity japanFlag;

        protected SceneNode flagNode;

       

        #region Minimap representation

        protected MinimapItem minimapItem;

        public MinimapItem MinimapItem
        {
            get { return minimapItem; }
        }

        #endregion

        #region ArrestingWires

        protected const float arrestingWiresSpan = 9.0f;

        public float ArrestingWiresSpan
        {
            get { return arrestingWiresSpan; }
        }

        protected bool isCatchingPlane = false;

        public bool IsCatchingPlane
        {
            get { return isCatchingPlane; }
        }

        protected bool isReleasingPlane = false;

        public bool IsReleasingPlane
        {
            get { return isReleasingPlane; }
        }

        public PlaneView HangarPlane
        {
            get { return hangarPlane; }
        }

        #endregion

        // protected float lastH;

        public CarrierView(int tileIndex, FrameWork framework, SceneNode parentNode)
            : base(tileIndex, framework, parentNode, "Carrier" + (++carrierCounter))
        {
            initOnScene();
        }


        public CarrierView(List<TileView> tileViews, FrameWork framework, SceneNode parentNode)
            : base(tileViews, framework, parentNode, "Carrier" + (++carrierCounter))
        {
            
            initOnScene();

        }





        public void CrewStatePlaneOnCarrier()
        {
            int count = crewAnimationStates.Length;

            for (int i = 0; i < count; i++)
            {
                AnimationState s = crewAnimationStates[i];
                s.Enabled = true;
                s.Loop = true;
            }
        }

        public void CrewStatePlaneAirborne()
        {
            crewAnimationStates[0].Enabled = true;
            crewAnimationStates[0].Loop = false;
        }

        protected void AddCrew()
        {
            crewAnimationStates = new AnimationState[3];
            SceneNode crewNode;
            Entity crew, signal;

            Billboard lightbillboard;
            BillboardSet lightbillboardset;

            for (int i = 0; i <= 2; i++)
            {
                crew = sceneMgr.CreateEntity(name + "Crew" + i, "CarrierCrew.mesh");

                crewNode = mainNode.CreateChildSceneNode(name + "Crew" + i + "Node");
                crewNode.Scale(0.8f, 0.8f, 0.8f);
                if (i == 0)
                {
                    crewNode.Position = new Vector3(-7.6f, 0.4f, 0.0f - 47.5f);
                    crewNode.Yaw(-Math.HALF_PI);
                    crewAnimationStates[i] = crew.GetAnimationState("start");

                    // znak ktorym macha obsluga lotniskowca
                    Quaternion q = new Quaternion(new Radian(new Degree(90)), Vector3.NEGATIVE_UNIT_X);
                    signal = sceneMgr.CreateEntity(name + "LSignal", "Signal.mesh");
                    crew.AttachObjectToBone("Bip01_L_Hand", signal, q, new Vector3(0.07f, 0, 0));

                    if (LevelView.IsNightScene)
                    {
                        // swiatelko
                        lightbillboardset = sceneMgr.CreateBillboardSet(name + "_crewlights1", 1);
                        lightbillboardset.MaterialName = "Effects/Flare";
                        lightbillboardset.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
                        lightbillboard = lightbillboardset.CreateBillboard(0, 0, 0, new ColourValue(0, 1, 0));
                        lightbillboard.SetDimensions(1, 1);
                        crew.AttachObjectToBone("Bip01_L_Hand", lightbillboardset, q, new Vector3(0.08f, 0.1f, -0.80f));
                    }


                    signal = sceneMgr.CreateEntity(name + "RSignal", "Signal.mesh");
                    q = new Quaternion(new Radian(new Degree(120)), Vector3.UNIT_X);
                    crew.AttachObjectToBone("Bip01_R_Hand", signal, q, new Vector3(0.07f, 0, 0));

                    if (LevelView.IsNightScene)
                    {
                        // swiatelko
                        lightbillboardset = sceneMgr.CreateBillboardSet(name + "_crewlights2", 1);
                        lightbillboardset.MaterialName = "Effects/Flare";
                        lightbillboardset.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
                        lightbillboard = lightbillboardset.CreateBillboard(0, 0, 0, new ColourValue(0, 1, 0));
                        lightbillboard.SetDimensions(1, 1);
                        crew.AttachObjectToBone("Bip01_R_Hand", lightbillboardset, q, new Vector3(0.07f, -0.3f, 0.8f));
                    }
                }
                else if (i == 1)
                {
                    crewNode.Position = new Vector3(-7.6f, 0.3f, -30.0f - 47.5f);
                    crewNode.Yaw(new Radian(-1.2f));
                    crewAnimationStates[i] = crew.GetAnimationState("idle");
                    crewAnimationStates[i].TimePosition = crewAnimationStates[i].Length/2.0f;
                }
                else if (i == 2)
                {
                    crewNode.Position = new Vector3(-7.6f, 0.3f, -32.0f - 47.5f);
                    crewNode.Yaw(new Radian(-2.2f));
                    crewAnimationStates[i] = crew.GetAnimationState("idle");
                }

                crewNode.AttachObject(crew);
            }
        }

        protected void InitLights(ColourValue c1, ColourValue c2)
        {
        	
            Billboard lightbillboard;
            BillboardSet lightbillboardset = sceneMgr.CreateBillboardSet(name + "_lights", 1);
            lightbillboardset.MaterialName = "Effects/Flare";
		
            if (!EngineConfig.LowDetails)
            {
                lightbillboard = lightbillboardset.CreateBillboard(-9.2f, 12.5f, -5.1f - 47.5f, c1);
                lightbillboard.SetDimensions(2, 2);

                Light light = sceneMgr.CreateLight(name + "_light1");
                light.Type = Light.LightTypes.LT_POINT;
            
             
                light.SetAttenuation(30.0f, 0.0f, 1.0f, 0.00f);
             
              
                light.DiffuseColour = new ColourValue(0.5f,0.0f,0.0f);
                light.SpecularColour = new ColourValue(0.05f, 0.05f, 0.05f);
                light.CastShadows = true;     
          
              
                SceneNode lightNode = mainNode.CreateChildSceneNode(new Vector3(-9.2f, 12.5f, -4.0f  -5.1f - 47.5f));
               
                lightNode.AttachObject(light);
           
            }

            lightbillboard = lightbillboardset.CreateBillboard(-10.7f, 12.3f, 12.6f - 47.5f, c2);
            lightbillboard.SetDimensions(2, 2);

            mainNode.AttachObject(lightbillboardset);
            //sceneMgr.ShowBoundingBoxes = true;
            
        }


        protected override void initOnScene()
        {
            if (tileViews != null && tileViews.Count != 9)
                return;

            storagePlanes = new List<PlaneView>();
            arrestingWires = new List<SceneNode>();

            // CARRIER

            if (EngineConfig.LowDetails)
            {
                compositeModel = sceneMgr.CreateEntity(name, "Carrier_low.mesh");
            }
            else
            {
                compositeModel = sceneMgr.CreateEntity(name, "Carrier.mesh");
              //  compositeModel = sceneMgr.CreateEntity(name, "Carrier_low.mesh");
            }

            compositeModel.CastShadows = EngineConfig.ShadowsQuality > 0; 
         

            carrierAerial1 = sceneMgr.CreateEntity(name + "Aerial1", "Aerial1.mesh");
            carrierAerial2 = sceneMgr.CreateEntity(name + "Aerial2", "Aerial2.mesh");
          // compositeModel.SetMaterialName("Carrier");
            mainNode.AttachObject(compositeModel);

            if (EngineConfig.LowDetails)
            {
                carrierHangar = sceneMgr.CreateEntity(name + "Hangar", "HangarFloor_low.mesh");
            }
            else
            {
                carrierHangar = sceneMgr.CreateEntity(name + "Hangar", "HangarFloor.mesh");
            }
            carrierHangarNode = mainNode.CreateChildSceneNode(name + "HangarNode", Vector3.ZERO);

            carrierHangarNode.AttachObject(carrierHangar);



            carrierAerial1Node = mainNode.CreateChildSceneNode(name + "Aerial1Node", new Vector3(-8.6f, 9.1f, 5.7f - 47.8f));
            carrierAerial1Node.AttachObject(carrierAerial1);

            carrierAerial2Node = mainNode.CreateChildSceneNode(name + "Aerial2Node", new Vector3(-10.7f, 4.0f, 12.6f - 47.8f));
            carrierAerial2Node.AttachObject(carrierAerial2);

            Vector3 oVector = new Vector3(0, 1, 0);

            aerialAnimation1 = new ConstRotateNodeAnimation(carrierAerial1Node, 40, oVector, "ConstRot");
            aerialAnimation1.Enabled = true;
            aerialAnimation1.Looped = true;

            aerialAnimation2 = new ConstRotateNodeAnimation(carrierAerial2Node, -15, oVector, "ConstRot");
            aerialAnimation2.Enabled = true;
            aerialAnimation2.Looped = true;

            EffectsManager.Singleton.RectangularEffect(sceneMgr, mainNode, name + "lWaterTrailNode", EffectsManager.EffectType.WATERTRAIL, new Vector3(-4.6f, -5.6f, -41.0f - 47.5f), new Vector2(17,12), new Quaternion(new Radian(new Degree(90)), Vector3.NEGATIVE_UNIT_Y), true);
          
            EffectsManager.Singleton.RectangularEffect(sceneMgr, mainNode,  name + "rWaterTrailNode", EffectsManager.EffectType.WATERTRAIL, new Vector3(4.6f, -5.6f, -41.0f - 47.5f), new Vector2(17,12), new Quaternion(new Radian(new Degree(90)), Vector3.NEGATIVE_UNIT_Y), true);
                
          
           

            float adjust = LevelView.TileWidth*1.7f;

            // CARRIER

            if (!backgroundDummy)
            {
                InitArrestingWires();

                AddCrew();

                InitLights(new ColourValue(0.9f, 0.2f, 0.0f), new ColourValue(0.0f, 0.2f, 0.9f));

                CrewStatePlaneOnCarrier();

                mainNode.Translate(new Vector3(UnitConverter.LogicToWorldUnits(firstTileIndex), 5, -1.0f));
                mainNode.SetDirection(Vector3.UNIT_X);
              //  mainNode.Roll(-Math.HALF_PI / 6.0f);
                if (FrameWork.DisplayMinimap)
                {
                    minimapItem =
                        new MinimapItem(mainNode, FrameWork.MinimapMgr, "CarrierMinimap.mesh",
                                        new ColourValue(0.162f, 0.362f, 0.59f), compositeModel);
                    minimapItem.ScaleOverride = new Vector2(0, 15); // stala wysokosc, niezale¿na od bounding box
                    minimapItem.Refresh();
                }
            }
            else
            {
                initFlag(new Vector3(0, 0, -45));
                mainNode.Translate(new Vector3(-800.0f, 5, UnitConverter.LogicToWorldUnits(firstTileIndex) + 1000));
                InitLights(new ColourValue(0.9f, 0.2f, 0.0f), new ColourValue(0.0f, 0.9f, 0.1f));
            }
        }

        protected void initFlag(Vector3 position)
        {
            japanFlag = sceneMgr.CreateEntity("JapanFlag" + LevelView.PropCounter.ToString(), "JapanFlag.mesh");

            flagNode = mainNode.CreateChildSceneNode("JapanFlagNode" + LevelView.PropCounter.ToString(), position);

            flagNode.Rotate(Vector3.UNIT_Y, Math.PI);
            flagNode.AttachObject(japanFlag);

            flagNode.Scale(new Vector3(6, 5.5f, 6));

            japanFlagState = japanFlag.GetAnimationState("idle");
            japanFlagState.Enabled = true;
            japanFlagState.Loop = true;
        }

        public void InitStoragePlaneOnCarrier(StoragePlane plane)
        {
            PlaneView planeView = new P47PlaneView(plane, sceneMgr, sceneMgr.RootSceneNode, "StoragePlane");
            storagePlanes.Add(planeView);
            int index = storagePlanes.Count;

            if (FrameWork.DisplayMinimap) planeView.MinimapItem.Hide();
            SceneNode p47Node = planeView.OuterNode;

            if (index == 1)
            {
                p47Node.Yaw(new Radian(Math.HALF_PI));
                p47Node.Pitch(0.1f);
                p47Node.Roll(0.03f);
                //p47Node.Translate(new Vector3(5.9f, 0.33f, 0));
                p47Node.Translate(new Vector3(5.9f, -0.23f, 0));
            }
            if (index == 2)
            {
                p47Node.Yaw(new Radian(Math.HALF_PI/2.0f));
                p47Node.Pitch(0.1f);
                //p47Node.Translate(new Vector3(5.9f, 0.24f, 0));
                p47Node.Translate(new Vector3(5.9f, -0.14f, 0));
            }

            //Ustawia zlozone skrzydla
            planeView.PlaneEntity.GetAnimationState("storage").Enabled = true;
        }

        
        public PlaneView FindStoragePlaneView(StoragePlane sp)
        {
            return storagePlanes.Find(delegate(PlaneView pv) { return pv.Plane == sp; });
        }

        public bool RemoveNextStoragePlane()
        {
            if (storagePlanes.Count == 0) return false;
            PlaneView view = storagePlanes[storagePlanes.Count - 1];
            view.PlaneNode.SetVisible(false);


            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                ViewHelper.DetachQuadrangles(sceneMgr, view.Plane);
            }

            storagePlanes.RemoveAt(storagePlanes.Count - 1);
            return true;
        }


        public void DestoryStoragePlanes()
        {
            if (storagePlanes.Count == 0) return;
            foreach(TileView tv in tileViews)
            {
                if (tv.LevelTile is AircraftCarrierTile)
                {
                    DestoryStoragePlane(tv.LevelTile as AircraftCarrierTile);
                }
            }
        }

        public bool DestoryStoragePlane(AircraftCarrierTile storageTile)
        {
            if (storagePlanes.Count == 0) return false;
            PlaneView view =
                storagePlanes.Find(delegate(PlaneView pv) { return (pv.Plane as StoragePlane).Tile == storageTile; });
            if (view == null) return false;
            
            view.Smash(); // ustawia animation state na 'die'
            return true;
        }

        /// <summary>
        /// Dodaje liny hamuj¹ce do lotniskowca
        /// </summary>
        protected void InitArrestingWires()
        {
            // arresting wires
            Carrier lCarrier = new Carrier(tileViews);

            float carrierBegin = lCarrier.GetBeginPosition().X;
            float carrierWidth = lCarrier.GetEndPosition().X - carrierBegin;

            float carrierCenter = carrierBegin + carrierWidth/2.0f;

            int i;
            for (i = 0; i < lCarrier.CarrierTiles.Count; i++)
            {
                if (lCarrier.CarrierTiles[i] is EndAircraftCarrierTile)
                {
                    float xPos = Mathematics.IndexToPosition(lCarrier.CarrierTiles[i].TileIndex);
                    //LevelTile.Width 
                    //+ 1.0f / 2.0f
                    float viewZ = -((float) (xPos - carrierCenter) + 47.2f );//- 6.0f); // korekta

                    InitArrestingWire(viewZ);
                }
            }
            activeArrestingWires = new List<SceneNode>(arrestingWires.Count);
            arrestingWiresH = new float[arrestingWires.Count];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="zCoordinate">Lokalna wspo³rzêdna Z po³o¿enia liny</param>
        private void InitArrestingWire(float zCoordinate)
        {
            float z = zCoordinate;
            Vector3 position = new Vector3(1.4f, 0.55f, zCoordinate);
            SceneNode wireNode = mainNode.CreateChildSceneNode(name + "_ArrestingWire" + z + "Node", position);

            SceneNode lWireNode =
                wireNode.CreateChildSceneNode(name + "_LArrestingWire" + z + "Node",
                                              new Vector3(arrestingWiresSpan/2.0f, 0.0f, 0.0f));
            initLArrestingWire(lWireNode);

            SceneNode rWireNode =
                wireNode.CreateChildSceneNode(name + "_RArrestingWire" + z + "Node",
                                              new Vector3(-arrestingWiresSpan/2.0f, 0.0f, 0.0f));
            initRArrestingWire(rWireNode);

            Entity lWire = sceneMgr.CreateEntity(name + "_LArrestingWire" + z, "ArrestingWire.mesh");
            Entity rWire = sceneMgr.CreateEntity(name + "_RArrestingWire" + z, "ArrestingWire.mesh");

            lWireNode.AttachObject(lWire);
            rWireNode.AttachObject(rWire);
            arrestingWires.Add(wireNode);
        }

        private void activateClosestArrestingWires(PlaneView p)
        {
            float diff;
            for (int i = 0; i < arrestingWires.Count; i++)
            {
                diff = p.RearWheelInnerNode.WorldPosition.x - arrestingWires[i].WorldPosition.x;
                if (Math.Abs(diff) < 0.1f && diff > 0 && !activeArrestingWires.Contains(arrestingWires[i]))
                {
                    activeArrestingWires.Add(arrestingWires[i]);
                }
            }
        }

       
        

        public bool IsHangaringFinished()
        {
            return isHangaringFinished;
        }

        public void ResetHangaringFinished()
        {
            isHangaringFinished = false;
        }


        public void StartCatchingPlane(PlaneView plane, EndAircraftCarrierTile carrierTile)
        {
          
            isCatchingPlane = true;
            if (carrierTile == null)
            {
                activeArrestingWires.Clear();
                activeArrestingWires.Add(arrestingWires[0]);
            }
            else
            {
                activateClosestArrestingWires(plane);
            }
            planeBeingCaught = plane;
        }

        public void StartReleasingPlane()
        {
            isCatchingPlane = false;
            isReleasingPlane = true;
        }

        private void initLArrestingWire(SceneNode lWire)
        {
            lWire.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_Z);
            lWire.SetScale(1, arrestingWiresSpan/2.0f, 1);
        }

        private void initRArrestingWire(SceneNode rWire)
        {
            initLArrestingWire(rWire);
            rWire.Orientation *= new Quaternion(-Math.PI, Vector3.UNIT_Z);
        }


        private float animateArrestingWire(SceneNode arrestingWire, float targetWorldXPos)
        {
            float lastH;
            SceneNode lWire = (SceneNode) arrestingWire.GetChild(0);
            SceneNode rWire = (SceneNode) arrestingWire.GetChild(1);
            lastH = lWire.WorldPosition.x - targetWorldXPos;
            Radian alpha = Math.ATan(lastH/(0.5f*arrestingWiresSpan)); // k¹t wychylenia kawalkow liny
            float length = lastH/Math.Sin(alpha); // dlugosc liny

            lWire.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            lWire.Orientation *= new Quaternion(Math.HALF_PI, Vector3.UNIT_Z);
            lWire.Orientation *= new Quaternion(alpha, Vector3.NEGATIVE_UNIT_Z);
            lWire.SetScale(1, length, 1);

            rWire.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            rWire.Orientation *= new Quaternion(Math.HALF_PI, Vector3.NEGATIVE_UNIT_Z);
            rWire.Orientation *= new Quaternion(alpha, Vector3.UNIT_Z);
            rWire.SetScale(1, length, 1);

            return lastH;
        }

        private void finishReleasingPlane()
        {
            for (int i = 0; i < activeArrestingWires.Count; i++) arrestingWiresH[i] = 0;
            activeArrestingWires.Clear();
            planeBeingCaught = null;
            isReleasingPlane = false;
        }

        /// <summary>
        /// Czy hamowanie zosta³o zakoñczone
        /// </summary>
        private bool shouldStopReleasingPlane()
        {
            for (int i = 0; i < arrestingWiresH.Length; i++)
            {
                if (Math.Abs(arrestingWiresH[i]) > 0.01f) return false;
            }
            return true;
        }

        public void StartHangaringPlane(PlaneView plane, int hangaringDirection)
        {
            hangarPlane = plane;
            isHangaringPlane = true;
            this.hangaringDirection = hangaringDirection;
        }



        public void updateTime(float timeSinceLastFrameUpdate)
        {
            aerialAnimation1.updateTime(timeSinceLastFrameUpdate);
            aerialAnimation1.animate();

            aerialAnimation2.updateTime(timeSinceLastFrameUpdate);
            aerialAnimation2.animate();

            if (backgroundDummy)
            {
                japanFlagState.AddTime(timeSinceLastFrameUpdate);
            }
            else
            {
                for (int i = 0; i < storagePlanes.Count; i++ )
                {
                    storagePlanes[i].updateTime(timeSinceLastFrameUpdate);
                    if(storagePlanes[i].AnimationState.AnimationName == "die" && storagePlanes[i].AnimationState.HasEnded)
                    {
                        storagePlanes[i].MinimapItem.Hide();
                        storagePlanes.Remove(storagePlanes[i]);
                    }
                }

                for (int i = 0; i < crewAnimationStates.Length; i++)
                {
                    crewAnimationStates[i].AddTime(timeSinceLastFrameUpdate);
                }
               
                // hangar
                if(isHangaringPlane)
                {
                    float targetDepth = hangarDepth * hangaringDirection;
                    float progress =  1 - (targetDepth - carrierHangarNode.Position.y)/targetDepth;
                    float step = 3.0f * timeSinceLastFrameUpdate * hangaringDirection; //* Math.Cos(Math.HALF_PI * progress);
                    
                    progress = Math.Abs(progress);
                    step *= 1 / (1 +  (float)System.Math.Pow((-1.5f + 3.0f * progress), 2)) * 0.75f; // p³ynne przyspieszenie i hamowanie wg. pochodniej arctan ktora wynosi 1 / (1 + x^2)

                    carrierHangarNode.Position = carrierHangarNode.Position + new Vector3(0, step, 0);
                    hangarPlane.Plane.Bounds.Move(0, step);

                    if (hangaringDirection == -1)
                    {
                        // opuszczanie
                        if (carrierHangarNode.Position.y < targetDepth)
                        {
                            hangarPlane.Plane.Bounds.Move(0, carrierHangarNode.Position.y - targetDepth);
                            carrierHangarNode.SetPosition(0, targetDepth, 0);
                            isHangaringPlane = false;
                        }
                    }
                    else
                    {
                        // podnoszenie 
                        if (carrierHangarNode.Position.y >= 0)
                        {
                            carrierHangarNode.SetPosition(0,0,0);
                            hangarPlane.Plane.Bounds.Move(0, -carrierHangarNode.Position.y);
                            isHangaringFinished = true;
                            isHangaringPlane = false;
                        }

                    }
                   
                }



                if (isCatchingPlane)
                {
                    activateClosestArrestingWires(planeBeingCaught); // zaczepiaj samolot o kolejne liny
                    for (int i = 0; i < activeArrestingWires.Count; i++)
                    {
                        arrestingWiresH[i] =
                            animateArrestingWire(activeArrestingWires[i],
                                                 planeBeingCaught.RearWheelInnerNode.WorldPosition.x);
                    }
                }
                if (isReleasingPlane)
                {
                    if (!shouldStopReleasingPlane())
                    {
                        for (int i = 0; i < activeArrestingWires.Count; i++)
                        {
                            animateArrestingWire(
                                activeArrestingWires[i],
                                activeArrestingWires[i].WorldPosition.x - arrestingWiresH[i]
                                );
                            arrestingWiresH[i] *= (1 - 10*timeSinceLastFrameUpdate);
                                // 0.99f; - wymuszamy zmniejszenie wysokosci trojkatow
                        }
                    }
                    else
                    {
                        // koniec
                        finishReleasingPlane();
                    }
                }
            }
        }
    }
}