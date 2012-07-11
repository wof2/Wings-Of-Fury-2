/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-06-25
 * Time: 11:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;
using Mogre;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Weapon;
using Wof.Model.Level.XmlParser;
using Math=System.Math;

namespace Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles
{
	/// <summary>
	/// Description of FlakBunkerTile.
	/// </summary>
	public class FlakBunkerTile : BunkerTile
	{
	    #region Public Constructor

        protected const float  MinAngleRight = 1 * Mogre.Math.PI / 20.0f;
        protected const float  MaxAngleRight = 3 * Mogre.Math.PI / 6.0f;
       
        
        protected const float MinAngleLeft = 3 * Mogre.Math.PI / 6;
        protected const float MaxAngleLeft = 19 * Mogre.Math.PI / 20.0f;
        
        protected const float DirectionChangePerSecond = 2 * Mogre.Math.PI / 10.0f;

	    protected float yAngle = 0;

	    protected bool adjustingBarrelAfterDirectionChange = false;

        private const float Epislon = 0.001f;
        private Model.Level.Direction direction = Model.Level.Direction.Right;

        protected Model.Level.Direction initialDirection;

        protected const float BarrelAdjustmentSpeed = 0.001f;

       
         

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


            initialDirection = Model.Level.Direction.Right;

            direction = initialDirection;
            OnDirectionChanged();
        }

        protected void OnDirectionChanged()
        {
            if (direction == Model.Level.Direction.Right)
            {
                MinAngle = MinAngleRight;
                MaxAngle = MaxAngleRight;
            }

            if (direction == Model.Level.Direction.Left)
            {
                MinAngle = MinAngleLeft;
                MaxAngle = MaxAngleLeft;
            }

           // MinAngle = 0;
           // MaxAngle = Mogre.Math.PI;
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

                if (direction == Direction.Changing)
                {
                    return false;
                }

                if(adjustingBarrelAfterDirectionChange)
                {
                    return false;
                }

        		if(GameConsts.FlakBunker.HorizonMinAltitude > refToLevel.UserPlane.Bounds.LowestY) 
        		{
        			return false;
        		}
                if (Math.Abs(angle) >= MaxAngle || Math.Abs(angle) <= MinAngle)
                {
                    return false;
                }

        		float dist = refToLevel.UserPlane.DistanceToTile(this);
        		return dist >=	GameConsts.FlakBunker.HorizonMinDistance && dist <=	GameConsts.FlakBunker.HorizonMaxDistance;
        		
        	}
        }

       

        protected  void  YRotation(int time, float timeUnit)
        {
            if(this.IsDestroyed)
            {
                return;
            }
            if (direction == Direction.Right)
            {
                if (Mathematics.IndexToPosition(this.TileIndex) > this.refToLevel.UserPlane.Position.X)
                {
                    direction = Direction.Changing;
                    changeDirection = Direction.Left;
                }
            }

            if (direction == Direction.Left)
            {
                if (Mathematics.IndexToPosition(this.TileIndex) < this.refToLevel.UserPlane.Position.X)
                {
                    direction = Direction.Changing;
                    changeDirection = Direction.Right;
                }
            }
            float scaleFactor = time / timeUnit;

            if (direction == Direction.Changing)
            {
                float dir = changeDirection != initialDirection ? 1 : -1;

                yAngle += dir * DirectionChangePerSecond * scaleFactor;
                bool completed = false;
                if (dir == 1 && yAngle >= Mogre.Math.PI)
                {
                    completed = true;
                    yAngle = Mogre.Math.PI;
                }

                if (dir == -1 && yAngle <= 0)
                {
                    completed = true;
                    yAngle = 0;
                } 
                
                if(completed)
                {
                      adjustingBarrelAfterDirectionChange = true;
                      if (changeDirection.HasValue)
                      {
                          if (changeDirection == Direction.Left)
                          {
                              direction = Model.Level.Direction.Left;
                          }
                          else if(changeDirection == Direction.Right)
                          {
                              direction = Model.Level.Direction.Right;
                          }
                          OnDirectionChanged();
                      }
                    changeDirection = null;
                }
            }

         //   Console.WriteLine("direction: " + direction + ";  changeDirection: " + changeDirection + "; yAngle:" + yAngle+"; adjustingBarrelAfterDirectionChange: " + adjustingBarrelAfterDirectionChange );
        }

        public override void Update(int time, float timeUnit)
        {
            base.Update(time, timeUnit);
            YRotation(time, timeUnit);
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
            	
                //wyliczam kat tylko jesli nie obracamy
                if(!changeDirection.HasValue)
                {
                    float angleBefore = angle;
                    SetAngle();
                    if (adjustingBarrelAfterDirectionChange)
                    {
                        float diff = angle - angleBefore;
                        angle = angleBefore + diff * BarrelAdjustmentSpeed * time;
                       // Console.WriteLine("angle:" + angle + "; diff:" + diff);
                        if (Math.Abs(angle - angleBefore) < Epislon)
                        {
                            adjustingBarrelAfterDirectionChange = false;
                        }
                    }
                } else
                {
                    adjustingBarrelAfterDirectionChange = false; // przerwij
                }

               
                
                
                //jesli uplynal czas od ostatniego strzalu.
                if (currentTime > GameConsts.FlakBunker.FireDelay)
                {
                    //jesli samolot jest w polu razenia.
                    
                    if (fireCondition)
                    {
                        //zadaje uszkodzenia.
                        float localAngle = angle;
                        if (localAngle > Mogre.Math.HALF_PI)
		                {
                            localAngle = Mogre.Math.PI - localAngle;
		                }

                        FlakBullet bullet = weaponManager.FlakFire(refToLevel.UserPlane, localAngle);

                        //powiadamia controler o strzale.
                        refToLevel.Controller.OnBunkerFire(this, refToLevel.UserPlane, false);

                       // Console.WriteLine("ANGLE: "+angle);
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

        /// <summary>
        /// Kat obrotu wzgledem osi Y (czyli Yaw)
        /// </summary>
	    public float YAngle
	    {
	        get { return yAngle; }
	    }

        public override Direction Direction
        {
            get { return direction; }
        }

        private Model.Level.Direction? changeDirection = null;

        public Direction? ChangeDirection
        {
            get { return changeDirection; }

        }

	    #endregion
		
	}
}
