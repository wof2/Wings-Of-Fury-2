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
		
		/// <summary>
		/// Kopiuje twarde dane z Achievementu. Nie kopiuje delegatow
		/// </summary>
		/// <param name="a"></param>
		public void CopyFrom(Achievement a) {
			this.Amount = a.Amount;
			this.amountDone = a.AmountDone; // bez settera - zeby nie zainicjowac "onFulfilled"
			this.Type = a.Type;
		
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
		
		public string GetUnFulfilledImageFilename()
		{
			return "astar.png";
		}
		
		public string GetFulfilledImageFilename()
		{
			return "astar_on.png";
		}
		
		
		public string GetImageFilename()
		{
			switch(type) {
				case AchievementType.Barracks:					
						return "a_barracks.png";
					break;					
				case AchievementType.Fortresses:					
						return "a_fortresses.png";
					break;					
				case AchievementType.FlakBunkers:
						return "a_flak_bunkers.png";
					break;	        
				case AchievementType.ConcreteBunkers:					
						return "a_concrete_bunkers.png";
					break;
				case AchievementType.EnemyBombers:					
						return "a_enemy_bombers.png";
					break;
				case AchievementType.EnemyFighters:					
						return "a_enemy_fighters.png";
					break;
				case AchievementType.Generals:					
						return "a_generals.png";
					break;
				case AchievementType.PatrolBoats:					
						return "a_patrolboats.png";
					break;
				case AchievementType.Soldiers:					
						return "a_soldiers.png";
					break;
									
				case AchievementType.Warships:					
						return "a_warships.png";
					break;					
					
				case AchievementType.Submarines:					
						return "a_submarines.png";
					break;						
					
				case AchievementType.WoodBunkers:					
						return "a_wood_bunkers.png";
					break;
						
					
			}
			return null;
		}
		
		
	}
}
