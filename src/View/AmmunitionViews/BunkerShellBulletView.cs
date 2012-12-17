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
    internal class BunkerShellBulletView : MissileBaseView<BunkerShellBulletView>
	{
	    protected readonly Vector3 hiddenPosition = new Vector3(-120000, -100000, 0);
        public BunkerShellBulletView(IFrameWork framework)
            : base(framework)
		{			
         
        }
        
        protected override void preInitOnScene()
        {
            ammunitionModel = sceneMgr.CreateEntity("BunkerShell" + ammunitionID.ToString(), "FlakBullet.mesh");
            ammunitionNode =
                sceneMgr.RootSceneNode.CreateChildSceneNode("BunkerShell" + ammunitionID.ToString(),
                                                            hiddenPosition);

            Vector3 oVector = new Vector3(0, 0, -1);

            innerNode =
                ammunitionNode.CreateChildSceneNode("BunkerShellInner" + ammunitionID.ToString(), new Vector3(0, 0, 0));
            innerNode.AttachObject(ammunitionModel);

            missileAnimation = new ConstRotateNodeAnimation(innerNode, 75, oVector, "ConstRot");
            missileAnimation.Enabled = true;
            missileAnimation.Looped = true;


            ammunitionNode.SetVisible(false);
            innerNode.SetVisible(false);
          
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

        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {
            missileAnimation.updateTime(timeSinceLastFrameUpdate);
            missileAnimation.animate();
        }
        
	}
}
