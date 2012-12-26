

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mogre;
using wingitor;
using Wof.Controller;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.XmlParser;

namespace Wingitor
{
    public partial class MainWindow  : Form
    {
        private Thread renderThread;
        
        protected System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        
        public EditorRenderPanel EditorRenderPanel
        {
            get { return editorRenderPanel; }
        }
        
        private void DebugBoxUpdater(Object myObject,
                                            EventArgs myEventArgs) {
        	lock (lockSync)
            {
	        	this.listBox1.Items.Clear();
	            foreach (var info in debugInfos)
	            {
	                this.listBox1.Items.AddRange(info.ToStringArray());
	                this.listBox1.Items.Add("");
	            }
        	}
        }

        public MainWindow(IGameTest gameTest)
        {
	
            InitializeComponent();
            
             timer.Tick += new EventHandler(DebugBoxUpdater);

		    // Sets the timer interval to 5 seconds.
		    timer.Interval = 500;
		    timer.Start();
       
       
            editorRenderPanel.SetGameTest(gameTest);
            if(!editorRenderPanel.Setup())
            {
                return;
            }
            OnLevelLoaded(editorRenderPanel.CurrentLevel.LevelParser);
              
            renderThread = new Thread(editorRenderPanel.Go);
            renderThread.Start();
            this.menu.SetMainWindow(this);
           

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                editorRenderPanel.Running = false;
                renderThread.Join();

            }
            catch (Exception)
            {
            }
           
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void quit(object sender, EventArgs e)
        {
            Close();
        }

        public void OnLevelLoaded(XmlLevelParser parser)
        {
            this.Text = EngineConfig.C_GAME_NAME + " level editor ( " + parser.MissionType + " ): " + parser.LevelFile;
            this.menu.OnLevelLoaded(parser);
         
        }

        private List<DebugInfo> debugInfos = new List<DebugInfo>();
        private object lockSync = new object();

        public void UpdateDebugBox(DebugInfo debugInfo)
        {
            lock (lockSync)
            {
                int index = debugInfos.IndexOf(debugInfo);
                if (index >= 0)
                {
                    debugInfos[index] = debugInfo;
                }
                else
                {
                    debugInfos.Add(debugInfo);

                }

            }

        }
        
    

        private void load(object sender, EventArgs e)
        {
           DialogResult result = openFileDialog.ShowDialog();
           if (result == DialogResult.OK)
           {
               try
               {
                   if (editorRenderPanel != null)
                   {
                       editorRenderPanel.ReloadLevel(openFileDialog.FileName);
                   }
                  
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message, "Error while loading level");
               }
               
           }

        }
        private void reload(object sender, EventArgs e)
        {
            editorRenderPanel.ReloadAllResourcesNextFrame();
        }

        private void save(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "WOF2 level file (*" + XmlLevelParser.C_LEVEL_POSTFIX + ")|*" + XmlLevelParser.C_LEVEL_POSTFIX;
            saveFileDialog.DefaultExt = XmlLevelParser.C_LEVEL_POSTFIX.Substring(1);
            DialogResult result = saveFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {

                try
                {
                    XmlLevelParser.SaveLevel(editorRenderPanel.CurrentLevel, saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while saving level");   
                }
               
                
            }
        }
     

       
    }

    
}
