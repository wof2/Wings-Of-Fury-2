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

using MOIS;
using System;
using System.Collections.Generic;
using BetaGUI;
using Mogre;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.Misc;
using FontManager = Wof.Languages.FontManager;
using Math=Mogre.Math;

namespace Wof.Controller.Screens
{
    internal class ButtonHolder
    {
        private Button option;

        public Button Option
        {
            get { return option; }
        }

        private String value;

        public String Value
        {
            get { return value; }
        }
	
        public ButtonHolder(Button button, String description)
        {
            option = button;
            value = description;
        }
    }


    internal abstract class AbstractOptionsScreen : AbstractScreen, BetaGUIListener
    {
   		
   		
   		protected bool showRestartRequiredMessage = true;
        protected bool autoGoBack = true;

        protected List<ButtonHolder> options;

     

        protected Window guiWindow;
        protected Button exitButton;
        protected Button nextButton;
        protected Button prevButton;
        
       
        public AbstractOptionsScreen(GameEventListener gameEventListener,
                                     IFrameWork framework, Viewport viewport, Camera camera) :
            base(gameEventListener, framework, viewport, camera)
        {
           
            this.fontSize = (uint)(0.73f * fontSize); // mniejsza czcionka w opcjach
        }

        protected override void CreateGUI()
        {
            base.CreateGUI();
            currentScreen = 0;
            createScreen();
        }
        
        protected List<object>availableOptions;

   

        private void createScreen()
        {
        	/*
            if (mGui != null)
            {
                mGui.killGUI();
            }*/
            currentButton = 0;
           // mGui = new GUI(FontManager.CurrentFont, fontSize);
          //  createMouse();

            Vector2 m = GetMargin();
            int h = (int)GetTextVSpacing();


            if (guiWindow != null) mGui.killWindow(guiWindow);

            guiWindow = mGui.createWindow(new Vector4(m.x,
                                                      m.y, Viewport.ActualWidth / 2,
                                                      Viewport.ActualHeight - m.y - h),
                                                     "bgui.window", (int)wt.NONE, getTitle());
/*
            Window logoWindow = mGui.createWindow(new Vector4(h, h, 12.0f * h, 1.40f * h), "", (int)wt.NONE, "");
            logoWindow.createStaticImage(new Vector4(0, 0, 12.0f * h, 1.40f * h), "wof2.png");
            logoWindow.show();

            Window ravenWindow = mGui.createWindow(new Vector4(viewport.ActualWidth - 9 * h, 0.8f * h, 8.0f * h, 4.27f * h), "", (int)wt.NONE, "");
            ravenWindow.createStaticImage(new Vector4(0, 0, 8.0f * h, 4.27f * h), "ravenlore.png");
            ravenWindow.show();
*/

            Callback cc = new Callback(this); // remember to give your program the BetaGUIListener interface

            availableOptions = GetAvailableOptions();

            LayoutOptions(availableOptions, guiWindow, cc);
            guiWindow.show();
        }

        protected virtual string GetOptionDisplayText(string option)
        {
             if (option.StartsWith("__"))
             {
                 return option.Substring(2);
             }
             else
             {
                 return option;
             }
        }


     

        protected virtual Vector4 GetOptionPos(uint index, Window window)
        {
            return new Vector4(window.w / 8, (index + 2) * GetTextVSpacing(), 3 * window.w / 4, GetTextVSpacing());
        }

        protected void HighlightButton(Button button)
        {
           foreach(Button b in this.buttons)
           {
               if(button == b)
               {
                   b.mO.MaterialName = "bgui.selected.button";
                   
               }else
               {
                    b.mO.MaterialName =  "bgui.button";
               }
              
              

           }

           //   : 


        }

        protected virtual void LayoutOptions(List<object>availableOptions, Window window, Callback cc)
        {
            // trzeba stworzyc siatke opcji
            // w jednej kolumnie nie powinno byc wiecej niz C_MAX_OPTIONS opcji

            int size = availableOptions.Count;
            uint posIndex = 0;
            options = new List<ButtonHolder>();
            uint j = 0;
            for (j = 0;
                 j < C_MAX_OPTIONS &&
                 availableOptions.Count > j + C_MAX_OPTIONS*currentScreen;
                 j++)
            {
                int index = (int) j + C_MAX_OPTIONS*currentScreen;
                String option;
               
                if (availableOptions[index]==null)
                {
                    continue;
                }
                posIndex++;
				option = availableOptions[index].ToString();
                Vector4 pos = GetOptionPos(j, window);


               
                if (option.StartsWith("__"))
                {
                    guiWindow.createStaticText(
                        pos,
                        GetOptionDisplayText(option));
                    if (OnOptionCreated != null)
                    {
                        OnOptionCreated(pos, false, option, (uint)index, currentScreen, null);
                    }
                    continue;
                }


              
               
                bool selected = IsOptionSelected(option);
                Button button = guiWindow.createButton(
                        pos,
                        selected ? "bgui.selected.button" : "bgui.button",
                        GetOptionDisplayText(option), cc, j);

                ButtonHolder holder = new ButtonHolder(button, option);
                 options.Add(holder);
                if(OnOptionCreated != null)
                {
                    OnOptionCreated(pos, selected, option, (uint)index, currentScreen, holder);
                }
                

               
            }
            uint totalOptions = (uint) options.Count;
            uint lastOptButton = posIndex;
            uint exitButtonIndex = lastOptButton;

            if (currentScreen != 0)
            {
                prevButton = guiWindow.createButton
                    (
                    new Vector4(
                        window.w / 8,
                        (lastOptButton + 2) * GetTextVSpacing(),
                        3 * window.w / 8, 
                        GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Previous),
                    cc,
                    j + 1
                    );
                totalOptions++;
                exitButtonIndex = lastOptButton + 1;
            }
            else
            {
                prevButton = null;
            }

            if (currentScreen < ((1.00f*availableOptions.Count)/C_MAX_OPTIONS) - 1)
            {
                nextButton = guiWindow.createButton
                    (
                    new Vector4(
                        window.w / 2,
                        (lastOptButton + 2) * GetTextVSpacing(),
                        3 * window.w / 8, 
                        GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Next),
                    cc,
                    prevButton == null ? (j + 1) : (j + 2)
                    );
                totalOptions++;
                exitButtonIndex = lastOptButton + 1;

            }
            else
            {
                nextButton = null;
            }
           
            exitButton = guiWindow.createButton
                (
                new Vector4(
                    window.w / 3,
                    (exitButtonIndex + 2) * GetTextVSpacing(),
                    window.w / 3,
                    GetTextVSpacing()),
                "bgui.button",
                LanguageResources.GetString(LanguageKey.Back),
                cc,
                totalOptions
                );
            totalOptions++;


            // Skopiuj przyciski to tablicy buttons, tak aby dzia³a³a obs³uga klawiatury
            initButtons((int) totalOptions);

            int k = 0;
            for (k = 0; k < options.Count; k++)
            {
                buttons[k] = options[k].Option;
            }

            if (prevButton != null && nextButton != null)
            {
                buttons[k] = prevButton;
                buttons[k + 1] = nextButton;
            }
            else if (prevButton != null)
            {
                buttons[k] = prevButton;
            }
            else if (nextButton != null)
            {
                buttons[k] = nextButton;
            }

            buttons[totalOptions - 1] = exitButton;
            selectButton(0);
            backButtonIndex = (int) totalOptions - 1;
            if(showRestartRequiredMessage)
            {
                guiWindow.createStaticTextAutoSplit(new Vector4(GetMargin().x, exitButton.Y + 2 * GetTextVSpacing(), window.w - GetMargin().x *2, GetTextVSpacing() * 2), LanguageResources.GetString(LanguageKey.ChangeOptionMessage1)+ " " + LanguageResources.GetString(LanguageKey.ChangeOptionMessage2));
          /*
            	guiWindow.createStaticText(
                    new Vector4(GetMargin().x, exitButton.Y + 1 * GetTextVSpacing(), window.w / 2, GetTextVSpacing()),
	                );
	            guiWindow.createStaticText(
                    new Vector4(GetMargin().x, exitButton.Y + 2 * GetTextVSpacing(), window.w / 2, GetTextVSpacing()),
	                );*/
            }
         
        }

        protected abstract String getTitle();

        protected abstract List<object>GetAvailableOptions();

        protected abstract void ProcessOptionSelection(ButtonHolder selected);

        protected abstract bool IsOptionSelected(String option);

        public delegate void OptionCreated(Vector4 pos, bool selected, string optionDisplayText, uint index, int page, ButtonHolder holder);

        protected event OptionCreated OnOptionCreated;


        #region BetaGUIListener Members


        protected virtual void GoToBack(Button referer)
        {
            gameEventListener.GotoOptionsScreen();
        }

        public virtual void onButtonPress(Button referer)
        {
            if (screenTime > C_RESPONSE_DELAY)
            {
                screenTime = 0;
                if (referer == nextButton)
                {
                    PlayClickSound();
                    currentScreen++;
                    createScreen();
                    return;
                }

                if (referer == prevButton)
                {
                    PlayClickSound();
                    currentScreen--;
                    createScreen();
                    return;
                }

                foreach (ButtonHolder holder in options)
                {
                    if (holder.Option == referer)
                    {
                        if (IsOptionSelected(holder.Value))
                        {
                            return;
                        }
                       // HighlightButton(holder.Option);
                        PlayClickSound();
                        ProcessOptionSelection(holder);
                        if (autoGoBack) GoToBack(referer);
                        return;
                    }
                }
                PlayClickSound();
                GoToBack(referer);
               
            }
        }

        #endregion
    }
}