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
using BetaGUI;
using Mogre;
using Wof.Controller.Screens;
using Wof.Languages;
using Wof.Misc;
using FontManager=Wof.Languages.FontManager;
using Math=Mogre.Math;

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

        private static readonly float xMargin = 0.03f;
        private static readonly float yMargin = 0.005f;
        private readonly float bgAnimationLength = 1.00f;

        private const float bgAnimationMaxOpacity = 0.5f;
      
        private Overlay iconOverlay;
		private OverlayElement iconElement;
        private Overlay messageBgOverlay;

        private Vector2 iconDefaultDimesions;

		private float radioIconWidth 
		{
			get {  return 0.05f; }
			
		}

        public static float XMargin
        {
            get { return xMargin; }
        }

        public static float YMargin
        {
            get { return yMargin; }
        }

        private float currentBgOpacity = 0.0f;
        private bool isIncreasingBgOpacity = false;
        private bool isDecreasingBgOpacity = false;

        private OverlayElement backgroundElement;
       

       // HasCustomIconPosition
       /* public GameMessages(Viewport mainViewport, float xMargin, float yMargin) : this(mainViewport)
        {
            this.xMargin = xMargin;
            this.yMargin = yMargin;
        }*/

        public GameMessages(Viewport mainViewport)
        {
            messageQueue = new List<MessageEntry>();
            this.mainViewport = mainViewport;

            messageOverlay = OverlayManager.Singleton.GetByName("Wof/HUD");
            messageBgOverlay = OverlayManager.Singleton.GetByName("Wof/MessageBarBg");
            backgroundElement = OverlayManager.Singleton.GetOverlayElement("Wof/MessageBarIconBg");
         
            CreateMessageContainer();
        }


        private void CreateMessageContainer()
        {
           // BetaGUI.GUI gui = new GUI();
           // Window w = gui.createWindow();
           // w.createStaticImage()

            backgroundElement.Hide();
            iconOverlay = OverlayManager.Singleton.GetByName("Wof/MessageBar");
            iconElement = OverlayManager.Singleton.GetOverlayElement("Wof/MessageBarIcon");
            iconElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            iconElement.SetDimensions(radioIconWidth, radioIconWidth);      
       		iconElement.Show();
       		iconOverlay.Hide(); // zewnetrzny kontener ukryje wszystko


            iconDefaultDimesions.x = Mogre.StringConverter.ParseReal(iconElement.GetParameter("width"));
            iconDefaultDimesions.y = Mogre.StringConverter.ParseReal(iconElement.GetParameter("height"));
       		
            messageElement = OverlayManager.Singleton.CreateOverlayElement(
                "TextArea", "messageElement " + DateTime.Now.Ticks);
            messageContainer = (OverlayContainer) OverlayManager.Singleton.CreateOverlayElement(
                                                      "Panel", "messageContainer " + DateTime.Now.Ticks);

            messageElement.SetDimensions(mainViewport.ActualWidth, mainViewport.ActualHeight);
            messageElement.MetricsMode = GuiMetricsMode.GMM_PIXELS;
           
            messageElement.SetParameter("font_name", FontManager.CurrentFont);

            messageElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            messageElement.SetParameter("char_height",  StringConverter.ToString(EngineConfig.CurrentFontSize));
            messageElement.SetParameter("colour_top", "0.1 0.1 0.1");
            messageElement.SetParameter("colour_bottom", "0.5 0.1 0.1");
            messageElement.Caption = "";

       
            messageContainer.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            messageContainer.SetDimensions(1.0f, 0.05f);
           // messageContainer.SetPosition(0.055f, 0.015f);

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

        protected void ShowCurrentMessage()
        {
            messageElement.Show();
            if (!iconOverlay.IsVisible)
            {
                iconOverlay.Show();
            }
            
            startTime = DateTime.Now;
        }
      
        protected bool instantBackground = false;
        
        public bool InstantBackground
        {
        	set { instantBackground = value; }
        	
        }

        public void UpdateControl(float timeSinceLastFrame)
        {
            if (currentMessage == null)
            {
                if (messageQueue.Count != 0)
                {
                 //   Console.WriteLine("Enqueuing");
                    currentMessage = messageQueue[0];
                    PrepareMessage();
                    messageQueue.RemoveAt(0);

                    if (currentMessage.NoBackground && messageElement != null && !messageElement.IsVisible)
                    {
                        ShowCurrentMessage();
                    }
                    
                    
                	if(instantBackground && messageElement != null && !messageElement.IsVisible) {
                	
	    		  		currentBgOpacity = bgAnimationMaxOpacity;
	    		  		SetBgOpacity(currentBgOpacity);
	                    ShowCurrentMessage();
                	}
                    
                    if(!currentMessage.NoBackground)
                    {
                        backgroundElement.Show();
                        isIncreasingBgOpacity = true;
                    }
                }
                else
                {
                   
                }
            }
            else
            {
                if(!currentMessage.NoBackground)
                {
                	
                	// BACKGROUND
                    if (isIncreasingBgOpacity)
                    {
                        if (currentBgOpacity < bgAnimationMaxOpacity)
                        {
                            SetBgOpacity(currentBgOpacity);
                            currentBgOpacity += timeSinceLastFrame / bgAnimationLength;
                            return;
                        }
                        else
                        {
                            isIncreasingBgOpacity = false;
                            currentBgOpacity = bgAnimationMaxOpacity;

                            // pokaz
                            ShowCurrentMessage();
                            backgroundElement.Show();
                        }
                    }
                    else
                        if (isDecreasingBgOpacity)
                        {
                            if (currentBgOpacity > 0.0f)
                            {
                                SetBgOpacity(currentBgOpacity);
                                currentBgOpacity -= timeSinceLastFrame / bgAnimationLength;
                                return;
                            }
                            else
                            {
                                isDecreasingBgOpacity = false;
                                currentBgOpacity = 0;
                                // ukryj
                                backgroundElement.Hide();
                                ClearMessage();
                                return;
                            }


                        }
                        else
                        {
                            UpdateMessage();
                        }
              
                	
                }
                else
                { 
                	
                	// no background

                    // show
                  //  Console.WriteLine("Showing");
                    if(messageElement != null &&!messageElement.IsVisible)
                    {
                        ShowCurrentMessage();
                    }
                    isDecreasingBgOpacity = false;
                    isIncreasingBgOpacity = false;
                    
                }
                
              
                TimeSpan diff = DateTime.Now.Subtract(startTime);
                if (!currentMessage.Permanent && diff.TotalMilliseconds > currentMessage.Time)
                {
               //     Console.WriteLine("Clearing");
               
                    isDecreasingBgOpacity = true;
                    if (messageElement != null) messageElement.Hide();
                    if (iconOverlay!= null && iconOverlay.IsVisible)
                    {
                        iconOverlay.Hide();
                    } 
                    if(instantBackground) 
                    {
                    	 backgroundElement.Hide();
	                     ClearMessage();
                    }
                    
                    if(currentMessage.NoBackground )
                    {
                        ClearMessage();
                        isDecreasingBgOpacity = false;
                        isIncreasingBgOpacity = false;
                    }
                    
                    

                }
            }
        }

      
        private void SetBgOpacity(float val)
        {
            if (backgroundElement == null) return;
            if (val > 1) val = 1.0f;
            if (val < 0) val = 0;
           
            
            MaterialPtr bgMaterial = backgroundElement.GetMaterial();
            bgMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetAlphaOperation(LayerBlendOperationEx.LBX_SOURCE1, LayerBlendSource.LBS_MANUAL, LayerBlendSource.LBS_CURRENT, val);
                //alpha_op_ex source1 src_manual src_current 0.3
            messageBgOverlay.Show();
        }

        private void PrepareMessage()
        {
            messageElement.Hide();
            
            // icon
            uint iconFrames = 1;
            float iconAnimationDuration = 1.0f;
            string changedIcon = null;
            if (currentMessage is IconedMessageEntry )
            {
                string icon = (currentMessage as IconedMessageEntry).Icon;
                Vector2 dim = (currentMessage as IconedMessageEntry).CustomIconDimensions;
                iconFrames = (currentMessage as IconedMessageEntry).IconFrames;
                iconAnimationDuration = (currentMessage as IconedMessageEntry).IconAnimationDuration;
                if (lastIconTexture != icon)
                {
                    lastIconTexture = icon;
                    changedIcon = icon;
                    if(!dim.IsZeroLength)
                    {
                        iconElement.Width = dim.x;
                        iconElement.Height = dim.y;
                       
                    } else
                    {
                       
                        iconElement.Width = iconDefaultDimesions.x;
                        iconElement.Height = iconDefaultDimesions.y;
                       
                    }
                }
            }
            else if(lastIconTexture != null)
            {
                
                changedIcon = "radio.png";
                lastIconTexture = null;
                iconElement.Width = iconDefaultDimesions.x;
                iconElement.Height = iconDefaultDimesions.y;
            }
           
            if(changedIcon != null)
            {
                try
                {
                    MaterialPtr mat = MaterialManager.Singleton.GetByName("MessageBarIcon");
                    if (iconFrames > 1)
                    {
                        mat.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetAnimatedTextureName(changedIcon, iconFrames, iconAnimationDuration);
                     //   mat.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(null);
                    }else
                    {
                      //  mat.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetAnimatedTextureName(null, 1);
                        mat.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(changedIcon);
                    }
                    
                }
                catch (Exception)
                {
                }
               
            }

           


            currentMessage.IncreaseX(XMargin);
            currentMessage.IncreaseY(YMargin);
            
            iconOverlay.Hide();
            
            
            if (!iconOverlay.IsVisible)
            {
				float iconYPos = currentMessage.Y - iconElement.Height * 0.5f; // ikonka w jedym poziomie z tekstem
	            
	            if(iconYPos <= yMargin) {
	            	iconYPos = currentMessage.Y + currentMessage.CharHeight * 0.25f; // przyklej ikonke do rogu ekranu
	            }            	
                iconElement.SetPosition((currentMessage.X), iconYPos);
            }

            messageContainer.SetPosition(iconElement.Width + currentMessage.X, currentMessage.Y);
            messageElement.SetParameter("char_height", currentMessage.getCharHeightString());
            messageElement.SetParameter("colour_top", currentMessage.ColourTopString);
            messageElement.SetParameter("colour_bottom", currentMessage.ColourBottomString);
          

            messageElement.Caption = currentMessage.GetMessageAdjustedByContainerWidth(mainViewport.ActualWidth * 0.8f, OverlayManager.Singleton.ViewportHeight);
           // messageElement.Caption = AbstractScreen.Wrap(currentMessage.Message, currentMessage.CharsPerLine); ;
            
        }

        private string lastIconTexture; 

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
            else return null;
        }
        
        public void ClearMessages(String messageTxt)
        {
            if (PeekMessage()  != null && PeekMessage().Equals(messageTxt))
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
        
        public void ClearMessages()
        {
         	ClearMessage();
         	messageQueue.Clear();
            
        }

        public void ClearMessage()
        {
            //startTime = null;
            currentMessage = null;
            if (messageElement != null)
            {
                messageElement.Caption = "";
            }
            if(iconOverlay != null && iconOverlay.IsVisible)  iconOverlay.Hide();
            isDecreasingBgOpacity = false;
            isIncreasingBgOpacity = false;
            if (backgroundElement != null) backgroundElement.Hide();
        }


        public void DestroyMessageContainer()
        {

            try
            {
                MaterialPtr mat = MaterialManager.Singleton.GetByName("MessageBarIcon");
                mat.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName("radio.png");
            }
            catch (Exception)
            {
            }

            if (messageOverlay != null)
            {
                messageOverlay.Hide();
                messageOverlay.Dispose();
                messageOverlay = null;
            }

            if(backgroundElement != null)
            {
                backgroundElement.Hide();
                backgroundElement.Dispose();
                backgroundElement = null;
            }

            messageContainer.Hide();

            OverlayManager.Singleton.DestroyOverlayElement(messageElement);
            messageElement = null;
            messageContainer.Dispose();
            messageContainer = null;
            
            if(iconOverlay != null)
            {
            	iconElement.Hide();
            	iconElement.Dispose();
	            iconElement = null;
	            iconOverlay.Hide();
	            iconOverlay.Dispose();
	            iconOverlay = null;
            }
           

            
        }
    }
}