/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-07-08
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using Math = Mogre.Math;

namespace Wof.View.AmmunitionViews
{
	/// <summary>
	/// Description of GunBulletView.
	/// </summary>
	internal class GunBulletView : MissileBaseView<GunBulletView>
	{
        protected readonly Vector3 hiddenPosition = new Vector3(-130000, -100000, 1000);

		public GunBulletView(IFrameWork framework) : base(framework)
		{		
         
        }


       

       
        
   //     protected List<NodeAnimation.NodeAnimation> animations = new List<NodeAnimation.NodeAnimation>();

       
        protected string getLocalSpriteName(string localName)
        {
            string name = "GunTrail" + ammunitionID + "_" + localName;
            return name;
        }

        protected string getLocalSpriteNameTop(string localName)
        {
            string name = "GunTrailTop" + ammunitionID + "_" + localName;
            return name;
        }

        /*protected string getEffectNameTop()
        {
            return "GunTrailTop" + ammunitionID + "_" + (localSpriteId++);
        }*/

        protected void hideEffect(string localName)
        {
            EffectsManager.Singleton.HideSprite(sceneMgr, ammunitionNode, EffectsManager.EffectType.GUNTRAIL, getLocalSpriteName(localName));
            EffectsManager.Singleton.HideSprite(sceneMgr, ammunitionNode, EffectsManager.EffectType.GUNTRAIL, getLocalSpriteNameTop(localName));
         }
        public override void MoveToHiddenPosition()
        {
            ammunitionNode.SetPosition(hiddenPosition.x, hiddenPosition.y, hiddenPosition.z);
        }

        protected override void preInitOnScene()
        {
           

            // showaj poprzednio skaszowane sprajty.
           
            float baseWidth = 1.5f;
            // FIXME: przesuwac nodey zamiast tworzyc wiecej niepotrzebnie
            prepareGunEffect(Vector3.ZERO, baseWidth, "left");
            prepareGunEffect(Vector3.ZERO, baseWidth, "right");
            prepareGunEffect(Vector3.ZERO, baseWidth, "middle");

            /*hideEffect("left");
            hideEffect("right");
            hideEffect("middle");*/
            Hide();
          
        }
        

        protected void prepareGunEffect(Vector3 gunPos, float baseWidth, string localName)
		{
		 	Quaternion orient, trailOrient;

            if (ammunitionNode == null)
            {
                ammunitionNode = sceneMgr.RootSceneNode.CreateChildSceneNode("AmmunitionNode" + ammunitionID, hiddenPosition);
		 	 
            }
            
		 	  
            orient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient *= new Quaternion(-Math.HALF_PI, Vector3.UNIT_X);
          

            float trailWidth = baseWidth * Math.RangeRandom(1.0f, 1.1f);


            Vector3 trailBase = new Vector3(gunPos.x, gunPos.y, gunPos.z);

            
           
        	//animations.Add(
            EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode,
                                                       getLocalSpriteName(localName),
                                                       EffectsManager.EffectType.GUNTRAIL,
                                                       trailBase - new Vector3(0, 0, Math.RangeRandom(-0.5f, 0.5f)),
                                                       new Vector2(trailWidth, 1.0f),
                                                       trailOrient, true);
        	//);
       

            orient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            trailOrient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
        

            
        	//animations.Add(
            EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode,
                                                       getLocalSpriteNameTop(localName),
                                                       EffectsManager.EffectType.GUNTRAIL,
                                                       trailBase - new Vector3(0, 0, Math.RangeRandom(0.5f, 2.0f)),
                                                       new Vector2(trailWidth, 1.0f),
                                                       trailOrient, true);
        	//);
           
        	
		}

        private PlaneType getPlaneType()
        {
             if((Ammunition.Owner is Model.Level.Planes.Plane))
             {
                 PlaneType type = (Ammunition.Owner as Model.Level.Planes.Plane).PlaneType;
                 return type;
             }
            return PlaneType.P47;
        }

        private Vector3 getGunPosLeft()
        {
                if (getPlaneType() == PlaneType.B25 || getPlaneType() == PlaneType.Betty)
                {
                    return new Vector3(-2.5f, -0.3f, -0.3f);
                } else
                {
                    return new Vector3(-1.5f, -0.6f, -0.3f);
                }

        }


        private Vector3 getGunPosRight()
        {
            if (getPlaneType() == PlaneType.B25 || getPlaneType() == PlaneType.Betty)
            {
                return new Vector3(2.5f, -0.3f, -0.3f);
            }
            else
            {
                return new Vector3(1.5f, -0.6f, -0.3f);
            }

        }

        private Vector3 getGunPosMiddle()
        {
            return new Vector3(0.0f, 0.3f, -0.3f);

        }
        
       
        public override void postInitOnScene()
        {

          //  Console.WriteLine("FREE BULLETS: " + GunBulletView.missileAvailablePool.Count);
           
        	if (ammunition is GunBullet)
            {
               
                if ((ammunition as GunBullet).IsDoubleView)
                {
                    float baseWidth = 1.5f;
                    prepareGunEffect(getGunPosLeft(), baseWidth, "left");
                    prepareGunEffect(getGunPosRight(), baseWidth, "right");
                }
                else
                {
                    
                    float baseWidth = 1.5f;
                    prepareGunEffect(getGunPosMiddle(), baseWidth, "middle");

                }


            }


            //Hide();
          //  base.postInitOnScene();         
            refreshPosition();         
            //ammunitionNode.SetVisible(true, true);
        }

        
      	public override void Hide()
        {
            if (ammunitionNode != null)
            {
                ammunitionNode.SetPosition(hiddenPosition.x, hiddenPosition.y, hiddenPosition.z);
                ammunitionNode.SetVisible(false, true);
            }
            hideEffect("left");
            hideEffect("right");
            hideEffect("middle");
      		//for(NodeAnimation.VisibilityNodeAnimation ani : animations) {
      		//	ani.
      		//}
            
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {          
        }
        
      
        public override void refreshPosition()
        {
            if (ammunition != null)
            {

                Vector3 axis;
                if (ammunition.Direction == Direction.Right)
                {
                    axis = Vector3.NEGATIVE_UNIT_Y;
                }
                else
                {
                    axis = Vector3.UNIT_Y;
                }


                GunBullet gb = ammunition as GunBullet;
                Vector2 v = UnitConverter.LogicToWorldUnits(ammunition.Center);
                ammunitionNode.Orientation = gb.LaunchOrientation;
                ammunitionNode.SetPosition((float)(v.x), (float)(v.y), gb.Position3.z);
               



            }
        	
           
        }
        
	}
}
