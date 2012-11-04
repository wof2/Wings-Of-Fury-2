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
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

using Wof.Languages;
using Wof.Misc;
using Wof.Model.Level;
using Wof.Model.Level.XmlParser;

namespace Wof.Controller.Screens
{
    public class LoadGameUtil
    {    	
    	private CompletedLevelsInfo completedLevelsInfo;    
    	
		public CompletedLevelsInfo CompletedLevelsInfo {
			get { return completedLevelsInfo; }
		}
    	
    	public List<Achievement> GetCompletedAchievementsForLevel(LevelInfo levelInfo) {
    		if(completedLevelsInfo != null && completedLevelsInfo.CompletedLevels != null && completedLevelsInfo.CompletedLevels.ContainsKey(levelInfo)) {
    		    return	completedLevelsInfo.CompletedLevels[levelInfo];
            }
            return null;
    		
    	}
    	
    	protected void UpdateCompletedLevelsInfo(CompletedLevelsInfo completedLevelsInfo) {
    		this.completedLevelsInfo = completedLevelsInfo;
    	}
    	
    	private LoadGameUtil()
    	{
    		this.completedLevelsInfo = GetCompletedLevels();
    		
    	}    	
    	
        private static readonly LoadGameUtil singleton = new LoadGameUtil();

        public static LoadGameUtil Singleton
        {
            get { return singleton; }
        }


        public static List<object> GetAllPossibleLevelsFull()
        {
            List<object> ret = new List<object>();
            List<uint> levels = GetAllPossibleLevels();
            foreach (uint u in levels)
            {    
            	ret.Add(new LevelInfo(XmlLevelParser.GetLevelFileName((uint)u), false));        
            }
            return ret;
        }

        private static List<uint> GetAllPossibleLevels()
        {
            List<uint> completedLevels = new List<uint>();

            uint i = 0;
            while (File.Exists(XmlLevelParser.GetLevelFileName(++i)))
            {
                completedLevels.Add((uint)i);
            }
            return completedLevels;
        }


       

        public static List<object> GetCustomLevels()
        {
            try
            {
            	string[] files = Directory.GetFiles(LevelInfo.C_CUSTOM_LEVELS_DIR, "*" + XmlLevelParser.C_LEVEL_POSTFIX);
            	List<object> ret = new List<object>();
            	foreach(string filename in files)
            	{	
            		ret.Add(new LevelInfo(filename, true));            		
            	}
                return ret;
            }
            catch (Exception)
            {                
                return new List<object>();
            }
        }

        public static List<object> GetCompletedLevelsFull()
        {
            List<object> ret = new List<object>();
            Dictionary<LevelInfo, List<Achievement>>  completedLevels = Singleton.CompletedLevelsInfo.CompletedLevels;
            foreach (LevelInfo info in completedLevels.Keys)
            {
            	ret.Add(info);
            	
              //  ret.Add( LanguageResources.GetString(LanguageKey.Level) + u);
            }
            return ret;
        }
        
        public static Stream GenerateStreamFromString(string s)
		{
		    MemoryStream stream = new MemoryStream();
		    StreamWriter writer = new StreamWriter(stream);
		    writer.Write(s);
		    writer.Flush();
		    stream.Position = 0;
		    return stream;
		}

        public static CompletedLevelsInfo GetCompletedLevels()
        {
            CompletedLevelsInfo  completedLevels = new CompletedLevelsInfo ();
			List<Achievement> emptyAchievements = new List<Achievement>();
            if (!File.Exists(LoadGameScreen.C_COMPLETED_LEVELS_FILE))
            {
            	completedLevels = CompletedLevelsInfo.GetDefaultCompletedLevelsInfo();            	
            	return completedLevels;          
            }
            else
            {
                try
                {
                    string levelsRaw = File.ReadAllText(LoadGameScreen.C_COMPLETED_LEVELS_FILE);                   
                    string levelsString = RijndaelSimple.Decrypt(levelsRaw);
                    
                	using (Stream stream = GenerateStreamFromString(levelsString))
					{
					     XmlSerializer serializer = new XmlSerializer(typeof(CompletedLevelsInfo));
					     ;
					    var levels = ( CompletedLevelsInfo)serializer.Deserialize(stream);					    
					    return (levels as CompletedLevelsInfo);
					}                  
                  
                }
                catch (Exception ex)
                {
                	completedLevels.CompletedLevels.Clear();
                	completedLevels.CompletedLevels.Add(new LevelInfo(XmlLevelParser.GetLevelFileName(1), false), emptyAchievements);
                    Console.WriteLine(ex);
                }
                                
                return completedLevels;
            }
        }

        public static CompletedLevelsInfo NewLevelCompleted(LevelInfo levelInfo, List<Achievement> achievements)
        {
        	
            CompletedLevelsInfo completedLevels = Singleton.CompletedLevelsInfo;         
            completedLevels.CompletedLevels[levelInfo] = achievements; 
            
                            
            MemoryStream stream = new MemoryStream();	
		    XmlSerializer serializer = new XmlSerializer(typeof(CompletedLevelsInfo));
            serializer.Serialize(stream, completedLevels);
            
            stream.Position = 0;
	        var sr = new StreamReader(stream);
	        var levelsRaw = sr.ReadToEnd();
	        stream.Close();
	      
            File.WriteAllText(LoadGameScreen.C_COMPLETED_LEVELS_FILE, RijndaelSimple.Encrypt(levelsRaw));
            
            Singleton.UpdateCompletedLevelsInfo(completedLevels);
            return completedLevels;
        }
    }
}