using System;
using System.Collections.Generic;
using System.Windows.Forms;

using wingitor;

namespace Wingitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  MainWindow mw = new MainWindow();
           // Application.Run(mw);
           // mw.Go();
           Application.Run(new Form1());
        }
    }
}
