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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Wof.Controller;
using Wof.Model.Configuration;
using Wof.Model.Level.Carriers;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Troops;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.XmlParser;
using Wof.Statistics;

namespace Wof.Model.Level
{
    public enum DayTime
    {
        Dawn,
        Noon,
        Night
    } ;

    /// <summary>
    /// Klasa reprezentujaca poziom gry.
    /// </summary>
    public class Level : IDisposable
    {
        #region Constants

        /// <summary>
        /// Okre�la do jakiej jednostki czasu b�d� odnoszone wektory pr�dko�ci. Je�li wektor
        /// ruchu ma d�ugo�� n tzn. �e obiekt b�dzie si� porusza� o n jednostek w czasie timeUnit.
        /// Warto�� wyra�ona w milisekundach.
        /// </summary>
        private const float timeUnit = 1000;

        /// <summary>
        /// Czas do pojawienia si� pierwszego samolotu.
        /// Wyra�ony w ms.
        /// </summary>
        private const float timeToNextEnemy = 1*60*1000;

        /// <summary>
        /// Czas do pojawienia si� nast�pnego samolotu.
        /// Wyra�ony w ms.
        /// </summary>
        private const float timeToFirstEnemy = 1*1*1000;

        #endregion

        #region fields

        /// <summary>
        /// Dane zapisane w pliku xml.
        /// </summary>
        private XmlLevelParser levelParser = null;

        /// <summary>
        /// Liczba zyc.
        /// </summary>
        private int lives;

        /// <summary>
        /// Samolot gracza.
        /// </summary>
        private readonly Plane userPlane;

        /// <summary>
        /// Lista z zolnierzami na planszy.
        /// </summary>
        private List<Soldier> soldierList;

        /// <summary>
        /// Lista samolotow przeciwnika.
        /// </summary>
        private readonly List<Plane> enemyPlanes;

        /// <summary>
        /// Lista bomb i rakiet na planszy.
        /// </summary>
        private List<Ammunition> ammunitionList;

        /// <summary>
        /// Lista bunkrow.
        /// </summary>
        private List<LevelTile> bunkersList;

        /// <summary>
        /// Lista wrogich jednostek na planszy.
        /// </summary>
        private List<LevelTile> enemyInstallationTiles;

        /// <summary>
        /// Lista czesci lotniskowca.
        /// </summary>
        private List<AircraftCarrierTile> aircraftTiles;

        /// <summary>
        /// Obiekt lotniskowca.
        /// </summary>
        private Carrier carrier;

        /// <summary>
        /// Jesli paliwo i olej sa powyzej zera, true,
        /// w przeciwnym razie false.
        /// </summary>
        private bool isEventAboutDestroySend = true;

        /// <summary>
        /// Informacja o polozeniu wysp z bunkrami wroga,
        /// wzgledem lotniskowca.
        /// </summary>
        private FlyDirectionHint flyDirectionHint;

        /// <summary>
        /// Liczba zolnierzy na planszy.
        /// </summary>
        private volatile int mSoldierCount;

        /// <summary>
        /// Obiekt przetwarzajacy informcacje.
        /// </summary>
        private readonly IController controller;

        /// <summary>
        /// Licznik uplynietego czasu od ostatniego wystrzalu.
        /// </summary>
        private int lastFireTick = Environment.TickCount;

        private List<StoragePlane> storagePlanes;

        public List<StoragePlane> StoragePlanes
        {
            get { return storagePlanes; }
            set { storagePlanes = value; }
        }

        /// <summary>
        /// Okre�la ile czasu pozosta�o do pojawienia si� nast�pnego samolotu
        /// </summary>
        private float currentTimeToNextEnemy = timeToFirstEnemy;

        /// <summary>
        /// Okre�la ile samolot�w pozosta�o w danym levelu.
        /// </summary>
        private int enemiesLeft;

        /// <summary>
        /// Statystyki poziomu
        /// </summary>
        private LevelStatistics mStatistics;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Publiczny konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="fileName">Nazwa pliku.</param>
        /// <author>Michal Ziober</author>
        public Level(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new IOException("File name must be set !");
            ReadEncodedXmlFile(fileName);
            SetAttributesForInstallations();
            soldierList = new List<Soldier>();
            bunkersList = new List<LevelTile>();
            ammunitionList = new List<Ammunition>();
            aircraftTiles = new List<AircraftCarrierTile>();
            enemyInstallationTiles = levelParser.Tiles.FindAll(Predicates.FindAllEnemyInstallationTiles());
            GetSoldiersCount(enemyInstallationTiles);
            bunkersList = levelParser.Tiles.FindAll(Predicates.GetAllBunkerTiles());
            mStatistics = new LevelStatistics();

            SetAircraftCarrierList();
            carrier = new Carrier(aircraftTiles);
            lives = 3;
            userPlane = new Plane(this, false);
            userPlane.RegisterWeaponEvent += userPlane_RegisterWeaponEvent;
            SetFlyDirectionHint();
            //dodane przez Emila
            //this.enemyPlane = new EnemyPlane(this, new PointD(100, 40), Direction.Right);
            //this.enemyPlane = new EnemyPlane(this);
            //this.enemyPlane.RegisterWeaponEvent += new RegisterWeapon(enemyPlane_RegisterWeaponEvent);
            enemyPlanes = new List<Plane>();
            enemiesLeft = levelParser.EnemyPlanes;
            currentTimeToNextEnemy = timeToFirstEnemy;

            //dodane przez Tomka
            storagePlanes = makeStoragePlanes();
        }


        /// <summary>
        /// Publiczny konstrutor dwuparametrowy.
        /// </summary>
        /// <param name="fileName">Nazwa pliku z 
        /// ktorego zostanie wczytana plansza.</param>
        /// <param name="controller">Obiekt przetwarzajacy zdarzenia.</param>
        public Level(String fileName, IController controller)
            : this(fileName)
        {
            this.controller = controller;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Zmienia stan silnika na w��czony.
        /// </summary>
        public void OnToggleEngineOn()
        {
            if (userPlane != null && !userPlane.IsBlockEngine)
                userPlane.SetInputFlag(InputFlag.EngineOn);
        }

        /// <summary>
        /// Informuje kontroler o tym, ze z lewej/prawej strony pojawil
        /// sie wrogi samolot
        /// </summary>
        /// <param name="left"></param>
        public void OnEnemyPlaneFromTheSide(Boolean left)
        {
            controller.OnEnemyPlaneFromTheSide(left);
        }

        public void OnPlaneForceChangeDirection(Plane plane)
        {
            if (!plane.IsEnemy)
            {
                controller.OnPlaneForceChangeDirection();
            }
        }

        public void OnPlaneForceGoDown(Plane plane)
        {
            if (!plane.IsEnemy)
            {
                controller.OnPlaneForceGoDown();
            }
        }


        /// <summary>
        /// Funkcja zmienia pozycje obiektow na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniego odswiezenia.</param>
        /// <author>Michal Ziober</author>
        public void Update(int time)
        {
            //kontrola wrogich samolot�w
            currentTimeToNextEnemy -= time;

            currentTimeToNextEnemy = Math.Max(0, currentTimeToNextEnemy);
            if (currentTimeToNextEnemy == 0)
            {
                if (enemiesLeft > 0 && enemyPlanes.Count < GameConsts.EnemyPlane.MaxSimultaneousEnemyPlanes)
                    //dodanie nowego samolotu
                {
                    EnemyPlane enemyPlane = new EnemyPlane(this);
                    enemyPlane.RegisterWeaponEvent += enemyPlane_RegisterWeaponEvent;
                    enemyPlanes.Add(enemyPlane);
                    Controller.OnRegisterPlane(enemyPlane);
                    currentTimeToNextEnemy = timeToNextEnemy; //odliczanie od pocz�tku
                }
                else
                {
                    currentTimeToNextEnemy = timeToNextEnemy;
                }
            }
            float scaleFactor = time / timeUnit;
            //sprawdzanie kolizji samolotu
            CheckPlaneCollisionWithGround(userPlane, scaleFactor);
            if (enemyPlanes.Count > 0)
            {
                Plane ep;
                for (int i = 0; i < enemyPlanes.Count; i++)
                {
                    ep = enemyPlanes[i];
                   
                    CheckPlaneCollisionWithGround(ep, scaleFactor);

                    // kolizje z samolotem gracza
                    if (userPlane.Bounds.Intersects(ep.Bounds))
                    {
                        // zniszcz samolot gracza przy kolizji tylko w trybie innym ni� 'easy'
                        if(EngineConfig.Difficulty.Equals(EngineConfig.DifficultyLevel.Easy))
                        {
                            // zabierz troch� �ycia przy zderzeniu - ten event b�dzie powtrzany kilka razy (w czasie gdy samolot b�d� si� dotyka�)
                            userPlane.Hit(userPlane.MaxOil * 0.08f, 0);
                        }
                        else
                        {
                            userPlane.Destroy(); 
                        }
                        ep.Destroy();
                    }

                    // kolizje z innymi samolotami 
                    if (i == 0 && enemyPlanes.Count > 1)
                    {
                        // wybieramy samolot wroga o ID=0 i sprawdzamy z wszystkimi pozosta�ymi samolotami wroga
                        for (int j = 1; j < enemyPlanes.Count; j++)
                        {
                            if (ep.Bounds.Intersects(enemyPlanes[j].Bounds))
                            {
                                enemyPlanes[j].Destroy();
                                ep.Destroy();
                            }
                        }
                    }
                }
            }

            //zmiana pozycji samolotu
            userPlane.Move(time, timeUnit);

            if (enemyPlanes.Count > 0)
            {
                Plane ep;
                for (int i = 0; i < enemyPlanes.Count; i++)
                {
                    ep = enemyPlanes[i];
                    ep.Move(time, timeUnit);
                    if ((ep.Oil <= 0 || ep.Petrol <= 0) && isEventAboutDestroySend)
                    {
                        controller.OnPlaneDestroyed(ep);
                        isEventAboutDestroySend = false;
                    }
                    
                }
            }

            //zmienia pozycje zolnierzy.
            SoldiersMove(time);

            //zmienia pozycje amunicji na planszy
            AmmunitionUpdate(time);

            //jesli samolot bedzie w zasiegu ktoregokolwiek z bunkrow
            //zostanie oddany strzal.
            if (bunkersList.Count > 0)
            {
                BunkerTile bunker;
                foreach (LevelTile tile in bunkersList)
                {
                    bunker = tile as BunkerTile;
                    if (bunker != null)
                        bunker.Fire(time);
                }
            }
        }

        /// <summary>
        /// Metoda wywo�ywana po naci�ni�ciu strza�ki w lewo.
        /// </summary>
        public void OnSteerLeft()
        {
            if (userPlane != null && !userPlane.IsBlockLeft)
                userPlane.SetInputFlag(InputFlag.Left);
        }

        /// <summary>
        /// Metoda wywo�ywana po naci�ni�ciu strza�ki w prawo.
        /// </summary>
        public void OnSteerRight()
        {
            if (userPlane != null && !userPlane.IsBlockRight)
                userPlane.SetInputFlag(InputFlag.Right);
        }

        /// <summary>
        /// Metoda wywo�ywana po naci�ni�ciu strza�ki w g�r�.
        /// </summary>
        public void OnSteerUp()
        {
            if (userPlane != null && !userPlane.IsBlockUp)
                userPlane.SetInputFlag(InputFlag.Up);
        }

        /// <summary>
        /// Metoda wywo�ywana po naci�ni�ciu przycisku odpowiedzialnego za obr�t samolotu ('z plec�w na brzuch').
        /// </summary>
        public void OnSpinPressed()
        {
            if (userPlane != null && !userPlane.IsBlockSpin)
                userPlane.SetInputFlag(InputFlag.Spin);
        }

        /// <summary>
        /// Metoda wywo�ywana po naci�ni�ciu strza�ki w d�.
        /// </summary>
        public void OnSteerDown()
        {
            if (userPlane != null && !userPlane.IsBlockDown)
                userPlane.SetInputFlag(InputFlag.Down);
        }

        /// <summary>
        /// Funkcja zostanie wywolana po nacisnieciu przycisku odpowiadajacego
        /// za otwarcie ognia cierzka amunicja.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void OnFireRocket()
        {
            if (userPlane.LocationState == LocationState.AircraftCarrier)
            {
                if (userPlane.CanChangeAmmunition)
                    controller.OnChangeAmmunition();
            }

            switch (userPlane.Weapon.SelectWeapon)
            {
                    //jesli uzytkownik wybral bomby jako ciezka amunicje.
                case WeaponType.Bomb:
                    if (userPlane.CanFireBomb)
                    {
                        if ((Environment.TickCount - lastFireTick) >= GameConsts.Bomb.FireInterval)
                        {
                            if (userPlane.Weapon.IsBombAvailable)
                                userPlane.Weapon.Fire(userPlane.RelativeAngle, WeaponType.Bomb);
                            lastFireTick = Environment.TickCount;
                        }
                    }
                    break;
                    //jesli uytkownik wybral rakiety jako ciezka amunicje.
                case WeaponType.Rocket:
                    if (userPlane.CanFireRocket)
                    {
                        if ((Environment.TickCount - lastFireTick) >= GameConsts.Rocket.FireInterval)
                        {
                            if (userPlane.Weapon.IsRocketAvailable)
                                userPlane.Weapon.Fire(userPlane.RelativeAngle, WeaponType.Rocket);
                            lastFireTick = Environment.TickCount;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Funkcja zostanie wywolana po nacisnieciu przycisku odpowiadajacego
        /// za otwarcie ognia z dzialka.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void OnFireGun()
        {
            if (userPlane.LocationState == LocationState.Air &&
                userPlane.PlaneState != PlaneState.Destroyed && userPlane.PlaneState != PlaneState.Crashed)
            {
                userPlane.Weapon.Fire(userPlane.RelativeAngle, WeaponType.Gun);
            }
        }

        /// <summary>
        /// Funkcja zostanie wywolana jesli widkok zakonczyl procedure
        /// usmiercania jednego zolnierza.
        /// </summary>
        public void OnSoldierEndDeath()
        {
            //sprawdzam stan wrogich instalacji.
            if (enemyInstallationTiles != null)
            {
                EnemyInstallationTile enemyTile;
                foreach (LevelTile levelTile in enemyInstallationTiles)
                {
                    enemyTile = levelTile as EnemyInstallationTile;
                    if (enemyTile != null)
                    {
                        if (!enemyTile.IsDestroyed && enemyTile.SoldierCount > 0)
                        {
                            return;
                        }
                    }
                }
            }

            //sprawdzam zolnierzy.
            if (soldierList != null)
            {
                foreach (Soldier s in soldierList)
                {
                    if (s.IsAlive)
                    {
                        return;
                    }
                }
            }
            //zolnierze nie zyja. konczymy poziom
            controller.OnReadyLevelEnd();
        }

        /// <summary>
        /// Metoda jest wywolywana przez kontroler w momencie, kiedy 
        /// uzytkownik wcisnie przycisk zmiany stanu podwozia. Jezeli
        /// samolot aktualnie nie chowa ani nie otwiera podwozia, stan
        /// podwozia jest zmieniany na TogglingIn (chowanie), 
        /// TogglingOut (otwieranie).
        /// </summary>
        /// <author>Jakub Tezycki</author>
        public void OnToggleGear()
        {
            if (userPlane != null)
            {
                if (userPlane.CanPlaneToggleGear())
                {
                    userPlane.ToggleGear();
                    controller.OnToggleGear(userPlane);
                }
            }
        }

        /// <summary>
        /// Metoda jest wywolywana przez kontroler w momencie zakonczenia
        /// animacji chowania/otwierania podwozia. Zmienia stan podwozia samolotu
        /// na oczekiwany
        /// </summary>
        /// <param name="plane"></param>
        public void OnGearToggled(Plane plane)
        {
            plane.GearToggled();
        }

        /// <summary>
        /// Metoda naprawia samolot, dope�nia oleju i b�zyny,
        /// wymienia bro� w zalezno�ci od parametru.
        /// </summary>
        /// <param name="weapon">bro� kt�ra bedzie wymieniana (rakiety lub bomby)</param>
        public void OnRestoreAmmunition(WeaponType weapon)
        {
            UserPlane.RepairPlane();
            UserPlane.OilRefuel();
            UserPlane.PetrolRefuel();
            switch (weapon)
            {
                case WeaponType.Bomb:
                    UserPlane.Weapon.RestoreBomb();
                    break;
                case WeaponType.Rocket:
                    UserPlane.Weapon.RestoreRocket();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Wywo�ywana gdy lina zostaje puszczona.
        /// </summary>
        /// <param name="carrierTile"></param>
        public void OnReleaseLine(EndAircraftCarrierTile carrierTile)
        {
            UserPlane.ReleaseLine(carrierTile);
        }

        /// <summary>
        /// Bierze nastepne �ycie. 
        /// Ustawia samolot na parametry startowe.
        /// </summary>
        /// <author
        public void NextLife()
        {
            if (lives - 1 > 0)
            {
                UserPlane.Init();
                UserPlane.Weapon.RestoreSelectedWeapon();
                storagePlanes.Remove(StoragePlanes[storagePlanes.Count - 1]);
                lives--;
            }
        }

        /// <summary>
        /// Zmniejsza liczbe zyc.
        /// </summary>
        public void SubtractionLive()
        {
            if (lives > 0)
                lives--;
        }

        /// <summary>
        /// Usuwa enemy plane. I zmniejsza ilo�� pozosta�ych na planszy wrog�w.
        /// </summary>
        public void ClearEnemyPlane(Plane ep)
        {
            enemyPlanes.Remove(ep);
            enemiesLeft -= 1;
        }

        /// <summary>
        /// Zabija zolnierzy, ktorzy sa w polu razenia.
        /// </summary>
        /// <param name="index">Index pola ktore zostalo trafione.</param>
        /// <param name="step">Zasieg razenia.</param>
        /// <author>Michal Ziober</author>
        /// <returns>Zwraca liczbe zabitych zolnierzy.</returns>
        public int KillVulnerableSoldiers(int index, int step)
        {
            List<Soldier> soldiers =
                SoldiersList.FindAll(Predicates.FindSoldierFromInterval(index - step, index + step));
            if (soldiers != null && soldiers.Count > 0)
                foreach (Soldier s in soldiers)
                {
                    if (s.CanDie)
                    {
                        //zmniejszam liczbe zolnierzy na planszy
                        this.SoldiersCount--;
                        Controller.OnSoldierBeginDeath(s, false);
                        s.Kill();
                    }
                }
            return soldiers.Count;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Rejestruje nowy pocisk na planszy.
        /// </summary>
        /// <param name="ammunition">Pocisk do zarejestrowania.</param>
        /// <author>Michal Ziober.</author>
        private void userPlane_RegisterWeaponEvent(Ammunition ammunition)
        {
            if(ammunition!=null) ammunitionList.Add(ammunition);
         
        }

        /// <summary>
        /// Rejestruje nowy pocisk na planszy.
        /// </summary>
        /// <param name="ammunition">Pocisk do zarejestrowania.</param>
        /// <author>Emil</author>
        private void enemyPlane_RegisterWeaponEvent(Ammunition ammunition)
        {
           if(ammunition!=null) ammunitionList.Add(ammunition);
        }

        /// <summary>
        /// Zmienia pozycje dla kazdego typu broni zarejestrowanego na 
        /// planszy.
        /// </summary>
        /// <param name="time"></param>
        /// <author>Michal Ziober.</author>
        private void AmmunitionUpdate(int time)
        {
            //jesli jest jakis pocisk na planszy
            if (ammunitionList.Count > 0)
            {
                //zmieniam pozycje kazdego pocisku.
                foreach (Ammunition ammun in ammunitionList)
                    ammun.Move(time);
                //usuwam zniszczone pociski.
                ammunitionList.RemoveAll(Predicates.RemoveAllDestroyedMissile());
            }
        }

        /// <summary>
        /// Porusza zolnierzami na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniego poruszenia.</param>
        /// <author>Michal Ziober</author>
        private void SoldiersMove(int time)
        {
            if (soldierList.Count > 0)
            {
                //usuwam martwych zolnierzy.
                soldierList.RemoveAll(Predicates.RemoveAllDeathSoldier());

                if (soldierList.Count > 0)
                {
                    //przesuwam zolnierzy znajdujacych sie na planszy
                    foreach (Soldier soldier in soldierList)
                        soldier.Move(time);
                }
            }
        }

        /// <summary>
        /// Pobiera czesci lotniskowca z listy wszystkich obiektow.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void SetAircraftCarrierList()
        {
            List<LevelTile> tmpList = LevelTiles.FindAll(Predicates.GetAllAircraftTiles());
            if (tmpList != null && tmpList.Count > 0)
            {
                AircraftCarrierTile airTile = null;
                foreach (LevelTile tile in tmpList)
                {
                    airTile = tile as AircraftCarrierTile;
                    aircraftTiles.Add(airTile);
                }
            }
        }

        /// <summary>
        /// Wczytuje plik xml podany w parametrze.
        /// </summary>
        /// <param name="fileName">Nazwa pliku do wczytania.</param>
        /// <remarks>Zaleca si� przed kontynuowaniem dalszych operacji sprawdzi�, czy aby napewno
        /// plik zostal wczytany poprawnie.</remarks>
        /// <author>Michal Ziober</author>
        private void ReadEncodedXmlFile(String fileName)
        {
            levelParser = new XmlLevelParser(fileName);
        }

        /// <summary>
        /// Ustawia dodatkowe wlasciwosc dla instalacji obronnych.
        /// </summary>
        /// <author>Michal Ziober</author>
        private void SetAttributesForInstallations()
        {
            EnemyInstallationTile enemyTile;
            if (LevelTiles != null)
            {
                int count = LevelTiles.Count;
                for (int i = 0; i < count; i++)
                {
                    enemyTile = LevelTiles[i] as EnemyInstallationTile;
                    if (enemyTile != null)
                    {
                        enemyTile.RegistrySoldierEvent += RegistrySoldierEvent;
                        enemyTile.LevelProperties = this;
                    }
                }
            }
        }

        /// <summary>
        /// Dodaje zolnierza do listy wszystkich zolnierzy.
        /// </summary>
        /// <param name="sender">Obiekt wysylajacy komunikat.</param>
        /// <param name="soldier">Zolnierz</param>
        /// <author>Michal Ziober</author>
        private void RegistrySoldierEvent(object sender, Soldier soldier)
        {
            if (soldier != null)
            {
                soldierList.Add(soldier);
            }
        }

        /// <summary>
        /// Sprawdza kolizje samolotu z tile'ami i obiektami na tile'ach
        /// </summary>
        /// <param name="plane">Samolot kt�ry ma by� sprawdzony</param>
        private void CheckPlaneCollisionWithGround(Plane plane ,float scaleFactor)
        {
            if (plane.PlaneState == PlaneState.Crashed)
                return;
            int index = Mathematics.PositionToIndex(plane.Center.X);

            //sprawdzam tile nad kt�rym jest samolot i 2 s�siednie
            for (int i = index - 1; i <= index + 1; i++)

                if (i >= 0 && i < LevelTiles.Count)
                {
                    //kolizje z obiektami na tile'u
                    if (plane.PlaneState != PlaneState.Destroyed &&
                        LevelTiles[i].ColisionRectangles != null)
                        for (int j = 0; j < LevelTiles[i].ColisionRectangles.Count; j++)
                        {
                            //Console.WriteLine("Plane at :" + plane.Center);
                            //Console.WriteLine("Checking collision with:" + LevelTiles[i].ColisionRectangles[j]);
                            if (LevelTiles[i].ColisionRectangles[j].Intersects(plane.Bounds))
                                plane.Destroy();
                        }
                    //***kolizja z terenem***
                    //je�li jest na lotniskowcu nie sprawdzam kolizji z lotniskowcem
                    if (plane.IsOnAircraftCarrier && LevelTiles[i].TileKind == TileKind.AircraftCarrier)
                        continue;

                    if (LevelTiles[i].HitBound != null &&
                        (LevelTiles[i].HitBound.Intersects(plane.Bounds) ||
                         plane.Bounds.LowestY < OceanTile.depth))
                    {
                        float terrainHeight;
                        if (LevelTiles[i].IsAircraftCarrier)
                        {
                            int pointsAbove = 0;
                            for (int j = 0; j < plane.Bounds.Peaks.Count; j++)
                            {
                                PointD p = plane.Bounds.Peaks[j];
                                if (p.Y >= Math.Max(LevelTiles[i].YEnd, LevelTiles[i].YBegin))
                                {
                                    pointsAbove++;
                                }
                            }
                            if (pointsAbove >= 3) terrainHeight = Math.Max(LevelTiles[i].YEnd, LevelTiles[i].YBegin);
                            else terrainHeight = 0;
                        }
                        else
                        {
                            terrainHeight = Math.Min(LevelTiles[i].YEnd, LevelTiles[i].YBegin);
                        }

                        plane.Crash(terrainHeight, LevelTiles[i].TileKind);
                    }
                }
                //Przypadek szczeg�lny gdy rozbijamy sie poza obszarem gdzie s� LevelTiles[kra�ce mapy].
                //Na przyk�ad w wyniku wy��czenia silnika i pe�nej pr�dko�ci przy You are not leaving yet.
                else if ((i < 0 || i >= LevelTiles.Count) && plane.Bounds.LowestY < OceanTile.depth)
                {
                    //0.1f terrainHeight dla Ocean
                    plane.Crash(0.1f, TileKind.Ocean);
                }
                
        }

        /// <summary>
        /// Metoda tworzy dodatkowe samoloty, na podstawie ilo�ci �y�.
        /// Umieszcza ka�dy samolot na oddzielnym tile pocz�wszy od pierwszego.
        /// </summary>
        /// <returns></returns>
        private List<StoragePlane> makeStoragePlanes()
        {
            List<StoragePlane> temp = new List<StoragePlane>();
            for (int i = 0; i < lives - 1; i++)
                temp.Add(new StoragePlane(this, Carrier.CarrierTiles[i]));
            return temp;
        }

        /// <summary>
        /// Ustawia informacje o wystepowaniu 
        /// instalacji obronnych na planszy.
        /// </summary>
        private void SetFlyDirectionHint()
        {
            if (enemyInstallationTiles != null && enemyInstallationTiles.Count > 0 &&
                aircraftTiles != null && aircraftTiles.Count > 0)
            {
                int aircroftIndex = aircraftTiles[0].TileIndex;
                bool left = false, right = false;
                foreach (LevelTile enemyInstallation in enemyInstallationTiles)
                {
                    if (enemyInstallation.TileIndex < aircroftIndex)
                    {
                        left = true;
                        break;
                    }
                }
                foreach (LevelTile enemyInstallation in enemyInstallationTiles)
                {
                    if (enemyInstallation.TileIndex > aircroftIndex)
                    {
                        right = true;
                        break;
                    }
                }

                if (left && right)
                    flyDirectionHint = FlyDirectionHint.Both;
                else if (left)
                    flyDirectionHint = FlyDirectionHint.Left;
                else if (right)
                    flyDirectionHint = FlyDirectionHint.Right;
                else
                    flyDirectionHint = FlyDirectionHint.None;
            }
            else flyDirectionHint = FlyDirectionHint.None;
        }

        /// <summary>
        /// Pobiera liczbe zolnierzy z instalacji obronnych.
        /// </summary>
        /// <param name="list">Lista elementow</param>
        private void GetSoldiersCount(List<LevelTile> list)
        {
            int soldierCount = 0;
            EnemyInstallationTile enemy = null;
            foreach (LevelTile tile in list)
                if (tile is EnemyInstallationTile)
                {
                    enemy = tile as EnemyInstallationTile;
                    soldierCount += enemy.SoldierCount;
                }
            this.mSoldierCount = soldierCount;
        }

        #endregion

        #region properties

        /// <summary>
        /// Informacja o polozeniu wysp z bunkrami wroga,
        /// wzgledem lotniskowca.
        /// </summary>
        public FlyDirectionHint FlyDirectionHint
        {
            get { return flyDirectionHint; }
        }

        /// <summary>
        ///Zwraca samolot obiekt samolotu gracza.
        /// </summary>
        /// <author>Michal Ziober</author>
        public Plane UserPlane
        {
            get { return userPlane; }
        }

        /// <summary>
        /// Zwraca obiekt samolotu.
        /// </summary>
        public Carrier Carrier
        {
            get { return carrier; }
        }

        /// <summary>
        /// Zwraca liste zolnierzy na planszy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<Soldier> SoldiersList
        {
            get { return soldierList; }
        }

        /// <summary>
        /// Zwraca liste samolotow przeciwnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<Plane> EnemyPlanes
        {
            get { return enemyPlanes; }
        }

        /// <summary>
        /// Zwraca liste bomb i rakiet zarejestrowanych na planszy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<Ammunition> AmmunitionList
        {
            get { return ammunitionList; }
        }

        /// <summary>
        /// Zwraca liste bunkrow na planszy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<LevelTile> BunkersList
        {
            get { return bunkersList; }
        }

        /// <summary>
        /// Zwraca liste czesci lotniskowca.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<AircraftCarrierTile> AircraftTile
        {
            get { return aircraftTiles; }
        }

        /// <summary>
        /// Zwraca obiekt przetwarzajacy.
        /// </summary>
        /// <author>Michal Ziober</author>
        public IController Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Pobiera statystyki poziomu.
        /// </summary>
        public LevelStatistics Statistics
        {
            get { return this.mStatistics; }
        }

        /// <summary>
        /// Zwraca liczbe zolnierzy, ktorzy znajduja sie obecnie na planszy.
        /// </summary>
        public int SoldiersCount
        {
            set { this.mSoldierCount = Math.Max(value, 0); }
            get { return this.mSoldierCount; }
        }

        #region Levels settings

        /// <summary>
        /// Pora dnia.
        /// </summary>
        /// <author>Michal Ziober</author>
        public DayTime DayTime
        {
            get { return levelParser.DayTime; }
        }

        /// <summary>
        /// Zwraca liste wczytanych obiektow.
        /// </summary>
        /// <author>Michal Ziober</author>
        public List<LevelTile> LevelTiles
        {
            get { return levelParser.Tiles; }
        }

        /// <summary>
        /// Zwraca typ taila na podstawie pozycji.
        /// </summary>
        /// <param name="positionX">Wspolrzedna X.</param>
        /// <returns>Typ ziemi na ktory spadl samolot.</returns>
        public LevelTile GetTileForPosition(float positionX)
        {
            int position = Mathematics.PositionToIndex(positionX);
            if (position > -1 && position < LevelTiles.Count)
                return LevelTiles[position];
            return null;
        }

        /// <summary>
        /// Zwraca liczbe zyc.
        /// </summary>
        /// <author>Michal Ziober</author>
        public int Lives
        {
            get { return lives; }
        }

        #endregion

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Zwolnienie zajetych zasobow.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void Dispose()
        {
            levelParser.Dispose();
            if (ammunitionList != null)
            {
                ammunitionList.Clear();
                ammunitionList = null;
            }
            if (soldierList != null)
            {
                soldierList.Clear();
                soldierList = null;
            }
            if (bunkersList != null)
            {
                bunkersList.Clear();
                bunkersList = null;
            }
            if (aircraftTiles != null)
            {
                aircraftTiles.Clear();
                aircraftTiles = null;
            }
            if (enemyInstallationTiles != null)
            {
                enemyInstallationTiles.Clear();
                enemyInstallationTiles = null;
            }
            if (storagePlanes != null)
            {
                storagePlanes.Clear();
                storagePlanes = null;
            }
        }

        #endregion
    }
}