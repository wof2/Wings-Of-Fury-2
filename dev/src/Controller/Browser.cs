/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Mogre;
using MOIS;
using Wof.Controller.AdAction;
using Wof.Controller.Screens;
using Wof.Languages;

namespace Wof.Controller
{
	/// <summary>
	/// Description of Browser.
	/// </summary>
	public partial class Browser : Form
	{
		private Point mousePos = new Point(0, 0);
		private bool eventsWired = false;

		protected bool canActivate = false;
	    private bool isActivated = false;		
		protected bool mouseOver = true;

       
        protected Vector2 origin;
        protected Vector2 dimensions;

		
		public bool MouseOver {
			get { return mouseOver; }
		}
	    protected Point lastScreenPos = new Point(-1,-1);
	    
	    protected bool isInitialState = true;
	    
	
	    public bool IsInitialState {
			get { return isInitialState; }
		}

	    public bool IsActivated
	    {
	        get 
	        { 
	        	return isActivated;
	        }
	        set 
	        {
                if(value)
                {
                    mouseOver = true;                   
                }
                else
                {
                    mouseOver = false;        
                }               
                isActivated = value;            
            }
	    }

	    public Point MousePos
	    {
	        get { return mousePos; }
	    }

	    public bool IsMouseOver()
        {           
	        return mouseOver;    
        }	

	    public bool IsMouseOver(Point screen)
		{            
            Point client = this.wofBrowser.PointToClient(screen);
            lastScreenPos = screen;         
            return this.wofBrowser.Bounds.Contains(client);
		}		
		
	    private Form gameForm;

	 
		public Browser(Form gameForm, AbstractScreen currentScreen)
		{	
			
			
            Vector2 m = currentScreen.GetMargin();

            origin = currentScreen.ViewportToScreen(new Vector2(m.x + currentScreen.Viewport.ActualWidth * 0.51f, (int)(m.y)));
            dimensions = currentScreen.ViewportToScreen(new Vector2(currentScreen.Viewport.ActualWidth * 0.47f, (int)currentScreen.Viewport.ActualHeight - m.y - currentScreen.GetTextVSpacing()));


            this.gameForm = gameForm;			
			InitializeComponent();
			//wofBrowser.DocumentText = File.ReadAllText("none.dat");
            this.wofBrowser.Url = new System.Uri(GetNewsUrl(), System.UriKind.Absolute);
		}


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public void SetParentOrigin(Vector2 newPos, AbstractScreen currentScreen)
	    {
            if (currentScreen == null || currentScreen.Viewport == null) return;
            Vector2 m = currentScreen.GetMargin();
	        origin = currentScreen.ViewportToScreen(new Vector2(m.x + currentScreen.Viewport.ActualWidth * 0.51f, (int)(m.y)));
	        origin += newPos;
	        SetPosition();
	    }

	    public void SetPosition()
        {
            
            SetBounds((int)(origin.x), (int)(origin.y), (int)dimensions.x, (int)dimensions.y);
        }



	    private void Document_MouseLeave(object sender, HtmlElementEventArgs e)
	    {
	        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser LEAVE");
	        mouseOver = false;           
	    }

	    private void Document_MouseOver(object sender, HtmlElementEventArgs e)
	    {
            mouseOver = true;
	        mousePos = e.MousePosition;         
	    }

       

	    void Document_MouseMove(object sender, HtmlElementEventArgs e)
        {
	        mousePos = e.MousePosition;  
	    }

	    private int scalePercent = 100;
	    public void SetScale(int percent)
	    {
	    	
	    	return; // na razie wylaczone z uwagi na ramki
	    	scalePercent = percent;
	    	
	    	
	    	if(wofBrowser.Document != null && wofBrowser.ReadyState.Equals(WebBrowserReadyState.Complete))
	    	{
	    		wofBrowser.Document.Body.Style = "zoom: " + scalePercent + "%";
	    		
	    		// nie mozna
				/*HtmlWindow docWindow = wofBrowser.Document.Window;
			
				foreach (HtmlWindow frameWindow in docWindow.Frames)
				{
					frameWindow.Document.Body.Style = "zoom: " + scalePercent + "%";
				}*/
	    	}
	    }
	   

        private string read404Page()
        {
            string text = File.ReadAllText("none.dat");
            text = RijndaelSimple.Decrypt(text);
            string[] keys = new string[]{
                                     LanguageKey.News404_1,
                                     LanguageKey.News404_2,
                                     LanguageKey.News404_3,
                                     LanguageKey.News404_4
                                 };
            
            text = text.Replace("{app_url}", Application.StartupPath);
            foreach (string s in keys)
            {
                text = text.Replace("{" + s + "}", LanguageResources.GetString(s));
            }
            return text;
        }

	    private bool isOffLine = false;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
            if(!wofBrowser.ReadyState.Equals(WebBrowserReadyState.Complete))
            {
                return;
            }
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "webBrowser1_DocumentCompleted");
            switch(wofBrowser.Document.Url.LocalPath)
        	{
        		case  "/navcancl.htm":
        			
        		case "/dnserrordiagoff_webOC.htm":
        	        isOffLine = true;
                    wofBrowser.Document.OpenNew(true);

        	        foreach (HtmlElement link in wofBrowser.Document.Links)
        	        {
                        link.Click += new HtmlElementEventHandler(offline_link_Click);
        	        }
                    wofBrowser.Document.Write(read404Page());
        			
        		break;                       			  	
        	}
            
            /*wofBrowser.Document.Body.SetAttribute("overflow-x", "hidden");
            wofBrowser.Document.Body.SetAttribute("overflow-y", "scroll");*/
  
            //  this.wofBrowser.
            this.wofBrowser.Document.MouseMove += new HtmlElementEventHandler(Document_MouseMove);
            this.wofBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            this.wofBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
            this.wofBrowser.Document.Click += new HtmlElementEventHandler(Document_Click);
            SetScale(scalePercent);
            if (!eventsWired)
            {
                this.wofBrowser.Navigating += wofBrowser_Navigating;
                eventsWired = true;
            }

        }

        void offline_link_Click(object sender, HtmlElementEventArgs e)
        {
           
        }

        void wofBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "wofBrowser_Navigating:"+e.Url.ToString());
            if (isOffLine)
            {
                e.Cancel = true;
                Hide(); 
                gameForm.WindowState = FormWindowState.Minimized;
                Process.Start(Game.GetDefaultBrowserPath(), e.Url.ToString());
                return;
             
            }

            if (eventsWired)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "wofBrowser_Navigating.eventsWired");
                this.wofBrowser.Navigating -= wofBrowser_Navigating;
                eventsWired = false;
            }
           
            if (isInitialState)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "wofBrowser_Navigating.isInitialState");
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                // this.Activate();
            }

            isInitialState = false;
        }

        void Document_Click(object sender, HtmlElementEventArgs e)
        {
            
        }

        void element_Click(object sender, HtmlElementEventArgs e)
        {
           
        }

	   

	    public new void Activate()
        {
        	canActivate = true;
        	base.Activate();
	        base.Focus();
        }

        
        private void Browser_Activated(object sender, EventArgs e)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Activated");
        	if(!canActivate)
        	{
        		gameForm.Activate();
        		return;
        	}
           // Console.WriteLine(" !!!!!!!!!!!!!! ACTIVATED !!!!!!!!!!!!!!");
            if(lastScreenPos.X >= 0)
            {
                Cursor.Position = lastScreenPos;
            }     
            canActivate = false;            
        }

        public new void Hide()
        {
        	isInitialState = false;
        	base.Hide();
        }
		
	

        private static string GetNewsUrl()
        {
            return EngineConfig.C_WOF_NEWS_PAGE + "?v=" + EngineConfig.C_WOF_VERSION + "&d=" +
                EngineConfig.C_IS_DEMO.ToString() + "&l=" + LanguageManager.ActualLanguageCode + "&e=" + EngineConfig.IsEnhancedVersion + "&noads=" + (AdManager.Singleton.ConnectionErrorOccured ? 1 : 0);
        }
		
        public void ReturnToInitialState()
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "ReturnToInitialState");
            
            if(!isInitialState)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "ReturnToInitialState.notIsInitialState");
            	this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                SetPosition();
                if (eventsWired)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "ReturnToInitialState.unwiring");
            	
                    this.wofBrowser.Navigating -= wofBrowser_Navigating;
                    eventsWired = false;
                }
                this.wofBrowser.Navigate(new Uri(GetNewsUrl(), UriKind.Absolute));
            }
            isInitialState = true;
        }
		void BrowserFormClosing(object sender, FormClosingEventArgs e)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "BrowserFormClosing");
		    ReturnToInitialState();
			e.Cancel = true;
			

		}
		
		void WofBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			
		}
	}
}
