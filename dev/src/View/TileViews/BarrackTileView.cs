using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Math=Mogre.Math;

namespace Wof.View.TileViews
{
    public class BarrackTileView : EnemyInstallationTileView
    {
        public BarrackTileView(LevelTile levelTile, FrameWork framework) : base(levelTile, framework)
        {
        }
        

        protected void initBarrack(SceneNode islandNode, float positionOnIsland)
        {
            String nameSuffix = tileID.ToString();

            installationEntity = sceneMgr.CreateEntity("Barracks" + nameSuffix, "Barracks.mesh");
            installationNode =
                islandNode.CreateChildSceneNode("Barracks" + nameSuffix, new Vector3(0, 0, positionOnIsland - 5.0f));
            installationNode.AttachObject(installationEntity);
            installationNode.Scale(new Vector3(1.0f, 1.2f, 1.0f));

            installationNode.Translate(new Vector3(0.0f, levelTile.HitBound.LowestY, 0.0f));

            if (LevelView.IsNightScene && EngineConfig.ShadowsQuality == 0) // cienie nie lubia innych zrodel swiatla :/
            {
                InitLightFlare(new ColourValue(1f, 1f, 0.9f), new Vector3(0, 1.9f, -2.6f), new Vector2(1.5f, 1.5f));
            }

            if (FrameWork.DisplayMinimap)
            {
                minimapItem =
                    new MinimapItem(installationNode, FrameWork.MinimapMgr, "Cube.mesh", new ColourValue(0, 0.8f, 0),
                                    installationEntity);
                minimapItem.ScaleOverride = new Vector2(0, 13); // stala wysokosc bunkra, niezale¿na od bounding box
                minimapItem.Refresh();
            }
        }

        public override void initOnScene(SceneNode parentNode, int tileCMVIndex, int compositeModelTilesNumber) 
        {
            base.initOnScene(parentNode, tileCMVIndex, compositeModelTilesNumber);
            initBarrack(parentNode, -getRelativePosition(parentNode, LevelTile));

            int variant = ((IslandTile) LevelTile).Variant;

            switch (variant)
            {
                case 0:
                    break;
                    //Flaga na srodku
                case 1:
                    initFlag(new Vector3(0, 2.2f, 0));
                    break;
            }
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {
            base.updateTime(timeSinceLastFrameUpdate);
           
            // miganie swiatla
            if ((levelTile as BarrackTile).IsDestroyed)
            {
                if (Math.RangeRandom(0.0f, 1.0f) > 0.9f)
                {
                    SetLightFlareVisibility(false);
                }
                if (Math.RangeRandom(0.0f, 1.0f) > 0.9f)
                {
                    SetLightFlareVisibility(true);
                }
            }
        }

        public override void Restore()
        {
            
        }

        public override void GunFire()
        {
            
        }

        public override void Destroy()
        {
            base.Destroy();
            ViewHelper.ReplaceMaterial(installationEntity, "Wood", "DestroyedWood");

            if (FrameWork.DisplayMinimap)
            {
                //Kolor szary
                minimapItem.Colour = new ColourValue(0.752f, 0.752f, 0.752f);
            }
        }
    }
}
