using System;
using System.Collections.Specialized;
using System.Xml;

namespace Wof.Languages
{
    internal class XmlLanguage
    {
        #region Fields

        private StringDictionary mLanguages;

        #endregion

        #region Constructors

        public XmlLanguage(string path)
        {
            XmlReader reader = null;
            try
            {
                mLanguages = new StringDictionary();
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                readerSettings.IgnoreWhitespace = true;
                reader = XmlReader.Create(path, readerSettings);
                Read(reader);
            }
            catch
            {
                mLanguages = null;
            }
            finally
            {
                if (reader != null && reader.ReadState != ReadState.Closed)
                    reader.Close();
            }
        }

        #endregion

        #region Methods

        private void Read(XmlReader reader)
        {
            string key = String.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                {
                    reader.MoveToAttribute("name");
                    key = reader.Value;
                    if (String.IsNullOrEmpty(key))
                        throw new Exception("Wrong key!");
                    reader.Read();
                    reader.Read();
                    mLanguages.Add(key, reader.Value);
                }
            }
        }

        #endregion

        #region Properties

        public StringDictionary Translation
        {
            get { return mLanguages; }
        }

        #endregion
    }
}