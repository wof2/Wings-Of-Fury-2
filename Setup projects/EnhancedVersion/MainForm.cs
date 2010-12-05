using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Wof.Controller;
using Wof.Controller.Screens;
namespace EnhancedVersionHelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Hide();
            DialogResult result =
                MessageBox.Show(
                    "This wizard helps you to get Enhanced version quickly and easily.\nInternet connection is required to continue.",
                    "Get your WOF2 Enhanced Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                // show wizard home
                webBrowser1.Url = new Uri(Game.GetEnhancedVersionHelperWebPageNakedUrl(), UriKind.Absolute);
                webBrowser1.Show();
            
            }
           
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
    
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void webBrowser1_LocationChanged(object sender, EventArgs e)
        {

        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string upd2 = "\nYou WOF2 (v."+ EngineConfig.C_WOF_VERSION+") is not up to date - please visit " + EngineConfig.C_WOF_HOME_PAGE + " to download the newest patch.";
            if (e.Url.ToString().Contains("/page/enhanced?status=ok"))
            {
                string upd = "";
                if (StartScreen.CheckAvailableUpdatesDo())
                {
                    upd = upd2;
                }
                MessageBox.Show(
                   "Your request will be reviewed as soon as possible. Check your inbox in a couple of hours." + upd,
                   "Thank you!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Dispose();
            }
            if (e.Url.ToString().Contains("/page/enhanced?status=already"))
            {
                string upd = "";
                if (StartScreen.CheckAvailableUpdatesDo())
                {
                    upd = upd2;
                }
                MessageBox.Show(
                   "Your request has already been sent. Please be patient. Check your inbox in a couple of hours or so." + upd,
                   "Thank you!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Dispose();
            }

            if (e.Url.ToString().Contains("/page/enhanced?status=error"))
            {
                MessageBox.Show(
                   "Error while sending request. Please check the data you entered and make sure the Internet connection is established. If the problem persists please check our forum for support: http://wingsoffury2.com",
                   "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Dispose();
            }
        }
    }
}
