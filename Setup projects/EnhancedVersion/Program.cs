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
                if (!Licensing.CanBuildEnhancedVersionHash())
                {
                    // nie udalo sie stworzyc hasha. Ktos probuje odpalic wersje rozszerzona
                    MessageBox.Show("Unable to build Enhanced version hash.\r\nWe are sorry but " + EngineConfig.C_GAME_NAME +
                                    " Enhanced version cannot be run under Windows Guest Account. Please run the game under Administrator account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
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
