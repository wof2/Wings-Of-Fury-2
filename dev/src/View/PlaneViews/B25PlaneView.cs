/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
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
using Wof.Model.Configuration;
using Wof.Model.Level;
using Wof.Model.Level.Planes;
using Wof.View.Effects;
using Math = Mogre.Math;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    /// <summary>
    /// Samolot F4U w widoku
    /// <author>Adam Witczak</author>
    /// </summary> 
    public class B25PlaneView : P47PlaneView
    {

      
        protected Entity bladeL, bladeR;

        public override List<int> GetCrossHairCameraIndexes()
        {
            return new List<int>() { 2, 3, 4 }; ;
        }

        public B25PlaneView(Plane plane, IFrameWork frameWork, SceneNode parentNode)
            : base(plane, frameWork, parentNode)
        {
        	bodyMaterialName = "B25/Body";
            destroyedBodyMaterialName = "B25/BodyDestroyed";
        }

     
        protected override void initWheels()
        {
            lWheelNode =
                innerNode.CreateChildSceneNode(name + "_LeftWheel", new Vector3(-3.5f, -1.0f, -2.0f),
                                               new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            lWheelNode.Scale(1.0f, 1.1f, 1.0f);

            lWheelInnerNode = lWheelNode.CreateChildSceneNode(name + "_LeftWheelInner");

            Entity lWheel = sceneMgr.CreateEntity(name + "_lWheel", "Wheel.mesh");
            lWheelInnerNode.AttachObject(lWheel);

            rWheelNode =
                innerNode.CreateChildSceneNode(name + "_RightWheel", new Vector3(3.5f, -1.0f, -2.0f),
                                               new Quaternion(Math.DegreesToRadians(20), Vector3.UNIT_X));
            rWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Math.DegreesToRadians(180));
            rWheelNode.Scale(1.0f, 1.1f, 1.0f);
            rWheelInnerNode = rWheelNode.CreateChildSceneNode(name + "_RightWheelInner");
           

            Entity rWheel = sceneMgr.CreateEntity(name + "_rWheel", "Wheel.mesh");
            rWheelInnerNode.AttachObject(rWheel);

            rearWheelNode =
                innerNode.CreateChildSceneNode(name + "_RearWheel", new Vector3(0.0f, -1.15f, -7.3f),
                                               new Quaternion(Math.DegreesToRadians(10), Vector3.UNIT_X));
            // rearWheelNode.Rotate(Vector3.NEGATIVE_UNIT_Y, Mogre.Math.DegreesToRadians(180));
            rearWheelNode.Scale(0.8f, 0.93f, 0.8f);
            rearWheelInnerNode = rearWheelNode.CreateChildSceneNode(name + "_RearWheelInner");

            Entity rearWheel = sceneMgr.CreateEntity(name + "_rearWheele", "Wheel.mesh");
            rearWheelInnerNode.AttachObject(rearWheel);

            // retract landing gear
            if (this.plane != null && this.plane.WheelsState == WheelsState.In)
            {
                LWheelInnerNode.Roll(new Radian(new Degree(120)));
                RWheelInnerNode.Roll(new Radian(new Degree(120)));
                RearWheelInnerNode.Pitch(new Radian(new Degree(120)));
                // IMPORTANT: this should be also set in animation manager later on (MaxAngle *= -1 to change direction of wheel movement)
            }

        }

        public override void ResetCameraHolders()
        {

            hangaringCameraHolder = planeNode.CreateChildSceneNode(name + "HangaringCameraHolder");
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "MainCameraHolder"));
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "BirdCameraHolder"));
            // cameraHolders.Add(planeNode.CreateChildSceneNode(name + "AboveLeftCameraHolder"));
            //  cameraHolders.Add(planeNode.CreateChildSceneNode(name + "AboveRightCameraHolder"));
            cameraHolders.Add(innerNode.CreateChildSceneNode(name + "BehindCameraHolder"));
            cameraHolders.Add(innerNode.CreateChildSceneNode(name + "NoseCameraHolder"));
            cameraHolders.Add(innerNode.CreateChildSceneNode(name + "RearCameraHolder"));

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

            /*
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
            */


            cameraHolders[2].ResetOrientation();
            cameraHolders[2].Position = new Vector3(0, 6.0f, 30);
            cameraHolders[2].Pitch(new Radian(-Math.HALF_PI * 0.01f));

            cameraHolders[3].ResetOrientation();
            cameraHolders[3].Position = new Vector3(0, 0.0f, -6);
            //cameraHolders[5].Pitch(new Radian(-Math.HALF_PI * 0.01f));

            cameraHolders[4].ResetOrientation();
            cameraHolders[4].Position = new Vector3(0, 0.0f, 15);
         //   cameraHolders[4].Pitch(new Radian(-Math.HALF_PI * 0.01f));
            cameraHolders[4].Yaw(new Radian(Math.PI));


            HangaringCameraHolder.ResetOrientation();
            HangaringCameraHolder.Position = new Vector3(19, 7.0f, 0);
            HangaringCameraHolder.Yaw(new Radian(Math.HALF_PI));
            HangaringCameraHolder.Pitch(new Radian(-Math.HALF_PI * 0.28f));






        }


        public override void SetBladeVisibility(bool visible)
        {
            bladeL.Visible = visible;
            bladeR.Visible = visible;
            airscrewL.Visible = visible;
            airscrewR.Visible = visible;
        }

       
        public override void SwitchToSlowEngine()
        {
            if (!bladeL.Visible && !airscrewL.Visible) return; // jesli calosc wczesniej nie byla widoczna
            bladeL.Visible = true;
            bladeR.Visible = true;
           
            airscrewL.Visible = false;
            airscrewR.Visible = false;

        }

        public override void SwitchToFastEngine()
        {
            bladeL.Visible = false; bladeR.Visible = false;
          
            airscrewL.Visible = true;
            airscrewR.Visible = true;


        }

        protected override void initBlade()
        { // BLADE
            bladeNodeL = innerNode.CreateChildSceneNode(name + "_BladeL", new Vector3(3.66f, 0.25f, -5.3f));
            bladeL = sceneMgr.CreateEntity(name + "_BladeL", "P47Blade.mesh");
            bladeNodeL.AttachObject(bladeL);
            bladeL.Visible = true; // tylko kiedy niskie obroty


            bladeNodeR = innerNode.CreateChildSceneNode(name + "_BladeR", new Vector3(-3.66f, 0.25f, -5.3f));
            bladeR = sceneMgr.CreateEntity(name + "_BladeR", "P47Blade.mesh");
            bladeNodeR.AttachObject(bladeR);
            bladeR.Visible = true; // tylko kiedy niskie obroty
            // BLADE

            // AIRSCREW
            airscrewL = sceneMgr.CreateEntity(name + "_AirscrewL", "Airscrew.mesh");
            airscrewL.CastShadows = false;
            bladeNodeL.AttachObject(airscrewL);
            airscrewL.Visible = false;

            airscrewR = sceneMgr.CreateEntity(name + "_AirscrewR", "Airscrew.mesh");
            airscrewR.CastShadows = false;
            bladeNodeR.AttachObject(airscrewR);
            airscrewR.Visible = false;
            // AIRSCREW
          
        }
        
		public override string GetMainMeshName()
        {
            return "B25.mesh";
        }
       
        protected override void initOnScene()
        {
           
           
            lWingNode = innerNode.CreateChildSceneNode(name + "LWingNode", new Vector3(-8.8f, -0.2f, -1.5f));
            rWingNode = innerNode.CreateChildSceneNode(name + "RWingNode", new Vector3(8.8f, -0.2f, -1.5f));

            if (plane != null && !this.plane.IsEnemy)
            {
            	    EnableNightLights();
            }

            // main nodes init
            planeEntity = sceneMgr.CreateEntity(name + "_Body", GetMainMeshName());
            planeEntity.CastShadows = EngineConfig.ShadowsQuality > 0;
            innerNode.AttachObject(planeEntity);
            outerNode.Scale(new Vector3(0.40f, 0.40f, 0.40f));

            initBlade();
            initWheels();

            SceneNode pilotNode = innerNode.CreateChildSceneNode(planeNode.Name + "Pilot", new Vector3(0, 1.0f, -0.4f));
            Entity pilotEntity = sceneMgr.CreateEntity(name + "_Pilot", "Pilot.mesh");
            pilotNode.AttachObject(pilotEntity);

            torpedoHolder = innerNode.CreateChildSceneNode(planeNode.Name + "TorpedoHolder", new Vector3(0, -1.6f, 1.6f));
            if (plane != null)
            {
                torpedoHolder.Scale(2.5f, 2.5f, 2.5f); // caly node jest skalowany x 0.4
                Entity torpedo = sceneMgr.CreateEntity(name + "_Torpedo", "Torpedo.mesh");
                torpedoHolder.AttachObject(torpedo);
            }


            ViewHelper.AttachAxes(sceneMgr, innerNode, 1.5f);

            refreshPosition();
            initAnimationManager();
            if (EngineConfig.SoundEnabled)
            {
                planePassSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_PLANE_PASS, this.planeNode, false, false);
            }

            if (this.plane != null && this.plane.WheelsState == WheelsState.In)
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

        public override void OnFireGun()
        {
            OnFireGunDo(new Vector3(3.6f, 1.0f, -4.3f), new Vector3(-3.6f, 1.0f, -4.3f), new Vector2(4.6f, 3.6f), false, 64);
            OnFireGunDo(new Vector3(1.8f, 0.8f, -3.3f), new Vector3(-1.8f, 0.8f, -3.3f), new Vector2(3.5f, 2.5f), false, 64);
            OnFireGunDo(new Vector3(9.0f, 0.55f, 13.5f), new Vector3(-9.0f, 0.5f, 13.5f), new Vector2(1.5f, 1.5f), true, 35);
           
        }
    }
}