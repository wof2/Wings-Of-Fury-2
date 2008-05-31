using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Wof.Languages
{
    internal static class XmlLanguageManager
    {
        #region Fields

        private const string LanguagesFolder = @"languages";
        private static Dictionary<string, StringDictionary> mLanguages;

        #endregion

        #region Methods

        private static string[] GetAvailableXmlFile()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                path = Path.Combine(path, LanguagesFolder);
                if (Directory.Exists(path))
                {
                    string[] langFiles = Directory.GetFiles(path, "language.*-*.xml");
                    return langFiles;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static void SetLanguages()
        {
            string[] languages = GetAvailableXmlFile();
            if (languages != null && languages.Length > 0)
            {
                mLanguages = new Dictionary<string, StringDictionary>();
                foreach (string path in languages)
                {
                    if (File.Exists(path))
                    {
                        XmlLanguage oneLang = new XmlLanguage(path);
                        if (oneLang.Translation != null)
                        {
                            if (oneLang.Translation.ContainsKey(LanguageKey.LanguageID))
                            {
                                string langId = GetCultureFromPath(path);
                                if (!String.IsNullOrEmpty(langId) && !mLanguages.ContainsKey(langId))
                                    mLanguages.Add(langId, oneLang.Translation);
                            }
                        }
                    }
                }
            }
        }

        private static string GetCultureFromPath(string path)
        {
            string file = Path.GetFileName(path);
            return file.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zwraca slownik z dostepnymi jezykami.
        /// </summary>
        public static Dictionary<string, StringDictionary> Languages
        {
            get
            {
                if (mLanguages == null)
                    SetLanguages();
                return mLanguages;
            }
        }

        /// <summary>
        /// Zwraca liste dostepnych jezykow.
        /// </summary>
        public static Dictionary<string, string> AvailableLanguages
        {
            get
            {
                if (mLanguages == null)
                    SetLanguages();
                if (mLanguages != null && mLanguages.Count > 0)
                {
                    Dictionary<String, String> availableLangs = new Dictionary<String, String>();
                    foreach (KeyValuePair<String, StringDictionary> lang in mLanguages)
                    {
                        if (lang.Value.ContainsKey(LanguageKey.LanguageID))
                        {
                            string langName = lang.Value[LanguageKey.LanguageID];
                            availableLangs.Add(langName, lang.Key);
                        }
                    }
                    return availableLangs;
                }
                return null;
            }
        }

        #endregion
    }
}