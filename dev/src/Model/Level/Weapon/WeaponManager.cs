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
using System.Text;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Troops;
using LevelRef = Wof.Model.Level.Level;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Delegat dodaje nowy pocisk(Bomba, Rakieta), do modelu.
    /// </summary>
    /// <param name="ammunition">Amunicja.</param>
    /// <author>Michal Ziober</author>
    public delegate void RegisterWeaponToModel(Ammunition ammunition);

    /// <summary>
    /// Klasa zarzadzajaca bronia.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class WeaponManager
    {
        #region Private Const

        /// <summary>
        /// Maksymalna liczba dostepnych rakiet.
        /// </summary>
        /// <author>Michal Ziober</author>
        public readonly int maxRockets = 15;

        /// <summary>
        /// Maksymalna liczba dostepnych torped.
        /// </summary>
        /// <author>Adam Witczak</author>
        public readonly int MaxTorpedoes = 3;


        

        /// <summary>
        /// Maksymalna liczba dostepnych bomb.
        /// </summary>
        /// <author>Michal Ziober</author>
        public readonly int maxBombs = 30;

        /// <summary>
        /// Minimalny dystans pomiedzy dwoma samolotami,
        /// aby jeden trafil w drugi.
        /// </summary>
        private const int DistanceBetweenPlanes = 65;

        #endregion

        #region Events

        /// <summary>
        /// Rejestruje dany typ broni w modelu.
        /// </summary>
        /// <author>Michal Ziober</author>
        public event RegisterWeaponToModel RegisterWeaponToModelEvent;

        #endregion

        #region Fields

        /// <summary>
        /// Liczba aktualnie dostepnych rakiet. 
        /// </summary>
        private int rocketCount;

        /// <summary>
        /// Liczba aktualnie dostepnych bomb.
        /// </summary>
        private int bombCount;

        /// <summary>
        /// Liczba aktualnie dostepnych torped.
        /// </summary>
        private int torpedoCount;

        

        /// <summary>
        /// Aktualnie wybrana bron.
        /// </summary>
        private WeaponType actualWeapon;

        /// <summary>
        /// Bron wybrana na lotniskowcu(rakiety, bomby).
        /// </summary>
        private WeaponType selectWeapon = WeaponType.Gun;

        /// <summary>
        /// Referencja do obiektu klasy Level.
        /// </summary>
        private LevelRef refToLevel;

        /// <summary>
        /// Wlasciciel broni.
        /// </summary>
        private Plane ammunitionOwner;

        /// <summary>
        /// Licznik uplynietego czasu od ostatniego wystrzalu.
        /// </summary>
        private int lastFireTick;

        /// <summary>
        /// Dzialko samolotu.
        /// </summary>
        private Gun gun;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Publiczny konstruktor dwuparametrowy.
        /// </summary>
        /// <param name="refLevel">Referncja do planszy.</param>
        /// <param name="owner">Wlasciciel broni.</param>
        /// <param name="rocketCount">Maksymalna liczba rakiet.</param>
        /// <param name="bombCount">Maksymalna liczba bomb.</param>
        /// <param name="torpedoCount">Maksymalna liczba torped.</param>
        public WeaponManager(LevelRef refLevel, Plane owner, int rocketCount, int bombCount, int torpedoCount)
        {
            refToLevel = refLevel;
            lastFireTick = Environment.TickCount;
            maxBombs = bombCount;
            maxRockets = rocketCount;
            MaxTorpedoes = torpedoCount;
            this.bombCount = maxBombs;
            this.rocketCount = maxRockets;
            this.torpedoCount = MaxTorpedoes;
            actualWeapon = WeaponType.Gun;
            ammunitionOwner = owner;
            gun = new Gun(refLevel);
        }

        /// <summary>
        /// Publiczny konstruktor dwuparametrowy.
        /// </summary>
        /// <param name="refLevel">Referncja do planszy.</param>
        /// <param name="owner">Wlasciciel broni.</param>
        public WeaponManager(LevelRef refLevel, Plane owner)
            : this(refLevel,
                   owner,
                   owner is EnemyPlane ? GameConsts.EnemyPlane.RocketCount : GameConsts.UserPlane.RocketCount,
                   GameConsts.UserPlane.BombCount, GameConsts.UserPlane.TorpedoCount)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca aktualnie uzywany typ broni.
        /// </summary>
        public WeaponType ActualWeapon
        {
            get { return actualWeapon; }
        }

        /// <summary>
        /// Zwraca liczbe bomb do wykorzystania.
        /// </summary>
        public int BombCount
        {
            get { return bombCount; }
        }

        /// <summary>
        /// Zwraca liczbe rakiet do wykorzystania.
        /// </summary>
        public int RocketCount
        {
            get { return rocketCount; }
        }
        
        /// <summary>
        /// Zwraca liczbe torped do wykorzystania.
        /// </summary>
        public int TorpedoCount
        {
            get { return torpedoCount; }
        }


        
        /// <summary>
        /// Zwraca wartosc okreslajaca, czy zostaly jeszcze jakies 
        /// rakiety - true.
        /// </summary>
        public bool IsRocketAvailable
        {
            get { return (rocketCount > 0); }
        }

        /// <summary>
        /// Zwraca wartosc okreslajaca, czy zostaly jeszcze jakies
        /// bomby - true.
        /// </summary>
        public bool IsBombAvailable
        {
            get { return (bombCount > 0); }
        }


        /// <summary>
        /// Zwraca wartosc okreslajaca, czy zostaly jeszcze jakies
        /// torpedy - true.
        /// </summary>
        public bool IsTorpedoAvailable
        {
            get { return (torpedoCount > 0); }
        }


        /// <summary>
        /// Pobiera lub ustawia jaki rodzaj cierzkiej amunicji bedzie 
        /// stosowany. Dostepne wartosci to Rocket i Bomb.
        /// </summary>
        public WeaponType SelectWeapon
        {
            set
            {
                if (value == WeaponType.Gun)
                    throw new Exception("Typ ciezkiej amunicji nie moze byc Gun");
                selectWeapon = value;
            }
            get
            {
                if (selectWeapon == WeaponType.Gun)
                    throw new Exception("Typ ciezkiej amunicji nie zostal wybrany");
                return selectWeapon;
            }
        }

        /// <summary>
        /// Jesli mozna uzyc aktualnie wybranego typu broni
        /// zwroci true, w przeciwnym przypadku zwroci false.
        /// </summary>
        public bool IsCurentWeaponAvailable
        {
            get
            {
                if (actualWeapon == WeaponType.Gun)
                    return true;
                if (actualWeapon == WeaponType.Bomb)
                    return IsBombAvailable;
                if (actualWeapon == WeaponType.Rocket)
                    return IsRocketAvailable;
                if (actualWeapon == WeaponType.Torpedo)
                    return IsTorpedoAvailable;

                return false;
            }
        }

        #endregion

        #region Private Methods 

        /// <summary>
        /// Oddaje strzal z dzialka.
        /// </summary>
        private void GunFire(float angle)
        {
            if (ammunitionOwner is EnemyPlane)
                EnemyPlaneFire(angle);
            else
            {
                //zwieksza liczve wystrzelonych pociskow
                this.refToLevel.Statistics.GunCount++;

                UserPlaneFire(angle);
            }
        }

        /// <summary>
        /// Symuluje strzal z samolotu przeciwnika.
        /// </summary>
        /// <param name="angle"></param>
        private void EnemyPlaneFire(float angle)
        {
            refToLevel.Controller.OnFireGun(ammunitionOwner);

            if (Environment.TickCount - lastFireTick >= Gun.FireInterval)
            {
                if (refToLevel.UserPlane != null && ammunitionOwner != null)
                {
                    //sprawdzam czy wrogi samolot nie trafil w samolot gracza.
                    if ((Math.Abs(ammunitionOwner.Center.X - refToLevel.UserPlane.Center.X) < DistanceBetweenPlanes) &&
                        Gun.IsHitEnemyPlane(ammunitionOwner, refToLevel.UserPlane))
                    {
                        //ubytek paliwa.
                        refToLevel.UserPlane.Hit(true);

                        //komunikat do controllera.
                        refToLevel.Controller.OnGunHitPlane(refToLevel.UserPlane);
                    }
                }

                //ustawiam nowy czas
                lastFireTick = Environment.TickCount;
            }
        }

        /// <summary>
        /// Symuluje strzal z samolotu gracza.
        /// </summary>
        /// <param name="angle">Kat nachylenia samolotu.</param>
        private void UserPlaneFire(float angle)
        {
            //dzwiek strzalu.
            refToLevel.Controller.OnFireGun(refToLevel.UserPlane);

            if (Environment.TickCount - lastFireTick >= Gun.FireInterval)
            {
                if (refToLevel.UserPlane != null && refToLevel.EnemyPlanes.Count > 0)
                {
                    //sprawdzam czy samolot gracza nie trafi³ w wrogi samolot.
                    foreach (EnemyPlane ep in refToLevel.EnemyPlanes)
                    {
                        if ((Math.Abs(ep.Center.X - refToLevel.UserPlane.Center.X) < DistanceBetweenPlanes) &&
                            Gun.IsHitEnemyPlane(refToLevel.UserPlane, ep))
                        {
                            //ubytek paliwa.
                            ep.Hit(true);

                            //komunikat do controllera.
                            refToLevel.Controller.OnGunHitPlane(ep);

                            return;
                        }
                    }
                }

                //trafi w ziemie.
                GunHitsGround();

                //ustawiam nowy czas
                lastFireTick = Environment.TickCount;
            }
        }

        /// <summary>
        /// Obsluga trafienia w ziemie.
        /// </summary>
        private void GunHitsGround()
        {
            if (ammunitionOwner.RelativeAngle < 0)
            {
                PointD hitPoint = gun.GetHitPosition(ammunitionOwner.Bounds, ammunitionOwner.Center,
                                                     ammunitionOwner.Direction);
           
                if (hitPoint != null)
                {

                    int index = Mathematics.PositionToIndex(hitPoint.X);
                    if (index >= 0 && index < refToLevel.LevelTiles.Count)
                    {

                        refToLevel.Controller.OnGunHit(refToLevel.LevelTiles[index], hitPoint.X, Math.Max(hitPoint.Y, 1));

                        if (refToLevel.LevelTiles[index] is BarrelTile)
                        {
                            BarrelTile barrel = refToLevel.LevelTiles[index] as BarrelTile;
                            if (!barrel.IsDestroyed)
                            {
                                barrel.Destroy();
                                refToLevel.Controller.OnTileDestroyed(barrel, null);
                                this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 2);
                            }
                        }
                        else
                            this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Wystrzeliwuje rakiete.
        /// </summary>
        private void RocketFire()
        {
            Rocket rocket = null;
            PointD position = null;
            float realAngle = 0;

            //startowa pozycja rakiety
            position = new PointD(ammunitionOwner.Center.X, ammunitionOwner.Center.Y);

            //kat nachylenia w zaleznosci od ustawienia samolotu.
            realAngle = ammunitionOwner.Bounds.IsObverse
                            ? -ammunitionOwner.RelativeAngle
                            : ammunitionOwner.RelativeAngle;

            //nowa rakieta
            rocket = new Rocket(position, (PointD) ammunitionOwner.MovementVector.Clone(),
                                refToLevel, realAngle, ammunitionOwner);

            
            //zwieksza liczbe uzytych rakiet
            if (!this.ammunitionOwner.IsEnemy)
                this.refToLevel.Statistics.RocketCount++;

            rocketCount--;
            RegisterWeaponToModelEvent(rocket);
            refToLevel.Controller.OnRegisterRocket(rocket);

        }

        /// <summary>
        /// Wystrzeliwuje rakiete.
        /// </summary>
        private void TorpedoFire()
        {
            Torpedo torpedo = null;
            PointD position = null;
            float realAngle = 0;

            //startowa pozycja rakiety
            position = new PointD(ammunitionOwner.Center.X, ammunitionOwner.Center.Y);

            //kat nachylenia w zaleznosci od ustawienia samolotu.
            realAngle = ammunitionOwner.Bounds.IsObverse
                            ? -ammunitionOwner.RelativeAngle
                            : ammunitionOwner.RelativeAngle;

            //nowa rakieta
            torpedo = new Torpedo(position, (PointD)ammunitionOwner.MovementVector.Clone(),
                                refToLevel, 0, ammunitionOwner);


            //zwieksza liczbe uzytych rakiet
            if (!this.ammunitionOwner.IsEnemy)
                this.refToLevel.Statistics.TorpedoCount++;

            torpedoCount--;
            RegisterWeaponToModelEvent(torpedo);
            refToLevel.Controller.OnRegisterTorpedo(torpedo);

        }
        /// <summary>
        /// Zrzuca bombe.
        /// </summary>
        private void BombFire()
        {
            PointD position = new PointD(refToLevel.UserPlane.Center.X, refToLevel.UserPlane.Center.Y);
            Bomb bomb = new Bomb(position, (PointD) refToLevel.UserPlane.MovementVector.Clone(),
                                 refToLevel, refToLevel.UserPlane.RelativeAngle, ammunitionOwner);
            bombCount--;
            //zwieksza liczbe uzytych bomb
            if (!this.ammunitionOwner.IsEnemy)
                this.refToLevel.Statistics.BombCount++;

            RegisterWeaponToModelEvent(bomb);
            refToLevel.Controller.OnRegisterBomb(bomb);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Samolot otwiera ogien.
        /// </summary>
        /// <param name="planeAngle">Kat nachylenia samolotu.</param>
        public void Fire(float planeAngle, WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Bomb:
                    BombFire();
                    break;
                case WeaponType.Gun:
                    GunFire(planeAngle);
                    break;
                case WeaponType.Rocket:
                    RocketFire();
                    break;
                case WeaponType.Torpedo:
                    TorpedoFire();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Uzupelnia bomby samolotu.
        /// </summary>
        public void RestoreBombs()
        {
            bombCount = maxBombs;
            selectWeapon = WeaponType.Bomb;
        }

        /// <summary>
        /// Uzupelnia rakiety samolotu.
        /// </summary>
        public void RestoreRockets()
        {
            rocketCount = maxRockets;
            selectWeapon = WeaponType.Rocket;
        }


        /// <summary>
        /// Uzupelnia rakiety samolotu.
        /// </summary>
        public void RestoreTorpedoes()
        {
            torpedoCount = MaxTorpedoes;
            selectWeapon = WeaponType.Torpedo;
        }



        /// <summary>
        /// uzupe³nia aktualnie wybran¹ broñ ciê¿k¹.
        /// </summary>
        public void RestoreSelectedWeapon()
        {
            if (SelectWeapon == WeaponType.Bomb)
                RestoreBombs();
            if (SelectWeapon == WeaponType.Rocket)
                RestoreRockets();

            if (SelectWeapon == WeaponType.Torpedo)
                RestoreTorpedoes();
        }

        /// <summary>
        /// Wybiera poprzedni typ broni.
        /// </summary>
        /// <param name="allWeapons">Jesli true, to funkcja bedzie rozpatrywala
        /// wszystkie typy broni, jesli false to tylko Gun i jeden typ cierzkiej
        /// amunicji(Rocket, Bomb)</param>
        public void PreviousWeapon(bool allWeapons)
        {
            if (allWeapons)
            {
                switch (actualWeapon)
                {
                    case WeaponType.Bomb:
                        actualWeapon = WeaponType.Gun;
                        break;

                    case WeaponType.Rocket:
                        actualWeapon = WeaponType.Bomb;
                        break;

                    case WeaponType.Torpedo:
                        actualWeapon = WeaponType.Rocket;
                        break;

                    case WeaponType.Gun:
                        actualWeapon = WeaponType.Torpedo;
                        break;
                }


            }
            else
            {
                if (actualWeapon == WeaponType.Bomb || actualWeapon == WeaponType.Rocket || actualWeapon == WeaponType.Torpedo)
                    actualWeapon = WeaponType.Gun;
                else
                    actualWeapon = selectWeapon;
            }
        }

        /// <summary>
        /// Wybiera nastepny typ broni.
        /// </summary>
        /// <param name="allWeapons">Jesli true, to funkcja bedzie rozpatrywala
        /// wszystkie typy broni, jesli false to tylko Gun i jeden typ cierzkiej
        /// amunicji(Rocket, Bomb)</param>
        public void NextWeapon(bool allWeapons)
        {
            if (allWeapons)
            {
                switch (actualWeapon)
                {
                    case WeaponType.Gun:
                        actualWeapon = WeaponType.Bomb;
                        break;

                    case WeaponType.Bomb:
                        actualWeapon = WeaponType.Rocket;
                        break;

                    case WeaponType.Rocket:
                        actualWeapon = WeaponType.Torpedo;
                        break;

                    case WeaponType.Torpedo:
                        actualWeapon = WeaponType.Gun;
                        break;
                }


            }
            else
            {
                if (actualWeapon == WeaponType.Bomb || actualWeapon == WeaponType.Rocket || actualWeapon == WeaponType.Torpedo)
                    actualWeapon = WeaponType.Gun;
                else
                    actualWeapon = selectWeapon;
            }
        }

        /// <summary>
        /// Zwraca opis obiektu.
        /// </summary>
        /// <returns>Opis obiektu: liczba kazdej z dostepnych broni; aktualnie
        /// wybrana bron.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Bomb count: " + bombCount);
            builder.AppendLine("Rocket count: " + rocketCount);
            builder.AppendLine("Torpedo count: " + torpedoCount);
            builder.AppendLine("Curent weapon: " + actualWeapon);

            return builder.ToString();
        }

        #endregion
    }
}