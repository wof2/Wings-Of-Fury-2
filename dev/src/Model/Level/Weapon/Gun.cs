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
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.Planes;
using Math=Mogre.Math;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.Model.Level.Weapon
{
    #region Class Gun

    /// <summary>
    /// Klasa implementujaca dzialanie dzialka.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class Gun
    {
        #region Public Const

      

        /// <summary>
        /// Zasieg dzialka w poziomie.
        /// </summary>
        /// <author>Michal Ziober</author>
        public const int RangeX = 90;

        /// <summary>
        /// Zasieg dzialka w pionie.
        /// </summary>
        /// <author>Michal Ziober</author>
        public const int RangeY = 90;

        #endregion

        #region Private Fields

        /// <summary>
        /// Referencja do obiektu planszy.
        /// </summary>
        /// <author>Michal Ziober</author>
        private Level referenceToLevel;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor jednoparametrowy. Tworzy obiekt 
        /// implementujacy dzialko o podanych argumentach.
        /// </summary>
        /// <param name="refToLevel">Referencja do obiektu planszy.</param>
        /// <author>Michal Ziober</author>
        public Gun(Level refToLevel)
        {
            referenceToLevel = refToLevel;
        }

        #endregion

        #region Private Method

      
        #endregion

        #region Public Method

        /// <summary>
        /// Sprawdza kolizje z ziemia.
        /// </summary>
        /// <param name="planeBound">Prostokat opisujacy samolot.</param>
        /// <param name="position">Pozycja samolotu.</param>
        /// <param name="direction">Kierunek lotu samolotu</param>
        /// <returns>Zwraca punkt zderzenia pocisku z nawierzchnia.</returns>
        /// <author>Adam Witczak</author>
        public PointD GetHitPosition(Quadrangle planeBound, PointD position, Direction direction)
        {

           
            // czy trafienie bedzie po prawej od samolotu czy po lewej
            Direction hitDirection;
            if (Math.Abs(planeBound.Angle) < Mogre.Math.HALF_PI)
            {
                hitDirection = Direction.Left;
            }
            else
            {
                hitDirection = Direction.Right;
            }

            if (direction == Direction.Right)
            {
                if (hitDirection == Direction.Left) hitDirection = Direction.Right; else hitDirection = Direction.Left;
            }
        
            LevelTile tile;
            int startIndex;
            int maxIndex;

            if (hitDirection == Direction.Left)
            {
                maxIndex = GetStartIndex(position.X);
                startIndex = System.Math.Max(0, maxIndex - 12);

            }
            else
            {
                startIndex = GetStartIndex(position.X);
                maxIndex = startIndex + 12;
            }

            for (int i = startIndex; i < maxIndex && i < referenceToLevel.LevelTiles.Count; i++)
            {
                // przeszukaj 12 tile'ów do przodu
                tile = referenceToLevel.LevelTiles[i];
                if (tile.HitBound != null)
                {
                    Line lineA = new Line(tile.HitBound.Peaks[1], tile.HitBound.Peaks[2]);
                    Line lineB = new Line(planeBound.Peaks[0], planeBound.Peaks[3]);
                    PointD cut = lineA.Intersect(lineB);
                    if (cut == null) continue;
                    if (cut.X > tile.HitBound.Peaks[2].X || cut.X < tile.HitBound.Peaks[1].X) continue; // sytuacja gdy samolot strzela dalej, za tile'em (czyli de facto kula uderzy³aby gdzies dalej mimo ze jest 'intersection' z prost¹)
                    if (IsCutInRangeX(cut, planeBound.Center)) return cut;
                }
            }
            return null;
        }
    /*    public static bool CanHitObjectByGun(IObject2D owner, IObject2D target)
        {
            return CanHitObjectByGun(owner, target, 0);
        }
        public static bool CanHitObjectByGun(IObject2D owner, IObject2D target, float tolerance)
        {
            return CanHitObjectByGun(owner, target, tolerance, false);
        }*/

        /// <summary>
        /// Funkcja sprawdza czy samolot bedzie mogl trafic dzialkiem w inny obiekt.
        /// </summary>
        /// <param name="owner">Samolot strzelajacy.</param>
        /// <param name="target">Samolot do ktorego strzelaja.</param>
        /// <param name="tolerance">Tolerancja. 0 - oznacza pewne trafienie.</param>
        /// <returns>Zwraca true jesli moze trafic wrogi samolot; false - w przeciwnym
        /// przypadku.</returns>
        /// <author>Michal Ziober</author>
        public static bool CanHitObjectByGun(IObject2D owner, IObject2D target, float tolerance, bool biDirectional)
        {
           
            // UWAGA: Zakladamy ze 

            if (!biDirectional)
            {
                Direction gunDirection = owner.MovementVector.X > 0 ? Direction.Right : Direction.Left;

                //   Console.WriteLine( (int)owner.Direction * (owner.Bounds.Peaks[2].X - owner.Bounds.Peaks[1].X));


                if (gunDirection == Direction.Right && owner.Center.X > target.Center.X)
                    return false;

                if (gunDirection == Direction.Left && owner.Center.X < target.Center.X)
                    return false;
            }


            /*  if (System.Math.Abs(owner.Center.X - target.Center.X) < 10 &&
                System.Math.Abs(owner.Center.Y - target.Center.Y) < 10)
                return false;
            */

            /*
            if ((owner.Bounds.Center - target.Bounds.Center).EuclidesLength > Math.Sqrt(RangeX * RangeX + RangeY * RangeY))
            {
                return false;
            }
            
            */


            Quadrangle ownerBound = owner.Bounds;
            Quadrangle targetBound = target.Bounds;
            Line lineA = new Line(ownerBound.Peaks[1], ownerBound.Peaks[2]);
            for (int i = 0; i < targetBound.Peaks.Count - 1; i++)
            {

                PointD start = targetBound.Peaks[i];
                start += new PointD(Math.RangeRandom(-tolerance * 0.5f, -tolerance * 0.5f), Math.RangeRandom(-tolerance * 0.5f, -tolerance * 0.5f));
                
                PointD finish = targetBound.Peaks[i + 1];
                finish += new PointD(Math.RangeRandom(-tolerance * 0.5f, -tolerance * 0.5f), Math.RangeRandom(-tolerance * 0.5f, -tolerance * 0.5f));

                Line lineB = new Line(start, finish);
              
                PointD cut = lineA.Intersect(lineB);
                if (cut == null)
                    continue;
                if (IsCutInRange(cut, target))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Sprawdza czy punkt przeciecia jest w zasiegu samolotu.
        /// </summary>
        /// <param name="cut">Punkt przeciecia dwoch prostych.</param>
        /// <param name="plane"></param>
        /// <returns>true jesli punkt przeciecia jest w zasiegu, false
        /// w przeciwnym przypadku.</returns>
        /// <author>Michal Ziober</author>
        private static bool IsCutInRange(PointD cut, IObject2D target)
        {
           // PointD diff = (cut - plane);
           PointD tpoint = target.Bounds.Center;
           
          //  return diff.EuclidesLength < (new PointD(Plane.Width, Plane.Height).EuclidesLength) * 0.5f ;
            return
                ((cut.Y > tpoint.Y - target.Bounds.Height) && (cut.Y < tpoint.Y + target.Bounds.Height))
                &&
                ((cut.X > tpoint.X - target.Bounds.Width) && (cut.X < tpoint.X + target.Bounds.Width));
        }

        /// <summary>
        /// Sprawdza czy punkt przeciecia jest w zasiegu samolotu.
        /// </summary>
        /// <param name="cut">Punkt przeciecia dwoch prostych.</param>
        /// <param name="plane">Pozycja samolotu.</param>
        /// <returns>true jesli punkt przeciecia jest w zasiegu, false
        /// w przeciwnym przypadku.</returns>
        /// <author>Michal Ziober</author>
        private static bool IsCutInRangeX(PointD cut, PointD plane)
        {
            return System.Math.Abs(cut.X - plane.X) < RangeX && System.Math.Abs(cut.Y - plane.Y) < RangeY;
        }

        /// <summary>
        /// Zwraca startowy indeks kawalka planszy od ktorego 
        /// zaczne sprawdzac kolizje.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Indeks startowego pola</returns>
        /// <author>Michal Ziober</author>
        private int GetStartIndex(float position)
        {
            int index = Mathematics.PositionToIndex(position);
            int min = System.Math.Max(0, index) + 1;
            return min;
        }

        #endregion
    }

    #endregion
}