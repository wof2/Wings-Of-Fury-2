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
	public class Achievement
	{
		
		private AchievementType type;
		
		
		public delegate void AchievementFulfilledDelegate(Achievement a);
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
				
				amountDone = value;
				if(OnUpdated != null) {
					OnUpdated(this);
				}
				
				if(OnFulfilled != null) {
					if(IsFulfilled()) {
						OnFulfilled(this);
					}
				}
				
					
				
			
			}
		}
		
		public Achievement()
		{
	
		}
			
		public Achievement(AchievementType type, int amount)
		{
			this.type = type;
			this.amount = amount;
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
			
		
		
		public string GetPinFilename()
		{
			switch(type) {
				case AchievementType.Barracks:					
						return "pin.png";
					break;
					
				case AchievementType.ConcreteBunkers:					
						return "pin.png";
					break;
				case AchievementType.EnemyBombers:					
						return "pin.png";
					break;
				case AchievementType.EnemyFighters:					
						return "pin.png";
					break;
				case AchievementType.Generals:					
						return "pin.png";
					break;
				case AchievementType.PatrolBoats:					
						return "pin.png";
					break;
				case AchievementType.Soldiers:					
						return "pin.png";
					break;
									
				case AchievementType.Warships:					
						return "pin.png";
					break;
				case AchievementType.WoodBunkers:					
						return "pin.png";
					break;
				
									
					
			}
			return null;
		}
	}
}
