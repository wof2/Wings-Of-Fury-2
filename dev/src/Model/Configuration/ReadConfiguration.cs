/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
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
using System.Reflection;
using System.Text;
using System.Xml;
using Wof.Controller;
using Wof.Model.Exceptions;
using Mogre;

namespace Wof.Model.Configuration
{
    /// <summary>
    /// Wczytuje konfiguracje elementow gry
    /// </summary>
    public class ReadConfiguration : IDisposable
    {
        #region Private Fields

        /// <summary>
        /// Ustawienia dla samolotu gracza.
        /// </summary>
        private Dictionary<String, float> mP47PlaneConfig = null;
        private Dictionary<String, float> mF4UPlaneConfig = null;
        private Dictionary<String, float> mB25PlaneConfig = null;

        
        private Dictionary<String, float> mEnemyPlaneConfig = null;
        private Dictionary<String, float> mSoldierConfig = null;
        private Dictionary<String, float> mWoodenBunker = null;
        private Dictionary<String, float> mConcreteBunker = null;
        private Dictionary<String, float> mShipWoodenBunker = null;
        private Dictionary<String, float> mShipConcreteBunker = null;
        private Dictionary<String, float> mBomb = null;
        private Dictionary<String, float> mRocket = null;
        private Dictionary<String, float> mTorpedo = null;

        /// <summary>
        /// Ustawienia dla efektow
        /// </summary>
        private Dictionary<String, float> mEffects = null;        

        private NumberFormatInfo format = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Wczytuje konfiguracje z podanego pliku.
        /// </summary>
        /// <param name="path">Sciezka do pliku zawierajacego konfiguracje lementow gry.</param>
        /// <param name="isCryptData">Czy dane sa zaszyfrowane.</param>
        public ReadConfiguration(string path, bool isCryptData)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(String.Format("Podana scierzka: {0} nie istnieje !", path), "path");
            if (!File.Exists(path))
                throw new ConfigurationFileNotFoundException(Path.GetFileName(path));

            Init();
            if (isCryptData)
                ReadFile(XmlTextReader.Create(new StringReader(RijndaelSimple.Decrypt(File.ReadAllText(path)))));
            else
                ReadFile(XmlReader.Create(new XmlTextReader(path), GetSettings()));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metoda zwraca konfiguracje dla podanego typu elementu gry.
        /// </summary>
        /// <param name="type">Typ elementu gry.</param>
        /// <returns>Slownik zawierajacy konfiguracje dla podanego elementu gry.</returns>
        public Dictionary<String, float> GetConfiguration(String type)
        {
            
            if (type.Equals(ConfigurationNames.P47))
                return mP47PlaneConfig;
            else if (type.Equals(ConfigurationNames.F4U))
                return mF4UPlaneConfig;
            else if (type.Equals(ConfigurationNames.B25))
                return mB25PlaneConfig;
            else if (type.Equals(ConfigurationNames.EnemyPlane))
                return mEnemyPlaneConfig;
            else if (type.Equals(ConfigurationNames.Soldier))
                return mSoldierConfig;
            else if (type.Equals(ConfigurationNames.WoodenBunker))
                return mWoodenBunker;
            else if (type.Equals(ConfigurationNames.ConcreteBunker))
                return mConcreteBunker;
            else if (type.Equals(ConfigurationNames.ShipConcreteBunker))
                return mShipConcreteBunker;
            else if (type.Equals(ConfigurationNames.ShipWoodenBunker))
                return mShipWoodenBunker;
            else if (type.Equals(ConfigurationNames.Bomb))
                return mBomb;
            else if (type.Equals(ConfigurationNames.Rocket))
                return mRocket;
            else if (type.Equals(ConfigurationNames.Torpedo))
                return mTorpedo;
            else if (type.Equals(ConfigurationNames.Effects))
                return mEffects;
            return null;
        }

        /// <summary>
        /// Inicjujej zmienne wartosciami domyslnymi.
        /// </summary>
        private void Init()
        {
            format = new NumberFormatInfo();
            format.NumberGroupSeparator = ",";

            mEnemyPlaneConfig = InitConfig(typeof (ConfigurationAttributes.EnemyPlane));
           
            mP47PlaneConfig = InitConfig(typeof(ConfigurationAttributes.UserPlane));
            mF4UPlaneConfig = InitConfig(typeof(ConfigurationAttributes.UserPlane));
            mB25PlaneConfig = InitConfig(typeof(ConfigurationAttributes.UserPlane));

            mConcreteBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mSoldierConfig = InitConfig(typeof (ConfigurationAttributes.Soldier));
            mWoodenBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mShipConcreteBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mShipWoodenBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mRocket = InitConfig(typeof (ConfigurationAttributes.Rocket));
            mTorpedo = InitConfig(typeof(ConfigurationAttributes.Torpedo));
            mBomb = InitConfig(typeof (ConfigurationAttributes.Bomb));
            mEffects = InitConfig(typeof(ConfigurationAttributes.Effects));
        }

        /// <summary>
        /// Inicjuje slownik konfiguracji wartosciami domyslnymi.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<String, float> InitConfig(Type type)
        {
            FieldInfo[] fields = type.GetFields();
            if (fields != null && fields.Length > 0)
            {
                Dictionary<String, float> config = new Dictionary<String, float>();
                foreach (FieldInfo fi in fields)
                    config.Add(fi.Name, -1);
                return config;
            }
            return new Dictionary<String, float>();
        }

        private bool ReadFile(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals(ConfigurationNames.P47))
                    {
                        if (!ReadConfig(reader, mP47PlaneConfig))
                            throw new XmlException("P47 jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.F4U))
                    {
                        if (!ReadConfig(reader, mF4UPlaneConfig))
                            throw new XmlException("F4U jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.B25))
                    {
                        if (!ReadConfig(reader, mB25PlaneConfig))
                            throw new XmlException("B25 jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.EnemyPlane))
                    {
                        if (!ReadConfig(reader, mEnemyPlaneConfig))
                            throw new XmlException("Enemy plane jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.Soldier))
                    {
                        if (!ReadConfig(reader, mSoldierConfig))
                            throw new XmlException("Soldier jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.ConcreteBunker))
                    {
                        if (!ReadConfig(reader, mConcreteBunker))
                            throw new XmlException("Concrete bunker jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.WoodenBunker))
                    {
                        if (!ReadConfig(reader, mWoodenBunker))
                            throw new XmlException("Wooden bunker jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.ShipConcreteBunker))
                    {
                        if (!ReadConfig(reader, mShipConcreteBunker))
                            throw new XmlException("ship Concrete bunker jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.ShipWoodenBunker))
                    {
                        if (!ReadConfig(reader, mShipWoodenBunker))
                            throw new XmlException("ship Wooden bunker jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.Rocket))
                    {
                        if (!ReadConfig(reader, mRocket))
                            throw new XmlException("Rocket jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.Torpedo))
                    {
                        if (!ReadConfig(reader, mTorpedo))
                            throw new XmlException("Torpedo jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.Bomb))
                    {
                        if (!ReadConfig(reader, mBomb))
                            throw new XmlException("Bomb jest niepoprawny !");
                    }
                    else if (reader.Name.Equals(ConfigurationNames.Effects))
                    {
                        if (!ReadConfig(reader, mEffects))
                            throw new XmlException("Effects jest niepoprawny !");
                    }
                }
            }
            return true;
        }

        private bool ReadConfig(XmlReader reader, Dictionary<String, float> config)
        {
            float value = -1;
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (config.ContainsKey(reader.Name.Trim()))
                    {
                        if (float.TryParse(reader.Value.Trim(), NumberStyles.Any, format, out value))
                            config[reader.Name.Trim()] = value;
                        else return false;
                    }
                   // else return false;
                }
                return true;
            }
            return false;
        }

        //private float Safe

        private XmlReaderSettings GetSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            return settings;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("User plane:");
            builder.Append(ConfigToString(mP47PlaneConfig));
            builder.AppendLine("Enemy plane:");
            builder.Append(ConfigToString(mEnemyPlaneConfig));
            builder.AppendLine("Soldier:");
            builder.Append(ConfigToString(mSoldierConfig));
            builder.AppendLine("Wooden bunker:");
            builder.Append(ConfigToString(mWoodenBunker));
            builder.AppendLine("Concrete bunker:");
            builder.Append(ConfigToString(mConcreteBunker));
            builder.AppendLine("Ship Wooden bunker:");
            builder.Append(ConfigToString(mShipWoodenBunker));
            builder.AppendLine("Ship Concrete bunker:");
            builder.Append(ConfigToString(mShipConcreteBunker));
            builder.AppendLine("Rocket:");
            builder.Append(ConfigToString(mRocket));
            builder.AppendLine("Torpedo:");
            builder.Append(ConfigToString(mTorpedo));
            builder.AppendLine("Bomb:");
            builder.Append(ConfigToString(mBomb));
            builder.AppendLine("Effects:");
            builder.Append(ConfigToString(mEffects));
            return builder.ToString();
        }

        private string ConfigToString(Dictionary<String, float> config)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<String, float> c in config)
                builder.AppendLine(String.Format("  {0} = {1};", c.Key, c.Value.ToString()));
            return builder.ToString();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (mConcreteBunker != null)
            {
                mConcreteBunker.Clear();
                mConcreteBunker = null;
            }
            if (mWoodenBunker != null)
            {
                mWoodenBunker.Clear();
                mWoodenBunker = null;
            }
 			if (mShipConcreteBunker != null)
            {
                mShipConcreteBunker.Clear();
                mShipConcreteBunker = null;
            }
            if (mShipWoodenBunker != null)
            {
                mShipWoodenBunker.Clear();
                mShipWoodenBunker = null;
            }

            
            if (mEnemyPlaneConfig != null)
            {
                mEnemyPlaneConfig.Clear();
                mEnemyPlaneConfig = null;
            }
            if (mSoldierConfig != null)
            {
                mSoldierConfig.Clear();
                mSoldierConfig = null;
            }
            if (mP47PlaneConfig != null)
            {
                mP47PlaneConfig.Clear();
                mP47PlaneConfig = null;
            }
            
            if (mRocket != null)
            {
                mRocket.Clear();
                mRocket = null;
            }

            if (mTorpedo != null)
            {
                mTorpedo.Clear();
                mTorpedo = null;
            }

            if (mBomb != null)
            {
                mBomb.Clear();
                mBomb = null;
            }
        }

        #endregion
    }
}