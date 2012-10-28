/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-06-25
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
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
	public class BunkerShellBullet : MissileBase
	{
		
		protected IObject2D target;
		protected static Random mRand  = new Random();
		protected readonly float maxFlyingDistance;
		
		protected float travelledDistance = 0;

        public BunkerShellBullet(float x, float y, Level level, IObject2D owner, IObject2D target, float fireAngle, float initialSpeed)
			: base(x,y, GetInitialVector(owner, target, initialSpeed) , level, fireAngle, owner)
        {
			 this.target = target;
             boundRectangle = new Quadrangle(new PointD(x, y), 3, 3);  
			 PointD diffVector = (target.Center - owner.Center);             
             maxFlyingDistance = diffVector.EuclidesLength * mRand.Next(90, 110) / 100.0f;
             diffVector.Normalise();      
             diffVector.X *= -1;                        	
             SetZRotationPerSecond(diffVector.X * 2.9f); // zaginanie toru lotu do ziemi
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


		protected static PointD GetInitialVector(IObject2D owner, IObject2D target, float initialSpeed) {
			
			
        	float speedCoeff = 2 * target.MovementVector.EuclidesLength /  GameConsts.P47Plane.Singleton.MaxSpeed;
        	float distanceCoeff = 2.0f;
       
        	float xSpread = distanceCoeff * speedCoeff * target.Bounds.Width;
            float ySpread = distanceCoeff * speedCoeff * target.Bounds.Height;
            
            float xPos  = mRand.Next((int)(target.Bounds.Center.X - xSpread * 0.5f), (int)(target.Bounds.Center.X + xSpread * 0.5f));
            float yPos  = mRand.Next((int)(target.Bounds.Center.Y*1.0f - ySpread * 0.5f), (int)(target.Bounds.Center.Y*1.0f + ySpread * 0.5f));
            PointD flakPosition = new PointD(xPos, yPos);
            
          	PointD direction = (flakPosition - owner.Center);
            
            direction.Normalise();
            
            return initialSpeed *direction;
		}
		
		
		protected override void ChangePosition(int time)
        {
            float coefficient = Mathematics.GetMoveFactor(time, MoveInterval);

            timeCounter += time;
           
               // Console.WriteLine(flyVector.X);

            float minFlyingSpeed = Owner.IsEnemy ? GameConsts.EnemyPlaneBase.Singleton.RangeFastWheelingMaxSpeed * GameConsts.EnemyPlaneBase.Singleton.MaxSpeed : GameConsts.UserPlane.Singleton.RangeFastWheelingMaxSpeed * GameConsts.UserPlane.Singleton.MaxSpeed;


            // rakieta wytraca prędkość uzyskaną od samolotu
            if (Math.Abs(flyVector.X) > Math.Abs(minFlyingSpeed * GameConsts.Rocket.BaseSpeed))
            {
                flyVector.X *= 0.995f;
            }

            if (Math.Abs(flyVector.Y) > Math.Abs(minFlyingSpeed * GameConsts.Rocket.BaseSpeed))
            {
                flyVector.Y *= 0.995f;
            }

            float angle = zRotationPerSecond * coefficient;
            //  boundRectangle.Rotate(angle);
            //  moveVector.Rotate(PointD.ZERO, angle);
            relativeAngle += angle * (int)Direction;
            flyVector.Rotate(PointD.ZERO, angle);

            PointD vector = new PointD(flyVector.X * coefficient, flyVector.Y * coefficient);
            boundRectangle.Move(vector);
            moveVector = vector; // orientacyjnie bo inne metody z tego korzystaja
            
            travelledDistance += vector.EuclidesLength;
           
        }

	    protected override void CheckCollisionWithEnemyPlanes()
	    {
	        
	    }

	    protected override void CheckCollisionWithUserPlane()
		{
			Plane p = refToLevel.UserPlane;
            if (p != null)
            {
            	
            	bool hit = false;
                
                //if((this.Position - p.Position).EuclidesLength < boundRectangle.Width * 2 )
                if (boundRectangle.Intersects(p.Bounds))
                {
                    p.Hit(this.ammunitionOwner);
                    Destroy();
                }
                              
            }
		}
		
		protected override void CheckCollisionWithGround()
		{
			
			if(this.Position.Y < 0) {
				Destroy();
			}
	
		}
	}
}
