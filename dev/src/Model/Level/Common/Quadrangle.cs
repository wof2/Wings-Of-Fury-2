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
using System.Drawing;
using System.Text;

namespace Wof.Model.Level.Common
{
    /// <summary>
    /// Klasa implementuje obiekt czworokat.
    /// </summary>
    /// <autor>Michal Ziober</autor>
    public class Quadrangle : IDisposable
    {
        #region Fields

        /// <summary>
        /// Lista wierzcholkow.
        /// </summary>
        private List<PointD> peaks;

        /// <summary>
        /// Okreœla aktualny k¹t czworok¹ta.
        /// </summary>
        private float angle;

        /// <summary>
        /// Okreœla pocz¹tkow¹ szerokoœæ
        /// </summary>
        private float width;

        /// <summary>
        /// Okreœla pocz¹tkowa wysokoœæ
        /// </summary>
        private float height;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor bezparametrowy.
        /// </summary>
        /// <autor>Michal Ziober</autor>
        public Quadrangle()
        {
            peaks = new List<PointD>();
            peaks.Add(new PointD(0, 0));
            peaks.Add(new PointD(0, 0));
            peaks.Add(new PointD(0, 0));
            peaks.Add(new PointD(0, 0));
            angle = 0;
            width = 0;
            height = 0;
        }

        /// <summary>
        /// Konstruktor czteroparametrowy.
        /// Wazna jest kolejnosc.
        /// </summary>
        /// <param name="point1">Pierwszy wierzcholek.</param>
        /// <param name="point2">Drugi wierzcholek.</param>
        /// <param name="point3">Trzeci wierzcholek.</param>
        /// <param name="point4">Czwarty wierzcholek.</param>
        /// <autor>Michal Ziober</autor>
        public Quadrangle(PointD point1, PointD point2, PointD point3, PointD point4)
        {
            PeakManager peakManager = new PeakManager(point1, point2, point3, point4);
            if (peakManager.Peaks.Count == 4)
            {
                peaks = peakManager.Peaks;
            }
            else
            {
                peaks = new List<PointD>();
            }
            angle = 0;
            width = Mathematics.MaxCoordinate(true, point1, point2, point3, point4).X -
                    Mathematics.MinCordinate(true, point1, point2, point3, point4).X;
            height = Mathematics.MaxCoordinate(false, point1, point2, point3, point4).Y -
                     Mathematics.MinCordinate(false, point1, point2, point3, point4).Y;
        }

        /// <summary>
        /// Konstruktor jednoparametrowy.
        /// Zakladam popawny porzadek wierzcholkow w kolekcji.
        /// </summary>
        /// <param name="peaksCollection">Kolekacja wierzcholkow.</param>
        /// <remarks>Zakladam poprawna kolejnosc wierzcholkow.</remarks>
        /// <exception cref="ArgumentNullException">Jesli kolekcja bedzie nullem.</exception>
        /// <exception cref="ArgumentException">Jesli rozmiara kolekcji bedzie rozny od 4.</exception>
        /// <autor>Michal Ziober</autor>
        public Quadrangle(ICollection<PointD> peaksCollection)
        {
            if (peaksCollection == null)
                throw new ArgumentNullException("Argument nie moze byc nullem !");
            if (peaksCollection.Count != 4)
                throw new ArgumentException("Rozmiar kolekcji powinien byc rowny 4!");

            peaks = new List<PointD>();
            foreach (PointD peak in peaksCollection)
                peaks.Add(new PointD(peak.X, peak.Y));
        }

        /// <summary>
        /// Konstruktor dwuparametrowy.
        /// </summary>
        /// <param name="point">Lewy dolny punkt czworokata.</param>
        /// <param name="width">Szerokosc czworokata.</param>
        /// <param name="height">Wysokosc czworokata.</param>
        /// <autor>Michal Ziober</autor>
        public Quadrangle(PointD point, float width, float height)
        {
            PointD tmpPoint = null;
            peaks = new List<PointD>();
            peaks.Add(point); //pierwszy wierzcholek
            tmpPoint = new PointD(point.X, point.Y + height);
            peaks.Add(tmpPoint); //drugi wierzcholek
            tmpPoint = new PointD(point.X + width, point.Y + height);
            peaks.Add(tmpPoint); //trzeci wierzcholek
            tmpPoint = new PointD(point.X + width, point.Y);
            peaks.Add(tmpPoint); //czwarty punkt

            angle = 0;
            this.width = width;
            this.height = height;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca liste wierzcholkow.
        /// </summary>
        /// <autor>Michal Ziober</autor>
        public List<PointD> Peaks
        {
            get { return peaks; }
        }

        /// <summary>
        /// Zwraca srodkowy punkt prostokata.
        /// </summary>
        /// <autor>Michal Ziober</autor>
        public PointD Center
        {
            get
            {
                PointD point = new PointD(0, 0);
                point.X = (peaks[0].X + peaks[2].X)/2;
                point.Y = (peaks[1].Y + peaks[3].Y)/2;

                return point;
            }
        }

        /// <summary>
        /// Zwraca kat nachylenia czworokata wzgledem poczatkowego ulozenia wirzcholkow.
        /// Kat jest wyrazony w radianach i z przedzialu [-pi;pi]
        /// </summary>
        /// <author>Emil</author>
        public float Angle
        {
            get { return angle; }
        }

        /// <summary>
        /// Zwraca najmniejsz¹ wartoœæ Y, któr¹ posiada jeden z punktów.
        /// </summary>
        /// <author>Emil</author>
        public float LowestY
        {
            get { return Mathematics.MinCordinate(false, peaks[0], peaks[1], peaks[2], peaks[3]).Y; }
        }

        /// <summary>
        /// Zwraca najmniejsz¹ wartoœæ X, któr¹ posiada jeden z punktów.
        /// </summary>
        /// <author>Emil</author>
        public float LeftMostX
        {
            get { return Mathematics.MinCordinate(true, peaks[0], peaks[1], peaks[2], peaks[3]).X; }
        }

        /// <summary>
        /// Zwraca najwieksza wartosc X.
        /// </summary>
        public float RightMostX
        {
            get { return Mathematics.MaxCoordinate(true, peaks[0], peaks[1], peaks[2], peaks[3]).X; }
        }

        /// <summary>
        /// Jesli prostokat jest odwrocony "brzuchem"
        /// do gory zwraca true, w przeciwnym przypadku false.
        /// </summary>
        public bool IsObverse
        {
            get { return peaks[1].Y < peaks[0].Y; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Zwraca, czy punkt dany punk lezy w srodku czworokata.
        /// (Kod na podstawie http://local.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/)
        /// </summary>
        /// <param name="p">Punkt do sprawdzenia</param>
        /// <returns>Czy dany punkt lezy w srodku czworokata (wlacznie z brzegiem)</returns>
        /// <author>Emil</author>
        private bool PointInside(PointD p)
        {
            int i;
            float angle = 0;
            PointD p1 = new PointD(), p2 = new PointD();

            for (i = 0; i < 4; i++)
            {
                p1.X = Peaks[i].X - p.X;
                p1.Y = Peaks[i].Y - p.Y;
                p2.X = Peaks[(i + 1)%4].X - p.X;
                p2.Y = Peaks[(i + 1)%4].Y - p.Y;
                angle += Angle2D(p1, p2);
            }

            if (Math.Abs(angle) < Math.PI)
                return (false);
            else
                return (true);
        }

        /// <summary>
        /// Zwraca kat miedzy dwoma wektorami na plaszczyznie.
        /// (Kod na podstawie http://local.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/)
        /// </summary>
        /// <param name="p1">Pierwszy wektor</param>
        /// <param name="p2">Drugi wektor</param>
        /// <returns>Kat miedzy danymi wektorami. Kat jest z predzialu  [-pi,pi]. </returns>
        /// <author>Emil</author>
        private float Angle2D(PointD p1, PointD p2)
        {
            float dtheta, theta1, theta2;

            theta1 = Mogre.Math.ATan2((float) p1.Y, (float) p1.X).ValueRadians;
            theta2 = Mogre.Math.ATan2((float) p2.Y, (float) p2.X).ValueRadians;

            dtheta = theta2 - theta1;
            while (dtheta > Mogre.Math.PI)
                dtheta -= 2*Mogre.Math.PI;
            while (dtheta < -Math.PI)
                dtheta += 2*Mogre.Math.PI;

            return (dtheta);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Funkcja zwrca tablice wierzcholkow czworokata.
        /// Skaluje wszystkie wspolrzedne odpowiednia do podanego 
        /// parametru.
        /// </summary>
        /// <param name="height">Wysokosc okna na ktorym chcemy narysowac
        /// prostokat.</param>
        /// <returns>Tablice obiektow typu System.Drawing.Point</returns>
        /// <autor>Michal Ziober</autor>
        public Point[] ToDrawingTable(int height)
        {
            Point[] table = new Point[peaks.Count];
            int count = table.Length;
            for (int i = 0; i < count; i++)
            {
                PointD point = peaks[i];
                table[i] = new PointD(point.X, height - point.Y).ToPoint();
            }
            return table;
        }

        /// <summary>
        /// Zwraca opis obiektu.
        /// </summary>
        /// <returns>String</returns>
        /// <autor>Michal Ziober</autor>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (peaks != null)
                foreach (PointD p in peaks)
                    builder.AppendLine(p.ToString());

            builder.AppendLine("Center point: " + Center.ToString());

            return builder.ToString();
        }

        /// <summary>
        /// Rzutuje dany czworokat na obiekt typu 
        /// RectangleF.
        /// </summary>
        /// <returns></returns>
        public RectangleF ToRectangleF()
        {
            return new RectangleF((float) peaks[0].X, (float) peaks[0].Y, (float) width, (float) height);
        }

        /// <summary>
        /// Zwraca, czy czworokat przecina sie z drugim czworokatem.
        /// (Kod na podstawie http://local.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/)
        /// </summary>
        /// <param name="q">Czworokat, z ktorym chcemy sprawdzic przeciecie.</param>
        /// <returns>Czy czworokaty sie przecinaja (Brzegi tez zaliczamu do czworokatow).</returns>
        /// <author>Emil</author>
        public bool Intersects(Quadrangle q)
        {
            //najpierw sprawdzam czy jeden wielokat nie lezy calkowice
            //nad, pod, z lewej lub z prawej
            bool allLeft = true, allRight = true, allUp = true, allDown = true;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (peaks[i].X >= q.peaks[j].X)
                        allLeft = false;
                    if (peaks[i].X <= q.peaks[j].X)
                        allRight = false;
                    if (peaks[i].Y >= q.peaks[j].Y)
                        allDown = false;
                    if (peaks[i].Y <= q.peaks[j].Y)
                        allUp = false;
                }

            //return !(allLeft || allRight || allUp || allDown);

            //jeœli ¿aden prostok¹t niele¿y ca³kowicie, po lewej, prawej, na górze lub na dole
            //to sprawdzam dok³adnie kolizjê

            if (!allLeft && !allRight && !allUp && !allDown)
            {
                foreach (PointD p in Peaks)
                    if (q.PointInside(p))
                        return true;
                foreach (PointD p in q.Peaks)
                    if (PointInside(p))
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Przesuwa czworokat o dany wektor.
        /// </summary>
        /// <param name="dx">Wspolrzedna x wektora przesuniecia</param>
        /// <param name="dy">Wspolrzedna y wetktora przesuniecia</param>
        /// <author>Emil</author>
        public void Move(float dx, float dy)
        {
            foreach (PointD p in peaks)
                p.Move(dx, dy);
        }

        /// <summary>
        /// Przesuwa czworokat o dany wektor.
        /// </summary>
        /// <param name="p">Wektor przesuniecia</param>
        /// <author>Emil</author>
        public void Move(PointD p)
        {
            foreach (PointD temp in peaks)
                temp.Move(p);
        }

        /// <summary>
        /// Obraca czworokat o dany kat wokol srodka.
        /// </summary>
        /// <param name="angle">Kat obrotu podany w radianach</param>
        /// <author>Emil</author>
        public void Rotate(float angle)
        {
            PointD rotateCenter = Center;
            foreach (PointD p in peaks)
            {
                p.Rotate(rotateCenter, angle);
            }
            //zapisanie aktualnego kata nachylenia i obciecie do odpowiedniego przedzialu
            this.angle += angle;
            while (this.angle > Mogre.Math.PI)
                this.angle -= 2*Mogre.Math.PI;
            while (this.angle < -Mogre.Math.PI)
                this.angle += 2*Mogre.Math.PI;
        }

        /// <summary>
        /// Robi tzw. lustrzane odbicie czworok¹ta.
        /// </summary>
        /// <author>Tomek</author>
        public void MirrorReflection()
        {
            PointD temp;

            temp = peaks[0];
            peaks[0] = peaks[3];
            peaks[3] = temp;

            temp = peaks[1];
            peaks[1] = peaks[2];
            peaks[2] = temp;
        }

        /// <summary>
        /// Robi tzw. lustrzane odbicie czworok¹ta.
        /// </summary>
        /// <author>Adam</author>
        public void HorizontalReflection()
        {
            return;

            PointD temp;

            temp = peaks[0];
            peaks[0] = peaks[1];
            peaks[1] = temp;

            temp = peaks[3];
            peaks[3] = peaks[2];
            peaks[2] = temp;

            angle += (float)Math.PI;

            while (this.angle > Mogre.Math.PI)
                this.angle -= 2 * Mogre.Math.PI;
            while (this.angle < -Mogre.Math.PI)
                this.angle += 2 * Mogre.Math.PI;
        }



        /// <summary>
        /// Rysuje czworokat. (Uzywane w przy testach)
        /// </summary>
        /// <param name="g">Graphics na ktorym ma byc narysowany czworokat</param>
        /// <param name="c">Kolor punktu</param>
        /// <author>Emil</author>
        public void Draw(Graphics g, Color c)
        {
            for (int i = 0; i < 4; i++)
            {
                Point fromPoint =
                    new Point(peaks[i].ToPoint().X, (int) g.VisibleClipBounds.Height - peaks[i].ToPoint().Y);
                Point toPoint =
                    new Point(peaks[(i + 1)%4].ToPoint().X,
                              (int) g.VisibleClipBounds.Height - peaks[(i + 1)%4].ToPoint().Y);
                g.DrawLine(new Pen(c), fromPoint, toPoint);
            }
            g.DrawRectangle(Pens.Black,
                            new Rectangle(
                                new Point(peaks[2].ToPoint().X - 4,
                                          (int) g.VisibleClipBounds.Height - peaks[2].ToPoint().Y), new Size(4, 4)));
            g.DrawEllipse(Pens.Black,
                          new Rectangle(
                              new Point(peaks[3].ToPoint().X - 4,
                                        (int) g.VisibleClipBounds.Height - peaks[3].ToPoint().Y - 4), new Size(4, 4)));
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Zwalnia zaalokowane zasoby.
        /// </summary>
        /// <autor>Michal Ziober</autor>
        public void Dispose()
        {
            if (peaks != null)
            {
                peaks.Clear();
                peaks = null;
            }
        }

        #endregion
    }
}