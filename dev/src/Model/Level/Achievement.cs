/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-11-02
 * Time: 19:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

using Wof.Misc;

namespace Wof.Model.Level
{
		
	/// <summary>
	/// Description of Achievement.
	/// </summary>
	[Serializable()]
	public class Achievement : IEquatable<Achievement>
	{
		
		private AchievementType type;
		
		
		public delegate void AchievementFulfilledDelegate(Achievement a, bool playSound);
		public delegate void AchievementUpdatedDelegate(Achievement a);
		
	//	[field: NonSerialized()]
		[XmlIgnore]
		protected AchievementFulfilledDelegate onFulfilled;
		
		[XmlIgnore]
		public Achievement.AchievementFulfilledDelegate OnFulfilled {
			get { return onFulfilled; }
			set { onFulfilled = value; }
		}
		
		[XmlIgnore]
		protected AchievementUpdatedDelegate onUpdated;
		
		[XmlIgnore]
		public Achievement.AchievementUpdatedDelegate OnUpdated {
			get { return onUpdated; }
			set { onUpdated = value; }
		}
		
		
		public AchievementType Type {
			get { return type; }
			set { type = value; }
		}
		private int amount;
		
		public int Amount {
			get { return amount; }
			set { amount = value; }
		}
		
		private int amountDone = 0;
		
	
		public int AmountDone {
			get { return amountDone; }
			set { 
				int  amountDoneBefore = amountDone;
				amountDone = Math.Min(value, amount);
				
				if(amountDoneBefore != amountDone) {
					if(OnUpdated != null) {
						OnUpdated(this);
					}
					
					if(OnFulfilled != null) {
						if(IsFulfilled()) {
							OnFulfilled(this, true);
						}
					}
				}
				
					
				
			
			}
		}
		
		public Achievement()
		{
	
		}
			
		public Achievement(AchievementType type, int amount) : this(type, amount, 0 )
		{
			
		}
		public Achievement(AchievementType type, int amount, int amountDone)
		{
			this.type = type;
			this.amount = amount;
			this.amountDone = amountDone;
		}
		public bool IsFulfilled() {
			if(this.amountDone >= amount) {
				return true;
			}
			return false;
		}
		
	    public override int GetHashCode() {
	    	return this.Type.GetHashCode();
	    }
		
		public bool Equals(Achievement other)
		{
			return type.Equals(other.Type);
		}	
		
		public string GetFulfilledImageFilename()
		{
			switch(type) {
				case AchievementType.Barracks:					
						return "astar_on.png";
					break;					
				case AchievementType.Fortresses:					
						return "astar_on.png";
					break;					
				case AchievementType.FlakBunkers:
						return "astar_on.png";
					break;	        
				case AchievementType.ConcreteBunkers:					
						return "astar_on.png";
					break;
				case AchievementType.EnemyBombers:					
						return "astar_on.png";
					break;
				case AchievementType.EnemyFighters:					
						return "astar_on.png";
					break;
				case AchievementType.Generals:					
						return "astar_on.png";
					break;
				case AchievementType.PatrolBoats:					
						return "astar_on.png";
					break;
				case AchievementType.Soldiers:					
						return "astar_on.png";
					break;
									
				case AchievementType.Warships:					
						return "astar_on.png";
					break;
					
					
				case AchievementType.Submarines:					
						return "astar_on.png";
					break;						
					
				case AchievementType.WoodBunkers:					
						return "astar_on.png";
					break;
							
					
			}
			return null;
		}
		
		
		public string GetImageFilename()
		{
			switch(type) {
				case AchievementType.Barracks:					
						return "astar.png";
					break;					
				case AchievementType.Fortresses:					
						return "astar.png";
					break;					
				case AchievementType.FlakBunkers:
						return "astar.png";
					break;	        
				case AchievementType.ConcreteBunkers:					
						return "astar.png";
					break;
				case AchievementType.EnemyBombers:					
						return "astar.png";
					break;
				case AchievementType.EnemyFighters:					
						return "astar.png";
					break;
				case AchievementType.Generals:					
						return "astar.png";
					break;
				case AchievementType.PatrolBoats:					
						return "astar.png";
					break;
				case AchievementType.Soldiers:					
						return "astar.png";
					break;
									
				case AchievementType.Warships:					
						return "astar.png";
					break;
					
					
				case AchievementType.Submarines:					
						return "astar.png";
					break;						
					
				case AchievementType.WoodBunkers:					
						return "astar.png";
					break;
						
					
			}
			return null;
		}
		
		
	}
}
