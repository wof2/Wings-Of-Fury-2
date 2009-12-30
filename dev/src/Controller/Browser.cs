/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
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
		
		
		protected bool mouseOver = false;
		
		public bool MouseOver {
			get { return mouseOver; }
		}
		
		public bool IsMouseOver(MouseState_NativePtr mouse)
		{
			Console.WriteLine(mouse.X.abs + " "+ mouse.X.rel);
			return this.Bounds.Contains(mouse.X.abs, mouse.Y.abs);
			
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
		
		void BrowserMouseLeave(object sender, EventArgs e)
		{
			mouseOver = false;
		}
		
		void BrowserMouseEnter(object sender, EventArgs e)
		{
			mouseOver = true;
		}
		
		void BrowserMouseMove(object sender, MouseEventArgs e)
		{
			
		}
	}
}
