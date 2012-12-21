/// Betajaen's GUI 015, C#/MOgre version by funguine.
/// Written by Robin "Betajaen" Southern 07-Nov-2006, http://www.ogre3d.org/wiki/index.php/BetaGUI
/// This code is under the Whatevar! licence. Do what you want; but keep the original copyright header.
/// 
/// Heavily modified by Adam Witczak 2008

using System;
using System.Collections.Generic;
using Mogre;
using System.Drawing;
using Wof.Controller;
using Wof.Languages;
using Wof.Misc;
using Font=Mogre.Font;
using FontManager=Mogre.FontManager;
using Math=System.Math;

namespace BetaGUI
{
    internal enum wt
    {
        NONE = 0,
        MOVE = 1,
        RESIZE = 2,
        RESIZE_AND_MOVE = 3
    } ;

    public class GUI
    {
        public uint wc, bc, tc, oc;
        public Overlay mO, mMPo;
        public Overlay mOTop;
        public List<Window> WN = new List<Window>();
        public Window mXW;
        public OverlayContainer mMP;
        public String mFont;
        public uint mFontSize;
        public Timer keyDelay = null;

        public String name;

        // by Adam
        private uint oldMouseX = 0;
        private uint oldMouseY = 0;

        public void SetZOrder(ushort zorder)
        {
        	mO.ZOrder = zorder;
        	mOTop.ZOrder =(ushort)(zorder + 1);
        }
        	
        
        public GUI(String font, uint fontSize)
        {
            mXW = null;
            mMP = null;
            mFont = font;
            mFontSize = fontSize;
            wc = 0;
            bc = 0;
            tc = 0;
            oc = 0;
            mO = OverlayManager.Singleton.Create("BetaGUI" + DateTime.Now.Ticks);         
            name = "BetaGUI" + DateTime.Now.Ticks;
            mO.Show();
            
            mOTop = OverlayManager.Singleton.Create("BetaGUI_top" + DateTime.Now.Ticks);         
            mOTop.Show();
            

            //createWindow(new Vector4(0, 0, 0, 0), "", (int)wt.NONE, " "); // hack na pierwsze okno :(
        }

        public GUI(String font, uint fontSize, String name)
        {
            mXW = null;
            mMP = null;
            mFont = font;
            mFontSize = fontSize;
            wc = 0;
            bc = 0;
            tc = 0;
            oc = 0;
            mO = OverlayManager.Singleton.Create(name);
            mOTop = OverlayManager.Singleton.Create(name+"Top");
            
            this.name = name;
            mO.Show();
            mOTop.Show();
        }

        public void hide()
        {
            for (int i = 0; i < WN.Count; i++)
                WN[i].hide();
            mMP.Hide();
        }

        public void show()
        {
            for (int i = 0; i < WN.Count; i++)
                WN[i].show();
            if (mMP != null) mMP.Show();
        }



        public void killGUI()
        {
            for (int i = 0; i < WN.Count; i++)
                WN[i].killWindow();
            WN.Clear();
            if (mMP != null)
            {
                mMP.Hide();
            }

            foreach (OverlayContainer container in mO.Get2DElementsIterator())
            {
                foreach (OverlayElement element in container.GetChildIterator())
                    OverlayManager.Singleton.DestroyOverlayElement(element);
                mO.Remove2D(container);
                OverlayManager.Singleton.DestroyOverlayElement(container);
            }
            OverlayManager.Singleton.Destroy(mO);
            
            
            foreach (OverlayContainer container in mOTop.Get2DElementsIterator())
            {
                foreach (OverlayElement element in container.GetChildIterator())
                    OverlayManager.Singleton.DestroyOverlayElement(element);
                mOTop.Remove2D(container);
                OverlayManager.Singleton.DestroyOverlayElement(container);
            }
            OverlayManager.Singleton.Destroy(mOTop);
            
            
            

            if (mMPo != null)
            {
                foreach (OverlayContainer container in mMPo.Get2DElementsIterator())
                {
                    foreach (OverlayElement element in container.GetChildIterator())
                        OverlayManager.Singleton.DestroyOverlayElement(element);
                    mMPo.Remove2D(container);
                    OverlayManager.Singleton.DestroyOverlayElement(container);
                }
                OverlayManager.Singleton.Destroy(mMPo);
            }
            mMP = null;
            if (keyDelay != null) keyDelay.Dispose();
        }
        
	    public int injectMouse(Point p, bool LMB)        
	    {
	    	return injectMouse((uint)p.X, (uint)p.Y, LMB);
	    }
	    public void showMousePointer()
	    {
	    	 if(!mMP.IsVisible) mMP.Show();
               
	    }
	    public void hideMousePointer()
	    {
	    	if(mMP!=null && mMP.IsVisible) {
	    		mMP.Hide();
	    	}
	    }
        public int injectMouse(uint x, uint y, bool LMB)
        {
            
            if (mMP != null)
            {
                if(!mMP.IsVisible) mMP.Show();
                mMP.SetPosition(x, y);
            }

            if (mXW != null)
            {
                int i = 0;
                foreach (Window win in WN)
                {
                    if (mXW == win)
                    {
                        mXW.killWindow();
                        WN.RemoveAt(i);
                        oldMouseX = x;  oldMouseY = y;
                        return -1;
                    }
                    i++;
                }
            } 

            // by Adam - jeœli nie by³o ruchu myszk¹ to nie przekszkadzamy klawiaturze
            if (x == oldMouseX && y == oldMouseY && !LMB)
            {
                return -1;
            }
            for (int i = 0; i < WN.Count; i++)
            {
                int ret = WN[i].check(x, y, LMB);
                if (ret >= 0)
                {
                    oldMouseX = x; oldMouseY = y;
                    return ret;
                }
            } 
            oldMouseX = x; oldMouseY = y;
            return -1;
        }
		public bool injectKey(String key, Point mousePos)
        {
			return injectKey(key, (uint)mousePos.X, (uint)mousePos.Y);
	
		}
        public bool injectKey(String key, uint x, uint y)
        {
            if (keyDelay == null)
                keyDelay = new Timer();
            if (keyDelay.Milliseconds > 150)
            {
                for (int i = 0; i < WN.Count; i++)
                {
                    if (WN[i].checkKey(key, x, y))
                        return true;
                }
            }
            return false;
        }

        public void injectBackspace(uint x, uint y)
        {
            injectKey("!b", x, y);
        }

        public Window createWindow(Vector4 D, String M, int T, String C)
        {
            Window w = new Window(D, M, T, C, this);
            WN.Add(w);
            return w;
        }

        public void destroyWindow(Window w)
        {
            mXW = w;
        }

        public void killWindow(Window w)
        {
            
            for (int i = 0; i < WN.Count; i++)
            {
                if(WN[i].Equals(w))
                {
                    WN.Remove(w);
                    w.killWindow();
                    //wc--;
                    break;
                }
                
            }
               
        }
        public OverlayContainer createOverlay(String N, Vector2 P, Vector2 D,
                                              String M, String C, bool A, Overlay target){
        	String t = "Panel";
            if (C != "")
                t = "TextArea";
            OverlayElement e = OverlayManager.Singleton.CreateOverlayElement(t, N + this.name + DateTime.Now.Ticks);
            e.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            e.SetDimensions(D.x, D.y);
            e.SetPosition(0, 0);
            
           
            //if (M != "")
            //    e.MaterialName = M;

            if (C != "")
            {
                if(A)
                {
                    e.Top = 6;
                    e.Left = 6;
                }
               
                e.Caption = C;
                e.SetParameter("font_name", mFont);
                e.SetParameter("char_height", StringConverter.ToString(mFontSize));
              
            }
            
           
            OverlayContainer c =
                (OverlayContainer)
                OverlayManager.Singleton.CreateOverlayElement("Panel", name + "ContainerHack" + (oc++).ToString());
            c.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            c.SetDimensions(D.x, D.y);
            c.SetPosition(P.x, P.y);
            
            if (M != "")
                c.MaterialName = M;
            c.AddChild(e);
            if (A)
            {
                target.Add2D(c);
                c.Show();
            }
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="N">Name</param>
        /// <param name="P"></param>
        /// <param name="D"></param>
        /// <param name="M">Material</param>
        /// <param name="C">Caption</param>
        /// <param name="A">Is Window?</param>
        /// <returns></returns>
        public OverlayContainer createOverlay(String N, Vector2 P, Vector2 D,
                                              String M, String C, bool A)
        {
        	return createOverlay(N,P,D,M,C,A,mO);
        }

        public OverlayContainer createMousePointer(Vector2 d, String m)
        {
            mMPo = OverlayManager.Singleton.Create("BetaGUI.MP");
            mMPo.ZOrder = 649;
            mMP = createOverlay("bg.mp", new Vector2(0, 0), d, m, "", false);
            mMPo.Add2D(mMP);
            mMPo.Show();
            mMP.Hide();
            return mMP;
        }
    } // class GUI

   
    public class Button
    {
        public OverlayContainer mO, mCP;
        public String mmn, mma;
        public Callback callback;
        protected float x;
        protected float y;
        public float w, h;

        protected Window window;

        public Window Window
        {
            get { return window; }
        }

        public virtual float X
        {
            get { return x; }
            set { x = value; }
        }

        public virtual float Y
        {
            get { return y; }
            set { y = value; }
        }


        public virtual void Translate(Vector2 move)
        {
            x += move.x;
            mO.SetParameter("left", StringConverter.ToString(x));
            
            y += move.y;
            mO.SetParameter("top", StringConverter.ToString(y));
        }

      


        ////////////////////////////////////////////////////////////////////////////
        // Adam Witczak
        public static float C_RESPONSE_DELAY = 0.10f;


        private static Timer keyDelay = new Timer();
        public String text;
        public uint id = 0;
        public bool activated = false;


        public static void ResetButtonTimer()
        {
            keyDelay.Reset();
        }

        public static bool CanChangeSelectedButton()
        {
            return (keyDelay.Milliseconds > C_RESPONSE_DELAY*1000);
        }

        public static bool CanChangeSelectedButton(float delayModifier)
        {
            return (keyDelay.Milliseconds > delayModifier*C_RESPONSE_DELAY*1000);
        }

        public static bool TryToPressButton(Button b)
        {
            // naciskanie klawiszy musi byæ wolne
            if (keyDelay.Milliseconds > 5*C_RESPONSE_DELAY*1000)
            {
                b.callback.LS.onButtonPress(b);
               
                ResetButtonTimer();
                return true;
            }
            return false;
        }

        public static bool TryToPressButton(Button b, float manualDelay)
        {
            // naciskanie klawiszy musi byæ wolne
            if (keyDelay.Milliseconds > manualDelay)
            {
                b.callback.LS.onButtonPress(b);
                ResetButtonTimer();
                return true;
            }
            return false;
        }

        public Button(Vector4 D, String M, String T, Callback C, Window P, uint ID, Window w)
            : this(D, M, T, C, P,w)
        {
            id = ID;
        }

        public Button(Vector4 D, String M, String T, Callback C, Window P, Window window)
        {
            this.window = window;
            text = T;

            x = D.x;
            y = D.y;
            w = D.z;
            h = D.w;
            mmn = M;
            mma = M;

            ResourcePtr ma = MaterialManager.Singleton.GetByName(mmn + ".active");
            if (ma != null)
            {
                mma += ".active";
            }
            ma = null;

            mO = P.mGUI.createOverlay(P.mO.Name + "b" +
                                      StringConverter.ToString(P.mGUI.bc++),
                                      new Vector2(x, y), new Vector2(w, h), M, "", false);

            mCP = P.mGUI.createOverlay(mO.Name + "c",
                                       new Vector2(4, (h - P.mGUI.mFontSize)/2),
                                       new Vector2(w, h), "", T, false);
           
            P.mO.AddChild(mO);
            mO.Show();
            mO.AddChild(mCP);
            mCP.Show();
            callback = C;
        }

        // Adam Witczak
        ////////////////////////////////////////////////////////////////////////////


        public void killButton()
        {
            foreach (OverlayContainer container in mO.GetChildContainerIterator())
                foreach (OverlayElement element in container.GetChildIterator())
                    OverlayManager.Singleton.DestroyOverlayElement(element);
            foreach (OverlayElement element in mO.GetChildIterator())
                OverlayManager.Singleton.DestroyOverlayElement(element);
            mO.Parent.RemoveChild(mO.Name);
            OverlayManager.Singleton.DestroyOverlayElement(mO);
        }

        public bool activate(bool a)
        {
            bool ret = activated;
            if (!a && mmn != "")
            {
                activated = false; // Adam Witczak
                mO.MaterialName = mmn;
                return false;
            }

            if (a && mma != "")
            {
                activated = true; // Adam Witczak
                mO.MaterialName = mma;
            }
            return ret;
        }

        public bool isin(float mx, float my, float px, float py)
        {
            return (!(mx >= x + px && my >= y + py)) || (!(mx <= x + px + w && my <= y + py + h));
        }
    }

    public class TextInput
    {
        public OverlayContainer mO, mCP;
        public String mmn, mma, value;
        public float x, y, w, h, length;

        public String getValue()
        {
            return value;
        }

        public void setValue(String v)
        {
            mO.Caption = value = v;
        } // mCP here if ...

        public TextInput(Vector4 D, String M, String V, uint L, Window P)
        {
            x = D.x;
            y = D.y;
            w = D.z;
            h = D.w;
            value = V;
            mmn = M;
            mma = M;
            length = L;

            ResourcePtr ma = MaterialManager.Singleton.GetByName(mmn + ".active");
            if (ma != null)
                mma += ".active";
            ma = null;
            mO = P.mGUI.createOverlay(P.mO.Name + "t" +
                                      StringConverter.ToString(P.mGUI.tc++),
                                      new Vector2(x, y), new Vector2(w, h), M, "", false);
            mCP = P.mGUI.createOverlay(mO.Name + "c",
                                       new Vector2(0, (h - P.mGUI.mFontSize)/2),
                                       new Vector2(w, h), "", V, false);
            P.mO.AddChild(mO);
            mO.Show();
            mO.AddChild(mCP);
            mCP.Show();
        }

        public void killTextInput()
        {
            foreach (OverlayContainer container in mO.GetChildContainerIterator())
                foreach (OverlayElement element in container.GetChildIterator())
                    OverlayManager.Singleton.DestroyOverlayElement(element);
            foreach (OverlayElement element in mO.GetChildIterator())
                OverlayManager.Singleton.DestroyOverlayElement(element);
            mO.Parent.RemoveChild(mO.Name);
            OverlayManager.Singleton.DestroyOverlayElement(mO);
        }

        public void activate(bool a)
        {
            if (!a && mmn != "")
                mO.MaterialName = mmn;

            if (a && mma != "")
                mO.MaterialName = mma;
        }


        public bool isin(float mx, float my, float px, float py)
        {
            return (!(mx >= x + px && my >= y + py)) || (!(mx <= x + px + w && my <= y + py + h));
        }
    }

    public class Window
    {
        public TextInput mATI;
        public Button mRZ, mAB, mTB; // resize, activebutton, titlebar
        public float x, y, w, h;
        public GUI mGUI;
        public OverlayContainer mO;
                
        public List<Button> mB = new List<Button>();
        public List<TextInput> mT = new List<TextInput>();
        public List<OverlayContainer> mI = new List<OverlayContainer>();


        // Adam Witczak
        public Button createButton(Vector4 D, String M, String T, Callback C, uint ID)
        {
            Button x = new Button(D, M, T, C, this, ID, this);
            mB.Add(x);
            return x;
        }


        public Button createButton(Vector4 D, String M, String T, Callback C)
        {
            Button x = new Button(D, M, T, C, this, this);
            mB.Add(x);
            return x;
        }

        public Button createInmovableButton(Vector4 D, String M, String T, Callback C, uint ID)
        {
            Button x = new InmovableButton(D, M, T, C, this, ID, this);
            mB.Add(x);
            return x;
        }


        public Button createInmovableButton(Vector4 D, String M, String T, Callback C)
        {
            Button x = new InmovableButton(D, M, T, C, this, this);
            mB.Add(x);
            return x;
        }

        public TextInput createTextInput(Vector4 D, String M, String V, uint L)
        {
            TextInput x = new TextInput(D, M, V, L, this);
            mT.Add(x);
            return x;
        }


        public OverlayContainer createStaticText(Vector4 D, String T)
        {
        	return createStaticText(D, T, ColourValue.White);
        }

        /// <summary>
        /// Tworzy tekst statyczny ale string zostanie automatycznie podzielony na nowe linie tak aby pasowal do okienka w ktorym ma byc wyswietlony
        /// </summary>
        /// <param name="D"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public OverlayContainer createStaticTextAutoSplit(Vector4 D, String T)
        {
            h = this.mGUI.mFontSize; // margin and font size
            Font font = (Font)(Mogre.FontManager.Singleton.GetByName(this.mGUI.mFont).Target);
            Vector2 averageSize = ViewHelper.GetTextAverageSize(T, font, h);

            
            int charsPerLine = (int)Math.Floor((D.z - 2 * h) / averageSize.x);
            string multiline = LanguageResources.SplitInsertingNewLinesByLength(T, charsPerLine);
            return createStaticText(D, multiline, ColourValue.White);
        }


        public static void ChangeContainerColour(OverlayContainer cont, ColourValue c)
        {
            ChangeContainerColour(cont, c, c);
        }

        public static void ChangeContainerColour(OverlayContainer cont,  ColourValue c1, ColourValue c2)
        {
        	OverlayContainer.ChildIterator i = cont.GetChildIterator();
        	while(i.MoveNext())
        	{
        		OverlayElement element =i.Current;        
        		if(element != null)
        		{
        			ChangeElementColour(element, c1, c2);
        		}
        	}

        }

        public static void ChangeElementColour(OverlayElement element, ColourValue c)
        {
            ChangeElementColour(element, c, c);
        }

        public static void ChangeElementColour(OverlayElement element, ColourValue c1, ColourValue c2)
        {
        	string colour1 = String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(c1.r), StringConverter.ToString(c1.g), StringConverter.ToString(c1.b));
        	string colour2 = String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(c2.r), StringConverter.ToString(c2.g), StringConverter.ToString(c2.b));
        	
        	element.SetParameter("colour_top", colour1);
            element.SetParameter("colour_bottom", colour2);
        }

        public OverlayContainer createStaticText(Vector4 D, String T, ColourValue c)
        {
            return createStaticText(D, T, c, c);
        }

        // zmiana zwracanego typu z void na OverlayContainer by Adam
        public OverlayContainer createStaticText(Vector4 D, String T, ColourValue cTop,  ColourValue cBottom)
        {
            OverlayContainer x = mGUI.createOverlay(mO.Name +
                                                    StringConverter.ToString(mGUI.tc++),
                                                    new Vector2(D.x, D.y), new Vector2(D.z, D.w), "", T, false);




            ChangeContainerColour(x, cTop, cBottom);
        	
            mO.AddChild(x);
            x.Show();
          
            return x;
        }

        public OverlayContainer createStaticImage(Vector2 pos, String imageName)
        {
            TexturePtr t = TextureManager.Singleton.Load(imageName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            uint width = t.SrcWidth;
            uint height = t.SrcHeight;
            t = null;
            return createStaticImage(new Vector4(pos.x, pos.y, width, height), imageName);

        }
        
 		public OverlayContainer createStaticImage(Vector4 posAndSize, String imageName)
        {
 			return createStaticImage(posAndSize, imageName, 0);
 	
	    }
 		 public OverlayContainer createStaticImage(Vector4 posAndSize, String imageName, ushort zOrder)
        {
 		 	return createStaticImage(posAndSize, imageName, false);
 		 	
 		 }
        public OverlayContainer createStaticImage(Vector4 posAndSize, String imageName, bool topLevel)
        {
            mGUI.tc++;
            MaterialPtr ptr = Wof.Misc.ViewHelper.CloneMaterial("bgui.image", "bgui.image_" + imageName + mO.Name + StringConverter.ToString(mGUI.tc));
            ptr.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetTextureName(imageName);
 
            //  alpha_op_ex source1 src_manual src_texture 0.5      
           // ptr.GetBestTechnique().GetPass(0).GetTextureUnitState(0).SetAlphaOperation(LayerBlendOperationEx.LBX_SOURCE1, LayerBlendSource.LBS_MANUAL, LayerBlendSource.LBS_TEXTURE, 0.5f);
 	
 	
           Overlay o;
           
           if(topLevel) {
           	 o = mGUI.mOTop;
           	
           } else {
           	 o = mGUI.mO;
           }
        
            OverlayContainer x = mGUI.createOverlay(mO.Name + StringConverter.ToString(mGUI.tc) + imageName,
                                                    new Vector2(posAndSize.x, posAndSize.y), new Vector2(posAndSize.z, posAndSize.w), ptr.Name, "", false, o);
                     
            ptr = null;

            mO.AddChild(x);
            x.Show();
            mI.Add(x);
                      
            return x;
        }




        public void hide()
        {
            mO.Hide();
        }

        public void show()
        {
            mO.Show();
        }

        public bool isVisible()
        {
            return mO.IsVisible;
        }

        public Window(Vector4 D, String M, int t, String C, GUI G)
        {
            x =  D.x;
            y =  D.y;
            w =  D.z;
            h =  D.w;
            mGUI = G;
            mTB = null;
            mRZ = null;
            mATI = null;
            mAB = null;
            mO = G.createOverlay("BetaGUI.w" + mGUI.name + StringConverter.ToString(G.wc++),
                                 new Vector2(D.x, D.y),
                                 new Vector2(D.z, D.w), M, C, true);
            if (t >= 2)
            {
                Callback c = new Callback();
                c.t = 4;
                mRZ = createButton(new Vector4(D.z - 16, D.w - 16, 16, 16), M + ".resize", "", c);
            }

            if (t == 1 || t == 3)
            {
                Callback c = new Callback();
                c.t = 3;
                mTB = createButton(new Vector4(0, 0, D.z, 22), M + ".titlebar", C, c);
            }
        }

        public void killWindow()
        {
            for (int i = 0; i < mB.Count; i++)
                mB[i].killButton();
            for (int i = 0; i < mT.Count; i++)
                mT[i].killTextInput();
            
            try
            {
	            for (int i = 0; i < mI.Count; i++)
	            {
	            	//foreach (OverlayContainer container in mI[i].GetChildContainerIterator())
	               // foreach (OverlayElement element in container.GetChildIterator())
	               //     OverlayManager.Singleton.DestroyOverlayElement(element);
	               
	             	OverlayManager.Singleton.DestroyOverlayElement(mI[i]);
	             	
	            }
            	/*foreach (OverlayElement element in mO.GetChildIterator())
                {
                     OverlayManager.Singleton.DestroyOverlayElement(element);
                 }*/
	            	 
                foreach (OverlayContainer container in mO.GetChildContainerIterator())
                {
                	
                	
                     foreach (OverlayElement element in container.GetChildIterator())
                     {
                         OverlayManager.Singleton.DestroyOverlayElement(element);
                     }
                        
                    
                }
               
	            mO.Hide();
	            mO.Dispose();
	            mO = null;
            }
            catch(Exception ex)
            {
            	LogManager.Singleton.LogMessage("Error while disposing GUI: "+ex.Message+". Stack: "+ex.StackTrace);
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="LMB"></param>
        /// <returns>index buttona ktory zostal nacisniety. -1 gdy nic nie bylo nacisniete</returns>
        public int check(uint px, uint py, bool LMB)
        {
            if (!mO.IsVisible)
                return -1;
           
           /* if ((px < x) || (px > x + w) || (py > y + h) || (py < y))
            {
                // Adam Witczak - myszka k³óci³a siê z klawiatur¹
                //  if (mAB != null)
                //      mAB.activate(false);
                return -1;
            }*/


            int ret = -1;
 
            if (mAB != null)
            {
                mAB.activate(false);
            }
                

            for (int i = 0; i < mB.Count; i++)
            {
                if (mB[i].isin(px, py, x, y))
                {
                    mB[i].activate(false);
                    continue;
                }

                if (mAB != null)
                {
                   mAB.activate(false);
                }
                    

                mAB = mB[i];
                ret = i;
                mAB.activate(true); // activated manually here: AbstractScreen.selectButton()

                if (LMB && mATI != null) // deaktywacja pola tekstowego
                {
                    mATI.activate(false);
                    mATI = null;
                }

                if (!LMB) continue; //return true;

                switch (mAB.callback.t)
                {
                    default:
                        return ret;
                    case 1:
                        //mAB.callback.fp(mAB); // FIX / what is this
                        return ret;
                    case 2:
                        mAB.callback.LS.onButtonPress(mAB);
                        return ret;
                    case 3:
                        mO.SetPosition(x = px - (mAB.w/2), y = py - (mAB.h/2));
                        return ret;
                    case 4:
                        mO.SetDimensions(w = px - x + 8, h = py - y + 8);
                        mRZ.mO.SetPosition(mRZ.X = w - 16, mRZ.Y = h - 16);
                        if (mTB != null)
                        {
                            mTB.mO.Width = mTB.w = w;
                        }
                        return ret;
                }
            }
            if (!LMB)
                return ret;

            for (int i = 0; i < mT.Count; i++)
            {
                if (mT[i].isin(px, py, x, y))
                    continue;
                mATI = mT[i];
                mATI.activate(true);
                return ret;
            }
            if (mATI != null)
            {
                mATI.activate(false);
                mATI = null;
            }
            return ret;
        }

        public bool checkKey(String k, uint px, uint py)
        {
            if (mATI == null)
                return false;
            if (!mO.IsVisible)
                return false;
            /*  if (!(px >= x && py >= y) || !(px <= x + w && py <= y + h))
                return false;*/
            // Adam Witczak - wymaga³o ¿eby myszka by³a aktywna
            if (k == "!b" && mATI.value.Length > 0)
            {
                mATI.setValue(mATI.value.Substring(0, mATI.value.Length - 1));
                foreach (OverlayElement element in mATI.mCP.GetChildIterator())
                    element.Caption = mATI.value;
                mGUI.keyDelay.Reset();
                return true;
            }

            if (mATI.value.Length >= mATI.length)
                return true;
            if (k != "!b")
                mATI.value += k;
            foreach (OverlayElement element in mATI.mCP.GetChildIterator()) // take from mCP if ...
                element.Caption = mATI.value;
            mGUI.keyDelay.Reset();
            return true;
        }
    }

    public class InmovableButton : Button
    {
        public InmovableButton(Vector4 D, string M, string T, Callback C, Window P, uint ID, Window w) : base(D, M, T, C, P, ID, w)
        {

        }

        public InmovableButton(Vector4 D, string M, string T, Callback C, Window P, Window window) : base(D, M, T, C, P, window)
        {
        }

        public override void Translate(Vector2 move)
        {
            // do nothing
        }
        public override float X
        {
            get { return x; }
            set {}
        }

        public override float Y
        {
            get { return y; }
            set {  }
        }
    }

    public interface BetaGUIListener
    {
        void onButtonPress(Button referer);
    }

    public class Callback
    {
        public uint t;
        public BetaGUIListener LS;

        public Callback()
        {
            t = 0;
        }

        public Callback(BetaGUIListener L)
        {
            t = 2;
            LS = L;
        }
    }
}