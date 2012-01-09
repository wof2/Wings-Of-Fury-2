/*
 * Copyright 2008 Adam Witczak, Jakub T�ycki, Kamil S�awi�ski, Tomasz Bilski, Emil Hornung, Micha� Ziober
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
using Mogre;
using Wof.Controller;
using Wof.Model.Configuration;
using Wof.Model.Level;
using Wof.Model.Level.Carriers;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Weapon;
using Math=Mogre.Math;

namespace Wof.Model.Level.Planes
{

    #region Enums

    
    public class StartPositionInfo
    {
    	public StartPositionType PositionType = StartPositionType.Carrier;    	
    	public PointD Position;    	
    	public Direction Direction = Direction.Left;
    	public float Speed = 0;	
    	public WheelsState WheelsState = WheelsState.Out;  
    	public EngineState EngineState = EngineState.SwitchedOff;
    	
    	public MissionType MissionType = MissionType.BombingRun;
       
    }
    
    /// <summary>
    /// Miejsce w kt�rym znajduje si� samolot na pocz�tku rozgrywki
    /// </summary>
    public enum StartPositionType
    {
    	Carrier,
    	Airborne    	
    }
    
    /// <summary>
    /// Enumerator m�wi co aktualnie przysz�o z kontrolera.
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

    public enum PlaneType
    {
        P47,
        F4U,
        A6M,
        B25
    } ;

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
    public class Plane : IRenderableQuadrangles, IObject2D
    {
        #region Constants


        /// <summary>
        /// Procent wartosci MaxOil po przekroczeniu ktorej silnik jest zatarty nie moze byc ponownie odpalony
        /// </summary>
        protected const float engineUnstartableOilThreshold = 0.15f;
        /// <summary>
        /// Maksymalna odleglosc od lotniskowca ktora moze spowodowac wyswietlenie hintu o ladowaniu
        /// </summary>
        protected const float potentiallyLandingMaxDistance = 200;

        protected const float potentiallyBadLandingMaxDistance = 100;

        /// <summary>
        /// K�t o jaki b�dzie si� zmienia�o nachylenie samolotu po jednokrotnym naci�ni�ciu strza�ki.
        /// (Wyra�ony w radianach)
        /// </summary>
        protected float rotateStep;

        /// <summary>
        /// Pr�dko�c przy wolnym ko�owaniu.
        /// </summary>
        protected float slowWheelingSpeed;

        /// <summary>
        /// maksymalna pr�dko�� przy szybkim ko�owaniu.
        /// </summary>
        protected float maxFastWheelingSpeed;

        /// <summary>
        /// Okre�la maksymaln� pr�dko�� samolotu z wysuni�tym podwoziem
        /// </summary>
        protected float maxWheelOutSpeed;

        /// <summary>
        /// Pr�dko�� przy na lotniskowcu przy kt�rej samolot opuszcza dzi�b.
        /// </summary>//musialem usunac const aby mozna bylo je ustwic. Michal
        protected float changeWheelsSpeed;

        /// <summary>
        /// K�t o kt�ry jest pochylony dzi�b samolotu w g�r� w czasie ko�owania na lotniskowcy.
        /// </summary>
        protected const float angleOnCarrier = Math.PI * 0.05f;

        /// <summary>
        /// Minimalna pr�dko�� lotu.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        protected float minFlyingSpeed;

        public float MinFlyingSpeed
        {
            get { return minFlyingSpeed; }
        }

        /// <summary>
        /// Maksymalny k�t przy kt�rym samolot mo�e zawr�ci� w powietrzu.
        /// </summary>
        protected const float maxTurningAngle = Math.PI/6;


        /// <summary>
        /// Szeroko�� prostok�ta ograniczaj�cego samolot
        /// </summary>
        protected float width;

        /// <summary>
        /// Wysoko�� prostok�ta ograniczaj�cego samolot
        /// </summary>
        protected float height;

        /// <summary>
        /// Okre�la odleglosc po wsp�rzednej X miedzy ko�ami a srodkiem samolotu.
        /// </summary>
        protected const float wheelDistanceFromCenter = 1.15f;

        /// <summary>
        /// Minimalna pr�dkosc smigiel(ustawiana zaraz po w��czeniu silnika).
        /// </summary>
        /// <author>Tomek</author>
        protected const int minAirscrewSpeed = 1000;

        /// <summary>
        /// Maxymalna pr�dkosc smigiel(ustawiana dla maksymalnej pr�dko�ci).
        /// </summary>
        /// <author>Tomek</author>
        private const int maxAirscrewSpeed = 3000;

        /// <summary>
        /// Przyspieszenie ziemskie [jednostka d�ugo�ci w modelu)/s^2]
        /// </summary>
        private const float gravitationalAcceleration = 50;


        /// <summary>
        /// Okre�la czas przez jaki samolot �wie�o po starcie jest traktowany jako 'isJustAfterTakeOff'
        /// </summary>
        private const float justAfterTakeOffTimerMax = 1.0f;
      

        /// <summary>
        /// Maksymalna ilo�� oleju.
        /// </summary>
        private const int maxOil = 100;

        public int MaxOil
        {
            get { return maxOil; }
        }

        /// <summary>
        /// Maksymalna ilo�� b�zyny.
        /// </summary>
        private const int maxPetrol = 100;

        public int MaxPetrol
        {
            get { return maxPetrol; }
        }

        /// <summary>
        /// Maksymalny k�t jaki mo�e mie� samolot z otwartym podwoziem.
        /// Wyra�ony jako liczba dodatnia w radianach.
        /// </summary>
        private const float maxWheelOutAngle = Math.PI/12.0f;

        /// <summary>
        /// K�t po przekroczeniu kt�rego samolot z otwartym podwoziem zacznie schodzi� w d�.
        /// </summary>
        private const float landingAngle = Math.PI/20.0f;

        /// <summary>
        /// Ilo�� oleju tracona, gdy samolot zostanie trafiony
        /// 
        /// </summary>
        protected float oilLeak;

        /// <summary>
        /// Maksymalny k�t, pod kt�rym mo�na zrzuci� bomb�.
        /// </summary>
        private const float maxFireBombAngle = 8*Math.PI/18.0f;
  
        
        /// <summary>
        /// Maksymalny k�t, pod kt�rym mo�na zrzuci� torped�.
        /// </summary>
        public const float MaxFireTorpedoAngle = Math.PI / 12.0f;


        

        /// OKre�la ile meter�w przed maxHeight samolot zaczyna sie prostowac.
        /// </summary>
        private const float maxHeightTurningRange = 0.80f;

        
        /// <summary>
        /// Okre�la ile tail przed ko�cem mapy samolot zaczyna zawracac.
        /// </summary>
        protected int turningTileMargin = 0;//11;

        /// <summary>
        /// Czas od momentu rozbicia samolotu do momentu przej�cia do nast�pnego �ycia.
        /// Wyra�ony w ms.
        /// </summary>
        private const float wreckTime = 3000;

        /// <summary>
        /// Maksymalna warto�� o jak� mo�e obr�ci� si� samolot w czasie timeUnit.
        /// </summary>
        protected float maxRotateValue;

        /// <summary>
        /// Si�a hamowania obrotu.
        /// </summary>
        protected float rotateBrakingFactor;

        /// <summary>
        /// Moc hamowania wody w poziomie.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        protected float waterXBreakingPower;

        /// <summary>
        /// Moc hamowania wody w pionie.//musialem usunac const aby mozna bylo je ustwic. Michal
        /// </summary>
        protected float waterYBreakingPower;

        /// <summary>
        /// Maksymalna predko�� ze�lizgiwania si� z lotniskowca.
        /// </summary>
        private const float maxSlippingFromCarrierSpeed = 7;

        /// <summary>
        /// Maksymalny k�t ze�lizgiwania si� z lotniskowca.
        /// </summary>
        private const float maxSlipingFromCarrierAngle = Math.PI*0.2f;

        /// <summary>
        /// Przy�pieszenie w czasie ze�lizgiwania si� z lotniskowca. 
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

        
        public StartPositionInfo StartPositionInfo
        {
        	get { return startPositionInfo; }
        }
        
        protected StartPositionInfo startPositionInfo;


        protected PlaneType planeType;
        
        /// <summary>
        /// Pr�dko�� obracania �mig�a
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
        /// Okre�la czy samolot jest defacto w stanie zmiany kierunku, czyli czy powinien porusza�
        /// si� zgodnie z turningVector. Samolot mo�e by� w stanie po zawr�ceniu, ale nie otrzyma�
        /// jeszcze komunikatu o tym, �e proces si� zako�czy�.
        /// </summary>
        protected bool isChangingDirection = false;
        
        protected bool lastIsChangingDirection = false;

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


        private float outOfOilFaultyEngineTimer = 0;
        private float outOfOilFaultyEngineTimerMax;

        private bool isEngineFaulty;

        /// <summary>
        /// Czy silnik jest uszkodzony z powodu braku oleju?
        /// </summary>
        public bool IsEngineFaulty
        {
            get { return isEngineFaulty; }
        }
        
        protected AttractorTarget attractorTarget;
        
        public AttractorTarget AttractorTarget
        {
        	get { return attractorTarget; }
        }
        

        /// <summary>
        /// Licznik ile razy zostal uruchomiony silnik.
        /// </summary>
        private int counterStartedEngine;

        /// <summary>
        /// Okre�la lokalizacje samoloty czy na lotniskowcu czy w powietrzu.
        /// </summary>
        protected LocationState locationState;

        /// <summary>
        /// Okre�la stan k�
        /// </summary>
        protected WheelsState wheelsState;

        /// <summary>
        /// Okre�la kierunek samolotu.
        /// </summary>
        protected Direction direction;

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ta strza�ka w lewo od ostatniego od�wie�enia.
        /// </summary>
        private bool isLeftPressed;

        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockLeft;

        public bool IsBlockLeft
        {
            get { return isBlockLeft; }
            set { isBlockLeft = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ta strza�ka w prawo od ostatniego od�wie�enia.
        /// </summary>
        private bool isRightPressed;
        
        
       

        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockRight;

        public bool IsBlockRight
        {
            get { return isBlockRight; }
            set { isBlockRight = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ta strza�ka w d� od ostatniego od�wie�enia.
        /// </summary>
        private bool isDownPressed;

        /// <summary>
        /// Okre�la czy zosta� wci�ni�ty przycisk obrotu samolotu (z 'plec�w' na 'brzuch').
        /// </summary>
        private bool isSpinPressed;


        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockDown;

        public bool IsBlockDown
        {
            get { return isBlockDown; }
            set { isBlockDown = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ta strza�ka w g�r� od ostatniego od�wie�enia.
        /// </summary>
        private bool isUpPressed;

        /// <summary>
        /// Wektor joysticka
        /// </summary>
        private Vector2? inputVector;

        private float GetInputVectorValueNormalised(DirectionAxis dir)
        {
            if (inputVector == null) return 1;
            float val;
            if(dir == DirectionAxis.Horizontal)
            {
                val = Math.Abs(inputVector.Value.x);
            }
            else
            {
                val = Math.Abs(inputVector.Value.y);
            }

            if (val > 1) val = 1;
            return val;
        }

        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockUp;

        public bool IsBlockUp
        {
            get { return isBlockUp; }
            set { isBlockUp = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ty przycisk strza�u z dzia�ka od ostatniego od�wie�enia.
        /// </summary>
        // private bool isFireGunPressed;
        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockFireGun;

        public bool IsBlockFireGun
        {
            get { return isBlockFireGun; }
            set { isBlockFireGun = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ty przycisk strza�u rakiety od ostatniego od�wie�enia.
        /// </summary>
        //private bool isFireRocketPressed;
        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockFireRocket;

        public bool IsBlockFireRocket
        {
            get { return isBlockFireRocket; }
            set { isBlockFireRocket = value; }
        }

        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockSpin;

        public bool IsBlockSpin
        {
            get { return isBlockSpin; }
            set { isBlockSpin = value; }
        }

        /// <summary>
        /// Okre�la czy zosta�a wci�ni�ty przycisk wy��czania silnika od ostatniego od�wie�enia.
        /// </summary>
        private bool isEngineKeyPressed;

        /// <summary>
        /// Okre�la czy klawisz jest zablokowany czy nie.
        /// </summary>
        private bool isBlockEngine;

        public bool IsBlockEngine
        {
            get { return isBlockEngine; }
            set { isBlockEngine = value; }
        }

        //private bool blockAllInput;

        /// <summary>
        /// Flaga okre�la czy przed chwil� zosta� uruchomiony silnik.
        /// Jest resretowana po puszczeniu przycisku uruchomienia silnika.
        /// </summary>
        private bool isEngineJustStarted;

        /// <summary>
        /// Flaga okre�la czy przed chwil� zosta� wy��czony silnik.
        /// Jest resretowana po puszczeniu przycisku uruchomienia silnika.
        /// </summary>
        private bool isEngineJustStopped;

        /// <summary>
        /// Okre�la czy samolot przed chwil� wystartowa� (z lewej strony lotniskowca)
        /// </summary>
        private bool isJustAfterTakeOff;

        /// <summary>
        /// pomocniczy licznik dla 'isJustAfterTakeOff'
        /// </summary>
        private float justAfterTakeOffTimer;

     
        /// <summary>
        /// Czy pokazano hint o ladowaniu 
        /// </summary>
        private bool isLandingHintDelivered;

        /// <summary>
        /// M�wi ile czasu b�dzie trwa�o ca�e zawracanie.
        /// Zmienna jest od�wie�ana za ka�dym zawracaniem.
        /// </summary>
        /// <author>Emil</author>
        protected float turningTime;

        /// <summary>
        /// Ile czasu pozosta�o do ko�ca zawracania.
        /// </summary>
        /// <author>Emil</author>
        protected float turningTimeLeft;

        protected Level level;

        /// <summary>
        /// Wektor u�ywany do poruszania samolotu podczas zawracania.
        /// </summary>
        protected PointD turningVector;

        /// <summary>
        /// Obiekt zarzadzajacy bronia samolotu.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected WeaponManager weaponManager;

        /// <summary>
        /// Okre�la stan podczas l�dowania
        /// </summary>
        private LandingState landingState;

        /// <summary>
        /// Obiekt typu EndAircraftTile, kt�ry z�apa� samolot w czasie l�dowania.
        /// </summary>
        private EndAircraftCarrierTile breakingEndCarrierTile;

        /// <summary>
        /// Okre�la czy samolot l�duje. Tzn. ma wysuni�te podwozie, jest przechylony max
        /// do g�ry a gracz wciska strza�k� do g�ry (i nie wciska strza�ki kierunku)
        /// </summary>
        private bool isLanding;

        /// <summary>
        /// Okre�la czy samolot przed chwil� schodzi� w d�. Je�li tak to bez wci�ni�tych strza�ek 
        /// w g�r�/d� ma lecie� prosto.
        /// </summary>
        private bool isAfterFlyingDown;

        /// <summary>
        /// Czas jaki min�� od momentu rozbicia.
        /// </summary>
        private float wreckTimeElapsed = 0;

        /// <summary>
        /// Okre�la czy samolot rozbi� si� w wodzie i ma ton��.
        /// </summary>
        private bool isSinking = false;

        /// <summary>
        /// Oznacza czy samolot jest w trakcie zaminay
        /// kierunku ruchu gdy osi�gnie wysoko�� maksymalna.
        /// </summary>
        private bool isMaxHeightRotate;

       
        /// <summary>
        /// Warto�� o jak� nale�y obr�ci� samolot w danej chwili.
        /// (Odpowiednik movementVector)
        /// </summary>
        private float rotateValue;

        /// <summary>
        /// Warto�� o jak� nale�y obr�ci� samolot w danej chwili.
        /// </summary>
        public float RotateValue
        {
            get { return rotateValue; }
        }

        /// <summary>
        /// Okre�la czy samolot spada z lotniskowca.
        /// </summary>
        private bool isFallingFromCarrier;

        /// <summary>
        /// Okre�la czy samolot ze�lizguje sie z lotniskowca.
        /// </summary>
        private bool isSlippingFromCarrier;

        /// <summary>
        /// Okre�la czy samolot ma spada� po rozbiciu (np. rozbicie o pionow� sciane)
        /// </summary>
        private bool isFallingAfterCrash;

        /// <summary>
        /// Pr�dko�� samoloto w czasie z�apania przez line.
        /// </summary>
        private float lineCatchSpeed;

        /// <summary>
        /// Pozycja wsp�rz�dnej X w 
        /// czasie s�apania samolotu przez line
        /// </summary>
        private float catchLinePositionX;

        /// <summary>
        /// Pozycja samolotu gdzie samolot ko�czy hamowanie.
        /// </summary>
        private float breakingEndPositionX;

        /// <summary>
        /// Okre�la czy samolot podnosi ty� przy poruszaniu si� na lotniskowcu
        /// </summary>
        private bool isRaisingTail;

        /// <summary>
        /// Okre�la czy samolot obni�a ty� przy poruszaniu si� na lotniskowcu
        /// </summary>
        private bool isLoweringTail;


        /// <summary>
        /// Okre�la czy samolot obr�ci� si� ruchem spinu
        /// </summary>
        protected bool spinned = false;

        public bool Spinned
        {
            get { return spinned; }
        }

        #endregion



        protected virtual void SetupConstants()
        {
            rotateStep = GameConsts.UserPlane.Singleton.UserRotateStep;

            slowWheelingSpeed = GameConsts.UserPlane.Singleton.RangeSlowWheelingSpeed *
                                GameConsts.UserPlane.Singleton.MaxSpeed;

            maxFastWheelingSpeed = GameConsts.UserPlane.Singleton.RangeFastWheelingMaxSpeed *
                                   GameConsts.UserPlane.Singleton.MaxSpeed;

            maxWheelOutSpeed = 0.5f * GameConsts.UserPlane.Singleton.MaxSpeed;

            changeWheelsSpeed = maxFastWheelingSpeed * 0.8f;

            minFlyingSpeed = GameConsts.UserPlane.Singleton.RangeFastWheelingMaxSpeed *
                             GameConsts.UserPlane.Singleton.MaxSpeed;

            width = GameConsts.UserPlane.Singleton.Width;

            height = GameConsts.UserPlane.Singleton.Height;

            oilLeak = 0;

            maxRotateValue = GameConsts.UserPlane.Singleton.UserMaxRotateValue;

            rotateBrakingFactor = GameConsts.UserPlane.Singleton.UserRotateBrakingFactor;

            waterXBreakingPower = GameConsts.UserPlane.Singleton.MaxSpeed * 0.01f;

            waterYBreakingPower = GameConsts.UserPlane.Singleton.MaxSpeed * 0.04f;
        }

        #region Public Constructor

      
        /// <summary>
        /// Brak inicjalizacji
        /// </summary>
        /// <param name="level"></param>
        /// <param name="isEnemy"></param>
        public Plane(Level level, bool isEnemy, PlaneType type)
            : this(level, isEnemy, null, type)
        {
        	
        }

       
            
        /// <summary>
        /// Konstruktor podstawowy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public Plane(Level level, bool isEnemy, StartPositionInfo info, PlaneType type)
        {
            SetupConstants();
            this.planeType = type;

            this.level = level;
            startPositionInfo = info;
            if(info != null) Init();

            weaponManager = new WeaponManager(level, this);
            if(info == null)
            {
            	weaponManager.SelectWeapon = WeaponType.Bomb;            	
            }else
            {
                if (info.MissionType == MissionType.Survival)
                {
                    weaponManager.SelectWeapon = WeaponType.Rocket;
                } else if (info.MissionType == MissionType.Dogfight)
	            {
	                weaponManager.SelectWeapon = WeaponType.Rocket;
	            } else if (info.MissionType == MissionType.Naval)
	            {
	                weaponManager.SelectWeapon = WeaponType.Torpedo;
	            } else if (info.MissionType == MissionType.Assassination)
	            {
	                weaponManager.SelectWeapon = WeaponType.Rocket;
	            } else
	            {
	            	weaponManager.SelectWeapon = WeaponType.Bomb;
	            }
            }
            
            weaponManager.RegisterWeaponToModelEvent += weaponManager_RegisterWeaponToModelEvent;

            this.isEnemy = isEnemy;
        }


        /// <summary>
        /// Tworzy samolot zgodnie z parametrami
        /// </summary>
        /// <param name="level"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isEnemy"></param>
        public Plane(Level level, float width, float height, bool isEnemy, StartPositionInfo info, PlaneType type)
            : this(level, isEnemy, info, type)
        {
            bounds = new Quadrangle(info.Position, width, height);
        }
     



        #endregion

        #region Properties

        /// <summary>
        /// Licznik uplynietego czasu od ostatniego wystrzalu.
        /// </summary>
        public int LastFireTick = Environment.TickCount;


        /// <summary>
        /// Zwraca lewy dolny wierzcho�ek, je�li samolot jest skierowany w prawo lub prawy dolny
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
        /// Czy moze sprobowac uruchomic silnik
        /// </summary>
        public bool CanTryToStartEngine
        {
            get
            {
              
                return (!isEnemy &&
                        petrol > 0 && //czy ilosc benzyny > 0
                        oil >= - engineUnstartableOilThreshold * maxOil && //czy ilosc oleju jest OK
                        planeState != PlaneState.Crashed &&
                        planeState != PlaneState.Destroyed //czy samolot nie jest znisczony ?
                        );
            }
            
        }

        /// <summary>
        /// Zwraca true, jesli samolot moze uruchomic silnik w tym momencie.
        /// False w przeciwnym przypadku.
        /// </summary>
        /// <author>Michal Ziober</author>
        public bool CanEngineBeStartedNow
        {
            get
            {
                //okres czasu jaki musi uplynac aby wlaczyc silnik w zaleznosci od stanu samolotu
                int timeThreshold = locationState == LocationState.Air ? GameConsts.UserPlane.Singleton.EngineCounterThresholdInAir : GameConsts.UserPlane.Singleton.EngineCounterThreshold;
                return (locationState != LocationState.AirTurningRound && CanTryToStartEngine && counterStartedEngine > timeThreshold);
                //czy mozna juz uruchomic silnik ?
            }
        }

        /// <summary>
        /// Pobiera lub ustawia licznik czasowy uruchomienia silnika.
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
        /// Zwraca true je�li samolot mo�e porusza� si� na lotniskowcu tzw. szybkim ko�owaniem.
        /// Zwraca false w przeciwnym wypadku.
        /// </summary>
        /// <authore>Tomek</authore>
        public bool CanSlowWheeling
        {
            get { return LocationState == LocationState.AircraftCarrier && !IsEngineWorking; }
        }

        /// <summary>
        /// Zwraca true je�li samolot mo�e porusza� si� na lotniskowcu tzw. wolnym ko�owaniem.
        /// Zwraca false w przeciwnym wypadku.
        /// </summary>
        /// <authore>Tomek</authore>
        public bool CanFastWheeling
        {
            get { return LocationState == LocationState.AircraftCarrier && IsEngineWorking; }
        }

        /// <summary>
        /// Sprawdza czy samolot mo�e zmieni� kierunek na lotniskowcu.
        /// </summary>
        /// /// <author>Tomek</author>
        public bool CanChangeDirectionOnAircraft
        {
            get
            {
                if (LocationState == LocationState.AircraftCarrier) return movementVector.X == 0;
                //    return Math.Abs(movementVector.X) <= 0.01f; <- Kamil obczaj co si� dzieje

                
                return false;
            }
        }

        /// <summary>
        /// Sprawdza czy samolot mo�e rozpocz�� zawracanie
        /// </summary>
        public bool CanTurnAround
        {
            get
            {
                // nie mozna zawracac na duzej wysokosci w taki sposob ze samolot zakonczylby zawrotke dziobem do gory
                if (Bounds.Center.Y > GameConsts.UserPlane.Singleton.MaxHeight * 0.9f * maxHeightTurningRange && ClimbingAngle < -0.5f)
                {
                    return false;
                }
                return System.Math.Abs(Angle) < maxTurningAngle; // czy nie mamy za du�ego k�ta?

            }
        }

        /// <summary>
        /// Sprawdza czy samolot mo�e rozpocz�� spin
        /// </summary>
        /// <author>Adam,Kamil</author>
        public bool CanSpin
        {
            get
            {
                return (GameConsts.UserPlane.Singleton.CanSpin > 0 &&  /*Math.Abs(rotateValue / rotateStep) <= 0.15f && */
                    wheelsState == WheelsState.In);
            }
        }


       

        /// <summary>
        /// Zwraca k�t nachylenia samolotu do kierunku wyznaczonego przez wektor ruchu.
        /// Liczony przeciwnie do ruchu wskaz�wek.
        /// Kat jest wyrazony w radianach i z przedzialu [-pi;pi]
        /// </summary>
        /// <author>Emil</author>
        public float Angle
        {
            get { return bounds.Angle; }
        }

        /// <summary>
        /// Zwraca k�t nachylenia samolotu do kierunku wyznaczonego przez wektor ruchu. 
        /// K�t dodatni osznacza wychylenie do g�ry, a ujemny do do�u.
        /// Kat jest wyrazony w radianach i z przedzialu [-pi;pi]
        /// </summary>
        public float RelativeAngle
        {
            get { return (float) direction*bounds.Angle; }
        }


        /// <summary>
        /// K�t pod jakim unosi si� / spada samolot: [-pi;pi]. Parametr niezale�ny od kierunku lotu.
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
        /// Sprawdza czy samolot ma wystarczaj�ca wysoko�� aby wystartowac
        /// </summary>
        public bool HasSpeedToStart
        {
            get
            {
                return movementVector.EuclidesLength >= maxFastWheelingSpeed;
            }
        }

        /// <summary>
        /// Zwraca true je�li ca�y samolot(wszytskie punkty czworokata) 
        /// nie znajduj� sie nad lotniskowcem.
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
                    if (Carrier.GetEndPosition().X + LevelTile.TileWidth >= Bounds.Peaks[i].X)
                    {
                        rightFlag = false;
                        break;
                    }
                }
                return rightFlag || leftFlag;
            }
        }

        /// <summary>
        /// Zwraca true je�li ca�y samolot(wszytskie punkty czworokata) 
        /// znajduj� sie nad lotniskowcem.
        /// </summary>
        public bool IsPlaneAboveCarrier
        {
            get
            {
                for (int i = 0; i < Bounds.Peaks.Count; i++)
                    if (Carrier.GetBeginPosition().X >= Bounds.Peaks[i].X)
                        return false;

                for (int i = 0; i < Bounds.Peaks.Count; i++)
                    if (Bounds.Peaks[i].X >= Carrier.GetEndPosition().X + LevelTile.TileWidth)
                        return false;

                return true;
            }
        }

        /// <summary>
        /// Zwraca true jesli �rodek samolotu znajduje sie nad lotniskowcem
        /// </summary>
        public bool IsCenterAboveCarrier
        {
            get
            {
                return Carrier.GetBeginPosition().X < Bounds.Center.X &&
                       Bounds.Center.X < Carrier.GetEndPosition().X + LevelTile.TileWidth;
            }
        }

        /// <summary>
        /// Zwraca true jesli ko�a samolotu znajduja sie nad lotniskowcem
        /// </summary>
        public bool IsGearAboveCarrier
        {
            get
            {
                return Carrier.GetBeginPosition().X < WheelsPosition.X &&
                       WheelsPosition.X < Carrier.GetEndPosition().X + LevelTile.TileWidth;
            }
        }

        /// <summary>
        /// Okresla czy samolot moze zmini� amunicjie 
        /// na lotniskowcu.
        /// </summary>
        public bool CanChangeAmmunition
        {
            get
            {
                return locationState == LocationState.AircraftCarrier && MovementVector.X <= 0.01f &&
                       Bounds.Center.X >= level.Carrier.GetRestoreAmunitionPosition().X &&
                       Bounds.Center.X <= level.Carrier.GetRestoreAmunitionPosition().X + LevelTile.TileWidth;
            }
        }

        /// <summary>
        /// Zwraca czy samolot mo�e wystrzeli� rakiet�.
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


     

        public bool IsNextTorpedoAvailable
        {
            get
            {
                return ((Environment.TickCount - LastFireTick) >= GameConsts.Torpedo.FireInterval);
            }
        }

        /// <summary>
        /// Zwraca czy samolot mo�e wystrzeli� rakiet�.
        /// </summary>
        public bool CanFireTorpedo
        {
            get
            {
                return
                    planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed &&
                    RelativeAngle < MaxFireTorpedoAngle && RelativeAngle > -MaxFireTorpedoAngle &&
                    locationState == LocationState.Air && !isMaxHeightRotate;
            }
        }

        
        /// <summary>
        /// Zwraca czy samolot mo�e zrzuci� bomb�.
        /// </summary>
        public bool CanFireBomb
        {
            get
            {
                bool locationOK;
                if(EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
                {
                    locationOK = (locationState == LocationState.Air || locationState == LocationState.AirTurningRound);
                } else
                {
                    locationOK = (locationState == LocationState.Air);
                }

                return
                    planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed &&
                    RelativeAngle < maxFireBombAngle && RelativeAngle > -maxFireBombAngle
                    && locationOK && !isMaxHeightRotate;
            }
        }

        /// <summary>
        /// Szybko�� samolotu.
        /// </summary>
        public float Speed
        {
            get { return movementVector.EuclidesLength; }
            set { movementVector.EuclidesLength = value; }
        }

        /// <summary>
        /// Zwraca czy samolot jest na lotniskowcu ( w stanie zawracania lub zwyk�ym)
        /// </summary>
        public bool IsOnAircraftCarrier
        {
            get { return (locationState == LocationState.AircraftCarrier || locationState == LocationState.CarrierTurningRound); }
        }

        /// <summary>
        /// Zwraca szerokos� samolotu.
        /// </summary>
        public static float Width
        {
            get { return GameConsts.UserPlane.Singleton.Width; }
        }

        /// <summary>
        /// Zwraca wysoko�� samolotu.
        /// </summary>
        public static float Height
        {
            get { return GameConsts.UserPlane.Singleton.Height; }
        }

        /// <summary>
        /// Zwraca odleg�o�� (r�nic� we wsp�rz�dnych X) samolotu od najbli�szego samolotu wroga.
        /// Je�li samolotu wroga nie ma, zwraca -1.
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
        /// Oblicza odleg�o�� do najbli�szego samolotu wroga. Zwraca -1 w przypadku gdy nie ma innych wrog�w
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

		/// <summary>
		/// Zwraca sume roznic odleglosci w osi X oraz Y (diffX + diffY)
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
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
        
        
        
        /// <summary>
        /// Zwraca odleg�o�� miedzy X srodka samolotu a X srodka IndexToPosition(t.TileIndex)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public float XDistanceToTile(LevelTile t)
        {
            return t != null
            	? System.Math.Abs(Center.X - Mathematics.IndexToPosition(t.TileIndex))
                       : -1;
        }

        /// <summary>
        /// Zwraca odleg�o�� miedzy Y srodka samolotu a Y srodka HitBound tilea
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public float YDistanceToTile(LevelTile t)
        {
        	//TODO: find tile's top Y
            return t != null
            	? System.Math.Abs(Center.Y - t.MaxY)
                       : -1;
        }
        
        /// <summary>
        /// Zwraca sume roznic odleglosci w osi X oraz Y (diffX + diffY)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public float DistanceToTile(LevelTile t)
        {
            float d = XDistanceToTile(t);
            if (d == -1) return d;
            return YDistanceToTile(t) + d;
        }

        #endregion

        #region Public Method

       
        public void InitNextLife()
        {
        	StartPositionInfo info = new StartPositionInfo();
        	info.Direction = Direction.Left;
        	info.EngineState = EngineState.SwitchedOff;
        	info.PositionType = StartPositionType.Carrier;
        	info.Speed = 0;
        	info.WheelsState = WheelsState.Out;
        	this.startPositionInfo = info;
        	Init();
        }
        
        
        /// <summary>
        /// Inicjuje pola samolotu odpowiednimi warto�ciami.
        /// Wywo�ywana w konstruktorze oraz przy nowym zyciu.
        /// </summary>
        public void Init()
        {
        	this.attractorTarget = new AttractorTarget();
        	StartPositionInfo info = startPositionInfo;
        	
            motorState = info.EngineState;
            planeState = PlaneState.Intact;
            wheelsState = info.WheelsState;
         
            
            landingState = LandingState.None;
            if(info.PositionType == StartPositionType.Carrier)
            {
            	locationState = LocationState.AircraftCarrier;
            }
            if(info.PositionType == StartPositionType.Airborne)
            {
            	locationState = LocationState.Air;
            }

            OilRefuel();
            petrol = maxPetrol;
            isEngineJustStarted = false;
            isEngineJustStopped = false;
            counterStartedEngine = 0;


       
            outOfOilFaultyEngineTimerMax = 0;
         



            breakingEndCarrierTile = null;
            if(level != null)
            {
                breakingEndPositionX = Carrier.GetRestoreAmunitionPosition().X + LevelTile.TileWidth;
            }
			
            direction = info.Direction;
            movementVector = new PointD(0, 0);
            if(info.PositionType == StartPositionType.Carrier)
            { 
            	bounds = new Quadrangle(GetStartingPosition(), width, height);   
                if(planeType != PlaneType.B25) // hardcode
                {
                    Rotate(angleOnCarrier * (float)direction);
                }
            	
            	airscrewSpeed = 0;
            	
            	
            }
            if(info.PositionType == StartPositionType.Airborne)
            { 
            	if(info.Position == null) 
            	{
            		info.Position = GetStartingPosition();
            		info.Position.Y = GameConsts.UserPlane.Singleton.MaxHeight * 0.5f;
            		info.Position.X += 100;
            	}
            	bounds = new Quadrangle(info.Position, width, height);
                if (info.Speed == 0)
                {
                    Speed = minFlyingSpeed;
                }
                else
                {
                    Speed = info.Speed;
                }

            	if(info.EngineState == EngineState.Working)
            	{
            		StartEngine();
                    // dzwiek silnika nie moze byc odpalony z tego miejsca. Jestesmy w innym watku...
                   
            	}
            
                movementVector = new PointD((float) direction*Speed, 0);
            }
           
            isChangingDirection = false;
			lastIsChangingDirection = false;
            UnblockAllInput();
            ResetInputFlags();

            isLanding = false;
            isAfterFlyingDown = false;
            isMaxHeightRotate = false;
           
            //dodane przez Emila
            rotateValue = 0;

            isSinking = false;
           
            turningTime = 0;
            turningTimeLeft = 0;
            turningVector = new PointD(0, 0);
            wreckTimeElapsed = 0;

            isFallingFromCarrier = false;
            isSlippingFromCarrier = false;
            isFallingAfterCrash = false;

            isJustAfterTakeOff = false;
            justAfterTakeOffTimer = 0; 

            isRaisingTail = false;
            isLoweringTail = false;

            
            spinned = false;
            isEngineFaulty = false;
            if(level!=null)
            {
                level.Controller.OnEngineRepaired(this);
            }
           
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
                    //jesli podwozie zadarte to je opu��
                    if (scaleFactor >=0)
                        lowerTailStep(scaleFactor);
            }
            if (motorState == EngineState.Working)
            {
                motorState = EngineState.SwitchedOff;
                airscrewSpeed = 0;
                if (!isEnemy) //wy��czam d�wi�k tylko dla samolotu gracza
                    level.Controller.OnTurnOffEngine(this);

            }
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k�t.
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(float angle)
        {
            Bounds.Rotate(angle);
            movementVector.Rotate(new PointD(0, 0), angle);
            if(locationState == LocationState.AirTurningRound)
            {
            	turningVector.Rotate(new PointD(0, 0), angle);
            }
            
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k�t ale zawsze w g�r�.
        /// </summary>
        /// <param name="angle"></param>
        public void RotateUp(float angle)
        {
            Rotate((float) direction*angle);
        }

        /// <summary>
        /// Obraca samolot i wektor ruchu o dany k�t ale zawsze w d�.
        /// </summary>
        /// <param name="angle"></param>
        public void RotateDown(float angle)
        {
            Rotate(-1.0f*(float) direction*angle);
        }

        /// <summary>
        /// Przesuwa samolot zgodnie z aktualnym wektorem, przetwarzaj�c sygna�y z klawiatury.
        /// </summary>
        /// <param name="time">Czas jaki up�yn�� od ostatniego wywo�ania Move. Wyra�ony w ms.</param>
        /// <param name="timeUnit">Warto�� czasu do kt�rej odnoszone s� wektor ruchu i warto�� obrotu. Wyra�ona w ms.</param>
        public virtual void Move(float time, float timeUnit)
        {
            //Console.WriteLine(Speed);
            float scaleFactor = time/timeUnit;
           
            // atraktory
            // TODO: czy to nie koliduje z innymi rzeczami
            bounds.Move(this.AttractorTarget.GetAttractorsMeanForce());
            
            if (planeState == PlaneState.Crashed)
            {
                MoveAfterCrash(time, timeUnit);
                return;
            }
            
            ProcessInput(time, timeUnit);
    
            //odj�cie benzyny i ewentualnie oleju
            if (this.motorState == EngineState.Working && (locationState == LocationState.Air || locationState == LocationState.AirTurningRound))
                petrol -= scaleFactor*movementVector.EuclidesLength/GameConsts.UserPlane.Singleton.MaxSpeed*
                          GameConsts.UserPlane.Singleton.PetrolLoss;


            petrol = System.Math.Max(petrol, 0);
            oilLeak = System.Math.Min(oilLeak, maxOil * 0.015f);
            
            if (planeState == PlaneState.Damaged && !IsOnAircraftCarrier )
            {
                if (!GameConsts.UserPlane.Singleton.PlaneCheat || isEnemy)
                {
                    oil -= scaleFactor * oilLeak;
                }
            }
        

            // koniec paliwa
            if (!GameConsts.UserPlane.Singleton.GodMode && planeState != PlaneState.Destroyed &&
                planeState != PlaneState.Crashed)
            {
                if (petrol == 0 || oil <= 0)
                    OutOfPetrolOrOil(scaleFactor);
            }

            if (motorState == EngineState.Working)
            {
                //zmiana wektora ruchu przy zawracaniu (TURN)
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

            //czy ma w��czony silnik, je�li nie to obr�t samolotu, tak �eby spada�
            if (!IsOnAircraftCarrier && planeState != PlaneState.Crashed && motorState == EngineState.SwitchedOff)
                FallDown(time, timeUnit, GlideType.glider);

            if (IsEngineWorking)
                airscrewSpeed = minAirscrewSpeed + (int) Math.Abs((int) (15f*movementVector.X));

            //opuszczenie lotniskowca (START)
            LeaveCarrier(time, timeUnit);

            //proces l�dowania
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

                    if (sin > 0) sin *= -sin*17; // si�a unosz�ca
                    else         sin *=  sin*26; // si�a sci�gaj�ca 

                    float liftVectorY = 0.7f*(1 - sin);
                   
                    bounds.Move(0, liftVectorY*scaleFactor);

                    //Grawitacja
                    bounds.Move(0, (-1.0f)*scaleFactor);
                   
                }

                  
                bounds.Move(scaleFactor * movementVector); 
                    
                //if(locationState != LocationState.AirTurningRound)
                {
                	UpdatePlaneAngle(scaleFactor);
                }
                	
                
            }
            else if (isLanding) //schodzenie do l�dowania
            {
                isLanding = false;
                PointD landingVector = new PointD(movementVector.X, -GameConsts.UserPlane.Singleton.LandingSpeed);
                bounds.Move(scaleFactor*landingVector);
                isAfterFlyingDown = true;
            }
            else //jest w fazie po schodzeniu w d� - ma lecie� r�wnolegle do pod�o�a
            {
                PointD tempVector = new PointD(movementVector.X, 0);
                bounds.Move(scaleFactor*tempVector);
            }
        }

        /// <summary>
        /// Porusza samolotem.
        /// </summary>
        /// <param name="newDirection"></param>
        /// <param name="scaleFactor"></param>
        /// <author>Tomek , Kamil S�awi�ski, Adam</author>
        public void SteerHorizontal(Direction newDirection, float scaleFactor)
        {
            float joyScale = GetInputVectorValueNormalised(DirectionAxis.Horizontal);
            switch (locationState)
            {
                case LocationState.AircraftCarrier:
                    if (landingState == LandingState.None)
                    {
                        if (this.direction != newDirection)
                        {
                            if (CanChangeDirectionOnAircraft)
                            {
                               // movementVector.X = 0;
                               // movementVector.Y = 0;
                                TurnRound(newDirection, TurnType.Carrier);
                            }
                            //hamowanie samolotu (MOCNE)
                            float subSpeed = GameConsts.UserPlane.Singleton.BreakingPower*scaleFactor*
                                             GameConsts.UserPlane.Singleton.MoveStep;
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
                                    movementVector = new PointD((float)newDirection * slowWheelingSpeed * joyScale, 0);
                                }
                                else
                                {
                                    //hamowanie samolotu (S�ABE)
                                    float subSpeed = scaleFactor * GameConsts.UserPlane.Singleton.MoveStep;
                                    float oldSpeed = movementVector.EuclidesLength;
                                    float newSpeed = movementVector.EuclidesLength - subSpeed;

                                    subSpeedToMin(subSpeed, 0);
                                    changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor);
                                }
                            }
                            else if (CanFastWheeling)
                            {
                                float addSpeed = joyScale * scaleFactor * GameConsts.UserPlane.Singleton.MoveStep;
                                float oldSpeed = movementVector.EuclidesLength;
                                float newSpeed = movementVector.EuclidesLength + addSpeed;

                                if (movementVector.EuclidesLength == 0)
                                    movementVector.X = (float) newDirection*addSpeed; //ruszenie samolotem

                                changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor); //zmiana nachylenia dziobu
                                addSpeedToMax(addSpeed, maxFastWheelingSpeed); //przyspieszanie do maxymalnej
                            }
                        }
                    }
                    break;
                case LocationState.Air:
                    if (this.direction == newDirection)
                    {
                       
                        float addSpeed = joyScale * scaleFactor * GameConsts.UserPlane.Singleton.MoveStep;
                        if ( (wheelsState == WheelsState.In || wheelsState == WheelsState.TogglingIn) && !isEngineFaulty)
                            addSpeedToMax(addSpeed, GameConsts.UserPlane.Singleton.MaxSpeed); //przyspieszanie do maxymalnej
                        else
                            addSpeedToMax(addSpeed, maxWheelOutSpeed); // przy uszkodzonym silniku oraz kiedy podwozie jest wysuniete - nie przyspieszamy za bardzo
                    }
                    else //kierunek przeciwny do kierunku lotu
                        if (CanTurnAround) //sprawdzam czy mo�e zawr�ci�
                        {
                            TurnRound(newDirection, TurnType.Airborne);
                        }
                    break;
            }
        }

      

        /// <summary>
        /// Powoduje rozpocz�cie obrotu samolotu (z 'plec�w' na 'brzuch').
        /// </summary>
        protected void Spin()
        {
            if (planeState != PlaneState.Crashed)
            {
                // rotateValue = 0; //hamuj� zmian� k�ta - test
                //isChangingDirection = true;
                //isBlockSpin = true;
                isBlockEngine = true;
                isBlockLeft = true;
                isBlockRight = true;
                //isBlockUp = true;
                //isBlockDown = true;
                isBlockSpin = true;
                //BlockMovementInput();
                level.Controller.OnSpinBegin(this);
               
            }
        }

        /// <summary>
        /// Powoduje rozpocz�cie zawracania.
        /// </summary>
        /// <param name="newDirection">Kierunenk lotu po zawr�ceniu.</param>
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
                //je�li zawraca przy jednoczesnym l�dowaniu, to powinien lecie� zgodnie z kierunkiem nachylenia dzioba a nie prosto
                rotateValue = 0; //hamuj� zmian� k�ta
                level.Controller.OnPrepareChangeDirection(newDirection, this, turnType);
            }
        }

       

        /// <summary>
        /// Nape�nia olej.
        /// </summary>
        public void OilRefuel()
        {
            oil = maxOil;
            oilLeak = 0;
        }

        /// <summary>
        /// Nape�nia b�zyn�.
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
            Random r = new Random();
            outOfOilFaultyEngineTimerMax = (float)r.NextDouble() * 7 + 5;
            isEngineFaulty = false; 
            planeState = PlaneState.Intact;
            level.Controller.OnEngineRepaired(this);
        }

        /// <summary>
        /// Ile punkt�w prostok�ta samolotu jest powy�ej warto�ci argumentu.
        /// </summary>
        public int PointsAbove(float Y)
        {
            int pointsAbove = 0;
            for (int j = 0; j < Bounds.Peaks.Count; j++)
            {
                PointD p = Bounds.Peaks[j];
                if (p.Y >= Y)
                {
                    pointsAbove++;
                }
            }

            return pointsAbove;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Ilosc oleju: " + oil);
            builder.AppendLine("Pozycja: " + Center);

            return builder.ToString();
        }

        /// <summary>
        /// Ustawia dan� flag� wci�ni�tego przycisku klawiatury.
        /// </summary>
        /// <param name="flag">Okre�la kt�r� flag� ustawi�.</param>
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
        /// Informuje samolot, �e nale�y zacz�c zawracanie.
        /// </summary>
        /// <param name="turningTime">Ile czasu b�dzie trwa�o zawracanie. Mierzony w SEKUNDACH.</param>
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
        /// Informuje samolot, �e zawracanie si� sko�czy�o i nale�y zmieni� kierunek lotu.
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
                    Speed = minFlyingSpeed; //zmini�em Tomek
                    //movementVector = -1 * turningVector;
                    //blockAllInput = false;
                    spinned = false;
                }
                if (type == TurnType.Carrier)
                {
                    direction = (Direction) (-1*(int) direction);
                    //this.bounds.Rotate(angleOnCarrier);
                    if (planeType != PlaneType.B25)
                    {
                        Rotate(2*angleOnCarrier*(float) direction); //((NA LOTNISKOWCU))
                    }
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
        /// Informuje samolot, �e obracanie si� sko�czy�o i nale�y zmieni� kierunek lotu.
        /// </summary>
        public void OnSpinEnd()
        {
            if (planeState != PlaneState.Crashed)
            {
                HorizontalReflection();
                isChangingDirection = false;
                isBlockSpin = false;
                //UnblockMovementInput();
                isBlockLeft = false;
                isBlockRight = false;
                isBlockEngine = false;

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
            //Console.WriteLine(rotateValue);
            bool isTryingOut = (wheelsState == WheelsState.In);

            bool ok1 = isTryingOut ? Math.Abs(rotateValue) < Math.PI*0.3f  : true; // kiedy samolot ma duza predkosc katowa nie powinno sie dac wystawic
             
            return (planeState != PlaneState.Crashed &&
                    locationState != LocationState.AircraftCarrier &&
                    locationState != LocationState.CarrierTurningRound &&
                    landingState == LandingState.None &&
                    wheelsState != WheelsState.TogglingIn &&
                    wheelsState != WheelsState.TogglingOut &&
                    ok1 &&
                    //dodane przez Emila
                    planeState != PlaneState.Destroyed &&
                    (
                        isTryingOut && RelativeAngle <= maxWheelOutAngle && RelativeAngle >= -maxWheelOutAngle ||
                        !isTryingOut
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
        
        
        public void Crash(float terrainHeight, TileKind tileKind)
        {
         	Crash(terrainHeight, tileKind, null);
        }
 		
        /// <summary>
        /// Powoduje rozbicie samolotu o teren.
        /// </summary>
        /// <param name="terrainHeight">Wysoko�� terenu o kt�ry si� rozbi�.</param>
        /// <param name="tileKind">Rodzaj terenu o kt�ry si� rozbi�.</param>
        public void Crash(float terrainHeight, TileKind tileKind, IAttractorSource attractorSource)
        {
            float viewDifferance = 1; //warto�� o jak� trzeba przesun�� samolot po rozbiciu, �eby nie by� nda ziemi�
            if (planeState != PlaneState.Crashed)
            {
                locationState = LocationState.Air;
                //potrzebne, �eby obr�t samolotu dzia�a� poprawnie - spyta� si� Adama dlaczego tak jest
                if (planeState != PlaneState.Destroyed)
                    //je�li nie by� wcze�niej Destroyed, to wysy�am komunikat, �eby by�a dobra animacja w View
                    level.Controller.OnPlaneDestroyed(this);
                if(IsEnemy) level.EnemyPlanesLeft--;
                planeState = PlaneState.Crashed;
                //odwr�cenie samolotu do poziomu
                if (tileKind != TileKind.Ocean)
                {
                    float tempAngle;
                    if (RelativeAngle >= -Math.PI/2.0f && RelativeAngle <= Math.PI/2.0f)
                        tempAngle = -Angle; //zwyk�e po�o�enie w poziomie
                    else
                        tempAngle = Math.PI - Angle; //samolot do g�ry nogami
                    Rotate(tempAngle);

                    //sprawdza pozycje zderzenia z ziemia.
                    DestroyBunkersAndKillSoldiers();
                }
                StopEngine(-1);
                //wyzerowanie czasu do ko�ca �ycia
                wreckTimeElapsed = 0;

                isSinking = (tileKind == TileKind.Ocean); //|| (tileKind == TileKind.Ship); - moved to attractors to sync the sinking speed
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
                //zni�enie samolotu na poziom tile'a
                level.Controller.OnPlaneCrashed(this, tileKind);
                if(attractorSource != null) this.attractorTarget.AddAttractor(attractorSource, attractorSource.GetHashCode().ToString());
                
                
                
            }
        }

        /// <summary>
        /// Powoduje zniszczenie samolotu - wy��czenie silnika i utrat� kontroli.
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

                if (isEnemy) level.Statistics.PlanesShotDown++;
                
            }
        }

        public void Hit(float oilTaken, float oilLeak)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                this.oilLeak += oilLeak;
                oil -= oilTaken;
               // oil = System.Math.Max(oil, 0);
            }
        }

        /// <summary>
        /// Metoda wywo�ywana w momencie trafienia samolotu przez wroga.
        /// </summary>
        /// <param name="hitByPlane">Je�li true, to znaczy �e zosta� trafioy przez samolot, 
        /// w przeciwnym przypadku, zosta� trafiony przez dzia�o na wyspie.</param>
        public void Hit(bool hitByPlane)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                planeState = PlaneState.Damaged;
                if (hitByPlane) //ma�e trafienie
                {
                  
                    oil -= GameConsts.UserPlane.Singleton.HitCoefficient;
                    if(isEnemy)
                    {
                        oilLeak += 0.001f * MaxOil;
                        oil -= GameConsts.UserPlane.Singleton.HitCoefficient * 1.5f; // przeciwnik dostaje wiecej damage'u
                        if (planeType == Planes.PlaneType.B25)
                        {
                            oil -= GameConsts.UserPlane.Singleton.HitCoefficient * 1.5f; // lepsze dzia�ko
                        }
                    }                     
                    if(GameConsts.UserPlane.Singleton.PlaneCheat)
                    {
                        if(isEnemy)
                        {
                            oil -= GameConsts.UserPlane.Singleton.HitCoefficient * 1.5f; // lepsze dzia�ko
                        } else
                        {
                            oil += GameConsts.UserPlane.Singleton.HitCoefficient / 2.0f; // dwa razy mniejsze uszkodzenia
                        }                        
                    }
                }
                else
                {
                    oilLeak += 0.0007f * MaxOil;
                    if (planeType == Planes.PlaneType.B25)
                    {
                        oilLeak -= 0.0003f * MaxOil; // lepszy pancerz
                    }

                }
               // oil = System.Math.Max(oil, 0);
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
                    level.KillVulnerableSoldiers(Convert.ToInt32(System.Math.Floor(Position.X/LevelTile.TileWidth)), 2, true);
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
        /// Resetuje flagi wci�ni�tych przycisk�w.
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
        /// Odblokowywuje wszystkie flagi wej�ciowe.
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
        /// Przetwarza sygna�y z klawiatury, kt�re pojawi�y si� od ostatniego wywo�ania ProcessInput.
        /// </summary>
        /// <param name="time">Czas jaki up�yn�� od ostatniego wywo�ania ProcessInput. Wyra�ony w ms.</param>
        /// <param name="timeUnit">Warto�� czasu do kt�rej odnoszone s� wektor ruchu i warto�� obrotu. Wyra�ona w ms.</param>       
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
                {
                	 TryToStartEngine(time);
                }
                else
                if(locationState != LocationState.AirTurningRound) // sa problemy przy rownoczesnym zawracaniu i wylaczaniu silnika
                {
                	TryToStopEngine(scaleFactor);
                }
                    
            }
            else
                ResetEngineParameters();


            bool relativeUp = isUpPressed;
            bool relativeDown = isDownPressed;

            float joyScale = GetInputVectorValueNormalised(DirectionAxis.Vertical);
            float rotationFactor;  
            switch (locationState)
            {
                    // mozna zmienic k�t nawet podczas zawracania
                case LocationState.AirTurningRound:
                case LocationState.Air:

                    if (motorState == EngineState.SwitchedOff)
                        break;

                    if(locationState == LocationState.AirTurningRound)
                    {
                        // w czasie zawracania odwracamy gore z dolem (gdyz jeszcze nie zostal zmieniony direction a samolot juz jest w przeciwna strone)
                        // tu jest PROBLEM
                       /* if (Bounds.Center.Y > 0.9f * GameConsts.UserPlane.Singleton.MaxHeight * maxHeightTurningRange)
                        {
                            rotateValue = 0;
                            rotationFactor = 0.00f; // jesli jestesmy przy 'suficie' to nie mozemy zmieniac k�ta i zawracac na raz
                        }
                        else*/
                        {
                            rotationFactor = 0.10f;// jak spowolnione jest unoszenie/obni�anie dzioba podczas zawracania
                        }
                      
                        relativeUp = isDownPressed;
                        relativeDown = isUpPressed;


                    }
                    else
                    {
                        rotationFactor = 1.0f;

                        if (isLeftPressed || isRightPressed)
                        {
                            Direction steerDir = isLeftPressed ? Direction.Left : Direction.Right;
                            SteerHorizontal(steerDir, scaleFactor);
                        }
                        else
                        {
                            //hamowanie samolotu
                            float subSpeed = scaleFactor * GameConsts.UserPlane.Singleton.MoveStep;
                            subSpeedToMin(subSpeed, minFlyingSpeed);
                        }

                    }
                   
                    if (relativeUp)
                    {
                       
                        if (wheelsState == WheelsState.In || wheelsState == WheelsState.TogglingIn ||
                            RelativeAngle < landingAngle ||
                            ((isLeftPressed || isRightPressed) && RelativeAngle < maxWheelOutAngle))
                        {

                            IncreaseRotateValue(joyScale * rotationFactor * scaleFactor * (float)direction * rotateStep); //zwyk�y obr�t
                        }
                        else if (!isLeftPressed && !isRightPressed)
                            //je�eli �aden przycisk kierunku nie jest wci�ni�ty, samolot powinien obni�y� lot
                        {
                            isLanding = true;
                            rotateValue = 0; //samolot nie powinien si� dalej obraca�
                            if (RelativeAngle > landingAngle)
                            {
                                float tempAngle = - rotationFactor * 1.0f*(float) direction*(RelativeAngle - landingAngle);
                                Rotate(tempAngle);
                            }
                        }
                        isAfterFlyingDown = false;
                    }
                    if (relativeDown)
                    {
                     
                        if (wheelsState == WheelsState.In || RelativeAngle > -maxWheelOutAngle)
                        {
                            IncreaseRotateValue(- joyScale * rotationFactor * joyScale * scaleFactor * (float)direction * rotateStep);
                        }
                        isAfterFlyingDown = false;
                    }


                    if (!relativeDown && !relativeUp)//"hamowanie" obrotu
                        DecreaseRotateValue(rotateBrakingFactor*scaleFactor*rotateStep);


                    // nie mozna wykonac spinu podczas zawracania
                    if (locationState == LocationState.Air)
                    {
                        if (isSpinPressed && !isBlockSpin && CanSpin)
                        {
                            Spin();
                        }
                    }
                    
                    break;

                case LocationState.AircraftCarrier:
                    if (landingState == LandingState.None)
                    {
                        if (isLeftPressed || isRightPressed)
                        {
                            Direction steerDir = isLeftPressed ? Direction.Left : Direction.Right;
                            SteerHorizontal(steerDir, scaleFactor);
                        }
                        else //hamowanie samolotu (S�ABE)
                        {
                            float subSpeed = scaleFactor*GameConsts.UserPlane.Singleton.MoveStep;
                            float oldSpeed = movementVector.EuclidesLength;
                            float newSpeed = movementVector.EuclidesLength - subSpeed;

                            subSpeedToMin(subSpeed, 0);
                            changeAngleWhileWheeling(oldSpeed, newSpeed, scaleFactor);
                        }
                    }
                    break;
            }
            //zawsze resetuj� wszystkie flagi !!!

            ResetInputFlags();
        }

        /// <summary>
        /// Metoda zmienia pochylenie dziobu samolotu na lotniskowcu 
        /// gdy osi�gn�� odpowiedni� pr�dko��.
        /// </summary>
        /// <param name="oldSpeed"></param>
        /// <param name="newSpeed"></param>
        /// <author>Tomek , Kamil S�awi�ski</author>
        private void changeAngleWhileWheeling(float oldSpeed, float newSpeed , float scaleFactor)
        {
            if (CanFastWheeling)
            {
                //przyspieszy�
                if (oldSpeed < changeWheelsSpeed && changeWheelsSpeed <= newSpeed || isRaisingTail)
                {
                    raiseTailStep(scaleFactor);
                }
                    //zwolni�
                else if (newSpeed <= changeWheelsSpeed && changeWheelsSpeed < oldSpeed || isLoweringTail)
                    lowerTailStep(scaleFactor);
            }
            //zwolni� bo wy��czony silnik
            else if (newSpeed <= changeWheelsSpeed && changeWheelsSpeed < oldSpeed || isLoweringTail)
                lowerTailStep(scaleFactor);
        }

        /// <summary>
        /// Metoda obni�a ogon samolotu na lotniskowcu 
        /// Dzieje si� to p�ynnie , gdy w��czona jest flaga isLoweringTail
        /// </summary>
        /// <author>Tomek , Kamil S�awi�ski</author>
        private void lowerTailStep(float scaleFactor)
        {
            if (planeType == PlaneType.B25)
            {
                return;
            }
        	//bounds.Rotate(-bounds.Angle);
        	//return;
            isLoweringTail = true;
            isRaisingTail = false;
          

            int deltaCount = (int)(scaleFactor * 2000)+1;
            float scaleFactorDelta = scaleFactor / deltaCount;

            float rotateDelta = (float)(direction) * (angleOnCarrier * 6) * scaleFactor;

            for (int i = 0; i < deltaCount; i++)
            {
                bounds.Rotate(rotateDelta);
                if (Math.Abs(Math.Abs(bounds.Angle) - angleOnCarrier) < 0.02)
                {
                    isLoweringTail = false;
                    break;
                }
            }
            
            
        }

        /// <summary>
        /// Metoda podnosi ogon samolotu na lotniskowcu 
        /// Dzieje si� to p�ynnie , gdy w��czona jest flaga isRaisingTail
        /// </summary>
        /// <author>Tomek , Kamil S�awi�ski</author>
        private void raiseTailStep(float scaleFactor)
        {
            if (planeType == PlaneType.B25)
            {
                if (Math.Abs(bounds.Angle) < 0.02)
                {
                    AirstripContact();
                }
                return;
            }
        	//bounds.Rotate(-bounds.Angle);
        	//return;
            isRaisingTail = true;
            isLoweringTail = false;

            int deltaCount = (int)(scaleFactor * 2000)+1;
            float scaleFactorDelta = scaleFactor / deltaCount;

            float rotateDelta = -(float)(direction) * (4 * angleOnCarrier) * scaleFactorDelta;

            for (int i = 0; i < deltaCount; i++)
            {
                bounds.Rotate(rotateDelta);
                if (Math.Abs(bounds.Angle) < 0.02)
                {
                    isRaisingTail = false;
                    AirstripContact();
                    break;
                }
            }
        }

        /// <summary>
        /// Dodaje pr�dko�ci.
        /// Jesli dodana wieksza od maksymalnej ustawia maksymaln�.
        /// </summary>
        /// <param name="addSpeed"></param>
        /// <param name="maxSpeed"></param>
        /// <author>Tomek</author>
        private void addSpeedToMax(float addSpeed, float maxSpeed)
        {
            //sprawdzam pr�dko��, �eby nie wzros�a powy�ej maxymalnej
            if (movementVector.EuclidesLength + addSpeed > maxSpeed)
                addSpeed = maxSpeed - movementVector.EuclidesLength;
            movementVector.Extend(addSpeed);
            UpdateAirscrewSpeed();
        }

        /// <summary>
        /// Odejmuje pr�dko��i. Je��i nowa preko�� mniejsza od minimalnej ustawia minimalna.
        /// </summary>
        /// <param name="subSpeed"></param>
        /// <param name="minSpeed"></param>
        /// <author>Tomek</author>
        private void subSpeedToMin(float subSpeed, float minSpeed)
        {
            //sprawdzam pr�dko��, �eby nie spad�a poni�ej minimalnej
            if (movementVector.EuclidesLength - subSpeed < minSpeed)
                movementVector.EuclidesLength = minSpeed;
            else
                movementVector.Extend(-subSpeed);
            UpdateAirscrewSpeed();
        }

        protected enum GlideType
        {
            destroyed,
            heightLimit,
            glider
        } ;
 
        
 
        /// <summary>
        /// padanie/szybowanie samolotu 
        /// </summary>
        /// <param name="time">Czas jaki up�yn�� od ostatniego wywo�ania ProcessInput. Wyra�ony w ms.</param>
        /// <param name="timeUnit">Warto�� czasu do kt�rej odnoszone s� wektor ruchu i warto�� obrotu. Wyra�ona w ms.</param>
        /// <param name="glideType">Czy ma by� delikatne szybowanie czy 'kamien w wode'?</param>
        protected void FallDown(float time, float timeUnit, GlideType glideType)
        {         
            float scaleFactor = time / timeUnit;
            float oldAngle = movementVector.Angle;
            
            PointD tempMV = (PointD)movementVector.Clone();

            switch (glideType)
            {
                case GlideType.destroyed:
                        tempMV.Y -= gravitationalAcceleration * scaleFactor;
                        movementVector = tempMV;
                    break;

                case GlideType.heightLimit:

                        float yForce = 1;

                        if (locationState == LocationState.AirTurningRound && isChangingDirection)// jesli zawracamy
                        {
                        	
                        	// za pierwszym razem tylko
                        	if(!lastIsChangingDirection && isChangingDirection)
                        	{
                        	//	tempMV.Y = scaleFactor;
                        	}
                        	
                        	// zmniejszaj pionowy movementVector na poczatkowej fazie obrotu 
                        	if(turningTime - turningTimeLeft <0.05f * turningTime )
                        	{
                        		if(tempMV.Y > 0)
                        		{
                        			tempMV.Y -= scaleFactor;
                        			tempMV.Y = System.Math.Max(0, tempMV.Y);
                        		}
                        		
                        		if(tempMV.Y < 0)
                        		{
                        			tempMV.Y += scaleFactor;
                        			tempMV.Y = System.Math.Min(0, tempMV.Y);
                        		}
                        	}   
                        	
                        	// jesli zawracamy to mimo ze samolot sie juz prawie obrocil to direction ma stary wiec nalezy zmienic znak 
                        	// sily (od polowy obrotu)
                        	yForce  = -(float)Math.Cos((Math.PI * (turningTime - turningTimeLeft) / turningTime));
                        	yForce *= 0.2f;
                        
                        }
						
                        // spowolnij obrotu UP/DOWN przekazane przez gracza (oci�a�y samolot)
                        if(rotateValue >= 0)
                        {
                            rotateValue -= 2.5f * scaleFactor; // wytra� jedn� jednostk� (radian) obrotu w 1 sek.
                            rotateValue = System.Math.Max(0, rotateValue);
                        }
                        else
                        {
                            rotateValue += 2.5f * scaleFactor; // wytra� jedn� jednostk� (radian) obrotu w 1 sek.
                            rotateValue = System.Math.Min(0, rotateValue);
                        }
                       

                        tempMV.Y -= yForce * gravitationalAcceleration * scaleFactor / 2;
                        tempMV.Y -= yForce * 0.4f * ClimbingAngle / Math.PI * gravitationalAcceleration * scaleFactor;
                                       
                        
                        // spowalniamy
                        if (ClimbingAngle > 0) tempMV.X -= 0.12f * tempMV.X * scaleFactor * ClimbingAngle / Math.PI;

                    break;

                case GlideType.glider:
                        float val = 0;
                        if (rotateValue >= 0)
                        {
                            rotateValue -= 2.5f * scaleFactor; // wytra� jedn� jednostk� (radian) obrotu w 1 sek.
                            rotateValue = System.Math.Max(0, rotateValue);
                        }
                        else
                        {
                            rotateValue += 2.5f * scaleFactor; // wytra� jedn� jednostk� (radian) obrotu w 1 sek.
                            rotateValue = System.Math.Min(0, rotateValue);
                        }
                        val += gravitationalAcceleration * scaleFactor / 5;
                        if(ClimbingAngle > 0)
                        {
                            val += 0.4f * ClimbingAngle / Math.PI * gravitationalAcceleration * scaleFactor;
                        }
                       
                        tempMV.Y -= val;
                      
                       

                        // spowalniamy
                        if (ClimbingAngle > 0) tempMV.X -= 0.10f * tempMV.X * scaleFactor * ClimbingAngle / Math.PI;
                        
                    break;
            }
           

            float newAngle = tempMV.Angle;
            float rot = (newAngle - oldAngle);
            
            Rotate(rot);
			lastIsChangingDirection  = isChangingDirection;
        }

        

        /// <summary>
        /// Sprawdza czy kt�ry� z wierzcho�k�w ma Y mniejszy lub r�wny 0.
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
        /// Pr�ba w��czenia silnika
        /// </summary>
        /// <param name="time"></param>
        private void TryToStartEngine(float time)
        {
            if (!isEngineJustStopped)
            {
                if (CanEngineBeStartedNow)
                {
                    StartEngineCounter = 0; //zeruje licznik.
                    StartEngine(); //uruchamiam silnik.
                    isEngineJustStarted = true;
                    level.Controller.OnTurnOnEngine(true, this);
                }
                else
                {
                    StartEngineCounter += (int) time; //zwiekszam licznik prob odpalenia silnika.
                    level.Controller.OnStartEngineFailed();
                }
            }
        }

        /// <summary>
        /// Pr�ba wy��czenia silnika.
        /// (nie mozna wy��czy� silnika zaraz po uruchomieniu)
        /// </summary>
        private void TryToStopEngine(float scaleFactor)
        {
            if (!isEngineJustStarted)
            {
                StopEngine(scaleFactor);
                isEngineJustStopped = true;
            }
        }

        /// <summary>
        /// ustawia parametry silnika na stan pocz�tkowy.
        /// </summary>
        private void ResetEngineParameters()
        {
          
            isEngineJustStarted = false;
            isEngineJustStopped = false;
            counterStartedEngine = 0;
        }

        /// <summary>
        /// Zwraca startow� pozycje samolotu.
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
                    movementVector.EuclidesLength/GameConsts.UserPlane.Singleton.MaxSpeed*(maxAirscrewSpeed - minAirscrewSpeed) +
                    minAirscrewSpeed;
            else
                airscrewSpeed = 0.0f;
        }


        

        /// <summary>
        /// Powoduje wy��czenie silinika i utrat� kontroli po tym jak sko�czy si� olej lub paliwo.
        /// </summary>
        protected void OutOfPetrolOrOil(float scaleFactor)
        {
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed)
            {
                if (!isEnemy && petrol > 0 && oil > -engineUnstartableOilThreshold * maxOil)
                {
                    // jesli mamy benzyne i nie ma oleju (ale nie jest jakas masakra)
                    // to user ma mozliwosc ponownie odpalic silnik
                    if(!isEngineFaulty)
                    {
                      // Console.WriteLine("silnik rozwalony");
                        isEngineFaulty = true;
                        level.Controller.OnEngineFaulty(this);
                    }
                    outOfOilFaultyEngineTimer += scaleFactor;
                    if (locationState == Planes.LocationState.Air && outOfOilFaultyEngineTimer >= outOfOilFaultyEngineTimerMax)
                    {
                        //Console.WriteLine("i gasze silnik");
                        // planeState = PlaneState.Destroyed;
                        StopEngine(scaleFactor);
                        outOfOilFaultyEngineTimer = 0;
                        Random r = new Random();
                        outOfOilFaultyEngineTimerMax = (float)r.NextDouble() * 7 + 5;
                        rotateValue = 0;
                    }
                }
                else
                {
                    planeState = PlaneState.Destroyed;
                    StopEngine(scaleFactor);
                    outOfOilFaultyEngineTimer = 0;
                    rotateValue = 0;
                    
                }
               
              
            }
        }

        #region Landing Methods

        
      
        /// <summary>
        /// Sprawdza czy samolot znajduje si� za samolotami na lotniskowcu
        /// </summary>
        private bool IsPotentiallyLanding
        {
            get
            {
            	bool cond1 = this.IsEngineWorking && this.locationState == LocationState.Air && wheelsState == WheelsState.Out && direction == Direction.Left;
            	Carrier c = Carrier;            	
            	if(!cond1) return false;

                float dist = Center.X - c.GetEndPosition().X; // c.GetEndPosition().Y jest liczone jak dla widoku
               // Console.WriteLine((Center.Y - c.Height) + "  > 0 ");
                return dist > 0 && dist <= potentiallyLandingMaxDistance && (Center.Y - c.Height) > 0;
            	
            }
        }

        private bool IsPotentiallyBadLanding
        {
            get
            {
            	bool cond1 = this.IsEngineWorking && this.locationState == LocationState.Air && wheelsState == WheelsState.Out && direction == Direction.Right;
            	Carrier c = Carrier;            	
            	if(!cond1) return false;

                float dist = c.GetBeginPosition().X - Center.X; // c.GetEndPosition().Y jest liczone jak dla widoku
               // Console.WriteLine((Center.Y - c.Height) + "  > 0 ");
                return dist > 0 && dist <= potentiallyBadLandingMaxDistance;
            	
            }
        }
        
        /// <summary>
        /// Proces l�dowania na lotniskowcu.
        /// Ustawia samolot gdy podchodzi do l�dowania.
        /// Odpowiada za ko�owanie na lotniskowcu.
        /// Hamowanie przez lin� chamuj�c�
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        public void LandingProcess(float time, float timeUnit)
        {
        	//ZBLIZAMY SIE DO LOTNISKOWCA
        	if(!isLandingHintDelivered && IsPotentiallyLanding)        		
        	{
        		level.Controller.OnPotentialLanding(this);
        	}

            if(IsPotentiallyBadLanding)        		
        	{
                level.Controller.OnPotentialBadLanding(this);
        	}
            
        	
        	// schowanie podwozia powoduje ze mozliwe jest ponowne pokazanie komunikatu o ladowaniu
        	if(this.wheelsState == WheelsState.In)
        	{
        		isLandingHintDelivered = false;
        	}
        	
        	
            //SAMOLOT PODCHODZI DO L�DOWANIA
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

            //ZMIANA KATA NA ZEROWY
            if (landingState == LandingState.InitWheeling)
                InitLandingWheeling(time, timeUnit);

            //KO�OWANIE PO LOTNISKOWCU
            if (landingState == LandingState.Wheeling)
                LandingWheeling();

            //HAMOWANIE PRZEZS LIN�
            if (landingState == LandingState.Breaking)
                LandingBreaking(time, timeUnit);
        }

        /// <summary>
        /// Inisjalizuje ko�owanie w czasie l�dowania.
        /// ustawia samolotu na wzd�u� lotniskowca.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void InitLandingWheeling(float time, float timeUnit)
        {
            float scaleFactor = time/timeUnit;
            AirstripContact();
            BlockMovementInput();
            rotateValue = 0;

            if (System.Math.Abs(Bounds.Angle) <= scaleFactor*rotateStep)
            {
                Bounds.Rotate((-1)*Bounds.Angle);
                landingState = LandingState.Wheeling;
            }
            else
                Bounds.Rotate((-1)*(System.Math.Sign(Bounds.Angle))*scaleFactor*rotateStep); //zmieni� na sta�� 
        }


        /// <summary>
        /// Ko�owanie po lotniskowcu w czasie lotu.
        /// </summary>
        private void LandingWheeling()
        {
            AirstripContact();
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
                    (direction == Direction.Right && //zeby nie wybucha� pop prawej stronie
                     Bounds.LeftMostX >= level.Carrier.GetEndPosition().X + LevelTile.TileWidth + 4))
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
            if (Speed <= GameConsts.UserPlane.Singleton.BreakingMinSpeed)
            {
                Speed = 0;
                level.Controller.OnReleasePlane(this, breakingEndCarrierTile);
            }
        }

        /// <summary>
        /// Wywo�ywana po zwolnieniu przez kontroler lin hamuj�cych.
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
        /// tzn nie pozwala mu unie�� si� w g�re, poprzez ustawienie 
        /// rectangla samolotu wzd�u� pasa startowego.
        /// </summary>
        public void AirstripContact()
        {
            float delta = level.Carrier.Height - Bounds.LowestY;
            Bounds.Move(0, delta);
            movementVector.Y = 0;
        }

        #endregion

        /// <summary>
        /// Funkcja ograniczaj�ca lot samolotu w g�re
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        protected void HeightLimit(float time, float timeUnit)
        {
            float rot = time/timeUnit*rotateStep*2;

            if (locationState == LocationState.CarrierTurningRound)
                return;

            //Console.WriteLine("angle:" + Angle);

            if (Bounds.Center.Y > GameConsts.UserPlane.Singleton.MaxHeight*maxHeightTurningRange)
            {
                FallDown(time, timeUnit, GlideType.heightLimit);
                level.OnPlaneForceGoDown(this);
            }
            
        }
     

        /// <summary>
        /// Funkcja ograniczaj�ca lot samolotu w prawo i w lewo
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        protected void VerticalBoundsLimit(float time, float timeUnit)
        {
            if (isMaxHeightRotate)
                return;

            bool normalFlight = (0 <= System.Math.Abs(Bounds.Angle) && System.Math.Abs(Bounds.Angle) <= System.Math.PI/2);
            bool upSideDownFlight = (System.Math.PI/2 < System.Math.Abs(Bounds.Angle) &&
                                     System.Math.Abs(Bounds.Angle) <= 3.1416f); //3.1416f to zaokr�glenie numeryczne System.Math.PI

            if (Mathematics.PositionToIndex(Bounds.Center.X) <= turningTileMargin)
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
            if (Mathematics.PositionToIndex(Bounds.Center.X) >= level.LevelTiles.Count - turningTileMargin)
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
        /// Funkcja odpowiadaj�ca za opuszczenie lotniskowca 
        /// w czasie startu
        /// </summary>
        private void LeaveCarrier(float time, float timeUnit)
        {
            float scaleFactor = time / timeUnit;

            // co si� dzieje zaraz po prawid�owym starcie
            if (isJustAfterTakeOff)
            {       	
            	
            	// zakonczyla sie procedura startu. Przestala dzialac sila sciagajaca w dol. Nalezy "zresetowac" wektor ruchu, gdyz zostal on odklejony od
            	// "strugi" przez sile sciagajaca do ziemi
                if(justAfterTakeOffTimer > justAfterTakeOffTimerMax)
                {
                    justAfterTakeOffTimer = 0;
                    isJustAfterTakeOff = false;                   
                    movementVector = new PointD(minFlyingSpeed * (int)direction * Math.Cos(bounds.Angle), minFlyingSpeed * (int)direction * Math.Sin(bounds.Angle));
                   
                    
                } else
                {
                	// sciaganie do ziemi
                    //Console.WriteLine(time);
                    if (justAfterTakeOffTimer < 0.5f * justAfterTakeOffTimerMax)
                    { 
                    	// spadanie (szybciej)
                    	float temp = -40.5f * scaleFactor * Math.Sin(Math.HALF_PI +  Math.TWO_PI * (justAfterTakeOffTimer / justAfterTakeOffTimerMax));
                    	//temp = 0;
                    
                        movementVector.Y += temp;
                    } else
                    {
                        // unoszenie (wolniej)                        
                        float temp = - 5.0f * scaleFactor * Math.Sin(Math.HALF_PI + Math.TWO_PI * (justAfterTakeOffTimer / justAfterTakeOffTimerMax));
                        //temp = 0;                      
                        movementVector.Y += temp;
                    }
                    
                    justAfterTakeOffTimer += scaleFactor;
                }
                
            }

            if (locationState == LocationState.AircraftCarrier && landingState == LandingState.None)
            {
                if (!isFallingFromCarrier && !IsGearAboveCarrier)
                {
                    
               ///     Console.WriteLine("speed:  " + maxFastWheelingSpeed + " =  " + GameConsts.UserPlane.Singleton.RangeFastWheelingMaxSpeed + " * " + GameConsts.UserPlane.Singleton.MaxSpeed);
                    
               //     Console.WriteLine("speed:  " + movementVector.EuclidesLength +" > "+ maxFastWheelingSpeed);
            
                    
                    if (HasSpeedToStart)
                    {                   
                        if (IsPlaneNotAboveCarrier)
                        {
                            
                            if (direction == Direction.Right)
                            {
                                level.Controller.OnPlaneWrongDirectionStart();
                                oil -= rightTakeOffOilLoss;
                                this.isJustAfterTakeOff = true;
                            } else
                            {
                                this.isJustAfterTakeOff = true;
                            }                          
                            locationState = LocationState.Air;
                           
                            level.Controller.OnTakeOff();

                            // automatycznie chowaj podwozie na easy 
                            if (EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy && level.UserPlane.WheelsState == WheelsState.Out)
                            {
                                level.OnToggleGear();
                            }
                        }
                    }
                    else
                    {
                        
                        raiseTailStep(scaleFactor);
                        isFallingFromCarrier = true;
                        isSlippingFromCarrier = Speed <= maxSlippingFromCarrierSpeed;
                        AirstripContact();
                        BlockMovementInput();
                    }
                }
            }
            FallFromCarrier(time, timeUnit);
        }

        /// <summary>
        /// Metoda wywo�ywana gdy samolot spada z lotniskowca.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void FallFromCarrier(float time, float timeUnit)
        {
            float scaleFactor = time/timeUnit;
            float peakX = (direction == Direction.Left)
                              ? Carrier.GetBeginPosition().X
                              : Carrier.GetEndPosition().X + LevelTile.TileWidth;
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
                        FallDown(time, timeUnit, GlideType.destroyed);
                }
            } else
            {
                
            }
        }

        /// <summary>
        /// Powoduje toni�cie samolotu lub tylko odczekanie odpowiedniej ilo�ci czasu.
        /// </summary>
        /// <param name="time">Czas od ostatniego wywo�ania metody. Mierzony w ms. </param>
        /// <param name="timeUnit">Warto�� czasu do kt�rej odnoszone s� wektor ruchu i warto�� obrotu. Wyra�ona w ms.</param>
        protected void MoveAfterCrash(float time, float timeUnit)
        {
            if (wreckTimeElapsed > wreckTime) //koniec czasu
            {
            	wreckTimeElapsed = 0;
            	// czyscimy atraktory
            	this.attractorTarget.ClearAttractors();
            	
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
                    if (bounds.LowestY <= OceanTile.waterDepth)
                    {
                        isFallingAfterCrash = false;
                        isSinking = true;
                    }
                }
            }

            wreckTimeElapsed += time;
        }

        /// <summary>
        /// Toni�cie samolotu
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        private void Sink(float time, float timeUnit)
        {
            //czy pod wod� nie ma ju� wyspy
            int index = Mathematics.PositionToIndex(Center.X);
            if (0 <= index && index < level.LevelTiles.Count)
            {
                LevelTile tile = level.LevelTiles[index];
                if (tile.TileKind == TileKind.Island)
                    movementVector.X = 0;
                if (tile.TileKind == TileKind.Ship)
                    movementVector.X = 0;
            }

            //aktualizacja movmentVector.X
            if (((float) direction*bounds.Angle) < 0 && movementVector.X != 0)
                //jesli skierowany do do�u && predkosc X nie jest zerem
                movementVector.X = (movementVector.X < 0)
                                       ? System.Math.Min(movementVector.X + waterXBreakingPower, 0)
                                       :
                                           System.Math.Max(movementVector.X - waterXBreakingPower, 0);
            else
                movementVector.X = 0;

            //aktualizacja movmentVector.Y
            movementVector.Y = (movementVector.Y >= 0)
                                   ? - GameConsts.UserPlane.Singleton.SinkingSpeed
                                   :
                                       System.Math.Min(movementVector.Y + waterYBreakingPower,
                                                       -GameConsts.UserPlane.Singleton.SinkingSpeed);

            bounds.Move((time/timeUnit)*movementVector);
        }

        /// <summary>
        /// Obraca samolot (i wektor ruchu) zgodnie z aktualn� warto�ci� obrotu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void UpdatePlaneAngle(float scaleFactor)
        {
            //rotateValue = rotateValue * (-Mogre.Math.Sin(RelativeAngle)/7.6f + 1.099f);

            if (wheelsState == WheelsState.Out || wheelsState == WheelsState.TogglingOut)
                if (
                    (RelativeAngle >= maxWheelOutAngle && (float) direction*rotateValue > 0) ||
                    (RelativeAngle <= -maxWheelOutAngle && (float) direction*rotateValue < 0)
                    )
                    rotateValue = 0;
           
            Rotate(scaleFactor*rotateValue);
        }

        /// <summary>
        /// Zwi�ksza (lub zmniejsza w zale�no�ci od znaku parametru value) warto�� rotateValue.
        /// Nowa warto�� jest obcinana do warto�ci maksymalnej
        /// </summary>
        /// <param name="value">O ile nale�y zwi�kszy� warto�� bezwzgl�dn� rotateValue</param>
        private void IncreaseRotateValue(float value)
        {
            rotateValue += value;
            if (value >= 0)
                rotateValue = System.Math.Min(rotateValue, maxRotateValue); //nie mo�e przekroczy� warto�ci max
            else
                rotateValue = System.Math.Max(rotateValue, -maxRotateValue); //nie mo�e by� mniejsza od -max
        }

        /// <summary>
        /// Ustawia ostatni wektor joysticka
        /// </summary>
        /// <param name="inputVector"></param>
        public void UpdateInputVector(Vector2? inputVector)
        {
            this.inputVector = inputVector;
        }


        /// <summary>
        /// Zmniejsza (lub zwi�ksza zale�no�ci od znaku parametru value) warto�� rotateValue.
        /// Nowa warto�� nie mo�e zmieni� znaku - jest obcinana do warto�ci 0.
        /// </summary>
        /// <param name="value"></param>
        private void DecreaseRotateValue(float value)
        {
            if (Angle == 0)
                return;

            if (rotateValue >= 0)
            {
                rotateValue -= value;
                rotateValue = System.Math.Max(rotateValue, 0); //nie mo�e spa�� poni�ej 0
            }
            else
            {
                rotateValue += value;
                rotateValue = System.Math.Min(rotateValue, 0); //nie mo�e wskoczy� powy�ej 0
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

        public PlaneType PlaneType
        {
            get { return planeType; }
        }

        /// <summary>
        /// Ilo�� oleju tracona, gdy samolot zostanie trafiony
        /// 
        /// </summary>
        public float OilLeak
        {
            get { return oilLeak; }
        }

        #endregion

        
      
    }
    #endregion
}