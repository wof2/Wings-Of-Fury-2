using System;
using System.Collections.Generic;
using System.Text;
using Wof.Model.Level.Common;
//using Wof.Model.Level.LevelTiles;
//using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
//using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Configuration;
namespace Wof.Model.Level.Infantry
{
    /// <author>Kamil S³awiñski</author>
    public class General : Soldier
    {

        /// <summary>
        /// Publiczny konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="posX">Pozycja startowa zolnierza (mierzona w tilesIndex.</param>
        /// <param name="direct">Kierunek w ktorym sie porusza.(Prawo,Lewo)</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <author>Kamil S³awiñski</author>
        /// <param name="offset"></param>
        public General(float posX, Direction direct, Level level, float offset)
            : base(posX,direct,level,offset, false)
        {

        }

        public override void Move(int time)
        {
            //Jesli zolnierz zyje.
            if (IsAlive)
            {

                int tileIndex = Mathematics.PositionToIndex(xPos);
                if (tileIndex != startPosition) leftBornTile = true;
                if (canReEnter //czy moze wejsc ponownie do bunkra.
                    && (tileIndex != startPosition) //jesli bunkier nie jest rodzicem.
                    && IsBunker(tileIndex) //jesli to jest bunkier.
                    && (time % ProbabilityCoefficient == 0)) //losowosc.
                {
                    BunkerTile bunker = refToLevel.LevelTiles[tileIndex] as BunkerTile;
                    if (bunker.IsDestroyed && bunker.CanReconstruct)
                    {
                        bunker.Reconstruct();
                        refToLevel.Controller.OnTileRestored(bunker);
                    }
                    if (!bunker.IsDestroyed)
                    {
                        //dodaje zolnierza do bunkra.
                        bunker.AddGeneral();
                        //wyslam sygnal do controllera aby usunal zolnierza z widoku.
                        refToLevel.Controller.UnregisterSoldier(this);
                        //usuwam zolnierza z planszy.
                        _soldierStatus = SoldierStatus.InBunker;
                    }
                    else ChangeLocation(time);
                }
                else ChangeLocation(time);


                //sprawdza czy ulynal czas bezsmiertelnosci
                if (!canDie)
                {
                    protectedTime += time;
                    if (protectedTime >= TimeUnit)
                        canDie = true;
                }

                //sprawdza czy ponownie moze wejsc do bunkra.
                if (!canReEnter)
                {
                    homelessCounterTime += time;
                    if (homelessCounterTime >= GameConsts.Soldier.HomelessTime)
                        canReEnter = true;
                }
            }
        }
    }
}
