/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-11-02
 * Time: 20:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Wof.Controller.Screens;
using Wof.Model.Level;
using System.Collections.Generic;

namespace Wof.Tests.ModelTest.LevelTest
{
	/// <summary>
	/// Description of LevelUtilsTest.
	/// </summary>
	public class LevelUtilsTest
	{
		public static void Test()
		{
			var list = new List<Achievement>();
			list.Add(new Achievement(AchievementType.Generals, 2324));
			LoadGameUtil.NewLevelCompleted(new LevelInfo(3), list);
		//	LoadGameUtil.GetCompletedLevels();
		}
		
		
		public static void Main()
        {
            Test();    
        }
	}
}
