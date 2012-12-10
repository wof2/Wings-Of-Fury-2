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
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.View;

namespace Wof.Controller
{

    
    public interface IController
    {

        void OnRegisterDebugInfo(DebugInfo debugInfo);

        /// <summary>
        /// Funkcja rejestruje samolot. Samolot moze byc 
        /// samolotem gracza lub nieprzyjaciela
        /// </summary>
        /// <param name="plane"></param>
        void OnRegisterPlane(Plane plane);

        PlaneView FindPlaneView(Plane plane);
        

     
        /// <summary>
        /// Funkcja rejestruje strzal do samolotu.
        /// </summary>
        /// <param name="bunker">Bunkier ktory strzelil.</param>
        /// <param name="plane">Samolot gracza.</param>
        /// <author>Michal Ziober</author>
        void OnBunkerFire(BunkerTile bunker, Plane plane, bool planeHit);

        /// <summary>
        /// Funkcja rejestruje na planszy nowego zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz do zarejestrowania.</param>
        /// <author>Michal Ziober</author>
        void OnRegisterSoldier(Soldier soldier, MissionType missionType);

        /// <summary>
        /// Funkcja usuwa zolnierza z widoku.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostanie odrejestrowany.</param>
        /// <author>Michal Ziober</author>
        void UnregisterSoldier(Soldier soldier);


        /// <summary>
        /// Funkcja rozpoczyna usmiercanie zolnierza.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal trafiony.</param>
        /// <param name="gun">Typ broni przez ktora zginal zolnierz.
        /// Jesli gun jest ustawione na true, zolnierz zginal przez dzialko.
        /// Jesli gun jest ustawione na false, zolnierz zginal przez rakiete(bombe).
        /// <param name="scream">czy ma byæ dŸwiêk</param>
        /// </param>
        /// <author>Michal Ziober</author>
        void OnSoldierBeginDeath(Soldier soldier, bool gun, bool scream);


        /// <summary>
        /// Zglasza ze zolnierz zaczyna sie przygotowywac do strzalu
        /// </summary>
        /// <param name="soldier"></param>
        /// <param name="maxTime"></param>
        void OnSoldierPrepareToFire(Soldier soldier, float maxTime);

        /// <summary>
        /// Zglasza ze zolnierz wlasnie odpalil pocisk i moze wracac do poprzedniego stanu
        /// </summary>
        /// <param name="soldier"></param>
        void OnSoldierEndPrepareToFire(Soldier soldier);




        /// <summary>
        /// Funkcja zglasza o zatrzymaniu pracy silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        void OnTurnOffEngine(Plane p);

        /// <summary>
        /// Funkcja zglasza o rozpoczeciu pracy silnika.
        /// </summary>
        /// <author>Michal Ziober</author>
        void OnTurnOnEngine(bool engineStartSound, Plane userPlane);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        void OnEngineFaulty(Plane p);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        void OnEngineRepaired(Plane p);


        /// <summary>
        /// Funkcja zglasz, ze proba uruchomienia silnika.
        /// samolotu sie niepowiodla.
        /// </summary>
        /// <author>Michal Ziober</author>
        void OnStartEngineFailed();

        /// <summary>
        /// Funkcja zglasza o trafieniu wrogiego samolotu bomba lub rakieta.
        /// </summary>
        /// <param name="plane">Samolot przeciwnika, ktory zostal trafiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w samolot.</param>
        /// <author>Michal Ziober</author>
        void OnEnemyPlaneBombed(Plane plane, Ammunition ammunition);

        /// <summary>
        /// Funkcja jest wywolywana jesli uderzony zostal tylko teren, lub struktury
        /// nie da sie zniszczyc tego typu amunicja.
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trfiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w teren.</param>
        /// <author>Michal Ziober</author>
        void OnTileBombed(LevelTile tile, Ammunition ammunition);


        /// <summary>
        /// Wywo³ane kiedy torpeda zosta³a uszkodzona i "zatonê³a"
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="ammunition"></param>
        /// <param name="torpedoFailure"></param>
        void OnTorpedoSunk(LevelTile tile, Torpedo ammunition, TorpedoFailure? torpedoFailure);

        /// <summary>
        /// Funkcja jest wywolywana jesli zostal trafiony wrogi obiekt,
        /// ktory mozna zniszczyc tego typu bronia.
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trafiony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w obiekt.
        /// Jesli trafienie bylo dzialkiem, i obiekt powinien byc zniszczony nalezy podac null.</param>
        /// <author>Michal Ziober</author>
        void OnTileDestroyed(LevelTile tile, Ammunition ammunition);

        /// <summary>
        /// Funkcja jest wywolywana jesli torpeda wpadnie do wody
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal trafiony.</param>
        /// <param name="torpedo">Torpeda ktora trafila w obiekt.</param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// 
        /// <author>Adam Witczak</author>
        void OnTorpedoHitGroundOrWater(LevelTile tile, Torpedo torpedo, float posX, float posY);

        /// <summary>
        /// Wywo³ywane przez Model w momencie podjêcia decyzji o obrocie samolotu
        /// </summary>
        /// <param name="newDirection">Przysz³y kierunek lotu</param>
        /// <param name="plane">Samolot ktory ma zawrocic</param>
        /// <param name="turnType">Wariant zawracania</param>
        /// <author>Adam Witczak</author>       
        void OnPrepareChangeDirection(Direction newDirection, Plane plane, TurnType turnType);

        /// <summary>
        /// Funkcja jest wywolywana jesli rozpoczê³a siê animacja obrotu samolotu w view
        /// </summary>
        /// <author>Adam Witczak</author>
        /// <param name="turnInfo">Przez ten parametr View przekazuje ca³kowity czas animacji obrotu liczony w SEKUNDACH oraz plane. 
        /// Przekazywany obiekt jest typu TurnInfo</param>      
        void OnPrepareChangeDirectionEnd(object turnInfo);

        /// <summary>
        /// Wywo³ywane przez View w momencie zakoñczenia animacji obrotu
        /// </summary>
        /// <author>Adam Witczak</author>       
        void OnChangeDirectionEnd(object plane);

        ///////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Wywo³ywane przez Model w momencie podjêcia decyzji odwróceniu samolotu (z 'pleców' na 'brzuch')
        /// </summary>    
        /// <param name="plane">Samolot, ktory ma zostaæ obrócony</param>     
        /// <author>Adam Witczak</author>    
        void OnSpinBegin(Plane plane);

        /// <summary>
        /// Wywo³ywane przez View aby powiadomiæ model, ¿e animacja jest zakoñczona
        /// </summary>    
        /// <param name="plane">Samolot, który ma zosta³ obrócony</param>     
        /// <author>Adam Witczak</author>    
        void OnSpinEnd(object plane);

        /// <summary>
        /// Funkcja informuje o zrzuceniu nowej bomby przez samolot.
        /// </summary>
        /// <param name="bomb">Bomba, ktora zostala zrzucona.</param>
        /// <author>Michal Ziober</author>
        void OnRegisterBomb(Bomb bomb);

        /// <summary>
        /// Wywolywane przez Model w momencie poczatku procedury zmiany 
        /// stanu podwozia.
        /// </summary>
        /// <param name="plane"></param>
        /// <author>Jakub Tezycki</author>
        void OnToggleGear(Plane plane);

        /// <summary>
        /// Funkcja informuje o wystrzeleniu rakiety przez samolot.
        /// </summary>
        /// <param name="rocket">Rakieta, ktora zostala wystrzelona.</param>
        /// <author>Michal Ziober</author>
        void OnRegisterRocket(Rocket rocket);
        
        /// <summary>
        /// Funkcja informuje o wystrzeleniu pocisku typu flak 
        /// </summary>
        /// <param name="flakBullet">pocisk falk, ktory zostal wystrzelony.</param>
        /// <author>Adam Witczak</author>
        void OnRegisterFlakBullet(FlakBullet flakBullet);

   	    /// <summary>
        /// Funkcja informuje o wystrzeleniu pocisku typu dzialko 
        /// </summary>
        /// <param name="gunBullet">pocisk dzialka, ktory zostal wystrzelony.</param>
        /// <author>Adam Witczak</author>
        void OnRegisterGunBullet(GunBullet gunBullet);

        /// <summary>
        /// Funkcja informuje o wystrzeleniu pocisku typu pocisk bunkra 
        /// </summary>
        /// <param name="shellBullet"></param>
        void OnRegisterBunkerShellBullet(BunkerShellBullet shellBullet);

        
        /// <summary>
        /// Funkcja informuje o wystrzeleniu torpedy przez samolot.
        /// </summary>
        /// <param name="torpedo">Torpeda, ktora zostala wystrzelona.</param>
        /// <author>Adam Witczak</author>
        void OnRegisterTorpedo(Torpedo torpedo);

        

        /// <summary>
        /// Metoda jest wywolywana przez View, w momencie zakonczenia
        /// animacji zmiany stanu podwozia.
        /// </summary>
        /// <param name="plane"></param>
        void OnGearToggleEnd(object plane);


        /// <summary>
        /// Funkcja informuje o wystrzeleniu pocisku z dzialka.
        /// </summary>
        /// <param name="plane">Samolot, ktory otworzyl ogien.</param>
        /// <author>Michal Ziober.</author>
        void OnFireGun(IObject2D plane);

        /// <summary>
        /// Jesli dzialko trafi w jakas czesc planszy.
        /// </summary>
        /// <param name="tile">Czesc planszy, ktora zostala trafiona przez dzialko. </param>
        /// <param name="posX">Pozycja x.</param>
        /// <param name="posY">Pozycja y.</param>
        /// <author>Michal Ziober.</author>
        void OnGunHit(LevelTile tile, float posX, float posY);

        /// <summary>
        /// Funkcja zostanie wywolana jesliz zostal zabity jakis zolnierz.
        /// </summary>
        /// <param name="soldier">Zolnierz, ktory zostal zabity.</param>
        /// <author>Michal Ziober.</author>
        void OnKillSoldier(Soldier soldier);

        /// <summary>
        /// Komunikat o zakonczeniu danego poziomu.
        /// </summary>
        /// <author>Michal Ziober.</author>
        void OnReadyLevelEnd();

        /// <summary>
        /// Funkcja zostaje wywo³ana gdy jest wymieniana amunicja w samolocie.
        /// </summary>
        void OnChangeAmmunition();

        /// <summary>
        /// Komunikat o rozbiciu samolotu.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="tileKind">W jaki teren uderzy³ samolot.</param>
        void OnPlaneCrashed(Plane plane, TileKind tileKind);

        /// <summary>
        /// Komunikat o zniszczeniu samolotu.
        /// </summary>
        /// <param name="plane"></param>
        void OnPlaneDestroyed(Plane plane);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile">Obiekt, ktory zostal uszkodzony.</param>
        /// <param name="ammunition">Bomba(Rakieta), ktora trafila w teren.</param>
        void OnFortressHit(FortressBunkerTile tile, Ammunition ammunition);
       


        /// <summary>
        /// Krzyk pilota wroga
        /// </summary>
        /// <param name="plane"></param>
        void OnWarCry(Plane plane);

        /// <summary>
        /// Funkcja Zostaje wywo³ana gdy lina hamuj¹ca z³apie samolot. 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="carrierTile"></param>
        void OnCatchPlane(Plane plane, EndAircraftCarrierTile carrierTile);

        /// <summary>
        /// Funkcja zostaje wywo³ana gdy lina hamuj¹ca zwalnia samolot.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="carrierTile"></param>
        void OnReleasePlane(Plane plane, EndAircraftCarrierTile carrierTile);

        /// <summary>
        /// Funkcja zostanie wywolana gdy rakieta przekroczy dopuszczalny dystans,
        /// lub gdy wyleci poza plansze.
        /// </summary>
        /// <param name="rocket">Rakieta do odrejestrowania.</param>
        /// <author>Michal Ziober</author>
        void OnUnregisterRocket(Rocket rocket);

        /// <summary>
        /// Funkcja zostanie wywolana gdy torpeda przekroczy dopuszczalny dystans,
        /// lub gdy wyleci poza plansze.
        /// </summary>
        /// <param name="torpedo">Torpeda do odrejestrowania.</param>
        /// <author>Adam Witczak</author>
        void OnUnregisterTorpedo(Torpedo torpedo);


        void OnUnregisterBunkerShellBullet(BunkerShellBullet shellBullet);
        
        
        void OnUnregisterAmmunition(Ammunition ammo);

        /// <summary>
        /// Funkcja zostaje wywolana gdy zolnierz odbudowuje bunkier.
        /// </summary>
        /// <param name="restoredBunker">Odbudowany bunkier.</param>
        void OnTileRestored(BunkerTile restoredBunker);

        /// <summary>
        /// Funkcja zostaje wywo³ana gdy samolot wystartowa³.
        /// </summary>
        void OnTakeOff();

        /// <summary>
        /// Funkcja zostaje wywo³ana gdy samolot dotkn¹l pod³o¿a lotniskowca.
        /// </summary>
        void OnTouchDown();

        /// <summary>
        /// Funkcja zostaje wywo³ana, gdy skoñczy siê animacja toniêcia (spalenia) samolotu.
        /// 
        /// </summary>
        void OnPlaneWrecked(Plane Plane);

        /// <summary>
        /// Funkcja zostaje wywo³ana, gdy samolot wroga mija samolot gracza.
        /// 
        /// </summary>
        void OnPlanePass(Plane plane);

        /// <summary>
        /// Ukrywa samolot (na razie)
        /// <author>Adam Witczak</author>
        /// </summary>
        /// <param name="plane"></param>
        void OnUnregisterPlane(Plane plane);


        /// <summary>
        /// Efekt trafien samolotu przez dzialko
        /// </summary>
        /// <param name="plane"></param>
        void OnGunHitPlane(Plane plane);

        /// <summary>
        /// Zdarzenie wywolywane przez model, informuje o pojawieniu
        /// sie wrogiego samolotu na planszy
        /// </summary>
        /// <param name="left">Cz z lewej?</param>
        void OnEnemyPlaneFromTheSide(Boolean left);

        /// <summary>
        /// Zdarzenie wywolywane przez model, kiedy samolot
        /// jest zmuszany do zawracania
        /// </summary>
        void OnPlaneForceChangeDirection();

        /// <summary>
        /// Zdarzenie wywolywane przez model, kiedy samolot
        /// jest zmuszany do obnizenia wysokosci
        /// </summary>
        void OnPlaneForceGoDown();

        /// <summary>
        /// Zdarzenie wywo³ywane gdy samolot wroga podejmie decyzje o
        /// zaatakowaniu samolotów na lotniskowcu.
        /// </summary>
        void OnEnemyAttacksCarrier();

        /// <summary>
        /// Zdarzenie wywo³ywane gdy samolot startuje ze z³ej strony.
        /// </summary>
        void OnPlaneWrongDirectionStart();


     
        /// <summary>
        /// Zaczyna pêtlê dŸwiêku z b¹belkami wodnymi
        /// </summary>
        void OnStartWaterBubblesSound();

        /// <summary>
        /// Zatrzymuje dŸwiêk z b¹belkami wodnymi
        /// </summary>
        void OnStopWaterBubblesSound();

        /// <summary>
        /// Metoda zglasza kolejny poziom zniszczen statku.
        /// </summary>
        /// <param name="tile">Element statku. Reprezentuje zniszczenia dla calego statku.</param>
        /// <param name="state">Reprezentuja aktualny poziom zniszczen dla danego statku.</param>
        void OnShipDamaged(ShipTile tile, ShipState state);

        /// <summary>
        /// Statek zaczyna ton¹æ
        /// </summary>
        /// <param name="tile"></param>
        void OnShipBeginSinking(ShipTile tile);  
        
        /// <summary>
        /// Statek tonie
        /// </summary>
        /// <param name="tile"></param>
        void OnShipSinking(ShipTile tile); 
        
         /// <summary>
        /// Statek jest pod powierzchni¹ wody
        /// </summary>
        /// <param name="tile"></param>
        //void OnShipUnderWater(ShipTile tile); 
        
        
        /// <summary>
        /// Statek zaton¹³
        /// </summary>
        /// <param name="tile"></param>
        void OnShipSunk(BeginShipTile tile);
        
        
        /// <summary>
        /// Samolot byc moze bedzie ladowal
        /// </summary>
        /// <param name="p"></param>
        void OnPotentialLanding(Plane p);


        void OnSecondaryFireOnCarrier(Plane userPlane);
        void OnRocketHitPlane(Rocket rocket, Plane plane);
        void OnLevelFinished();
        void OnPlayFanfare();
        void OnPotentialBadLanding(Plane p);
        void OnShipBeginSubmerging(LevelTile tile);
        void OnShipBeginEmerging(LevelTile tile);
        void OnShipSubmerging(LevelTile tile);
        void OnShipEmerging(LevelTile tile);
        void OnShipEmerged(LevelTile tile);
        void OnShipSubmerged(LevelTile tile);
        
        
        IFrameWork GetFramework();
        
    	
		void OnAchievementFulFilled(Achievement a, bool playSound);
    	
		void OnAchievementUpdated(Achievement a);
    }
}