using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Planes;
using Wof.View.Effects;
using Wof.View.NodeAnimation;

namespace wingitor.Tests
{
    public class EnemyBomberTestScene : ISceneTest
    {
        public IFrameWork Framework { get; set; }
       
       
        public void OnRegisterLevel(Level currentLevel)
        {
        	EnemyPlaneBase enemyPlane;
         //   enemyPlane = new EnemyFighter(currentLevel);        
         //   addEnemyPlane(currentLevel, enemyPlane);
            
         //   enemyPlane = new EnemyBomber(currentLevel);        
          //  addEnemyPlane(currentLevel, enemyPlane);
            
            enemyPlane = new EnemyBomber(currentLevel);        
            addEnemyPlane(currentLevel, enemyPlane);
        }
        
        protected void addEnemyPlane(Level currentLevel, EnemyPlaneBase enemyPlane) {
        	
        	 
         	
             StartPositionInfo info = new StartPositionInfo();
             
             info.Position = new PointD( UnitConverter.WorldToLogicUnits( Framework.Camera.RealPosition + ViewHelper.RandomVector3(50,20,0)));
        	 info.Direction = Direction.Right;
        	 info.EngineState = EngineState.Working;
        	 info.PositionType = StartPositionType.Airborne;
        //	 info.Speed = 0.1f;
        	 info.WheelsState = WheelsState.In;
        	 enemyPlane.ReInit(info);
             enemyPlane.RegisterWeaponEvent += currentLevel.enemyPlane_RegisterWeaponEvent;
        
        
             currentLevel.EnemyPlanes.Add(enemyPlane);
             currentLevel.Controller.OnRegisterPlane(enemyPlane);
           //  currentLevel.UserPlane.Speed = 0;
        }
        
        
    }
}
