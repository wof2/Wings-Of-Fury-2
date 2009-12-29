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
using FontManager = Wof.Languages.FontManager;

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
   		protected int C_MAX_OPTIONS = 12;
   		
   		protected bool showRestartRequiredMessage = true;

        private List<ButtonHolder> options;
        private int currentScreen;

        protected Window guiWindow;
        private Button exitButton;
        private Button nextButton;
        private Button prevButton;
        
       
     

        protected Root root;

        public AbstractOptionsScreen(GameEventListener gameEventListener,
                                     IFrameWork framework, Viewport viewport, Camera camera, Root root) :
            base(gameEventListener, framework, viewport, camera)
        {
            this.root = root;
            this.fontSize = (uint)(0.83f * fontSize); // mniejsza czcionka w opcjach
        }

        protected override void CreateGUI()
        {
            base.CreateGUI();
            currentScreen = 0;
            createScreen();
        }
        
        public static int AddControlsInfoToGui(Window guiWindow, GUI mGui, int left, int top, int initialTopSpacing, float width, float textVSpacing, uint fontSize)
        {
        	
        	int y = initialTopSpacing;
        	int h = (int)textVSpacing;


            OverlayContainer c;
            y += (int)(h*2);
            c = guiWindow.createStaticText(new Vector4(left - 10, top + y, width, h), LanguageResources.GetString(LanguageKey.Controls));
            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f, 0.8f, 0.0f), new ColourValue(0.9f, 0.7f, 0.0f));

            uint oldFontSize = mGui.mFontSize;
            mGui.mFontSize = fontSize;

            y += (int)h;
            c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
            	                           LanguageResources.GetString(LanguageKey.Engine) + ": (" + KeyMap.GetName(KeyMap.Instance.Engine)+ " hold)");
            
            // "Engine: E (hold)");
            y += (int)(h * 0.83f);
          


            if(KeyMap.Instance.Left == KeyCode.KC_LEFT && KeyMap.Instance.Right == KeyCode.KC_RIGHT )
            { 
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + ": ");
            	guiWindow.createStaticImage(new Vector4(left + width * 0.5f, top + y - 0.41f * oldFontSize, oldFontSize * 0.95f, oldFontSize * 0.95f), "arrow_left.png");
           		guiWindow.createStaticImage(new Vector4(left + width * 0.5f + h * 0.95f, top + y - 0.41f * oldFontSize, oldFontSize * 0.95f, oldFontSize * 0.95f), "arrow_right.png");
            } else
            { 
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
            		                           LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + ": " + KeyMap.GetName(KeyMap.Instance.Left) + "/" +  KeyMap.GetName(KeyMap.Instance.Right));
            	
            }
           

            y += (int)(h * 0.83f);    
            if(KeyMap.Instance.Up == KeyCode.KC_UP && KeyMap.Instance.Down == KeyCode.KC_DOWN )
            {
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.Pitch) + ": ");
            	guiWindow.createStaticImage(new Vector4(left + width * 0.5f, top + y - 0.35f * oldFontSize, oldFontSize * 0.95f, oldFontSize * 0.95f), "arrow_up.png");
            	guiWindow.createStaticImage(new Vector4(left + width * 0.5f + h * 0.95f, top + y - 0.35f * oldFontSize, oldFontSize * 0.95f, oldFontSize * 0.95f), "arrow_down.png");

            } else
            {
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.Pitch) + ": "  + KeyMap.GetName(KeyMap.Instance.Up) + "/" +  KeyMap.GetName(KeyMap.Instance.Down));
            }



            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h),  
                                           LanguageResources.GetString(LanguageKey.Spin) + ": " + KeyMap.GetName(KeyMap.Instance.Spin));
            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h), 
                                           LanguageResources.GetString(LanguageKey.Gear) + ": " + KeyMap.GetName(KeyMap.Instance.Gear));
            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h), 
                                           LanguageResources.GetString(LanguageKey.Gun) + ": " + KeyMap.GetName(KeyMap.Instance.GunFire));
            y += (int)(h * 0.83f);

            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h), 
                                           LanguageResources.GetString(LanguageKey.Bombs) + "/" + LanguageResources.GetString(LanguageKey.Rockets)+ ": " + KeyMap.GetName(KeyMap.Instance.AltFire));
            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h), 
                                           LanguageResources.GetString(LanguageKey.Camera) + ": " + KeyMap.GetName(KeyMap.Instance.Camera));
            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.Zoomin) + ": " + "Page UP");

            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h), 
                                           LanguageResources.GetString(LanguageKey.Zoomout) + ": " + "Page DOWN");
            y += (int)(h * 0.83f);
            c = guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.RearmEndMission) + ": " + KeyMap.GetName(KeyMap.Instance.AltFire));

            mGui.mFontSize = oldFontSize;
            
            return y;
        }

        private void createScreen()
        {
            if (mGui != null)
            {
                mGui.killGUI();
            }
            currentButton = 0;
            mGui = new GUI(FontManager.CurrentFont, fontSize);
            createMouse();

            guiWindow = mGui.createWindow(new Vector4(0,
                                                      0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int) wt.NONE, getTitle());
            Callback cc = new Callback(this); // remember to give your program the BetaGUIListener interface

            List<String> availableOptions = GetAvailableOptions(root);

            LayoutOptions(availableOptions, guiWindow, cc);
            guiWindow.show();
        }

        protected virtual void LayoutOptions(List<String> availableOptions, Window window, Callback cc)
        {
            // trzeba stworzyc siatke opcji
            // w jednej kolumnie nie powinno byc wiecej niz C_MAX_OPTIONS opcji

            int size = availableOptions.Count;

            options = new List<ButtonHolder>();
            uint j = 0;
            for (j = 0;
                 j < C_MAX_OPTIONS &&
                 availableOptions.Count > j + C_MAX_OPTIONS*currentScreen;
                 j++)
            {
                String optionValue = availableOptions[(int) j + C_MAX_OPTIONS*currentScreen];
                if (optionValue == null)
                {
                    continue;
                }
                else if (optionValue.StartsWith("__"))
                {
                    guiWindow.createStaticText(
                        new Vector4(viewport.ActualWidth / 6, (j + 2) * GetTextVSpacing(), 2 * viewport.ActualWidth / 3, GetTextVSpacing()),
                        optionValue.Substring(2));
                    continue;
                }


                Button button = guiWindow.createButton(
                    new Vector4(
                        viewport.ActualWidth/6,
                        (j + 2) * GetTextVSpacing(), 2 * viewport.ActualWidth / 3, GetTextVSpacing()),
                        IsOptionSelected(optionValue) ? "bgui.selected.button" : "bgui.button",
                        optionValue, cc, j);

                options.Add(new ButtonHolder(button, optionValue));
            }
            uint totalOptions = (uint) options.Count;

            if (currentScreen != 0)
            {
                prevButton = guiWindow.createButton
                    (
                    new Vector4(
                        viewport.ActualWidth/6,
                        (C_MAX_OPTIONS + 2) * GetTextVSpacing(), 
                        viewport.ActualWidth / 3, 
                        GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Previous),
                    cc,
                    j + 1
                    );
                totalOptions++;
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
                        viewport.ActualWidth/2,
                        (C_MAX_OPTIONS + 2) * GetTextVSpacing(),
                        viewport.ActualWidth / 3, 
                        GetTextVSpacing()),
                    "bgui.button",
                    LanguageResources.GetString(LanguageKey.Next),
                    cc,
                    prevButton == null ? (j + 1) : (j + 2)
                    );
                totalOptions++;
            }
            else
            {
                nextButton = null;
            }

            exitButton = guiWindow.createButton
                (
                new Vector4(
                    viewport.ActualWidth/3,
                    (C_MAX_OPTIONS + 3) * GetTextVSpacing(),
                    viewport.ActualWidth/3,
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
            	guiWindow.createStaticText(
	                new Vector4(viewport.ActualWidth / 6, C_MAX_OPTIONS * GetTextVSpacing() + 5 * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
	                LanguageResources.GetString(LanguageKey.ChangeOptionMessage1));
	            guiWindow.createStaticText(
	                new Vector4(viewport.ActualWidth / 6, C_MAX_OPTIONS * GetTextVSpacing() + 6 * GetTextVSpacing(), viewport.ActualWidth / 2, GetTextVSpacing()),
	                LanguageResources.GetString(LanguageKey.ChangeOptionMessage2));
            }
         
        }

        protected abstract String getTitle();

        protected abstract List<String> GetAvailableOptions(Root root);

        protected abstract void ProcessOptionSelection(String selected);

        protected abstract bool IsOptionSelected(String option);

        #region BetaGUIListener Members

        public void onButtonPress(Button referer)
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
                        PlayClickSound();
                        ProcessOptionSelection(holder.Value);
                        break;
                    }
                }
                PlayClickSound();
                gameEventListener.GotoOptionsScreen();
            }
        }

        #endregion
    }
}