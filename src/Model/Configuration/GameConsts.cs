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

using Mogre;

namespace Wof.Model.Configuration
{
    /// <summary>
    /// DOMYŚLNE wartości zmiennych które zaciągane są z plików XML
    /// </summary>
    public static class GameConsts
    {
        public class UserPlane
        {
            #region Fields

            ///<summary>
            /// Ilość oleju, która wycieka z samolotu gracza w czasie timeUnit.
            /// </summary>
            public static float OilLoss = 1;

            /// <summary>
            /// Wartosc o jaka bedzie zwiekszany(zmniejszany) wektor ruchu po nacisnieciu strzalki.
            /// </summary>
            public static float MoveStep = 10;

            /// <summary>
            /// Maxsymalna prędkośc samolotu jaką może wyciągnąć
            /// </summary>
            public static float MaxSpeed = 80;

            /// <summary>
            /// Ilość benzyny, którą samolot spala w czasie timeUnit.
            /// </summary>
            public static float PetrolLoss = 1;

            /// <summary>
            /// Liczba dostepnych bomb po jednorazowym pobraniu amunicji.
            /// </summary>
            public static int BombCount = 30;

            /// <summary>
            /// Liczba dostepnych rakiet po jednorazowym pobraniu amunicji.
            /// </summary>
            public static int RocketCount = 15;

            
            /// <summary>
            /// Liczba dostepnych torped po jednorazowym pobraniu amunicji.
            /// </summary>
            public static int TorpedoCount = 3;

         

            /// <summary>
            /// Stała mówi ile razy moveStep jest większy w czasie hamowannia na lotniskowcu.
            /// (Gdy jest przysikana strzałka przeciwna do ruchu samolotu.)
            /// </summary>
            public static float BreakingPower = 4;

            /// <summary>
            /// Ilość oleju tracona, gdy samolot zostanie trafiony. Dodawana
            /// do ogolnej ilosci traconego oleju po kazdym trafieniu.
            /// </summary>
            public static float HitCoefficient = 0.4f;

            /// <summary>
            /// Określa jaką częścią maksymalnej prędkości jest prędkość przy wolnym kołowaniu.
            /// </summary>
            public static float RangeSlowWheelingSpeed = 0.004f;

            /// <summary>
            /// Określa jaką częścia maksymalnej prędkości jest prędkoc przy szybkim kołowaniu.
            /// Jednocześnie określa jaką cześcią prędkości maksymalnej jest prędkość minimalna lotu.
            /// </summary>
            public static float RangeFastWheelingMaxSpeed = 0.3f;

            /// <summary>
            /// Prog, po przekroczeniu ktorego zostaje uruchomiony silnik. Czas podany w milisekundach.
            /// </summary>
            public static int EngineCounterThreshold = 2000;

            /// <summary>
            /// Prog, po przekroczeniu ktorego zostaje uruchomiony silnik, gdy samolot unosi sie
            /// w powietrzu. Czas podany w milisekundach.
            /// </summary>
            public static int EngineCounterThresholdInAir = 500;

            /// <summary>
            /// Prędkośc samolotu w czasie lądowania poniżej której samolot sie zatrzymuje.
            /// </summary>
            public static int BreakingMinSpeed = 5;

            /// <summary>
            /// Określa z jaką prędkością samolot będzie tonąć. Wyrażona jako liczba dodatnia.
            /// </summary>
            public static float SinkingSpeed = 1.3f;

            /// <summary>
            /// Maksymalna wysokość na jaką samolot może się wzbić.
            /// </summary>
            public static int MaxHeight = 100;

            /// <summary>
            /// Składowa Y wektora ruchu, gdy samolot ląduje.
            /// </summary>
            public static float LandingSpeed = 2;

            /// <summary>
            /// Określa jak bardzo będzie się zmieniała wartość obrotu w czasie timeUnit
            /// po naciśnięciu strzałki.
            /// </summary>
            public static float UserRotateStep = Math.PI/1.5f; // 2.0943951

            /// <summary>
            /// Siła hamowania obrotu, czyli ile razy szybcieh hamowana jest wartość obrotu.
            /// niż zwiększana
            /// </summary>
            public static float UserRotateBrakingFactor = 1.5f;

            /// <summary>
            /// Maksymalna wartość o jaką może obrócić się samolot w czasie timeUnit.
            /// </summary>
            public static float UserMaxRotateValue = Math.PI/1.5f; // 2.0943951

            /// <summary>
            /// Określa czy samolot gracza reaguje na brak paliwa i oleju.
            /// </summary>
            public static bool GodMode = false;


            /// <summary>
            /// Czy włączony jest 'PlaneCheat' - cheat
            /// </summary>
            public static bool PlaneCheat = false;


            /// <summary>
            /// Szerokość prostokąta ograniczającego samolot
            /// </summary>
            public static float Width = 5.5f;

            /// <summary>
            /// Wysokość prostokąta ograniczającego samolot
            /// </summary>
            public static float Height = 2.1f;

            #endregion
        }

        public class EnemyPlane
        {
            #region Consts

            /// <summary>
            /// Maksymalna ilość samolotów wroga
            /// </summary>
            public static float MaxSimultaneousEnemyPlanes = 3;

            /// <summary>
            /// Prędkość samolotu wroga.
            /// </summary>
            public static float Speed = 36;

            /// <summary>
            /// Ilość oleju, która wycieka z samolotu wroga w czasie timeUnit.
            /// </summary>
            public static float OilLoss = 1;

            /// <summary>
            /// Najmniejszy pułap na jakim może latać samolot wroga.
            /// </summary>
            public static float MinPitch = 10;

            /// <summary>
            /// Określa maksymalną odległość wroga od gracza (na osi X), w której może atakować.
            /// Dodane, żeby samolot wroga strzelał z w miarę bliskiej odległości.
            /// </summary>
            public static float ViewRange = 80;

            /// <summary>
            /// Liczba dostepnych rakiet.
            /// </summary>
            public static int RocketCount = 5;

            /// <summary>
            /// Czas jaki musi minąć, żeby samolot mógł wystrzelić kolejną rakietę.
            /// Wyrażony w ms.
            /// </summary>
            public static int NextRocketInterval = 1000;

            /// <summary>
            /// Okresli o ile metrów samolot wroga może się pomylić w ataku na dodatkowe samoloty.
            /// </summary>
            public static int StoragePlaneDistanceFault = 16;

            /// <summary>
            /// Minimalna odległość na którą może się zbliżyć do samolotu gracza.
            /// Lecąc naprzeciwko.
            /// </summary>
            public static int SafeUserPlaneDistance = 30;

            /// <summary>
            /// Okresla kiedy samolot wroga moze zaatakować(wystrzelic rakiete) do samolotu na lotniskowcu.
            /// </summary>
            public static float AttackStoragePlaneDistance = 20.5f;

            /// <summary>
            /// Określa na jakiej odlełości od lotniskowca włacza się alarm.
            /// </summary>
            public static float CarrierDistanceAlarm = 1200;

            /// <summary>
            /// Jak szybka zmienia kąt samolot wroga.
            /// </summary>
            public static float EnemyRotateStep = Math.PI/3; // 1.04719755

            /// <summary>
            /// Domyślna wartość czasu po jakim pojawi się pierwszy wrogi samolot.
            /// </summary>
            public static int DefaultTimeToFirstEnemyPlane = 1 * 60 * 1000;

            /// <summary>
            /// Domyślna wartość czasu po jakim pojawi się kolejny wrogi samolot.
            /// </summary>
            public static int DefaultTimeToNextEnemyPlane = 1 * 60 * 1000; 
            #endregion
        }

        public class Soldier
        {
            #region Consts

            /// <summary>
            /// Minimalna predkosc zolnierza.
            /// </summary>
            public static int MinSpeed = 4;

            /// <summary>
            /// Maksymalna predkosc zolnierza.
            /// </summary>
            public static int MaxSpeed = 7;

            /// <summary>
            /// Czas jaki musi uplynac, aby zolnierz mogl wejsc
            /// ponownie do bunkra.
            /// </summary>
            public static float HomelessTime = 10000;

            #endregion
        }

        public class WoodenBunker
        {
            #region Consts

            /// <summary>
            /// Odstep pomiedzy strzalami.
            /// </summary>
            public static float FireDelay = 800;

            /// <summary>
            /// Szerokosc pola widzenia samolotu.
            /// </summary>
            public static float HorizonHeight = 60;

            /// <summary>
            /// Wysokosc pola widzenia.
            /// </summary>
            public static float HorizonWidth = 30;

            #endregion
        }


        public class ShipWoodenBunker
        {
            #region Consts

            /// <summary>
            /// Odstep pomiedzy strzalami.
            /// </summary>
            public static float FireDelay = 800;

            /// <summary>
            /// Szerokosc pola widzenia samolotu.
            /// </summary>
            public static float HorizonHeight = 60;

            /// <summary>
            /// Wysokosc pola widzenia.
            /// </summary>
            public static float HorizonWidth = 30;

            #endregion
        }


        public class ConcreteBunker
        {
            #region Consts

            /// <summary>
            /// Odstep pomiedzy strzalami.
            /// </summary>
            public static float FireDelay = 800;

            /// <summary>
            /// Szerokosc pola widzenia.
            /// </summary>
            public static float HorizonHeight = 65;

            /// <summary>
            /// Wysokosc pola widzenia.
            /// </summary>
            public static float HorizonWidth = 35;

            #endregion
        }

        public class FortressBunker
        {
            #region Consts

            /// <summary>
            /// Odstep pomiedzy strzalami.
            /// </summary>
            public static float FireDelay = 800;

            /// <summary>
            /// Szerokosc pola widzenia.
            /// </summary>
            public static float HorizonHeight = 65;

            /// <summary>
            /// Wysokosc pola widzenia.
            /// </summary>
            public static float HorizonWidth = 35;

            #endregion
        }

        public class ShipConcreteBunker
        {
            #region Consts

            /// <summary>
            /// Odstep pomiedzy strzalami.
            /// </summary>
            public static float FireDelay = 800;

            /// <summary>
            /// Szerokosc pola widzenia samolotu.
            /// </summary>
            public static float HorizonHeight = 60;

            /// <summary>
            /// Wysokosc pola widzenia.
            /// </summary>
            public static float HorizonWidth = 30;

            #endregion
        }

        public class Bomb
        {
            #region Consts

            /// <summary>
            /// Przedzial czasu, jaki musi uplynac, aby gracz mogl zrzucic nastepna bombe.
            /// Podany w milisekundach(ms).
            /// </summary>
            public static int FireInterval = 200;

            /// <summary>
            /// Przyciaganie ziemskie.
            /// </summary>
            public static float Gravitation = 1.05f;

            /// <summary>
            /// Wspolczynnik oporu powietrza.
            /// </summary>
            public static float AirResistance = 0.97f;

            /// <summary>
            /// Przerwa pomiedzy kazdym przyrostem przyspieszenia.
            /// </summary>
            public static int AccelerationInterval = 100;

            #endregion
        }

        public class Rocket
        {
            #region Consts

            /// <summary>
            /// Przedzial czasu, jaki musi uplynac, aby gracz mogl zrzucic nastepna rakiete.
            /// Podany w milisekundach(ms).
            /// </summary>
            public static int FireInterval = 350;

            /// <summary>
            /// Maksymalna predkosc pozioma rakiety.
            /// </summary>
            public static int BaseSpeed = 30;

            #endregion
        }

        public class Torpedo
        {
            #region Consts

            /// <summary>
            /// Przedzial czasu, jaki musi uplynac, aby gracz mogl zrzucic nastepna torpedę.
            /// Podany w milisekundach(ms).
            /// </summary>
            public static int FireInterval = 350;

            /// <summary>
            /// Maksymalna predkosc pozioma rakiety.
            /// </summary>
            public static int BaseSpeed = 30;

            #endregion
        }

        public class Effects
        {
            #region Consts

            /// <summary>
            /// Przedział czasu jaki musi uplynac aby efekt BulletTime zwiekszyl sie o jedna jednostke.
            /// </summary>
            public static int BulletLoadTime = 1000;

            #endregion
        }
       
        public class Game
        {
            #region Consts

            /// <summary>
            /// Czy ma być aktywny cheat 'AllLevels'
            /// </summary>
            public static bool AllLevelsCheat = false;

            /// <summary>
            /// Czy ma być aktywny cheat 'Lives'
            /// </summary>
            public static bool LivesCheat = false;

            

            #endregion
        }
    }
}