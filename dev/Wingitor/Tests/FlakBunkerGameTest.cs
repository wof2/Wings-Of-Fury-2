using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using wingitor;
using Wingitor;
using Wof.Controller;
using Wof.Model.Level.XmlParser;

namespace wingitor.Tests
{
    public class FlakBunkerGameTest : IGameTest
    {
        readonly List<ISceneTest> sceneTests = new List<ISceneTest> { new FlakBunkerTestScene() };

        public string LevelFilename
        {
            get { return "levels/level-16" + XmlLevelParser.C_LEVEL_POSTFIX; }
          //  get { return "levels/level-1" + XmlLevelParser.C_LEVEL_POSTFIX; }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow mw = new MainWindow(new FlakBunkerGameTest());
            Application.Run(mw);
        }

        #region Implementation of IGameTest

        public FlakBunkerGameTest()
        {
           
        }
       
        public IList<ISceneTest> SceneTests
        {
            get { return sceneTests; }
        }

        #endregion

      

    }
}
