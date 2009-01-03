using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Wof.Controller;
using Wof.Model.Exceptions;

namespace Wof.Model.Configuration
{
    public class ReadConfiguration : IDisposable
    {
        #region Private Fields

        private Dictionary<String, float> mUserPlaneConfig = null;
        private Dictionary<String, float> mEnemyPlaneConfig = null;
        private Dictionary<String, float> mSoldierConfig = null;
        private Dictionary<String, float> mWoodenBunker = null;
        private Dictionary<String, float> mConcreteBunker = null;
        private Dictionary<String, float> mBomb = null;
        private Dictionary<String, float> mRocket = null;
        private Dictionary<String, float> mTorpedo = null;


        

        private NumberFormatInfo format = null;

        #endregion

        #region Constructors

        public ReadConfiguration(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Strumien nie moze byc null-em!");
            Init();
            ReadFile(XmlReader.Create(stream, GetSettings()));
        }

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

        public Dictionary<String, float> GetConfiguration(String type)
        {
            if (type.Equals(ConfigurationNames.UserPlane))
                return mUserPlaneConfig;
            else if (type.Equals(ConfigurationNames.EnemyPlane))
                return mEnemyPlaneConfig;
            else if (type.Equals(ConfigurationNames.Soldier))
                return mSoldierConfig;
            else if (type.Equals(ConfigurationNames.WoodenBunker))
                return mWoodenBunker;
            else if (type.Equals(ConfigurationNames.ConcreteBunker))
                return mConcreteBunker;
            else if (type.Equals(ConfigurationNames.Bomb))
                return mBomb;
            else if (type.Equals(ConfigurationNames.Rocket))
                return mRocket;
            else if (type.Equals(ConfigurationNames.Torpedo))
                return mTorpedo;

            return null;
        }

        private void Init()
        {
            format = new NumberFormatInfo();
            format.NumberGroupSeparator = ",";

            mEnemyPlaneConfig = InitConfig(typeof (ConfigurationAttributes.EnemyPlane));
            mUserPlaneConfig = InitConfig(typeof (ConfigurationAttributes.UserPlane));
            mConcreteBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mSoldierConfig = InitConfig(typeof (ConfigurationAttributes.Soldier));
            mWoodenBunker = InitConfig(typeof (ConfigurationAttributes.Bunker));
            mRocket = InitConfig(typeof (ConfigurationAttributes.Rocket));
            mTorpedo = InitConfig(typeof(ConfigurationAttributes.Torpedo));


            
            mBomb = InitConfig(typeof (ConfigurationAttributes.Bomb));
        }

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
                    if (reader.Name.Equals(ConfigurationNames.UserPlane))
                    {
                        if (!ReadConfig(reader, mUserPlaneConfig))
                            throw new XmlException("User plane jest niepoprawny !");
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
                    else return false;
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
            builder.Append(ConfigToString(mUserPlaneConfig));
            builder.AppendLine("Enemy plane:");
            builder.Append(ConfigToString(mEnemyPlaneConfig));
            builder.AppendLine("Soldier:");
            builder.Append(ConfigToString(mSoldierConfig));
            builder.AppendLine("Wooden bunker:");
            builder.Append(ConfigToString(mWoodenBunker));
            builder.AppendLine("Concrete bunker:");
            builder.Append(ConfigToString(mConcreteBunker));
            builder.AppendLine("Rocket:");
            builder.Append(ConfigToString(mRocket));
            builder.AppendLine("Torpedo:");
            builder.Append(ConfigToString(mTorpedo));
            builder.AppendLine("Bomb:");
            builder.Append(ConfigToString(mBomb));
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
            if (mUserPlaneConfig != null)
            {
                mUserPlaneConfig.Clear();
                mUserPlaneConfig = null;
            }
            if (mWoodenBunker != null)
            {
                mWoodenBunker.Clear();
                mWoodenBunker = null;
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