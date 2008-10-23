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
using Mogre;
using Wof.Languages;
using FontManager=Wof.Languages.FontManager;

namespace Wof.Controller.Indicators
{
    /// <summary>
    /// Klasa odpowiedzialna za wyœwietlanie wiadomoœci (powiadomieñ gracza) w trakcie gry
    /// </summary>
    internal class GameMessages
    {
      
        private MessageEntry currentMessage;
        private DateTime startTime;

        private List<MessageEntry> messageQueue;

        private OverlayContainer messageContainer;
        private OverlayElement messageElement;

        private Overlay messageOverlay;
        private Viewport mainViewport;

        private readonly float xMargin = 0.01f;
        private readonly float yMargin = 0.01f;


        public GameMessages(Viewport mainViewport, float xMargin, float yMargin) : this(mainViewport)
        {
            this.xMargin = xMargin;
            this.yMargin = yMargin;
        }

        public GameMessages(Viewport mainViewport)
        {
            messageQueue = new List<MessageEntry>();
            this.mainViewport = mainViewport;

            messageOverlay = OverlayManager.Singleton.GetByName("Wof/HUD");
            CreateMessageContainer();
        }


        private void CreateMessageContainer()
        {
            messageElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "messageElement " + DateTime.Now.Ticks);
            messageContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                      "Panel", "messageContainer " + DateTime.Now.Ticks);

            messageElement.SetDimensions(mainViewport.ActualWidth, mainViewport.ActualHeight);
            messageElement.MetricsMode = GuiMetricsMode.GMM_PIXELS;

            messageElement.SetParameter("font_name", FontManager.CurrentFont);

            messageElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            messageElement.SetParameter("char_height", "0.03");
            messageElement.SetParameter("colour_top", "0.8 0.34 0.34");
            messageElement.SetParameter("colour_bottom", "0.8 0.1 0.1");
            messageElement.Caption = "";

            messageContainer.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            messageContainer.SetDimensions(1.0f, 1.0f);
            messageContainer.SetPosition(0.01f, 0.01f);

            messageContainer.AddChild(messageElement);
            messageOverlay.Add2D(messageContainer);
            messageContainer.Show();
        }


        public bool IsMessageQueueEmpty()
        {
            return messageQueue.Count == 0;
        }

        public void AppendMessage(String message)
        {
            messageQueue.Add(new MessageEntry(message));
        }

        public void AppendMessage(float x, float y, String message)
        {
            messageQueue.Add(new MessageEntry(x, y, message));
        }

        public void AppendMessage(MessageEntry messageEntry)
        {
            messageQueue.Add(messageEntry);
        }


        public void UpdateControl()
        {
            if (currentMessage == null)
            {
                if (messageQueue.Count != 0)
                {
                    currentMessage = messageQueue[0];
                    DisplayMessage();
                    messageQueue.RemoveAt(0);
                }
            }
            else
            {
                UpdateMessage();

                TimeSpan diff = DateTime.Now.Subtract(startTime);
                if (!currentMessage.Permanent && diff.TotalMilliseconds > currentMessage.Time)
                {
                    ClearMessage();
                }
            }
        }

        private void DisplayMessage()
        {
            startTime = DateTime.Now;

            currentMessage.IncreaseX(xMargin);
            currentMessage.IncreaseY(yMargin);

            messageContainer.SetPosition(currentMessage.X, currentMessage.Y);
            messageElement.SetParameter("char_height", currentMessage.getCharHeightString());
            messageElement.SetParameter("colour_top", currentMessage.ColourTop);
            messageElement.SetParameter("colour_bottom", currentMessage.ColourBottom);

            messageElement.Caption = currentMessage.Message;
            messageElement.Show();
        }

        private void UpdateMessage()
        {
            if (messageElement != null && currentMessage.Blinking)
            {
                if (((DateTime.Now.Millisecond/100.0f)%100) > 7)
                {
                    if (messageElement.IsVisible) messageElement.Hide();
                }
                else
                {
                    if (!messageElement.IsVisible) messageElement.Show();
                }
            }
        }

        public String PeekMessage()
        {
            if (currentMessage != null)
            {
                return currentMessage.Message;
            }
            else return "";
        }

        public void ClearMessages(String messageTxt)
        {
            if (PeekMessage().Equals(messageTxt))
            {
                ClearMessage();
            }
           
            for (int i = 0; i < messageQueue.Count; i++)
            {
                if (messageQueue[i].Message.Equals(messageTxt))
                {
                    messageQueue.Remove(messageQueue[i]);
                }
            }
           

        }

        public void ClearMessage()
        {
            //startTime = null;
            currentMessage = null;
            if (messageElement != null)
            {
                messageElement.Caption = "";
            }
        }


        public void DestroyMessageContainer()
        {
            if (messageOverlay != null)
            {
                messageOverlay.Hide();
                messageOverlay.Dispose();
                messageOverlay = null;
            }

            messageContainer.Hide();

            OverlayManager.Singleton.DestroyOverlayElement(messageElement);
            messageElement = null;
            messageContainer.Dispose();
            messageContainer = null;
        }
    }
}