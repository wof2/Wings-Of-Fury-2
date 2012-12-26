using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Wof.Controller;
using Wof.Controller.Screens;
using Wof.Tools;

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
        public string getEnhancedZipFileLocation()
        {
            String downloadPath = EngineConfig.getLocalDirectoryByReflection();
            downloadPath = Path.Combine(downloadPath, @"enhanced.zip");
            return downloadPath;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document != null && webBrowser1.Document.Body != null && webBrowser1.Document.Body.InnerHtml.Contains("WOF_ENHANCED_DOWNLOAD_LINK"))
            {
                Match match = Regex.Match(webBrowser1.Document.Body.InnerHtml, @"WOF_ENHANCED_DOWNLOAD_LINK:(.*) ", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    // Finally, we get the Group value and display it.
                    string url = match.Groups[1].Value;
                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(enhancedVersion_DownloadFileCompleted);
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(enhancedVersion_DownloadProgressChanged);

                        webClient.DownloadFileAsync(new Uri(url), getEnhancedZipFileLocation());
                    }
                }

            }
        }

        private void enhancedVersion_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            HtmlElement element = getDownloadProgressElement(this.webBrowser1.Document);
            if (element != null)
            {
                element.InnerHtml = "<strong>Download progress: " + e.ProgressPercentage + "%</strong>";
            }

        }

        private HtmlElement getDownloadProgressElement(HtmlDocument doc)
        {
            if (doc != null && doc.Body != null && doc.Body.InnerHtml.Contains("WOF_ENHANCED_DOWNLOAD_LINK"))
            {
                HtmlElement element = webBrowser1.Document.GetElementById("download_progress");
                return element;

            }
            return null;

        }
        private void enhancedVersion_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            HtmlElement element = getDownloadProgressElement(webBrowser1.Document);
            if (element != null)
            {

                string location = getEnhancedZipFileLocation();
                if (File.Exists(location))
                {
                    Zipfiles.ExtractZipFile(location, EngineConfig.C_LOCAL_DIRECTORY);
                    element.InnerHtml = "<strong>Download completed - you can now play Wings of Fury 2 Enhanced edition.</strong>";
                    element.InnerHtml += "<br />For future reference your license file is stored in directory: " + location;
                }
                else
                {
                    element.InnerHtml = "Download completed but file does not exist on local system. Please install Enhanced version manually. Check your e-mail for instructions";
                }
            }
        }

        private void webBrowser1_LocationChanged(object sender, EventArgs e)
        {
      
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.Document.Title.Contains("Internet Explorer cannot display the webpage"))
            {
                MessageBox.Show(
                 "No Internet connection!",
                 "yaiks!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
