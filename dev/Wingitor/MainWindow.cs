using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mogre;

namespace Wingitor
{
    public partial class MainWindow  : Form
    {
        private Thread renderThread;
        public MainWindow()
        {
            InitializeComponent();

            if(!renderPanel.Setup())
            {
                return;
            }
            renderThread = new Thread(renderPanel.Go);
            renderThread.Start();
           

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                renderPanel.Dispose();
                renderThread.Join();

            }
            catch (Exception)
            {
            }
           
        }

       
     

       
    }
}
