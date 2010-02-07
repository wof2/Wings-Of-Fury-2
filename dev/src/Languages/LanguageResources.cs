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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Wof.Controller;
using Wof.Properties;

namespace Wof.Languages
{
    public static class LanguageResources
    {
        #region Fields

        private static readonly Dictionary<string, string> mAvailableLanguages = new Dictionary<string, string>();
        private static readonly Object mlockResourceManager = typeof (LanguageResources);

        private static StringDictionary mActualLanguage;

        #endregion

        #region Static Constructor

        static LanguageResources()
        {
            //AvailableLanguages();
        }

        #endregion

        #region Properties

        private static StringDictionary XMLManager
        {
            get
            {
                lock (mlockResourceManager)
                {
                    if (mActualLanguage == null)
                    {
                       
                        if (!XmlLanguageManager.Languages.ContainsKey(Settings.Default.Language))
                        {
                            Settings.Default.Language = LanguageManager.CultureType.English;
                            Settings.Default.Save();
                        }

                        FontManager.SetCurrentFont(Settings.Default.Language);
                        mActualLanguage = XmlLanguageManager.Languages[Settings.Default.Language];

                        UpdateFontSize(Settings.Default.Language);
                            
                    }

                 

                    return mActualLanguage;
                }
            }
            set
            {
                if (value == null)
                    mActualLanguage = null;
            }
        }

        public static string Language
        {
            get { return Settings.Default.Language; }
            set { SetLanguage(value); }
        }

        #endregion

        #region Methods

        public static string GetString(string name)
        {
            /*Pobiera jezyki z resourcow.
            if (Manager == null)
                return String.Format(CultureInfo.InvariantCulture, "Translation error: {0}", name);

            return Manager.GetString(name);*/
            //pobiera jezyki z XML.
            if (XMLManager == null)
                return String.Empty;
            if (XMLManager.ContainsKey(name))
                return XMLManager[name];
            return String.Empty;
        }

        private static void GetAvailableLanguages()
        {
            mAvailableLanguages.Clear();
            string fileName = String.Format(CultureInfo.InvariantCulture,
                                            "{0}.resources.dll", @"Wof");
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            foreach (DirectoryInfo subdir in dir.GetDirectories("*-*"))
            {
                string assPath = Path.Combine(subdir.FullName, fileName);
                if (File.Exists(assPath))
                {
                    ResourceManager rm = null;
                    try
                    {
                        CultureInfo ci = new CultureInfo(subdir.Name);
                        Assembly assLoad = Assembly.GetEntryAssembly().GetSatelliteAssembly(ci);
                        rm = new ResourceManager("language." + subdir.Name, assLoad);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    if (rm == null)
                        continue;
                    string langName = rm.GetString(LanguageKey.LanguageID);
                    mAvailableLanguages.Add(subdir.Name, langName);
                }
            }
        }

        /*private static void SetupLanguage()
        {
            try
            {
                if (!mAvailableLanguages.ContainsKey(mLanguage))
                {
                    string[] langs = new string[mAvailableLanguages.Keys.Count];
                    mAvailableLanguages.Keys.CopyTo(langs, 0);
                    mLanguage = langs[0];
                }
                //Setup culture info
                mCultureInfo = new CultureInfo(mLanguage);
                if (mCultureInfo != null)
                {
                    Thread.CurrentThread.CurrentCulture = mCultureInfo;
                    //Setup resource manager
                    Assembly asm = Assembly.GetEntryAssembly();
                    if (asm != null)
                    {
                        Assembly sasm = asm.GetSatelliteAssembly(mCultureInfo);
                        mResourceManager = new WofResourceManager("language." + mLanguage, sasm);
                    }
                }
            }
            catch (Exception expt)
            {
                Debug.WriteLine(expt.ToString());
                MessageBoxOptions localizedOptions = (MessageBoxOptions)0;
                if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
                    localizedOptions = MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;

                MessageBox.Show(Application.OpenForms[0],
                                "Cannot setup language. Please reinstall application." + Environment.NewLine + expt.ToString(),
                                "Fatal Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                localizedOptions);
                Environment.Exit(1);
            }
        }*/


        private  static void UpdateFontSize(string lang)
        {
            EngineConfig.CurrentFontSize = 0.035f;
            if (lang.Equals("en-GB"))
            {
                EngineConfig.CurrentFontSize *= 0.75f;
            }
            else
            if (lang.Equals("ru-RU"))
            {
                EngineConfig.CurrentFontSize *= 1.1f;
            }
            else
            if (lang.Equals("ua-UA"))
            {
                EngineConfig.CurrentFontSize *= 1.1f;
            } 
        }
        private static void SetLanguage(string lang)
        {
            if (String.IsNullOrEmpty(lang))
                return;

            if (Settings.Default.Language.Equals(lang))
                return;

            lock (mlockResourceManager)
            {
                XMLManager = null;
                UpdateFontSize(lang);
                //SetupLanguage();
            }
        }

        #endregion
    }
}