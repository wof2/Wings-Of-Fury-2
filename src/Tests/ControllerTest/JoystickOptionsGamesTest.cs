/*
 * Created by SharpDevelop.
 * User: w
 * Date: 2014-12-25
 * Time: 14:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Mogre;
using BetaGUI;
using Wof.Controller;
using Wof.Controller.Screens;

namespace Wof.Tests.ControllerTest
{
	/// <summary>
	/// Description of JoystickOptionsGamesTest.
	/// </summary>
	public class JoystickOptionsGamesTest : Game
	{
		static JoystickOptionsGamesTest game;
		public JoystickOptionsGamesTest()
		{			
		}

		static bool shouldReload;
		
		[STAThread]   
        protected new static void Main(string[] args)
        {    
         
            try
            {
                game = new JoystickOptionsGamesTest();
               
                try{
                    if (EngineConfig.DebugInfo)
                    {
                        User32.SetWindowPos(User32.PtrToConsole, (IntPtr) 0, consolePosition.X, consolePosition.Y, 0, 0,
                                            User32.SWP_NOSIZE);
                    }
                }
                catch(Exception ex) {
                	
                }
               
                EngineConfig.DisplayingMinimap = false;
               
                game.Go();
              
              
            }
            catch (SEHException sex)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    FrameWorkStaticHelper.ShowOgreException(sex);
                else
                    throw;
            }
            catch(RootInitializationException)
            {
                // i tak będzie reload
                shouldReload = true;
            }

            if (shouldReload)
            {
            	ReloadGame();
			
            }
	        if (game.afterExit != null) game.afterExit();
            
           
        }
               
      
        
        
		public override void CreateScene()
        {
            if (currentScreen == null)
            {
                   currentScreen = new JoystickOptionsScreen(this, this, viewport, camera, inputKeyboard, inputJoysticks);
         
            }
            if(!currentScreen.Displayed())
            {
            	currentScreen.DisplayGUI(false);
            }
           
        }
	}
}
