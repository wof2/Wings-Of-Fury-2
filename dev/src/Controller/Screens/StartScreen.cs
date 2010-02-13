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
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading;

using BetaGUI;
using Mogre;
using Wof.Languages;
using FontManager = Wof.Languages.FontManager;

namespace Wof.Controller.Screens
{
    internal class StartScreen : AbstractScreen, BetaGUIListener
    {
        private Window guiWindow;

     //   private GUI updatesGUI;
      //  private Window updatesGUIWindow;


        private const float C_QUIT_AD_PROBABILITY = 0.0f;
        private Thread newUpdatesThread;

        private static bool updatesChecked = false;

        public StartScreen(GameEventListener gameEventListener,
                            IFrameWork framework, Viewport viewport, Camera camera) :
                               base(gameEventListener, framework, viewport, camera)
        {
            this.fontSize = (uint)(0.83f * fontSize); // mniejsza czcionka na ekranie startowym
        }
        
        protected bool areUpdatesAvailable = false;
        public override void CleanUp(Boolean justMenu)
        {
            base.CleanUp(justMenu);
          

            if (newUpdatesThread != null)
            {

                try
                {
                   if(newUpdatesThread.ThreadState != ThreadState.Stopped) newUpdatesThread.Abort();
                }
                catch (Exception)
                {
                }
            }

           /* if (updatesGUI != null)
            {
                updatesGUI.killGUI();
            }*/

        }

        private uint updateButtonIndex;
        protected override void CreateGUI()
        {
        	
        	 
                    
            base.CreateGUI();
            Vector2 m = GetMargin();
            int h = (int)GetTextVSpacing();
            string gameName = EngineConfig.C_GAME_NAME;
            
            if(EngineConfig.C_IS_DEMO)
            {
                gameName += " Demo";
            }
           
            
            guiWindow = mGui.createWindow(new Vector4(m.x,
                                                      m.y, Viewport.ActualWidth/2,
                                                      Viewport.ActualHeight - m.y - h ),
                                          			  "bgui.window", (int)wt.NONE, gameName);


            if (EngineConfig.IsEnhancedVersion)
            {
                guiWindow.createStaticImage(
                    new Vector4(0, GetTextVSpacing(), 0.95f * 8.7f * GetTextVSpacing(), 0.95f * GetTextVSpacing()), "enhanced.png", 100);
               
            }

            Callback cc = new Callback(this); // remember to give your program the BetaGUIListener interface

            int buttonCount = 11;
          //  if(EngineConfig.IsEnhancedVersion)
            {
                buttonCount += 1;
            }

            int backIndex = buttonCount - 1;

            initButtons(buttonCount, backIndex);
            uint i = 0;

      

            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.NewGame), cc, i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.CompletedLevels),
                                                cc, i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                              "bgui.button", LanguageResources.GetString(LanguageKey.CustomLevels),
                                              cc, i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.Highscores), cc,
                                                i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.Options), cc, i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.Tutorial), cc,
                                                i++);
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.Credits), cc, i++);

           

            updateButtonIndex = i;
            // indeks wystepuje jeszcze w metodzie checkAvailableUpdates() 
            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                           "bgui.button", LanguageResources.GetString(LanguageKey.CheckForUpdates), cc, i++);

            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                          "bgui.button", LanguageResources.GetString(LanguageKey.Donate), cc, i++);


            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                           "bgui.button", LanguageResources.GetString(LanguageKey.EnhancedVersion), cc, i++);


            pinImageToButton(buttons[i-1], "pin.png", 1.6f);

          //  if (EngineConfig.IsEnhancedVersion)
            {
                buttons[i] = guiWindow.createButton(new Vector4(0, (i + 2) * h, Viewport.ActualWidth / 2, h),
                                                          "bgui.button", LanguageResources.GetString(LanguageKey.Planes), cc, i++);

                pinImageToButton(buttons[i - 1], "pin.png", 1.6f);
            }


            buttons[i] = guiWindow.createButton(new Vector4(0, (i + 3) * h, Viewport.ActualWidth / 2, h),
                                                "bgui.button", LanguageResources.GetString(LanguageKey.Quit), cc, i);
          


            
            selectButton(0);
            if (!screenStateSet)
            {
               //  SetMousePosition((uint)(guiWindow.x + buttons[0].x + (Viewport.ActualWidth / 4)), (uint)(guiWindow.y + buttons[0].y + h / 2.0F));
           
            }
            
     
       
            guiWindow.show();
            //onNewUpdates();
            if (!updatesChecked)
            {
                newUpdatesThread = new Thread(checkAvailableUpdates);
                newUpdatesThread.Start(); 
            }
           
           
     
		
        }
        public override void FrameStarted(FrameEvent evt)
        {
            base.FrameStarted(evt);
        	lock(this)
        	{
        		if(areUpdatesAvailable)
        		{
        			onNewUpdates();
        			areUpdatesAvailable = false;
        		}
        	}
          	
        }
     
      
        protected void onNewUpdates() 
        {
            uint index = updateButtonIndex;
            //updatesGUI = new GUI(FontManager.CurrentFont, fontSize);
           // updatesGUI.SetZOrder(500);

    		BetaGUI.Button b = (buttons[index] as BetaGUI.Button);
            pinImageToButton(b, "new_updates.png");
            
        }
        
      
        
        protected void checkAvailableUpdates()
        {
            string url = EngineConfig.C_WOF_UPDATE_CHECK_PAGE + "?v=" + EngineConfig.C_WOF_VERSION + "&d=" + EngineConfig.C_IS_DEMO.ToString() + "&l=" + LanguageManager.ActualLanguageCode + "&e=" + EngineConfig.IsEnhancedVersion;
        	

			// For HTTP, cast the request to HttpWebRequest
			// allowing setting more properties, e.g. User-Agent.
			// An HTTP response can be cast to HttpWebResponse.
			try
			{
				WebRequest request = WebRequest.Create (url);
				using (WebResponse response = request.GetResponse())
				{
				   // Ensure that the correct encoding is used. 
				   // Check the response for the Web server encoding.
				   // For binary content, use a stream directly rather
				   // than wrapping it with StreamReader.
				 
				   using (StreamReader reader = new StreamReader
				      (response.GetResponseStream(), Encoding.UTF8))
				   {
                       
				       string content = reader.ReadToEnd();
				       updatesChecked = true;
				       lock(this)
				       {
				       		areUpdatesAvailable = "1".Equals(content);
				       }
				      
				       
				   }
                   response.Close();

				}
			}
			catch(Exception ex)
			{
                if(LogManager.Singleton != null)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_NORMAL, "Unable to connect to the \"check updates\" URL");
                }
				
			}
			
			
        }

        #region BetaGUIListener Members

        public void onButtonPress(Button referer)
        {
       
            if (screenTime > C_RESPONSE_DELAY)
            {
                PlayClickSound();


                string NewGame = LanguageResources.GetString(LanguageKey.NewGame);
                string CompletedLevels = LanguageResources.GetString(LanguageKey.CompletedLevels);
                string CustomLevels = LanguageResources.GetString(LanguageKey.CustomLevels);
                string Highscores = LanguageResources.GetString(LanguageKey.Highscores);
                string Options = LanguageResources.GetString(LanguageKey.Options);
                string Tutorial = LanguageResources.GetString(LanguageKey.Tutorial);
                string Credits = LanguageResources.GetString(LanguageKey.Credits);
                string Donate = LanguageResources.GetString(LanguageKey.Donate);
                string EnhancedVersion = LanguageResources.GetString(LanguageKey.EnhancedVersion);
                string Planes = LanguageResources.GetString(LanguageKey.Planes);
                string CheckForUpdates = LanguageResources.GetString(LanguageKey.CheckForUpdates);
                string Quit = LanguageResources.GetString(LanguageKey.Quit);



                if (referer.text.Equals(NewGame))
                {
                    gameEventListener.StartGame(EngineConfig.CurrentPlayerPlaneType);
                }
                else if(referer.text.Equals(CompletedLevels))
                {
                    gameEventListener.GotoLoadGameScreen();
                }
                else if(referer.text.Equals(CustomLevels))
                {
                    gameEventListener.GotoCustomLevelsScreen();
                }
                else  if(referer.text.Equals(Highscores))
                {
                    gameEventListener.GotoHighscoresScreen();
                }
                else if (referer.text.Equals(Options))
                {
                    gameEventListener.GotoOptionsScreen();
                }
                else if (referer.text.Equals(Tutorial))
                {
                    gameEventListener.GotoTutorialScreen();
                }
                else if (referer.text.Equals(Credits))
                {
                    gameEventListener.GotoCreditsScreen();
                }
                else if (referer.text.Equals(Donate))
                {
                    gameEventListener.GotoDonateScreen();
                }
                else if (referer.text.Equals(EnhancedVersion))
                {
                    gameEventListener.GotoEnhancedVersionScreen();
                }
                else if (referer.text.Equals(Planes))
                {
                    gameEventListener.GotoPlanesScreen();
                }
                else if (referer.text.Equals(CheckForUpdates))
                {
                    gameEventListener.GotoUpdateWebPage();
                }
                else if (referer.text.Equals(Quit))
                {
                    if(Mogre.Math.RangeRandom(0, 1) > (1 - C_QUIT_AD_PROBABILITY))
                    {
                        gameEventListener.GotoQuitScreen();
                    }
                    else
                    {
                        gameEventListener.ExitGame();
                    }
             
                   
                }
            }
        }

        #endregion
    }
}