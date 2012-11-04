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
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;

namespace Wof.Model.Level.Common
{
    /// <summary>
    /// Statyczna klasa z predykatami do wyszukiwania oraz usuwania
    /// elementow z listy.
    /// </summary>
    /// <remarks>Zastosowanie predykatow do operacji na listach zwieksza wydajnosc
    /// operacji na listach.</remarks>
    /// <author>Michal Ziober</author>
    public static class Predicates
    {
        #region Private Fields

        /// <summary>
        /// Pole ra¿enia.
        /// </summary>
        private const int JarringField = 3;

        #endregion

        #region Public Methods

        /// <summary>
        /// Predykat do wyszukiwania obiektow na liscie spelniajacych
        /// zadane warunki.
        /// </summary>
        /// <param name="isXCoordinate">Jesli true to bedzie sprawdzac wspolrzedna x,
        /// w przeciwnym przypadku - y.</param>
        /// <param name="coordinate">Punkt z szukana wspolrzedna.</param>
        /// <returns>Jesli porowananie sie powiedzie zwroci true, false w przeciwnym przypadku.</returns>
        public static Predicate<PointD> GetAllForXYCoordinate(bool isXCoordinate, float coordinate)
        {
            return delegate(PointD pD)
                       {
                           if (isXCoordinate)
                               return pD.X.Equals(coordinate);
                           else
                               return pD.Y.Equals(coordinate);
                       };
        }

        /// <summary>
        /// Predykat do wyszukiwania obiektow, 
        /// ktore dziedzicza po abstarkcyjnej klasie BunkerTile.
        /// </summary>
        /// <returns>Jesli zostanie spelniony warunek, zwroci dany element.</returns>
        public static Predicate<LevelTile> GetAllBunkerTiles()
        {
            return delegate(LevelTile tiles) { return ((tiles as BunkerTile) != null); };
        }
        
        public static Predicate<Achievement> GetAchievementByType(AchievementType type)
        {
            return delegate(Achievement achievement) { return achievement.Type == type; };
        }
        
 		public static Predicate<Achievement> GetCompletedAchievements()
        {
 			return delegate(Achievement achievement) { return achievement.IsFulfilled(); };
        }

        /// <summary>
        /// Predykat do wyszukiwania obiektow, 
        /// ktore dziedzicza po abstarkcyjnej klasie ShipTile.
        /// </summary>
        /// <returns>Jesli zostanie spelniony warunek, zwroci dany element.</returns>
        public static Predicate<LevelTile> GetAllShipTiles()
        {
            return delegate(LevelTile tiles) { return ((tiles as ShipTile) != null); };
        }


        public static Predicate<Plane> GetAllEnemyFighters()
        {
            return delegate(Plane plane) { return plane is EnemyFighter; };
        }

        public static Predicate<Plane> GetAllEnemyBombers()
        {
            return delegate(Plane plane) { return plane is EnemyBomber; };
        }


        /// <summary>
        /// Predykat do wyszukiwania obiektow, 
        /// ktore dziedzicza po abstarkcyjnej klasie AircraftCarrierTile.
        /// </summary>
        /// <returns>Jesli porowananie sie powiedzie zwroci true, 
        /// false w przeciwnym przypadku.</returns>
        public static Predicate<LevelTile> GetAllAircraftCarrierTiles()
        {
            return delegate(LevelTile tile) { return ((tile as AircraftCarrierTile) != null); };
        }

        /// <summary>
        /// Predykat usuwa wszystkie pociski z listy, ktorych stan 
        /// jest zaznaczony jako Destroyed.
        /// </summary>
        /// <returns>Jesli dany pocisk jest zniszczony zwroci true,
        /// w przeciwnym przypadku false.</returns>
        public static Predicate<Ammunition> RemoveAllDestroyedMissiles()
        {
            return delegate(Ammunition missile) { return (missile.State == MissileState.Destroyed); };
        }

        /// <summary>
        /// Predykat usuwa wszystkich martwych zolnierzy z planszy.
        /// </summary>
        /// <returns>Jesli zwroci true, zolnierza nalezy usunac, w przeciwnym przypadku 
        /// zolnierz zostaje na planszy.</returns>
        public static Predicate<Soldier> RemoveAllDeadSoldiers()
        {
            return delegate(Soldier soldier) { return !soldier.IsAlive; };
        }

        /// <summary>
        /// Predykat usuwa wszystkich martwych zolnierzy z planszy.
        /// </summary>
        /// <returns>Jesli zwroci true, genera³a nalezy usunac, w przeciwnym przypadku 
        /// genera³ zostaje na planszy.</returns>
        public static Predicate<General> RemoveAllDeadGenerals()
        {
            return delegate(General general) { return !general.IsAlive; };
        }

        /// <summary>
        /// Predykat szuka zolnierzy, ktorzy znajduja sie na tile o indexie 
        /// zadanym w parametrze. 
        /// </summary>
        /// <param name="position">Index elementu planszy.</param>
        /// <returns>True - jesli zolnierz znjaduje sie na tym kawalku planszy, w przeciwnym
        /// przypadku false.</returns>
        public static Predicate<Soldier> FindSoldierByPosition(int position)
        {
            return delegate(Soldier s)
                       {
                           int pos = Mathematics.PositionToIndex(s.Position.X);
                           return pos == position;
                       };
        }

        /// <summary>
        /// Predykat szuka zolnierzy ktorzy znajduja sie na tiles o indeksach pomiedzy
        /// pierwszym a drugim parametrem.
        /// </summary>
        /// <param name="start">Startowy tile.</param>
        /// <param name="end">Koncowy tile</param>
        /// <returns>True - jesli zolnierz znjaduje sie na kawalku planszy o indeksie spelniajacym 
        /// warunki przypadku false.</returns>
        public static Predicate<Soldier> FindSoldierFromInterval(int start, int end)
        {
            return delegate(Soldier s)
            {
                if (s.IsAlive)
                {
                    int pos = Mathematics.PositionToIndex(s.Position.X);
                    return start <= pos && pos <= end;
                }
                return false;
            };
        }

        /// <summary>
        /// Predykat szuka genera³ów ktorzy znajduja sie na tiles o indeksach pomiedzy
        /// pierwszym a drugim parametrem.
        /// </summary>
        /// <param name="start">Startowy tile.</param>
        /// <param name="end">Koncowy tile</param>
        /// <returns>True - jesli genera³ znjaduje sie na kawalku planszy o indeksie spelniajacym 
        /// warunki przypadku false.</returns>
        public static Predicate<General> FindGeneralFromInterval(int start, int end)
        {
            return delegate(General s)
            {
                if (s.IsAlive)
                {
                    int pos = Mathematics.PositionToIndex(s.Position.X);
                    return start <= pos && pos <= end;
                }
                return false;
            };
        }

        /// <summary>
        /// Predykat szuka zolnierzy ktorzy 'urodzi³' siê na tile'u o okreœlonym indeksie
        /// </summary>
        /// <returns>True - jesli zolnierz znjaduje sie na kawalku planszy o indeksie spelniajacym 
        /// warunki przypadku false.</returns>
        public static Predicate<Soldier> FindSoldierFromStartingIndex(int index)
        {
            return delegate(Soldier s)
                       {
                           return s.StartLevelIndex == index;
                       };
        }



        /// <summary>
        /// Predykat szuka zolnierzy ktorzy znajduja sie na tiles o indeksach pomiedzy
        /// pierwszym a drugim paraetrem.
        /// </summary>
        /// <param name="position">Wspolrzedna trafienia.</param>
        /// <returns>True - jesli zolnierz znjaduje sie w trafionym polu w  
        /// innym przypadku false.</returns>
        public static Predicate<Soldier> FindSoldierFromInterval(float position)
        {
            return
                delegate(Soldier s) { return position - JarringField <= s.Position.X && s.Position.X <= position + JarringField; };
        }

        /// <summary>
        /// Szuka poczatek wyspy.
        /// </summary>
        /// <returns>Jesli obiekt jest poczatkiem wyspy zwraca true,
        /// false w przeciwnym przypadku.</returns>
        public static Predicate<LevelTile> FindStartIsland()
        {
            return delegate(LevelTile tile) { return (tile is BeginIslandTile); };
        }


        /// <summary>
        /// Szuka koniec wyspy.
        /// </summary>
        /// <returns>Jesli obiekt jest koncem wyspy zwraca true,
        /// false w przeciwnym przypadku.</returns>
        public static Predicate<LevelTile> FindEndIsland()
        {
            return delegate(LevelTile tile) { return (tile is EndIslandTile); };
        }

        /// <summary>
        /// Szuka instalacji obronnych na wyspie.
        /// </summary>
        /// <returns>Jesli dany alement jest instalacja obronna zwraca true,
        /// false - przeciwnym przypadku.</returns>
        public static Predicate<LevelTile> FindAllEnemyInstallationTiles()
        {
            return delegate(LevelTile tile) { return (tile is EnemyInstallationTile); };
        }

        #endregion
    }
}