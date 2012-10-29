/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
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
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Implementacja torpedy.
    /// </summary>
    public class Torpedo : Ammunition
    {
        #region Static Fields

        /// <summary>
        /// Szerokosc prostokąta opisującego torpede.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const float TorpedoWidth = 2.5f;

        /// <summary>
        /// Wysokosc prostokata opisujacego torpede.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const float TorpedoHeight = 0.35f;

        /// <summary>
        /// Minimalna wspolrzedna y, po przekroczeniu ktorej sprawdzana jest kolizja
        /// z powierzchnia.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const float MinYPosition = 1.5f;


        /// <summary>
        /// Maksymalny dopuszczalny dystans pomiedzy samolotem rodzicem a torpeda.
        /// Jesli dystans bedzie wiekszy rakieta bedzie odrejestrowana.
        /// </summary>
        /// <author>Adam Witczak</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int MaxDistanceToPlane = 600;

        /// <summary>
        /// Maksymalny dopuszczalny pionowy dystans 
        /// pomiedzy torpeda a samolotem rodzicem.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int MaxHeightDistanceToPlane = 200;



        #endregion

        #region Private Fields

        /// <summary>
        /// Czy torpeda znajduje sie w wodzie.
        /// </summary>
        private bool mIsInWater;


        public bool IsInWater
        {
            get
            {
                return mIsInWater;
            }

        }

        protected float dropAngle;

        protected float dropHeight;

        protected float maxDropHeight = 20.0f;

        /// <summary>
        /// Odległość jaką przepłynęła torpeda do tej port
        /// </summary>
        protected float waterTravelDistance = 0;

        /// <summary>
        /// Max. odległość jaką może przepłynąć torpeda
        /// </summary>
        protected float maxWaterTravelDistance = 100.0f;

        /// <summary>
        /// Min. odległość jaką może przepłynąć torpeda zanim się uzbroi
        /// </summary>
        protected float minWaterTravelDistance = 15.0f;


        /// <summary>
        /// Wartosc zapamietuje czas od ostatniego przesuniecia.
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
        /// <param name="angle"></param>
        /// <param name="owner"></param>
        /// <author>Michal Ziober</author>
        public Torpedo(float x, float y, PointD planeSpeed, Level level, float angle, IObject2D owner)
            : base(level, angle, owner)
        {
            dropAngle = owner.RelativeAngle;
            //prostokat opisujacy obiekt.
            this.boundRectangle = new Quadrangle(new PointD(x, y - 1), TorpedoWidth, TorpedoHeight);
            dropHeight = y - 1;
            //wektor ruchu.
            this.moveVector = new PointD(WidthCoefficient * planeSpeed.X, HeightCoefficient);
        }

        /// <summary>
        /// Konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="position">Pozycja startowa amunicji.</param>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <author>Michal Ziober</author>
        public Torpedo(PointD position, PointD planeSpeed, Level level, float angle, IObject2D owner)
            : this(position.X, position.Y, planeSpeed, level, angle, owner)
        { }

        #endregion

        #region IMove

        /// <summary>
        /// Zmienia pozycje torpedy w zaleznosci od czasu oraz
        /// sprawdza kolizje z obiektami na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniej zmiany</param>
        /// <author>Michal Ziober</author>
        public override void Move(int time)
        {
            base.Move(time);

            //    if (!Unregister())
            {
                if (this.mIsInWater)
                {
                    //zmienia pozycje.
                    ChangePositionInWater(time);
                    //sprawdza kolizje z ziemia.
                    CheckCollisionWithGroundAndShips();
                }
                else
                {
                    //zmienia pozycje
                    ChangePositionInAir(time);

                    //sprawdza kolizje z wrogimi samolotami.
                    CheckCollisionWithPlanes();

                    //sprawdza kolizje z ziemia.
                    CheckCollisionWithGroundAndShips();
                }
            }
        }

        /// <summary>
        /// Zmienia pozycje torpedy w powietrzu.
        /// </summary>
        /// <param name="time">Czas od ostatniego przesuniecia.</param>
        /// <author>Michal Ziober</author>
        private void ChangePositionInAir(int time)
        {
            //zaleznosc od uplynietego czasu
            float coefficient = Mathematics.GetMoveFactor(time);

            //co n ms zwiekszamy przyspieszenie.
            if (Environment.TickCount - this.timer > GameConsts.Bomb.AccelerationInterval)
            {
                //uwzgledniam opor powietrza oraz przyspieszenie ziemskie,
                base.moveVector = new PointD(moveVector.X * GameConsts.Bomb.AirResistance,
                                        moveVector.Y * GameConsts.Bomb.Gravitation);
                this.timer = Environment.TickCount;
            }

            //wyliczam o ile przesunac bombe.
            PointD vector = new PointD(moveVector.X * coefficient, moveVector.Y * coefficient);

            //przesuwam prostokat.
            base.boundRectangle.Move(vector);

            //jesli ponizej linii wody to bedzie plynac.
            if (boundRectangle.Center.Y < 0)
            {
                this.mIsInWater = true;
                int index = Mathematics.PositionToIndex(Center.X);
                if (index > -1 && index < refToLevel.LevelTiles.Count)
                {

                    refToLevel.Controller.OnTorpedoHitGroundOrWater(refToLevel.LevelTiles[index], this, boundRectangle.Center.X, boundRectangle.Center.Y);
                    if (dropHeight > maxDropHeight || refToLevel.LevelTiles[index] is ShipTile || refToLevel.LevelTiles[index] is ShipBunkerTile)
                    {
                        // torpedo damaged
                        SinkTorpedo(TorpedoFailure.TooHigh);
                    }
                }

            }

        }

        public void SinkTorpedo(TorpedoFailure? torpedoFailure)
        {
            int index = Mathematics.PositionToIndex(Center.X);
            if (index > -1 && index < refToLevel.LevelTiles.Count)
            {
                if (this.state == MissileState.Intact)
                {
                   
                    refToLevel.Controller.OnTorpedoSunk(refToLevel.LevelTiles[index], this, torpedoFailure);
                    Destroy();
                }
                
                return;
            }
        }

        /// <summary>
        /// Zmienia pozycje torpedy w wodzie.
        /// </summary>
        /// <param name="time">Czas od ostatniego przesuniecia.</param>
        /// <author>Michal Ziober</author>
        private void ChangePositionInWater(int time)
        {
            bool damaged = false;
            // torpeda upuszczona pod za dużym kątem
            if (dropAngle > Plane.MaxFireTorpedoAngle / 2.0f || dropAngle < -Plane.MaxFireTorpedoAngle / 2.0f)
            {
                // i za daleko przepłynęła
                if (waterTravelDistance > maxWaterTravelDistance / 2.0f)
                {
                    damaged = true;
                }
            }

            if (waterTravelDistance > maxWaterTravelDistance)
            {
                damaged = true;
            }


            if (damaged)
            {
                SinkTorpedo(TorpedoFailure.LongTravelDistance);
            }

            //zaleznosc od uplynietego czasu
            float coefficient = Mathematics.GetMoveFactor(time);

            //wyliczam o ile przesunac torpede.
            PointD vector = new PointD(moveVector.X * coefficient, 0);

            if (Math.Abs(vector.X) < GameConsts.Torpedo.BaseSpeed * 1.5f)
            {
                vector.X += moveVector.X * 0.5f * time / 1000.0f;
            }
            waterTravelDistance += Math.Abs(vector.X);

            //przesuwam prostokat.
            base.boundRectangle.Move(vector);
        }



        /// <summary>
        /// Sprawdza kolizje torpedy z samolotem wroga. 
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithPlanes()
        {
            if (base.refToLevel.EnemyPlanes != null && base.refToLevel.EnemyPlanes.Count > 0)
            {
                foreach (EnemyPlaneBase ep in base.refToLevel.EnemyPlanes)
                {
                    //sprawdzam czy aby nie ma zderzenia.
                    if (boundRectangle.Intersects(ep.Bounds))
                    {
                        //niszcze samolot przeciwnika.
                        ep.Destroy();

                        //wysylam sygnal do kontrollera
                        base.refToLevel.Controller.OnEnemyPlaneBombed(ep, this);

                        //niszcze bombe.
                        base.state = MissileState.Destroyed;
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza kolizje z terenem, bunkrami , barakami, beczkami, etc.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithGroundAndShips()
        {
            if (Center.Y < MinYPosition && waterTravelDistance > 0)
            {
                int index = Mathematics.PositionToIndex(Center.X);
                if (index > -1 && index < refToLevel.LevelTiles.Count)
                {
                    //jesli nie ma kolizji z zadnym obiektem
                    if (refToLevel.LevelTiles[index].InCollision(this.boundRectangle) == CollisionType.None) return;

                    //jesli nie da sie zniszczyc dany obiekt torpeda.
                    if (this.IsInWater)
                    {
                        if (!CanBeDestroyed(index) && !(refToLevel.LevelTiles[index] is OceanTile))
                        {
                            refToLevel.Controller.OnTileBombed(refToLevel.LevelTiles[index], this);
                            Destroy();
                        }
                        else
                        {
                            LevelTile destroyTile = refToLevel.LevelTiles[index];
                            if (destroyTile is BeginShipTile || destroyTile is EndShipTile)
                            {
                            	if(waterTravelDistance < this.minWaterTravelDistance)
		                        {
		                            SinkTorpedo(null);
		                            return;
		                        }
                            	
                                ShipTile st = destroyTile as ShipTile;
                                if (!st.IsDestroyed && !st.IsSunkDown && st.Depth <= this.Bounds.Height)
                                {
                                    if (st.ShipOwner.IsLastHit)//jesli mozemy zatopic okret
                                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                                    st.ShipOwner.TorpedoHit(this, destroyTile as ShipTile);//trafiamy w okret torpeda
                                    refToLevel.Statistics.HitByTorpedo++;
                                    //refToLevel.KillVulnerableSoldiers(index, 6);
                                }
                                //niszcze bombe.
                                //Destroy();
                                state = MissileState.Destroyed;

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza pole o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks trafionego elementu.</param>
        /// <returns>Jesli obiekt da sie zniszczyc torpedą zwraca true, w przeciwnym przypadku false.</returns>
        private bool CanBeDestroyed(int index)
        {
            LevelTile tile = refToLevel.LevelTiles[index];
            return ((tile is BeginShipTile) || (tile is EndShipTile));
        }

        /// <summary>
        /// Funkcja sprawdza czy mozna odrejestrowac torpede. Jesli mozna
        /// odrejetrowuje ja.
        /// </summary>
        /// <returns>Jesli torpeda zostanie odrejestrowana, zwroci true,
        /// false w przeciwnym przupadku.</returns>
        /// <author>Adam Witczak</author>
        private bool Unregister()
        {
            if (!(ammunitionOwner is EnemyPlaneBase))
            {
                if ((System.Math.Abs(Center.X - refToLevel.UserPlane.Center.X) > MaxDistanceToPlane) ||
                    ((System.Math.Abs(Center.Y - refToLevel.UserPlane.Center.Y) > MaxHeightDistanceToPlane)
                     ))
                {
                    Destroy();
                   // refToLevel.Controller.OnUnregisterTorpedo(this);
                   // state = MissileState.Destroyed;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (System.Math.Abs(refToLevel.UserPlane.Center.X - Center.X) > MaxDistanceToPlane)
                {
                    Destroy();
                    //refToLevel.Controller.OnUnregisterTorpedo(this);
                   // state = MissileState.Destroyed;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion
    }
}
