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
		public GunBulletView(IFrameWork framework) : base(framework)
		{		
         
        }

        
       
        protected override void preInitOnScene()
        {
        	Vector3 gun1Pos = new Vector3(0,0, -10);
            Vector3 gun2Pos = new Vector3(0,0, 10);
            float baseWidth = 10;
        
         	prepareGunEffect(gun1Pos, gun2Pos, baseWidth);
			Hide();
        }
        
        protected List<NodeAnimation.NodeAnimation> animations = new List<NodeAnimation.NodeAnimation>();
 
        
        protected void prepareGunEffect(Vector3 gun1Pos, Vector3 gun2Pos, float baseWidth)
		{
		 	  Quaternion orient, trailOrient;
         
		 	ammunitionNode = sceneMgr.RootSceneNode.CreateChildSceneNode("AmmunitionNode"+ammunitionID);
		 	 
		 	  
            orient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient *= new Quaternion(-Math.HALF_PI, Vector3.UNIT_X);
          

            float trailWidth = baseWidth * Math.RangeRandom(1.0f, 1.1f);
            string leftTrailName = EffectsManager.BuildSpriteEffectName(ammunitionNode, EffectsManager.EffectType.GUNTRAIL, "LeftGunTrail" + ammunitionID);
            string rightTrailName = EffectsManager.BuildSpriteEffectName(ammunitionNode, EffectsManager.EffectType.GUNTRAIL, "RightGunTrail" + ammunitionID);
       
            Vector3 leftTrailBase = new Vector3(gun1Pos.x, 0.1f, gun1Pos.z - trailWidth * 0.5f);
            Vector3 rightTrailBase = new Vector3(gun2Pos.x, 0.1f, gun2Pos.z - trailWidth * 0.5f);
          
			bool  showLeftTrail = true, showRightTrail =true;
           
           
            if (showLeftTrail)
            {
            	animations.Add(
                EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode, "LeftGunTrail" + ammunitionID,
                                                           EffectsManager.EffectType.GUNTRAIL,
                                                           leftTrailBase - new Vector3(0, 0, Math.RangeRandom(0.0f, 2.0f)),
                                                           new Vector2(trailWidth, 1.0f),
                                                           trailOrient, false)
            	);
            }

            if (showRightTrail)
            {
            	animations.Add(
                EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode, "RightGunTrail" + ammunitionID,
                                                           EffectsManager.EffectType.GUNTRAIL,
                                                           rightTrailBase - new Vector3(0, 0, Math.RangeRandom(0f, 2.0f)),
                                                           new Vector2(trailWidth, 1.0f),
                                                           trailOrient, false)
            	);
            }

            orient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            trailOrient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
        

            if (showLeftTrail)
            {
            	animations.Add(
                EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode, "LeftGunTrailTop" + ammunitionID,
                                                           EffectsManager.EffectType.GUNTRAIL,
                                                           leftTrailBase - new Vector3(0, 0, Math.RangeRandom(0.5f, 2.0f)),
                                                           new Vector2(trailWidth, 1.0f),
                                                           trailOrient, false)
            	);
            }
            if (showRightTrail)
            {
            	animations.Add(
                EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode, "RightGunTrailTop" + ammunitionID,
                                                           EffectsManager.EffectType.GUNTRAIL,
                                                           rightTrailBase - new Vector3(0, 0, Math.RangeRandom(0.5f, 2.0f)),
                                                           new Vector2(trailWidth, 1.0f),
                                                           trailOrient, false)
            	);
            }
        	
		}
        
        public override void postInitOnScene()
        {
          //  base.postInitOnScene();         
            refreshPosition();         
            ammunitionNode.SetVisible(true, true);
        }

        
               
      	public override void Hide()
        {
      		ammunitionNode.SetVisible(false, true);
      		//for(NodeAnimation.VisibilityNodeAnimation ani : animations) {
      		//	ani.
      		//}
            
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {          
        }
        
	}
}
