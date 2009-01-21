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
using System.Text;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.Watercraft;

namespace Wof.Model.Level.LevelTiles
{

    #region TileKind

    /// <summary>
    /// Rodzaj czesci planszy.
    /// </summary>
    public enum TileKind
    {
        /// <summary>
        /// Ocean.
        /// </summary>
        Ocean,

        /// <summary>
        /// Lotniskowiec.
        /// </summary>
        AircraftCarrier,

        /// <summary>
        /// Wyspa.
        /// </summary>
        Island,
        /// <summary>
        /// statek.
        /// </summary>
        Ship
    }

    #endregion

    /// <summary>
    /// Klasa bazowa dla wszystkich obiektow wczytywanych z 
    /// pliku xml.
    /// </summary>
    public abstract class LevelTile : IRenderableQuadrangles
    {
        #region Const

        /// <summary>
        /// Szerokosc jednego elementu.
        /// </summary>
        public const int Width = 10;

        #endregion

        #region Fields



        /// <summary>
        /// Okreœla czy statek ma ton¹æ.
        /// </summary>
        protected bool isSinking = false;

        /// <summary>
        /// Okreœla czy statek zaton¹³.
        /// </summary>
        protected bool isSunkDown = false;

        



        /// <summary>
        /// Czas jaki min¹³ od momentu kiedy rozpoczê³o siê toniêcie.
        /// </summary>
        private float wreckTimeElapsed = 0;


        /// <summary>
        /// Moc hamowania wody w pionie (do toniêcia).
        /// </summary>
        private readonly float waterYBreakingPower = SinkingSpeed * 0.5f;



        /// <summary>
        /// Okreœla z jak¹ prêdkoœci¹ statek bêdzie ton¹æ. Wyra¿ona jako liczba dodatnia.
        /// </summary>
        public static float SinkingSpeed = 0.6f;

        /// <summary>
        /// Czas od momentu rozbicia statku do momentu zakoñczenia toniêcia.
        /// Wyra¿ony w ms.
        /// </summary>
        private const float wreckTime = 12000;


        /// <summary>
        /// Wysokoœæ nad poziomem morza. Standardowo 0. Przy toniêciu < 0
        /// </summary>
        protected float depth = 0.0f;




        /// <summary>
        /// Wysokosc poczatku elementu.
        /// </summary>
        protected float yBegin;

        /// <summary>
        /// Wysokosc konca elementu.
        /// </summary>
        protected float yEnd;

        /// <summary>
        /// Prostokat opisujacy obiekt.
        /// </summary>
        private Quadrangle hitBound;

        /// <summary>
        /// Dodatkowa lista obiektow z ktorymi moga
        /// wystapic kolizje.
        /// </summary>
        private List<Quadrangle> collisionRectangles;

        /// <summary>
        /// Indeks tile na liscie obiektow.
        /// </summary>
        protected int tilesIndex;

        /// <summary>
        /// Typ pola.
        /// </summary>
        protected int type;

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca wysokosc poczatku elementu.
        /// </summary>
        public float YBegin
        {
            get { return yBegin; }
        }

        /// <summary>
        /// Zwraca wysokosc konca elementu.
        /// </summary>
        public float YEnd
        {
            get { return yEnd; }
        }

        /// <summary>
        /// Zwraca czworokat opisuj¹cy tile'a (teren wyspu, ocean, lotniskowiec).
        /// </summary>
        public Quadrangle HitBound
        {
            get { return hitBound; }
        }

        /// <summary>
        /// Zwraca liste prostokatow, z ktorymi moze wystapic 
        /// kolizja podczas gry.
        /// </summary>
        public List<Quadrangle> ColisionRectangles
        {
            get { return collisionRectangles; }
        }

        public bool IsSinking
        {
            get { return isSinking; }
        }

        public bool IsSunkDown
        {
            get { return isSunkDown; }
        }

        /// <summary>
        /// Pobiera lub ustawia index obiektu na plaszy.
        /// </summary>
        public virtual int TileIndex
        {
            set
            {
                tilesIndex = value;
                int positionX = value * Width;
                hitBound = new Quadrangle(new PointD(positionX, 0), new PointD(positionX, YBegin),
                                          new PointD(positionX + Width, yEnd), new PointD(positionX + Width, 0));

                if (collisionRectangles != null && collisionRectangles.Count > 0)
                {
                    List<Quadrangle> tmpList = collisionRectangles;
                    collisionRectangles = new List<Quadrangle>();
                    for (int i = 0; i < tmpList.Count; i++)
                    {
                        collisionRectangles.Add(new Quadrangle(tmpList[i].Peaks));
                    }
                    for (int i = 0; i < collisionRectangles.Count; i++)
                    {
                        collisionRectangles[i].Move(positionX, 0);
                    }
                }
            }
            get { return tilesIndex; }
        }

        /// <summary>
        /// Zwraca rodzaj danej czesci planszy.
        /// </summary>
        public TileKind TileKind
        {
            get
            {
                if (this is IslandTile)
                    return TileKind.Island;
                if (this is ShipTile)
                    return TileKind.Ship;
                if (this is OceanTile)
                    return TileKind.Ocean;
                if (this is AircraftCarrierTile)
                    return TileKind.AircraftCarrier;
                throw new Exception("Nieznany typ Tile'a");
            }
        }

        /// <summary>
        /// Zwraca czy dany tile jest koñcem lub pocz¹tkiem lotniskowca.
        /// </summary>
        public bool IsAircraftCarrier
        {
            get { return (this is BeginAircraftCarrierTile || this is EndAircraftCarrierTile || this is AircraftCarrierTile); }
        }

        public bool isShipBunker
        {
            get { return (this is ShipBunkerTile); }
        }

        /// <summary>
        /// Pobiera typ(wariant) pola.
        /// </summary>
        public int Variant
        {
            get { return type; }
        }

        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor czteroparametrowy.
        /// </summary>
        /// <param name="yBegin">Wysokosc poczatku elementu.</param>
        /// <param name="yEnd">Wysokosc konca elementu.</param>
        /// <param name="hitBound">Czworokat opisujacy.</param>
        /// <param name="colisionRectanglesList">Lista prostokatow z ktorymi moga wystapic zderzenia.</param>    
        public LevelTile(float yBegin, float yEnd, Quadrangle hitBound, List<Quadrangle> colisionRectanglesList)
        {
            this.yBegin = yBegin;
            this.yEnd = yEnd;
            if (hitBound != null)
                this.hitBound = hitBound;

            collisionRectangles = colisionRectanglesList;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Zwraca opis danego elementu.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("yBegin: " + yBegin);
            builder.AppendLine("yEnd: " + yEnd);
            if (hitBound != null)
                builder.AppendLine("Hit bound: " + hitBound.ToString());

            return builder.ToString();
        }


        public virtual void StartSinking()
        {
            isSinking = true;
        }

        public virtual void StopSinking()
        {
            isSinking = false;
        }

        /// <summary>
        /// Toniêcie tile'a. Zwraca o ile tile zaton¹³, lub 0 w przypadku zakoñczenia toniêcia
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        public virtual float Sink(float time, float timeUnit)
        {

            if (wreckTimeElapsed > wreckTime) //koniec czasu
            {
                StopSinking();
                isSunkDown = true;
                return 0;
            }

            

            float YVal = 0;
            //aktualizacja movmentVector.Y
            YVal = (YVal >= 0)
                                   ? SinkingSpeed
                                   : System.Math.Min(YVal + waterYBreakingPower, -SinkingSpeed);
            YVal = YVal * (time / timeUnit);
            depth += YVal;
            yBegin -= YVal;
            yEnd -= YVal;
            foreach (Quadrangle q in this.ColisionRectangles)
            {
                q.Move(0, -YVal);
            }
            this.HitBound.Move(0, -YVal);

            wreckTimeElapsed += time;

            return YVal;

        }


        /// <summary>
        /// Funkcja sprawdza czy dany obiekt jest w kolizji
        /// z innym prostokatem:bomba, rakieta, etc
        /// </summary>
        /// <param name="quad">Prostokat z ktorym sprawdzamy kolizje.</param>
        /// <returns>Jesli kolizja wystapila choc z jednym elementem zwraca true;
        /// w przeciwnym przypadku zwraca false.</returns>
        public virtual bool InCollision(Quadrangle quad)
        {
            return (quad != null && hitBound != null && hitBound.Intersects(quad));      
        }

        /// <summary>
        /// Funkcja sprawdza czy dany obiekt jest w kolizji z innym obiektem.
        /// </summary>
        /// <param name="center">Punkt srodkowy obiektu z ktorym sprawdzamy kolizje.</param>
        /// <returns>Jesli punkt srodkowy obiektu jest ponizej linni powierzchni - kolizja nastapila.</returns>
        public virtual bool InCollision(PointD center)
        {
            return ((this.YBegin + this.YEnd) / 2.0f) >= center.Y;
        }

        #endregion

        #region IBoundingBoxes Members

        public List<Quadrangle> BoundingQuadrangles
        {
            get
            {
                List<Quadrangle> result = new List<Quadrangle>();
                if (collisionRectangles != null && collisionRectangles.Count > 0)
                {
                    result.AddRange(collisionRectangles);
                }
                result.Add(hitBound);
                return result;
            }
        }

        public string Name
        {
            get { return "Tile" + GetHashCode(); }
        }

        #endregion
    }
}