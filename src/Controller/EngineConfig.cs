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
using System.Globalization;
using System.IO;
using FSLOgreCS;
using Wof.Languages;
using Wof.Model.Level.Planes;
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
        /// Wersja tej kompilacji WOfa. Powinna byæ w formacie X.XX
        /// </summary>
        public static readonly String C_WOF_VERSION = "1.00";

        public static readonly bool C_IS_INTERNAL_TEST = true;
        public static readonly String C_IS_INTERNAL_TEST_INFO = "!!! Internal test version !!! ";

 		public static readonly String C_GAME_NAME = "Wings Of Fury 2: Return of the legend";
 		
        /// <summary>
        /// Czy bie¿¹ca kompilacja jest demem?
        /// </summary>
        public static readonly bool C_IS_DEMO = false;

        public static readonly bool IsEnhancedVersion = Licensing.IsEhnancedVersion();


        public static readonly String C_ENGINE_CONFIG = "wofconf.dat";
        public static readonly String C_OGRE_CFG = "ogre.cfg";

        public static readonly String C_WOF_HOME_PAGE = "http://www.wingsoffury2.com";
        public static readonly String C_WOF_NEWS_PAGE = "http://www.wingsoffury2.com/news.php";
        public static readonly String C_WOF_UPDATE_CHECK_PAGE = "http://www.wingsoffury2.com/update_chk.php";
    

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
       
       
        public const float GameSpeedMultiplierSlow = 0.5f;
 		public const float GameSpeedMultiplierNormal = 1.0f;

        
 		
        /// <summary>
        /// Do efektu bullet-time
        /// </summary>
        public static float CurrentGameSpeedMultiplier = GameSpeedMultiplierNormal;

   

        public static bool UseAsyncModel = false;
        public static bool UpdateHydraxEveryFrame = true;

        
        public static bool Gore = false;
        public static bool MinimapNoseCamera = false;

        public static readonly bool StaticGeometryIslands = true; // wysypy u¿ywaj¹ geometri statycznej do renderownia (untested)
        public static bool LowDetails = false; // niskie detale obiektow, mniej efektow (nadpisywane przez wofconf.dat)
        public static bool InverseKeys = false; // czy przyciski UP / DOWN s¹ zamienione? (nadpisywane przez wofconf.dat)
        public static bool SpinKeys = false; // Nie zapisywane do Wofconf.dat , czy trzeba chwilowo odwrócic przyciski podczas spinu
        public static bool ShowIntro = true; // czy ma byæ odgrywane intro? (nadpisywane przez wofconf.dat)

        public static PlaneType CurrentPlayerPlaneType;

        public static bool DisplayMinimap = true;


        private static bool displayingMinimap;
        public static bool DisplayingMinimap
        {
            set { displayingMinimap = value; }
            get { return displayingMinimap; }
        } // czy pokazywaæ minimape? (nadpisywane przez wofconf.dat)

      

 		public static bool UseHydrax = true; // czy korzystaæ z zaawansowanej symulacji wody? (nadpisywane przez wofconf.dat)

 		public static bool UseHardwareTexturePreloader = true; // czy wysylac do karty graficznej tesktury przed rozpoczeciem gry
 		
 		public static int HardwareTexturePreloaderTextureLimit;
        
 		public static bool AudioStreaming;
 		
        public static string Language = "en-GB";
      
        public enum DifficultyLevel
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        };

       
        
        
        
        public enum ShadowsQualityTypes
        {
        	None = 0,
        	Low  = 1,
        	Medium = 2,
        	High  = 3
        	
        }

        public static DifficultyLevel Difficulty = DifficultyLevel.Easy; 

        public static bool SoundEnabled = true;
        public static FreeSL.FSL_SOUND_SYSTEM SoundSystem = FreeSL.FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND;
        public static int MusicVolume = 40;

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
        /// Wartoœæ przeciwna do LowDetails
        /// </summary>
        public static bool ExplosionLights = true; // rozb³yski œwiat³a przy wybuchach bomb

        /// <summary>
        /// Wartoœæ przeciwna do LowDetails
        /// </summary>
        public static bool BodiesStay = true;

        /// <summary>
        /// Wartoœæ przeciwna do LowDetails
        /// </summary>
        public static ShadowsQualityTypes ShadowsQuality = ShadowsQualityTypes.Medium;

        /// <summary>
        /// Pokazuje dodatkowe informacje w trakcie gry
        /// </summary>
        public static bool DebugInfo = false;

        /// <summary>
        /// Szybki start z pominiêciem intro oraz menu
        /// </summary>
        public static bool DebugStart = false;

        /// <summary>
        /// Poziom (level) uruchamiany przy trybie DebugStart
        /// </summary>
        public static int DebugStartLevel = 1;


        public static readonly int C_LOADING_DELAY = 0; //3000;

        /// <summary>
        /// Bazowa wysokoœæ czcionki wyra¿ona w procentowej wysokoœci wzglêdem ekranu. Wykorzystywane przez AbstractScreen
        /// </summary>
        public static readonly float C_FONT_SIZE = 0.035f;


       



      
     /*   public static void SetDisplayMinimap(bool enabled)
        {
            EngineConfig.DisplayingMinimap = enabled;
        }*/
        public static void LoadEngineConfig()
        {
            try
            {
                if (File.Exists(C_ENGINE_CONFIG))
                {
                    String[] configOptions = File.ReadAllLines(C_ENGINE_CONFIG);

                    BloomEnabled = bool.Parse(configOptions[0]);
                    SoundEnabled = bool.Parse(configOptions[1]);
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
                        MusicVolume = 40;
                    }

                  
                    try
                    {
                        LowDetails = bool.Parse(configOptions[5]);
                    }
                    catch (Exception)
                    {
                        LowDetails = false;
                    }
                    ExplosionLights = !LowDetails;
                    BodiesStay = !LowDetails;

                    try
                    {
                        InverseKeys = bool.Parse(configOptions[6]);
                    }
                    catch (Exception)
                    {
                        InverseKeys = false;
                    }

                  
                    //Console.WriteLine(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

                    try
                    {
                        LanguageManager.SetLanguage(configOptions[7]);
                    }
                    catch (Exception)
                    {
                        LanguageManager.SetLanguage("en-GB");
                    }
					try
					{
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
					}
					catch(Exception)
					{
						Difficulty = DifficultyLevel.Easy;
					}
					
                    try
					{
                    	if(ShowIntro)
                    	{
                    		// intro mo¿na tylko wy³aczyæ. Chodzi o to zeby zapobiec w³¹czeniu intra jeœli uruchomiono program z -SkipIntro
                            ShowIntro = bool.Parse(configOptions[9]);
                    	}
                    	
                    }
                    catch(Exception)
                    {
                    	ShowIntro = true;
                    }
                     try
					{
                        DisplayMinimap = bool.Parse(configOptions[10]);
                    }
                    catch(Exception)
                    {
                    	DisplayMinimap = true;
                    }
                  //  DisplayingMinimap = DisplayMinimap;


                    try
					{
                    	UseHydrax = bool.Parse(configOptions[11]);
                    }
                    catch(Exception)
                    {
                    	UseHydrax = false;
                    }


                    
                //  UseHydrax = false;
                  
                    try
					{
						 //Difficulty
	                    switch (configOptions[12])
	                    {
	                        case "None":
	                            ShadowsQuality = ShadowsQualityTypes.None;
	                            break;
	                         case "Low":
	                            ShadowsQuality = ShadowsQualityTypes.Low;
	                            break;
	                        case "Medium":
	                             ShadowsQuality = ShadowsQualityTypes.Medium;
	                            break;
	                        case "High":
	                             ShadowsQuality = ShadowsQualityTypes.High;
	                            break;
	                    }
					}                    
					catch(Exception)
					{
						ShadowsQuality = ShadowsQualityTypes.Medium;
					}
					
					try
					{
                    	 AudioStreaming = bool.Parse(configOptions[13]);
                    }
                    catch(Exception)
                    {
                    	 AudioStreaming = false;
                    }
                    try
					{
                        UseHardwareTexturePreloader = bool.Parse(configOptions[14]);
                    }
                    catch(Exception)
                    {
                        UseHardwareTexturePreloader = false;
                    }

					try
					{
                        Gore = bool.Parse(configOptions[15]);
                    }
                    catch(Exception)
                    {
                        Gore = false;
                    }
                    
                    try
					{
                        HardwareTexturePreloaderTextureLimit = int.Parse(configOptions[16]);
                    }
                    catch(Exception)
                    {
                        HardwareTexturePreloaderTextureLimit = 64;
                    }


                    

                    try
                    {
                        UseAsyncModel = bool.Parse(configOptions[17]);
                    }
                    catch (Exception)
                    {
                        UseAsyncModel = false;
                    }
                    
                    try
                    {
                        UpdateHydraxEveryFrame = bool.Parse(configOptions[18]);
                    }
                    catch (Exception)
                    {
                        UpdateHydraxEveryFrame = true;
                    }


                    try
                    {

                        CurrentPlayerPlaneType =
                            (PlaneType)Enum.Parse(typeof(PlaneType), configOptions[19]);
                    }
                    catch (Exception)
                    {
                        CurrentPlayerPlaneType = PlaneType.P47;
                    }

                    if(!EngineConfig.IsEnhancedVersion)
                    {
                        CurrentPlayerPlaneType = PlaneType.P47;
                    }
								
                    
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
            String[] configuration = new String[20];
            configuration[0] = BloomEnabled.ToString();
            configuration[1] = SoundEnabled.ToString();
            configuration[2] = SoundSystem.ToString();
            configuration[3] = SoundVolume.ToString();
            configuration[4] = MusicVolume.ToString();
            configuration[5] = LowDetails.ToString();
            configuration[6] = InverseKeys.ToString();
            configuration[7] = Settings.Default.Language;
            configuration[8] = Difficulty.ToString();
            configuration[9] = ShowIntro.ToString();
            configuration[10] = DisplayMinimap.ToString();
            configuration[11] = UseHydrax.ToString();
            configuration[12] = ShadowsQuality.ToString();
            configuration[13] = AudioStreaming.ToString();
            configuration[14] = UseHardwareTexturePreloader.ToString();
            configuration[15] = Gore.ToString();
            configuration[16] = HardwareTexturePreloaderTextureLimit.ToString();
            configuration[17] = UseAsyncModel.ToString();
            configuration[18] = UpdateHydraxEveryFrame.ToString();
            configuration[19] = CurrentPlayerPlaneType.ToString();
            
             
            ExplosionLights = !LowDetails;
            BodiesStay = !LowDetails;
         
            //File.Create(C_ENGINE_CONFIG);
            File.WriteAllLines(C_ENGINE_CONFIG, configuration);
           
            string materialDir = "../../media/materials/scripts/ParentScripts/";
            if(ShadowsQuality >0 /*&& UseHydrax*/)
            {
            	File.Copy(materialDir+"0NormalMappedSpecular.base", materialDir+"0NormalMappedSpecular.material",true );
            } else
            {
            	File.Copy(materialDir+"0NormalMappedSpecularNoShadows.base", materialDir+"0NormalMappedSpecular.material",true );
            	
            }
           
        }
    }
}