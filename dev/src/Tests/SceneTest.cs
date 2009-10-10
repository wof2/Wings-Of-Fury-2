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
using System.Runtime.InteropServices;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.View.NodeAnimation;
using Math=Mogre.Math;

namespace Wof.Tests
{
    internal class SceneTestProgram
    {
        private static void Main(string[] args)
        {
            try
            {
                SceneTest app = new SceneTest();
                app.Go();
            }
            catch (SEHException)
            {
                // Check if it's an Ogre Exception 
                if (OgreException.IsThrown)
                    SceneTest.ShowOgreException();
                else
                    throw;
            }
        }
    }

    /// <summary>
    /// Klasa do testowania Modeli 3D i animacji
    /// <author>Adam Witczak</author>
    /// 
    /// </summary>
    public class SceneTest : FrameWork
    {
        protected float time = 0;

        protected SceneNode carrierNode,
                            japanFlagNode,
                            playerNode,
                            a6mNode,
                            p47OuterNode,
                            p47InnerNode,
                            bladeNode,
                            rearWheelNode,
                            lWheelNode,
                            rWheelNode,
                            islandNode,
                            island2Node,
                            island3Node,
                            island6Node,
                            flakBarrelNode,
                            gunEmplacementNode,
                            woodenBunkerNode,
                            tentNode,
                            carrierAerial1Node,
                            carrierAerial2Node,
                            lCarrierWaterTrailNode,
                            rCarrierWaterTrailNode,
                            lGunHitNode,
                            rGunHitNode;

        protected SceneNode cameraNode, enemyAirscrewNode;

        protected Entity flakBarrel, p47, a6m;

        protected AnimationState p47AnimationState, flakBarrelState, japanFlagState;
        protected AnimationState[] soldiersState;


        protected PlaneNodeAnimationManager p47Animation, p47InnerAnimation;

        /// <summary>
        /// Tworzy i umiejscawia kamerê na scenie
        /// </summary>
        public override void CreateCamera()
        {
            EngineConfig.ManualCamera = true;

            camera = sceneMgr.CreateCamera("MainCamera");
            camera.NearClipDistance = 1;
            camera.Position = new Vector3(0, 0, 0);
            camera.LookAt(Vector3.NEGATIVE_UNIT_Z);

            CreateMinimapCamera();
            CreateOverlayCamera();
        }

        

        public override void CreateFrameListener()
        {
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            SetCompositorEnabled(CompositorTypes.OLDMOVIE, false);
        }

        /// <summary>
        /// Buduje testow¹ scenê
        /// </summary>
        public override void CreateScene()
        {
            //float cameraDistance = 50.0f;
            // mgr.LoadWorldGeometry("Terrain.xml");
            //sceneMgr.AmbientLight = ColourValue.Black; ; //; new ColourValue(1.0f, 1.0f, 1.0f);


            // Set the material
            sceneMgr.SetSkyBox(true, "Skybox/Morning", 5000);
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
            // create a default point light


            Light light = sceneMgr.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.Position = new Vector3(0, 1000, 0);
            light.Direction = new Vector3(0, -5, 0);
            light.DiffuseColour = new ColourValue(1.0f, 1.0f, 1.0f);
            light.SpecularColour = new ColourValue(0.05f, 0.05f, 0.05f);

            // OCEAN          
            Plane plane = new Plane();
            plane.normal = Vector3.UNIT_Y;
            plane.d = 0;
            MeshManager.Singleton.CreatePlane("OceanPlane",
                                              ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                                              5000, 5000, 10, 10, true, 1, 10, 10, Vector3.UNIT_Z);

            Entity ocean = sceneMgr.CreateEntity("Ocean", "OceanPlane");
            ocean.SetMaterialName("Ocean2_HLSL");
            ocean.CastShadows = false;

            sceneMgr.RootSceneNode.AttachObject(ocean);
            // OCEAN

            ViewHelper.AttachAxes(sceneMgr, sceneMgr.RootSceneNode, 0.001f);


            // CARRIER
            Entity carrier2 = minimapMgr.CreateEntity("Carrier", "Carrier.mesh");


            Entity carrier = sceneMgr.CreateEntity("Carrier", "Carrier.mesh");
            Entity carrierAerial1 = sceneMgr.CreateEntity("CarrierAerial1", "Aerial1.mesh");
            Entity carrierAerial2 = sceneMgr.CreateEntity("CarrierAerial2", "Aerial2.mesh");

            Entity lWaterTrail = sceneMgr.CreateEntity("LWaterTrail", "TwoSidedPlane.mesh");
            lWaterTrail.CastShadows = false;
            lWaterTrail.SetMaterialName("Effects/WaterTrail");

            Entity rWaterTrail = sceneMgr.CreateEntity("RWaterTrail", "TwoSidedPlane.mesh");
            rWaterTrail.CastShadows = false;
            rWaterTrail.SetMaterialName("Effects/WaterTrail");

            carrierNode = sceneMgr.RootSceneNode.CreateChildSceneNode("Carrier");
            carrierNode.AttachObject(carrier);

            carrierAerial1Node = carrierNode.CreateChildSceneNode("carrierAerial1Node", new Vector3(-8.6f, 9.1f, 5.7f));
            carrierAerial1Node.AttachObject(carrierAerial1);

            carrierAerial2Node =
                carrierNode.CreateChildSceneNode("carrierAerial2Node", new Vector3(-10.7f, 4.0f, 12.6f));
            carrierAerial2Node.AttachObject(carrierAerial2);


            lCarrierWaterTrailNode =
                carrierNode.CreateChildSceneNode("lCarrierWaterTrailNode", new Vector3(-4.6f, -4.8f, -40.0f));
            lCarrierWaterTrailNode.AttachObject(lWaterTrail);
            lCarrierWaterTrailNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Math.HALF_PI);
            lCarrierWaterTrailNode.Scale(2.0f, 1f, 1.5f);

            rCarrierWaterTrailNode =
                carrierNode.CreateChildSceneNode("rCarrierWaterTrailNode", new Vector3(4.6f, -4.8f, -40.0f));
            rCarrierWaterTrailNode.AttachObject(rWaterTrail);
            rCarrierWaterTrailNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Math.HALF_PI);
            rCarrierWaterTrailNode.Scale(2.0f, 1f, 1.5f);


            carrierNode.Translate(new Vector3(100, 5, 0));
            carrierNode.SetDirection(Vector3.UNIT_X);

            // CARRIER


            // ISLAND1

            /*   Entity island = sceneMgr.CreateEntity("Island1", "Island3.mesh");

            islandNode = sceneMgr.RootSceneNode.CreateChildSceneNode("Island1");
            islandNode.AttachObject(island);
            islandNode.Translate(new Vector3(-100, 1.35f, 0));
            islandNode.SetDirection(Vector3.UNIT_X);*/

            // ISLAND2
            /*    Entity island2 = sceneMgr.CreateEntity("Island2", "Island2.mesh");

            island2Node = sceneMgr.RootSceneNode.CreateChildSceneNode("Island2");
            island2Node.AttachObject(island2);
            island2Node.Translate(new Vector3(-170, -0.35f, 0));
            island2Node.SetDirection(Vector3.UNIT_X);*/

            // ISLAND3
            Entity island3 = sceneMgr.CreateEntity("Island3", "Island5.mesh");

            island3Node = sceneMgr.RootSceneNode.CreateChildSceneNode("Island3");
            island3Node.AttachObject(island3);
            island3Node.Translate(new Vector3(-460, 1.35f, 0));
            island3Node.SetDirection(Vector3.UNIT_X);

            // ISLAND4
            Entity island6 = sceneMgr.CreateEntity("Island6", "Island6.mesh");

            islandNode = sceneMgr.RootSceneNode.CreateChildSceneNode("Island6");
            islandNode.AttachObject(island6);
            islandNode.Translate(new Vector3(-200, 5.35f, 0));
            islandNode.SetDirection(Vector3.UNIT_X);


            float rot, xpos, zpos, ypos;

            // WOODEN BUNKER INSTALLATION          
            gunEmplacementNode = islandNode.CreateChildSceneNode("GunEmplacement", new Vector3(0.0f, 1.1f, 5.0f));

            Entity sandbags = sceneMgr.CreateEntity("Sandbags", "Sandbags.mesh");
            gunEmplacementNode.AttachObject(sandbags);

            woodenBunkerNode = gunEmplacementNode.CreateChildSceneNode("WoodenBunkerNode", new Vector3(0, 0.0f, 4.0f));
            Entity woodenBunker = sceneMgr.CreateEntity("WoodenBunker", "Bunker.mesh");
            // woodenBunker.SetMaterialName("Concrete"); // aby by³ betonowy
            woodenBunkerNode.AttachObject(woodenBunker);

            Entity flakBase = sceneMgr.CreateEntity("FlakBase", "FlakBase.mesh");
            gunEmplacementNode.AttachObject(flakBase);

            flakBarrel = sceneMgr.CreateEntity("FlakBarrel", "FlakBarrel.mesh");
            flakBarrelNode = gunEmplacementNode.CreateChildSceneNode("FlakBarrelNode", new Vector3(0, 0.5f, 0));
            flakBarrelNode.AttachObject(flakBarrel);
            // WOODEN BUNKER INSTALLATION 


            // PALM TREES
            SceneNode palmNode;
            Entity palm;


            for (int i = 0; i < 150; i++)
            {
                palm = sceneMgr.CreateEntity("Palm" + i, "PalmTree.mesh");
                rot = Math.RangeRandom(-90.0f, 90.0f);
                zpos = Math.RangeRandom(-200.0f, 200.0f);
                xpos = Math.RangeRandom(-10.0f, 10.0f);
                ypos = Math.RangeRandom(0.1f, 0.3f);

                palmNode = islandNode.CreateChildSceneNode("PalmNode" + i, new Vector3(xpos, ypos, zpos));
                palmNode.Rotate(Vector3.UNIT_Y, rot);
                palmNode.Scale(1, Math.RangeRandom(0.9f, 1.1f), 1);
                palmNode.AttachObject(palm);
            }
            //islandNode.Scale(2.0f, 2.0f, 2.0f);
            // PALM TREES


            // JAPAN FLAG
            Entity japanFlag = sceneMgr.CreateEntity("JapanFlag", "JapanFlag.mesh");
            japanFlagNode = islandNode.CreateChildSceneNode("JapanFlagNode", new Vector3(0, 1, -5));
            japanFlagNode.Rotate(Vector3.UNIT_Y, Math.PI);
            japanFlagNode.AttachObject(japanFlag);

            japanFlagState = japanFlag.GetAnimationState("idle");
            japanFlagState.Enabled = true;
            japanFlagState.Loop = true;
            // JAPAN FLAG


            // TENT - just for fun
            // ???

            Entity tent = sceneMgr.CreateEntity("Tent", "Barracks.mesh");
            tentNode = islandNode.CreateChildSceneNode("TentNode", new Vector3(0, 1, 17));
            tentNode.AttachObject(tent);
            // TENT


            // SOLDIERS
            SceneNode soldierNode;
            Entity soldier;
            soldiersState = new AnimationState[15];
            for (int i = 0; i < soldiersState.Length; i++)
            {
                soldier = sceneMgr.CreateEntity("Soldier" + i, "Soldier.mesh");
                if (i%3 == 0)
                {
                    soldier.SetMaterialName("General");
                }

                rot = Math.RangeRandom(-30.0f, 30.0f);
                zpos = Math.RangeRandom(-10.0f, 20.0f);
                xpos = Math.RangeRandom(-0.0f, 1.0f);
                ypos = 0.8f;

                soldierNode =
                    SceneMgr.RootSceneNode.CreateChildSceneNode("SoldierNode" + i, new Vector3(xpos, ypos, zpos));
                soldierNode.Rotate(Vector3.UNIT_Y, rot);
                soldierNode.AttachObject(soldier);


                switch (i%3)
                {
                    case 0:
                        soldiersState[i] = soldier.GetAnimationState("run");
                        break;

                    case 1:
                        soldiersState[i] = soldier.GetAnimationState("die1");
                        break;

                    case 2:
                        soldiersState[i] = soldier.GetAnimationState("die2");
                        break;
                }
                soldiersState[i].Loop = true;
                soldiersState[i].Enabled = true;
            }
            //islandNode.Scale(0.7f, 0.7f, 0.7f);

            // SOLDIERS


            /*d
              BillboardSet skies =  SceneManager.CreateBillboardSet("Sky",1);
              skies.MaterialName = "Skyplane/Morning";
              Billboard sky = skies.CreateBillboard(new Vector3(0.0f, 0.0f, -100.0f));
              SceneManager.RootSceneNode.AttachObject(skies);
            */


//////////////////////////////////////////////////////  
// A6M
            /*
            a6m = sceneMgr.CreateEntity("A6M", "A6M.mesh");
            a6mNode = sceneMgr.RootSceneNode.CreateChildSceneNode("EnemyNode");
            a6mNode.AttachObject(a6m);


            // AIRSCREW
            enemyAirscrewNode = a6mNode.CreateChildSceneNode("EnemyAirscrew", new Vector3(0.0f, 0.0f, -5.70f));
          
            Entity enemyAirscrew = sceneMgr.CreateEntity("EnemyAirscrew", "Airscrew.mesh");
            enemyAirscrew.SetMaterialName("A6M/Airscrew");
            enemyAirscrew.CastShadows = false;
            enemyAirscrewNode.AttachObject(enemyAirscrew);
            // AIRSCREW


            a6mNode.LookAt(Vector3.NEGATIVE_UNIT_X, Node.TransformSpace.TS_WORLD);

           
            a6mNode.Scale(new Vector3(0.5f, 0.5f, 0.5f));
            a6mNode.Position = new Vector3(0, 13.0f, 0);
            ViewHelper.AttachAxes(sceneMgr, a6mNode, 1.5f);
*/
// A6M
//////////////////////////////////////////////////////         


//////////////////////////////////////////////////////
// P47
            p47 = sceneMgr.CreateEntity("P47Body", "P47Body.mesh");
            playerNode = sceneMgr.RootSceneNode.CreateChildSceneNode("PlayerNode", new Vector3(350, 0, 0));


            p47OuterNode = playerNode.CreateChildSceneNode("OuterNode");
            p47InnerNode = p47OuterNode.CreateChildSceneNode("InnerNode");
            p47InnerNode.AttachObject(p47);

            p47OuterNode.Scale(new Vector3(0.5f, 0.5f, 0.5f));
            p47OuterNode.LookAt(Vector3.NEGATIVE_UNIT_X, Node.TransformSpace.TS_WORLD);


            // SMOKE

            ParticleSystem smokeSystem = sceneMgr.CreateParticleSystem("SmokeSystem", "Smokes/Smoke");

            SceneNode smokeNode = playerNode.CreateChildSceneNode("Smoke", new Vector3(0.0f, 0.0f, 3.0f));
            smokeSystem.GetEmitter(0).Direction = new Vector3(0.0f, 0.0f, 1.0f);

            smokeNode.AttachObject(smokeSystem);

            // SMOKE


            // ANIMATION   
            // p47AnimationState = p47.GetAnimationState("manual");
            // p47AnimationState.Loop = true;
            // p47AnimationState.Enabled = true;
            // ANIMATION


            // BLADE
            bladeNode = p47InnerNode.CreateChildSceneNode("Blade", new Vector3(0.0f, 0.0f, -7.0f));
            Entity p47Blade = sceneMgr.CreateEntity("Blade", "P47Blade.mesh");
            bladeNode.AttachObject(p47Blade);
            p47Blade.Visible = false; // tylko kiedy niskie obroty
            // BLADE


            // AIRSCREW
            Entity airscrew = sceneMgr.CreateEntity("Airscrew", "Airscrew.mesh");
            airscrew.CastShadows = false;
            bladeNode.AttachObject(airscrew);
            // AIRSCREW


            // GUNHIT

            Entity lGunHit = sceneMgr.CreateEntity("LGunHit", "TwoSidedPlane.mesh");
            lGunHit.CastShadows = false;
            lGunHit.SetMaterialName("Effects/GunHit");

            Entity rGunHit = sceneMgr.CreateEntity("RGunHit", "TwoSidedPlane.mesh");
            rGunHit.CastShadows = false;
            rGunHit.SetMaterialName("Effects/GunHit");


            lGunHitNode = p47InnerNode.CreateChildSceneNode("lGunHitNode", new Vector3(-4.0f, -0.5f, -4.2f));
            lGunHitNode.AttachObject(lGunHit);
            lGunHitNode.Rotate(Vector3.NEGATIVE_UNIT_Z, Math.HALF_PI);
            lGunHitNode.Scale(0.5f, 0.5f, 0.7f);

            rGunHitNode = p47InnerNode.CreateChildSceneNode("rGunHitNode", new Vector3(4.0f, -0.5f, -4.2f));
            rGunHitNode.AttachObject(rGunHit);
            rGunHitNode.Rotate(Vector3.NEGATIVE_UNIT_Z, Math.HALF_PI);
            rGunHitNode.Scale(0.5f, 0.5f, 0.7f);
            // GUNHIT


            // WHEELS

            lWheelNode =
                p47InnerNode.CreateChildSceneNode("LeftWheel", new Vector3(-3.0f, -1.6f, -2.3f),
                                                  new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            Entity lWheel = sceneMgr.CreateEntity("lWheel", "Wheel.mesh");
            lWheelNode.AttachObject(lWheel);


            rWheelNode =
                p47InnerNode.CreateChildSceneNode("RightWheel", new Vector3(3.0f, -1.6f, -2.3f),
                                                  new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            rWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Math.DegreesToRadians(180));
            Entity rWheel = sceneMgr.CreateEntity("rWheel", "Wheel.mesh");
            rWheelNode.AttachObject(rWheel);


            rearWheelNode =
                p47OuterNode.CreateChildSceneNode("RearWheel", new Vector3(0.0f, -0.6f, 5.3f),
                                                  new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            // rearWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Mogre.Math.DegreesToRadians(180));

            Entity rearWheel = sceneMgr.CreateEntity("rearWheele", "Wheel.mesh");
            rearWheelNode.AttachObject(rearWheel);
            rearWheelNode.Scale(0.7f, 0.7f, 0.7f);

            // WHEELS 


            // ViewHelper.AttachAxes(sceneMgr, p47InnerNode, 1.5f);
            playerNode.Position = new Vector3(0, 10.0f, 0);


            // p47Animation = new PlaneAnimationManager(0, new PlayerPlaneViewnew Wof.Model.Level.Planes.Plane(),this,playerNode,);          
            // p47Animation.enableAll();


            // p47Animation.switchTo(PlaneAnimationManager.AnimationType.IDLE);
            // p47Animation.Enabled = false;


            // CAMERA


            cameraNode = playerNode.CreateChildSceneNode("Camera");
            cameraNode.AttachObject(camera);

            cameraNode.Translate(new Vector3(0, 0, 100));

            //   sceneMgr.RootSceneNode.AttachObject(camera);


// P47
//////////////////


            // CHMURY
            BillboardSet clouds1 = sceneMgr.CreateBillboardSet("Clouds1");
            clouds1.MaterialName = "Effects/Cloud1";

            for (int i = -11; i < 11; i += 2)
            {
                Billboard cloud1 = (Billboard) clouds1.CreateBillboard(i*10, 100 + Math.RangeRandom(-50, 50), -500);
                cloud1.SetDimensions(200 + Math.RangeRandom(-i, i), 100 + Math.RangeRandom(-i, 0));
                cloud1.Rotation = Math.DegreesToRadians(Math.RangeRandom(5, 5));
            }
            sceneMgr.RootSceneNode.AttachObject(clouds1);


            BillboardSet clouds2 = sceneMgr.CreateBillboardSet("Clouds2");
            clouds2.MaterialName = "Effects/Cloud2";

            for (int i = -10; i < 10; i += 2)
            {
                Billboard cloud2 = clouds2.CreateBillboard(i*100, 100 + Math.RangeRandom(-50, 50), -500);
                cloud2.SetDimensions(200 + Math.RangeRandom(-i, i), 100 + Math.RangeRandom(-i, 0));
                cloud2.Rotation = Math.DegreesToRadians(Math.RangeRandom(5, 5));
            }
            sceneMgr.RootSceneNode.AttachObject(clouds2);
        }


        public override void ModelFrameStarted(FrameEvent evt)
        {
            
        }

        /// <summary>
        /// Handler zdarzenia FrameStarted: animacja
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public override bool FrameStarted(FrameEvent evt)
        {
        	
            time += evt.timeSinceLastFrame;

            //  japanFlagState.AddTime(evt.timeSinceLastFrame);
/*
            p47Animation.updateTimeAll(evt.timeSinceLastFrame);
            p47Animation.animateAll();
          
            

            p47Animation.switchTo(PlaneAnimationManager.AnimationType.INNERTURN);    
            if (p47Animation.CurrentAnimation.Ended)
            {
                p47Animation.rewindAll(true);
                p47Animation[PlaneAnimationManager.AnimationType.IDLE].Enabled = false;
            }
*/
            //////////////////////////////////////////////////////////

            //   enemyAirscrewNode.Rotate(Vector3.NEGATIVE_UNIT_Z, evt.timeSinceLastFrame * 50.0f, Node.TransformSpace.TS_LOCAL);


            float ySpeed, xSpeed;
            /*  if (time > 10)
              {

                  if (p47AnimationState.AnimationName == "manual")
                  {
                      p47AnimationState = p47.GetAnimationState("die");
                      p47AnimationState.Loop = false;
                      p47AnimationState.Enabled = true;

                  }
                  p47AnimationState.AddTime(evt.timeSinceLastFrame); // animacja stanowa
                  ySpeed = -2.0f; xSpeed = -2.0f;
                  bladeNode.Rotate(Vector3.NEGATIVE_UNIT_Z, evt.timeSinceLastFrame * 1.0f, Node.TransformSpace.TS_LOCAL);
        
              }

              else*/
            {
                ySpeed = 0.0f;
                xSpeed = -40;
                bladeNode.Rotate(Vector3.NEGATIVE_UNIT_Z, evt.timeSinceLastFrame*50.0f, Node.TransformSpace.TS_LOCAL);
            }


            playerNode.Translate(xSpeed*evt.timeSinceLastFrame, ySpeed*evt.timeSinceLastFrame, 0,
                                 Node.TransformSpace.TS_LOCAL);
            // a6mNode.Translate(0, ySpeed * evt.timeSinceLastFrame, xSpeed * evt.timeSinceLastFrame, Node.TransformSpace.TS_LOCAL);

            carrierAerial1Node.Rotate(Vector3.NEGATIVE_UNIT_Y, evt.timeSinceLastFrame*1.0f, Node.TransformSpace.TS_LOCAL);

            /*
            Vector3 distance = gunEmplacementNode.WorldPosition - playerNode.WorldPosition;
         
            //&& flakBarrelNode.Orientation.
            if (Mogre.Math.Abs(distance.Length) < 50 )
            {
                if (flakBarrelState == null || flakBarrelState.Enabled == false )
                {
                    flakBarrelState = flakBarrel.GetAnimationState("fire");
                    flakBarrelState.Loop = true;
                    flakBarrelState.Enabled = true;
                }

                flakBarrelState.AddTime(evt.timeSinceLastFrame);  
                flakBarrelNode.LookAt(playerNode.Position, Node.TransformSpace.TS_WORLD);
                
            }
            else
            {
               // if(flakBarrelState != null) flakBarrelState.Enabled = false;
            }
            */

            for (int i = 0; i < soldiersState.Length; i++)
            {
                soldiersState[i].AddTime(evt.timeSinceLastFrame);
            }


            if (window.IsClosed)
                return false;

            OnUpdateModel(evt);

            return !shutDown;
        }
    }
}