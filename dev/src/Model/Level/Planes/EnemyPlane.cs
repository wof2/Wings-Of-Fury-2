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
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.LevelTiles.Watercraft;
namespace Wof.Model.Level.Planes
{
    public enum AttackObject
    {
        Carrier,
        UserPlane,
        None
    }

    internal class EnemyPlane : Plane
    {
        #region Constants


        /// <summary>
        /// pomocniczy licznik (czêstotliwoœæ okrzyków bojowych)
        /// </summary>
        private float warCryTimer;



        /// <summary>
        /// Okreœla czas po jakim mo¿e nast¹piæ ponowny okrzyk bojowy
        /// </summary>
        private const float warCryTimerMin = 40.0f;



        /// <summary>
        /// Maksymalna odlegloœæ od samolotu gracza, na któr¹ mo¿e siê oddaliæ.
        /// </summary>
        private const float distanceFromUserPlane = 10;

        /// <summary>
        /// Bezpieczna odleg³oœæ (na osi Y) od samolotu gracza.
        /// </summary>
        private const float safeUserPlaneHeightDiff = 15;

        /// <summary>
        /// Maksymalna odlegloœæ od samolotów na lotniskowcu, na któr¹ mo¿e siê oddaliæ.
        /// </summary>
        private const float distanceFromStoragePlanes = 30;

        /// <summary>
        /// Maksymalny k¹t jaki mo¿e mieæ samolot wroga.
        /// </summary>
        private const float maxAngle = maxTurningAngle - float.Epsilon;

        /// <summary>
        /// Okreœla na jak¹ wysokoœæ nad samolot gracza bêdzie siê wznosi³/opada³ samolot wroga.
        /// </summary>
        private const float userPlaneHeightDiff = 4;

        /// <summary>
        /// Wysokoœæ samolotu.
        /// </summary>
        private const float height = 2;

        /// <summary>
        /// Szerokoœæ samolotu.
        /// </summary>
        private const float width = 5.5f;

        #endregion

        #region Fields

        private PointD temp;

        /// <summary>
        /// Poprzednie po³o¿enia samolotu
        /// </summary>
        private PointD[] interpolateSet;

        /// <summary>
        /// Liczba punktów interpolacji
        /// </summary>      
        private int interpolationPoints = 5;

        private int lastInterpolationNull = -1;

        /// <summary>
        /// Czas jaki pozosta³, ¿eby móc wystrzeliæ kolejn¹ rakietê.
        /// </summary>
        private float timeToNextRocket = 0;

        /// <summary>
        /// K¹t o jaki bêdzie siê zmienia³o nachylenie samolotu w czasie timeUnit.
        /// (Wyra¿ony w radianach)
        /// </summary>
        private float rotateStep = GameConsts.EnemyPlane.EnemyRotateStep;

        private bool isAlarmDelivered;

        private float carrierDistance;

        private AttackObject attackObject;

        public AttackObject AttackObject
        {
            get { return attackObject; }
            set { attackObject = value; }
        }

        //komentarz do wywalenia

        #endregion

        #region Constructors

        public EnemyPlane(Level level, float width, float height)
            : base(level, width, height, true, null)
        {
            isEnemy = true;
            movementVector = new PointD((float) direction*GameConsts.EnemyPlane.Speed, 0);
            locationState = LocationState.Air;
            wheelsState = WheelsState.In;
            motorState = EngineState.Working;
            isAlarmDelivered = false;
            temp = new PointD(0, 0);

            interpolateSet = new PointD[interpolationPoints];
            for (int i = 0; i < interpolateSet.Length; i++)
            {
                interpolateSet[i] = null;
            }
        }

        /// <summary>
        /// Tworzy samolot z wylosawnym po³o¿eniem (któryœ z krañców planszy).
        /// </summary>
        /// <param name="level"></param>
        public EnemyPlane(Level level)
            : base(level, true)
        {
        	StartPositionInfo info = new StartPositionInfo();
        	
            //wylosowanie pozycji
            Random r = new Random();
            int atEnd = r.Next(0, 2); //losuje 0 albo 1
            float endPos = (level.LevelTiles.Count)*LevelTile.Width - 2.0f*width;

            float x;

            if (level.MissionType == MissionType.Dogfight && level.EnemyPlanesLeft == level.EnemyPlanesPoolCount)
            {
                // pierwszy samolot jest blizej lotniskowca
                x = atEnd * endPos * 0.6f + (1 - atEnd) * endPos * 0.4f;
            }
            else
            {
                x = atEnd * endPos + (1 - atEnd) * endPos * 0.05f;
            }
           
            x += r.Next(-6, 6);
            float y = r.Next(30, 40);
            info.Direction = atEnd == 0 ? Direction.Right : Direction.Left;
            info.EngineState = EngineState.Working;
            info.WheelsState  = WheelsState.In;
            info.PositionType = StartPositionType.Airborne;
            info.Position = new PointD(x,y);
            info.Speed = GameConsts.EnemyPlane.Speed*0.01f*r.Next(90, 111);
            bounds = new Quadrangle(new PointD(x, y), width, height);
            this.startPositionInfo = info;
            Init();

            attackObject = AttackObject.None;
 		//	StartEngine();
            level.OnEnemyPlaneFromTheSide(!(atEnd == 1));
            temp = new PointD(0, 0);

            interpolateSet = new PointD[interpolationPoints];
            for (int i = 0; i < interpolateSet.Length; i++)
            {
                interpolateSet[i] = null;
            }

            //Console.WriteLine("Enemy plane from the " + (atEnd==1?"right.":"left."));
        }

        #endregion

        #region Public Methods

        public override void Move(float time, float timeUnit)
        {
            timeToNextRocket -= time;
            timeToNextRocket = Math.Max(0, timeToNextRocket);
            //dodane tymczasowo, bo po wgraniu planszy pierwszy komunikat odœwie¿enia przychodzi 
            //z bardzo du¿¹ wartoœci¹ time
         //   if (time > timeUnit/10)
         //       return;
            if (planeState == PlaneState.Crashed)
            {
                MoveAfterCrash(time, timeUnit);
                return;
            }
            float scaleFactor = time/timeUnit;
            warCryTimer += scaleFactor;

            UpdateDamage(scaleFactor); //wyciek oleju
            HeightLimit(time, timeUnit);
            VerticalBoundsLimit(time, timeUnit);

            if (planeState == PlaneState.Destroyed) //jeœli zniszczony to tylko spada
            {
                FallDown(time, timeUnit);
            }
            else
            {
                //czy ma ju¿ zawróciæ
                if (locationState == LocationState.Air)
                {
                    if (IsPassingUserPlane)
                    {
                        level.Controller.OnPlanePass(level.UserPlane);
                    }
                    if (ShouldTurnRound)
                    {
                        if (RelativeAngle != 0) //jeœli musi podci¹gn¹æ lot, to najpierw wyrównuje do poziomu
                            SteerToHorizon(scaleFactor);
                        else
                            TurnRound((Direction)(-1 * (int)direction), TurnType.Airborne);
                        randomDistance = new Random().Next(-GameConsts.EnemyPlane.StoragePlaneDistanceFault,
                                                           GameConsts.EnemyPlane.StoragePlaneDistanceFault) +
                                         (0.1f) * new Random().Next(-10, 10);
                    }
                    else //nie musi zawracaæ - kontynuuj lot
                        ChangePitch(scaleFactor);
                }
                //zmiana wektora ruchu przy zawracaniu
                if (locationState == LocationState.AirTurningRound && isChangingDirection)
                {
                    turningTimeLeft -= time;
                    movementVector = -Mogre.Math.Cos((turningTimeLeft / turningTime) * Mogre.Math.PI) * turningVector;
                    // Speed = MinFlyingSpeed; - wylaczone by Adam (samolot dziwnie zawraca³ ;)
                    // Console.WriteLine(this.Name + " " + movementVector.ToString());

                }
                if (locationState == LocationState.Air && planeState != PlaneState.Crashed &&
                    motorState == EngineState.SwitchedOff)
                    FallDown(time, timeUnit);

                //atak samolotu gracza
                if (ShouldBeChasingUserPlane)
                    AttackUserPlane(level.UserPlane, scaleFactor);
            }
            // Console.WriteLine(temp + "   -   " + movementVector);

            // zmiana by Adam (wyg³adzenie ruchu)
            if (movementVector.EuclidesLength > 0)
            {
                temp.X = movementVector.X;
                if (isChangingDirection)
                    temp.Y = movementVector.Y;
                else
                    temp.Y = movementVector.Y*0.7f;
            }
            else
            {
                temp.X /= 2.0f;
                temp.Y /= 2.0f;
            }


            //Interpolacja by Kamil
            if (lastInterpolationNull != (interpolateSet.Length - 1))
            {
                for (int i = 0; i < interpolateSet.Length; i++)
                {
                    if (interpolateSet[i] == null)
                    {
                        lastInterpolationNull = i;
                        break;
                    }
                }
            }

            for (int i = 1; i <= lastInterpolationNull; i++)
            {
                interpolateSet[i] = interpolateSet[i - 1];
            }

            interpolateSet[0] = temp;


            PointD finalValue = new PointD();
            for (int i = 0; i <= lastInterpolationNull; i++)
            {
                finalValue.X += interpolateSet[i].X;
                finalValue.Y += interpolateSet[i].Y;
            }
            finalValue.X /= lastInterpolationNull + 1;
            finalValue.Y /= lastInterpolationNull + 1;
           


            //bounds.Move(scaleFactor*finalValue);
            //Koniec interpolacji by Kamil

            bounds.Move(scaleFactor * temp);
            // zmiana by Adam

            if (IsEngineWorking)
            {
                airscrewSpeed = minAirscrewSpeed + (int) Mogre.Math.Abs((int) (15f*movementVector.X));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Zmienia pu³ap samolotu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void ChangePitch(float scaleFactor)
        {
            //jeœli za bardzo leci w dó³ - podci¹gam lot. Przede wszystkim ma siê nie rozbiæ
            if (ShouldSteerUp)
            {
                Rotate((float) direction*scaleFactor*rotateStep);
                return;
            }
            EnemyPlane closestEnemyPlane;

            if (ShouldBeChasingUserPlane)
            {
                if (attackObject != AttackObject.UserPlane)
                {
                    attackObject = AttackObject.UserPlane;
                    isAlarmDelivered = false;
                }


                if ((closestEnemyPlane = GetNearestEnemyPlaneCrashThreat()) != null) //czy ma omin¹æ inne samoloty
                {
                    if (closestEnemyPlane.PlaneState != PlaneState.Crashed)
                        AvoidEnemyPlaneCrash(scaleFactor, closestEnemyPlane);
                }
                else if (ShouldAvoidUserPlaneCrash) //czy ma omin¹æ gracza
                {
                    Trace.WriteLine("AVOIDING!!!");
                    AvoidUserPlaneCrash(scaleFactor);
                }
                else
                {
                   
                    //czy ma lecieæ w dó³
                    float yDiff =  1 + Math.Abs(Center.Y - level.UserPlane.Center.Y);
                    float yDiffNorm = (float)Math.Log10(yDiff) * 0.5f;

                   

                    if (Center.Y > level.UserPlane.Center.Y + userPlaneHeightDiff && RelativeAngle > -maxAngle)
                    {
                        Trace.WriteLine("DOWN PITCH: " + yDiff + " normalized: " + yDiffNorm);
                        RotateDown(scaleFactor * rotateStep * yDiffNorm);
                    }
                    else //czy ma lecieæ w górê
                    {
                        if (Center.Y < level.UserPlane.Center.Y - userPlaneHeightDiff && RelativeAngle < maxAngle)
                        {
                            Trace.WriteLine("UP PITCH: " + yDiff + " normalized: " + yDiffNorm);
                            RotateUp(scaleFactor * rotateStep * yDiffNorm);
                        }
                        else //czy ma prostowaæ samolot 
                        if (Math.Abs(RelativeAngle) >= 0)
                        {
                            Trace.WriteLine("HORIZON PITCH: " + yDiff + " normalized: " + yDiffNorm);
                            SteerToHorizon(scaleFactor);
                            
                        }
                                
                    }
                }
            }
            else
            {
                carrierDistance = Math.Abs(Center.X - level.Carrier.GetRestoreAmunitionPosition().X);

                if (PlanesOnAircraftPos.CompareTo(level.Carrier.GetRestoreAmunitionPosition()) == 0)
                {
                    isAlarmDelivered = false;
                    attackObject = AttackObject.None;
                }
                else
                {
                    attackObject = AttackObject.Carrier;
                }
                // 
                if (attackObject == AttackObject.Carrier && !isAlarmDelivered &&
                    carrierDistance < GameConsts.EnemyPlane.CarrierDistanceAlarm)
                {
                    level.Controller.OnEnemyAttacksCarrier();
                    isAlarmDelivered = true;
                }

                if ((closestEnemyPlane = GetNearestEnemyPlaneCrashThreat()) != null) //czy ma omin¹æ inne samoloty
                {
                    if (closestEnemyPlane.PlaneState != PlaneState.Crashed)
                        AvoidEnemyPlaneCrash(scaleFactor, closestEnemyPlane);
                }
                else if (ShouldAvoidUserPlaneCrash) //najpierw ma nie zderzyæ siê z graczem
                    AvoidUserPlaneCrash(scaleFactor);
                else
                {
                    if (IsAfterPlanesOnCarrier) //jeœli ju¿ min¹³ cel, to zwiêkszam pu³ap
                    {
                        if (RelativeAngle < maxAngle)
                            RotateUp(scaleFactor*rotateStep);
                    }
                    else
                    {
                        if (CanHitStoragePlanes)
                        {
                            AttackStoragePlanes();
                        }
                        else
                        {
                            // dziób do do³u jeœli atakuje coœ lub jest b. blisko lotniskowca
                            if (RelativeAngle > -maxAngle &&
                                (carrierDistance < 0.2f*GameConsts.EnemyPlane.CarrierDistanceAlarm ||
                                 attackObject != AttackObject.None))
                                RotateDown(scaleFactor*rotateStep);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Steruje samolotem tak, ¿eby wróci³ do poziomu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void SteerToHorizon(float scaleFactor)
        {
            Rotate(-1.0f*(float) direction*Math.Sign(RelativeAngle)*
                   Math.Min(rotateStep*scaleFactor, Mogre.Math.Abs(RelativeAngle)));
        }

        /// <summary>
        /// Wystrzeliwuje rakietê blokuj¹c zbyt czêste strza³y.
        /// </summary>
        private void FireRocket()
        {
            if (timeToNextRocket <= 0)
            {
                timeToNextRocket = GameConsts.EnemyPlane.NextRocketInterval;
                weaponManager.Fire(Angle, WeaponType.Rocket);
            }
        }

        /// <summary>
        /// Okreœla czy samolot mo¿e trafiæ samolot gracza - rakiet¹ albo z dzia³ka.
        /// </summary>
        /// <param name="rocketAttack">Jeœli true sprawdzana bêdzie mo¿liwoœæ trafienia rakiet¹
        /// w przeciwnym przypadku mo¿liwoœæ trafienia dzia³kiem.</param>
        /// <returns></returns>
        private bool CanHitUserPlane(bool rocketAttack, float tolerance)
        {
            if (rocketAttack)
                return weaponManager.RocketCount > 0 && Rocket.CanHitEnemyPlane(this, level.UserPlane, tolerance);
            else
                return Gun.CanHitPlane(this, level.UserPlane, tolerance);

          
        }

        /// <summary>
        /// Próbuje zaatkaowaæ samolot gracza. Najpierw sprawdza mo¿liwoœæ ataku rakiet¹
        /// a póŸniej dzia³kiem.
        /// </summary>
        private void AttackUserPlane(Plane userPlane, float scaleFactor)
        {
            //return; //chwilowo do testów
            //sprawdzam czy samolot nie jest za daleko, ¿eby atakowaæ
            
                // staraj sie dogonic samolot gracza
            if (IsTurnedTowardsUserPlane(userPlane) && Speed < GameConsts.EnemyPlane.Speed * 1.4f)
            {
                Speed += 0.8f * scaleFactor;
            }
            
            if (!IsTurnedTowardsUserPlane(userPlane) && Speed > Plane.MinFlyingSpeed)
            {
                Speed -= 0.8f * scaleFactor;
            }


            if (Math.Abs(Center.X - level.UserPlane.Center.X) > GameConsts.EnemyPlane.ViewRange)
            {
                return;
            }


            if (CanHitUserPlane(true, 100 - GameConsts.EnemyPlane.Accuracy)) //najpierw próbuje strzeliæ rakiet¹
            {
                if(warCryTimer > warCryTimerMin)
                {
                    level.Controller.OnWarCry(this);
                    warCryTimer = 0;
                }
                FireRocket();
            }

            else if (CanHitUserPlane(false, 100 - GameConsts.EnemyPlane.Accuracy))
            {
                if (warCryTimer > warCryTimerMin)
                {
                    level.Controller.OnWarCry(this);
                    warCryTimer = 0;
                }
                weaponManager.Fire(Angle, WeaponType.Gun);
            }
               
        }

        /// <summary>
        /// Próbuje zaatakowaæ samoloty na lotniskowcu. Nie bêdzie strzela³, jeœli gracz jest 
        /// na lotniskowcu.
        /// </summary>
        private void AttackStoragePlanes()
        {
            if (warCryTimer > warCryTimerMin)
            {
                level.Controller.OnWarCry(this);
                warCryTimer = 0;
            }
            // zmiana by Adam. Samolot moze zaatakowac nawet jesli samolot gracza jest na lotniskowcu, ale szansa jest niewielka
            // || Mogre.Math.RangeRandom(0.0f, 1.0f) > 0.99f
            // wycowalem
            if (!level.UserPlane.IsOnAircraftCarrier)
                FireRocket();
        }

        /// <summary>
        /// Steruje tak samolotem, ¿eby nie wpaœæ na samolot gracza
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void AvoidUserPlaneCrash(float scaleFactor)
        {
            if (Center.Y - level.UserPlane.Center.Y > 0)
            {
                if(RelativeAngle < maxAngle) RotateUp(scaleFactor * 1.25f * rotateStep);
            } else
            {
                if(RelativeAngle > -maxAngle) RotateDown(scaleFactor * 1.25f * rotateStep);
            }
                
            
        }

        /// <summary>
        /// Steruje tak samolotem, ¿eby nie wpaœæ na inny wrogi samolot
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="ep"></param>
        private void AvoidEnemyPlaneCrash(float scaleFactor, EnemyPlane ep)
        {
            if (Center.Y - ep.Center.Y > 0)
            {
                if (RelativeAngle < maxAngle) RotateUp(scaleFactor * 1.25f * rotateStep);
            }
            else
            {
                if (RelativeAngle > -maxAngle) RotateDown(scaleFactor * 1.25f * rotateStep);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sprawdza, czy samolot powinien zawróciæ - tzn. czy nie jest za daleko od samolotu gracza
        /// </summary>
        private bool ShouldTurnRound
        {
            get
            {
                if (ShouldBeChasingUserPlane) //leci za samolotem
                {
                    return IsAfterUserPlane &&
                           !ShouldSteerUp &&
                           (Math.Abs(Center.X - level.UserPlane.Center.X) > distanceFromUserPlane);
                }
                else //atakuje samoloty na lotniskowcu
                {
                    if (!IsAfterPlanesOnCarrier ||
                        ShouldSteerUp ||
                        (Math.Abs(Center.X - PlanesOnAircraftPos.X) < distanceFromStoragePlanes))
                        return false;
                    return DistanceToClosestPlane() > 20;
                }
            }
        }

        private float lastAbsDistance = -1.0f;

        /// <summary>
        /// Sprawdza, czy samolot min¹³ w³aœnie samolot gracza
        /// </summary>
        private bool IsPassingUserPlane
        {
            get
            {
                float currentDistanceToUserPlane = Center.X - level.UserPlane.Center.X;
                float absDistance = Mogre.Math.Abs(currentDistanceToUserPlane);
                float distanceFactor = 6.5f * Mogre.Math.Abs(level.UserPlane.MovementVector.X) / Plane.MinFlyingSpeed;
                float absYDistance = Mogre.Math.Abs(Center.Y - level.UserPlane.Center.Y);

                bool result = (absDistance < lastAbsDistance && absDistance < distanceFactor && absYDistance < 1.5f * distanceFactor);

                lastAbsDistance = absDistance;
                return result;
            }
        }

        /// <summary>
        /// Sprawdza, czy znajduje siê za samolotem gracza.
        /// </summary>
        private bool IsAfterUserPlane
        {
            get
            {
                //return (direction == Direction.Right && Center.X > level.UserPlane.Center.X) ||
                //         (direction == Direction.Left && Center.X < level.UserPlane.Center.X);
                return (direction == Direction.Right && bounds.LeftMostX > level.UserPlane.Bounds.RightMostX) ||
                       (direction == Direction.Left && bounds.RightMostX < level.UserPlane.Bounds.LeftMostX);
            }
        }

        /// <summary>
        /// Sprawdza, czy znajduje siê za samolotem gracza.
        /// </summary>
        private bool IsTurnedTowardsUserPlane(Plane userPlane)
        {
            return (direction == Direction.Right && bounds.LeftMostX < userPlane.Bounds.RightMostX) ||
                   (direction == Direction.Left && bounds.RightMostX > userPlane.Bounds.LeftMostX);
       
        }

        /// <summary>
        /// Sprawdza czy samolot znajduje siê za samolotami na lotniskowcu
        /// </summary>
        private bool IsAfterPlanesOnCarrier
        {
            get
            {
                return (direction == Direction.Right && Center.X > PlanesOnAircraftPos.X) ||
                       (direction == Direction.Left && Center.X < PlanesOnAircraftPos.X);
            }
        }

        /// <summary>
        /// Sprawdza czy samolot powinien zwiêkszyæ pu³ap, ¿eby nie spaœæ poni¿ej minimalnego.
        /// </summary>
        private bool ShouldSteerUp
        {
            get
            {
            	ShipTile st = this.GetNearestShipCrashThreat();
            	if(st != null)
            	{	
            		//Console.WriteLine("ydiff" + YDistanceToTile(st) + " ->" + (15*height));
             
            		if(YDistanceToTile(st) < 15*height)
	            	{
	            		return true;
	            	}
            	}
            	
                if (bounds.LowestY <= GameConsts.EnemyPlane.MinPitch) //czy ju¿ nie jest za nisko
                    return true;
                if (RelativeAngle >= 0)
                    return false;
                // if (this.Center.Y > GameConsts.UserPlane.MaxHeight * 0.9f) return false;

                //czas po którym samolot dotrze do mininalnego pu³apu
                float timeToGetTheLowestPitch = (bounds.LowestY - GameConsts.EnemyPlane.MinPitch)/
                                                Math.Abs(movementVector.Y);
                //czas po którym samolot "wyprostuje siê"
                float timeToGetToHorizonatal = Math.Abs(RelativeAngle)/rotateStep;
                return timeToGetToHorizonatal >= 0.5*timeToGetTheLowestPitch;
            }
        }

        /// <summary>
        /// Zwraca czy samolot wroga ma lecieæ w kierunku samolotu gracza.
        /// </summary>
        private bool ShouldBeChasingUserPlane
        {
            get
            {
                return !level.UserPlane.IsOnAircraftCarrier && //nie atakuje samolotu na lotniskowcu
                       (
                           level.StoragePlanes.Count <= 0 || //nie ma samolotow na lotniskowcu
                           weaponManager.RocketCount <= 0 || //nie ma ju¿ rakiet
                           (Math.Abs(Center.X - level.UserPlane.Center.X) < Math.Abs(Center.X - PlanesOnAircraftPos.X))
                       );
            }
        }

        /// <summary>
        /// Zwraca pozycjê samolotów pozosta³ych na lotniskowcu.
        /// Jeœli nie ma samolotów zwraca pozycjê RestoreTile
        /// </summary>
        private PointD PlanesOnAircraftPos
        {
            get
            {
                if (level.StoragePlanes.Count <= 0 || level.UserPlane.IsOnAircraftCarrier)
                    //gdy nie ma samolotów zwraca pozycjê RestoreTile
                    return level.Carrier.GetRestoreAmunitionPosition();
                else
                    return
                        new PointD(level.StoragePlanes[0].Bounds.Center.X + randomDistance,
                                   level.StoragePlanes[0].Bounds.Center.Y);
                //return new PointD(level.StoragePlanes[level.StoragePlanes.Count - 1].Bounds.Center.X - 2*LevelTile.Width, level.StoragePlanes[level.StoragePlanes.Count - 1].Bounds.Center.Y); 
            }
        }

        private float randomDistance;

        /// <summary>
        /// Zwraca czy samolot mo¿e trafiæ któryœ z samolotów na lotniskowcu.
        /// </summary>
        private bool CanHitStoragePlanes
        {
            get
            {
                for (int i = 0; i < level.StoragePlanes.Count; i++)
                    if (Math.Abs(level.StoragePlanes[i].Center.X - Center.X) <
                        GameConsts.EnemyPlane.AttackStoragePlaneDistance &&
                        level.StoragePlanes[i].PlaneState == PlaneState.Intact &&
                        Rocket.CanHitEnemyPlane(this, level.StoragePlanes[i]))
                        return true;
                return false;
                //RelativeAngle < -maxAngle/2; 
            }
        }

        /// <summary>
        /// Sprawdza, czy trzeba w danym momencie tak sterowaæ samolotem, ¿eby nie rozbi³
        /// siê o samolot gracza.
        /// </summary>
        private bool ShouldAvoidUserPlaneCrash
        {
            get
            {
                if (
                    (direction == level.UserPlane.Direction && Math.Abs(Center.X - level.UserPlane.Center.X) > 0.2f * GameConsts.EnemyPlane.SafeUserPlaneDistance && Math.Abs(Center.Y - level.UserPlane.Center.Y) > 0.2f * safeUserPlaneHeightDiff) ||

                    IsAfterUserPlane ||
                    Math.Abs(Center.X - level.UserPlane.Center.X) > GameConsts.EnemyPlane.SafeUserPlaneDistance ||
                    Math.Abs(Center.Y - level.UserPlane.Center.Y) > safeUserPlaneHeightDiff
                    )
                    return false;
                return true;
            }
        }
        
        /// <summary>
        /// Zwraca ship tile ktory jest najblizej i jednoczesnie jest zagrodzeniem z uwagi na odleglosc
        /// </summary>
        /// <returns></returns>
        private ShipTile GetNearestShipCrashThreat()
        {
        	
            float dist = float.MaxValue;
            float temp;
            int index = -1;
            ShipTile st;
            for (int i = 0; i < level.ShipsList.Count; i++)
            {
            	st = (ShipTile)(level.ShipsList[i]);
            	float x = XDistanceToTile(st);
            	float y = YDistanceToTile(st);
                if ( x != -1 && 
            	     y != -1 &&
                     x < width * 30 &&
                     y < height * 20
                    )
                {
                    temp = x+y;
                    if (dist > temp)
                    {
                        dist = temp;
                        index = i;
                    }
                }
            }
            if (index != -1) return level.ShipsList[index] as ShipTile;
            return null;
        	
        }

        /// <summary>
        /// Zwraca najbli¿szy wrogi samolot gracza, który jest BLI¯EJ ni¿ minimalna bezpieczna odleg³oœæ, lec¹cy naprzeciw bie¿¹cego samolotu. NULL, gdy nie ma samolotu spe³niaj¹cego te warunki
        /// </summary>
        /// <returns></returns>
        private EnemyPlane GetNearestEnemyPlaneCrashThreat()
        {
            float dist = float.MaxValue;
            float temp;
            int index = -1;
            Plane ep;
            for (int i = 0; i < level.EnemyPlanes.Count; i++)
            {
                ep = level.EnemyPlanes[i];

                // testowo wy³¹czony warunek
                /* 
             if (direction == ep.Direction)
                {
                    continue;
                }*/
                if (ep.Equals(this)) continue;

                if (
                    XDistanceToPlane(ep) < GameConsts.EnemyPlane.SafeUserPlaneDistance &&
                    YDistanceToPlane(ep) < safeUserPlaneHeightDiff
                    )
                {
                    temp = DistanceToPlane(ep);
                    if (dist > temp)
                    {
                        dist = temp;
                        index = i;
                    }
                }
            }
            if (index != -1) return level.EnemyPlanes[index] as EnemyPlane;
            return null;
        }

        private void UpdateDamage(float scaleFactor)
        {
            if (planeState == PlaneState.Damaged)
                oil -= scaleFactor*GameConsts.EnemyPlane.OilLoss;
            oil = Math.Max(oil, 0);

            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed && oil == 0)
                OutOfPetrolOrOil(scaleFactor);
        }

        #endregion
    }
}