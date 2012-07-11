/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-07-08
 * Time: 18:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Wof.Controller;
using Wof.View.AmmunitionViews;

namespace Wof.View
{
	/// <summary>
	/// Description of AmmunitionViewFactory.
	/// </summary>
    internal class AmmunitionViewFactory
	{
		protected Type type;
		public AmmunitionViewFactory(Type type)
		{
			this.type = type;
				
			
		}
		
		public AmmunitionView GetAmmunitionView(IFrameWork frameWork) {
			
			
			if(type==typeof(RocketView)) {
				return new RocketView(frameWork);
			}			
		
			if(type==typeof(FlakBulletView)) {
				return new FlakBulletView(frameWork);
			}
			
			if(type==typeof(GunBulletView)) {
				return new GunBulletView(frameWork);
			}

            if (type == typeof(BunkerShellBulletView))
            {
                return new BunkerShellBulletView(frameWork);
            }			
			
			
			throw new NotImplementedException();
		}
		
		
		
	}
}
