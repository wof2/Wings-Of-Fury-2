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
using Wof.Model.Configuration;
using Wof.Model.Level;
using Wof.Model.Level.Planes;
using Wof.View.Effects;
using Math=Mogre.Math;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    /// <summary>
    /// Samolot P47 w widoku
    /// <author>Adam Witczak, Kamil S³awiñski</author>
    /// </summary> 
    public class P47PlaneView : PlaneView
    {
        protected static int planeCounter = 1;
        protected Entity blade;
        protected Entity airscrew;

        protected bool hasSmokeTrail = false;
        protected string bodyMaterialName = "P47/Body";
        protected string destroyedBodyMaterialName = "P47/DestroyedBody";
       

        public bool HasSmokeTrail
        {
            get { return hasSmokeTrail; }
        }

        public bool ShouldHaveSmokeTrail
        {
            get { return (Math.Abs(Plane.RotateValue) >= 2.0); }
        }

        public P47PlaneView(Plane plane, IFrameWork frameWork, SceneNode parentNode, String baseName)
            : base(plane, frameWork, parentNode, baseName + "_" + planeCounter.ToString())
        {
            planeCounter++;
            lWingNode = innerNode.CreateChildSceneNode(name + "LWingNode", new Vector3(-8.8f, 0.0f, -1.5f));
            rWingNode = innerNode.CreateChildSceneNode(name + "RWingNode", new Vector3(8.8f, 0.0f, -1.5f));
            StartSmokeTrails();
            StopSmokeTrails();
        }

        public void StopSmokeTrails()
        {
            hasSmokeTrail = false;
            EffectsManager.Singleton.NoSmoke(sceneMgr, lWingNode, EffectsManager.SmokeType.SMOKETRAIL);
            EffectsManager.Singleton.NoSmoke(sceneMgr, rWingNode, EffectsManager.SmokeType.SMOKETRAIL);
        }

        public void StartSmokeTrails()
        {
            hasSmokeTrail = true;
            EffectsManager.Singleton.Smoke(sceneMgr,
                                           lWingNode,
                                           EffectsManager.SmokeType.SMOKETRAIL,
                                           new Vector3(0, 0, 0),
                                           new Vector3(0, 0, 1),
                                           new Vector2(0.4f, 0.3f)
                );
            EffectsManager.Singleton.Smoke(sceneMgr,
                                           rWingNode,
                                           EffectsManager.SmokeType.SMOKETRAIL,
                                           new Vector3(0, 0, 0),
                                           new Vector3(0, 0, 1),
                                           new Vector2(0.4f, 0.3f)
                );
        }


        protected override void initBlade()
        {
            // BLADE
            bladeNode = innerNode.CreateChildSceneNode(name + "_Blade", new Vector3(0.0f, 0.1f, -7.0f));
            blade = sceneMgr.CreateEntity(name + "_Blade", "P47Blade.mesh");
            bladeNode.AttachObject(blade);
            blade.Visible = true; // tylko kiedy niskie obroty
            // BLADE

            // AIRSCREW
            airscrew = sceneMgr.CreateEntity(name + "_Airscrew", "Airscrew.mesh");
            airscrew.CastShadows = false;
            bladeNode.AttachObject(airscrew);
            airscrew.Visible = false;
            // AIRSCREW
        }

        public void SwitchToFastEngine()
        {
            blade.Visible = false;
            airscrew.Visible = true;

       
        }

        public void SwitchToSlowEngine()
        {
            if (!blade.Visible && !airscrew.Visible) return; // jesli calosc wczesniej nie byla widoczna
            blade.Visible = true;
            airscrew.Visible = false;

          
        }

        public override void SetBladeVisibility(bool visible)
        {
            blade.Visible = visible;
            airscrew.Visible = visible;
        }

        public override void ShowTorpedo()
        {
            torpedoHolder.SetVisible(true);
        }

        public override void HideTorpedo()
        {
            torpedoHolder.SetVisible(false);
        }

        protected override void initWheels()
        {
            lWheelNode =
                innerNode.CreateChildSceneNode(name + "_LeftWheel", new Vector3(-3.0f, -1.0f, -2.3f),
                                               new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            lWheelInnerNode = lWheelNode.CreateChildSceneNode(name + "_LeftWheelInner");

            Entity lWheel = sceneMgr.CreateEntity(name + "_lWheel", "Wheel.mesh");
            lWheelInnerNode.AttachObject(lWheel);

            rWheelNode =
                innerNode.CreateChildSceneNode(name + "_RightWheel", new Vector3(3.0f, -1.0f, -2.3f),
                                               new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            rWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Math.DegreesToRadians(180));
            rWheelInnerNode = rWheelNode.CreateChildSceneNode(name + "_RightWheelInner");

            Entity rWheel = sceneMgr.CreateEntity(name + "_rWheel", "Wheel.mesh");
            rWheelInnerNode.AttachObject(rWheel);

            rearWheelNode =
                innerNode.CreateChildSceneNode(name + "_RearWheel", new Vector3(0.0f, -0.2f, 5.3f),
                                               new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            // rearWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Mogre.Math.DegreesToRadians(180));
            rearWheelNode.Scale(0.7f, 0.7f, 0.7f);
            rearWheelInnerNode = rearWheelNode.CreateChildSceneNode(name + "_RearWheelInner");

            Entity rearWheel = sceneMgr.CreateEntity(name + "_rearWheele", "Wheel.mesh");
            rearWheelInnerNode.AttachObject(rearWheel);
            
            // retract landing gear
            if(this.plane != null && this.plane.WheelsState == WheelsState.In) 
            {
            	LWheelInnerNode.Roll(new Radian(new Degree(90)));
            	RWheelInnerNode.Roll(new Radian(new Degree(90)));
            	RearWheelInnerNode.Pitch(new Radian(new Degree(45)));
            	// IMPORTANT: this should be also set in animation manager later on (MaxAngle *= -1 to change direction of wheel movement)
            }
                               
        }

        public override void ResetCameraHolders()
        {

            hangaringCameraHolder = planeNode.CreateChildSceneNode(name + "HangaringCameraHolder");
        	cameraHolders.Add(planeNode.CreateChildSceneNode(name + "MainCameraHolder"));
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "BirdCameraHolder"));
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "AboveLeftCameraHolder"));
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "AboveRightCameraHolder"));
            cameraHolders.Add(innerNode.CreateChildSceneNode(name + "BehindCameraHolder"));
            cameraHolders.Add(innerNode.CreateChildSceneNode(name + "NoseCameraHolder"));
            
            // MAIN CAMERA HOLDER
            cameraHolders[0].ResetOrientation();
            cameraHolders[0].Position = new Vector3(0, 2.5f, 8);
            cameraHolders[0].LookAt(new Vector3(0, 0, -1), Node.TransformSpace.TS_LOCAL);

            // BIRD CAMERA HOLDER
            cameraHolders[1].ResetOrientation();
            cameraHolders[1].Position = new Vector3(0, 30, 0);
            cameraHolders[1].Pitch(new Radian(-Math.HALF_PI));
            //cameraHolders[1].Roll(new Radian(Mogre.Math.HALF_PI));

            // ABOVE CAMERA HOLDERS
            
            cameraHolders[2].ResetOrientation();
            cameraHolders[2].Position = new Vector3(-19, 2.0f, 0);
            cameraHolders[2].Yaw(new Radian(-Math.HALF_PI));
           // cameraHolders[2].Pitch(new Radian(-Math.HALF_PI * 0.28f));
            cameraHolders[2].Pitch(new Radian(-Math.HALF_PI * 0.01f));
            
            cameraHolders[3].ResetOrientation();
            cameraHolders[3].Position = new Vector3(19, 2.0f, 0);
            cameraHolders[3].Yaw(new Radian(Math.HALF_PI));
           // cameraHolders[3].Pitch(new Radian(-Math.HALF_PI * 0.28f));
            cameraHolders[3].Pitch(new Radian(-Math.HALF_PI * 0.01f));


            cameraHolders[4].ResetOrientation();
            cameraHolders[4].Position = new Vector3(0, 4.0f, 30);        
            cameraHolders[4].Pitch(new Radian(-Math.HALF_PI * 0.01f));
            
            cameraHolders[5].ResetOrientation();
            cameraHolders[5].Position = new Vector3(0, 0.0f, -6);        
            //cameraHolders[5].Pitch(new Radian(-Math.HALF_PI * 0.01f));

            HangaringCameraHolder.ResetOrientation();
            HangaringCameraHolder.Position = new Vector3(19, 7.0f, 0);
            HangaringCameraHolder.Yaw(new Radian(Math.HALF_PI));
            HangaringCameraHolder.Pitch(new Radian(-Math.HALF_PI * 0.28f));
            

            
            
            
         
        }

        public override void SmashPaint()
        {
            if (!EngineConfig.LowDetails && !GameConsts.UserPlane.PlaneCheat)
            {
                ViewHelper.ReplaceMaterial(planeEntity, bodyMaterialName, destroyedBodyMaterialName);
            }
        }


        public override void RestorePaint()
        {
            // polski samolot ma niezniszalny lakier;)
            if (!EngineConfig.LowDetails && !GameConsts.UserPlane.PlaneCheat)
            {
                ViewHelper.ReplaceMaterial(planeEntity, destroyedBodyMaterialName, bodyMaterialName);
            }
        }

    
        protected override void initOnScene()
        {
            // main nodes init
            planeEntity = sceneMgr.CreateEntity(name + "_Body", "P47Body.mesh");
          
            if(GameConsts.UserPlane.PlaneCheat)
            {
                ViewHelper.ReplaceMaterial(planeEntity, bodyMaterialName, "P47/BodyPL");
                bodyMaterialName = "P47/BodyPL";
            }
             
            

            planeEntity.CastShadows = EngineConfig.ShadowsQuality > 0; 
            innerNode.AttachObject(planeEntity);
            outerNode.Scale(new Vector3(0.4f, 0.4f, 0.4f));

            initBlade();
            initWheels();


            torpedoHolder = innerNode.CreateChildSceneNode(planeNode.Name + "TorpedoHolder", new Vector3(0,-1.6f,1.6f));
            if(plane != null)
            {
              /*  if (this.plane.Direction == Direction.Right)
                {
                    torpedoHolder.Orientation = new Quaternion(Math.HALF_PI, Vector3.NEGATIVE_UNIT_Y);
                }
                else
                {
                    torpedoHolder.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_Y);

                } */
                torpedoHolder.Scale(2.5f, 2.5f, 2.5f); // caly node jest skalowany x 0.4
                Entity torpedo = sceneMgr.CreateEntity(name + "_Torpedo", "Torpedo.mesh");
                torpedoHolder.AttachObject(torpedo);
            }
           


          
           // torpedoHolder.SetVisible(false);


            ViewHelper.AttachAxes(sceneMgr, innerNode, 1.5f);

            refreshPosition();
            initAnimationManager();
            if(this.plane != null && this.plane.WheelsState == WheelsState.In) 
            {
            	this.animationMgr.switchToGearUpDown(false);
	            this.animationMgr.CurrentAnimation.Enabled = false;
	            animationMgr.disableAll();
            }
           
            
            
            if (plane != null && plane.LocationState == LocationState.Air)
            {
                animationMgr.switchToIdle();
            }
            animationMgr.enableBlade();

            // vertex animation
            animationState = PlaneEntity.GetAnimationState("manual");

            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem =
                    new MinimapItem(outerNode, this.frameWork.MinimapMgr, "Cube.mesh", new ColourValue(0, 0.9f, 0),
                                    planeEntity);
                minimapItem.ScaleOverride = new Vector2(0, 5);
                minimapItem.MinimapNode.Translate(0, 0, 10.0f);
                minimapItem.Refresh();
            }
            // kamery
          

            ResetCameraHolders();
        }
    }
}