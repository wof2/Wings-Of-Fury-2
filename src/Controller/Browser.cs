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
	    private bool isActivated = false;
		
		protected bool mouseOver = true;
		
		public bool MouseOver {
			get { return mouseOver; }
		}

	    protected Point lastScreenPos = new Point(-1,-1);

	    public bool IsActivated
	    {
	        get { return isActivated; }
	        set {
                if(value)
                {
                    mouseOver = true;
                    //mousePos = new Point(0, 0); // mysz nad obszarem
                }
                else
                {
                    mouseOver = false;
                   // mousePos = new Point(-1, -1); // mysz nad obszarem
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
           // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser: " + mousePos.X + " , " + mousePos.Y + ", over : " + Bounds.Contains(mousePos.X, mousePos.Y));
            //	Console.WriteLine(mouse.X.abs + " "+ mouse.X.rel);
	        return mouseOver;
           // return this.webBrowser1.Bounds.Contains(MousePos.X, MousePos.Y);

        }	

	    public bool IsMouseOver(uint screenX, uint screenY)
		{
            Point screen = new Point((int)screenX, (int)screenY);
            Point client = this.webBrowser1.PointToClient(screen);

            lastScreenPos = screen;
           // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "main: " + mouse.X.abs + " , " + mouse.Y.abs + ", over : " + Bounds.Contains(mouse.X.abs, mouse.Y.abs));
			//Console.WriteLine(mouse.X.abs + " "+ mouse.X.rel);

            return this.webBrowser1.Bounds.Contains(client);

          
			
		}		
		
		
		public Browser()
		{
           
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

          
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

	    private void Document_MouseLeave(object sender, HtmlElementEventArgs e)
	    {
	        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser LEAVE");
	        mouseOver = false;
           // mousePos = new Point(-1,-1); // mysz poza obszarem
           // Console.WriteLine("bbb");
	    }

	    private void Document_MouseOver(object sender, HtmlElementEventArgs e)
	    {
            mouseOver = true;
	        mousePos = e.MousePosition;
           // Console.WriteLine("aaa");
	    }

        private Point mousePos = new Point(0, 0);

	    void Document_MouseMove(object sender, HtmlElementEventArgs e)
        {
           // e.MousePosition
	        mousePos = e.MousePosition;
            
          
	    }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          //  LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "document loaded");
            this.webBrowser1.Document.MouseMove += new HtmlElementEventHandler(Document_MouseMove);
            this.webBrowser1.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            this.webBrowser1.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
        }

        private void Browser_Activated(object sender, EventArgs e)
        {
           // Console.WriteLine(" !!!!!!!!!!!!!! ACTIVATED !!!!!!!!!!!!!!");
            if(lastScreenPos.X >= 0)
            {
                Cursor.Position = lastScreenPos;
            }
               
            
        }

	}
}
