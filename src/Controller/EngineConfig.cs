/*
 * Copyright 2008 Adam Witczak, Jakub T�ycki, Kamil S�awi�ski, Tomasz Bilski, Emil Hornung, Micha� Ziober
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
using System.Globalization;
using System.IO;
using FSLOgreCS;
using Wof.Languages;
using Wof.Properties;

namespace Wof.Controller
{
    /// <summary>
    /// Statyczne ustawienia gry
    /// <author>Adam Witczak</author>
    /// </summary>
    public class EngineConfig
    {
        /// <summary>
        /// Wersja tej kompilacji WOfa. Powinna si� by� w formacie X.XX
        /// </summary>
        public static readonly String C_WOF_VERSION = "2.00";

        /// <summary>
        /// Czy bie��ca kompilacja jest demem?
        /// </summary>
        public static readonly bool C_IS_DEMO = false;


        public static readonly String C_ENGINE_CONFIG = "wofconf.dat";


        public static readonly bool UseLastHardwareSettings = false;

        
        public static readonly bool DisplayAxes = false;

        public static readonly bool DisplayBoundingQuadrangles = false;

        public static readonly bool AutoEncodeXMLs = true;

        /// <summary>
        /// Ustawiane tylko przy ladowaniu zmiennych z argv (-FreeLook). Punkt odniesienia
        /// </summary>
        public static bool FreeLook = false;

        public static bool AttachCameraToPlayerPlane = true;
        public static bool ManualCamera = false;

        public static bool BloomEnabled = false;


        // TO DO
        public static bool Gore = false;
        public static bool MinimapNoseCamera = false;

        public static readonly bool StaticGeometryIslands = true; // wysypy u�ywaj� geometri statycznej do renderownia (untested)
        public static bool LowDetails = false; // niskie detale obiektow, mniej efektow (nadpisywane przez wofconf.dat)
        public static bool InverseKeys = false; // czy przyciski UP / DOWN s� zamienione? (nadpisywane przez wofconf.dat)
        public static bool SpinKeys = false; // Nie zapisywane do Wofconf.dat , czy trzeba chwilowo odwr�cic przyciski podczas spinu
        public static bool ShowIntro = true; // czy ma by� odgrywane intro? (nadpisywane przez wofconf.dat)
        public static bool DisplayMinimap = true; // czy pokazywa� minimape? (nadpisywane przez wofconf.dat)

        public static string Language = "en-GB";

        public enum DifficultyLevel
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        } ;

        public enum JoystickButtons
        {
            Gun = 4,
            Enter = 4,
            Gear = 3,
            Rocket = 1,
            Camera = 6,
            Engine = 2,
            Escape = 5

        } ;

        public static DifficultyLevel Difficulty = DifficultyLevel.Easy; 

        public static bool SoundEnabled = true;
        public static FreeSL.FSL_SOUND_SYSTEM SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND;
        public static int MusicVolume = 80;

        protected static int soundVolume = 100;
        public static int SoundVolume
        {
             set 
             { 
                 soundVolume = value;
             }

             get
             {
                 return soundVolume;
             }
        }

        /// <summary>
        /// Warto�� przeciwna do LowDetails
        /// </summary>
        public static bool ExplosionLights = true; // rozb�yski �wiat�a przy wybuchach bomb

        /// <summary>
        /// Warto�� przeciwna do LowDetails
        /// </summary>
        public static bool BodiesStay = true;

        /// <summary>
        /// Warto�� przeciwna do LowDetails
        /// </summary>
        public static bool Shadows = true;

        /// <summary>
        /// Pokazuje dodatkowe informacje w trakcie gry
        /// </summary>
        public static bool DebugInfo = false;

        /// <summary>
        /// Szybki start z pomini�ciem intro oraz menu
        /// </summary>
        public static bool DebugStart = false;

        /// <summary>
        /// Poziom (level) uruchamiany przy trybie DebugStart
        /// </summary>
        public static int DebugStartLevel = 1;


        public static readonly int C_LOADING_DELAY = 3500;

        /// <summary>
        /// Bazowa wysoko�� czcionki wyra�ona w procentowej wysoko�ci wzgl�dem ekranu. Wykorzystywane przez AbstractScreen
        /// </summary>
        public static readonly float C_FONT_SIZE = 0.035f;


        public static void LoadEngineConfig()
        {
            try
            {
                if (File.Exists(C_ENGINE_CONFIG))
                {
                    String[] configOptions = File.ReadAllLines(C_ENGINE_CONFIG);

                    BloomEnabled = "true".Equals(configOptions[0]);
                    SoundEnabled = "true".Equals(configOptions[1]);
                    try
                    {
                        SoundSystem =
                            (FreeSL.FSL_SOUND_SYSTEM) Enum.Parse(typeof (FreeSL.FSL_SOUND_SYSTEM), configOptions[2]);
                    }
                    catch (Exception)
                    {
                        SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND;
                    }
                    try
                    {
                        SoundVolume = int.Parse(configOptions[3]);
                    }
                    catch (Exception)
                    {
                        SoundVolume = 100;
                    }

                    try
                    {
                        MusicVolume = int.Parse(configOptions[4]);
                    }
                    catch (Exception)
                    {
                        MusicVolume = 100;
                    }

                    LowDetails = "true".Equals(configOptions[5]);
                    InverseKeys = "true".Equals(configOptions[6]);
                    ExplosionLights = !LowDetails;
                    BodiesStay = !LowDetails;
                    Shadows = !LowDetails;

                    try
                    {
                        LanguageManager.SetLanguage(configOptions[7]);
                    }
                    catch (Exception)
                    {
                        LanguageManager.SetLanguage("en-GB");
                    }

                    //Difficulty
                    switch (configOptions[8])
                    {
                        case "Easy":
                            Difficulty = DifficultyLevel.Easy;
                            break;
                        case "Medium":
                            Difficulty = DifficultyLevel.Medium;
                            break;
                        case "Hard":
                            Difficulty = DifficultyLevel.Hard;
                            break;
                    }
                    ShowIntro = "true".Equals(configOptions[9]);
                    DisplayMinimap = "true".Equals(configOptions[10]);
                    
                }
                else
                {
                    SaveEngineConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SaveEngineConfig()
        {
            String[] configuration = new String[11];
            configuration[0] = BloomEnabled ? "true" : "false";
            configuration[1] = SoundEnabled ? "true" : "false";
            configuration[2] = SoundSystem.ToString();
            configuration[3] = SoundVolume.ToString();
            configuration[4] = MusicVolume.ToString();
            configuration[5] = LowDetails ? "true" : "false";
            configuration[6] = InverseKeys ? "true" : "false";
            configuration[7] = Settings.Default.Language;
            configuration[8] = Difficulty.ToString();
            configuration[9] = ShowIntro ? "true" : "false";
            configuration[10] = DisplayMinimap ? "true" : "false";
            ExplosionLights = !LowDetails;
            BodiesStay = !LowDetails;
            Shadows = !LowDetails;

            //File.Create(C_ENGINE_CONFIG);
            File.WriteAllLines(C_ENGINE_CONFIG, configuration);
        }
    }
}