using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wof.Controller;
using Wof.View.TileViews;

namespace Wof.View
{
    class BackGroundDummyIslandView : IslandView
    {
        public const string C_DUMMY_ISLAND_ROUND = "IslandRound";
        public const string C_DUMMY_ISLAND_LAGUNA = "Laguna";
        public const string C_DUMMY_ISLAND_DLAGUNA = "DoubleLaguna";
        public const string C_DUMMY_ISLAND_6 = "Island6";
        public const string C_DUMMY_RADAR_DOME = "RadarDome";

        

        public BackGroundDummyIslandView(int indexTile, string meshName, IFrameWork framework, SceneNode parentNode) : base(indexTile, framework, parentNode)
        {
            this.meshName = meshName;
            this.count = 10; // na razie testowo. Nie ma tileviewsow wiec trzeba ustawic jakas dlugosc
            initOnScene();
        }

      
    }
}
