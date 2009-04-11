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
using System.Collections.Generic;
using System.Diagnostics;

using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Math = Mogre.Math;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Klasa implementujaca zachownie rakiet na planszy.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class Rocket : Ammunition
    {
        #region Fields

        /// <summary>
        /// Pole widzenia.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const int ViewRange = 70;

        /// <summary>
        /// Szerokosc wrazliwego pola na trafienia.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float HitShift = 1.05f;

        /// <summary>
        /// Wspolczynnik zmiany pozycji.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public const int MoveInterval = 6000;

        /// <summary>
        /// Szerokosc prostokata opisujacego rakiete.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float RocketWidth = 0.2f;

        /// <summary>
        /// Wysokosc prostokata opisujacego rakiete.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float RocketHeight = 1.62f;

        /// <summary>
        /// Czas opadania w milisekundach.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const int dropTime = 200;

        /// <summary>
        /// Predkosc spradania.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const int DropSpeed = -70;

        /// <summary>
        /// Maksymalny dopuszczalny dystans pomiedzy samolotem rodzicem a rakieta.
        /// Jesli dystans bedzie wiekszy rakieta bedzie odrejestrowana.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const int MaxDistanceToPlane = 600;

        /// <summary>
        /// Maksymalny dopuszczalny pionowy dystans 
        /// pomiedzy rakieta a samolotem rodzicem.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const int MaxHeightDistanceToPlane = 200;

        /// <summary>
        /// Przesuniecie pionowe rakiety wzgledem samolotu
        /// przy starcie.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float HeightShift = 0.9f;

        /// <summary>
        /// Przesuniecie poziome rakiety wzgledem samolotu
        /// przy starcie.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float WidthShift = 0.9f;

        /// <summary>
        /// Wspolrzedna y, po przekroczeniu ktorej sprawdzana jest kolizja z lotniskowcem. 
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const float MinYPositionForAircraft = 9;

        /// <summary>
        /// Licznik czasu.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private int timeCounter;

        /// <summary>
        /// Wektor ruchu z napedem silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        private PointD flyVector;

        /// <summary>
        /// Pocz¹tkowy wektor ruchu z napedem silnika.
        /// </summary>
        /// <author>Adam Witczak</author>
        private PointD initPlaneSpeed;
        

        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor szescioparametrowy. Tworzy 
        /// nowa rakiete na planszy.
        /// </summary>
        /// <param name="x">Wspolrzedna x.</param>
        /// <param name="y">Wspolrzedna y.</param>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <param name="angle">Kat nachylenia.</param>
        /// <param name="owner">Wlasciciel amunicji.</param>
        /// <author>Michal Ziober</author>
        public Rocket(float x, float y, PointD planeSpeed, Level level, float angle, Plane owner)
            : base(planeSpeed, level, angle, owner)
        {
            timeCounter = 0;

            //prostokat opisujacy obiekt.
            if (planeSpeed.X >= 0)
                boundRectangle = new Quadrangle(new PointD(x - WidthShift, y - HeightShift), RocketWidth, RocketHeight);
            else
                boundRectangle = new Quadrangle(new PointD(x + WidthShift, y - HeightShift), RocketWidth, RocketHeight);

            float yDropSpeed;

            yDropSpeed = owner.Bounds.IsObverse ? -DropSpeed : DropSpeed;


            //kierunek ruchu podczas lotu z silnikiem.
            float speedX = planeSpeed.X >= 0 ? GameConsts.Rocket.BaseSpeed : -GameConsts.Rocket.BaseSpeed;

            //wektor ruchu podczas spadania.
            moveVector = new PointD(planeSpeed.X, yDropSpeed);
            
            //weektor ruchu podczas pracy silnika.
            if (planeSpeed.X >= 0)
            {
                flyVector = new PointD(planeSpeed.X * 0.7f * GameConsts.Rocket.BaseSpeed, planeSpeed.Y * 0.7f * GameConsts.Rocket.BaseSpeed);
            } else
            {
                flyVector = new PointD(planeSpeed.X * 0.7f * GameConsts.Rocket.BaseSpeed, planeSpeed.Y * 0.7f * GameConsts.Rocket.BaseSpeed);
            }

            initPlaneSpeed = planeSpeed;
            
           
           
        }

        /// <summary>
        /// Konstruktor piecioparametrowy.Tworzy
        /// nowa rakiete na planszy.
        /// </summary>
        /// <param name="position">Pozycja rakiety.</param>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <param name="angle">Kat nachylenia.</param>
        /// <param name="owner">Wlasciciel rakiety.</param>
        /// <author>Michal Ziober</author>
        public Rocket(PointD position, PointD planeSpeed, Level level, float angle, Plane owner)
            : this(position.X, position.Y, planeSpeed, level, angle, owner)
        {
        }

        #endregion

        #region IMove Members

        /// <summary>
        /// Zmienia pozycje rakiety w zaleznosci od czasu oraz
        /// sprawdza kolizje z obiektami na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniej zmieny</param>
        /// <author>Michal Ziober</author>
        public override void Move(int time)
        {
            //zmienia pozycje.
            ChangePosition(time);

            //jesli nie zostala odrejstrowana.
            if (!Unregister())
            {
                //jesli jest to rakieta samolotu gracza.
                if (!(ammunitionOwner is EnemyPlane))
                    CheckCollisionWithPlanes(); //sprawdzam zderzenie z wrogim samolotem.
                else
                {
                    //kolizje z samolotami na lotniskowcu
                    if (Position.Y < MinYPositionForAircraft)
                        CheckCollisionWithStoragePlane();

                    //sprawdzam kolizje z samolotem gracza.
                    CheckCollisionWithUserPlane();
                }

                //obsluga zderzenia z ziemia.
                CheckCollisionWithGround();

                //sprawdzam kolizje z lotniskowce.
                CheckCollisionWithCarrier(this);
            }
        }

        /// <summary>
        /// Sprawdzam kolizje z samolotami na lotniskowcu.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithStoragePlane()
        {
            List<StoragePlane> storageToRemove = new List<StoragePlane>();

            if (refToLevel.StoragePlanes != null && refToLevel.StoragePlanes.Count > 0)
            {
                foreach (StoragePlane storagePlane in refToLevel.StoragePlanes)
                    if (boundRectangle.Intersects(storagePlane.Bounds))
                    {
                        //niszczy samolot na lotniskowcu.
                        storagePlane.Destroy();

                        //zmniejsza liczbe zyc.
                        refToLevel.SubtractionLive();

                        //odrejestruje samolot na lotniskowcu.
                        refToLevel.Controller.OnUnregisterPlane(storagePlane);

                        //niszcze rakiete.
                        state = MissileState.Destroyed;

                        //odrejestruje rakiete
                        refToLevel.Controller.OnUnregisterRocket(this);

                        storageToRemove.Add(storagePlane);
                        break;
                    }

                if (storageToRemove.Count > 0)
                {
                    foreach (StoragePlane sp in storageToRemove)
                    {
                        refToLevel.StoragePlanes.Remove(sp);
                    }
                    storageToRemove.Clear();
                }
            }
        }


        /// <summary> 
        /// Zmienia pozycje rakiety.
        /// </summary>
        /// <param name="time">Czas od ostatniego przesuniecia.</param>
        /// <author>Michal Ziober</author>
        private void ChangePosition(int time)
        {
            float coefficient = Mathematics.GetMoveFactor(time, MoveInterval);
            timeCounter += time;
            if (timeCounter <= dropTime) //swobodne spadanie
            {
                PointD vector = new PointD(moveVector.X * coefficient * 6, moveVector.Y * coefficient);
                boundRectangle.Move(vector);
            }
            else //naped silnikowy
            {
               // Console.WriteLine(flyVector.X);

                // rakieta wytraca prêdkoœæ uzyskan¹ od samolotu
                if (Math.Abs(flyVector.X) > Math.Abs(Plane.MinFlyingSpeed * GameConsts.Rocket.BaseSpeed))
                {
                    flyVector.X *= 0.995f;
                }

                if (Math.Abs(flyVector.Y) > Math.Abs(Plane.MinFlyingSpeed * GameConsts.Rocket.BaseSpeed))
                {
                    flyVector.Y *= 0.995f;
                }

                PointD vector = new PointD(flyVector.X * coefficient, flyVector.Y * coefficient);
                boundRectangle.Move(vector);
            }
        }

        /// <summary> 
        /// Sprawdza kolizje z wrogimi samolotami 
        /// oraz obsluguje zderzania z nimi.
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
                        //niszczy wrogi samolot
                        ep.Destroy();

                        //wyslam sygnal do controllera aby usunal samolot z widoku.
                        refToLevel.Controller.OnEnemyPlaneBombed(ep, this);

                        //zwiekszam liczbe trafionych obiektow przez rakiete
                        refToLevel.Statistics.HitByRocket++;

                        //niszcze rakiete.
                        state = MissileState.Destroyed;
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza kolizje z samolotem gracza.
        /// </summary>
        /// <remarks>Tylko dla rakiety wroga.</remarks>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithUserPlane()
        {
            if (refToLevel.UserPlane != null)
            {
                if (boundRectangle.Intersects(refToLevel.UserPlane.Bounds))
                {
                    //odrejestruje samolot gracza.
                    refToLevel.Controller.OnPlaneDestroyed(refToLevel.UserPlane);
                    refToLevel.UserPlane.Destroy();

                    //odrejestruje rakiete.
                    refToLevel.Controller.OnUnregisterRocket(this);

                    //niszcze rakiete.
                    state = MissileState.Destroyed;
                }
            }
        }

        /// <summary>
        /// Sprawdza kolizje z podlozem.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void CheckCollisionWithGround()
        {
            int index = Mathematics.PositionToIndex(Position.X);
            LevelTile tile;
            if (index > -1 && index < refToLevel.LevelTiles.Count)
            {
            	tile = refToLevel.LevelTiles[index];
            	if (tile is ShipTile)
            	{
            		//boundRectangle.Intersects(
            	}
            	//CollisionType c = CollisionType.None;
            	CollisionType c = tile.InCollision(this.boundRectangle);
            	if (c == CollisionType.None) return;
            	    
                //jesli nie da sie zniszczyc dany obiekt rakieta.
                if(c == CollisionType.Hitbound || c == CollisionType.CollisionRectagle)
                {
                    if (tile is BarrelTile)
	                {
	                    BarrelTile destroyTile = tile as BarrelTile;
	                    if (!destroyTile.IsDestroyed)
	                    {
	                        destroyTile.Destroy();
	                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                            refToLevel.Statistics.HitByRocket += refToLevel.KillVulnerableSoldiers(index, 2, true);
	                    }
                        else
                            refToLevel.Controller.OnTileBombed(tile, this);
	                }
	                else if (tile is EnemyInstallationTile)
	                {
                        FortressBunkerTile fortressTile = null;
	                    EnemyInstallationTile enemyTile = null;
	                    LevelTile destroyTile = tile;
                            //Obsluga fortress bunker
                            if ((fortressTile = destroyTile as FortressBunkerTile) != null && !fortressTile.IsDestroyed)
                            {
                                //Trafienie zniszczonego fortress bunker
                                if (fortressTile.IsDestroyed)
                                {
                                    refToLevel.Controller.OnTileBombed(destroyTile, this);
                                }
                                else
                                {
                                    fortressTile.Hit();
                                    //Ostatnie trafienie!
                                    if (fortressTile.ShouldBeDestroyed)
                                    {
                                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                                        refToLevel.Statistics.HitByRocket++;
                                        fortressTile.Destroy();
                                    }
                                    //Trafienie rakiety uszkadzaj¹ce fortress bunker
                                    else
                                    {
                                        refToLevel.Controller.OnTileDamaged(destroyTile, this);
                                        refToLevel.Statistics.HitByRocket++;
                                    }
                                }
                            }
                            else if ((enemyTile = destroyTile as EnemyInstallationTile) != null && !enemyTile.IsDestroyed)
                            {
                                refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                                refToLevel.Statistics.HitByRocket++;
                                enemyTile.Destroy();
                            }
                            else
                                refToLevel.Controller.OnTileBombed(tile, this);
	                }
                    else
                        refToLevel.Controller.OnTileBombed(tile, this);                	
                } 
                else if(c == CollisionType.Altitude) 
                {
                	refToLevel.Controller.OnTileBombed(tile, this);
                }
              

                //zabija zolnierzy, ktorzy sa w zasiegu.
                refToLevel.Statistics.HitByRocket += refToLevel.KillVulnerableSoldiers(index, 1, true);

                //niszcze bombe
                state = MissileState.Destroyed;
            }
        }

        /// <summary>
        /// Sprawdza jak oddzialywuje rakieta na pole o zadanym indeksie.
        /// </summary>
        /// <param name="index">Indeks pola z ktorym zderzyla sie bomba.</param>
        /// <returns>Jesli dany obiekt da sie zniszczyc za pomoca rakiety zwroci true,
        /// false w przeciwnym przypadku.</returns>
        /// <author>Michal Ziober</author>
        /*private bool CanBeDestroyed(int index)
        {
            return !(refToLevel.LevelTiles[index] is EnemyInstallationTile) && !(refToLevel.LevelTiles[index] is BarrelTile);
        }*/

        /// <summary>
        /// Funkcja sprawdza czy mozna odrejestrowac rakiete. Jesli mozna
        /// odrejetrowuje ja.
        /// </summary>
        /// <returns>Jesli rakieta zostanie odrejestrowana, zwroci true,
        /// false w przeciwnym przupadku.</returns>
        /// <author>Michal Ziober</author>
        private bool Unregister()
        {
            if (!(ammunitionOwner is EnemyPlane))
            {
                if ((System.Math.Abs(Center.X - refToLevel.UserPlane.Center.X) > MaxDistanceToPlane) ||
                    ((System.Math.Abs(Center.Y - refToLevel.UserPlane.Center.Y) > MaxHeightDistanceToPlane) &&
                     flyVector.Y > 0))
                {
                    refToLevel.Controller.OnUnregisterRocket(this);
                    state = MissileState.Destroyed;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (System.Math.Abs(refToLevel.UserPlane.Center.X - Center.X) > MaxDistanceToPlane)
                {
                    refToLevel.Controller.OnUnregisterRocket(this);
                    state = MissileState.Destroyed;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion

        #region Static Method

        /// <summary>
        /// Funkcja sprawdza czy samolot bedzie mogl trafic rakieta w inny obiekt.
        /// </summary>
        /// <param name="plane">Samolot strzelajacy.</param>
        /// <param name="enemyPlane">Samolot, ktory chemy trafic.</param>
        /// <returns>Zwraca true jesli moze trafic wrogi samolot; false - w przeciwnym
        /// przypadku.</returns> 
        /// <author>Michal Ziober</author>
        public static bool CanHitEnemyPlane(Plane plane, Plane enemyPlane)
        {
            if (plane.Direction == Direction.Right && plane.Center.X > enemyPlane.Center.X)
                return false;

            if (plane.Direction == Direction.Left && plane.Center.X < enemyPlane.Center.X)
                return false;

            if (System.Math.Abs(plane.Center.X - enemyPlane.Center.X) < 10 &&
                System.Math.Abs(plane.Center.Y - enemyPlane.Center.Y) < 10)
                return false;

            Quadrangle planeQuad = new Quadrangle(plane.Bounds.Peaks);
            planeQuad.Move(0, -HeightShift);
            Line lineA = new Line(planeQuad.Peaks[1], planeQuad.Peaks[2]);
            for (int i = 0; i < enemyPlane.Bounds.Peaks.Count - 1; i++)
            {
                Line lineB = new Line(enemyPlane.Bounds.Peaks[i], enemyPlane.Bounds.Peaks[i + 1]);
                PointD cut = lineA.Intersect(lineB);
                if (cut == null)
                    continue;

                if (InEnemyRange(cut, planeQuad.Center, enemyPlane.Center))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Sprawdza czy punkt przeciecia jest w zasiegu samolotu.
        /// </summary>
        /// <param name="cut">Punkt przeciecia dwoch prostych.</param>
        /// <param name="plane">Pozycja samolotu.</param>
        /// <returns>true jesli punkt przeciecia jest w zasiegu, false
        /// w przeciwnym przypadku.</returns>
        /// <author>Michal Ziober</author>
        private static bool InEnemyRange(PointD cut, PointD plane, PointD enemyPlane)
        {
            return
                System.Math.Abs(cut.X - plane.X) < ViewRange &&
                ((cut.Y > enemyPlane.Y - HitShift) && (cut.Y < enemyPlane.Y + HitShift));
        }

        #endregion
    }
}