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
using Wof.Model.Level.LevelTiles;

namespace Wof.Model.Level.Common
{
    /// <summary>
    /// Klasa udostepnia metody do wyznaczenia
    /// kolejnosci wspolrzednych w czworokacie, etc.
    /// </summary>
    /// <author>Michal Ziober</author>
    public static class Mathematics
    {
        #region Const

        /// <summary>
        /// Maksymalny czas pomiedzy odswierzeniami.
        /// </summary>
        /// <value>Czas w milisekundach.</value>
        /// <author>Michal Ziober</author>
        public const int MoveInterval = 1000;

        /// <summary>
        /// Epsilon. Jesli roznica dwoch liczb jest mniejsza to zakladamy
        /// ze sa rowne.
        /// </summary>
        public const float Epsilon = 0.000001f;

        #endregion

        #region Public Method

        /// <summary>
        /// Funkcja zwraca wezel o najmniejszej wspolrzednej(X lub Y).
        /// </summary>
        /// <param name="xCoordinate">Jesli true, to porownwane beda
        /// wspolrzedne x, jesli false - to y.</param>
        /// <param name="list">Parametry do porownania...</param>
        /// <returns>Punkt o najmniejszej wspolrzednej.</returns>
        /// <author>Michal Ziober</author>
        public static PointD MinCordinate(bool xCoordinate, params PointD[] list)
        {
            PointD tmpPoint = null;
            PointD minPoint = new PointD(float.MaxValue, float.MaxValue);
            for (int i = 0; i < list.Length; i++)
            {
                tmpPoint = list[i];
                if (xCoordinate)
                {
                    if (tmpPoint.X < minPoint.X)
                        minPoint = tmpPoint;
                }
                else
                {
                    if (tmpPoint.Y < minPoint.Y)
                        minPoint = tmpPoint;
                }
            }

            return minPoint;
        }

        /// <summary>
        /// Funkcja zwraca wezel o najwiekszej wspolrzednej(X lub Y).
        /// </summary>
        /// <param name="xCoordinate">Jesli true, to porownwane beda
        /// wspolrzedne x, jesli false - to y.</param>
        /// <param name="list">Parametry do porownania...</param>
        /// <returns>Punkt o najwiekszej wspolrzednej.</returns>
        /// <author>Michal Ziober</author>
        public static PointD MaxCoordinate(bool xCoordinate, params PointD[] list)
        {
            PointD tmpPoint = null;
            PointD maxPoint = new PointD(float.MinValue, float.MinValue);
            for (int i = 0; i < list.Length; i++)
            {
                tmpPoint = list[i];
                if (xCoordinate)
                {
                    if (tmpPoint.X > maxPoint.X)
                        maxPoint = tmpPoint;
                }
                else
                {
                    if (tmpPoint.Y > maxPoint.Y)
                        maxPoint = tmpPoint;
                }
            }

            return maxPoint;
        }

        /// <summary>
        /// Przeksztalca pozycje na planszy na index obiektu.
        /// </summary>
        /// <param name="posX">Pozycja na planszy.</param>
        /// <returns>Index elementu na liscie wszystkich elementow w ktorym zawiera sie 
        /// dana pozycja.</returns>
        /// <author>Michal Ziober</author>
        public static int PositionToIndex(float posX)
        {
            int index = Convert.ToInt32(Math.Floor(posX/LevelTile.Width));
            return index;
        }

        /// <summary>
        /// Przekszta³ca indeks pola na wspolrzedna x poczatku danego
        /// pola.
        /// </summary>
        /// <param name="index">Indeks pola.</param>
        /// <returns>Wspolrzedna x poczatku pola.</returns>
        /// <author>Michal Ziober</author>
        public static float IndexToPosition(int index)
        {
            return index*LevelTile.Width;
        }

        /// <summary>
        /// Wylicza o ile nalezy przesunac dany obiekt.
        /// </summary>
        /// <param name="time">Czas od osataniego poruszenia.</param>
        /// <param name="timeUnit">Przedzial czasu pomiedzy kolejnymi przesunieciami.</param>
        /// <returns>Zwraca  odcinek o jaki nalezy przesunac obiekt.</returns>
        /// <author>Michal Ziober</author>
        public static float GetMoveFactor(int time, int timeUnit)
        {
            float segment = ((float) time)/((float) timeUnit);
            return segment;
        }

        /// <summary>
        /// Wylicza o ile nalezy przesunac dany obiekt.
        /// </summary>
        /// <param name="time">Czas od osataniego poruszenia.</param>
        /// <returns>Zwraca  odcinek o jaki nalezy przesunac obiekt.</returns>
        /// <author>Michal Ziober</author>
        public static float GetMoveFactor(int time)
        {
            return GetMoveFactor(time, MoveInterval);
        }

        /// <summary>
        /// Sprawdza czy odcinek ab przecina sie z odcnikiem cd.
        /// </summary>
        /// <param name="a">Pocz¹tek odcinka ab</param>
        /// <param name="b">Koniec odcinka ab</param>
        /// <param name="c">Pocz¹tek odcinka cd</param>
        /// <param name="d">Koniec odcinka cd</param>
        /// <returns></returns>
        public static bool SegmentsIntersect(PointD a, PointD b, PointD c, PointD d)
        {
            if (PointInsideSegment(a, b, c) || PointInsideSegment(a, b, d) ||
                PointInsideSegment(c, d, a) || PointInsideSegment(c, d, b))
                return true;
            if (detMatrix(a, b, c)*detMatrix(a, b, d) >= 0 || detMatrix(c, d, a)*detMatrix(c, d, b) >= 0)
                return false;
            return true;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Sprawdza czy punkt c zmajduje siê na odcinku ab
        /// </summary>
        /// <param name="a">Pocz¹tek odcinka.</param>
        /// <param name="b">Koniec odcinka</param>
        /// <param name="c">Sprawdzany punkt.</param>
        /// <returns></returns>
        private static bool PointInsideSegment(PointD a, PointD b, PointD c)
        {
            float det = detMatrix(a, b, c);

            if (det != 0)
                return false;
            else
            {
                if ((Math.Min(a.X, b.X) <= c.X) && (c.X <= Math.Max(a.X, b.X)) &&
                    (Math.Min(a.Y, b.Y) <= c.Y) && (c.Y <= Math.Max(a.Y, b.Y)))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Oblicza wyznacznik macierzy z³o¿ony z 3 pubktów w 2D.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static float detMatrix(PointD a, PointD b, PointD c)
        {
            return a.X*b.Y + b.X*c.Y + c.X*a.Y - c.X*b.Y - a.X*c.Y - b.X*a.Y;
        }

        #endregion
    }
}