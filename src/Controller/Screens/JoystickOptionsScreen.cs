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


using System.Collections.Generic;
using MOIS;
using BetaGUI;
using Mogre;
using Wof.Controller.Input.KeyboardAndJoystick;
using Wof.Languages;
using Wof.Misc;

namespace Wof.Controller.Screens
{
    class JoystickOptionsScreen : AbstractOptionsScreen, BetaGUIListener
    {
    	protected readonly JoystickChangerHelper joystickChangerHelper;
		bool controlsCaptureStarted = false;
		
		protected IList<JoyStick> joysticks;
		
		float joystickWindowsInitialY;
        readonly List<Window> joystickWindows = new List<Window>();
        readonly List<Window> joystickPointers = new List<Window>();
        readonly List<int> joystickPointerAxis = new List<int>();
        
        const int resetButtonID = 1234;
		
        public JoystickOptionsScreen(GameEventListener gameEventListener,
                                      IFrameWork framework, Viewport viewport, Camera camera, Keyboard keyboard, IList<JoyStick> joysticks) :
                                         base(gameEventListener, framework, viewport, camera)
        {
			this.joysticks = joysticks;
			
    		C_MAX_OPTIONS = 5;
    		showRestartRequiredMessage = false;  
    		autoGoBack = false;
    		
    		    		
    		joystickChangerHelper = new JoystickChangerHelper(keyboard, joysticks, this);
    		joystickChangerHelper.onControlsChanged += controlsChangerHelper_onControlsChanged;
    		joystickChangerHelper.onControlsCaptureStarted += controlsChangerHelper_onControlsCaptureStarted;
			joystickChangerHelper.onControlsCaptureEnded += controlsChangerHelper_onControlsCaptureEnded;
        	joystickChangerHelper.onChangeButtonAdded += joystickChangerHelper_onChangeButtonAdded;
        }

		void joystickChangerHelper_onChangeButtonAdded(Button button)
		{
			Button[] arr = new Button[buttons.Length +1];
			buttons.CopyTo(arr, 0);
			arr[arr.Length -1] = button;			
			buttons = arr;
			buttonsCount++;
			
			options.Add(new ButtonHolder(button, button.text));
			
		}

		void controlsChangerHelper_onControlsChanged()
		{
			RecreateGUI();
			return;
		}
		
		void controlsChangerHelper_onControlsCaptureStarted()
		{
			// todo
			controlsCaptureStarted = true;
			skipHandlingGuiButtons = true;
			return;
			
		}
		
		void controlsChangerHelper_onControlsCaptureEnded()
		{
			controlsCaptureStarted = false;
			skipHandlingGuiButtons = false;
			return;
		}
		
		
  		protected override string getTitle()
        {
            return LanguageResources.GetString(LanguageKey.JoystickOptions);
        }

        protected override List<object> GetAvailableOptions()
        {
            List<object> availableModes = new List<object>();                   
            FrameWorkStaticHelper.GetNumberOfAvailableJoysticks();            
            for(int i=0; i< joysticks.Count; i++){            	
            	availableModes.Add(LanguageResources.GetString(LanguageKey.Joystick)+" "+(i+1)+" - ID=" +joysticks[i].Vendor()+"_"+joysticks[i].ID);
			}      
            return availableModes;
        }
  
        public override void RecreateGUI()
		{
         	joystickChangerHelper.Clear();          	
          	base.RecreateGUI();
		}

        protected override void ProcessOptionSelection(ButtonHolder holder)
        {        	
        	string jid;
        	if(holder.Option.id.Equals(resetButtonID)){
				KeyMap.Instance.BackToDefaults(joysticks);
				KeyMap.Instance.Value = KeyMap.Instance.Value;
				//FrameWorkStaticHelper.SetCurrentJoystickIndex(0);
				
				for(int i = 0; i <FrameWorkStaticHelper.GetNumberOfAvailableJoysticks(); i++) {
					jid= joysticks[i].Vendor()+"_"+joysticks[i].ID;
  					
					if( jid.Equals(KeyMap.Instance.CurrentJoystick)) {  						
						FrameWorkStaticHelper.SetCurrentJoystickIndex(i);  
						break;
					}
				}
				joystickChangerHelper.UpdateCurrentJoystick();
				this.RecreateGUI();			
				return;
			}
			
            bool restart = false;
			string[] opts = holder.Value.Split(new string[]{"ID="}, System.StringSplitOptions.RemoveEmptyEntries);
			
			jid = opts[1];
			
			KeyMap.Instance.CurrentJoystick = jid;
			KeyMap.Instance.Value = KeyMap.Instance.Value;
			
			for(int i=0; i< joysticks.Count; i++){    
				if(jid.Equals(joysticks[i].Vendor()+"_"+joysticks[i].ID)) {
					FrameWorkStaticHelper.SetCurrentJoystickIndex(i);
					joystickChangerHelper.UpdateCurrentJoystick();
					
					this.RecreateGUI();			
					break;
				}
			}			
		}
        
       
        protected override bool IsOptionSelected(int index, ButtonHolder holder)
        {
        	string option = holder.Value;
        	if(holder.Option.id.Equals(resetButtonID)){
        		return false;
        	}       
        	
        	if( FrameWorkStaticHelper.GetNumberOfAvailableJoysticks() == 1 && index == 0) return true;
        	
			string[] opts = option.Split(new string[]{"ID="}, System.StringSplitOptions.RemoveEmptyEntries);			
			string jid = opts[1];
						
			return jid.Equals(KeyMap.Instance.CurrentJoystick);
        	
        }
		
  		public override void onButtonPress(Button referer)
        {
  			if(controlsCaptureStarted) {
  				return;
  			}  			
  			base.onButtonPress(referer);  
  			if(referer.text.Contains("ID=")) {				
  				RecreateGUI();			
  			}  			
  		}
  		
  		protected override Vector4 GetOptionPos(uint index, Window window)
        {
            return new Vector4(0, (index + 2) * GetTextVSpacing(), window.w, GetTextVSpacing());
        }

		void CreateJoystickPointerControl(Window parentWindow, float y, int axisNo)
		{
			ColourValue cv;		   
			string windowMaterial = "bgui.window";
			if(axisNo == KeyMap.Instance.JoystickHorizontalAxisNo || axisNo == KeyMap.Instance.JoystickVerticalAxisNo) {
		    	cv = new ColourValue(1.0f, 0.8f, 0.0f);
		    	windowMaterial =  "bgui.window.yellow";
		    }else {
		    	cv = ColourValue.White;
		    }
			float pointerWindowSize = parentWindow.w * 0.4f;
						
			int yshift = (int)(joystickWindows.Count * (0.85f*GetTextVSpacing()));
			
			var pos = new Vector4(parentWindow.x + 0.5f*(parentWindow.w  - pointerWindowSize  ), parentWindow.y + y + GetTextVSpacing() + yshift,  pointerWindowSize, 0.75f*GetTextVSpacing());
			joystickWindows.Add(mGui.createWindow(pos, windowMaterial, (int)wt.NONE, ""));
		    int index =joystickWindows.Count-1;
		    
		    pos.x = GetTextVSpacing(); // left
		    pos.y -= parentWindow.y;
		    pos.w = GetTextVSpacing(); // height
		    
		    
		    
		    parentWindow.createStaticText(pos, LanguageResources.GetString(LanguageKey.JoystickAxis)+ " "+ axisNo, cv);
			
			joystickPointerAxis.Add(axisNo);
			joystickPointers.Add(mGui.createWindow(getJoystickPointerRect(joystickWindows[index], axisNo), "bgui.window.pointer", (int)wt.NONE, ""));
			
			
		}

		void CreateJoystickPointerControls(Window window)
		{
			joystickWindows.Clear();
			joystickPointers.Clear();
			joystickPointerAxis.Clear();
			for (int z = 0; z < FrameWorkStaticHelper.GetCurrentJoystick(joysticks).JoyStickState.AxisCount; z ++) {
				CreateJoystickPointerControl(window, joystickWindowsInitialY, z);			
			}
		}
  	
		
        protected override void LayoutOptions(List<object>availableOptions, Window window, Callback cc)
        {   
        	
          	uint oldFontSize = fontSize;
          
          	SetFontSize((uint)(fontSize * 0.8f));
          	mGui.mFontSize = fontSize;
          		
            base.LayoutOptions(availableOptions, window, cc); 
  			SetFontSize(oldFontSize);		
  			mGui.mFontSize  = oldFontSize;
  			
            if(FrameWorkStaticHelper.GetNumberOfAvailableJoysticks()==0) {
            	
            	float imgSize = fontSize * 10;
            	window.createStaticImage(new Vector4((window.w - imgSize) * 0.5f, (window.h - imgSize) * 0.5f , imgSize, imgSize), "nojoysticks.png");
            	return;
            }
            float h = GetTextVSpacing();
            //totalOptions
            float y = joystickChangerHelper.AddControlsInfoToGui(guiWindow, mGui, (int) h, (int)(GetTextVSpacing() * 0.8f * (availableOptions.Count + 3)), 0, viewport.ActualWidth * 0.75f, 0.75f* GetTextVSpacing(),(uint)(GetFontSize() * 0.75f));
			
            
			y += (int)(h*1);
			
			const string txt = "Reset";
			var txtSize = ViewHelper.MeasureText(mGui.mFont, txt, fontSize);
			
            var pos = new Vector4(h, y, txtSize, h);            
			var resetButton = guiWindow.createButton(pos, "bgui.button", txt, new Callback(this), resetButtonID);
			joystickChangerHelper_onChangeButtonAdded(resetButton);
			
			y += (int)(h*0.5f);
			
			joystickWindowsInitialY = y;			
			
			
			CreateJoystickPointerControls(window);
			selectButton(currentButton);
			
           // mGui.mFontSize = oldFontSize;
           
              // joystick info
	       // var txt = "Current joystick reading: "+ FrameWorkStaticHelper.GetJoystickVector(joysticks, false);
           // guiWindow.createStaticText(pos, txt); 
           
           
            

        }
       
        
        protected Vector4 getJoystickPointerRect(Window joystickWindow, int axisNo) {
        	float value = FrameWorkStaticHelper.GetJoystickVector(joysticks, false, axisNo);
        	//vvector.y *= EngineConfig.InverseKeys ? -1.0f : 1.0f;
        	
            float pointerSize = joystickWindow.h;
            
            Vector4 pos = new Vector4(joystickWindow.x, joystickWindow.y,pointerSize,pointerSize);
            pos.x += joystickWindow.w * 0.5f + (joystickWindow.w * 0.5f - pointerSize * 0.5f) * value - pointerSize * 0.5f;
            pos.y += joystickWindow.h * 0.5f - pointerSize * 0.5f;
          
            return pos;
        }
        
         public override void FrameStarted(FrameEvent evt)
         {
         	
         	base.FrameStarted(evt);
         	
         	int i = 0;
         	foreach(var jp in joystickPointers) {         
         		var newPos = getJoystickPointerRect(joystickWindows[i], joystickPointerAxis[i]);
         		jp.SetRect(newPos);
         		i++;
         		
         	}
         
           
         	
         }

      

      

       
    }
}