/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-11-02
 * Time: 22:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Wof.Misc;

namespace Wof.Model.Level
{

	[Serializable()]
	public class CompletedLevelsInfo
	{
		protected SerializableDictionary<LevelInfo, List<Achievement>> completedLevels;
		
		public SerializableDictionary<LevelInfo, List<Achievement>> CompletedLevels {
			get { return completedLevels; }
			set { completedLevels = value; }
		}
		
		public CompletedLevelsInfo(){
			
		}
		
		public CompletedLevelsInfo( SerializableDictionary<LevelInfo, List<Achievement>> completedLevels)
		{
			this.completedLevels = completedLevels;
		}
		
		public static CompletedLevelsInfo GetDefaultCompletedLevelsInfo() {
			
			var list = new SerializableDictionary<LevelInfo, List<Achievement>>();
			list.Add(new LevelInfo(1), new List<Achievement>());
			return new CompletedLevelsInfo(list);
			
			
		}
		
		
	}
}
