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
using System.IO;
using System.Text.RegularExpressions;
using AdManaged;
using Mogre;
using Wof.Controller.AdAction;
using Wof.Languages;
using Wof.Misc;
using Wof.Model.Level.Common;
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using FontManager=Mogre.FontManager;

namespace Wof.Controller.Screens
{
    /// <summary>
    /// <author>Adam Witczak</author>
    /// </summary>
    internal class IntroScreen : AbstractScreen
    {
       

    	public const float C_INTRO_AD_PROBABILITY = 1.0f;
        public const String C_TEXTURE_NAME = "IntroScreen";
        private readonly Overlay overlay;

        private int currentScreen;
        private int maxScreens;
        private DateTime lastChange;
        private EffectTextureAnimation animation;
        
        private bool wasInitialized = false;

        /// <summary>
        /// Czas animacji (w sek) zwi¹zanych z poszczególnymi screenami
        /// </summary>
        private float[] screenTimes = { 2.1f, 1.5f};//, 2.5f, 2.0f };

        /// <summary>
        /// Minimalny czas (w sek) przez jaki screen musi byæ na ekranie
        /// </summary>
        private float[] screenMinTimes = {2.0f, 1.5f};// , 1.0f, 1.0f };


        string currentMaterialName; 
      

        private Pair<uint, uint> textureDimensions;



        /// <summary>
        /// Czy screen jest reklam¹
        /// </summary>
        private bool[] isScreenAnAd = { true, true, false, false };


        public const string C_AD_ZONE = "pregame";
        public const string C_AD_MATERIAL = "AdMaterial";
        private AdManager.Ad currentAd = null;
		Queue<int> adIds = new Queue<int>();
       
       
        private int getFirstNonAdIndex()
        {
            int i = 1;
            foreach (bool b in isScreenAnAd)
            {
                if(!b)
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        public IntroScreen(GameEventListener gameEventListener,
                               IFrameWork framework, Viewport viewport, Camera camera)
            :
                                  base(gameEventListener, framework, viewport, camera)
        {

            currentScreen = 0;
          //  textureDimensions = new Pair<uint, uint>(1280,1024); // default

            overlay = OverlayManager.Singleton.GetByName("Wof/Intro");
         
           
            
            //int n = 1;
            int firstN = getFirstNonAdIndex();
            int n = firstN;
           
            while (
                MaterialManager.Singleton.ResourceExists(C_TEXTURE_NAME + n))
            {
                // preload
                MaterialManager.Singleton.GetByName(C_TEXTURE_NAME + n).Load();
                n++;
            }

            maxScreens = screenTimes.Length;// n - 1;

           
            
            initAdScreens();
           
        }

       


        


        public override void CreateScene()
        {
            
        }

        public override void CreateOcean()
        {
            
        }
        public override void FrameStarted(FrameEvent evt)
        {
            base.FrameStarted(evt);
            AdManager.Singleton.Work(null);
            
            if(!wasInitialized)
            {
            	wasInitialized = true;
            	NextScreen();
            
            }else
            {
            	// jeœli screen jest wystarczaj¹co d³ugo na ekranie - przewijamy
	            TimeSpan diff = DateTime.Now.Subtract(lastChange);
	            if (diff.TotalSeconds > screenTimes[currentButton])
	            {
	                NextScreen();
	            }
            	
            }
            
           
          
          

        }

        protected override void CreateSkybox()
        {
            sceneMgr.AmbientLight = new ColourValue(1.0f, 1.0f, 1.0f);
            sceneMgr.ShadowFarDistance = 1000;
            sceneMgr.ShadowColour = new ColourValue(0.8f, 0.8f, 0.8f);
        }

        protected override void CreateGUI()
        {
           /* if (!initScreen(currentScreen))
            {
                // nie udalo sie?
                NextScreen();
            }
           
            overlay.Show();*/
        }


        private void initAdScreens() 
        {
  			/*      	
        	int i = 1;
        	foreach(bool ad in isScreenAnAd)         	
        	{
        		if(ad) 
        		{
        			int id = 0;
        			// pobierz i ustaw na bie¿ac¹
                	AdManager.AdStatus status = AdManager.Singleton.GetAdAsync(C_AD_ZONE, 1.0f, out id);
                	if (status == AdManager.AdStatus.OK)
	                {
	                	adIds.Enqueue(id);	              
	        		}
        			i++;
        		}
        	}
        	*/
        }
        
      
        	
        private bool initScreen(int i)
        {            
            MaterialPtr overlayMaterial = null;
            TextureUnitState unit;
            animation = null;
            currentMaterialName = null;
            if (isScreenAnAd[i - 1]) // poczatkowo i = 1
            {
            	if(Mogre.Math.RangeRandom(0, 1) < (1 - C_INTRO_AD_PROBABILITY))
            	{
            		return false;            		
            	}
            	showAdText(viewport);
	            	
            	//if(adIds.Count == 0) return false;
            	AdManager.AdStatus status = AdManager.Singleton.GetAd(C_AD_ZONE, 1.0f, out currentAd);
               
            	//AdManager.AdStatus status = AdManager.Singleton.GatherAsyncResult(adIds.Dequeue(), AdManager.C_AD_DOWNLOAD_TIMEOUT, out currentAd);
                if (status == AdManager.AdStatus.OK)
                {
                    // pobieranie OK.
                    currentMaterialName = C_AD_MATERIAL;
                    string path = AdManager.Singleton.LoadAdTexture(currentAd);
                    if(path == null)
                    {
                       return false;
                    }
                    OverlayManager.Singleton.GetByName("Wof/AdText").Show();
                    
                  

                    overlayMaterial = MaterialManager.Singleton.GetByName(currentMaterialName);
                    overlayMaterial.Load();
                    unit = overlayMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0);

                    unit.SetTextureName(path);
                    AdManager.Singleton.RegisterImpression(currentAd);

                    //   int count;
                    //   count = adAction.Get_Ad_Impression_Counter(currentAd.id);
                    //    Console.WriteLine("Pobrañ: " + count);
                  
                }else
                {
                	hideAdText();
                    return false;
                }
            	
            }
            else
            {
            	
                currentMaterialName = C_TEXTURE_NAME + currentScreen;
                overlayMaterial = MaterialManager.Singleton.GetByName(currentMaterialName);
                unit = overlayMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
                hideAdText();
            }

            if(i == 3)
            {
                SoundManager3D.Instance.PlayAmbient("sounds/raven.wav", EngineConfig.SoundVolume, false, false);
            }

            textureDimensions = unit.GetTextureDimensions();
            PointD scale = new PointD(1,1);
            // skaluj overlay tak aby tekstury nie zmienia³y swoich proporcji
            float prop = 1.0f;
            if(isScreenAnAd[currentScreen - 1])
            {
                // reklamy maja zachowac oryginalna rozdzielczosc 
                scale = AdSizeUtils.ScaleAdToDisplay(textureDimensions, new PointD(Viewport.ActualWidth, Viewport.ActualHeight), true);
                scale = 0.65f * scale;
            }
            else
            {
                prop = 1.0f / ((1.0f * textureDimensions.first / textureDimensions.second) / (1.0f * Viewport.ActualWidth / Viewport.ActualHeight));
            }
            overlay.SetScale(scale.X, scale.Y * prop);
           


          
            animation =
                       new EffectTextureAnimation(null, unit, screenTimes[i - 1], "IntroAnimation",
                                                  VisibilityNodeAnimation.VisibilityType.VISIBLE,
                                                  VisibilityNodeAnimation.VisibilityType.VISIBLE);

            animation.Enabled = true;
            animation.Looped = true;
            OverlayContainer container = overlay.GetChild("Wof/IntroScreen");
            container.MaterialName = currentMaterialName;

            EffectsManager.Singleton.AddCustomEffect(animation);
            lastChange = DateTime.Now;
            return true;
        }
        
      

        private void NextScreen()
        {
           
            lastChange = DateTime.Now;
            currentScreen++;
            if (currentScreen > 1)
            {
                if(currentMaterialName != null && animation != null)
                {
                   
                    AdManager.Singleton.CloseAd(currentAd);
                    AdManager.Singleton.Work(null); // wyslij, na wszelki wypadek
                 
                  
                    MaterialManager.Singleton.Unload(currentMaterialName);
                    EffectsManager.Singleton.RemoveAnimation(animation); 
                }
               
            }
            
            if (currentScreen > maxScreens)
            {
                hideAdText();
                GotoStartScreen();
                return;
            }

            if(!initScreen(currentScreen))
            {
                // nie udalo sie?
                NextScreen();
            }
            else
            {
                overlay.Show();
            }

           
        }

        private void GotoStartScreen()
        {
            overlay.Hide();
            overlay.Dispose();
            gameEventListener.GotoStartScreen();
        }


        public override void MouseReceived(string button)
        {
            if (CanChangeScreen()) NextScreen();
        }

     
        private bool CanChangeScreen()
        {
            TimeSpan diff = DateTime.Now.Subtract(lastChange);
            if (diff.TotalSeconds > screenMinTimes[currentScreen - 1])
            {
                return true;
            }
            return false;
        }

        public override void KeyReceived(string key)
        {
            if ("ESC".Equals(key) && !isScreenAnAd[currentScreen - 1])
            {
                GotoStartScreen();
                
            }
            else
            {
                if(CanChangeScreen()) NextScreen();
               
                
            }
        }
    }
}