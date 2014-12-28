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
using System.Collections;
using System.Collections.Generic;
using MOIS;
using Mogre;
using BetaGUI;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.Misc;

namespace Wof.Controller.Screens
{
	/// <summary>
	/// Description of ControlsChangerHelper.
	/// </summary>
	public class ControlsChangerHelper : AbstractChangerHelper
	{ 
		
		#region implemented abstract members of AbstractChangerHelper

		protected override void OnChangeButtonAddedDo(Button b)
		{			
			if (onChangeButtonAdded != null) {
				onChangeButtonAdded(b);
			}
			
		}

		#endregion

	
		public delegate void OnControlsChanged();		
		public event OnControlsChanged onControlsChanged;
		
		public delegate void OnControlsCaptureStarted();		
		public event OnControlsCaptureStarted onControlsCaptureStarted;
		
		public delegate void OnControlsCaptureEnded();		
		public event OnControlsCaptureEnded onControlsCaptureEnded;
		
		public delegate void OnChangeButtonAdded(Button button);		
		public event OnChangeButtonAdded onChangeButtonAdded;
		
		public ControlsChangerHelper(Keyboard keyboard, MenuScreen parent) : base(keyboard, parent) {
			
			onControlsCaptureStarted += ControlsChangerHelper_onControlsCaptureStarted;
			onControlsCaptureEnded += ControlsChangerHelper_onControlsCaptureEnded;
	    	
		}
		
		protected void ActivateKeyboard()
		{
			keyboard.KeyPressed+= keyboard_KeyPressed;
			keyboard.KeyReleased += keyboard_KeyReleased;			
		}
		
		protected void DisableKeyboard()
		{
			keyboard.KeyPressed-= keyboard_KeyPressed;
			keyboard.KeyReleased -= keyboard_KeyReleased;
		}
		
		bool keyboard_KeyPressed(KeyEvent arg)
		{
			return true;
		}

		bool keyboard_KeyReleased(KeyEvent arg)
		{
			if(arg.key.Equals(KeyCode.KC_ESCAPE)){
				CloseControlChangeWindow();
				return false;
			}
						
			String langKey = GetLanguageKeyById(currentKeyId);
			
			// check for conflicts
			
			if(!langKey.Equals(LanguageKey.Pitch) && !langKey.Equals(LanguageKey.AccelerateBreakTurn)) {
				
				if(KeyMap.CheckKeyCodeConflict(langKey, arg.key, TypeOfControl.Keyboard)) {
					// handle
					return true;
				}
				
			}
			
			// keys
			if(langKey.Equals(LanguageKey.Engine)){
				KeyMap.Instance.Engine = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Spin)){
				KeyMap.Instance.Spin = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Gear)){
				KeyMap.Instance.Gear = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Gun)){
				KeyMap.Instance.Gun = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Bombs)){
				KeyMap.Instance.Bombs = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Camera)){
				KeyMap.Instance.Camera = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Zoomin)){
				KeyMap.Instance.ZoomIn = arg.key;				
			}
			if(langKey.Equals(LanguageKey.Zoomout)){
				KeyMap.Instance.ZoomOut = arg.key;				
			}
			if(langKey.Equals(LanguageKey.BulletTimeEffect)){
				KeyMap.Instance.BulletTimeEffect = arg.key;				
			}
			// pitch
			if(langKey.Equals(LanguageKey.Pitch)){
				
				if(twoStep == 1) {	
					if(KeyMap.CheckKeyCodeConflict("Up", arg.key, TypeOfControl.Keyboard, new [] {"Down"})) {						
						return true;
					}
					twoStep++;
					KeyMap.Instance.Up = arg.key;	
					controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN,  GUI.OVERLAY_TITLE_MARGIN + parentGui.mFontSize*2, controlChangeWindow.w, parentGui.mFontSize ), KeyMap.Instance.Up.ToString(), new ColourValue(0.9f, 0.7f, 0.0f));
					controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN,  GUI.OVERLAY_TITLE_MARGIN + parentGui.mFontSize*3, controlChangeWindow.w, parentGui.mFontSize ), LanguageResources.GetString(LanguageKey.Pitch) + " (2/2)");
					return true; // only first step
				}else {
				  	twoStep = 1;
				  	if(KeyMap.CheckKeyCodeConflict("Down", arg.key, TypeOfControl.Keyboard, new [] {"Up"})) {						
						return true;
					}
				  	KeyMap.Instance.Down = arg.key;					  
				  	// finish
				}
				
			}
				
			if(langKey.Equals(LanguageKey.AccelerateBreakTurn)){
				
				if(twoStep == 1) {	
					if(KeyMap.CheckKeyCodeConflict("Left", arg.key, TypeOfControl.Keyboard, new [] {"Right"})) {						
						return true;
					}
					twoStep++;
					KeyMap.Instance.Left = arg.key;	
					controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN,GUI.OVERLAY_TITLE_MARGIN + parentGui.mFontSize*2, controlChangeWindow.w, parentGui.mFontSize ), KeyMap.Instance.Left.ToString(), new ColourValue(0.9f, 0.7f, 0.0f));
					controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN, GUI.OVERLAY_TITLE_MARGIN +  parentGui.mFontSize*3, controlChangeWindow.w, parentGui.mFontSize ), LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + " (2/2)");
					

					return true; // only first step
				}else {
				  	twoStep = 1;
				  	if(KeyMap.CheckKeyCodeConflict("Right", arg.key, TypeOfControl.Keyboard, new [] {"Left"})) {
						// handle
						return true;
					}
				  	KeyMap.Instance.Right = arg.key;					  
				  	// finish
				}
				
			}		
		
			KeyMap.Instance.Value = KeyMap.Instance.Value;
			
			CloseControlChangeWindow();
			
			// notify parent to refresh
			if(onControlsChanged != null){
				onControlsChanged();
			}
			return true;
		}
		
		int twoStep = 1;

		void ControlsChangerHelper_onControlsCaptureStarted()
		{
			capturingKeys = true;
		}

		void ControlsChangerHelper_onControlsCaptureEnded()
		{
			capturingKeys = false;
		}
		
		
		
	    protected override void DisplayControlChangeWindow(uint id)
		{
			 if(onControlsCaptureStarted != null){
				onControlsCaptureStarted();
			 }
			  ActivateKeyboard();
			  string caption = "";
			  currentKeyId = id;
			 
			
			  caption += LanguageResources.GetString( GetLanguageKeyById(id));
			
			  caption += " = ... ";			  
			  //LanguageResources.GetString(LanguageKey.Pause);
			  
			  
              float width = ViewHelper.MeasureText(parentGui.mFont, caption+" (1/2) ", parentGui.mFontSize);
			  
			  
              controlChangeWindow = parentGui.createWindow(new Vector4(parentGuiWindow.x + (parentGuiWindow.w - width)/2.0f,parentGuiWindow.y+ parentGuiWindow.h/4,width,parentGuiWindow.h/2),
                                          "bgui.window", (int) wt.NONE, caption);
			  
			  if( GetLanguageKeyById(id).Equals(LanguageKey.Pitch)) {
			  	  controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN, parentGui.mFontSize +GUI.OVERLAY_TITLE_MARGIN, controlChangeWindow.w, parentGui.mFontSize ), LanguageResources.GetString(LanguageKey.Pitch) + " (1/2)");
					
			  }
			    if( GetLanguageKeyById(id).Equals(LanguageKey.AccelerateBreakTurn)) {
			  	  controlChangeWindow.createStaticText(new Vector4(GUI.OVERLAY_TITLE_MARGIN, parentGui.mFontSize + GUI.OVERLAY_TITLE_MARGIN, controlChangeWindow.w, parentGui.mFontSize ), LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + " (1/2)");
					
			  }
			  controlChangeWindow.show();
        
		}
		
		protected override void CloseControlChangeWindow()
		{		
			DisableKeyboard();
		//	controlChangeWindow.hide();		
			parentGui.killWindow(controlChangeWindow);			
			controlChangeWindow = null;
			if(onControlsCaptureEnded != null){
				onControlsCaptureEnded();
			}
		}
		
		
		#region BetaGUIListener implementation
		public override void onButtonPress(Button referer){
		
			// wylacz obsluge klawiatury w oknie powyzej
			// pokaz okno, czekaj na guzik
			if(capturingKeys) {
				return;
			}
			DisplayControlChangeWindow(referer.id);
			
		}
		#endregion		
		
			
	
		public override int AddControlsInfoToGui(Window guiWindow, GUI mGui, int left, int top, int initialTopSpacing, float width, float textVSpacing, uint fontSize)
        {
        	
        	int y = initialTopSpacing;
        	int h = (int)textVSpacing;
            uint oldFontSize = mGui.mFontSize;

            int leftOrg = left;
            
            // przesuniecie o szerokosc guzika (fontsize) + fontSize
            left += (int)(fontSize + fontSize);
            
            //imgVDiff = 0;
          

            OverlayContainer c;
            y += (int)(h*1);
            c = guiWindow.createStaticText(new Vector4(left - 10, top + y, width, h), LanguageResources.GetString(LanguageKey.Controls));
            AbstractScreen.SetOverlayColor(c, new ColourValue(1.0f, 0.8f, 0.0f), new ColourValue(0.9f, 0.7f, 0.0f));

          
            mGui.mFontSize = fontSize;
            float spaceSize = ViewHelper.MeasureText(mGui.mFont, " ", mGui.mFontSize);
            float imgSize = fontSize;
            float imgVDiff = - Mogre.Math.Abs(imgSize - fontSize)*0.5f;

            y += (int)(h*1.5f);
			var pos = new Vector4(left, top + y, width, h);
            c =
                guiWindow.createStaticText(pos,
				LanguageResources.GetString(LanguageKey.Engine) + ": " + KeyMap.GetName(KeyMap.Instance.Engine) + " (" + LanguageResources.GetString(LanguageKey.Hold) + ")");
           
            
            Setup(mGui, guiWindow);          
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Engine );
            
            // "Engine: E (hold)");
            y += (int)(h*1);

            
            AddChangeButton(new Vector2(leftOrg, top + y), fontSize, LanguageKey.AccelerateBreakTurn);
            
            if(KeyMap.Instance.Left == KeyCode.KC_LEFT && KeyMap.Instance.Right == KeyCode.KC_RIGHT )
            { 
                string ctrl1 = LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + ": ";
                float width1 = ViewHelper.MeasureText(mGui.mFont, ctrl1, mGui.mFontSize);

            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h), ctrl1);


                guiWindow.createStaticImage(new Vector4(left + width1, top + y + imgVDiff, imgSize, imgSize), "arrow_left.png");
                guiWindow.createStaticImage(new Vector4(left + width1 + imgSize + spaceSize * 0.5f, top + y + imgVDiff, imgSize, imgSize), "arrow_right.png");
            } else
            { 
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
            		                           LanguageResources.GetString(LanguageKey.AccelerateBreakTurn) + ": " + KeyMap.GetName(KeyMap.Instance.Left) + "/" +  KeyMap.GetName(KeyMap.Instance.Right));
            	
            }
           

            y += (int)(h*1); 
            
            AddChangeButton(new Vector2(leftOrg, top + y),fontSize, LanguageKey.Pitch);
            
            
            if(KeyMap.Instance.Up == KeyCode.KC_UP && KeyMap.Instance.Down == KeyCode.KC_DOWN )
            {
                string ctrl2 = LanguageResources.GetString(LanguageKey.Pitch) + ": ";
                float width2 = ViewHelper.MeasureText(mGui.mFont, ctrl2, mGui.mFontSize);

            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h), ctrl2);

                guiWindow.createStaticImage(new Vector4(left + width2,                              top + y + imgVDiff, imgSize, imgSize), "arrow_up.png");
                guiWindow.createStaticImage(new Vector4(left + width2 + imgSize + spaceSize * 0.5f, top + y + imgVDiff, imgSize, imgSize), "arrow_down.png");

            } else
            {
            	c =
                guiWindow.createStaticText(new Vector4(left, top + y, width, h),
                                           LanguageResources.GetString(LanguageKey.Pitch) + ": "  + KeyMap.GetName(KeyMap.Instance.Up) + "/" +  KeyMap.GetName(KeyMap.Instance.Down));
            }



            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,  
                                           LanguageResources.GetString(LanguageKey.Spin) + ": " + KeyMap.GetName(KeyMap.Instance.Spin));
            
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Spin);
            
            
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos, 
                                           LanguageResources.GetString(LanguageKey.Gear) + ": " + KeyMap.GetName(KeyMap.Instance.Gear));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Gear);
            

            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos, 
                                           LanguageResources.GetString(LanguageKey.Gun) + ": " + KeyMap.GetName(KeyMap.Instance.Gun));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Gun);
            
            y += (int)(h*1);
			pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.Bombs) + "/" + LanguageResources.GetString(LanguageKey.Rockets)+ ": " + KeyMap.GetName(KeyMap.Instance.Bombs));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Bombs);
            
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.Camera) + ": " + KeyMap.GetName(KeyMap.Instance.Camera));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Camera);
            
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.Zoomin) + ": " +  KeyMap.GetName(KeyMap.Instance.ZoomIn));
 			AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Zoomin);
           
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.Zoomout) + ": " +  KeyMap.GetName(KeyMap.Instance.ZoomOut));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.Zoomout);
            
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.BulletTimeEffect) + ": " +  KeyMap.GetName(KeyMap.Instance.BulletTimeEffect));
            AddChangeButton(new Vector2(leftOrg, pos.y),fontSize, LanguageKey.BulletTimeEffect);
            
            
            y += (int)(h*1);
            pos = new Vector4(left, top + y, width, h);
            c = guiWindow.createStaticText(pos,
                                           LanguageResources.GetString(LanguageKey.RearmEndMission) + ": " + KeyMap.GetName(KeyMap.Instance.Bombs));
            

            mGui.mFontSize = oldFontSize;
            
            return y+top;
        }
	
	}
}
