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

using Mogre;
using System;
using System.Drawing;
using Math = Mogre.Math;

namespace Wof.Model.Level.Common
{
    /// <summary>
    /// Klasa opisujaca punkt w przestrzeni
    /// na liczba typu float.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class PointD : ICloneable, IComparable<PointD>
    {
        #region Fields

        /// <summary>
        /// Wspolrzedna X.
        /// </summary>
        private float mX;

        /// <summary>
        /// Wspolrzedna Y.
        /// </summary>
        private float mY;

        #endregion

        public static PointD ZERO
        {
            get { return new PointD(0, 0); }
        }
        #region Public Constructors

        
        /// <summary>
        /// Konstruktor bezparametrowy.
        /// </summary>
        public PointD()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="point">Punkt na podstawie ktorego,
        /// zostanie stworzony nowy punkt.</param>
        public PointD(PointD point)
            : this(point.X, point.Y)
        {
        }

        /// <summary>
        /// Konstruktor dwuparametrowy.
        /// </summary>
        /// <param name="x">Wspolrzedna x.</param>
        /// <param name="y">Wspolrzedna y.</param>
        public PointD(float x, float y)
        {
            mX = x;
            mY = y;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pobiera lub ustawia wpolrzedna X.
        /// </summary>
        public float X
        {
            get { return mX; }
            set { mX = value; }
        }

        /// <summary>
        /// Pobiera lub ustawia wspolrzedna Y.
        /// </summary>
        public float Y
        {
            get { return mY; }
            set { mY = value; }
        }

        /// <summary>
        /// Sprawdza czy wspolrzedne danego punktu sa rowne 0.
        /// Jesli obie wspolrzedne sa rowne zero - zwroci true,
        /// w p.p. - false.
        /// </summary>
        public bool IsEmpty
        {
            get { return ((mX == 0) && (mY == 0)); }
        }

        /// <summary>
        /// Pobiera k¹t nachylenia wektora poprowadzonego do punktu z osi¹ OX.
        /// (Mierzony w radianach z przedzia³u [-pi;pi])
        /// </summary>
        public float Angle
        {
            get { return Math.ATan2(mY, mX).ValueRadians; }
        }
        
        public float GetRelativeAngle(Direction direction)
        {
         	 
         	float mvectorangle = Angle; 
	    	if(direction == Direction.Left) {
	    		if(mvectorangle < 0) {
	    			mvectorangle = -(mvectorangle + Math.PI);	    			
	    		}else {
	    			mvectorangle = Math.PI - mvectorangle;
	    		}
         		
         	}
         	
         	return mvectorangle;
        }
         	
         	
         	
      

        /// <summary>
        /// Zwraca d³ugoœæ euklidesowa gdy obiekt traktujemy
        /// jako wektor.
        /// Zmienia d³ugoœæ wektora nie zmieniaj¹c kierunku.
        /// </summary>
        /// <returns>d³ugoœæ</returns>
        /// <author>Tomek</author>
        public float EuclidesLength
        {
            get { return Math.Sqrt(X*X + Y*Y); }
            set
            {
                if (value > 0)
                {
                	float angle = Angle;
                    mX = Math.Cos(angle)*value;
                    mY = Math.Sin(angle)*value;
                } 
                else
                {
                   mX = 0;
                   mY = 0;
                }
               
            }
            /*
            set
            {
                if (value != 0 && !this.IsEmpty)
                {
                	float len = EuclidesLength;
                	float lenProp = value / len;
                	mX*= lenProp;
                	mY*= lenProp;
                   // mX = Math.Cos(Angle)*value;
                  //  mY = Math.Sin(Angle)*value;
                } 
                else
                {
                   mX = 0;
                   mY = 0;
                }
               
            }*/
        }

        public int SignX
        {
            get { return System.Math.Sign(X); }
        }

        public int SignY
        {
            get { return System.Math.Sign(Y); }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Operator dadawania.
        /// </summary>
        /// <param name="pointA">Pierwszy parametr.</param>
        /// <param name="pointB">Drugi parametr.</param>
        /// <returns>Sume dwoch punktow.</returns>
        public static PointD operator +(PointD pointA, PointD pointB)
        {
            return new PointD(pointA.mX + pointB.mX, pointA.mY + pointB.mY);
        }

        /// <summary>
        /// Operator odejmowania.
        /// </summary>
        /// <param name="pointA">Pierwszy parametr.</param>
        /// <param name="pointB">Drugi parametr.</param>
        /// <returns>Roznice dwoch punktow.</returns>
        public static PointD operator -(PointD pointA, PointD pointB)
        {
            return new PointD(pointA.mX - pointB.mX, pointA.mY - pointB.mY);
        }

        /// <summary>
        /// Mnozenia przez liczbe.
        /// </summary>
        /// <param name="d">Liczba, przez ktora zostanie pomnozony punkt.</param>
        /// <param name="p">Punkt.</param>
        /// <returns>Iloczyn punktu i liczby.</returns>
        public static PointD operator *(float d, PointD p)
        {
            return new PointD(p.X*d, p.Y*d);
        }
        
         /// <summary>
        /// Dzielenia przez liczbe.
        /// </summary>
        /// <param name="d">Liczba, przez ktora zostanie podzielony punkt.</param>
        /// <param name="p">Punkt.</param>
        /// <returns>Iloczyn punktu i liczby.</returns>
        public static PointD operator /(PointD p, float d)
        {
            return new PointD(p.X/d, p.Y/d);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Przesuwa dany punkt o podany wektor.
        /// </summary>
        /// <param name="x">Przesuniecie wzgledem wspolrzednej x.</param>
        /// <param name="y">Przesuniecie wzgledem wspolrzednej y.</param>
        public void Offset(float x, float y)
        {
            mX += x;
            mY += y;
        }
        
        /// <summary>
        /// Przeksztalca dany punkt w obiekt klasy
        /// Vector2
        /// </summary>
        /// <returns>Obiekt klasy Vector2.</returns>
        public Vector2 ToVector2()
        {
            Vector2 point = new Vector2();
            point.x = mX;
            point.y = mY;

            return point;
        }

        /// <summary>
        /// Przeksztalca dany punkt w obiekt klasy
        /// System.Drawing.Point
        /// </summary>
        /// <returns>Obiekt klasy Point.</returns>
        public Point ToPoint()
        {
            Point point = new Point();
            point.X = Convert.ToInt32(mX + 0.5);
            point.Y = Convert.ToInt32(mY + 0.5);

            return point;
        }

        public override int GetHashCode()
        {
//xor.
            return (int) mX ^ (int) mY;
        }

        /// <summary>
        /// Porownuje dwa obiekty.
        /// </summary>
        /// <param name="obj">Obiekt do porownania.</param>
        /// <returns>Jesli zwroci true - obiekty sa rowne.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PointD))
                return false;

            return (this == (PointD) obj);
        }

        /// <summary>
        /// Tworzy opis obiektu.
        /// </summary>
        /// <returns>Zwraca opis obiektu.</returns>
        public override string ToString()
        {
            return "X: " + mX + " Y: " + mY;
        }

        /// <summary>
        /// Przesuwa punkt o dany wektor.
        /// </summary>
        /// <param name="dx">Wspolrzedna x wektora przesuniecia</param>
        /// <param name="dy">Wspolrzedna y wetktora przesuniecia</param>
        /// <author>Emil</author>
        public void Move(float dx, float dy)
        {
            X += dx;
            Y += dy;
        }

        /// <summary>
        /// Przesuwa punkt o dany wektor.
        /// </summary>
        /// <param name="p">Wektor przesuniecia</param>
        /// <author>Emil</author>
        public void Move(PointD p)
        {
            X += p.X;
            Y += p.Y;
        }

        /// <summary>
        /// Obraca punkt o kat angle wzgledem punktu rotateCenter.
        /// </summary>
        /// <param name="rotateCenter">Srodek obrotu</param>
        /// <param name="angle">Kat o ktory, chcemy obrocic (podany w radianach).</param>
        /// <author>Emil</author>
        public void Rotate(PointD rotateCenter, float angle)
        {
            if (rotateCenter == this)
                return;
            //mX = (mX - rotateCenter.mX) * Math.Cos(angle) + rotateCenter.mX - (mY - rotateCenter.mY) * Math.Sin(angle);
            //mY = (mX - rotateCenter.mX) * Math.Sin(angle) + (mY - rotateCenter.mY) * Math.Cos(angle) + rotateCenter.mY;

            Move(-1*rotateCenter);
            float oldX = mX,
                  oldY = mY;

            mX = oldX*Math.Cos(angle) - oldY*Math.Sin(angle);
            mY = oldX*Math.Sin(angle) + oldY*Math.Cos(angle);
            Move(rotateCenter);
        }


        /// <summary>
        /// Zwiêksza odleg³oœæ punktu od punktu (0,0), nie zmieniaj¹c k¹ta nachylenia.
        /// </summary>
        /// <param name="length">D³ugoœæ o jak¹ ma byæ przesuniêty punkt.</param>
        public void Extend(float length)
        {
            //tworzê wektor o kierunku zgodnym z danym punktem i d³ugoœci¹ lenght
            PointD addVector = new PointD(Math.Cos(Angle)*length, Math.Sin(Angle)*length);
            Move(addVector);        
         
        }
        
        public void Normalise()
        {
        	PointD p = new PointD( this / EuclidesLength);
        	this.X = p.X;
        	this.Y = p.Y;
        }

        /// <summary>
        /// Rysuje punkt jako kolo o danym kolorze. (Uzywane przy testach)
        /// </summary>
        /// <param name="g">Graphics na ktorym ma byc narysowany czworokat</param>
        /// <param name="c">Kolor punktu</param>
        /// <author>Emil</author>
        public void Draw(Graphics g, Color c)
        {
            float x = mX;
            float y = g.VisibleClipBounds.Height - mY;

            g.FillEllipse(new SolidBrush(c), (float) (x - 3), (float) (y - 3), 6.0f, 6.0f);
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Tworzy nowy obiekt z o takich samych 
        /// wspolrzednych.
        /// </summary>
        /// <returns>Zwraca nowa instacje klasy Object.</returns>
        public Object Clone()
        {
            PointD newPoint = new PointD(mX, mY);
            return newPoint;
        }

        #endregion

        #region IComparable<PointD> Members

        /// <summary>
        /// Porownuje dwa obiekty.
        /// </summary>
        /// <param name="other">Obiekt do porownania.</param>
        /// <returns>Jesli wspolrzedne sa rowne zwroci zero.</returns>
        public int CompareTo(PointD other)
        {
            if (mX == other.mX)
                return mY.CompareTo(other.mY);

            return mX.CompareTo(other.mX);
        }

        #endregion
    }
}