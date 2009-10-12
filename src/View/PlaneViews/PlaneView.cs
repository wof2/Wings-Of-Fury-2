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
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using Wof.View.VertexAnimation;
using Math=Mogre.Math;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.View
{

   

    /// <summary>
    /// Reprezentaja samolotów w module widoku 
    /// <author>Adam Witczak , Kamil S³awiñski</author>
    /// </summary>
    public abstract class PlaneView : VertexAnimable, CameraHolder, WaterTrailGenerator
    {

        /// <summary>
        /// Node trzymaj¹cy wizualizacjê torpedy
        /// </summary>
        protected SceneNode torpedoHolder;

        public bool IsReadyForLastWaterTrail
        {
            get
            {
                return
                    (!EngineConfig.LowDetails && Environment.TickCount - lastWaterTrailTime > 50 &&
                     Plane.IsEngineWorking && PlaneNode.Position.y > 0 && PlaneNode.Position.y < 5);
            }
        }

        protected float lastWaterTrailTime = 0.0f;

        public float LastWaterTrailTime
        {
            get { return lastWaterTrailTime; }
            set { lastWaterTrailTime = value; }
        }


        protected Entity planeEntity;

        public Entity PlaneEntity
        {
            get { return planeEntity; }
        }

        protected SceneManager sceneMgr;

        protected Plane plane;

        public Plane Plane
        {
            get { return plane; }
        }

        protected bool isSmokingSlightly = false;

        public bool IsSmokingSlightly
        {
            get { return isSmokingSlightly; }
            set { isSmokingSlightly = value; }
        }

        protected bool isSmokingHeavily = false;

        public bool IsSmokingHeavily
        {
            get { return isSmokingHeavily; }
            set { isSmokingHeavily = value; }
        }

        protected AnimationState animationState;

        public AnimationState AnimationState
        {
            get { return animationState; }
        }

        #region SceneNodes

        protected SceneNode parentNode;
        protected SceneNode planeNode, innerSteeringNode, outerSteeringNode, outerNode, innerNode, idleNode;
        protected SceneNode bladeNode, lWingNode, rWingNode;

        protected SceneNode lWheelNode, rWheelNode, rearWheelNode;
        protected List<SceneNode> cameraHolders;
        protected SceneNode hangaringCameraHolder;


        public List<SceneNode> CameraHolders
        {
            get { return cameraHolders; }
        }


        public SceneNode RearWheelNode
        {
            get { return rearWheelNode; }
        }

        public SceneNode RWheelNode
        {
            get { return rWheelNode; }
        }

        public SceneNode LWheelNode
        {
            get { return lWheelNode; }
        }

        public SceneNode LWingNode
        {
            get { return lWingNode; }
        }

        public SceneNode RWingNode
        {
            get { return rWingNode; }
        }

        protected SceneNode lWheelInnerNode, rWheelInnerNode, rearWheelInnerNode;

        public SceneNode RearWheelInnerNode
        {
            get { return rearWheelInnerNode; }
        }

        public SceneNode RWheelInnerNode
        {
            get { return rWheelInnerNode; }
        }

        public SceneNode LWheelInnerNode
        {
            get { return lWheelInnerNode; }
        }

        public SceneNode PlaneNode
        {
            get { return planeNode; }
        }

        public SceneNode ParentNode
        {
            get { return parentNode; }
        }

        public SceneNode BladeNode
        {
            get { return bladeNode; }
        }

        public SceneNode InnerNode
        {
            get { return innerNode; }
        }

        public SceneNode OuterNode
        {
            get { return outerNode; }
        }

        public SceneNode IdleNode
        {
            get { return idleNode; }
        }

        /// <summary>
        /// Do sterowania (dziób góra/dó³)
        /// </summary>
        public SceneNode InnerSteeringNode
        {
            get { return innerSteeringNode; }
        }

        /// <summary>
        /// Do obrotu na lotniskowcu (samolot jest pochylony i musi sie obracac)
        /// </summary>
        public SceneNode OuterSteeringNode
        {
            get { return outerSteeringNode; }
        }

        #endregion

        protected String name;

        public String Name
        {
            get { return name; }
        }

        protected PlaneNodeAnimationManager animationMgr;

        public PlaneNodeAnimationManager AnimationMgr
        {
            get { return animationMgr; }
        }

        #region Minimap representation

        protected MinimapItem minimapItem;

        public MinimapItem MinimapItem
        {
            get { return minimapItem; }
        }

        public SceneNode HangaringCameraHolder
        {
            get { return hangaringCameraHolder; }
        }

        #endregion

        protected void InitLight(SceneNode parent, ColourValue c1, Vector3 localPosition, Vector2 size)
        {
            Billboard lightbillboard;
            BillboardSet lightbillboardset =
                sceneMgr.CreateBillboardSet(parent.Name + "_lights" + c1.ToString() + "_" + localPosition.ToString(), 1);
            lightbillboardset.MaterialName = "Effects/Flare";
            lightbillboard = lightbillboardset.CreateBillboard(localPosition, c1);
            lightbillboard.SetDimensions(size.x, size.y);
            parent.AttachObject(lightbillboardset);
        }


        private VisibilityNodeAnimation crossHairEffectNodeAnimation;
        public abstract void SetBladeVisibility(bool visible);

        /// <summary>
        /// Wizualizacja podczepionej torpedy
        /// </summary>
        public abstract void ShowTorpedo();

        /// <summary>
        /// Wizualizacja podczepionej torpedy
        /// </summary>
        public abstract void HideTorpedo();


        public void ShowCrossHair(float distance)
        {
            Quaternion q = Quaternion.IDENTITY;
            q.FromAngleAxis(new Radian(Math.HALF_PI), Vector3.UNIT_X);
            crossHairEffectNodeAnimation = EffectsManager.Singleton.RectangularEffect(sceneMgr, innerNode, "CrossHair",
                                                       EffectsManager.EffectType.CROSSHAIR, new Vector3(0, 0, -distance),
                                                       new Vector2(3, 3), q, true);
        }


        public void HideCrossHair()
        {
            crossHairEffectNodeAnimation = null;
            EffectsManager.Singleton.HideSprite(sceneMgr, innerNode, EffectsManager.EffectType.CROSSHAIR, "CrossHair");
        }

        protected void DestroyCrossHair()
        {
            crossHairEffectNodeAnimation = null;
            EffectsManager.Singleton.NoSprite(sceneMgr, innerNode, EffectsManager.EffectType.CROSSHAIR, "CrossHair");
        }

        /// <summary>
        /// Celownik zwiêksza swoj¹ przezroczystoœæ kiedy jest blisko kamery. TODO: na razie jest jeden materia³ dla wszystkich celowników w grze
        /// </summary>
        /// <param name="camera"></param>
        public void UpdateCrossHairVisibility(Camera camera)
        {
            if (crossHairEffectNodeAnimation != null)
            {
                float normalVisibilityDist = 3.0f;
                float dist = (crossHairEffectNodeAnimation.Node.WorldPosition - camera.WorldPosition).Length;
                float visibility = dist * normalVisibilityDist;
                if (visibility < 0) visibility = 0;
                if (visibility > 100) visibility = 100;
                visibility /= 100.0f;

                try
                {
                    MovableObject obj = crossHairEffectNodeAnimation.Node.GetAttachedObject(0);
                    if (obj != null && obj is BillboardSet)
                    {
                        ((BillboardSet)obj).GetMaterial().GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetAlphaOperation(LayerBlendOperationEx.LBX_MODULATE, LayerBlendSource.LBS_TEXTURE, LayerBlendSource.LBS_MANUAL, visibility, visibility);

                    }
                }
                catch (Exception)
                {
                    
                  
                }
                
               
            }
        }

       

        public void ResetWheels()
        {
            if (rWheelInnerNode == null || lWheelInnerNode==null) return;
            rWheelInnerNode.ResetOrientation();
            lWheelInnerNode.ResetOrientation();
            rearWheelInnerNode.ResetOrientation();


            RotateNodeAnimation ra;
            animationMgr.switchTo(PlaneNodeAnimationManager.AnimationType.L_GEAR_UP);
            ra = (animationMgr.CurrentAnimation as RotateNodeAnimation);
            ra.MaxAngle = Math.Abs(ra.MaxAngle);

            animationMgr.switchTo(PlaneNodeAnimationManager.AnimationType.R_GEAR_UP);
            ra = (animationMgr.CurrentAnimation as RotateNodeAnimation);
            ra.MaxAngle = Math.Abs(ra.MaxAngle);

            animationMgr.switchTo(PlaneNodeAnimationManager.AnimationType.REAR_GEAR_UP);
            ra = (animationMgr.CurrentAnimation as RotateNodeAnimation);
            ra.MaxAngle = Math.Abs(ra.MaxAngle);
        }

        public PlaneView(Plane plane, SceneManager sceneMgr, SceneNode parentNode, String name)
        {
            this.sceneMgr = sceneMgr;
            this.plane = plane;
            this.parentNode = parentNode;
            this.name = name;

            planeNode = parentNode.CreateChildSceneNode(name);
            outerSteeringNode = planeNode.CreateChildSceneNode(name + "OuterSteeringNode");
            innerSteeringNode = outerSteeringNode.CreateChildSceneNode(name + "InnerSteeringNode");

            outerNode = innerSteeringNode.CreateChildSceneNode(name + "OuterNode");

            idleNode = outerNode.CreateChildSceneNode(name + "IdleNode");

            innerNode = idleNode.CreateChildSceneNode(name + "InnerNode");
            initSmokeSystems();
            cameraHolders = new List<SceneNode>();
            initOnScene();
        }

        protected virtual void initOnScene()
        {
            initAnimationManager();
            initWheels();
            initBlade();
            animationState = PlaneEntity.GetAnimationState("manual");
        }

        protected abstract void initWheels();
        protected abstract void initBlade();

        public void initAnimationManager()
        {
            animationMgr = new PlaneNodeAnimationManager(0, this);
            animationMgr.disableAll();
        }

        protected void initSmokeSystems()
        {
            EffectsManager.Singleton.Smoke(sceneMgr, OuterNode);
            EffectsManager.Singleton.NoSmoke(sceneMgr, OuterNode);


          //  EffectsManager.Singleton.Smoke(sceneMgr, planeNode, new Vector3(0, -1, 0), Vector3.UNIT_Y);
          //  EffectsManager.Singleton.NoSmoke(sceneMgr, planeNode);
        }

        public virtual void refreshPosition()
        {
            //Modelowy plane jest null'em dla samolocikow reprezentujacych zycia
            if (plane != null)
            {
            	
                Vector2 v = UnitConverter.LogicToWorldUnits(plane.Center);
                planeNode.SetPosition(v.x, v.y, 0.0f);
                if (!plane.IsChangingDirection)
                {
                    if (plane.Direction == Direction.Right)// && !Plane.spinned)
                    {
                        InnerSteeringNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.NEGATIVE_UNIT_Y);
                    }
                    else
                    {
                        InnerSteeringNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_Y);
                    }
                    
					
                    InnerSteeringNode.Orientation *= new Quaternion((float)plane.RelativeAngle, Vector3.UNIT_X);

                    if (Plane.Spinned)
                        InnerSteeringNode.Orientation *= new Quaternion((float)Math.PI, Vector3.UNIT_Z);
                    
                    if(!plane.IsEnemy)
                    { 
                    	//LogManager.Singleton.LogMessage("REFRESH outerNode: " + outerNode.Orientation.w +" "+ outerNode.Orientation.x +" "+ outerNode.Orientation.y +" "+ outerNode.Orientation.z, LogMessageLevel.LML_CRITICAL);
	      				//LogManager.Singleton.LogMessage("REFRESH InnerSteeringNode: " + InnerSteeringNode.Orientation.w +" "+ InnerSteeringNode.Orientation.x +" "+ InnerSteeringNode.Orientation.y +" "+ InnerSteeringNode.Orientation.z, LogMessageLevel.LML_CRITICAL);
	      				//LogManager.Singleton.LogMessage("plane.RelativeAngle " + plane.RelativeAngle, LogMessageLevel.LML_CRITICAL);
                    	
                    }
                   
                }
                // refresh minimap
                if (minimapItem != null)
                {
                    minimapItem.Refresh();
                }
            }
        }

       

       
        public virtual void SmashPaint()
        {
             
        }
        public virtual void Smash()
        {
            SmashPaint();
            animationState = PlaneEntity.GetAnimationState("die");
            animationState.Loop = false;
            animationState.Enabled = true;

            animationMgr.disableIdle();

            SetBladeVisibility(false);
        }

        public virtual void RestorePaint()
        {
        }


        public virtual void Restore()
        {
            RestorePaint();
            animationState = PlaneEntity.GetAnimationState("manual");
            animationState.Loop = false;
            animationState.Enabled = true;
            SetBladeVisibility(true);
            EffectsManager.Singleton.NoSmoke(sceneMgr, OuterNode, EffectsManager.SmokeType.LIGHTSMOKE);
            EffectsManager.Singleton.NoSmoke(sceneMgr, OuterNode, EffectsManager.SmokeType.NORMAL);
            ResetWheels();

            OuterNode.ResetOrientation();
            InnerNode.ResetOrientation();
            animationMgr.disableAll();
            animationMgr.enableBlade();
        }


        #region VertexAnimable members

       

        public void updateTime(float timeSinceLastFrame)
        {
           
            animationState.AddTime(timeSinceLastFrame);
        }

        public void rewind()
        {
            animationState.TimePosition = 0;
        }

        public void enableAnimation()
        {
            animationState.Enabled = true;
        }

        public void disableAnimation()
        {
            animationState.Enabled = false;
        }

        #endregion

        #region CameraHolder members

        public List<SceneNode> GetCameraHolders()
        {
            return cameraHolders;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void ResetCameraHolders();

        #endregion
    }
}