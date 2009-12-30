/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
namespace Wof.Controller
{
	partial class Browser
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(0, 0);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(292, 266);
			this.webBrowser1.TabIndex = 0;
			this.webBrowser1.Url = new System.Uri("http://www.wingsoffury2.com", System.UriKind.Absolute);
			// 
			// Browser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.webBrowser1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Browser";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Browser";
			this.MouseEnter += new System.EventHandler(this.BrowserMouseEnter);
			this.MouseLeave += new System.EventHandler(this.BrowserMouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BrowserMouseMove);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.WebBrowser webBrowser1;
	}
}
