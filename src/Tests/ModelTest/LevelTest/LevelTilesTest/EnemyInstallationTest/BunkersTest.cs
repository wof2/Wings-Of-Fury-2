using System;
using System.Text;
using System.Collections.Generic;
using Wof.View;
using Wof.Controller;
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Planes;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using System.Threading;

namespace View.src.Tests.ModelTest.LevelTest.LevelTilesTest.EnemyInstallationTest
{
    public class TestClass
    {
        /*public static void Main()
        {
            BunkersTest testB = new BunkersTest();
            testB.SoldierRegistryTest();
        }*/
    }

    class BunkersTest
    {
        private Level level;
        private SimpleController simpleController;

        public BunkersTest()
        {
            try
            {
                simpleController = new SimpleController();
                string[] levelsName = LevelsManager.GetAvailableLevels();
                if (levelsName != null && levelsName.Length > 0)
                {
                    this.level = new Level(levelsName[0], simpleController);
                }
            }
            catch (Exception exc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exc.ToString());
                Console.WriteLine();
                Console.WriteLine(level.ErrorMessage);
            }
        }

        public void Test()
        {
            Random rand = new Random();
            this.level.UserPlane.SteerRight();
            int interval;
            while (true)
            {
                this.level.UserPlane.Move(1);
                interval = rand.Next(200, 600);
                this.level.Update(interval);
                Thread.Sleep(interval);
            }
        }

        public void SoldierRegistryTest()
        {
            BunkerTile bunker = null;
            foreach (LevelTile tile in this.level.BunkersList)
            {
                bunker = tile as BunkerTile;
                if (bunker != null)
                    bunker.Destroy();
            }
        }
    }

    class SimpleController : IController
    {
        private int count;

        public SimpleController()
        {
            this.count = 0;
        }

        #region IController Members

        public void OnBunkerFire(BunkerTile bunker, Plane plane)
        {
            Console.WriteLine(bunker.ToString() + "Samolot: " + plane.ToString()); 
        }

        public void OnRegistrySoldier(Soldier soldier)
        {
            count++;
            Console.WriteLine("Zolnierz numer: " + this.count + " " + soldier.ToString());
        }

        #endregion
    }
}
