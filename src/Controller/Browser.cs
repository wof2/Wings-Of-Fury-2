/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Mogre;
using MOIS;
using Wof.Controller.AdAction;
using Wof.Controller.Screens;
using Wof.Languages;
using Wof.Tools;

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

	    private Point lastScreenPos = new Point(-1,-1);
	    
	    protected bool isInitialState = true;
        protected Mutex mutCache;
        protected ManualResetEvent condReset;

	
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

     
        /*
	    public bool IsMouseOver()
        {           
	        return mouseOver;    
        }*/
        public bool IsReady()
        { 
            if(this.InvokeRequired)
            {
                return (bool)this.Invoke(new BoolDelegate(IsReady));
            }
            return isReady; 
           
        }
        protected delegate bool PointDelegate(Point p);
        protected delegate void IntDelegate(int i);
        protected delegate void VoidDelegate();
        protected delegate void UriDelegate(Uri uri);
        protected delegate bool BoolDelegate();
        

	    
		
	    private Form gameForm;

	 
		public Browser(Form gameForm, AbstractScreen currentScreen)
		{
            mutCache = new System.Threading.Mutex();    //mutex for browser list

            condReset = new ManualResetEvent(false);    //condition to make the thread wait
			
            Vector2 m = currentScreen.GetMargin();

            origin = currentScreen.ViewportToScreen(new Vector2(m.x + currentScreen.Viewport.ActualWidth * 0.51f, (int)(m.y)));
            dimensions = currentScreen.ViewportToScreen(new Vector2(currentScreen.Viewport.ActualWidth * 0.47f, (int)currentScreen.Viewport.ActualHeight - m.y - currentScreen.GetTextVSpacing()));


            this.gameForm = gameForm;	
		
			InitializeComponent();

            CreateBrowser();
            
            this.Activated += new System.EventHandler(this.Browser_Activated); // dopiero po zakonczeniu i po zrobieniu Show() (ktore z kolei wywoła onLoad, potem Hide)
            this.Load += new System.EventHandler(this.Browser_Load);
            this.Shown += new System.EventHandler(this.Browser_Shown);

		}
        public void HideBrowser()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(HideBrowser));
                return;
            }
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "HideBrowser");
		   
            Visible = false;
        }

        public void KillBrowser()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(KillBrowser));
                return;
            }
            Hide();
          //  Close();
            Dispose();
        }
      
       public void ShowBrowserAndTopMostScale(int scale)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new IntDelegate(ShowBrowserAndTopMostScale), new object[] {scale});
                return;
            }

            SetScale(scale);
            TopMost = true;
            if (!Visible)
            {
                Show();
            }
            
        }
        private void Browser_Load(object sender, EventArgs e)
        {
           
        }

	    private bool isReady = false;
       

        private delegate void NewBrowserDelegate();

       
        private void CreateBrowser()
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CreateBrowserStart");
		   
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            
            this.wofBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wofBrowser
            // 
        
            this.wofBrowser.AllowWebBrowserDrop = false;
            this.wofBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wofBrowser.Location = new System.Drawing.Point(0, 0);
            this.wofBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.wofBrowser.MinimumSize = new System.Drawing.Size(1, 1);
            this.wofBrowser.Name = "wofBrowser";
            this.wofBrowser.ScriptErrorsSuppressed = true;
            this.wofBrowser.TabIndex = 1;
            this.wofBrowser.TabStop = false;
            this.wofBrowser.WebBrowserShortcutsEnabled = false;
            this.wofBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.wofBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WofBrowserNavigated);
           // this.wofBrowser.Dock = DockStyle.Bottom;
           // 284, 262
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          //  this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.wofBrowser);

            this.ResumeLayout(false);
           
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowserFormClosing);
            wofBrowser.DocumentText = File.ReadAllText("none.dat");
            this.wofBrowser.Url = new System.Uri(GetNewsUrl(), System.UriKind.Absolute);
           
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "CreateBrowserEnd");
            
            isReady = true;
           
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

	    public Point LastScreenPos
	    {
	        get { return lastScreenPos; }
	    }


	    public void Navigate(Uri uri)
        {
           
            if(isReady && Visible)
            {
                if (this.InvokeRequired)
                {
                    Invoke(new UriDelegate(Navigate), new object[] {uri} );
                    return;

                } 
                this.wofBrowser.Navigating -= wofBrowser_Navigating;
                eventsWired = false;
                this.wofBrowser.Url = uri;
              
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
            if(this.InvokeRequired)
            {
                Invoke(new VoidDelegate(SetPosition));
                return;
            }
            SetBounds((int)(origin.x), (int)(origin.y), (int)dimensions.x, (int)dimensions.y);
            this.wofBrowser.SetBounds(0, (int)(homeButton.Height * 1.2f), (int)dimensions.x, (int)dimensions.y - (int)(homeButton.Height * 1.2f));
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
	    private HtmlDocument evDownloadPage;

        private string getEnhancedZipFileLocation()
        {
            String downloadPath = System.IO.Directory.GetCurrentDirectory();
            downloadPath = Path.Combine(downloadPath, @"enhanced.zip");
            return downloadPath;
            
        }

        private bool downloadEnhancedVersion(HtmlDocument downloadLink)
        {
            //<!-- WOF_ENHANCED_DOWNLOAD_LINK:http://wingsoffury2.com/enhanced_get.php?version=3.4&type=donate&email=graph12@wp.pl&hash=160;101;201;137;80;48;234;43;166;153;12;216;115;213;98;66;202;234;28;127;&directdownload=1 -->
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "webBrowser1_downloadEnhancedVersion");
            Match match = Regex.Match(downloadLink.Body.InnerHtml, @"WOF_ENHANCED_DOWNLOAD_LINK:(.*) ", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string url = match.Groups[1].Value;
                if(Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(enhancedVersion_DownloadFileCompleted);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(enhancedVersion_DownloadProgressChanged);
             
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "webBrowser1_downloadEnhancedVersion - download started");
                    webClient.DownloadFileAsync(new Uri(url), getEnhancedZipFileLocation());
                   


                    return true;
                }
            }
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "webBrowser1_downloadEnhancedVersion - unable to find EV download link in URL:" + downloadLink.Url);
            return false;
           
        }

        private HtmlElement getDownloadProgressElement()
        {
            if (this.wofBrowser.Document != null && this.wofBrowser.Document.Body != null && wofBrowser.Document.Body.InnerHtml.Contains("WOF_ENHANCED_DOWNLOAD_LINK"))
            {
                HtmlElement element = this.wofBrowser.Document.GetElementById("download_progress");
                return element;

            }
            return null;

        }
        void enhancedVersion_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            HtmlElement element = getDownloadProgressElement();
            if (element != null)
            {
                element.InnerHtml = "Download progress: "+e.ProgressPercentage+ "%";
            }

        }

      
 

        void enhancedVersion_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            HtmlElement element = getDownloadProgressElement();
            if (element != null)
            {
               
                string location = getEnhancedZipFileLocation();
                if(File.Exists(location))
                {
                    Zipfiles.ExtractZipFile(location, EngineConfig.C_GAME_INSTALL_DIR);
                    element.InnerHtml = "Download completed - please restart the game!";
                    
                }else
                {
                    element.InnerHtml = "Download completed but file does not exist on local system. Please install Enhanced version manually. Check your e-mail for instructions";
                }
            }
        }




	    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
            if(!wofBrowser.ReadyState.Equals(WebBrowserReadyState.Complete))
            {
                return;
            }
            string path;
            if (wofBrowser.Document!=null && wofBrowser.Document.Url != null)
            {
                path = wofBrowser.Document.Url.LocalPath;
            }else
            {
                path = null;
            }

            if (wofBrowser.Document != null && wofBrowser.Document.Body != null && wofBrowser.Document.Body.InnerHtml.Contains("WOF_ENHANCED_DOWNLOAD_LINK"))
            {
                downloadEnhancedVersion(wofBrowser.Document);
              
            }

            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "webBrowser1_DocumentCompleted:" + path);
            switch(path)
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
               // this.WindowState = FormWindowState.Maximized;
              //  this.FormBorderStyle = FormBorderStyle.Fixed3D;
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
            if (InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Activate));
                return;
            } 
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Activate explicid");
            canActivate = true;
        	base.Activate();
	   //     base.Focus();
        }
        
        private void Browser_Activated(object sender, EventArgs e)
        {
            
            if(!Visible)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Activated -> not visible so returning");
               
                return;
            }
            /*
        	if(!canActivate)
        	{
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Activated -> Can't activate browser -> game.Activate");
                if(gameForm.InvokeRequired)
                {
                    gameForm.BeginInvoke(new VoidDelegate(gameForm.Activate));
                }else
                {
                    gameForm.Activate();
                }
        		
        		return;
        	}*/
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Activated:" + lastScreenPos);
           // Console.WriteLine(" !!!!!!!!!!!!!! ACTIVATED !!!!!!!!!!!!!!");
         
            if(lastScreenPos.X >= 0)
            {
                Cursor.Position = lastScreenPos;
            }     
            canActivate = false;            
        }

        public new void Hide()
        {
            if (InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Hide));
                return;
            }
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
            if (InvokeRequired)
            {
                this.Invoke(new VoidDelegate(ReturnToInitialState));
                return;
            }
           
            
           
            if(!isInitialState || !this.Visible)
            {
               
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "ReturnToInitialState.notIsInitialState or not visible");
            	this.Show();
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                SetPosition();
            }else
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "ReturnToInitialState -> no need");
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

	    private bool firstTime = false;
        public new void Show()
        {
           /* if(firstTime)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Shown (first time).Hide");
                firstTime = false;
                return;
            }*/
          
            base.Show();
          
        }

        private void Browser_Shown(object sender, EventArgs e)
        {
          //  ReturnToInitialState();
         //   
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Browser_Shown -> hide");
            	
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "GoHome");
                if (eventsWired)
                {
                    this.wofBrowser.Navigating -= wofBrowser_Navigating;
                    eventsWired = false;
                }
                this.wofBrowser.Navigate(new Uri(GetNewsUrl(), UriKind.Absolute));
            }
        }


      
        public void SetLastMouseScreenPos(Point screen)
        {
            lastScreenPos = screen;
           // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "lastScreenPos=" + lastScreenPos);
        }

        public bool IsMouseOver(Point screen)
        {

            if (!isReady) return false;

            Point client;
            if (InvokeRequired)
            {
                PointDelegate deleg = new PointDelegate(IsMouseOver);
                Object o = this.Invoke(deleg, new object[] { screen });
                if (o == null) return false;

                return (bool)o;
            }

            client = this.PointToClient(screen);
           
            bool over = this.ClientRectangle.Contains(client);
            if (!over)
            {
               // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "IsMouseOver=false");
            }
           
            return over;
        }		

        private void Document_MouseLeave(object sender, HtmlElementEventArgs e)
        {

         /*   if (!this.Bounds.Contains(PointToScreen(mousePos)))
            {
                mouseOver = false;
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser Document_MouseLeave: " + Bounds + " mouse:" + PointToScreen(mousePos));
            } else
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser Document_MouseLeave [cancelled]: " + Bounds + " mouse:" + PointToScreen(mousePos));
            }*/
           
            
        }

        private void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {/*
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser Document_MouseOver");
            mouseOver = true;*/
            mousePos = e.MousePosition;
        }

        
        private void Browser_MouseEnter(object sender, EventArgs e)
        {
        //    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser Browser_MouseEnter");
        //    mouseOver = true;
     
        }

        private void Browser_MouseLeave(object sender, EventArgs e)
        {/*
            if (!this.Bounds.Contains(PointToScreen(mousePos)))
            {
                mouseOver = false;
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser Browser_MouseLeave");
            }*/

        }
        void Document_MouseMove(object sender, HtmlElementEventArgs e)
        {
         //   LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "mouse:" + e.MousePosition);
            mousePos = e.MousePosition;
        }

       
	}
}
