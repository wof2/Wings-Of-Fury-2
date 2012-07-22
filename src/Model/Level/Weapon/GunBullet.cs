/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-06-25
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Mogre;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;

namespace Wof.Model.Level.Weapon
{
	/// <summary>
	/// Description of FlakBullet.
	/// </summary>
	public class GunBullet : MissileBase
	{
		
		protected static Random mRand  = new Random();
		protected readonly float maxFlyingDistance;
		
		protected float travelledDistance = 0;
	
		protected const float baseMaxDistance = 200;

	    private bool isReversed;

	    private bool isDoubleView;
	    
	    protected float planeTurningProgress;
	    
		public float PlaneTurningProgress {
			get { return planeTurningProgress; }
		}

	    private Quaternion launchOrientation;

        // (reversed ? Mogre.Math.PI - fireAngle : fireAngle)

		public GunBullet(float x, float y, Quaternion launchOrientation, Level level, IObject2D owner, bool reversed, bool doubleView)
            : base(x, y, (reversed ? -1 : 1) * owner.MovementVector, level, 0, owner)
        {

		    this.launchOrientation = launchOrientation;
			// flyVector = new PointD(GameConsts.Rocket.BaseSpeed, GameConsts.Rocket.BaseSpeed);
        
		     isReversed = reversed;
		     isDoubleView = doubleView;
             boundRectangle = new Quadrangle(new PointD(x, y), 1, 1);  			  
             maxFlyingDistance = baseMaxDistance * mRand.Next(90, 110) / 100.0f;      
			 this.planeTurningProgress = planeTurningProgress;  
             position3 = new Vector3(x,y,0);
        }

        /// <summary>
        /// Czy kierunek lotu poczatkowo byl przeciwny do wlasciciela pocisku? (B25)
        /// </summary>
	    public bool IsReversed
	    {
	        get { return isReversed; }
        }

        /// <summary>
        /// Czy w widoku ma byc 2krotny pocisk
        /// </summary>
        public bool IsDoubleView
        {
            get { return isDoubleView; }
        }

	    public Vector3 Position3
	    {
	        get { return position3; }
	    }

	    public Quaternion LaunchOrientation
	    {
	        get { return launchOrientation; }
	    }


	    protected override bool OutOfFuel() {
			if(!base.OutOfFuel()) {
				
				if(travelledDistance >= maxFlyingDistance) {
					Destroy();
					return true;
				}
			}
			return false;
		}
				
		public float GetDamage(IObject2D obj) {
			
          
			float dist = (obj.Bounds.Center - Position).EuclidesLength;

            if (obj.Bounds.Intersects(this.Bounds)){

                float damage = GameConsts.Gun.BaseDamage * GameConsts.UserPlane.Singleton.HitCoefficient;
            	return damage;
            }
            
            return 0;
            
		}
		
		protected override void ChangePosition(int time)
        {
            float coefficient = Mathematics.GetMoveFactor(time, MoveInterval);

            timeCounter += time;


            Vector3 v3d =  Vector3.NEGATIVE_UNIT_Z* GameConsts.Gun.BaseSpeed;
            v3d = launchOrientation * v3d;
            v3d *=  coefficient;
                 
            boundRectangle.Move(v3d.x, v3d.y);
            
            moveVector = new PointD(v3d.x, v3d.y); // orientacyjnie bo inne metody z tego korzystaja
            position3 += v3d;
            travelledDistance += v3d.Length;
   
           
        }

	    private Vector3 position3 = new Vector3();

		
		protected override void CheckCollisionWithUserPlane()
		{
			Wof.Model.Level.Planes.Plane p = refToLevel.UserPlane;
            if (p != null)
            {
            	
            	bool hit = false;
                float damage = GetDamage(p);              
                
                if(damage>0)
                {
                  
	             	refToLevel.UserPlane.Hit(damage, 0);     
	               	hit=true;	   
	                //powiadamia controler o trafieniu.	               

                    Destroy();
                }
                              
            }
		}

        protected override void CheckCollisionWithEnemyPlanes()
        {
            if (refToLevel.EnemyPlanes.Count > 0)
            {
                foreach (EnemyPlane ep in refToLevel.EnemyPlanes)
                {
                    if (this.Owner == ep) continue;

                    //sprawdzam czy aby nie ma zderzenia.
                    if (boundRectangle.Intersects(ep.Bounds))
                    {
                        //niszczy wrogi samolot
                        //ubytek paliwa.
                        ep.Hit(true);

                        //komunikat do controllera.
                        refToLevel.Controller.OnGunHitPlane(ep);

                        //zwiekszam liczbe trafionych obiektow przez rakiete
                        refToLevel.Statistics.HitByGun++;

                        //niszcze pocisk
                        Destroy();
                    }
                }
            }
        }
		protected override void CheckCollisionWithGround()
		{
			if(this.Position.Y >= 15) {
				return;
            } 
            LevelTile tile;
            int index = Mathematics.PositionToIndex(Position.X);
            
            if (index >= 0 && index < refToLevel.LevelTiles.Count)
            {
            	tile = refToLevel.LevelTiles[index];
             	CollisionType c = tile.InCollision(this.boundRectangle);
            	if (c == CollisionType.None) return;
            	
            	//jesli nie da sie zniszczyc dany obiekt z dzialka.
                if(c == CollisionType.Hitbound || c == CollisionType.CollisionRectagle)
                {
                    if (refToLevel.LevelTiles[index] is BarrelTile)
                    {
	                    BarrelTile barrel = refToLevel.LevelTiles[index] as BarrelTile;
	                    if (!barrel.IsDestroyed)
	                    {
	                        barrel.Destroy();
	                        refToLevel.Controller.OnTileDestroyed(barrel, null);
	                        this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 2, false);
	                    }
	                }
                    else 
                    {
	                    this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 0, false);
                    }
	                  	
                } 
                else if(c == CollisionType.Altitude) 
                {
                	//refToLevel.Controller.OnTileBombed(tile, this);
                }
                
                this.Destroy();
            	refToLevel.Controller.OnGunHit(refToLevel.LevelTiles[index], Position.X, System.Math.Max(this.Position.Y, 1));

               
            }
		}
	}
}
