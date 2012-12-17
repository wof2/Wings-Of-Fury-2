using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wof.Controller
{
    public partial class ErrorBox : Form
    {
        public ErrorBox(String errorTitle, String errorMessage)
        {
            InitializeComponent();
            this.textBox1.Text = errorMessage;
            titleLabel.Text = errorTitle;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openLogLocationButton_Click(object sender, EventArgs e)
        {
            Process.Start(EngineConfig.C_LOCAL_DIRECTORY);
            //Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(Game.GetDefaultBrowserPath(), EngineConfig.C_WOF_SUPPORT_PAGE);
        }
    }
}
