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
    public class ExplosionTestScene : ISceneTest
    {
        public IFrameWork Framework { get; set; }
       
       
        public void OnRegisterLevel(Level currentLevel)
        {

            int i = -70;
            int space = 30;

            EffectsManager.Singleton.Sprite(
             Framework.SceneMgr,
             Framework.SceneMgr.RootSceneNode,
             new Vector3(i, 50, 0),
             new Vector2(20, 20),
             EffectsManager.EffectType.GUNTRAIL,
             true,
             0
             );
            i += space;

          EffectsManager.Singleton.Sprite(
           Framework.SceneMgr,
           Framework.SceneMgr.RootSceneNode,
           new Vector3(i, 50, 0),
           new Vector2(20, 20),
           EffectsManager.EffectType.FLAK,
           true,
           0
           );
          i += space;

            EffectsManager.Singleton.Sprite(
            Framework.SceneMgr,
            Framework.SceneMgr.RootSceneNode,
            new Vector3(i, 50, 0),
            new Vector2(20, 20),
            EffectsManager.EffectType.EXPLOSION1,
            true,
            0
            );
            i += space;


            EffectsManager.Singleton.Sprite(
              Framework.SceneMgr,
              Framework.SceneMgr.RootSceneNode,
              new Vector3(i, 50, 0),
              new Vector2(20, 20),
              EffectsManager.EffectType.EXPLOSION2,
              true,
              0
              );
            i += space;

            EffectsManager.Singleton.Sprite(
                Framework.SceneMgr,
                Framework.SceneMgr.RootSceneNode,
                new Vector3(i, 50, 0),
                new Vector2(20, 20),
                EffectsManager.EffectType.EXPLOSION3,
                true,
                0
                );
            i += space;

            EffectsManager.Singleton.Sprite(
               Framework.SceneMgr,
               Framework.SceneMgr.RootSceneNode,
               new Vector3(i, 50, 0),
               new Vector2(20, 20),
               EffectsManager.EffectType.EXPLOSION4,
               true,
               0
               );
            i += space;



            // mixed
            EffectsManager.Singleton.Sprite(
               Framework.SceneMgr,
               Framework.SceneMgr.RootSceneNode,
               new Vector3(i, 50, 0),
               new Vector2(20, 20),
               EffectsManager.EffectType.EXPLOSION4,
               true,
               1
               );

            EffectsManager.Singleton.Sprite(
              Framework.SceneMgr,
              Framework.SceneMgr.RootSceneNode,
              new Vector3(i, 50, 0),
              new Vector2(15, 15),
              EffectsManager.EffectType.EXPLOSION1,
              true,
              1
              );
            i += space;

            // mixed
            EffectsManager.Singleton.Sprite(
               Framework.SceneMgr,
               Framework.SceneMgr.RootSceneNode,
               new Vector3(i, 50, 0),
               new Vector2(20, 20),
               EffectsManager.EffectType.EXPLOSION4,
               true,
               2
               );

            EffectsManager.Singleton.Sprite(
              Framework.SceneMgr,
              Framework.SceneMgr.RootSceneNode,
              new Vector3(i, 50, 0),
              new Vector2(15, 15),
              EffectsManager.EffectType.EXPLOSION2,
              true,
              2
              );
            i += space;


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
