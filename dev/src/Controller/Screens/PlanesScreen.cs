/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
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
using BetaGUI;
using Mogre;
using Wof.Languages;
using Wof.Model.Level.Planes;
using FontManager = Wof.Languages.FontManager;

namespace Wof.Controller.Screens
{
    internal class PlanesScreen : AbstractScreen, BetaGUIListener
    {
        protected uint smallFontSize = 0;
        #region Private Fields

        private Window guiWindow;
       
        #endregion

        #region GameScreen Members

        public PlanesScreen(GameEventListener gameEventListener,
                                IFrameWork framework, Viewport viewport, Camera camera) :
            base(gameEventListener, framework, viewport, camera)
        {
            fontSize = (uint)(0.83f * fontSize); // mniejsza czcionka na ekranie opcji
            smallFontSize = (uint)(0.7f * fontSize); // bardzo małe napisy
            // uwaga 'smallFontSize', moze byc nie zainicjalizowane jesli uzyto innego kontruktora!!!
        }

        private const string C_P47_OFF = "bgui.button";
        private const string C_P47_2_OFF = "bgui.button.p47";


        private const string C_F4U_OFF = "bgui.button";
        private const string C_F4U_2_OFF = "bgui.button.f4u";

        private const string C_P47_ON = "bgui.selected.button";
        private const string C_P47_2_ON = "bgui.selected.button.p47";


        private const string C_F4U_ON = "bgui.selected.button";
        private const string C_F4U_2_ON = "bgui.selected.button.f4u";

        private const string C_B25_ON = "bgui.selected.button";
        private const string C_B25_2_ON = "bgui.selected.button.b25";

        private const string C_B25_OFF = "bgui.button";
        private const string C_B25_2_OFF = "bgui.button.b25";


        protected override void CreateGUI()
        {
            base.CreateGUI();

            Vector2 m = GetMargin();
            int h = (int)GetTextVSpacing();


            guiWindow = mGui.createWindow(new Vector4(m.x,
                                                      m.y, Viewport.ActualWidth / 2,
                                                      Viewport.ActualHeight - m.y - h),
                                                      "bgui.window", (int)wt.NONE, LanguageResources.GetString(LanguageKey.Planes));

            
            Callback cc = new Callback(this); // remember to give your program the BetaGUIListener interface
            mGui.mFontSize = smallFontSize;
          
        

            mGui.mFontSize = fontSize;
            initButtons(7, 6);

            string matp47, matp47_2;
            string matf4u, matf4u_2;
            string matb25, matb25_2;

            if(EngineConfig.CurrentPlayerPlaneType == PlaneType.P47)
            {
                matp47 = C_P47_ON;
                matf4u = C_F4U_OFF;
                matb25 = C_B25_OFF;
               
                matp47_2 = C_P47_2_ON;
                matf4u_2 = C_F4U_2_OFF;
                matb25_2 = C_B25_2_OFF;

            }
            else
            if (EngineConfig.CurrentPlayerPlaneType == PlaneType.B25)
            {
                matp47 = C_P47_OFF;
                matf4u = C_F4U_OFF;
                matb25 = C_B25_ON;

                matp47_2 = C_P47_2_OFF;
                matf4u_2 = C_F4U_2_OFF;
                matb25_2 = C_B25_2_ON;
            }
            else
            {
                matp47 = C_P47_OFF;
                matf4u = C_F4U_ON;
                matb25 = C_B25_OFF;

                matp47_2 = C_P47_2_OFF;
                matf4u_2 = C_F4U_2_ON;
                matb25_2 = C_B25_2_OFF;
            }



            buttons[0] = guiWindow.createButton(new Vector4(5, 2 * GetTextVSpacing(), -5 + guiWindow.w * 0.5f, GetTextVSpacing()), matp47,
                                                LanguageResources.GetString(LanguageKey.P47),
                                                cc, 0);

            buttons[1] = guiWindow.createButton(new Vector4(buttons[0].x + buttons[0].w, 2 * GetTextVSpacing(), buttons[0].w, GetTextVSpacing()), matf4u,
                                              LanguageResources.GetString(LanguageKey.F4U),
                                              cc, 1);

            buttons[2] = guiWindow.createButton(new Vector4(5, 2 * GetTextVSpacing() + buttons[0].h + guiWindow.h * 0.4f, -5 + guiWindow.w * 0.5f, GetTextVSpacing()), matb25,
                                                LanguageResources.GetString(LanguageKey.B25),
                                                cc, 2);

            Vector4 p47img = new Vector4(buttons[0].x, buttons[0].y + buttons[0].h, buttons[0].w, guiWindow.h*0.4f);
            Vector4 f4uimg = new Vector4(buttons[1].x, buttons[1].y + buttons[1].h, buttons[1].w, guiWindow.h*0.4f);
            Vector4 b25img = new Vector4(buttons[2].x, buttons[2].y + buttons[2].h, buttons[2].w, guiWindow.h * 0.4f);



            buttons[3] = guiWindow.createButton(p47img, matp47_2,
                                              "",
                                              cc, 3);
            buttons[3].mma = "";
                 

            buttons[4] = guiWindow.createButton(f4uimg, matf4u_2,
                                              "",
                                              cc, 4);
            buttons[4].mma = "";


            buttons[5] = guiWindow.createButton(b25img, matb25_2,
                                            "",
                                            cc, 5);
            buttons[5].mma = "";

          

            buttons[6] = guiWindow.createButton(new Vector4(5, 26 * GetTextVSpacing(), -10 + Viewport.ActualWidth / 2, GetTextVSpacing()), "bgui.button",
                                               LanguageResources.GetString(LanguageKey.Back), cc, 6);
           
            guiWindow.show();
        }


        #endregion

        #region BetaGUIListener Members

     
        public void onButtonPress(Button referer)
        {
            if (screenTime > C_RESPONSE_DELAY)
            {
                if (referer == buttons[backButtonIndex])
                {
                    PlayClickSound();
                    gameEventListener.GotoStartScreen();
                    return;
                }


                if (!EngineConfig.IsEnhancedVersion)
                {
                    PlayClickSound();
                    gameEventListener.GotoEnhancedVersionScreen();
                    return;
                }


                if (referer == buttons[0] || referer == buttons[3])
                {
                    PlayClickSound();
                    if (EngineConfig.IsEnhancedVersion)
                    {
                        this.forceRebuild = true;
                        buttons[0].mmn = C_P47_ON;
                        buttons[1].mmn = C_F4U_OFF;
                        buttons[2].mmn = C_B25_OFF;

                        buttons[3].mmn = C_P47_2_ON;
                        buttons[4].mmn = C_F4U_2_OFF;
                        buttons[5].mmn = C_B25_2_OFF;

                        EngineConfig.CurrentPlayerPlaneType = PlaneType.P47;
                        EngineConfig.SaveEngineConfig();
                    }
                }else
                if (referer == buttons[1] || referer == buttons[4])
                {
                    PlayClickSound();
                    if (EngineConfig.IsEnhancedVersion)
                    {
                        this.forceRebuild = true;
                        buttons[0].mmn = C_P47_OFF;
                        buttons[1].mmn = C_F4U_ON;
                        buttons[2].mmn = C_B25_OFF;

                        buttons[3].mmn = C_P47_2_OFF;
                        buttons[4].mmn = C_F4U_2_ON;
                        buttons[5].mmn = C_B25_2_OFF;

                        EngineConfig.CurrentPlayerPlaneType = PlaneType.F4U;
                        EngineConfig.SaveEngineConfig();
                    }
                }
                else
                if (referer == buttons[2] || referer == buttons[5]) 
                {
                    PlayClickSound();
                    if (EngineConfig.IsEnhancedVersion)
                    {
                        this.forceRebuild = true;
                        buttons[0].mmn = C_P47_OFF;
                        buttons[1].mmn = C_F4U_OFF;
                        buttons[2].mmn = C_B25_ON;
                        
                        buttons[3].mmn = C_P47_2_OFF;
                        buttons[4].mmn = C_F4U_2_OFF;
                        buttons[5].mmn = C_B25_2_ON;

                        EngineConfig.CurrentPlayerPlaneType = PlaneType.B25;
                        EngineConfig.SaveEngineConfig();
                    }
                }
            }
        }

        #endregion
    }
}