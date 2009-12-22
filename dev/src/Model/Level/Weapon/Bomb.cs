/*
 * Copyright 2008 Adam Witczak, Jakub Tê¿ycki, Kamil S³awiñski, Tomasz Bilski, Emil Hornung, Micha³ Ziober
 *
 * This file is part of Wings Of Fury 2.
 * 
 * Freeware Licence Agreement
 * 
 * This licence agreement only applies to the free version of this software.
 * Terms and Conditions
 * 
 * BY DOWNLOADING, INSTALLING, USING, TRANSMITTING, DISTRIBUTING OR COPYING THIS SOFTWARE ("THE SOFTWARE"), YOU AGREE TO THE TERMS OF THIS AGREEMENT (INCLUDING THE SOFTWARE LICENCE AND DISCLAIMER OF WARRANTY) WITH WINGSOFFURY2.COM THE OWNER OF ALL RIGHTS IN RESPECT OF THE SOFTWARE.
 * 
 * PLEASE READ THIS DOCUMENT CAREFULLY BEFORE USING THE SOFTWARE.
 *  
 * IF YOU DO NOT AGREE TO ANY OF THE TERMS OF THIS LICENCE THEN DO NOT DOWNLOAD, INSTALL, USE, TRANSMIT, DISTRIBUTE OR COPY THE SOFTWARE.
 * 
 * THIS DOCUMENT CONSTITUES A LICENCE TO USE THE SOFTWARE ON THE TERMS AND CONDITIONS APPEARING BELOW.
 * 
 * The Software is licensed to you without charge for use only upon the terms of this licence, and WINGSOFFURY2.COM reserves all rights not expressly granted to you. WINGSOFFURY2.COM retains ownership of all copies of the Software.
 * 1. Licence
 * 
 * You may use the Software without charge.
 *  
 * You may distribute exact copies of the Software to anyone.
 * 2. Restrictions
 * 
 * WINGSOFFURY2.COM reserves the right to revoke the above distribution right at any time, for any or no reason.
 *  
 * YOU MAY NOT MODIFY, ADAPT, TRANSLATE, RENT, LEASE, LOAN, SELL, REQUEST DONATIONS OR CREATE DERIVATE WORKS BASED UPON THE SOFTWARE OR ANY PART THEREOF.
 * 
 * The Software contains trade secrets and to protect them you may not decompile, reverse engineer, disassemble or otherwise reduce the Software to a humanly perceivable form. You agree not to divulge, directly or indirectly, until such trade secrets cease to be confidential, for any reason not your own fault.
 * 3. Termination
 * 
 * This licence is effective until terminated. The Licence will terminate automatically without notice from WINGSOFFURY2.COM if you fail to comply with any provision of this Licence. Upon termination you must destroy the Software and all copies thereof. You may terminate this Licence at any time by destroying the Software and all copies thereof. Upon termination of this licence for any reason you shall continue to be bound by the provisions of Section 2 above. Termination will be without prejudice to any rights WINGSOFFURY2.COM may have as a result of this agreement.
 * 4. Disclaimer of Warranty, Limitation of Remedies
 * 
 * TO THE FULL EXTENT PERMITTED BY LAW, WINGSOFFURY2.COM HEREBY EXCLUDES ALL CONDITIONS AND WARRANTIES, WHETHER IMPOSED BY STATUTE OR BY OPERATION OF LAW OR OTHERWISE, NOT EXPRESSLY SET OUT HEREIN. THE SOFTWARE, AND ALL ACCOMPANYING FILES, DATA AND MATERIALS ARE DISTRIBUTED "AS IS" AND WITH NO WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED. WINGSOFFURY2.COM DOES NOT WARRANT, GUARANTEE OR MAKE ANY REPRESENTATIONS REGARDING THE USE, OR THE RESULTS OF THE USE, OF THE SOFTWARE WITH RESPECT TO ITS CORRECTNESS, ACCURACY, RELIABILITY, CURRENTNESS OR OTHERWISE. THE ENTIRE RISK OF USING THE SOFTWARE IS ASSUMED BY YOU. WINGSOFFURY2.COM MAKES NO EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS INCLUDING, WITHOUT LIMITATION, THE WARRANTIES OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE WITH RESPECT TO THE SOFTWARE. NO ORAL OR WRITTEN INFORMATION OR ADVICE GIVEN BY WINGSOFFURY2.COM, IT'S DISTRIBUTORS, AGENTS OR EMPLOYEES SHALL CREATE A WARRANTY, AND YOU MAY NOT RELY ON ANY SUCH INFORMATION OR ADVICE.
 * 
 * IMPORTANT NOTE: Nothing in this Agreement is intended or shall be construed as excluding or modifying any statutory rights, warranties or conditions which by virtue of any national or state Fair Trading, Trade Practices or other such consumer legislation may not be modified or excluded. If permitted by such legislation, however, WINGSOFFURY2.COM' liability for any breach of any such warranty or condition shall be and is hereby limited to the supply of the Software licensed hereunder again as WINGSOFFURY2.COM at its sole discretion may determine to be necessary to correct the said breach.
 * 
 * IN NO EVENT SHALL WINGSOFFURY2.COM BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, AND THE LOSS OF BUSINESS INFORMATION OR COMPUTER PROGRAMS), EVEN IF WINGSOFFURY2.COM OR ANY WINGSOFFURY2.COM REPRESENTATIVE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. IN ADDITION, IN NO EVENT DOES WINGSOFFURY2.COM AUTHORISE YOU TO USE THE SOFTWARE IN SITUATIONS WHERE FAILURE OF THE SOFTWARE TO PERFORM CAN REASONABLY BE EXPECTED TO RESULT IN A PHYSICAL INJURY, OR IN LOSS OF LIFE. ANY SUCH USE BY YOU IS ENTIRELY AT YOUR OWN RISK, AND YOU AGREE TO HOLD WINGSOFFURY2.COM HARMLESS FROM ANY CLAIMS OR LOSSES RELATING TO SUCH UNAUTHORISED USE.
 * 5. General
 * 
 * All rights of any kind in the Software which are not expressly granted in this Agreement are entirely and exclusively reserved to and by WINGSOFFURY2.COM.
 * 
 * 
 */

using System;
using System.Diagnostics;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Klasa implementujaca zachowanie bomb na planszy.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class Bomb : Ammunition
    {
        #region Const Fields

        /// <summary>
        /// Szerokosc prostok¹ta opisuj¹cego bombe.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float BombWidth = 0.2f;

        /// <summary>
        /// Wysokosc prostokata opisujacego bombe.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float BombHeight = 0.85f;

        #endregion

        #region Private Fields

        /// <summary>
        /// Wartosc zapamietuje czas od ostatniego dzialania 
        /// sil cierzkosci oraz oporu powietrza.
        /// </summary>
        /// <author>Michal Ziober</author>
        private int timer = Environment.TickCount;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor dwuparametrowy.
        /// </summary>
        /// <param name="x">Wspolrzedna startowa x.</param>
        /// <param name="y">Wspolrzedna startowa y.</param>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu Level.</param>
        /// <author>Michal Ziober</author>
        public Bomb(float x, float y, PointD planeSpeed, Level level, float angle, IObject2D owner)
            : base(planeSpeed, level, angle, owner)
        {
            //prostokat opisujacy obiekt.
            boundRectangle = new Quadrangle(new PointD(x, y - 1), BombWidth, BombHeight);

            //wektor ruchu.
            moveVector = new PointD(WidthCoefficient * planeSpeed.X, HeightCoefficient);
        }

        /// <summary>
        /// Konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="position">Pozycja startowa amunicji.</param>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <author>Michal Ziober</author>
        public Bomb(PointD position, PointD planeSpeed, Level level, float angle, IObject2D owner)
            : this(position.X, position.Y, planeSpeed, level, angle, owner)
        {
        }

        #endregion

        #region IMove Members

        /// <summary>
        /// Zmienia pozycje bomby w zaleznosci od czasu oraz
        /// sprawdza kolizje z obiektami na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniej zmiany</param>
        /// <author>Michal Ziober</author>
        public override void Move(int time)
        {
            base.Move(time);
            //zmienia pozycje.
            ChangePosition(time);

            //sprawdzam zderzenie z wrogim samolotem.
            CheckCollisionWithPlanes();

            //obsluga zderzenia z ziemia.
            CheckCollisionWithGround();

            //sprawdzam kolizje z lotniskowcem.
            CheckCollisionWithCarrier(this);
        }

        /// <summary>
        /// Zmienia pozycje bomby.
        /// </summary>
        /// <param name="time">Czas od ostatniego przesuniecia.</param>
        /// <author>Michal Ziober</author>
        private void ChangePosition(int time)
        {
            //zaleznosc od uplynietego czasu
            float coefficient = Mathematics.GetMoveFactor(time);

            //co n ms zwiekszamy przyspieszenie.
            if (Environment.TickCount - timer > GameConsts.Bomb.AccelerationInterval)
            {
                //uwzgledniam opor powietrza oraz przyspieszenie ziemskie,
                moveVector = new PointD(moveVector.X*GameConsts.Bomb.AirResistance,
                                        moveVector.Y*GameConsts.Bomb.Gravitation);
                timer = Environment.TickCount;
            }

            //wyliczam o ile przesunac bombe.
            PointD vector = new PointD(moveVector.X*coefficient, moveVector.Y*coefficient);

            //przesuwam prostokat.
            boundRectangle.Move(vector);
        }

        /// <summary>
        /// Sprawdza kolizje z samolotami wroga. 
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithPlanes()
        {
            if (refToLevel.EnemyPlanes.Count > 0)
            {
                foreach (EnemyPlane ep in refToLevel.EnemyPlanes)
                {
                    //sprawdzam czy aby nie ma zderzenia.
                    if (boundRectangle.Intersects(ep.Bounds))
                    {
                        //powa¿nie uszkadam samolot przeciwnika.
                        ep.Hit(ep.MaxOil * 0.75f, 0);

                        //wysylam sygnal do kontrollera
                        refToLevel.Controller.OnEnemyPlaneBombed(ep, this);

                        refToLevel.Statistics.HitByBomb++;

                        //niszcze bombe.
                        state = MissileState.Destroyed;
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza kolizje z terenem, bunkrami , barakami, etc.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithGround()
        {
            int index = Mathematics.PositionToIndex(Center.X);
            LevelTile tile;
            if (index > -1 && index < refToLevel.LevelTiles.Count)
            {
                tile = refToLevel.LevelTiles[index];
                //jeœli nie ma kolizji wyjdz.
                CollisionType c = refToLevel.LevelTiles[index].InCollision(this.boundRectangle);
                if (c == CollisionType.None)
                    return;
                
                if (tile is BarrelTile)
                {
                    BarrelTile destroyTile = tile as BarrelTile;
                    if (!destroyTile.IsDestroyed)
                    {
                        destroyTile.Destroy();
                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                        refToLevel.Statistics.HitByBomb += refToLevel.KillVulnerableSoldiers(index, 2, true);
                    }
                    else
                        refToLevel.Controller.OnTileBombed(tile, this);
                }
                else if (tile is EnemyInstallationTile)
                {
                    WoodBunkerTile woodbunker = null;
                    ShipBunkerTile shipbunker = null;
                    BarrackTile barrack = null;
                    if (tile is WoodBunkerTile)
                    {
                        if ((woodbunker = tile as WoodBunkerTile) != null && !woodbunker.IsDestroyed)
                        {
                            refToLevel.Controller.OnTileDestroyed(tile, this);
                            refToLevel.Statistics.HitByBomb++;
                            woodbunker.Destroy();
                        }
                        else
                            refToLevel.Controller.OnTileBombed(tile, this);
                    }
                    else if (tile is ShipWoodBunkerTile)
                    {
                        if ((shipbunker = tile as ShipBunkerTile) != null && !shipbunker.IsDestroyed)
                        {
                            refToLevel.Controller.OnTileDestroyed(tile, this);
                            refToLevel.Statistics.HitByBomb++;
                            shipbunker.Destroy();
                        }
                        else
                            refToLevel.Controller.OnTileBombed(tile, this);
                    }
                    else if (tile is BarrackTile)
                    {
                        if ((barrack = tile as BarrackTile) != null && !barrack.IsDestroyed)
                        {
                            refToLevel.Controller.OnTileDestroyed(tile, this);
                            refToLevel.Statistics.HitByBomb++;
                            barrack.Destroy();
                        }
                        else
                            refToLevel.Controller.OnTileBombed(tile, this);
                    }
                    else
                        refToLevel.Controller.OnTileBombed(tile, this);
                }       
                else
                {
                    refToLevel.Controller.OnTileBombed(tile, this);
                }

                //zabijam zolnierzy, ktorzy sa w polu razenia.
                refToLevel.Statistics.HitByBomb += refToLevel.KillVulnerableSoldiers(index, 0, true);

                //niszcze bombe.
                state = MissileState.Destroyed;
            }
        }

        /// <summary>
        /// Sprawdza pole o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks trafionego elementu.</param>
        /// <returns>Jesli obiekt da sie zniszczyc bomba zwraca true, w przeciwnym przypadku false.</returns>
        /*private bool CanBeDestroyed(int index)
        {
            LevelTile tile = refToLevel.LevelTiles[index];
            return ((tile is WoodBunkerTile) || (tile is BarrackTile) || (tile is BarrelTile) || (tile is ShipBunkerTile));
        }*/

        #endregion
    }
}