/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-07-08
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Mogre;
using Wof.Controller;
using Wof.View.NodeAnimation;

namespace Wof.View.AmmunitionViews
{
	/// <summary>
	/// Description of FlakBulletView.
	/// </summary>
	internal class FlakBulletView : MissileBaseView<FlakBulletView>
    {
        protected readonly Vector3 hiddenPosition = new Vector3(-120000, -130000, 1000);
		public FlakBulletView(IFrameWork framework) : base(framework)
		{			
         
        }
        
        protected override void preInitOnScene()
        {
            ammunitionModel = sceneMgr.CreateEntity("Flak" + ammunitionID.ToString(), "FlakBullet.mesh");
            ammunitionNode =
                sceneMgr.RootSceneNode.CreateChildSceneNode("Flak" + ammunitionID.ToString(),
                                                            hiddenPosition);

            Vector3 oVector = new Vector3(0, 0, -1);

            innerNode =
                ammunitionNode.CreateChildSceneNode("FlakInner" + ammunitionID.ToString(), new Vector3(0, 0, 0));
            innerNode.AttachObject(ammunitionModel);
            innerNode.SetScale(0.7f,0.7f,0.7f);
            missileAnimation = new ConstRotateNodeAnimation(innerNode, 75, oVector, "ConstRot");
            missileAnimation.Enabled = true;
            missileAnimation.Looped = true;

            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem =
                    new MinimapItem(ammunitionNode, framework.MinimapMgr, "Cube.mesh", ColourValue.White,
                                    ammunitionModel);
                minimapItem.ScaleOverride = new Vector2(4, 2);
                minimapItem.Refresh();
                minimapItem.Hide();
            }

            ammunitionNode.SetVisible(false);
            innerNode.SetVisible(false);
          
        }

	    public override void MoveToHiddenPosition()
	    {
            ammunitionNode.SetPosition(hiddenPosition.x, hiddenPosition.y, hiddenPosition.z);
	    }

	    public override void postInitOnScene()
        {
            base.postInitOnScene();
            innerNode.SetVisible(true);
          
        }

               
      	public override void Hide()
        {
            ammunitionNode.SetPosition(hiddenPosition.x, hiddenPosition.y, hiddenPosition.z);
            innerNode.SetVisible(false, false);
            if (EngineConfig.ExplosionLights && LevelView.IsNightScene) explosionFlash.Visible = false;

            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem.Hide();
            }
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {
            missileAnimation.updateTime(timeSinceLastFrameUpdate);
            missileAnimation.animate();
        }
        
	}
}
