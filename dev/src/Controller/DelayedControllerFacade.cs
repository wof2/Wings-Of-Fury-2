using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Wof.Model.Level;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.Controller.Screens;

namespace Wof.Controller
{
    public class DelayedControllerFacade : IController
    {

        private Dictionary<int, KeyValuePair<String, object[]>> jobs;

        private int lastId = 0;

        private IController controller;

        public DelayedControllerFacade(IController controller)
        {
            jobs = new Dictionary<int, KeyValuePair<string, object[]>>();
            this.controller = controller;
        }

        public void ClearJobs()
        {
            lock (this)
            {
                jobs.Clear();
                lastId = 0;
            }
        }

        private void AddJob(String methodName)
        {
            lock (this)
            {
                jobs.Add(lastId++, new KeyValuePair<String, object[]>(methodName, null));
            }
        }

        private void AddJob(String methodName, object[] arguments)
        {
            lock (this)
            {
                jobs.Add(lastId++, new KeyValuePair<String, object[]>(methodName, arguments));
            }
        }

        public void DoJobs()
        {
            lock (this)
            {
                try
                {
                    foreach (KeyValuePair<int, KeyValuePair<String, object[]>> job in jobs)
                    {
                        KeyValuePair<String, object[]> jobInfo = job.Value;
                        controller.GetType().GetMethod(jobInfo.Key).Invoke(controller, jobInfo.Value);
                    }
                  
                }
                catch (Exception ex)
                {

                    ex.ToString();
                   
                }
                finally
                {
                    // czyscimy nawet w wypadku wyjatku dla zapewnienia ciaglosci przetwarzania
                    jobs.Clear();
                    lastId = 0;
                }
                
            }
        }

        #region Implementation of IController

        /// <summary>
        /// Funkcja rejestruje samolot. Samolot moze byc 
        /// samolotem gracza lub nieprzyjaciela
        /// </summary>
        /// <param name="plane"></param>
        public void OnRegisterPlane(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[]{plane});
        }

        /// <summary>
        /// Funkcja rejestruje strzal do samolotu.
        /// </summary>
        /// <param name="bunker">Bunkier ktory strzelil.</param>
        /// <param name="plane">Samolot gracza.</param>
        /// <author>Michal Ziober</author>
        public void OnBunkerFire(BunkerTile bunker, Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { bunker, plane });
        }

        /// <summary>
        /// Funkcja rejestruje na planszy nowego zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz do zarejestrowania.</param>
        /// <author>Michal Ziober</author>
        public void OnRegisterSoldier(Soldier soldier, MissionType missionType)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier, missionType });
        }

        /// <summary>
        /// Funkcja usuwa zolnierza z widoku.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostanie odrejestrowany.</param>
        /// <author>Michal Ziober</author>
        public void UnregisterSoldier(Soldier soldier)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier });
        }

        /// <summary>
        /// Funkcja rozpoczyna usmiercanie zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal trafiony.</param>
        /// <param name="gun">Typ broni przez ktora zginal zolnierz.
        /// Jesli gun jest ustawione na true, zolnierz zginal przez dzialko.
        /// Jesli gun jest ustawione na false, zolnierz zginal przez rakiete(bombe).
        /// <param name="scream">czy ma być dźwięk</param>
        /// </param>
        /// <author>Michal Ziober</author>
        public void OnSoldierBeginDeath(Soldier soldier, bool gun, bool scream)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier, gun, scream });
        }

        public void OnSoldierPrepareToFire(Soldier soldier, float maxTime)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier, maxTime });
        }

        public void OnSoldierEndPrepareToFire(Soldier soldier)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier });
        }

        /// <summary>
        /// Funkcja zglasza o zatrzymaniu pracy silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void OnTurnOffEngine(Plane p)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { p });
        }

        /// <summary>
        /// Funkcja zglasza o rozpoczeciu pracy silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void OnTurnOnEngine(bool engineStartSound)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { engineStartSound });
        }

        public void OnEngineFaulty(Plane p)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { p });
        }

        public void OnEngineRepaired(Plane p)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { p });
        }

        /// <summary>
        /// Funkcja zglasz, ze proba uruchomienia silnika.
        /// samolotu sie niepowiodla.
        /// </summary>
        /// <author>Michal Ziober</author>
        public void OnStartEngineFailed()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Funkcja zglasza o trafieniu wrogiego samolotu bomba lub rakieta.
        /// </summary>
        /// <param name="plane">Samolot przeciwnika, ktory zostal trafiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w samolot.</param>
        /// <author>Michal Ziober</author>
        public void OnEnemyPlaneBombed(Plane plane, Ammunition ammunition)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane, ammunition });
        }

        /// <summary>
        /// Funkcja jest wywolywana jesli uderzony zostal tylko teren, lub struktury
        /// nie da sie zniszczyc tego typu amunicja.
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trfiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w teren.</param>
        /// <author>Michal Ziober</author>
        public void OnTileBombed(LevelTile tile, Ammunition ammunition)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, ammunition });
        }

        /// <summary>
        /// Wywołane kiedy torpeda została uszkodzona i "zatonęła"
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="ammunition"></param>
        /// <param name="torpedoFailure"></param>
        public void OnTorpedoSunk(LevelTile tile, Torpedo ammunition, TorpedoFailure? torpedoFailure)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, ammunition, torpedoFailure });
        }

        /// <summary>
        /// Funkcja jest wywolywana jesli zostal trafiony wrogi obiekt,
        /// ktory mozna zniszczyc tego typu bronia.
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trafiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w obiekt.
        /// Jesli trafienie bylo dzialkiem, i obiekt powinien byc zniszczony nalezy podac null.</param>
        /// <author>Michal Ziober</author>
        public void OnTileDestroyed(LevelTile tile, Ammunition ammunition)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, ammunition });
        }

        /// <summary>
        /// Funkcja jest wywolywana jesli torpeda wpadnie do wody
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trafiony.</param>
        /// <param name="torpedo">Torpeda ktora trafila w obiekt.</param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// 
        /// <author>Adam Witczak</author>
        public void OnTorpedoHitGroundOrWater(LevelTile tile, Torpedo torpedo, float posX, float posY)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, torpedo, posX, posY });
        }

        /// <summary>
        /// Wywoływane przez Model w momencie podjęcia decyzji o obrocie samolotu
        /// </summary>
        /// <param name="newDirection">Przyszły kierunek lotu</param>
        /// <param name="plane">Samolot ktory ma zawrocic</param>
        /// <param name="turnType">Wariant zawracania</param>
        /// <author>Adam Witczak</author>       
        public void OnPrepareChangeDirection(Direction newDirection, Plane plane, TurnType turnType)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { newDirection, plane, turnType });
        }

        /// <summary>
        /// Funkcja jest wywolywana jesli rozpoczęła się animacja obrotu samolotu w view
        /// </summary>
        /// <author>Adam Witczak</author>
        /// <param name="turnInfo">Przez ten parametr View przekazuje całkowity czas animacji obrotu liczony w SEKUNDACH oraz plane. 
        /// Przekazywany obiekt jest typu TurnInfo</param>      
        public void OnPrepareChangeDirectionEnd(object turnInfo)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { turnInfo });
        }

        /// <summary>
        /// Wywoływane przez View w momencie zakończenia animacji obrotu
        /// </summary>
        /// <author>Adam Witczak</author>       
        public void OnChangeDirectionEnd(object plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Wywoływane przez Model w momencie podjęcia decyzji odwróceniu samolotu (z 'pleców' na 'brzuch')
        /// </summary>    
        /// <param name="plane">Samolot, ktory ma zostać obrócony</param>     
        /// <author>Adam Witczak</author>    
        public void OnSpinBegin(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Wywoływane przez View aby powiadomić model, że animacja jest zakończona
        /// </summary>    
        /// <param name="plane">Samolot, który ma został obrócony</param>     
        /// <author>Adam Witczak</author>    
        public void OnSpinEnd(object plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Funkcja informuje o zrzuceniu nowej bomby przez samolot.
        /// </summary>
        /// <param name="bomb">Bomba, ktora zostala zrzucona.</param>
        /// <author>Michal Ziober</author>
        public void OnRegisterBomb(Bomb bomb)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { bomb });
        }

        /// <summary>
        /// Wywolywane przez Model w momencie poczatku procedury zmiany 
        /// stanu podwozia.
        /// </summary>
        /// <param name="plane"></param>
        /// <author>Jakub Tezycki</author>
        public void OnToggleGear(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Funkcja informuje o wystrzeleniu rakiety przez samolot.
        /// </summary>
        /// <param name="rocket">Rakieta, ktora zostala wystrzelona.</param>
        /// <author>Michal Ziober</author>
        public void OnRegisterRocket(Rocket rocket)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { rocket });
        }

        /// <summary>
        /// Funkcja informuje o wystrzeleniu torpedy przez samolot.
        /// </summary>
        /// <param name="torpedo">Torpeda, ktora zostala wystrzelona.</param>
        /// <author>Adam Witczak</author>
        public void OnRegisterTorpedo(Torpedo torpedo)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { torpedo });
        }

        /// <summary>
        /// Metoda jest wywolywana przez View, w momencie zakonczenia
        /// animacji zmiany stanu podwozia.
        /// </summary>
        /// <param name="plane"></param>
        public void OnGearToggleEnd(object plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Funkcja informuje o wystrzeleniu pocisku z dzialka.
        /// </summary>
        /// <param name="plane">Samolot, ktory otworzyl ogien.</param>
        /// <author>Michal Ziober.</author>
        public void OnFireGun(IObject2D plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Jesli dzialko trafi w jakas czesc planszy.
        /// </summary>
        /// <param name="tile">Czesc planszy, ktora zostala trafiona przez dzialko. </param>
        /// <param name="posX">Pozycja x.</param>
        /// <param name="posY">Pozycja y.</param>
        /// <author>Michal Ziober.</author>
        public void OnGunHit(LevelTile tile, float posX, float posY)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, posX, posY });
        }

        /// <summary>
        /// Funkcja zostanie wywolana jesliz zostal zabity jakis zolnierz.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal zabity.</param>
        /// <author>Michal Ziober.</author>
        public void OnKillSoldier(Soldier soldier)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { soldier });
        }

        /// <summary>
        /// Komunikat o zakonczeniu danego poziomu.
        /// </summary>
        /// <author>Michal Ziober.</author>
        public void OnReadyLevelEnd()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Funkcja zostaje wywołana gdy jest wymieniana amunicja w samolocie.
        /// </summary>
        public void OnChangeAmmunition()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Komunikat o rozbiciu samolotu.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="tileKind">W jaki teren uderzył samolot.</param>
        public void OnPlaneCrashed(Plane plane, TileKind tileKind)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane, tileKind });
        }

        /// <summary>
        /// Komunikat o zniszczeniu samolotu.
        /// </summary>
        /// <param name="plane"></param>
        public void OnPlaneDestroyed(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal uszkodzony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w teren.</param>
        public void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, ammunition });
        }

        /// <summary>
        /// Krzyk pilota wroga
        /// </summary>
        /// <param name="plane"></param>
        public void OnWarCry(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Funkcja Zostaje wywołana gdy lina hamująca złapie samolot. 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="carrierTile"></param>
        public void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane, carrierTile });
        }

        /// <summary>
        /// Funkcja zostaje wywołana gdy lina hamująca zwalnia samolot.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="carrierTile"></param>
        public void OnReleasePlane(Plane plane, EndAircraftCarrierTile carrierTile)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane, carrierTile });
        }

        /// <summary>
        /// Funkcja zostanie wywolana gdy rakieta przekroczy dopuszczalny dystans,
        /// lub gdy wyleci poza plansze.
        /// </summary>
        /// <param name="rocket">Rakieta do odrejestrowania.</param>
        /// <author>Michal Ziober</author>
        public void OnUnregisterRocket(Rocket rocket)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { rocket });
        }

        /// <summary>
        /// Funkcja zostanie wywolana gdy torpeda przekroczy dopuszczalny dystans,
        /// lub gdy wyleci poza plansze.
        /// </summary>
        /// <param name="torpedo">Torpeda do odrejestrowania.</param>
        /// <author>Adam Witczak</author>
        public void OnUnregisterTorpedo(Torpedo torpedo)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { torpedo });
        }

        /// <summary>
        /// Funkcja zostaje wywolana gdy zolnierz odbudowuje bunkier.
        /// </summary>
        /// <param name="restoredBunker">Odbudowany bunkier.</param>
        public void OnTileRestored(BunkerTile restoredBunker)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { restoredBunker });
        }

        /// <summary>
        /// Funkcja zostaje wywołana gdy samolot wystartował.
        /// </summary>
        public void OnTakeOff()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Funkcja zostaje wywołana gdy samolot dotknąl podłoża lotniskowca.
        /// </summary>
        public void OnTouchDown()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Funkcja zostaje wywołana, gdy skończy się animacja tonięcia (spalenia) samolotu.
        /// 
        /// </summary>
        public void OnPlaneWrecked(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Funkcja zostaje wywołana, gdy samolot wroga mija samolot gracza.
        /// 
        /// </summary>
        public void OnPlanePass(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Ukrywa samolot (na razie)
        /// <author>Adam Witczak</author>
        /// </summary>
        /// <param name="plane"></param>
        public void OnUnregisterPlane(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Efekt trafien samolotu przez dzialko
        /// </summary>
        /// <param name="plane"></param>
        public void OnGunHitPlane(Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { plane });
        }

        /// <summary>
        /// Zdarzenie wywolywane przez model, informuje o pojawieniu
        /// sie wrogiego samolotu na planszy
        /// </summary>
        /// <param name="left">Cz z lewej?</param>
        public void OnEnemyPlaneFromTheSide(bool left)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { left });
        }

        /// <summary>
        /// Zdarzenie wywolywane przez model, kiedy samolot
        /// jest zmuszany do zawracania
        /// </summary>
        public void OnPlaneForceChangeDirection()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Zdarzenie wywolywane przez model, kiedy samolot
        /// jest zmuszany do obnizenia wysokosci
        /// </summary>
        public void OnPlaneForceGoDown()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Zdarzenie wywoływane gdy samolot wroga podejmie decyzje o
        /// zaatakowaniu samolotów na lotniskowcu.
        /// </summary>
        public void OnEnemyAttacksCarrier()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Zdarzenie wywoływane gdy samolot startuje ze złej strony.
        /// </summary>
        public void OnPlaneWrongDirectionStart()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Zaczyna pętlę dźwięku z bąbelkami wodnymi
        /// </summary>
        public void OnStartWaterBubblesSound()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Zatrzymuje dźwięk z bąbelkami wodnymi
        /// </summary>
        public void OnStopWaterBubblesSound()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Metoda zglasza kolejny poziom zniszczen statku.
        /// </summary>
        /// <param name="tile">Element statku. Reprezentuje zniszczenia dla calego statku.</param>
        /// <param name="state">Reprezentuja aktualny poziom zniszczen dla danego statku.</param>
        public void OnShipDamaged(ShipTile tile, ShipState state)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile, state });
        }

        /// <summary>
        /// Statek zaczyna tonąć
        /// </summary>
        /// <param name="tile"></param>
        public void OnShipBeginSinking(ShipTile tile)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile });
        }

        /// <summary>
        /// Statek tonie
        /// </summary>
        /// <param name="tile"></param>
        public void OnShipSinking(ShipTile tile)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile });
        }

        /// <summary>
        /// Statek jest pod powierzchnią wody
        /// </summary>
        /// <param name="tile"></param>
        //void OnShipUnderWater(ShipTile tile); 
        
        
        /// <summary>
        /// Statek zatonął
        /// </summary>
        /// <param name="tile"></param>
        public void OnShipSunk(BeginShipTile tile)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { tile });
        }
        
        public void OnPotentialLanding(Plane p)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { p });
        }

        public void OnSecondaryFireOnCarrier()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        public void OnRocketHitPlane(Rocket rocket, Plane plane)
        {
            AddJob(MethodBase.GetCurrentMethod().Name, new object[] { rocket, plane });
        }
        public void OnLevelFinished()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        public void OnPlayFanfare()
        {
            AddJob(MethodBase.GetCurrentMethod().Name);
        }

        #endregion
    }
}
