using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Wof.Controller;
using Wof.Tools;
namespace EnhancedVersionHelper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
              
                if (EngineConfig.IsEnhancedVersion)
                {
                    MessageBox.Show("You already have Wings of Fury 2: Return of the legend enhanced version!",
                                    "Wings of Fury 2 - Enhanced Version");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message+", "+exception.InnerException.Message+". "+exception.StackTrace);
             
            }
           
        }
    }
}
