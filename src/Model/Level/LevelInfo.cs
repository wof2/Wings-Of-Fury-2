/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-11-02
 * Time: 23:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;
using Wof.Languages;
using Wof.Model.Level.XmlParser;

namespace Wof.Model.Level
{
	/// <summary>
	/// Description of LevelInfo_cs.
	/// </summary>
	[Serializable()]
	public class LevelInfo : IEquatable<LevelInfo>, IComparable<LevelInfo>
	{
		 public const string C_ENHANCED_LEVELS_DIR = "enhanced_levels/";
		 
		private string filename;
		
		public string Filename {
			get { return filename; }
			set { filename = value; }
		}
		private bool isCustom;
		
		public bool IsCustom {
			get { return isCustom; }
			set { isCustom = value; }
		}
		
		public LevelInfo(uint levelNo)
		{
			this.filename = XmlLevelParser.GetLevelFileName(levelNo);
			this.isCustom = false;
		}
		
		public LevelInfo(string filename,bool isCustom)
		{
			this.filename = filename;
			this.isCustom = isCustom;
		}
		
		public LevelInfo()
		{
		}
		
		public uint? GetLevelNo() {
			return GetLevelNo(Filename);		
		}
		
		private static uint? GetLevelNo(string filename) {
			uint? levelNo = null;
			try {
				Match match = Regex.Match(filename, Regex.Escape(XmlParser.XmlLevelParser.C_LEVEL_PREFIX)+ "([0-9]+)" +Regex.Escape(XmlParser.XmlLevelParser.C_LEVEL_POSTFIX)+"$", RegexOptions.IgnoreCase);
				if(match != null && match.Success){
					levelNo =  uint.Parse(match.Groups[1].Value);
				}	
			}
		    catch(Exception ex) {
		    	
		    }
			return levelNo;			
		}
	
		public override string ToString()
		{
			return GetDisplayName();
		}

		public string GetDisplayName() {
			if(!isCustom) {
				    uint? levelNo = GetLevelNo(Filename);
				    if(!levelNo.HasValue) {
				    	levelNo = 0;
				    }
				   
					return LanguageResources.GetString(LanguageKey.Level) + levelNo;
					
				
			} else {
				return GetCustomLevelName(filename);
			}
			
		}
		
	    public static string GetCustomLevelName(string path)
        {
            int maxLen = 30;
            string name = path.Substring(C_ENHANCED_LEVELS_DIR.Length);
            if (name.Length > maxLen)
            {
                name = name.Substring(0, maxLen);
            }
            return name;
        }
	    
	     public override int GetHashCode() {
	    	return filename.GetHashCode();
	    }
		public override bool Equals(Object other)
		{
			if(other is LevelInfo) {
				return Equals(other as LevelInfo);
				
			} else {
				return base.Equals(other);
			}
		}
		
		public bool Equals(LevelInfo other)
		{
			return other.filename.Equals(filename);
		
		}
		
		public int CompareTo(LevelInfo other)
		{
			return string.Compare(this.Filename, other.Filename, StringComparison.InvariantCulture);
		}
	}
}
