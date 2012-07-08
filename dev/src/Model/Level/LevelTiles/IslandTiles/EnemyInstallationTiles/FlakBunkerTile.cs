/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-06-25
 * Time: 11:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.XmlParser;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{
	/// <summary>
	/// Description of FlakBunkerTile.
	/// </summary>
	public class FlakBunkerTile : BunkerTile
	{
		   #region Public Constructor

        /// <summary>
        /// Konstruktor szescioparametrowy. Tworzy 
        /// nowy bunkier drewniany na planszy.
        /// </summary>
        /// <param name="yBegin">Poczatek bunkru.</param>
        /// <param name="yEnd">Koniec bunkru.</param>
        /// <param name="hitBound">Prostokat opisujacy bunkier.</param>
        /// <param name="soldierNum">Liczba zolnierzy.</param>
        /// <param name="type">Typ bunkru.</param>
        /// <param name="collisionRectangle">Lista prostokatow z ktorymi moga wystapic zderzenia.</param>
        public FlakBunkerTile(float yBegin, float yEnd,float viewXShift, Quadrangle hitBound, int soldierNum, int generalNum, int type,
                              List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, soldierNum, generalNum, type, collisionRectangle)
        {
            //instalacja przy starcie nie jest zniszczona.
            enemyState = EnemyInstallationState.Intact;
            //pole razenia Ustawione podczas ustawiania indeksu.
            horizon = null;
            currentTime = 0;
            
            MinAngle = Mogre.Math.PI / 6.0f;
        	MaxAngle = 5 * Mogre.Math.PI / 6.0f;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Odbudowuje drewniany bunkier.
        /// </summary>
        public override void Reconstruct()
        {
            base.Reconstruct();
        }
        
        /// <summary>
        /// Wartosc od 1 do 2. Gdzie 1.0 to pelna celnosc. 2.0 to najgorsza
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static float GetAccuracyCoefficient(float distance)
        {
        	if(distance < GameConsts.FlakBunker.HorizonMinDistance || distance > GameConsts.FlakBunker.HorizonMaxDistance)
        	{
        		return 2.0f;
        	}
        	float bestRange =  (GameConsts.FlakBunker.HorizonMaxDistance - GameConsts.FlakBunker.HorizonMinDistance) / 2;
        	
        	return 1 +  Math.Abs(bestRange - distance) / bestRange;
        	
        }
        
        protected bool IsFireConditionMet
        {
        	get 
        	{
        		if(GameConsts.FlakBunker.HorizonMinAltitude > refToLevel.UserPlane.Bounds.LowestY) 
        		{
        			return false;
        		}
        		float dist = refToLevel.UserPlane.DistanceToTile(this);
        		return dist >=	GameConsts.FlakBunker.HorizonMinDistance && dist <=	GameConsts.FlakBunker.HorizonMaxDistance;
        		
        	}
        }
        
      

        /// <summary>
        /// Prowadzi ostrzal samolotu.
        /// </summary>
        public override void Fire(int time)
        {
            base.Fire(time);
            //jesli nie jest zniszczony i samolot jeszcze jest caly
            if (!IsDestroyed && UserPlaneNotYetDestroyed)
            {
            	bool fireCondition = IsFireConditionMet;
            	//wyliczam kat
                SetAngle();
                
                //jesli uplynal czas od ostatniego strzalu.
                if (currentTime > GameConsts.FlakBunker.FireDelay)
                {
                    //jesli samolot jest w polu razenia.
                    
                    if (fireCondition)
                    {
                        //zadaje uszkodzenia.
                        
                        if (angle > Mogre.Math.HALF_PI)
		                {
		                    angle = Mogre.Math.PI - angle;
		                }		                

                        FlakBullet bullet = weaponManager.FlakFire(refToLevel.UserPlane, angle);
                        Console.WriteLine("ANGLE: "+angle);
                      	//Zeruje licznik. Czekam kolejna sekunde.
	                    currentTime = 0;
                        
                    }
                }
                else //zwiekszam odstep czasu od ostatniego strzalu
                    currentTime += time;

                
            }
        }

        /// <summary>
        /// Funkcja niszczy instalacje obronna.
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
        }

        #endregion

        #region Properties

        public override string GetXMLName
        {
            get { return Nodes.FlakBunker; }
        }
        /// <summary>
        /// Pobiera lub ustawia index obiektu.
        /// </summary>
        public override int TileIndex
        {
            get { return base.TileIndex; }
            set
            {
                //indeks obiektu na liscie
                base.TileIndex = value; 

				horizon = new Quadrangle(new PointD(value*TileWidth - GameConsts.FlakBunker.HorizonMaxDistance/2, 5),
                                         GameConsts.FlakBunker.HorizonMaxDistance,
                                         GameConsts.FlakBunker.HorizonMinDistance);                
            }
        }
      

        #endregion
		
	}
}
