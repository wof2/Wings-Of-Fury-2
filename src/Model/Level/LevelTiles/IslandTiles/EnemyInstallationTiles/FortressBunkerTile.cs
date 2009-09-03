using System;
using System.Collections.Generic;
using System.Text;

using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Weapon;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{
    public class FortressBunkerTile : ConcreteBunkerTile
    {
        private int rocketHitsLeft;

        public bool ShouldBeDestroyed
        {
            get
            {
                return (!IsDestroyed && rocketHitsLeft == 0);
            }
        }

        public FortressBunkerTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int soldierNum, int generalNum ,int type,
                                  List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, soldierNum, generalNum, type, collisionRectangle)
        {
            rocketHitsLeft = 3;
        }

        public void Hit()
        {
            if (!IsDestroyed && rocketHitsLeft > 0) rocketHitsLeft--;          
        }
        
    }
}
