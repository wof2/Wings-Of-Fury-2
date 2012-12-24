/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-11-05
 * Time: 12:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using BetaGUI;
using Mogre;
using Wof.Misc;
using Wof.Model.Level;

namespace Wof.Controller.Indicators
{
	/// <summary>
	/// Description of AchievementIcon.
	/// </summary>
	public class AchievementIcon : IDisposable
	{
		
		protected Achievement achievement;
		protected Window achievementsWindow;
	    protected Viewport viewport;
		
		protected OverlayContainer textContainer = null;
		protected OverlayContainer imageContainer = null;
		protected OverlayContainer imageContainerFulfilled = null;
		
		
		public AchievementIcon(Achievement a, Window achievementsWindow, Viewport viewport)
		{
			this.achievement = a;
			this.achievementsWindow = achievementsWindow;
		    this.viewport = viewport;
		}
		
		public void Update(int index) {
			string achString = achievement.AmountDone +"/"+ achievement.Amount;
			
			uint h = achievementsWindow.mGUI.mFontSize;
		    uint margin = h/2;
		   // float hsize = 6*h;

            float dist = viewport.ActualWidth / 4.5f;
            float hsize = dist / 5.0f;
            
      
			uint totalh = (uint)achievementsWindow.h;
			DisposeTextContainer();
            float textWidth = ViewHelper.MeasureText(achievementsWindow.mGUI.mFont, achString, h);

            float textAlign = (hsize - textWidth) / 2.0f;
            textContainer = achievementsWindow.createStaticText(new Vector4((index) * hsize + margin + textAlign, hsize + h, hsize, hsize), achString, MessageEntry.DefaultColourTop, MessageEntry.DefaultColourBottom);

			if(imageContainer == null){
	        		string filename = achievement.GetImageFilename();
                    imageContainer = achievementsWindow.createStaticImage(new Vector4((index) * hsize + margin, h, hsize, hsize), filename, false);
        	}
			
         	if(achievement.IsFulfilled()) {
        	
        		if(imageContainerFulfilled == null){	        		
	        		//DisposeImageContainer();
        			string filename = achievement.GetFulfilledImageFilename();
                    imageContainerFulfilled = achievementsWindow.createStaticImage(new Vector4((index) * hsize + margin, h, hsize * 0.5f, hsize * 0.5f), filename, true);
        		}
        	}
			
		}
		
		//public void Di
		
		private void DisposeImageContainer() {
			
			if(imageContainer != null) {			
				imageContainer.Hide();
				achievementsWindow.mI.Remove(imageContainer);
				achievementsWindow.mO.RemoveChild(imageContainer.Name);
				OverlayManager.Singleton.DestroyOverlayElement(imageContainer);
                
				imageContainer.Dispose();		
				imageContainer = null;				
			}
		}
		
		private void DisposeFullfilledImageContainer() {
			if(imageContainerFulfilled != null) {			
				imageContainerFulfilled.Hide();
				achievementsWindow.mI.Remove(imageContainerFulfilled);
				achievementsWindow.mO.RemoveChild(imageContainerFulfilled.Name);
				OverlayManager.Singleton.DestroyOverlayElement(imageContainerFulfilled);
                
				imageContainerFulfilled.Dispose();		
				imageContainerFulfilled	= null;	
			}
			
		}
		
		private void DisposeTextContainer() {
			if(textContainer != null) {
				textContainer.Hide();
				achievementsWindow.mO.RemoveChild(textContainer.Name);
			 	OverlayManager.Singleton.DestroyOverlayElement(textContainer);
                
				
				textContainer.Dispose();
				textContainer = null;				
			}
			
		}
		
		public void Dispose()
		{
			DisposeFullfilledImageContainer();
			DisposeImageContainer();
			DisposeTextContainer();
		}
	}
}
