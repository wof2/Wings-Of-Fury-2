/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using Mogre;
using MOIS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wof.Controller
{
	/// <summary>
	/// Description of Browser.
	/// </summary>
	public partial class Browser : Form
	{
		protected bool canActivate = false;
	    private bool isActivated = false;		
		protected bool mouseOver = true;
		
		public bool MouseOver {
			get { return mouseOver; }
		}
	    protected Point lastScreenPos = new Point(-1,-1);
	    
	    protected bool isFullScreen = false;
	    
		public bool IsFullScreen {
			get { return isFullScreen; }
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
		
		public Browser(Form gameForm)
		{
            this.gameForm = gameForm;			
			InitializeComponent();
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

        private Point mousePos = new Point(0, 0);

	    void Document_MouseMove(object sender, HtmlElementEventArgs e)
        {
	        mousePos = e.MousePosition;  
	    }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          //  LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "document loaded");
            this.wofBrowser.Document.MouseMove += new HtmlElementEventHandler(Document_MouseMove);
            this.wofBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            this.wofBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
           
            this.wofBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WofBrowserNavigating);
        }

        public new void Activate()
        {
        	canActivate = true;
        	base.Activate();  
        }
       
        
        private void Browser_Activated(object sender, EventArgs e)
        {
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

		
		void WofBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			
			this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Activate();
            isFullScreen = true;
            
			//e.Cancel = true;
			
			//wofBrowser.Navigate(e.Url, true);
		}
		
		void BrowserFormClosing(object sender, FormClosingEventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
			this.FormBorderStyle = FormBorderStyle.None;
			e.Cancel = true;
			isFullScreen = false;
		}
	}
}
