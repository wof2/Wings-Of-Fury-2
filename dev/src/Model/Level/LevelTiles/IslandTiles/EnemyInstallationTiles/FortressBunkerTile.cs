using System;
using System.Collections.Generic;
using System.Text;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Common;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{
    public class FortressBunkerTile : ConcreteBunkerTile
    {
        public FortressBunkerTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int soldierNum, int type,
                                  List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, soldierNum, type, collisionRectangle)
        {

        }
    }
}
