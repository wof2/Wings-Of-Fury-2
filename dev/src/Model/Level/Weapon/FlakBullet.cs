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

namespace Wof.Model.Level.Weapon
{
	/// <summary>
	/// Description of FlakBullet.
	/// </summary>
	public class FlakBullet : Ammunition
	{
		
		public FlakBullet(float x, float y, Level level, IObject2D owner)
			: base(new PointD(0,0), level, 0, owner)
        {
             boundRectangle = new Quadrangle(new PointD(x, y), 0, 0);
             moveVector = new PointD(0, 0);
           
        }
		
		public float GetDamage(IObject2D obj) {
			
			float dist = (obj.Bounds.Center - Position).EuclidesLength;                
      
            if(dist < GameConsts.FlakBunker.DamageRange)
            {
            	float damageCoeff = ((GameConsts.FlakBunker.DamageRange - dist) / GameConsts.FlakBunker.DamageRange);
            	float damage = GameConsts.FlakBunker.MaxDamagePerHit * damageCoeff * GameConsts.UserPlane.Singleton.HitCoefficient;
            	return damage;
            }
            
            return 0;
            
		}
		
		
	}
}
