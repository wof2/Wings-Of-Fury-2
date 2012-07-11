using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level;
using Wof.View.Effects;
using Wof.View.NodeAnimation;

namespace wingitor.Tests
{
    public class FlakBunkerTestScene : ISceneTest
    {
        public IFrameWork Framework { get; set; }
       
       
        public void OnRegisterLevel(Level currentLevel)
        {

            /*
            float angle;
            angle = Mogre.Math.RangeRandom(-20, 20);

            VisibilityNodeAnimation na;
            na = EffectsManager.Singleton.Sprite(Framework.SceneMgr, Framework.SceneMgr.RootSceneNode,
                                           new Vector3(0, 50, 0)+
                                           ViewHelper.UnsignedRandomVector3(5, 1.5f, 4), new Vector2(60, 60),
                                           EffectsManager.EffectType.DUST_CLOUD, true, "0", new Quaternion(new Radian(new Degree(180)), Vector3.UNIT_Y) * new Quaternion(new Radian(new Degree(-90)), Vector3.UNIT_X) * new Quaternion(new Radian(new Degree(angle)), Vector3.UNIT_Y)
                                           );
            na.Duration = 1.5f;

            angle = Mogre.Math.RangeRandom(-20, 20);
            na = EffectsManager.Singleton.Sprite(Framework.SceneMgr, Framework.SceneMgr.RootSceneNode,
                                          new Vector3(0, 50, 0) +
                                          ViewHelper.UnsignedRandomVector3(5, 1.5f, 4), new Vector2(60, 60),
                                          EffectsManager.EffectType.DUST_CLOUD, true, "1", new Quaternion(new Radian(new Degree(-90)), Vector3.UNIT_X) * new Quaternion(new Radian(new Degree(180)), Vector3.UNIT_Y) * new Quaternion(new Radian(new Degree(angle)), Vector3.UNIT_Y)
                                          );
            na.Duration = 1.5f;*/
        }
    }
}
