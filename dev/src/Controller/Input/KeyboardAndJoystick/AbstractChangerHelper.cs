/*
 * Created by SharpDevelop.
 * User: w
 * Date: 2014-12-26
 * Time: 19:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using MOIS;
using Mogre;
using BetaGUI;
using Wof.Controller.Screens;

namespace Wof.Controller.Input.KeyboardAndJoystick
{
	/// <summary>
	/// Description of AbstractChangerHelper.
	/// </summary>
	public abstract class AbstractChangerHelper  : BetaGUIListener
	{
		protected uint lastButtonId = 1;
		protected MenuScreen parent;
		protected Callback callback;
		protected MOIS.Keyboard keyboard;
		protected Window parentGuiWindow;
		protected Window controlChangeWindow;
	
		
		protected uint currentKeyId;
		protected bool capturingKeys = false;
				
		protected readonly IDictionary<String, uint> identifiers = new Dictionary<String, uint>();
		protected GUI parentGui;
		
		protected AbstractChangerHelper(Keyboard keyboard, MenuScreen parent)
		{
			this.parent = parent;
			this.callback = new Callback(this); 
			this.keyboard = keyboard;
		}
		
		public void Setup(GUI parentGui, Window parentGuiWindow) {
			this.parentGui = parentGui;
			this.parentGuiWindow = parentGuiWindow;
		}
		
		public abstract void onButtonPress(Button referer);

		
		protected abstract void OnChangeButtonAddedDo(Button b);
		
		
		public virtual Button AddChangeButton(Vector2 topLeft, uint buttonSize, String identifier)
        {
			
    	  	// if (holder == null) return;
    	  	uint curId;
    	  	if(identifiers.ContainsKey(identifier)) {
    	  		curId = identifiers[identifier];
    	  	}else {
    	  		curId = ++lastButtonId;
    	  		identifiers[identifier] = curId;
    	  	}
    	  	
			var vector4 = new Vector4(topLeft.x, topLeft.y, buttonSize, buttonSize);
			//guiWindow.createStaticImage(vector4, "gear.png", (ushort)(1100));
         
			
			Button b = parentGuiWindow.createButton(vector4, "bgui.button.gear","" , callback, curId);
			OnChangeButtonAddedDo(b);
			return b;
    	 	 
    	}
		
		protected abstract void DisplayControlChangeWindow(uint id);
		protected abstract void CloseControlChangeWindow();
			
		public abstract int AddControlsInfoToGui(Window guiWindow, GUI mGui, int left, int top, int initialTopSpacing, float width, float textVSpacing, uint fontSize);
		
		protected String GetLanguageKeyById(uint id) {
			String key = null;
			foreach(KeyValuePair<String, uint> o in identifiers) {
				if(o.Value.Equals(id)) {
					key = o.Key;
					break;
				}
			}
			return key;
		}
		
	}
}
