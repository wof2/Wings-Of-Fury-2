using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Wingitor;
using Wof.Model.Level;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.XmlParser;

namespace wingitor
{
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void shipTileTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void OnLevelLoaded(XmlLevelParser parser)
        {
             
            this.timeToFirstEnemy.Text = parser.TimeToFirstEnemyPlane.ToString();
            this.timeToNextEnemy.Text = parser.TimeToNextEnemyPlane.ToString();
            this.dayTime.SelectedText = parser.DayTime.ToString();
            this.missionType.SelectedText = parser.MissionType.ToString();
           
            List<LevelTile> tiles = parser.Tiles;

            levelTiles.Items.Clear();
            List<OceanTile> oceanTiles = new List<OceanTile>();
            foreach (LevelTile tile in tiles)
            {
                if(tile is OceanTile)
                {                	
                	oceanTiles.Add(tile as OceanTile);
                } else
                {
                	if(oceanTiles.Count > 0)
                	{
                		levelTiles.Items.Add("Ocean - width = " + oceanTiles.Count);
                		oceanTiles.Clear();
                	}else
                	{
                		levelTiles.Items.Add(tile.GetXMLName);
                	}
                	
                }
                
            }
            
            // na koniec
            if(oceanTiles.Count > 0)
        	{
        		levelTiles.Items.Add("Ocean - width = " + oceanTiles.Count);
        		oceanTiles.Clear();
        	}

        }

        private void deleteLevelTile_Click(object sender, EventArgs e)
        {
          
            Level level = mainWindow.EditorRenderPanel.CurrentLevel;
            foreach (int index in levelTiles.SelectedIndices)
            {
                levelTiles.Items.RemoveAt(index);
                level.LevelTiles.RemoveAt(index);
               
            }
            mainWindow.EditorRenderPanel.ReloadLevel();

            
        }

        private MainWindow mainWindow;
        public void SetMainWindow(MainWindow window)
        {
            this.mainWindow = window;
        }
    }
}
