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
using System.Globalization;
using System.IO;
using System.Xml;
using Wof.Controller;
using Wof.Model.Exceptions;
using Wof.Model.Level.Common;

namespace Wof.Model.Level.XmlParser
{
    public class TilesManager : IDisposable
    {
        /// <summary>
        /// Sciezka do katalogu z tileami gry (dat).
        /// </summary>
        private static string path = String.Empty;

        /// <summary>
        /// Sciezka do katalogu z tileami gry (xml).
        /// </summary>
        private static string rawPath = String.Empty;

        /// <summary>
        /// Slownik dla obiektow.
        /// </summary>
        private Dictionary<String, TilesNode> dictionary = null;

        /// <summary>
        /// Jesli isOk = true plik zostal wczyatany poprawnie.
        /// </summary>
        private bool isOkRead = false;

        private int concretebunkerCount = 0; //
        private int woodenbunkerCount = 0;   //
        private int terrainCount = 0;        // Bardzo zle rozwiazanie !!! - Poprawic.
        private int barrackCount = 0;        //
        private int barrelCount = 0;         //

        /// <summary>
        /// Publiczny konstruktor bezparametrowy.
        /// </summary>
        public TilesManager()
        {
            dictionary = new Dictionary<String, TilesNode>();
            Read();
        }

        /// <summary>
        /// Slownik obiektow.
        /// </summary>
        public Dictionary<String, TilesNode> Dictionary
        {
            get { return dictionary; }
        }

        /// <summary>
        /// Zwraca informacje o tym czy plik tiles.xml zostal
        /// poprawnie wczytany.
        /// </summary>
        public bool IsReadOK
        {
            get { return isOkRead; }
        }

        /// <summary>
        /// Zwraca sciezke do pliku tiles.dat.
        /// </summary>
        private static String TilesPath
        {
            get
            {
                if (String.IsNullOrEmpty(path))
                {
                    path = Directory.GetCurrentDirectory();
                    path = Path.Combine(path, @"levels");
                    path = Path.Combine(path, @"tiles.dat");
                }
                return path;
            }
        }

        /// <summary>
        /// Zwraca sciezke do pliku tiles.xml.
        /// </summary>
        private static String RAWTilesPath
        {
            get
            {
                if (String.IsNullOrEmpty(rawPath))
                {
                    rawPath = Directory.GetCurrentDirectory();
                    rawPath = Path.Combine(rawPath, @"levels");
                    rawPath = Path.Combine(rawPath, @"tiles.xml");
                }
                return rawPath;
            }
        }

        private void Read()
        { 
            // automatically reencode XML file to DAT file
            if (EngineConfig.AutoEncodeXMLs && File.Exists(RAWTilesPath))
            {
                File.WriteAllText(TilesPath,  RijndaelSimple.Encrypt(File.ReadAllText(RAWTilesPath)));
            }

            if (!File.Exists(TilesPath))
            {
                 throw new TilesFileNotFoundException(Path.GetFileName(TilesPath));
            }
               

            try
            {
                string contents = RijndaelSimple.Decrypt(File.ReadAllText(TilesPath));
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(new StringReader(contents), settings))
                {
                    NumberFormatInfo format = new NumberFormatInfo();
                    format.NumberGroupSeparator = ",";
                    while (reader.Read())
                    {
                        // Process only the elements
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name.Equals(Nodes.Tile))
                                ReadTile(reader, format);
                        }
                    }
                    reader.Close();
                }
                isOkRead = true;
            }
            catch
            {
                isOkRead = false;
            }
        }

        /// <summary>
        /// Wczytuje plik tiles.xml i na jego podstawie tworzy 
        /// slownik z obiektami potrzebny przy wczytywaniu plikow level-x.xml.
        /// </summary>
        /// <param name="reader"></param>
        private void ReadTile(XmlReader reader, NumberFormatInfo format)
        {
            //jesli element ma atrybuty.            
            TilesNode node = null;
            if (reader.HasAttributes)
            {
                node = new TilesNode();
                ReadAttributes(node, reader, format); 
            }

            //collision rectangle
            if (node != null && !String.IsNullOrEmpty(node.ID) && IsVariantionTiles(node.ID, node.Variation))
            {
                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes)
                        {
                            reader.MoveToAttribute(0);
                            node.CollisionRectangle.Add(StringToQuadrangle(reader.Value, format));
                        }
                    }
                }
            }

            //Dodaje do slownika
            if (node != null && node.IsValidName)
                if (!dictionary.ContainsKey(node.ID))
                    dictionary.Add(node.ID, node);

            reader.MoveToElement();
        }

        private bool IsVariantionTiles(String name, int variant)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            //szczegol dla pliku 6.12.2007
            if (name.Contains(Nodes.Terrain) && variant == 0)
                return false;

            if (name.Contains(Nodes.WoodenBunker))
                return true;
            else if (name.Contains(Nodes.ConcreteBunker))
                return true;
            else if (name.Contains(Nodes.Barrack))
                return true;
            else if (name.Contains(Nodes.Terrain))
                return true;
            else if (name.Contains(Nodes.Barrels))
                return true;

            return false;
        }

        private void ReadAttributes(TilesNode node, XmlReader reader, NumberFormatInfo format)
        {
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                //przechodze do i-tego atrybutu
                reader.MoveToAttribute(i);
                if (reader.Name.Equals(Attributes.Name))
                {
                    node.BaseName = reader.Value;
                  
                    if (reader.Value.Equals(Nodes.Terrain))
                    {
                        terrainCount++;
                    }
                    else if (reader.Value.Equals(Nodes.WoodenBunker))
                    {
                        woodenbunkerCount++;
                    }
                    else if (reader.Value.Equals(Nodes.ConcreteBunker))
                    {
                        concretebunkerCount++;
                    }
                    else if (reader.Value.Equals(Nodes.Barrack))
                    {
                        barrackCount++;
                    }
                    else if (reader.Value.Equals(Nodes.Barrels))
                    {
                        barrelCount++;
                    }
                   /* else if (reader.Value.Equals(Nodes.Ocean))
                    {
                        // wyj¹tek dla oceanu
                        node.ID = node.BaseName;
                        
                    }*/
                   
                }
                else if (reader.Name.Equals(Attributes.YStart))
                    node.YStart = float.Parse(reader.Value, format);
                else if (reader.Name.Equals(Attributes.YEnd))
                    node.YEnd = float.Parse(reader.Value, format);
                else if (reader.Name.Equals(Attributes.Variation))
                    node.Variation = int.Parse(reader.Value);
            }
            if(node.ID.Length ==0 ) node.GenerateID();
        }


        private Quadrangle StringToQuadrangle(string strRect, NumberFormatInfo format)
        {
            String[] splitRect = strRect.Split(new string[] {"]["},
                                               StringSplitOptions.RemoveEmptyEntries);
            if (splitRect.Length != 3)
                return null;
            splitRect[0] = splitRect[0].Replace("[", String.Empty);
            splitRect[2] = splitRect[2].Replace("]", String.Empty);
            String[] splitPos = splitRect[0].Split(new string[] {","},
                                                   StringSplitOptions.RemoveEmptyEntries);
            if (splitPos.Length != 2)
                return null;
            Quadrangle quad = null;
            try
            {
                PointD pointLD = new PointD();
                //Wspolrzedna x lewego gornego rogu 
                pointLD.X = float.Parse(splitPos[0].Trim(), format);
                //wpolrzedna y lewego gornego rogu
                float ltY = float.Parse(splitPos[1].Trim(), format);
                //szerokosc
                float width = float.Parse(splitRect[1].Trim(), format);
                //wysokosc
                float height = float.Parse(splitRect[2].Trim(), format);

                pointLD.Y = ltY - height;
                quad = new Quadrangle(pointLD, width, height);
            }
            catch
            {
                quad = null;
            }

            return quad;
        }

        private RectangleD StringToRectangle(string strRect, NumberFormatInfo format)
        {
            String[] splitRect = strRect.Split(new string[] {"]["},
                                               StringSplitOptions.RemoveEmptyEntries);
            if (splitRect.Length != 3)
                return null;
            splitRect[0] = splitRect[0].Replace("[", String.Empty);
            splitRect[2] = splitRect[2].Replace("]", String.Empty);
            String[] splitPos = splitRect[0].Split(new string[] {","},
                                                   StringSplitOptions.RemoveEmptyEntries);
            if (splitPos.Length != 2)
                return null;
            RectangleD returnRect = new RectangleD();
            try
            {
                returnRect.X = float.Parse(splitPos[0].Trim(), format);
                returnRect.Y = float.Parse(splitPos[1].Trim(), format);
                returnRect.Width = float.Parse(splitRect[1].Trim(), format);
                returnRect.Height = float.Parse(splitRect[2].Trim(), format);
            }
            catch
            {
                return null;
            }
            return returnRect;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (dictionary != null)
                dictionary.Clear();
            dictionary = null;
        }

        #endregion
    }
}