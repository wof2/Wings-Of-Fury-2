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
using Mogre;
using Wof.Model.Configuration;
using Wof.Model.Level.Planes;
using Wof.Controller;
using Math=Mogre.Math;

namespace Wof.View.NodeAnimation
{
    /// <summary>
    /// Manager do zarz¹dzania animacj¹ samolotów
    /// <author>Adam Witczak</author>
    /// </summary>
    public class PlaneNodeAnimationManager : NodeAnimationManager
    {
        private readonly PlaneView planeView;

        public Degree IdleMaxAngle
        {
            set { (this[AnimationType.IDLE] as SinRotateNodeAnimation).MaxAngle = value; }
            get { return (this[AnimationType.IDLE] as SinRotateNodeAnimation).MaxAngle; }
        }

        /// <summary>
        /// Zwraca true jeœli mo¿na w danej chwili zatrzymaæ animacje. Zwraca false jeœli samolot za chwilê bêdzie zawraca³ i nie mo¿na przerwac wymiany komunikatów z modelem 
        /// </summary>
        public bool CanStopAnimation
        {
            get { return !prepareToChangeDirection && !prepareToChangeDirectionOnCarrier && !prepareToSpin; }
        }

        /// <summary>
        /// Jeœli ustawione na true oznacza, ¿e samolot za chwilê wykona zawracanie (czekamy a¿ skoñczy siê bie¿¹ca animacja)
        /// </summary>
        private bool prepareToChangeDirection = false;

        /// <summary>
        /// Flaga ustawiana z zewn¹trz, po przekazaniu komunikatu z modelu przed uruchomieniem wlasciwej animacji zawracania samolotu
        /// </summary>
        public bool PrepareToChangeDirection
        {
            get { return prepareToChangeDirection; }
            set { prepareToChangeDirection = value; }
        }

        
        private bool prepareToChangeDirectionOnCarrier = false;

        /// <summary>
        /// Flaga ustawiana z zewn¹trz, po przekazaniu komunikatu z modelu przed uruchomieniem wlasciwej animacji zawracania samolotu na lotniskowcu
        /// </summary>
        public bool PrepareToChangeDirectionOnCarrier
        {
            get { return prepareToChangeDirectionOnCarrier; }
            set { prepareToChangeDirectionOnCarrier = value; }
        }

              
        private bool prepareToSpin = false;

        /// <summary>
        /// Jeœli ustawione na true oznacza, ¿e samolot za chwilê wykona obrót ('z pleców na brzuch')
        /// </summary>
        public bool PrepareToSpin
        {
            get { return prepareToSpin; }
            set { prepareToSpin = value; }
        }

        /// <summary>
        /// Pole przechowuj¹ce delegata, który ma byæ odpalony po drugiej czêœci dwuetapowej animacji np.:Spin
        /// </summary>
        /// <author>Kamil S³awiñski</author>
        private NodeAnimation.NotityFinish queuedOnFinish = null;

        /// <summary>
        /// Argumenty delegata, który ma byæ odpalony po drugiej czêœci dwuetapowej animacji np.:Spin
        /// </summary>
        /// <author>Kamil S³awiñski</author>
        public object queuedOnFinishInfo = null;

        /*
        private bool queuedWaiting;

        /// <summary>
        /// Zwraca true jeœli zakolejkowana jest druga czêœæ dwuetapowej animacji np.:Spin
        /// </summary>
        /// <author>Kamil S³awiñski</author>
        public bool QueuedWaiting
        {
            get { return queuedWaiting; }
        }*/

        #region Animation types

        public enum AnimationType
        {
            IDLE = 0,
            BLADE,
            OUTERTURN,
            INNERTURN,
            TURN_ON_CARRIER,
            L_GEAR_UP,
            R_GEAR_UP,
            REAR_GEAR_UP,
            SPIN_PHASE_ONE,
            SPIN_PHASE_TWO,
            DEATH_SPIN
        } ;

        #endregion

        #region Methods

        public PlaneNodeAnimationManager(float currentTime, PlaneView planeView)
            : base(currentTime)
        {
            this.planeView = planeView;
            foreach (string name in Enum.GetNames(typeof (AnimationType)))
            {
                AnimationType type = (AnimationType) Enum.Parse(typeof (AnimationType), name);

               

                register(type, 0);
            }
        }

        public void enableBlade()
        {
            this[AnimationType.BLADE].Enabled = true;
        }

        public void disableBlade()
        {
            this[AnimationType.BLADE].Enabled = false;
        }

        public void switchToIdle()
        {
            switchToIdle(true);
        }

        public void disableIdle()
        {
            this[AnimationType.IDLE].Enabled = false;
        }

        public void changeBladeSpeed(float rpm)
        {
            (this[AnimationType.BLADE] as ConstRotateNodeAnimation).MaxAngle = rpm;
        }


        public void switchToIdle(bool disableOthers)
        {
            if (disableOthers) disableAll();
            enableBlade();
            switchTo(AnimationType.IDLE);
            Enabled = true;
            Looped = true;
            currentAnimation.TimeScale = 1.0f;
        }


        public void switchToTurn()
        {
            switchToTurn(true, null, null);
        }

        public void switchToTurn(bool disableOthers)
        {
            switchToTurn(disableOthers, null, null);
        }

        public void switchToTurn(bool disableOthers, NodeAnimation.NotityStart onStart,
                                 NodeAnimation.NotityFinish onFinish)
        {
            if (disableOthers) disableAll();
            enableBlade();

            switchTo(AnimationType.INNERTURN);
            rewind(true);
            Looped = false;

            switchTo(AnimationType.OUTERTURN);
            rewind(true);
            Looped = false;


            TurnInfo ti = new TurnInfo();
            ti.plane = planeView.Plane;
            ti.turnDurationInSeconds = currentAnimation.Duration;
            ti.turnType = TurnType.Airborne;

            if (onStart != null)
            {
                currentAnimation.onStart = onStart;
                currentAnimation.onStartInfo = ti;
            }
            if (onFinish != null)
            {
                currentAnimation.onFinish = onFinish;
                currentAnimation.onFinishInfo = ti;
            }
        }

        public void switchToTurnOnCarrier(bool disableOthers, NodeAnimation.NotityStart onStart,
                                          NodeAnimation.NotityFinish onFinish)
        {
            if (disableOthers) disableAll();
            enableBlade();


            switchTo(AnimationType.TURN_ON_CARRIER);
            rewind(true);
            Looped = false;


            TurnInfo ti = new TurnInfo();
            ti.plane = planeView.Plane;
            ti.turnDurationInSeconds = currentAnimation.Duration;
            ti.turnType = TurnType.Carrier;

            if (onStart != null)
            {
                currentAnimation.onStart = onStart;
                currentAnimation.onStartInfo = ti;
            }
            if (onFinish != null)
            {
                currentAnimation.onFinish = onFinish;
                currentAnimation.onFinishInfo = ti;
            }
        }

        public void switchToDeathSpin(bool disableOthers, NodeAnimation.NotityStart onStart, NodeAnimation.NotityFinish onFinish)
        {
            bool enableBladed = false;
            switchTo(AnimationType.BLADE);
            if (currentAnimation.Enabled)
            {
                enableBladed = true;
            }
            if (disableOthers) disableAll();

            if (enableBladed)
            {
                enableBlade();
            }

            switchTo(AnimationType.DEATH_SPIN);
            rewind(true);
            Looped = true;

            if (onStart != null)
            {
                currentAnimation.onStart = onStart;
                currentAnimation.onStartInfo = null;
            }
            if (onFinish != null)
            {
                currentAnimation.onFinish = onFinish;
                currentAnimation.onFinishInfo = null;
            }
        }

        public void switchToSpin(bool disableOthers, NodeAnimation.NotityStart onStart,
                                 NodeAnimation.NotityFinish onFinish, object onFinishInfo , bool queue)
        {
            if (disableOthers) disableAll();
            enableBlade();

            //  this.planeView.OuterNode.ResetOrientation();

            switchTo(queue ? AnimationType.SPIN_PHASE_ONE : AnimationType.SPIN_PHASE_TWO);
            rewind(true);
            Looped = false;

            if (onStart != null)
            {
                currentAnimation.onStart = onStart;
                currentAnimation.onStartInfo = null;
            }
            if (onFinish != null)
            {
                //1sza faza spinu
                if (queue)
                {    
                    currentAnimation.onFinish = OnFinishHalfSpin;

                    queuedOnFinish = onFinish;
                    queuedOnFinishInfo = onFinishInfo;
                }
                //2ga faza spinu
                else
                {
                    currentAnimation.onFinish = onFinish;
                    currentAnimation.onFinishInfo = onFinishInfo;
                }

            }
        }


        public void switchToGearUpDown()
        {
            switchToGearUpDown(true, null, null);
        }

        public void switchToGearUpDown(bool disableOthers)
        {
            switchToGearUpDown(disableOthers, null, null);
        }

        public void switchToGearUpDown(bool disableOthers, NodeAnimation.NotityStart onStart,
                                       NodeAnimation.NotityFinish onFinish)
        {
            if (disableOthers) disableAll();

            switchTo(AnimationType.L_GEAR_UP);
            (currentAnimation as RotateNodeAnimation).MaxAngle *= -1;
            rewind(true);
            Looped = false;

            switchTo(AnimationType.R_GEAR_UP);
            (currentAnimation as RotateNodeAnimation).MaxAngle *= -1;
            rewind(true);
            Looped = false;


            switchTo(AnimationType.REAR_GEAR_UP);
            (currentAnimation as RotateNodeAnimation).MaxAngle *= -1;
            rewind(true);
            Looped = false;


            if (onStart != null)
            {
                currentAnimation.onStart = onStart;
            }
            if (onFinish != null)
            {
                currentAnimation.onFinish = onFinish;
            }
        }

        public void SetGearsVisible(bool visible)
        {
            if (planeView is P47PlaneView)
            {
                planeView.LWheelNode.SetVisible(visible);
                planeView.RWheelNode.SetVisible(visible);
                planeView.RearWheelNode.SetVisible(visible);
            }
        }

        private bool register(AnimationType animationType, float duration)
        {
            string animationName = getNameByType(animationType);
            switch (animationType)
            {
                    #region IDLE

                case AnimationType.IDLE:
                    {
                        this[animationName] = new SinRotateNodeAnimation(
                            planeView.IdleNode,
                            (duration <= 0 ? 1.0f : duration),
                            new Degree(2),
                            Math.TWO_PI,
                            Vector3.UNIT_Z,
                            animationName
                            );
                    }
                    break;

                    #endregion

                    #region BLADE

                    case AnimationType.BLADE:
                    {
                        float airscrewSpeed = 1000; // 1000 RPM
                        if (planeView.Plane != null)
                        {
                            airscrewSpeed = planeView.Plane.AirscrewSpeed;
                        }
                        List<SceneNode> nodes = new List<SceneNode>();
                        if (planeView.BladeNode != null) nodes.Add(planeView.BladeNode);
                        if (planeView.BladeNodeL != null) nodes.Add(planeView.BladeNodeL);
                        if (planeView.BladeNodeR != null) nodes.Add(planeView.BladeNodeR);

                        this[animationName] = new ConstRotateNodeAnimation(
                            nodes,
                            airscrewSpeed,
                            Vector3.UNIT_Z,
                            animationName
                            );
                        this[animationName].Looped = true;
                    }
                    break;

                    #endregion

                    #region TURN

                case AnimationType.INNERTURN:
                    {


                        this[animationName] = new GaussRotateNodeAnimation(
                            planeView.InnerNode,
                            (duration <= 0 ? GameConsts.UserPlane.Singleton.TurningDuration : duration),
                            new Degree(60),
                            10.0f,
                            Vector3.UNIT_Z,
                            animationName
                            );
                        this[animationName].Looped = false;
                    }
                    break;

                case AnimationType.OUTERTURN:
                    {
                        this[animationName] = new SinRotateNodeAnimation(
                            planeView.OuterNode,
                            (duration <= 0 ? GameConsts.UserPlane.Singleton.TurningDuration : duration),
                            new Degree(180),
                            Math.HALF_PI,
                            Vector3.UNIT_Y,
                            animationName
                            );
                        this[animationName].Looped = false;
                        (this[animationName] as SinRotateNodeAnimation).ReturnToInitialOrientation = true;
                    }
                    break;

                case AnimationType.TURN_ON_CARRIER:
                    {
                        this[animationName] = new SinRotateNodeAnimation(
                            planeView.OuterSteeringNode,
                            (duration <= 0 ? 5.0f : duration),
                            new Degree(180),
                            Math.HALF_PI,
                            Vector3.UNIT_Y,
                            animationName
                            );
                        this[animationName].Looped = false;
                        (this[animationName] as SinRotateNodeAnimation).ReturnToInitialOrientation = true;
                    }
                    break;

                    #endregion

                    #region GEARS_UP

                case AnimationType.L_GEAR_UP:
                    {
                        if (planeView is P47PlaneView)
                        {
                            Degree deg = new Degree(90);
                            if(planeView is B25PlaneView)
                            {
                                deg = 120;
                            }
                            this[animationName] = new SinRotateNodeAnimation(
                                (planeView as P47PlaneView).LWheelInnerNode,
                                (duration <= 0 ? 2.0f : duration),
                                deg,
                                Math.HALF_PI,
                                Vector3.NEGATIVE_UNIT_Z,
                                animationName
                                );
                            this[animationName].Looped = false;
                        }
                    }
                    break;

                case AnimationType.R_GEAR_UP:
                    {
                        if (planeView is P47PlaneView)
                        {
                            Degree deg = new Degree(90);
                            if (planeView is B25PlaneView)
                            {
                                deg = 120;
                            }
                            this[animationName] = new SinRotateNodeAnimation(
                                (planeView as P47PlaneView).RWheelInnerNode,
                                (duration <= 0 ? 2.0f : duration),
                                deg,
                                Math.HALF_PI,
                                Vector3.NEGATIVE_UNIT_Z,
                                animationName
                                );
                            this[animationName].Looped = false;
                        }
                    }
                    break;

                case AnimationType.REAR_GEAR_UP:
                    {
                        if (planeView is P47PlaneView)
                        {
                            Degree deg = new Degree(45);
                            if (planeView is B25PlaneView)
                            {
                                deg = 120;
                            }
                            this[animationName] = new SinRotateNodeAnimation(
                                (planeView as P47PlaneView).RearWheelInnerNode,
                                (duration <= 0 ? 2.0f : duration),
                               deg,
                                Math.HALF_PI,
                                Vector3.NEGATIVE_UNIT_X,
                                animationName
                                );
                            this[animationName].Looped = false;
                        }
                    }
                    break;

                    #endregion

                    case AnimationType.SPIN_PHASE_ONE:
                    {
                        this[animationName] = new AccRotateNodeAnimation(
                        planeView.OuterNode,
                        1.0f,
                        new Degree(90),
                        1,
                        Vector3.UNIT_Z,
                        animationName
                        );

                    /*    this[animationName] = new HalfSinRotateNodeAnimation(
                                                            planeView.OuterNode,
                                                            1.2f,
                                                            new Degree(90),
                                                            Math.PI,
                                                            Vector3.UNIT_Z,
                                                            animationName,
                                                            true
                                                            );
                        */
                        this[animationName].Looped = false;
                    }
                    break;
                    case AnimationType.SPIN_PHASE_TWO:
                    {

                        this[animationName] = new Acc2RotateNodeAnimation(
                         planeView.OuterNode,
                         1.0f,
                         new Degree(90),
                         1,
                         Vector3.UNIT_Z,
                         animationName
                         );

                        /*this[animationName] = new HalfSinRotateNodeAnimation(
                                                            planeView.OuterNode,
                                                            1.2f,
                                                            new Degree(90),
                                                            Math.PI,
                                                            Vector3.UNIT_Z,
                                                            animationName,
                                                            false
                                                            );
                        */
                        this[animationName].Looped = false;
                    }
                    break;

                    case AnimationType.DEATH_SPIN:
                    {
                        this[animationName] = new AccRotateNodeAnimation(
                                                              planeView.OuterNode,
                                                              9,
                                                              new Degree(360*3),
                                                              new Radian(Mogre.Math.Sqrt(3)),
                                                              Vector3.UNIT_Z,
                                                              animationName
                                                              );
                        this[animationName].Looped = true;         
                    }
                    break;


                   
                default:
                    return false;
            }

            if (animations.Count == 1) currentName = animationName;
            return true;
        }

        private void OnFinishHalfSpin(object args)
        {
            //EngineConfig.SpinKeys = true;
            switchToSpin(true, null, queuedOnFinish, queuedOnFinishInfo , false);
        }

        #endregion
    }
}