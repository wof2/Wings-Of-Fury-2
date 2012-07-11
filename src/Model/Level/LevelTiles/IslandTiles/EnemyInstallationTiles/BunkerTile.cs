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
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.Weapon;
using Math=Mogre.Math;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{
    /// <summary>
    /// Klasa abstarkcyjna dla bunkrow.
    /// </summary>
    /// <author>Michal Ziober</author>
    public abstract class BunkerTile : EnemyInstallationTile, IObject2D
    {
        #region Const

        /// <summary>
        /// Minimalny kat nachylenia dzialka.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected float MinAngle = Math.PI / 4.0f;

        /// <summary>
        /// Maksymalny kat nachylenia dzialka.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected float MaxAngle = 3 * Math.PI / 4.0f;

        /// <summary>
        /// Dziewiedziesiat stopni w radianach.
        /// </summary>
        private const float NinetyDegree = Math.PI / 2.0f;

        /// <summary>
        /// Przesuniecie reflektora w jednym kroku.
        /// </summary>
        private const float ReflectorMoveStep = Math.PI / 180.0f;

        /// <summary>
        /// Dopuszczalna roznica pomiedzy katem reflectora a katem dzialka.
        /// </summary>
        private const float EpsilonAngle = Math.PI / 90.0f;

        /// <summary>
        /// Minimalny czas, ktory jest potrzebny na odbudowe bunkru.
        /// Podany w milisekundach.
        /// </summary>
        private const int RecoveryTime = 10000;

        #endregion

        #region Protected & Private Fields


        private const float valleyFireDistance = 150;

        /// <summary>
        /// Czas, ktory uplynal od ostatniego strzalu.
        /// </summary>
        protected float currentTime;

        /// <summary>
        /// Obszar widoczny dla dzialka.
        /// </summary>
        protected Quadrangle horizon;

        /// <summary>
        /// Kat nachylenia dzialka od podloza.
        /// </summary>
        protected float angle;

        /// <summary>
        /// Kat nachylenia reflektora wzgledem podloza.
        /// </summary>
        protected float lightAngle;

        /// <summary>
        /// Generator liczb pseudolosowych.
        /// </summary>
        protected System.Random mRand;

        /// <summary>
        /// Kierunek ruchu reflektora.
        /// </summary>
        private Direction lightDirection = Direction.Right;

        /// <summary>
        /// Zmienna przetrzymuje czas zniszczenia bunkra.
        /// </summary>
        private int destroyTime;


        private bool hasRockets;


        private float preparingToVolleyTime = 0;
        private float volleyPrepareDelay = 4000;

        private int volleyCount = 6;
        private int maxVolleyCount = 6;

        private float preparingToNextMissileTime = 0;

        /// <summary>
        /// Kat o ktory obraca sie pocisk wystrzelony z bazooki w ciagu sekundy w osi Z
        /// </summary>
        protected float bazookaRotationPerSecond = (float)System.Math.PI * 0.35f;




        /// <summary>
        /// Wielokrotnosc nominalnej prêdkoœci rakiety samolotowej
        /// </summary>
        protected const float RocketSpeedMultiplier = 0.4f;

        /// <summary>
        /// Wielokrotnosc nominalnej prêdkoœci rakiety samolotowej
        /// </summary>
        protected const float RocketDistanceMultiplier = 0.6f;


        /// <summary>
        /// Obiekt zarzadzajacy bronia 
        /// </summary>
        /// <author></author>
        protected WeaponManager weaponManager;

       


        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor szescioparametrowy. Tworzy 
        /// nowy bunkier na planszy.
        /// </summary>
        /// <param name="yBegin">Poczatek bunkru.</param>
        /// <param name="yEnd">Koniec bunkru.</param>
        /// <param name="hitBound">Prostokat z ktorym beda sprawdzane kolizje z bronia.</param>
        /// <param name="soldierNum">Liczba zolnierzy.</param>
        /// <param name="type">Typ bunkru.</param>
        /// <param name="collisionRectangle">Lista prostokatow z ktorymi moga wystapic zderzenia.</param>
        public BunkerTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int soldierNum, int generalNum, int type,
                          List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, soldierNum, generalNum, type, collisionRectangle)
        {
            angle = NinetyDegree;
            lightAngle = MinAngle;
            mRand = new System.Random(System.Environment.TickCount);
        }

        #endregion

        #region Abstarct & Virtual & Public Method

        /// <summary>
        /// Prowadzi ostrzal samolotu.
        /// </summary>
        /// <author>Michal Ziober</author>
        public virtual void Fire(int time)
        {
            if (!IsDestroyed && UserPlaneNotYetDestroyed)
            {
                if (HasRockets)
                {
                    if (ShouldFire && CanFire)
                    {

                        // rakiety gotowe. zaczynamy liczyc czas
                        if (volleyCount == maxVolleyCount)
                        {
                            preparingToVolleyTime += time;
                        }

                        if (preparingToVolleyTime >= volleyPrepareDelay)
                        {

                            if (volleyCount > 0)
                            {
                                preparingToNextMissileTime += time;
                                if (preparingToNextMissileTime > volleyPrepareDelay*0.1f)
                                {
                                    // strzal

                                    PointD dir = refToLevel.UserPlane.Position - Center;
                                    dir.Normalise();

                                    bool inView = (dir.Angle > Math.PI*0.5f - Math.PI*0.3f &&
                                                   dir.Angle < Math.PI*0.5f + Math.PI*0.3f);
                                    if (inView)
                                    {

                                        float dist = refToLevel.UserPlane.DistanceToClosestPlane();

                                        if (dist > valleyFireDistance / 2.0)
                                        {
                                            // strzelaj tak zeby nie rozwalic samolotow japonskich

                                            PointD moveVector = GameConsts.Rocket.BaseSpeed*RocketSpeedMultiplier*dir;
                                            dir.X *= (moveVector.X >= 0) ? 1.0f : -1.0f;
                                            float relative = dir.Angle +
                                                             Math.RangeRandom(-0.1f*Math.PI,
                                                                              0.1f*Math.PI);

                                            Rocket rocket = Weapon.RocketFire(relative, moveVector,
                                                                              bazookaRotationPerSecond);
                                            rocket.MaxDistanceToOwner *= RocketDistanceMultiplier*
                                                                         Mathematics.RangeRandom(0.8f, 1.2f);
                                            rocket.MaxHeightDistanceToOwner *= RocketDistanceMultiplier*
                                                                               Mathematics.RangeRandom(0.8f, 1.2f);

                                            volleyCount--;
                                            preparingToNextMissileTime = 0;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                volleyCount = maxVolleyCount;
                                preparingToVolleyTime = 0;
                            }

                        }


                    }

                }
            }

        }

        /// <summary>
        /// Odbudowuje bunkier.
        /// </summary>
        /// <author>Michal Ziober</author>
        public virtual void Reconstruct()
        {
            enemyState = EnemyInstallationState.Intact;
            angle = NinetyDegree;
            lightAngle = MinAngle;
            
            EnemyInstallationTile.IncrementIntactInstallationCount();
        }

        /// <summary>
        /// Funkcja niszczy instalacje obronna.
        /// </summary> 
        /// <author>Michal Ziober</author>
        public override void Destroy()
        {
            angle = 0;
            lightAngle = MinAngle;
            this.destroyTime = System.Environment.TickCount;
            base.Destroy();
        }

        /// <summary>
        /// Dodaje jednego zolnierza do bunkra.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void AddSoldier()
        {
            soldiersCount++;
        }


        /// <summary>
        /// Dodaje jednego zolnierza do bunkra.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void AddGeneral()
        {
            generalsCount++;
        }


        #endregion

        #region Protected & Private Methods

        /// <summary>
        /// Przesuniecie dzia³ka w widoku od poczatku tile'a bunkra
        /// </summary>
        protected virtual float GetGunXShift()
        {
            return viewXShift + (Direction == Direction.Right ? 1 : -1) * 6;
        }

        /// <summary>
        /// Ustawia kat dzialka.    
        /// </summary>
        /// <author>Michal Ziober</author>
        protected virtual void SetAngle()
        {
        	
            float interval = refToLevel.UserPlane.Center.X;
			if(horizon!= null)
        	{
				interval -= horizon.Center.X + GetGunXShift();        		
        	}
        	
            //pionowo nad ziemia
            if (interval == 0)
                angle = NinetyDegree;

            float evaluateAngle = 0;
            if (interval < 0)
                evaluateAngle = Math.PI - Math.ATan(refToLevel.UserPlane.Center.Y / Math.Abs(interval)).ValueRadians;
            else
                evaluateAngle = Math.ATan(refToLevel.UserPlane.Center.Y / interval).ValueRadians;

            //Ustawiam kat.
            angle = System.Math.Min(MaxAngle, System.Math.Max(evaluateAngle, MinAngle));

          //  Console.WriteLine("Kat:" +angle+ " "+(angle / (Math.PI)));
            //Ustawiam kat reflectora
            SetReflectorAngle();
        }

        /// <summary>
        /// Ustawia kat reflektora.
        /// </summary>
        private void SetReflectorAngle()
        {
            //ustawienie kata.
            if (System.Math.Abs(lightAngle - angle) < EpsilonAngle)
            {
                if (lightAngle == MinAngle)
                {
                    lightAngle += ReflectorMoveStep;
                    lightDirection = Direction.Right;
                }
                else if (lightAngle == MaxAngle)
                {
                    lightAngle -= ReflectorMoveStep;
                    lightDirection = Direction.Left;
                }
                else
                {
                    lightAngle = angle;
                    return;
                }
            }
            else if (lightDirection == Direction.Right)
            {
                lightAngle += ReflectorMoveStep;
                lightAngle = System.Math.Min(lightAngle, MaxAngle);
                if (lightAngle == MaxAngle)
                    lightDirection = Direction.Left;
            }
            else if (lightDirection == Direction.Left)
            {
                lightAngle -= ReflectorMoveStep;
                lightAngle = System.Math.Max(MinAngle, lightAngle);
                if (lightAngle == MinAngle)
                    lightDirection = Direction.Right;
            }

            //jesli roznica pomiedzy katami jest dopuszczalna.
            if (System.Math.Abs(lightAngle - angle) < EpsilonAngle)
                if (mRand.Next(0, 1) == 0)//prawdopodobienstwo - narazie stale
                    lightAngle = angle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca kat nachylenia dzialka.
        /// 90 stopni(w radianach) - dzialko jest ustawione pionowo do gory.
        /// </summary>
        public float Angle
        {
            get { return angle; }
        }

        /// <summary>
        /// Zwraca kat nachylenia reflectora.
        /// </summary>
        public float ReflectorAngle
        {
            get { return lightAngle; }
        }

        /// <summary>
        /// Zwraca informacje czy dzialko jest oswietlone.
        /// Jesli kat dzialka i reflektora sa takie same to zwroci true.
        /// </summary>
        public bool IsIlluminatedShot
        {
            get { return System.Math.Abs(angle - lightAngle) < Mathematics.Epsilon; }
        }

        /// <summary>
        /// Sprawdza czy samolot nie zostal jeszcze zniszczony.
        /// </summary>
        protected bool UserPlaneNotYetDestroyed
        {
            get
            {
                return ((refToLevel.UserPlane.PlaneState != PlaneState.Crashed) &&
                        (refToLevel.UserPlane.PlaneState != PlaneState.Destroyed));
            }
        }

        /// <summary>
        /// Zwraca informacje, czy w³¹czyæ reflektor czy tez nie.
        /// Jesli jest noc to zwrocone bedzie true.
        /// </summary>
        public bool TurnReflector
        {
            get { return this.refToLevel != null && this.refToLevel.DayTime == DayTime.Night; }
        }

        /// <summary>
        /// Zwraca informacje o tym czy bunkier moze zostac odbudowany.
        /// </summary>
        public bool CanReconstruct
        {
            get { return System.Environment.TickCount - RecoveryTime > destroyTime; }
        }

        public bool CanAddSoldier
        {
            get { return (soldiersCount + generalsCount) < maxInfantryCount; }
        }

      

        public virtual PointD Center
        {
            get
            {
                return Bounds.Center + new PointD(viewXShift + 4.0f, maxY);

            }
        }

        public Quadrangle Bounds
        {
            get
            {
                return new Quadrangle(new PointD(Mathematics.IndexToPosition(this.TileIndex), (yBegin + yEnd) / 2.0f), LevelTile.TileWidth, this.MaxY); ;
            }
        }

        public virtual Direction Direction
        {
            get { return Model.Level.Direction.Right; }
        }

        public float RelativeAngle
        {
            get
            {
                return this.angle;

            }
        }

        public PointD MovementVector
        {
            get
            {
                return new PointD(0, 0);
            }
        }

        public bool IsEnemy
        {
            get
            {
                return true;
            }
        }


        public bool HasRockets
        {
            get { return hasRockets; }
            set { hasRockets = value; }
        }

        public override Level LevelProperties
        {
            set
            {
                base.LevelProperties = value;
                weaponManager = new WeaponManager(this.refToLevel, this, maxVolleyCount * 5, 0, 0);
                weaponManager.RegisterWeaponToModelEvent += refToLevel.rocket_RegisterWeaponEvent;
                weaponManager.SelectWeapon = WeaponType.Rocket;
            }
            get
            {
                return base.LevelProperties;
            }
        }


        public WeaponManager Weapon
        {
            get { return weaponManager; }
        }

        protected bool ShouldFire
        {
            get
            {
                PointD diff = refToLevel.UserPlane.Position - new PointD(Mathematics.IndexToPosition(this.TileIndex), this.MaxY);
                return diff.EuclidesLength < valleyFireDistance;  // zolnierz musi zbyc zwrocony w strone samolotu

            }

        }

        protected bool CanFire
        {
            get
            {
                return !IsDestroyed && Weapon.RocketCount > 0;

            }
        }



        #endregion
    	
		public float AbsoluteAngle {
			get {
				 float realAngle = Bounds.IsObverse
                            ? -RelativeAngle
                            : RelativeAngle;
				 return realAngle;
			}
		}
    }
}