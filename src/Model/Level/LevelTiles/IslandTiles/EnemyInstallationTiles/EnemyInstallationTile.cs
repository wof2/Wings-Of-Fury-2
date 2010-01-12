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
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.XmlParser;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{

    #region Registry Soldier delegate

    /// <summary>
    /// Delegat dodajacy nowego zolnierza.
    /// </summary>
    /// <param name="sender">Obiekt wysylajacy.</param>
    /// <param name="soldier">Obiekt do dodania.</param>
    public delegate void RegistrySoldier(object sender, Soldier soldier);

    #endregion

    /// <summary>
    /// Klasa abstarkcyjna dla wszystkich instalacji obronnych na 
    /// planszy.
    /// </summary>
    public abstract class EnemyInstallationTile : IslandTile, IDestroyable, IRefsToLevel
    {

      

        #region Static Fields

        /// <summary>
        /// Liczba niezniszczonych instalacji obronnych.
        /// </summary>
        private static int installationsCount = 0;

        /// <summary>
        /// Obiekt synchronizujacy.
        /// </summary>
        private static Object lockObject = new Object();

        #endregion

        #region Fields

        /// <summary>
        /// Stan instalacji obronnej.
        /// </summary>
        protected EnemyInstallationState enemyState;

        /// <summary>
        /// Liczba zolnierzy w danej instalacji.
        /// </summary>
        protected int soldiersCount;


        protected int maxInfantryCount;

        /// <summary>
        /// Liczba generalow w danej instalacji.
        /// </summary>
        protected int generalsCount;

        
        #endregion

        #region Public Constructor

        /// <summary>
        /// Publiczny konstruktor czteroparametrowy.
        /// </summary>
        /// <param name="yBegin">Wspolrzedna y poczatku instalacji.</param>
        /// <param name="yEnd">Wspolrzedna y konca instalacji.</param>
        /// <param name="hitBound">Prostokat opisujacy obiekt.</param>
        /// <param name="soldierNum">Liczba zolnierzy.</param>
        /// <param name="type">Typ bunkra.</param>
        /// <param name="collisionRectangle">Lista prostokatow z ktorymi moga wystapic zderzenia.</param>
        public EnemyInstallationTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int soldierNum, int generalNum, int type,
                                     List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, type, collisionRectangle, true)
        {
            Initializing();
            soldiersCount = soldierNum;
            generalsCount = generalNum;
            maxInfantryCount = generalsCount + soldiersCount;
            IncrementIntactInstallationCount();
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Funkcja inicjalizuje prywatne zmienne.
        /// </summary>
        public void Initializing()
        {
            enemyState = EnemyInstallationState.None;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pobiera stan instalacji obronnej.
        /// </summary>
        public bool IsDestroyed
        {
            get { return (enemyState == EnemyInstallationState.Destroyed); }
        }

        /// <summary>
        /// Zwraca ilosc zolnierzy znajdujacych sie w bunkrze.
        /// </summary>
        public int SoldierCount
        {
            get { return soldiersCount; }
        }

        /// <summary>
        /// Zwraca ilosc genera³ów znajdujacych sie w bunkrze.
        /// </summary>
        public int GeneralCount
        {
            get { return generalsCount; }
        }



        #endregion

        #region Public Method

       
       /* public CollisionType InCollision(Quadrangle quad)
        {
            if (quad == null) return false;
            List<Quadrangle> list = BoundingQuadrangles;
            for (int i = 0; i < list.Count; i++)
                if (list[i].Intersects(quad))
                    return CollisionType.;
            return false;
        }*/

        /// <summary>
        /// Funkcja niszczy instalacje obronna.
        /// </summary>
        public virtual void Destroy()
        {
            enemyState = EnemyInstallationState.Destroyed;

            Soldier newSoldier = null;
            Direction curentDirect = Direction.Left;
            Random rand = new Random(Environment.TickCount);
            for (int i = 0; i < generalsCount; i++)
            {
                newSoldier = new General(tilesIndex, curentDirect, refToLevel, rand.Next(1, 9));
				
                // generals run faster
                newSoldier.Speed = rand.Next((int)(GameConsts.Soldier.MinSpeed * 1.1f), (int)(GameConsts.Soldier.MaxSpeed * 1.1f));
                if (curentDirect == Direction.Left)
                    curentDirect = Direction.Right;
                else
                    curentDirect = Direction.Left;
                //Wysylam event aby dodac nowego zolnierza do listy.
                RegistrySoldierEvent(this, newSoldier);
                //rejestruje zolnierza w controlerze.
                refToLevel.Controller.OnRegisterSoldier(newSoldier, this.refToLevel.MissionType);
            }

            newSoldier = null;
			bool holdsBazooka;
            for (int i = 0; i < soldiersCount; i++)
            {
            	holdsBazooka = rand.Next(0,10) > 5;
                //Soldier.SoldierType stype = Soldier.SoldierType.SOLDIER;
                if (this is ShipBunkerTile)
                {
                    newSoldier = new Seaman(tilesIndex, curentDirect, refToLevel, rand.Next(1, 9), holdsBazooka);
                    // stype = Soldier.SoldierType.SEAMAN;
                }
                else
                {
                    /*
                    if (rand.NextDouble() > 0.67f)
                    {
                        stype = Soldier.SoldierType.GENERAL;
                    }*/
                    newSoldier = new Soldier(tilesIndex, curentDirect, refToLevel, rand.Next(1, 9), holdsBazooka);
                }
                //newSoldier = new Soldier(tilesIndex, curentDirect, refToLevel, rand.Next(1, 9), stype);
                newSoldier.Speed = rand.Next(GameConsts.Soldier.MinSpeed, GameConsts.Soldier.MaxSpeed);
                if (curentDirect == Direction.Left)
                    curentDirect = Direction.Right;
                else
                    curentDirect = Direction.Left;
                //Wysylam event aby dodac nowego zolnierza do listy.
                RegistrySoldierEvent(this, newSoldier);
                //rejestruje zolnierza w controlerze.
                refToLevel.Controller.OnRegisterSoldier(newSoldier, this.refToLevel.MissionType);
            }
            rand = null;
            //zeruje liczbe zolnierzy w bunkrze.
            soldiersCount = 0;

            //zeruje liczbe genera³ów w bunkrze.
            generalsCount = 0;

            DecrementIntactInstallationCount();
        }

        /// <summary>
        /// Zwieksza o 1 liczbe niezniszczonych instalacji obronnych.
        /// </summary>
        public static void IncrementIntactInstallationCount()
        {
            lock (lockObject)
            {
                installationsCount++;
            }
        }

        /// <summary>
        /// Zmniejsza o 1 liczbe niezniszczonych instalacji obronnych.
        /// </summary>
        public static void DecrementIntactInstallationCount()
        {
            lock (lockObject)
            {
                installationsCount--;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Ustawia prywatna referencje do planszy.
        /// </summary>
        public Level LevelProperties
        {
            set { refToLevel = value; }
        }

        /// <summary>
        /// Zwraca liczbe niezniszczonych instalacji obronnych wroga.
        /// </summary>
        public static int Count
        {
            get { return installationsCount; }
        }

        public int MaxInfantryCount
        {
            get { return maxInfantryCount; }
        }

        #endregion

        #region Registry soldier event

        /// <summary>
        /// Komunikat o dodaniu nowego zolnierza.
        /// </summary>
        public event RegistrySoldier RegistrySoldierEvent;

        #endregion
    }
}