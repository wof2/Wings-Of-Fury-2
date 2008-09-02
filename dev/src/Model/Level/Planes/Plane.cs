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
using Wof.Model.Level.Carriers;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Weapon;
using Math=Mogre.Math;

namespace Wof.Model.Level.Planes
{

    #region Enums

    /// <summary>
    /// Enumerator mówi co aktualnie przysz³o z kontrolera.
    /// </summary>
    public enum InputFlag
    {
        Up,
        Down,
        Left,
        Right,
        EngineOn,
        Spin
    } ;

    /// <summary>
    /// Rodzaj zawracania samolotu.
    /// </summary>
    public enum TurnType
    {
        Airborne,
        Carrier
    } ;

    /// <summary>
    /// Informacje o obrocie samolotu.
    /// </summary>
    public struct TurnInfo
    {
        public Plane plane;
        public float turnDurationInSeconds;
        public TurnType turnType;
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Rejestruje dany typ broni w modelu.
    /// </summary>
    /// <param name="ammunition">Amunicja do zarejestrowania.</param>
    /// <author>Michal Ziober</author>
    public delegate void RegisterWeapon(Ammunition ammunition);

    #endregion

    #region Class Plane 

    /// <summary>
    /// Klasa opisujaca dzialanie samolotu.
    /// </summary>
    public class Plane : IRenderableQuadrangles
    {
        #region Constants

        /// <summary>
        /// K¹t o jaki bêdzie siê zmienia³o nachylenie samolotu po jednokrotnym naciœniêciu strza³ki.
        /// (Wyra¿ony w radianach)
        /// </summary>
        private readonly float rotateStep = GameConsts.UserPlane.UserRotateStep;

        /// <summary>
        /// Prêdkoœc przy wolnym ko³owaniu.
        /// </summary>
        private readonly float slowWheelingSpeed = GameConsts.UserPlane.RangeSlowWheelingSpeed*GameConsts.UserPlane.MaxSpeed;

        /// <summary>
        /// maksymalna prêdkoœæ przy szybkim ko³owaniu.
        /// </summary>
        private static readonly float maxFastWheelingSpeed = GameConsts.UserPlane.RangeFastWheelingMaxSpeed*
                                                    GameConsts.UserPlane.MaxSpeed;

        /// <summary>
        /// Okreœla maksymaln¹ prêdkoœæ samolotu z wysuniêtym podwoziem
        /// </summary>
        private readonly float maxWheelOutSpeed = 0.5f*GameConsts.UserPlane.MaxSpeed;

        /// <summary>
        /// Prêdkoœæ przy na lotniskowcu przy której samolot opuszcza dziób.
        /// </summary>//musialem usunac const aby mozna bylo je ustwic. Michal
        private static readonly float changeWheelsSpeed = maxFastWheelingSpeed*0.8f;

        /// <summary>
        /// K¹t o który jest pochylony dziób samolotu w górê w czasie ko³owania na lotniskowcy.
        /// </summary>
        private const float angleOnCarrier = Math.PI*0.05f;

        /// <summary>
        /// Minimalna prêdkoœæ lotu.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        private readonly float minFlyingSpeed = GameConsts.UserPlane.RangeFastWheelingMaxSpeed*GameConsts.UserPlane.MaxSpeed;

        public float MinFlyingSpeed
        {
            get { return minFlyingSpeed; }
        }

        /// <summary>
        /// Maksymalny k¹t przy którym samolot mo¿e zawróciæ w powietrzu.
        /// </summary>
        protected const float maxTurningAngle = Math.PI/6;


        /// <summary>
        /// Szerokoœæ prostok¹ta ograniczaj¹cego samolot
        /// </summary>
        private readonly float width = GameConsts.UserPlane.Width;

        /// <summary>
        /// Wysokoœæ prostok¹ta ograniczaj¹cego samolot
        /// </summary>
        private readonly float height = GameConsts.UserPlane.Height;

        /// <summary>
        /// Okreœla odleglosc po wspó³rzednej X miedzy ko³ami a srodkiem samolotu.
        /// </summary>
        private const float wheelDistanceFromCenter = 1.15f;

        /// <summary>
        /// Minimalna prêdkosc smigiel(ustawiana zaraz po w³¹czeniu silnika).
        /// </summary>
        /// <author>Tomek</author>
        protected const int minAirscrewSpeed = 1000;

        /// <summary>
        /// Maxymalna prêdkosc smigiel(ustawiana dla maksymalnej prêdkoœci).
        /// </summary>
        /// <author>Tomek</author>
        private const int maxAirscrewSpeed = 3000;

        /// <summary>
        /// Przyspieszenie ziemskie [jednostka d³ugoœci w modelu)/s^2]
        /// </summary>
        private const float gravitationalAcceleration = 50;

        /// <summary>
        /// Maksymalna iloœæ oleju.
        /// </summary>
        private const int maxOil = 100;

        public int MaxOil
        {
            get { return maxOil; }
        }

        /// <summary>
        /// Maksymalna iloœæ bêzyny.
        /// </summary>
        private const int maxPetrol = 100;

        public int MaxPetrol
        {
            get { return maxPetrol; }
        }

        /// <summary>
        /// Maksymalny k¹t jaki mo¿e mieæ samolot z otwartym podwoziem.
        /// Wyra¿ony jako liczba dodatnia w radianach.
        /// </summary>
        private const float maxWheelOutAngle = Math.PI/12.0f;

        /// <summary>
        /// K¹t po przekroczeniu którego samolot z otwartym podwoziem zacznie schodziæ w dó³.
        /// </summary>
        private const float landingAngle = Math.PI/20.0f;

        /// <summary>
        /// Iloœæ oleju tracona, gdy samolot zostanie trafiony
        /// 
        /// </summary>
        private float oilLeak = GameConsts.UserPlane.HitCoefficient;

        /// <summary>
        /// Maksymalny k¹t, pod którym mo¿na zrzuciæ bombê.
        /// </summary>
        private const float maxFireBombAngle = 8*Math.PI/18.0f;

        /// OKreœla ile meterów przed maxHeight samolot zaczyna sie prostowac.
        /// </summary>
        private const float maxHeightTurningRange = 0.85f;

        /// <summary>
        /// Okreœla ile tail przed koñcem mapy samolot zaczyna zawracac.
        /// </summary>
        protected int turningTails = 11;

        /// <summary>
        /// Czas od momentu rozbicia samolotu do momentu przejœcia do nastêpnego ¿ycia.
        /// Wyra¿ony w ms.
        /// </summary>
        private const float wreckTime = 3000;

        /// <summary>
        /// Maksymalna wartoœæ o jak¹ mo¿e obróciæ siê samolot w czasie timeUnit.
        /// </summary>
        private readonly float maxRotateValue = GameConsts.UserPlane.UserMaxRotateValue;

        /// <summary>
        /// Si³a hamowania obrotu.
        /// </summary>
        private readonly float rotateBrakingFactor = GameConsts.UserPlane.UserRotateBrakingFactor;

        /// <summary>
        /// Moc hamowania wody w poziomie.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        private readonly float waterXBreakingPower = GameConsts.UserPlane.MaxSpeed*0.01f;

        /// <summary>
        /// Moca hamowania wody w pionie.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        private readonly float waterYBreakingPower = GameConsts.UserPlane.MaxSpeed*0.04f;

        /// <summary>
        /// Maksymalna predkoœæ zeœlizgiwania siê z lotniskowca.
        /// </summary>
        private const float maxSlippingFromCarrierSpeed = 7;

        /// <summary>
        /// Maksymalny k¹t zeœlizgiwania siê z lotniskowca.
        /// </summary>
        private const float maxSlipingFromCarrierAngle = Math.PI*0.2f;

        /// <summary>
        /// Przyœpieszenie w czasie zeœlizgiwania siê z lotniskowca. 
        /// </summary>
        private const float SlippingFromCarrierAcceleration = 10;

        /// <summary>
        /// Okresla ile jest tracone paliwo w jednostce czasu.
        /// </summary>
        private const float rightTakeOffOilLoss = 0.1f*maxOil;

        #endregion

        #region Events

        /// <summary>
        /// Przesyla zdarzenie do modelu aby dodal nowa bron(Bombe, Rakiete).
        /// </summary>
        /// <author>Michal Ziober</author>
        public event RegisterWeapon RegisterWeaponEvent;

        #endregion

        #region Fields

        /// <summary>
        /// Prêdkoœæ obracania œmig³a
        /// </summary>
        protected float airscrewSpeed = 0; // 1000 RPM - 1 tys obrotow na minute    

        public float AirscrewSpeed
        {
            get { return airscrewSpeed; }
        }

        /// <summary>
        /// Okresla czy samolot jest samolotem wroga
        /// </summary>
        protected bool isEnemy = false;

        public bool IsEnemy
        {
            get { return isEnemy; }
        }

     

        /// <summary>
        /// Okreœla czy samolot jest defacto w stanie zmiany kierunku, czyli czy powinien poruszaæ
        /// siê zgodnie z turningVector. Samolot mo¿e byæ w stanie po zawróceniu, ale nie otrzyma³
        /// jeszcze komunikatu o tym, ¿e proces siê zakoñczy³.
        /// </summary>
        protected bool isChangingDirection = false;

        protected Quadrangle bounds;

        protected PointD movementVector;

        /// <summary>
        /// Stan silnika.
        /// </summary>
        protected EngineState motorState;

        /// <summary>
        /// Stan samolotu.
        /// </summary>
        protected PlaneState planeState;

        /// <summary>
        /// Ilosc benzyny. 
        /// </summary>
        private float petrol;

        public float Petrol
        {
            get { return petrol; }
        }

        /// <summary>
        /// Ilosc oleju.
        /// </summary>
        protected float oil;

        public float Oil
        {
            get { return oil; }
        }

        /// <summary>
        /// Licznik ile razy zostal uruchomiony silnik.
        /// </summary>
        private int counterStartedEngine;

        /// <summary>
        /// Okreœla lokalizacje samoloty czy na lotniskowcu czy w powietrzu.
        /// </summary>
        protected LocationState locationState;

        /// <summary>
        /// Okreœla stan kó³
        /// </summary>
        protected WheelsState wheelsState;

        /// <summary>
        /// Okreœla kierunek samolotu.
        /// </summary>
        protected Direction direction;

        /// <summary>
        /// Okreœla czy zosta³a wciœniêta strza³ka w lewo od ostatniego odœwie¿enia.
        /// </summary>
        private bool isLeftPressed;

        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockLeft;

        public bool IsBlockLeft
        {
            get { return isBlockLeft; }
            set { isBlockLeft = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêta strza³ka w prawo od ostatniego odœwie¿enia.
        /// </summary>
        private bool isRightPressed;

        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockRight;

        public bool IsBlockRight
        {
            get { return isBlockRight; }
            set { isBlockRight = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêta strza³ka w dó³ od ostatniego odœwie¿enia.
        /// </summary>
        private bool isDownPressed;

        /// <summary>
        /// Okreœla czy zosta³ wciœniêty przycisk obrotu samolotu (z 'pleców' na 'brzuch').
        /// </summary>
        private bool isSpinPressed;


        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockDown;

        public bool IsBlockDown
        {
            get { return isBlockDown; }
            set { isBlockDown = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêta strza³ka w górê od ostatniego odœwie¿enia.
        /// </summary>
        private bool isUpPressed;

        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockUp;

        public bool IsBlockUp
        {
            get { return isBlockUp; }
            set { isBlockUp = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêty przycisk strza³u z dzia³ka od ostatniego odœwie¿enia.
        /// </summary>
        // private bool isFireGunPressed;
        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockFireGun;

        public bool IsBlockFireGun
        {
            get { return isBlockFireGun; }
            set { isBlockFireGun = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêty przycisk strza³u rakiety od ostatniego odœwie¿enia.
        /// </summary>
        //private bool isFireRocketPressed;
        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockFireRocket;

        public bool IsBlockFireRocket
        {
            get { return isBlockFireRocket; }
            set { isBlockFireRocket = value; }
        }

        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockSpin;

        public bool IsBlockSpin
        {
            get { return isBlockSpin; }
            set { isBlockSpin = value; }
        }

        /// <summary>
        /// Okreœla czy zosta³a wciœniêty przycisk wy³¹czania silnika od ostatniego odœwie¿enia.
        /// </summary>
        private bool isEngineKeyPressed;

        /// <summary>
        /// Okreœla czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockEngine;

        public bool IsBlockEngine
        {
            get { return isBlockEngine; }
            set { isBlockEngine = value; }
        }

        //private bool blockAllInput;

        /// <summary>
        /// Flaga okreœla czy przed chwil¹ zosta³ uruchomiony silnik.
        /// Jest resretowana po puszczeniu przycisku uruchomienia silnika.
        /// </summary>
        private bool IsEngineJustStarted;

        /// <summary>
        /// Flaga okreœla czy przed chwil¹ zosta³ wy³¹czony silnik.
        /// Jest resretowana po puszczeniu przycisku uruchomienia silnika.
        /// </summary>
        private bool IsEngineJustStopped;

        /// <summary>
        /// Mówi ile czasu bêdzie trwa³o ca³e zawracanie.
        /// Zmienna jest odœwie¿ana za ka¿dym zawracaniem.
        /// </summary>
        /// <author>Emil</author>
        protected float turningTime;

        /// <summary>
        /// Ile czasu pozosta³o do koñca zawracania.
        /// </summary>
        /// <author>Emil</author>
        protected float turningTimeLeft;

        protected Level level;

        /// <summary>
        /// Wektor u¿ywany do poruszania samolotu podczas zawracania.
        /// </summary>
        protected PointD turningVector;

        /// <summary>
        /// Obiekt zarzadzajacy bronia samolotu.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected WeaponManager weaponManager;

        /// <summary>
        /// Okreœla stan podczas l¹dowania
        /// </summary>
        private LandingState landingState;

        /// <summary>
        /// Obiekt typu EndAircraftTile, który z³apa³ samolot w czasie l¹dowania.
        /// </summary>
        private EndAircraftCarrierTile breakingEndCarrierTile;

        /// <summary>
        /// Okreœla czy samolot l¹duje. Tzn. ma wysuniête podwozie, jest przechylony max
        /// do góry a gracz wciska strza³kê do góry (i nie wciska strza³ki kierunku)
        /// </summary>
        private bool isLanding;

        /// <summary>
        /// Okreœla czy samolot przed chwil¹ schodzi³ w dó³. Jeœli tak to bez wciœniêtych strza³ek 
        /// w górê/dó³ ma lecieæ prosto.
        /// </summary>
        private bool isAfterFlyingDown;

        /// <summary>
        /// Czas jaki min¹³ od momentu rozbicia.
        /// </summary>
        private float wreckTimeElapsed = 0;

        /// <summary>
        /// Okreœla czy samolot rozbi³ siê w wodzie i ma ton¹æ.
        /// </summary>
        private bool isSinking = false;

        /// <summary>
        /// Oznacza czy samolot jest w trakcie zaminay
        /// kierunku ruchu gdy osi¹gnie wysokoœæ maksymalna.
        /// </summary>
        private bool isMaxHeightRotate;

        /// <summary>
        /// Oznacza prêdkoœæ przed rozpoczêciem rotacji,
        /// gdy samolot znajdzie sie na maksymalnej wysokosci.
        /// </summary>
        private float speedBeforeMaxHeightRotata;

        /// <summary>
        /// Wartoœæ o jak¹ nale¿y obróciæ samolot w danej chwili.
        /// (Odpowiednik movementVector)
        /// </summary>
        private float rotateValue;

        /// <summary>
        /// Wartoœæ o jak¹ nale¿y obróciæ samolot w danej chwili.
        /// </summary>
        public float RotateValue
        {
            get { return rotateValue; }
        }

        /// <summary>
        /// Okreœla czy samolot spada z lotniskowca.
        /// </summary>
        private bool isFallingFromCarrier;

        /// <summary>
        /// Okreœla czy samolot zeœlizguje sie z lotniskowca.
        /// </summary>
        private bool isSlippingFromCarrier;

        /// <summary>
        /// Okreœla czy samolot ma spadaæ po rozbiciu (np. rozbicie o pionow¹ sciane)
        /// </summary>
        private bool isFallingAfterCrash;

        /// <summary>
        /// Prêdkoœæ samoloto w czasie z³apania przez line.
        /// </summary>
        private float lineCatchSpeed;

        /// <summary>
        /// Pozycja wspó³rzêdnej X w 
        /// czasie s³apania samolotu przez line
        /// </summary>
        private float catchLinePositionX;

        /// <summary>
        /// Pozycja samolotu gdzie samolot koñczy hamowanie.
        /// </summary>
        private float breakingEndPositionX;

        /// <summary>
        /// Okreœla czy samolot podnosi ty³ przy poruszaniu siê na lotniskowca
        /// </summary>
        private bool isRaisingTail;

        /// <summary>
        /// Okreœla czy samolot obni¿a ty³ przy poruszaniu siê na lotniskowca
        /// </summary>
        private bool isLoweringTail;

        /// <summary>
        /// Okreœla czy samolot obróci³ siê ruchem spinu
        /// </summary>
        protected bool spinned = false;

        public bool Spinned
        {
            get { return spinned; }
        }

        #endregion

        #region Public Constructor

        public Plane()
        {
        }

        /// <summary>
        /// Konstruktor bezparametrowy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public Plane(Level level, bool isEnemy)
        {
            this.level = level;

            Init();

            weaponManager = new WeaponManager(level, this);
            weaponManager.SelectWeapon = WeaponType.Bomb; //Domyslna cierzka amunicja.
            weaponManager.RegisterWeaponToModelEvent += weaponManager_RegisterWeaponToModelEvent;

            this.isEnemy = isEnemy;
        }

        public Plane(Level level, Quadrangle bounds, bool isEnemy)
            : this(level, isEnemy)
        {
            this.bounds = bounds;
        }

        /// <summary>
        /// Tworzy samolot w miejscu podanym w parametrze i wyskoœci i szerokoœæi zgodnej ze sta³ymi
        /// w klasie Plane.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="startPosition">Pocz¹tkowa pozycja samolotu</param>
        /// <param name="direction">Kierunek samolotu</param>
        /// <author>Emil</author>
        /// <param name="isEnemy"></param>
        public Plane(Level level, PointD startPosition, Direction direction, bool isEnemy)
            : this(level, isEnemy)
        {
            bounds = new Quadrangle(startPosition, width, height);
            this.direction = direction;
        }

        /// <summary>
        /// Tworzy samolot w miejscu podanym w parametrze.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="startPosition">Pocz¹tkowa pozycja samolotu</param>
        /// <author>Tomek</author>
        public Plane(Level level, PointD startPosition)
            : this(level, false)
        {
            bounds = new Quadrangle(startPosition, width, height);
        }

        /// <summary>
        /// Tworzy samolot zgodnie z parametrami
        /// </summary>
        /// <param name="level"></param>
        /// <param name="startPosition"></param>
        /// <param name="direction"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isEnemy"></param>
        public Plane(Level level, PointD startPosition, Direction direction, float width, float height, bool isEnemy)
            : this(level, isEnemy)
        {
            bounds = new Quadrangle(startPosition, width, height);
            this.direction = direction;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca lewy dolny wierzcho³ek, jeœli samolot jest skierowany w prawo lub prawy dolny
        /// w przeciwnym wypadku.
        /// </summary>
        public PointD Position
        {
            get
            {
                //jesli samolot porusza sie w prawo.
                if (direction == Direction.Right)
                    return bounds.Peaks[0];
                else //w lewo
                    return bounds.Peaks[3];
            }
        }

        /// <summary>
        /// Pobiera wartosc czy silnik jest wlaczony.
        /// </summary>
        /// <author>Michal Ziober</author>
        public bool IsEngineWorking
        {
            get { return (motorState == EngineState.Working); }
        }

        /// <summary>
        /// Zwraca true, jesli samolot moze uruchomic silnik.
        /// False w przeciwnym przypadku.
        /// </summary>
        /// <author>Michal Ziober</author>
        public bool CanEngineBeStarted
        {
            get
            {
                //okres czasu jaki musi uplynac aby wlaczyc silnik w zaleznosci od stanu samolotu
                int timeThreshold = planeState == PlaneState.Intact && !isEnemy && locationState == LocationState.Air ? GameConsts.UserPlane.EngineCounterThresholdInAir : GameConsts.UserPlane.EngineCounterThreshold;
                return (petrol > 0 && //czy ilosc benzyny > 0
                        oil > 0 && //czy ilosc oleju > 0
                        planeState != PlaneState.Destroyed && //czy samolot nie jest znisczony ?
                        counterStartedEngine > timeThreshold);
                //czy mozna juz uruchomic silnik ?
            }
        }

        /// <summary>
        /// Pobiera lub ustawia licznik prob uruchomien silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        public int StartEngineCounter
        {
            get { return counterStartedEngine; }
            set { counterStartedEngine = value; }
        }

        /// <summary>
        /// Pobiera Carrier z levelu.
        /// </summary>
        public Carrier Carrier
        {
            get { return level.Carrier; }
        }

        /// <summary>
        /// Pobiera stan samolotu.
        /// </summary>
        public PlaneState PlaneState
        {
            get { return planeState; }
        }

        public PointD Center
        {
            get { return Bounds.Center; }
        }

        public Quadrangle Bounds
        {
            get { return bounds; }
        }

        public PointD MovementVector
        {
            get { return movementVector; }
        }

        public LocationState LocationState
        {
            get { return locationState; }
            set { locationState = value; }
        }

        public WheelsState WheelsState
        {
            get { return wheelsState; }
            set { wheelsState = value; }
        }

        public PointD WheelsPosition
        {
            get
            {
                PointD p =
                    new PointD(Bounds.Center.X + ((int) direction)*wheelDistanceFromCenter, Bounds.Center.Y - height/2);
                p.Rotate(Bounds.Center, Bounds.Angle);
                return p;
            }
        }

        public WeaponManager Weapon
        {
            get { return weaponManager; }
        }

        /// <summary>
        /// Zwraca true jeœli samolot mo¿e poruszaæ siê na lotniskowcu tzw. szybkim ko³owaniem.
        /// Zwraca false w przeciwnym wypadku.
        /// </summary>
        /// <authore>Tomek</authore>
        public bool CanSlowWheeling
        {
            get { return LocationState == LocationState.AircraftCarrier && !IsEngineWorking; }
        }

        /// <summary>
        /// Zwraca true jeœli samolot mo¿e poruszaæ siê na lotniskowcu tzw. wolnym ko³owaniem.
        /// Zwraca false w przeciwnym wypadku.
        /// </summary>
        /// <authore>Tomek</authore>
        public bool CanFastWheeling
        {
            get { return LocationState == LocationState.AircraftCarrier && IsEngineWorking; }
        }

        /// <summary>
        /// Sprawdza czy samolot mo¿e zmieniæ kierunek na lotniskowcu.
        /// </summary>
        /// /// <author>Tomek</author>
        public bool CanChangeDirectionOnAircraft
        {
            get
            {
                if (LocationState == LocationState.AircraftCarrier) return movementVector.X == 0;
                //    return Math.Abs(movementVector.X) <= 0.01f; <- Kamil obczaj co siê dzieje

                
                return false;
            }
        }

        /// <summary>
        /// Sprawdza czy samolot mo¿e rozpocz¹æ spin
        /// </summary>
        /// <author>Adam</author>
        public bool CanSpin
        {
            get
            {
                Console.WriteLine(rotateValue / rotateStep);
                return (Math.Abs(rotateValue / rotateStep) <= 0.15f && wheelsState == WheelsState.In);
            }
        }


       

        /// <summary>
        /// Zwraca k¹t nachylenia samolotu do kierunku wyznaczonego przez wektor ruchu.
        /// Liczony przeciwnie do ruchu wskazówek.
        /// Kat jest wyrazony w radianach i z przedzialu [-pi;pi]
        /// </summary>
        /// <author>Emil</author>
        public float Angle
        {
            get { return bounds.Angle; }
        }

        /// <summary>
        /// Zwraca k¹t nachylenia samolotu do kierunku wyznaczonego przez wektor ruchu. 
        /// K¹t dodatni osznacza wychylenie do góry, a ujemny do do³u.
        /// Kat jest wyrazony w radianach i z przedzialu [-pi;pi]
        /// </summary>
        public float RelativeAngle
        {
            get { return (float) direction*bounds.Angle; }
        }


        /// <summary>
        /// K¹t pod jakim unosi siê / spada samolot: [-pi;pi]
        /// </summary>
        public float ClimbingAngle
        {
            get
            {
                if(RelativeAngle >= 0)
                {
                    if (RelativeAngle > Math.HALF_PI) return Math.PI - RelativeAngle;
                    return RelativeAngle;
                }

                if (RelativeAngle < -Math.HALF_PI) return -(Math.PI + RelativeAngle);
                return RelativeAngle;

               
            }
        }


        public Direction Direction
        {
            get { return direction; }
        }

        public bool IsChangingDirection
        {
            get { return isChangingDirection; }
        }

        /// <summary>
        /// Sprawdza czy samolot ma wystarczaj¹ca wysokoœæ aby wystartowac
        /// </summary>
        public bool HasSpeedToStart
        {
            get { return movementVector.EuclidesLength == maxFastWheelingSpeed; }
        }

        /// <summary>
        /// Zwraca true jeœli ca³y samolot(wszytskie punkty czworokata) 
        /// nie znajduj¹ sie nad lotniskowcem.
        /// </summary>
        public bool IsPlaneNotAboveCarrier
        {
            get
            {
                bool leftFlag = true;
                for (int i = 0; i < Bounds.Peaks.Count; i++)
                {
                    if (Carrier.GetBeginPosition().X <= Bounds.Peaks[i].X)
                    {
                        leftFlag = false;
                        break;
                    }
                }

                bool rightFlag = true;
                for (int i = 0; i < Bounds.Peaks.Count; i++)
                {
                    if (Carrier.GetEndPosition().X + LevelTile.Width >= Bounds.Peaks[i].X)
                    {
                        rightFlag = false;
                        break;
                    }
                }
                return rightFlag || leftFlag;
            }
        }

        /// <summary>
        /// Zwraca true jeœli ca³y samolot(wszytskie punkty czworokata) 
        /// znajduj¹ sie nad lotniskowcem.
        /// </summary>
        public bool IsPlaneAboveCarrier
        {
            get
            {
                for (int i = 0; i < Bounds.Peaks.Count; i++)
                    if (Carrier.GetBeginPosition().X >= Bounds.Peaks[i].X)
                        return false;

                for (int i = 0; i < Bounds.Peaks.Count; i++)
                    if (Bounds.Peaks[i].X >= Carrier.GetEndPosition().X + LevelTile.Width)
                        return false;

                return true;
            }
        }

        /// <summary>
        /// Zwraca true jesli œrodek samolotu znajduje sie nad lotniskowcem
        /// </summary>
        public bool IsCenterAboveCarrier
        {
            get
            {
                return Carrier.GetBeginPosition().X < Bounds.Center.X &&
                       Bounds.Center.X < Carrier.GetEndPosition().X + LevelTile.Width;
            }
        }

        /// <summary>
        /// Zwraca true jesli ko³a samolotu znajduja sie nad lotniskowcem
        /// </summary>
        public bool IsGearAboveCarrier
        {
            get
            {
                return Carrier.GetBeginPosition().X < WheelsPosition.X &&
                       WheelsPosition.X < Carrier.GetEndPosition().X + LevelTile.Width;
            }
        }

        /// <summary>
        /// Okresla czy samolot moze zminiæ amunicjie 
        /// na lotniskowcu.
        /// </summary>
        public bool CanChangeAmmunition
        {
            get
            {
                return locationState == LocationState.AircraftCarrier && MovementVector.X == 0 &&
                       Bounds.Center.X >= level.Carrier.GetRestoreAmunitionPosition().X &&
                       Bounds.Center.X <= level.Carrier.GetRestoreAmunitionPosition().X + LevelTile.Width;
            }
        }

        /// <summary>
        /// Zwraca czy samolot mo¿e wystrzeliæ rakietê.
        /// </summary>
        public bool CanFireRocket
        {
            get
            {
                return
                    planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed &&
                    locationState == LocationState.Air;
            }
        }

        /// <summary>
        /// Zwraca czy samolot mo¿e zrzuciæ bombê.
        /// </summary>
        public bool CanFireBomb
        {
            get
            {
                return
                    planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed &&
                    RelativeAngle < maxFireBombAngle && RelativeAngle > -maxFireBombAngle
                    && locationState == LocationState.Air && !isMaxHeightRotate;
            }
        }

        /// <summary>
        /// Szybkoœæ samolotu.
        /// </summary>
        public float Speed
        {
            get { return movementVector.EuclidesLength; }
            set { movementVector.EuclidesLength = value; }
        }

        /// <summary>
        /// Zwraca czy samolot jest na lotniskowcu ( w stanie zawracania lub zwyk³ym)
        /// </summary>
        public bool IsOnAircraftCarrier
        {
            get { return (locationState == LocationState.AircraftCarrier || locationState == LocationState.CarrierTurningRound); }
        }

        /// <summary>
        /// Zwraca szerokosæ samolotu.
        /// </summary>
        public static float Width
        {
            get { return GameConsts.UserPlane.Width; }
        }

        /// <summary>
        /// Zwraca wysokoœæ samolotu.
        /// </summary>
        public static float Height
        {
            get { return GameConsts.UserPlane.Height; }
        }

        /// <summary>
        /// Zwraca odleg³oœæ (ró¿nicê we wspó³rzêdnych X) samolotu od najbli¿szego samolotu wroga.
        /// Jeœli samolotu wroga nie ma, zwraca -1.
        /// </summary>
        public float XDistanceToClosestPlane()
        {
            float minDist = float.MaxValue;
            float temp;

            for (int i = 0 ; i < level.EnemyPlanes.Count; i++)
            {
                if (Equals(level.EnemyPlanes[i])) continue;
                temp = XDistanceToPlane(level.EnemyPlanes[i]);
                if (temp != -1 && temp < minDist) minDist = temp;
            }

            if (minDist.Equals(float.MaxValue)) return -1;
            return minDist;
        }

        public float YDistanceToClosestPlane()
        {
            float minDist = float.MaxValue;
            float temp;

            for (int i = 0; i < level.EnemyPlanes.Count; i++)
            {
                if (Equals(level.EnemyPlanes[i])) continue;
                temp = YDistanceToPlane(level.EnemyPlanes[i]);
                if (temp != -1 && temp < minDist) minDist = temp;
            }
            if (minDist.Equals(float.MaxValue)) return -1;
            return minDist;
        }

        /// <summary>
        /// Oblicza odleg³oœæ do najbli¿szego samolotu wroga. Zwraca -1 w przypadku gdy nie ma innych wrogów
        /// </summary>
        /// <returns></returns>
        public float DistanceToClosestPlane()
        {
            float minDist = float.MaxValue;
            float temp;
            for (int i = 0; i < level.EnemyPlanes.Count; i++)
            {
                if (Equals(level.EnemyPlanes[i])) continue;
                temp = DistanceToPlane(level.EnemyPlanes[i]);
                if (temp != -1 && temp < minDist) minDist = temp;
            }
            if (minDist.Equals(float.MaxValue)) return -1;
            return minDist;
        }


        public float DistanceToPlane(Plane p)
        {
            float d = XDistanceToPlane(p);
            if (d == -1) return d;
            return YDistanceToPlane(p) + d;
        }

        public float XDistanceToPlane(Plane p)
        {
            return p != null
                       ?
                           (p.PlaneState == PlaneState.Destroyed ||
                            p.PlaneState == PlaneState.Crashed)
                               ? -1
                               :
                                   System.Math.Abs(Center.X - p.Center.X)
                       : -1;
        }

        public float YDistanceToPlane(Plane p)
        {
            return p != null
                       ?
                           (p.PlaneState == PlaneState.Destroyed ||
                            p.PlaneState == PlaneState.Crashed)
                               ? -1
                               :
                                   System.Math.Abs(Center.Y - p.Center.Y)
                       : -1;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Inicjuje pola samolotu odpowiednimi wartoœciami.
        /// Wywo³ywana w konstruktorze oraz przy nowym rzyciu.
        /// </summary>
        public void Init()
        {
            motorState = EngineState.SwitchedOff;
            planeState = PlaneState.Intact;
            wheelsState = WheelsState.Out;
            landingState = LandingState.None;
            locationState = LocationState.AircraftCarrier;

            oil = maxOil;
            petrol = maxPetrol;
            IsEngineJustStarted = false;
            IsEngineJustStopped = false;
            counterStartedEngine = 0;

            breakingEndCarrierTile = null;

            direction = Direction.Left;
            bounds = new Quadrangle(GetStartingPosition(), width, height);
            movementVector = new PointD(0, 0);
            Rotate(angleOnCarrier*(float) direction);
            isChangingDirection = false;

            UnblockAllInput();
            ResetInputFlags();

            isLanding = false;
            isAfterFlyingDown = false;
            isMaxHeightRotate = false;
            speedBeforeMaxHeightRotata = 0;

            //dodane przez Emila
            rotateValue = 0;

            isSinking = false;
            airscrewSpeed = 0;
            turningTime = 0;
            turningTimeLeft = 0;
            turningVector = new PointD(0, 0);
            wreckTimeElapsed = 0;

            isFallingFromCarrier = false;
            isSlippingFromCarrier = false;
            isFallingAfterCrash = false;

            isRaisingTail = false;
            isLoweringTail = false;

            breakingEndPositionX = Carrier.GetRestoreAmunitionPosition().X + LevelTile.Width;

            spinned = false;
        }

        /// <summary>
        /// Zatrzymuje samolot. 
        /// Zeruje wektor ruchu.
        /// </summary>
        public void StopPlane()
        {
            movementVector.X = 0;
            movementVector.Y = 0;
        }

        /// <summary>
        /// Wlacza silnik.
        /// </summary>
        public void StartEngine()
        {
            motorState = EngineState.Working;
            airscrewSpeed = minAirscrewSpeed;
        }

        /// <summary>
        /// Wylacza silnik.
        /// </summary>
        public void StopEngine(float scaleFactor)
        {
            if (locationState == LocationState.AircraftCarrier && landingState == LandingState.None)
            {
                if (movementVector.EuclidesLength >= changeWheelsSpeed || isLoweringTail)
                    //jesli podwozie zadarte to je opuœæ
                    if (scaleFactor >=0)
                        lowerTailStep(scaleFactor);
            }
            if (motorState == EngineState.Working)
            {
                motorState = EngineState.SwitchedOff;
                airscrewSpeed = 0;
                if (!isEnemy) //wy³¹czam dŸwiêk tylko dla samolotu gracza
                    level.Controller.OnTurnOffEngine();
            }
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k¹t.
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(float angle)
        {
            Bounds.Rotate(angle);
            movementVector.Rotate(new PointD(0, 0), angle);
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k¹t ale zawsze w górê.
        /// </summary>
        /// <param name="angle"></param>
        public void RotateUp(float angle)
        {
            Rotate((float) direction*angle);
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k¹t ale zawsze w dó³.
        /// </summary>
        /// <param name="angle"></param>
        public void RotateDown(float angle)
        {
            Rotate(-1.0f*(float) direction*angle);
        }

        /// <summary>
        /// Przesuwa samolot zgodnie z aktualnym wektorem, przetwarzaj¹c sygna³y z klawiatury.
        /// </summary>
        /// <param name="time">Czas jaki up³yn¹³ od ostatniego wywo³ania Move. Wyra¿ony w ms.</param>
        /// <param name="timeUnit">Wartoœæ czasu do której odnoszone s¹ wektor ruchu i wartoœæ obrotu. Wyra¿ona w ms.</param>
        public virtual void Move(float time, float timeUnit)
        {
            //Console.WriteLine(Speed);
            float scaleFactor = time/timeUnit;
            if (planeState == PlaneState.Crashed)
            {
                MoveAfterCrash(time, timeUnit);
                return;
            }

            ProcessInput(time, timeUnit);
    
            //odjêcie benzyny i ewentualnie oleju
            if (this.motorState == EngineState.Working && (locationState == LocationState.Air || locationState == LocationState.AirTurningRound))
                petrol -= scaleFactor*movementVector.EuclidesLength/GameConsts.UserPlane.MaxSpeed*
                          GameConsts.UserPlane.PetrolLoss;


            petrol = System.Math.Max(petrol, 0);
            if (planeState == PlaneState.Damaged && !IsOnAircraftCarrier)
                oil -= scaleFactor*GameConsts.UserPlane.OilLoss;
            oil = System.Math.Max(oil, 0);

            if (GameConsts.UserPlane.GodMode == 0 && planeState != PlaneState.Destroyed &&
                planeState != PlaneState.Crashed)
            {
                if (petrol == 0 || oil == 0)
                    OutOfPetrolOrOil(scaleFactor);
            }
            if (motorState == EngineState.Working)
            {
                //zmiana wektora ruchu przy zawracaniu
                if (planeState != PlaneState.Destroyed && locationState == LocationState.AirTurningRound &&
                    isChangingDirection)
                {
                    turningTimeLeft -= time;
                    if (!(Speed >= minFlyingSpeed && movementVector.SignX != turningVector.SignX))
                    {
                        movementVector = -Math.Cos(turningTimeLeft / turningTime * Math.PI) * turningVector;
                    }
                }
            }

            //czy ma w³¹czony silnik, jeœli nie to obrót samolotu, tak ¿eby spada³
            if (!IsOnAircraftCarrier && planeState != PlaneState.Crashed && motorState == EngineState.SwitchedOff)
                FallDown(time, timeUnit);

            if (IsEngineWorking)
                airscrewSpeed = minAirscrewSpeed + (int) Math.Abs((int) (15f*movementVector.X));

            //opuszczenie lotniskowca (START)
            LeaveCarrier(time, timeUnit);

            //proces l¹dowania
            LandingProcess(time, timeUnit);

            //ograniczenia
            VerticalBoundsLimit(time, timeUnit);
            HeightLimit(time, timeUnit);

            if (!isLanding && !isAfterFlyingDown || motorState == EngineState.SwitchedOff ||
                planeState == PlaneState.Destroyed)
            {
                if (locationState == LocationState.Air)
                {
                    float sin = Math.Sin(RelativeAngle);

                    if (sin > 0) sin *= -sin*17;
                    else         sin *=  sin*30;

                    float liftVectorY = 0.7f*(1 - sin);
                    bounds.Move(0, liftVectorY*scaleFactor);

                    //Grawitacja
                    bounds.Move(0, (-1.0f)*scaleFactor);
                }

                bounds.Move(scaleFactor * movementVector); 
                UpdatePlaneAngle(scaleFactor);
            }
            else if (isLanding) //schodzenie do l¹dowania
            {
                isLanding = false;
                PointD landingVector = new PointD(movementVector.X, -GameConsts.UserPlane.LandingSpeed);
                bounds.Move(scaleFactor*landingVector);
                isAfterFlyingDown = true;
            }
            else //jest w fazie po schodzeniu w dó³ - ma lecieæ równolegle do pod³o¿a
            {
                PointD tempVector = new PointD(movementVector.X, 0);
                bounds.Move(scaleFactor*tempVector);
            }
        }

        /// <summary>
        /// Porusza samolotem.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="scaleFactor"></param>
        /// <author>Tomek , Kamil S³awiñski</author>
        public void Steer(Direction direction, float scaleFactor)
        {
            switch (locationState)
            {
                case LocationState.AircraftCarrier:
                    if (landingState == LandingState.None)
                    {
                        if (this.direction != direction)
                        {
                            if (CanChangeDirectionOnAircraft)
                            {
                               // movementVector.X = 0;
                               // movementVector.Y = 0;
                                TurnRound(direction, TurnType.Carrier);
                            }
                            //hamowanie samolotu (MOCNE)
                            float subSpeed = GameConsts.UserPlane.BreakingPower*scaleFactor*
                                             GameConsts.UserPlane.MoveStep;
                            float oldSpeed = movementVector.EuclidesLength;
                            float newSpeed = movementVector.EuclidesLength - subSpeed;

                            subSpeedToMin(subSpeed, 0);
                            changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor);
                        }
                        else
                        {
                            if (CanSlowWheeling)
                            {
                                if(movementVector.EuclidesLength <= slowWheelingSpeed)
                                {
                                    movementVector = new PointD((float)direction * slowWheelingSpeed, 0);
                                }
                                else
                                {
                                    //hamowanie samolotu (S£ABE)
                                    float subSpeed = scaleFactor * GameConsts.UserPlane.MoveStep;
                                    float oldSpeed = movementVector.EuclidesLength;
                                    float newSpeed = movementVector.EuclidesLength - subSpeed;

                                    subSpeedToMin(subSpeed, 0);
                                    changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor);
                                }
                            }
                            else if (CanFastWheeling)
                            {
                                float addSpeed = scaleFactor*GameConsts.UserPlane.MoveStep*0.75f; // todo: przeniesc 0.75 do sta³ej
                                float oldSpeed = movementVector.EuclidesLength;
                                float newSpeed = movementVector.EuclidesLength + addSpeed;

                                if (movementVector.EuclidesLength == 0)
                                    movementVector.X = (float) direction*addSpeed; //ruszenie samolotem

                                changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor); //zmiana nachylenia dziobu
                                addSpeedToMax(addSpeed, maxFastWheelingSpeed); //przyspieszanie do maxymalnej
                            }
                        }
                    }
                    break;
                case LocationState.Air:
                    if (this.direction == direction)
                    {
                        float addSpeed = scaleFactor*GameConsts.UserPlane.MoveStep;
                        if (wheelsState == WheelsState.In || wheelsState == WheelsState.TogglingIn)
                            addSpeedToMax(addSpeed, GameConsts.UserPlane.MaxSpeed); //przyspieszanie do maxymalnej
                        else
                            addSpeedToMax(addSpeed, maxWheelOutSpeed);
                    }
                    else //kierunek przeciwny do kierunku lotu
                        if (System.Math.Abs(Angle) < maxTurningAngle) //sprawdzam czy mo¿e zawróciæ
                        {
                            TurnRound(direction, TurnType.Airborne);
                        }
                    break;
            }
        }

        /// <summary>
        /// Powoduje rozpoczêcie obrotu samolotu (z 'pleców' na 'brzuch').
        /// </summary>
        protected void Spin()
        {
            if (planeState != PlaneState.Crashed)
            {
              //  rotateValue = 0; //hamujê zmianê k¹ta
                isChangingDirection = true;
                isBlockSpin = true;
                BlockMovementInput();
                level.Controller.OnSpinBegin(this);
               
            }
        }

        /// <summary>
        /// Powoduje rozpoczêcie zawracania.
        /// </summary>
        /// <param name="newDirection">Kierunenk lotu po zawróceniu.</param>
        /// <param name="turnType">Rodzaj zawracania.</param>
        protected void TurnRound(Direction newDirection, TurnType turnType)
        {
            if (planeState != PlaneState.Crashed)
            {
                if (turnType == TurnType.Airborne)
                    locationState = LocationState.AirTurningRound;
                else
                    locationState = LocationState.CarrierTurningRound;

                isAfterFlyingDown = false;
                //jeœli zawraca przy jednoczesnym l¹dowaniu, to powinien lecieæ zgodnie z kierunkiem nachylenia dzioba a nie prosto
                rotateValue = 0; //hamujê zmianê k¹ta
                level.Controller.OnPrepareChangeDirection(newDirection, this, turnType);
            }
        }

        /// <summary>
        /// Odejmuje pewna ilosc oleju.
        /// Funkcja jest wywolywana po trafieniu pocisku w samolot.
        /// </summary>
        public void OilSubtraction()
        {
            //ilosc oleju jaka zostanie odjeta po trafieniu.
            //wartosc zostanie wyznaczona eksperymentalnie.
            oil -= oilLeak;
        }

        /// <summary>
        /// Nape³nia olej.
        /// </summary>
        public void OilRefuel()
        {
            oil = maxOil;
        }

        /// <summary>
        /// Nape³nia bêzynê.
        /// </summary>
        public void PetrolRefuel()
        {
            petrol = maxPetrol;
        }

        /// <summary>
        /// Naprawa samolotu.
        /// </summary>
        public void RepairPlane()
        {
            planeState = PlaneState.Intact;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Ilosc oleju: " + oil);
            builder.AppendLine("Pozycja: " + Center);

            return builder.ToString();
        }

        /// <summary>
        /// Ustawia dan¹ flagê wciœniêtego przycisku klawiatury.
        /// </summary>
        /// <param name="flag">Okreœla któr¹ flagê ustawiæ.</param>
        public void SetInputFlag(InputFlag flag)
        {
            switch (flag)
            {
                case InputFlag.Up:
                    isUpPressed = true;
                    break;
                case InputFlag.Down:
                    isDownPressed = true;
                    break;
                case InputFlag.Left:
                    isLeftPressed = true;
                    break;
                case InputFlag.Right:
                    isRightPressed = true;
                    break;
                case InputFlag.EngineOn:
                    isEngineKeyPressed = true;
                    break;
                case InputFlag.Spin:
                    isSpinPressed = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Informuje samolot, ¿e nale¿y zacz¹c zawracanie.
        /// </summary>
        /// <param name="turningTime">Ile czasu bêdzie trwa³o zawracanie. Mierzony w SEKUNDACH.</param>
        public void OnPrepareChangeDirectionEnd(float turningTime)
        {
            if (planeState != PlaneState.Crashed)
            {
                this.turningTime = turningTime*1000;
                turningTimeLeft = turningTime*1000;
                isChangingDirection = true;
                turningVector = (PointD) movementVector.Clone();
            }
        }

        /// <summary>
        /// Informuje samolot, ¿e zawracanie siê skoñczy³o i nale¿y zmieniæ kierunek lotu.
        /// </summary>
        public void OnChangeDirectionEnd(TurnType type)
        {
            if (planeState != PlaneState.Crashed)
            {
                if (type == TurnType.Airborne)
                {
                    direction = (Direction) (-1*(int) direction);
                    locationState = LocationState.Air;
                    isChangingDirection = false;
                    Speed = minFlyingSpeed; //zmini³em Tomek
                    //movementVector = -1 * turningVector;
                    //blockAllInput = false;
                    spinned = false;
                }
                if (type == TurnType.Carrier)
                {
                    direction = (Direction) (-1*(int) direction);
                    //this.bounds.Rotate(angleOnCarrier);
                    Rotate(2*angleOnCarrier*(float) direction); //((NA LOTNISKOWCU))
                    isChangingDirection = false;
                    locationState = LocationState.AircraftCarrier;
                }
            }
        }

        private void HorizontalReflection()
        {
            bounds.HorizontalReflection();
            direction = (Direction)(-1 * (int)direction);
            spinned = !spinned;
        }

        /// <summary>
        /// Informuje samolot, ¿e obracanie siê skoñczy³o i nale¿y zmieniæ kierunek lotu.
        /// </summary>
        public void OnSpinEnd()
        {
            if (planeState != PlaneState.Crashed)
            {
                HorizontalReflection();
                isChangingDirection = false;
                isBlockSpin = false;
                UnblockMovementInput();
            }
        }

        /// <summary>
        /// Metoda jest wywolywana przez kontroler w momencie, kiedy 
        /// uzytkownik wcisnie przycisk zmiany stanu podwozia. Jezeli
        /// samolot aktualnie nie chowa ani nie otwiera podwozia, stan
        /// podwozia jest zmieniany na TogglingIn (chowanie), 
        /// TogglingOut (otwieranie).
        /// </summary>
        /// <author>Jakub Tezycki</author>
        public void ToggleGear()
        {
            if (wheelsState == WheelsState.In)
            {
                wheelsState = WheelsState.TogglingOut;
            }
            else if (wheelsState == WheelsState.Out)
            {
                wheelsState = WheelsState.TogglingIn;
                isAfterFlyingDown = false; //dodane przez Emila
            }
        }

        /// <summary>
        /// Metoda sprawdza, czy w aktualnym stanie samolot moze 
        /// zmienic stan podwozia
        /// </summary>
        /// <returns></returns>
        public Boolean CanPlaneToggleGear()
        {
            return (planeState != PlaneState.Crashed &&
                    locationState != LocationState.AircraftCarrier &&
                    locationState != LocationState.CarrierTurningRound &&
                    landingState == LandingState.None &&
                    wheelsState != WheelsState.TogglingIn &&
                    wheelsState != WheelsState.TogglingOut &&
                    //dodane przez Emila
                    planeState != PlaneState.Destroyed &&
                    (
                        wheelsState == WheelsState.In &&
                        RelativeAngle <= maxWheelOutAngle &&
                        RelativeAngle >= -maxWheelOutAngle ||
                        wheelsState != WheelsState.In
                    )
                   );
        }

        /// <summary>
        /// Metoda zmienia stan podwozia z przejsciowego (TogglingIn/Out)
        /// na ostateczny (In/Out).
        /// </summary>
        public void GearToggled()
        {
            WheelsState state = wheelsState;
            if (state == WheelsState.TogglingIn)
            {
                wheelsState = WheelsState.In;
            }
            else if (state == WheelsState.TogglingOut)
            {
                wheelsState = WheelsState.Out;
            }
        }

        /// <summary>
        /// Powoduje rozbicie samolotu o teren.
        /// </summary>
        /// <param name="terrainHeight">Wysokoœæ terenu o który siê rozbi³.</param>
        /// <param name="tileKind">Rodzaj terenu o który siê rozbi³.</param>
        public void Crash(float terrainHeight, TileKind tileKind)
        {
            float viewDifferance = 1; //wartoœæ o jak¹ trzeba przesun¹æ samolot po rozbiciu, ¿eby nie by³ nda ziemi¹
            if (planeState != PlaneState.Crashed)
            {
                locationState = LocationState.Air;
                //potrzebne, ¿eby obrót samolotu dzia³a³ poprawnie - spytaæ siê Adama dlaczego tak jest
                if (planeState != PlaneState.Destroyed)
                    //jeœli nie by³ wczeœniej Destroyed, to wysy³am komunikat, ¿eby by³a dobra animacja w View
                    level.Controller.OnPlaneDestroyed(this);
                planeState = PlaneState.Crashed;
                //odwrócenie samolotu do poziomu
                if (tileKind != TileKind.Ocean)
                {
                    float tempAngle;
                    if (RelativeAngle >= -Math.PI/2.0f && RelativeAngle <= Math.PI/2.0f)
                        tempAngle = -Angle; //zwyk³e po³o¿enie w poziomie
                    else
                        tempAngle = Math.PI - Angle; //samolot do góry nogami
                    Rotate(tempAngle);

                    //sprawdza pozycje zderzenia z ziemia.
                    DestroyBunkersAndKillSoldiers();
                }
                StopEngine(-1);
                //wyzerowanie czasu do koñca ¿ycia
                wreckTimeElapsed = 0;

                isSinking = (tileKind == TileKind.Ocean);
                if (!isSinking)
                {
                    movementVector = new PointD(0, 0);
                }

                isFallingAfterCrash = (tileKind == TileKind.AircraftCarrier && !IsCenterAboveCarrier);
                if (isFallingAfterCrash)
                {
                    movementVector.X = 0;
                    movementVector.Y = (movementVector.Y >= 0) ? 0 : movementVector.Y;
                }
                else
                    bounds.Move(0, terrainHeight - bounds.LowestY - viewDifferance);
                //zni¿enie samolotu na poziom tile'a
                level.Controller.OnPlaneCrashed(this, tileKind);
            }
        }

        /// <summary>
        /// Powoduje zniszczenie samolotu - wy³¹czenie silnika i utratê kontroli.
        /// </summary>
        public void Destroy()
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                planeState = PlaneState.Destroyed;
                StopEngine(-1);
                rotateValue = 0;
                level.Controller.OnPlaneDestroyed(this);

                if (IsOnAircraftCarrier)
                {
                    Crash(Carrier.Height, TileKind.AircraftCarrier);
                }
            }
        }

        public void Hit(float oilTaken, float oilLeak)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                this.oilLeak += oilLeak;
                oil -= oilTaken;
                oil = System.Math.Max(oil, 0);
            }
        }

        /// <summary>
        /// Metoda wywo³ywana w momencie trafienia samolotu przez wroga.
        /// </summary>
        /// <param name="hitByPlane">Jeœli true, to znaczy ¿e zosta³ trafioy przez samolot, 
        /// w przeciwnym przypadku, zosta³ trafiony przez dzia³o na wyspie.</param>
        public void Hit(bool hitByPlane)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                planeState = PlaneState.Damaged;
                if (hitByPlane) //ma³e trafienie
                    oil -= GameConsts.UserPlane.HitCoefficient;
                else
                {
                    oilLeak += GameConsts.UserPlane.HitCoefficient;
                    oil -= oilLeak;
                }
                oil = System.Math.Max(oil, 0);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Funkcja sprawdza z czym zderzyl sie samolot podczas spadania.
        /// Jesli zderzyl sie z bunkrem lub barakiem to go niszczy.
        /// Zabija wszystkich zolnierzy na sasiednich tailach.
        /// </summary>
        private void DestroyBunkersAndKillSoldiers()
        {
            LevelTile crashTile = level.GetTileForPosition(Position.X);
            if (crashTile != null)
            {
                if (crashTile is EnemyInstallationTile)
                {
                    EnemyInstallationTile enemyTile = crashTile as EnemyInstallationTile;
                    Rocket rocket = new Rocket(Position.X, 0, new PointD(), level, 0, this);
                    if (!enemyTile.IsDestroyed)
                    {
                        level.Controller.OnTileDestroyed(crashTile, rocket);
                        enemyTile.Destroy();
                    }
                    else
                        level.Controller.OnTileBombed(crashTile, rocket);
                    level.Controller.OnUnregisterRocket(rocket);
                }
                if (!(crashTile is OceanTile) && !(crashTile is AircraftCarrierTile))
                    level.KillVulnerableSoldiers(Convert.ToInt32(System.Math.Floor(Position.X/LevelTile.Width)), 2);
            }
        }

        /// <summary>
        /// Funkcja odbiera zdarzenie od WeaponManager-a i przesyla go do 
        /// modelu.
        /// </summary>
        /// <param name="ammunition">Nowy pocisk do zarejestrowania.</param>
        private void weaponManager_RegisterWeaponToModelEvent(Ammunition ammunition)
        {
            RegisterWeaponEvent(ammunition);
        }

        /// <summary>
        /// Resetuje flagi wciœniêtych przycisków.
        /// </summary>
        private void ResetInputFlags()
        {
            isLeftPressed = false;
            isRightPressed = false;
            isUpPressed = false;
            isDownPressed = false;
            isEngineKeyPressed = false;
            isSpinPressed = false;
        }

        /// <summary>
        /// Odblokowywuje wszystkie flagi wejœciowe.
        /// </summary>
        private void UnblockAllInput()
        {
            isBlockLeft = false;
            isBlockRight = false;
            isBlockUp = false;
            isBlockDown = false;
            isBlockFireGun = false;
            isBlockFireRocket = false;
            isBlockEngine = false;
            isBlockSpin = false;
        }

        /// <summary>
        /// Blokowywuje wszystkie flagi ruchu.
        /// </summary>
        private void BlockMovementInput()
        {
            isBlockLeft = true;
            isBlockRight = true;
            isBlockUp = true;
            isBlockDown = true;
            isBlockSpin = true;
        }

        /// <summary>
        /// Odblokowywuje wszystkie flagi ruchu.
        /// </summary>
        private void UnblockMovementInput()
        {
            isBlockLeft = false;
            isBlockRight = false;
            isBlockUp = false;
            isBlockDown = false;
            isBlockSpin = false;
        }

        /// <summary>
        /// Przetwarza sygna³y z klawiatury, które pojawi³y siê od ostatniego wywo³ania ProcessInput.
        /// </summary>
        /// <param name="time">Czas jaki up³yn¹³ od ostatniego wywo³ania ProcessInput. Wyra¿ony w ms.</param>
        /// <param name="timeUnit">Wartoœæ czasu do której odnoszone s¹ wektor ruchu i wartoœæ obrotu. Wyra¿ona w ms.</param>       
        private void ProcessInput(float time, float timeUnit)
        {
            if (planeState == PlaneState.Destroyed && !IsOnAircraftCarrier)
            {
                ResetInputFlags();
                return;
            }
            float scaleFactor = time/timeUnit;

            //ZMIANA STANY SILNIKA
            if (isEngineKeyPressed)
            {
                if (!IsEngineWorking)
                    TryToStartEngine(time);
                else
                    TryToStopEngine(scaleFactor);
            }
            else
                ResetEngineParameters();

            switch (locationState)
            {
                case LocationState.Air:
                    if (motorState == EngineState.SwitchedOff)
                        break;
                    if (isLeftPressed || isRightPressed)
                    {
                        Direction steerDir = isLeftPressed ? Direction.Left : Direction.Right;
                        Steer(steerDir, scaleFactor);
                    }
                    else
                    {
                        //hamowanie samolotu
                        float subSpeed = scaleFactor*GameConsts.UserPlane.MoveStep;
                        subSpeedToMin(subSpeed, minFlyingSpeed);
                    }
                    if (isUpPressed)
                    {
                       
                        if (wheelsState == WheelsState.In || wheelsState == WheelsState.TogglingIn ||
                            RelativeAngle < landingAngle ||
                            ((isLeftPressed || isRightPressed) && RelativeAngle < maxWheelOutAngle))
                        {
                            IncreaseRotateValue(scaleFactor*(float) direction*rotateStep); //zwyk³y obrót
                        }
                        else if (!isLeftPressed && !isRightPressed)
                            //je¿eli ¿aden przycisk kierunku nie jest wciœniêty, samolot powinien obni¿yæ lot
                        {
                            isLanding = true;
                            rotateValue = 0; //samolot nie powinien siê dalej obracaæ
                            if (RelativeAngle > landingAngle)
                            {
                                float tempAngle = - 1.0f*(float) direction*(RelativeAngle - landingAngle);
                                Rotate(tempAngle);
                            }
                        }
                        isAfterFlyingDown = false;
                    }
                    if (isDownPressed)
                    {
                     
                        if (wheelsState == WheelsState.In || RelativeAngle > -maxWheelOutAngle)
                        {
                            IncreaseRotateValue(-scaleFactor*(float) direction*rotateStep);
                        }
                        isAfterFlyingDown = false;
                    }
                    if (!isDownPressed && !isUpPressed)//"hamowanie" obrotu
                        DecreaseRotateValue(rotateBrakingFactor*scaleFactor*rotateStep);
                                      
                    if (isSpinPressed && !isBlockSpin && CanSpin)
                    {
                        Spin();
                    }
                    break;

                case LocationState.AircraftCarrier:
                    if (landingState == LandingState.None)
                    {
                        if (isLeftPressed || isRightPressed)
                        {
                            Direction steerDir = isLeftPressed ? Direction.Left : Direction.Right;
                            Steer(steerDir, scaleFactor);
                        }
                        else //hamowanie samolotu (S£ABE)
                        {
                            float subSpeed = scaleFactor*GameConsts.UserPlane.MoveStep;
                            float oldSpeed = movementVector.EuclidesLength;
                            float newSpeed = movementVector.EuclidesLength - subSpeed;

                            subSpeedToMin(subSpeed, 0);
                            changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor);
                        }
                    }
                    break;
            }
            //zawsze resetujê wszystkie flagi !!!
            ResetInputFlags();
        }

        /// <summary>
        /// Metoda zmienia pochylenie dziobu samolotu na lotniskowcu 
        /// gdy osi¹gn¹³ odpowiedni¹ prêdkoœæ.
        /// </summary>
        /// <param name="oldSpeed"></param>
        /// <param name="newSpeed"></param>
        /// <author>Tomek , Kamil S³awiñski</author>
        private void changeAngleWhileWheeling(float oldSpeed, float newSpeed , float scaleFactor)
        {
            if (CanFastWheeling)
            {
                //przyspieszy³
                if (oldSpeed < changeWheelsSpeed && changeWheelsSpeed <= newSpeed || isRaisingTail)
                {
                    raiseTailStep(scaleFactor);
                }
                    //zwolni³
                else if (newSpeed <= changeWheelsSpeed && changeWheelsSpeed < oldSpeed || isLoweringTail)
                    lowerTailStep(scaleFactor);
            }
            //zwolni³ bo wy³¹czony silnik
            else if (newSpeed <= changeWheelsSpeed && changeWheelsSpeed < oldSpeed || isLoweringTail)
                lowerTailStep(scaleFactor);
        }

        /// <summary>
        /// Metoda obni¿a ogon samolotu na lotniskowcu 
        /// Dzieje siê to p³ynnie , gdy w³¹czona jest flaga isLoweringTail
        /// </summary>
        /// <author>Tomek , Kamil S³awiñski</author>
        private void lowerTailStep(float scaleFactor)
        {
            isLoweringTail = true;
            isRaisingTail = false;
            bounds.Rotate((float) (direction)*(angleOnCarrier * 6) * scaleFactor);
            if (Math.Abs(Math.Abs(bounds.Angle) - angleOnCarrier) < 0.01)
            {
                isLoweringTail = false;
            }
        }

        /// <summary>
        /// Metoda podnosi ogon samolotu na lotniskowcu 
        /// Dzieje siê to p³ynnie , gdy w³¹czona jest flaga isRaisingTail
        /// </summary>
        /// <author>Tomek , Kamil S³awiñski</author>
        private void raiseTailStep(float scaleFactor)
        {
            isRaisingTail = true;
            isLoweringTail = false;
            bounds.Rotate(-(float)(direction) * (angleOnCarrier * 4) * scaleFactor);
            if (Math.Abs(bounds.Angle) < 0.01)
            {
                isRaisingTail = false;
                AirstripCotact();
            }
        }

        /// <summary>
        /// Dodaje prêdkoœci.
        /// Jesli dodana wieksza od maksymalnej ustawia maksymaln¹.
        /// </summary>
        /// <param name="addSpeed"></param>
        /// <param name="maxSpeed"></param>
        /// <author>Tomek</author>
        private void addSpeedToMax(float addSpeed, float maxSpeed)
        {
            //sprawdzam prêdkoœæ, ¿eby nie wzros³a powy¿ej maxymalnej
            if (movementVector.EuclidesLength + addSpeed > maxSpeed)
                addSpeed = maxSpeed - movementVector.EuclidesLength;
            movementVector.Extend(addSpeed);
            UpdateAirscrewSpeed();
        }

        /// <summary>
        /// Odejmuje prêdkoœæi. Jeœ³i nowa prekoœæ mniejsza od minimalnej ustawia minimalna.
        /// </summary>
        /// <param name="subSpeed"></param>
        /// <param name="minSpeed"></param>
        /// <author>Tomek</author>
        private void subSpeedToMin(float subSpeed, float minSpeed)
        {
            //sprawdzam prêdkoœæ, ¿eby nie spad³a poni¿ej minimalnej
            if (movementVector.EuclidesLength - subSpeed < minSpeed)
                movementVector.EuclidesLength = minSpeed;
            else
                movementVector.Extend(-subSpeed);
            UpdateAirscrewSpeed();
        }

        /// <summary>
        /// Spadanie/szybowanie samolotu 
        /// </summary>
        /// <param name="time">Czas jaki up³yn¹³ od ostatniego wywo³ania ProcessInput. Wyra¿ony w ms.</param>
        /// <param name="timeUnit">Wartoœæ czasu do której odnoszone s¹ wektor ruchu i wartoœæ obrotu. Wyra¿ona w ms.</param>
        protected void FallDown(float time, float timeUnit)
        {
            bool isSliding = (locationState == LocationState.Air && planeState != PlaneState.Crashed && planeState != PlaneState.Destroyed && motorState == EngineState.SwitchedOff); // samolot moze spadac zniszczony albo szybowaæ po wy³¹czeniu silnika
            rotateValue = 0;
            float scaleFactor = time/timeUnit;
            float oldAngle = movementVector.Angle;
            movementVector.Y -= isSliding ?  gravitationalAcceleration * scaleFactor / 8 : gravitationalAcceleration * scaleFactor;
       
            // szybszy spadek wektora jeœli dziob jest do gory
            if (isSliding) movementVector.Y -= 0.2f * ClimbingAngle / Math.PI * gravitationalAcceleration * scaleFactor;

            // spowalniamy
            if (isSliding && ClimbingAngle > 0) movementVector.X -= 0.10f * movementVector.X * scaleFactor * ClimbingAngle / Math.PI;

            


            float newAngle = movementVector.Angle;
            float rot = (newAngle - oldAngle);
            bounds.Rotate(rot);
        }

        /// <summary>
        /// Sprawdza czy któryœ z wierzcho³ków ma Y mniejszy lub równy 0.
        /// </summary>
        /// <returns></returns>
        private bool isUnderWater()
        {
            for (int i = 0; i < bounds.Peaks.Count; i++)
                if (bounds.Peaks[i].Y <= 0)
                    return true;
            return false;
        }

        /// <summary>
        /// Próba w³¹czenia silnika
        /// </summary>
        /// <param name="time"></param>
        private void TryToStartEngine(float time)
        {
            if (!IsEngineJustStopped)
            {
                if (CanEngineBeStarted)
                {
                    StartEngineCounter = 0; //zeruje licznik.
                    StartEngine(); //uruchamiam silnik.
                    IsEngineJustStarted = true;
                    level.Controller.OnTurnOnEngine();
                }
                else
                {
                    StartEngineCounter += (int) time; //zwiekszam licznik prob odpalenia silnika.
                    level.Controller.OnStartEngineFailed();
                }
            }
        }

        /// <summary>
        /// Próba wy³¹czenia silnika.
        /// (nie mozna wy³¹czyæ silnika zaraz po uruchomieniu)
        /// </summary>
        private void TryToStopEngine(float scaleFactor)
        {
            if (!IsEngineJustStarted)
            {
                StopEngine(scaleFactor);
                IsEngineJustStopped = true;
            }
        }

        /// <summary>
        /// ustawia parametry silnika na stan pocz¹tkowy.
        /// </summary>
        private void ResetEngineParameters()
        {
            IsEngineJustStarted = false;
            IsEngineJustStopped = false;
            counterStartedEngine = 0;
        }

        /// <summary>
        /// Zwraca startow¹ pozycje samolotu.
        /// </summary>
        private PointD GetStartingPosition()
        {
            return Carrier.GetRestoreAmunitionPosition();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void UpdateAirscrewSpeed()
        {
            if (motorState == EngineState.Working)
                airscrewSpeed =
                    movementVector.EuclidesLength/GameConsts.UserPlane.MaxSpeed*(maxAirscrewSpeed - minAirscrewSpeed) +
                    minAirscrewSpeed;
            else
                airscrewSpeed = 0.0f;
        }

        /// <summary>
        /// Powoduje wy³¹czenie silinika i utratê kontroli po tym jak skoñczy siê olej lub paliwo.
        /// </summary>
        protected void OutOfPetrolOrOil(float scaleFactor)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                planeState = PlaneState.Destroyed;
                StopEngine(scaleFactor);
                rotateValue = 0;
            }
        }

        #region Landing Methods

        /// <summary>
        /// Proces l¹dowania na lotniskowcu.
        /// Ustawia samolot gdy podchodzi do l¹dowania.
        /// Odpowiada za ko³owanie na lotniskowcu.
        /// Hamowanie przez linê chamuj¹c¹
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        public void LandingProcess(float time, float timeUnit)
        {
            //SAMOLOT PODCHODZI DO L¥DOWANIA
            if (locationState == LocationState.Air &&
                wheelsState == WheelsState.Out &&
                IsCenterAboveCarrier &&
                Bounds.LowestY <= level.Carrier.Height + 1 &&
                0 <= ((int) direction)*bounds.Angle && ((int) direction)*bounds.Angle <= landingAngle + 0.01)
            {
                locationState = LocationState.AircraftCarrier;
                landingState = LandingState.InitWheeling;
                level.Controller.OnTouchDown();
            }

            //ZMIANA KONTA NA ZEROWY
            if (landingState == LandingState.InitWheeling)
                InitLandingWheeling(time, timeUnit);

            //KO£OWANIE PO LOTNISKOWCU
            if (landingState == LandingState.Wheeling)
                LandingWeeling();

            //HAMOWANIE PRZEZS LINÊ
            if (landingState == LandingState.Breaking)
                LandingBreaking(time, timeUnit);
        }

        /// <summary>
        /// Inisjalizuje ko³owanie w czasie l¹dowania.
        /// ustawia samolotu na wzd³u¿ lotniskowca.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void InitLandingWheeling(float time, float timeUnit)
        {
            float scaleFactor = time/timeUnit;
            AirstripCotact();
            BlockMovementInput();
            rotateValue = 0;

            if (System.Math.Abs(Bounds.Angle) <= scaleFactor*rotateStep)
            {
                Bounds.Rotate((-1)*Bounds.Angle);
                landingState = LandingState.Wheeling;
            }
            else
                Bounds.Rotate((-1)*(System.Math.Sign(Bounds.Angle))*scaleFactor*rotateStep); //zmieniæ na sta³¹ 
        }


        /// <summary>
        /// Ko³owanie po lotniskowcu w czasie lotu.
        /// </summary>
        private void LandingWeeling()
        {
            AirstripCotact();
            breakingEndCarrierTile = Carrier.IsOnEndCarrier(Bounds.Center);
            if (direction == Direction.Left && breakingEndCarrierTile != null)
            {
                level.Controller.OnCatchPlane(this, breakingEndCarrierTile);
                lineCatchSpeed = Speed;
                catchLinePositionX = Center.X;
                landingState = LandingState.Breaking;
            }
            if (IsPlaneNotAboveCarrier)
            {
                if (direction == Direction.Left ||
                    (direction == Direction.Right && //zeby nie wybucha³ pop prawej stronie
                     Bounds.LeftMostX >= level.Carrier.GetEndPosition().X + LevelTile.Width + 4))
                {
                    landingState = LandingState.None;
                    locationState = LocationState.Air;
                    UnblockMovementInput();
                    level.Controller.OnTakeOff();
                }
            }
        }

        /// <summary>
        /// Hamowanie na lotniskowcu.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void LandingBreaking(float time, float timeUnit)
        {
            float oldSpeed = Speed;

            float a = lineCatchSpeed/System.Math.Abs(catchLinePositionX - breakingEndPositionX);
            float b = - a*breakingEndPositionX;
            if (a*Center.X + b > 0)
            {
                Speed = a*Center.X + b;
            }
            else
            {
                Speed = 0;
            }

            float scaleFactor = time / timeUnit;

            changeAngleWhileWheeling(oldSpeed, Speed, scaleFactor);
            if (Speed <= GameConsts.UserPlane.BreakingMinSpeed)
            {
                Speed = 0;
                level.Controller.OnReleasePlane(this, breakingEndCarrierTile);
            }
        }

        /// <summary>
        /// Wywo³ywana po zwolnieniu przez kontroler lin hamuj¹cych.
        /// </summary>
        /// <param name="breakingEndCarrierTile"></param>
        public void ReleaseLine(AircraftCarrierTile breakingEndCarrierTile)
        {
            locationState = LocationState.AircraftCarrier;
            landingState = LandingState.None;
            StopPlane();
            this.breakingEndCarrierTile = null;
            UnblockAllInput();
            isLanding = false;
            isAfterFlyingDown = false;
        }

        /// <summary>
        /// Metoda "przykleja samolot do pasa startowego" 
        /// tzn nie pozwala mu unieœæ siê w góre, poprzez ustawienie 
        /// rectangla samolotu wzd³u¿ pasa startowego.
        /// </summary>
        public void AirstripCotact()
        {
            float delta = level.Carrier.Height - Bounds.LowestY;
            Bounds.Move(0, delta);
            movementVector.Y = 0;
        }

        #endregion

        /// <summary>
        /// Funkcja ograniczaj¹ca lot samolotu w góre
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        protected void HeightLimit(float time, float timeUnit)
        {
            float rot = time/timeUnit*rotateStep*2;

            if (locationState == LocationState.CarrierTurningRound)
                return;

            if (Bounds.Center.Y > GameConsts.UserPlane.MaxHeight*maxHeightTurningRange)
            {
                level.OnPlaneForceGoDown(this);
                bool normalFlightToUp = direction == Direction.Left && -1*System.Math.PI/2 < Angle && Angle <= 0 ||
                                        direction == Direction.Right && 0 <= Angle && Angle < System.Math.PI/2;

                bool upSideDownFlightToUp = direction == Direction.Left && /*(-1) * Math.PI <= this.Angle &&*/
                                            Angle <= (-1)*System.Math.PI/2 ||
                                            direction == Direction.Right && System.Math.PI/2 <= Angle &&
                                            Angle <= System.Math.PI;

                if ((normalFlightToUp || upSideDownFlightToUp) && !isMaxHeightRotate)
                {
                    isMaxHeightRotate = true;
                    speedBeforeMaxHeightRotata = Speed;
                }

                if (upSideDownFlightToUp)
                {
                    float delta = Math.Abs(Math.PI - Math.Abs(Bounds.Angle));
                    if (delta <= rot)
                    {
                        Bounds.Rotate((int) direction*delta);
                        movementVector.Y = 0;
                        isBlockDown = false;
                        isBlockUp = false;

                        Speed = speedBeforeMaxHeightRotata;
                        isMaxHeightRotate = false;
                    }
                    else
                    {
                        rotateValue = 0;
                        Bounds.Rotate((int) direction*rot);
                        Speed = minFlyingSpeed;
                        isBlockDown = true;
                        isBlockUp = true;

                        if ((int) direction*Angle <= 0)
                        {
                            isBlockDown = false;
                            isBlockUp = false;

                            Speed = speedBeforeMaxHeightRotata;
                            isMaxHeightRotate = false;
                        }
                    }
                }
                if (normalFlightToUp)
                {
                    float delta = System.Math.Abs(Bounds.Angle);

                    if (delta <= rot)
                    {
                        Bounds.Rotate((-1)*(int) direction*delta);
                        movementVector.Y = 0;
                        isBlockDown = false;
                        isBlockUp = false;

                        Speed = speedBeforeMaxHeightRotata;
                        isMaxHeightRotate = false;
                    }
                    else
                    {
                        rotateValue = 0;
                        Bounds.Rotate((-1)*(int) direction*rot);
                        Speed = minFlyingSpeed;
                        isBlockDown = true;
                        isBlockUp = true;
                    }
                }
            }
        }

        /// <summary>
        /// Funkcja ograniczaj¹ca lot samolotu w prawo i w lewo
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        protected void VerticalBoundsLimit(float time, float timeUnit)
        {
            if (isMaxHeightRotate)
                return;

            bool normalFlight = (0 <= System.Math.Abs(Bounds.Angle) && System.Math.Abs(Bounds.Angle) <= System.Math.PI/2);
            bool upSideDownFlight = (System.Math.PI/2 < System.Math.Abs(Bounds.Angle) &&
                                     System.Math.Abs(Bounds.Angle) <= 3.1416f); //3.1416f to zaokr¹glenie numeryczne System.Math.PI

            if (Mathematics.PositionToIndex(Bounds.Center.X) <= turningTails)
            {
                if ((direction == Direction.Left && normalFlight) ||
                    (direction == Direction.Right && upSideDownFlight))
                {
                    if (locationState != LocationState.AirTurningRound)
                    {
                        rotateValue = 0;
                        locationState = LocationState.AirTurningRound;
                        level.OnPlaneForceChangeDirection(this);
                        level.Controller.OnPrepareChangeDirection(direction, this, TurnType.Airborne);
                    }
                }
            }
            if (Mathematics.PositionToIndex(Bounds.Center.X) >= level.LevelTiles.Count - turningTails)
            {
                if ((direction == Direction.Right && normalFlight) ||
                    (direction == Direction.Left && upSideDownFlight))
                {
                    if (locationState != LocationState.AirTurningRound)
                    {
                        rotateValue = 0;
                        locationState = LocationState.AirTurningRound;
                        level.OnPlaneForceChangeDirection(this);
                        level.Controller.OnPrepareChangeDirection(direction, this, TurnType.Airborne);
                    }
                }
            }
        }

        /// <summary>
        /// Funkcja odpowiadaj¹ca za opuszczenie lotniskowca 
        /// w czasie startu
        /// </summary>
        private void LeaveCarrier(float time, float timeUnit)
        {
            if (locationState == LocationState.AircraftCarrier && landingState == LandingState.None)
            {
                if (!isFallingFromCarrier && !IsGearAboveCarrier)
                {
                    if (HasSpeedToStart)
                    {
                        if (IsPlaneNotAboveCarrier)
                        {
                            if (direction == Direction.Right)
                            {
                                level.Controller.OnPlaneWrongDirectionStart();
                                oil -= rightTakeOffOilLoss;
                            }
                            locationState = LocationState.Air;
                            level.Controller.OnTakeOff();
                        }
                    }
                    else
                    {
                        float scaleFactor = time / timeUnit;
                        raiseTailStep(scaleFactor);
                        isFallingFromCarrier = true;
                        isSlippingFromCarrier = Speed <= maxSlippingFromCarrierSpeed;
                        AirstripCotact();
                        BlockMovementInput();
                    }
                }
            }
            FallFromCarrier(time, timeUnit);
        }

        /// <summary>
        /// Metoda wywo³ywana gdy samolot spada z lotniskowca.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void FallFromCarrier(float time, float timeUnit)
        {
            float scaleFactor = time/timeUnit;
            float peakX = (direction == Direction.Left)
                              ? Carrier.GetBeginPosition().X
                              : Carrier.GetEndPosition().X + LevelTile.Width;
            if (isFallingFromCarrier)
            {
                if (isSlippingFromCarrier)
                {
                    if (System.Math.Abs(bounds.Angle) < maxSlipingFromCarrierAngle)
                        Rotate(-(int) direction*rotateStep*scaleFactor);
                    else
                        isSlippingFromCarrier = false;
                }
                else
                {
                    if (System.Math.Abs(bounds.Center.X - peakX) <= height/2 || IsCenterAboveCarrier)
                    {
                        movementVector.Y -= SlippingFromCarrierAcceleration*scaleFactor;
                        if (System.Math.Abs(movementVector.X) <= maxSlippingFromCarrierSpeed)
                            movementVector.X += (int) direction*SlippingFromCarrierAcceleration*scaleFactor;
                    }
                    else
                        FallDown(time, timeUnit);
                }
            }
        }

        /// <summary>
        /// Powoduje toniêcie samolotu lub tylko odczekanie odpowiedniej iloœci czasu.
        /// </summary>
        /// <param name="time">Czas od ostatniego wywo³ania metody. Mierzony w ms. </param>
        /// <param name="timeUnit">Wartoœæ czasu do której odnoszone s¹ wektor ruchu i wartoœæ obrotu. Wyra¿ona w ms.</param>
        protected void MoveAfterCrash(float time, float timeUnit)
        {
            if (wreckTimeElapsed > wreckTime) //koniec czasu
            {
                level.Controller.OnPlaneWrecked(this);
                if (IsEnemy) //odrejestrownaie samolotu wroga
                {
                    level.Controller.OnUnregisterPlane(this);
                    level.ClearEnemyPlane(this);
                }
            }
            else
            {
                if (isSinking)
                    Sink(time, timeUnit);
                if (isFallingAfterCrash)
                {
                    movementVector.Y -= gravitationalAcceleration*time/timeUnit;
                    bounds.Move((time/timeUnit)*movementVector);
                    if (bounds.LowestY <= OceanTile.depth)
                    {
                        isFallingAfterCrash = false;
                        isSinking = true;
                    }
                }
            }

            wreckTimeElapsed += time;
        }

        /// <summary>
        /// Toniêcie samolotu
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void Sink(float time, float timeUnit)
        {
            //czy pod wod¹ nie ma ju¿ wyspy
            int index = Mathematics.PositionToIndex(Center.X);
            if (0 <= index && index < level.LevelTiles.Count)
            {
                LevelTile tile = level.LevelTiles[index];
                if (tile.TileKind == TileKind.Island)
                    movementVector.X = 0;
            }

            //aktualizacja movmentVector.X
            if (((float) direction*bounds.Angle) < 0 && movementVector.X != 0)
                //jesli skierowany do do³u && predkosc X nie jest zerem
                movementVector.X = (movementVector.X < 0)
                                       ? System.Math.Min(movementVector.X + waterXBreakingPower, 0)
                                       :
                                           System.Math.Max(movementVector.X - waterXBreakingPower, 0);
            else
                movementVector.X = 0;

            //aktualizacja movmentVector.Y
            movementVector.Y = (movementVector.Y >= 0)
                                   ? - GameConsts.UserPlane.SinkingSpeed
                                   :
                                       System.Math.Min(movementVector.Y + waterYBreakingPower,
                                                       -GameConsts.UserPlane.SinkingSpeed);

            bounds.Move((time/timeUnit)*movementVector);
        }

        /// <summary>
        /// Obraca samolot (i wektor ruchu) zgodnie z aktualn¹ wartoœci¹ obrotu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void UpdatePlaneAngle(float scaleFactor)
        {
            //rotateValue = rotateValue * (-Mogre.Math.Sin(RelativeAngle)/7.6f + 1.099f);

            if (wheelsState == WheelsState.Out)
                if (
                    (RelativeAngle >= maxWheelOutAngle && (float) direction*rotateValue > 0) ||
                    (RelativeAngle <= -maxWheelOutAngle && (float) direction*rotateValue < 0)
                    )
                    rotateValue = 0;
           
            Rotate(scaleFactor*rotateValue);
        }

        /// <summary>
        /// Zwiêksza (lub zmniejsza w zale¿noœci od znaku parametru value) wartoœæ rotateValue.
        /// Nowa wartoœæ jest obcinana do wartoœci maksymalnej
        /// </summary>
        /// <param name="value">O ile nale¿y zwiêkszyæ wartoœæ bezwzglêdn¹ rotateValue</param>
        private void IncreaseRotateValue(float value)
        {
            rotateValue += value;
            if (value >= 0)
                rotateValue = System.Math.Min(rotateValue, maxRotateValue); //nie mo¿e przekroczyæ wartoœci max
            else
                rotateValue = System.Math.Max(rotateValue, -maxRotateValue); //nie mo¿e byæ mniejsza od -max
        }

        /// <summary>
        /// Zmniejsza (lub zwiêksza zale¿noœci od znaku parametru value) wartoœæ rotateValue.
        /// Nowa wartoœæ nie mo¿e zmieniæ znaku - jest obcinana do wartoœci 0.
        /// </summary>
        /// <param name="value"></param>
        private void DecreaseRotateValue(float value)
        {
            if (Angle == 0)
                return;

            if (rotateValue >= 0)
            {
                rotateValue -= value;
                rotateValue = System.Math.Max(rotateValue, 0); //nie mo¿e spaœæ poniŸej 0
            }
            else
            {
                rotateValue += value;
                rotateValue = System.Math.Min(rotateValue, 0); //nie mo¿e wskoczyæ powy¿ej 0
            }
        }
        #endregion

        #region IBoundingBoxes Members

        public List<Quadrangle> BoundingQuadrangles
        {
            get
            {
                List<Quadrangle> result = new List<Quadrangle>();
                result.Add(bounds);
                return result;
            }
        }

        public string Name
        {
            get { return "Plane" + GetHashCode(); }
        }

        #endregion 
    }
    #endregion
}