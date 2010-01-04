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
			this.wofBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// wofBrowser
			// 
			this.wofBrowser.AllowWebBrowserDrop = false;
			this.wofBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.wofBrowser.IsWebBrowserContextMenuEnabled = false;
			this.wofBrowser.Location = new System.Drawing.Point(0, 0);
			this.wofBrowser.Margin = new System.Windows.Forms.Padding(0);
			this.wofBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.wofBrowser.Name = "wofBrowser";
			this.wofBrowser.ScriptErrorsSuppressed = true;
			this.wofBrowser.Size = new System.Drawing.Size(300, 300);
			this.wofBrowser.TabIndex = 1;
			this.wofBrowser.TabStop = false;
			this.wofBrowser.Url = new System.Uri("http://www.wingsoffury2.com", System.UriKind.Absolute);
			this.wofBrowser.WebBrowserShortcutsEnabled = false;
			this.wofBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
			// 
			// Browser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.Controls.Add(this.wofBrowser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Browser";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Browser";
			this.Activated += new System.EventHandler(this.Browser_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowserFormClosing);
			this.ResumeLayout(false);
        }
		private System.Windows.Forms.WebBrowser wofBrowser;

	}
}
