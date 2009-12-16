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

using System.Diagnostics;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.Planes;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Klasa abstarkcyjna dla roznego rodzaju broni.
    /// </summary>
    /// <author>Michal Ziober</author>
    [DebuggerDisplay("MoveVector = {moveVector} , Angle = {relativeAngle}")]
    public abstract class Ammunition
    {
        #region Protected Fields

        /// <summary>
        /// Wspolczynnk predkosci poziomej.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected const float WidthCoefficient = 0.9f;

        /// <summary>
        /// Wspolczynnik predkosci pionowej.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected const float HeightCoefficient = -12;

        /// <summary>
        /// Prostokat opisujacy obiekt.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected Quadrangle boundRectangle;

        /// <summary>
        /// Wektor ruchu.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected PointD moveVector;

        /// <summary>
        /// Referencja do planszy.
        /// </summary>
        /// <author>Michal Ziober</author>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected Level refToLevel;

        /// <summary>
        /// Stan pocisku.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected MissileState state;

        /// <summary>
        /// Kat sapadania.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected float relativeAngle;

        /// <summary>
        /// Wlasciciel broni.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected Plane ammunitionOwner;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Publiczny konstruktor bezargumentowy. Tworzy obiekt amunicji
        /// o podanych parametrach.
        /// </summary>
        /// <param name="planeSpeed">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu level.</param>
        /// <param name="angle">Kat nachylenia.</param>
        /// <param name="owner">Wlasciciel broni.</param>
        /// <author>Michal Ziober</author>
        public Ammunition(PointD planeSpeed, Level level, float angle, Plane owner)
        {
            //referencja na obiekt Level
            refToLevel = level;

            //kat pod jakim spada pocisk
            relativeAngle = angle;

            //ustawiam pocisk jako niezniszczony.
            state = MissileState.Intact;

            //wlasciciel broni.
            ammunitionOwner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca pozycje obiektu 
        /// w zaleznosci od kierunku ruchu.
        /// </summary>
        /// <author>Michal Ziober</author>
        public PointD Position
        {
            get
            {
                //jesli porusza sie w prawo.
                if (moveVector.X > 0)
                    return boundRectangle.Peaks[3];
                    //jesli porusza sie w lewo.
                else
                    return boundRectangle.Peaks[0];
            }
        }

        /// <summary>
        /// Zwraca stan pocisku. 
        /// </summary>
        /// <example>Jesli pocisk zderzyl sie z jakims 
        /// obiektem zwroci Destroyed.</example>
        /// <author>Michal Ziober</author>
        public MissileState State
        {
            get { return state; }
        }

        /// <summary>
        /// Zwraca srodek prostokata opisujacego bombe.
        /// </summary>
        /// <author>Michal Ziober</author>
        public PointD Center
        {
            get { return (PointD) boundRectangle.Center.Clone(); }
        }

        /// <summary>
        /// Kat pod jakim spada pocisk.
        /// </summary>
        /// <author>Michal Ziober</author>
        public float Angle
        {
            get { return relativeAngle; }
        }

        /// <summary>
        /// Zwraca kierunek ruchu bomby.
        /// </summary>
        /// <author>Michal Ziober</author>
        public Direction Direction
        {
            get
            {
                if (moveVector.X >= 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
        }

        /// <summary>
        /// Zwraca wlasciciela broni.
        /// </summary>
        /// <author>Michal Ziober</author>
        public Plane Owner
        {
            get { return ammunitionOwner; }
        }

        #endregion

        #region Public Abstract Method

        /// <summary>
        /// Zmienia pozycje amunicji w zaleznosci od czasu.
        /// </summary>
        /// <param name="time">Czas od ostatniej zmiany</param>
        /// <author>Michal Ziober</author>
        public abstract void Move(int time);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sprawdzam kolizje z lotniskowce.
        /// </summary>
        /// <param name="ammunition">Rodzaj amunicji.</param>
        /// <author>Michal Ziober</author>
        protected void CheckCollisionWithCarrier(Ammunition ammunition)
        {
            if (Center.Y < 6)
            {
                if (refToLevel.AircraftTiles != null && refToLevel.AircraftTiles.Count > 0)
                {
                    foreach (AircraftCarrierTile tile in refToLevel.AircraftTiles)
                    {
                        if (tile.HitBound != null)
                        {
                            if (boundRectangle.Intersects(tile.HitBound))
                            {
                                refToLevel.Controller.OnTileBombed(tile, ammunition);
                                state = MissileState.Destroyed;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}