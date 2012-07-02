using System;
using System.Collections.Generic;
using System.Text;
using Wof.Model.Level.Common;

namespace Wof.Model.Level.LevelTiles.Watercraft
{
	public class SinkComponent : ISinkComponent
	{

		protected LevelTile tile;

		private bool isSubmerging = false;


		/// <summary>
		/// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
		/// </summary>
		public static float SinkingSpeed = 1.15f;


		/// <summary>
		/// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
		/// </summary>
		public static float SubmergingSpeed = 6.0f;


		/// <summary>
		/// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
		/// </summary>
		public static float EmergingSpeed = 4.0f;


		/// <summary>
		/// Czas od momentu rozbicia statku do momentu zakończenia tonięcia.
		/// Wyrażony w ms.
		/// </summary>
		private const float maxSinkingTime = 30000;
		
		private const float maxSubmergeTime = 4000;
		private const float maxEmergeTime = 6000;

		

		/// <summary>
		/// Wysokość nad poziomem morza. Standardowo 0. Przy tonięciu < 0
		/// </summary>
		protected float depth = 0f;

		/// <summary>
		/// Określa czy statek ma tonąć.
		/// </summary>
		protected bool isSinking = false;

		/// <summary>
		/// Określa czy statek zatonął.
		/// </summary>
		protected bool isSunkDown = false;


		/// <summary>
		/// Czas jaki minął od momentu kiedy rozpoczęło się tonięcie.
		/// </summary>
		private float sinkingTimeElapsed = 0;
		private float submergeTimeElapsed = 0;
		private float emergeTimeElapsed = 0;

		/// <summary>
		/// Moc hamowania wody w pionie (do tonięcia).
		/// </summary>
		private readonly float waterYBreakingPower = SinkingSpeed * 0.5f;


		private bool isSubmerged = false;



		private float sinkingTime;


		private bool isEmerging = false;
		private bool isEmerged = true;

        protected IRefsToLevel irefsToLevel;
		public SinkComponent(LevelTile tile, IRefsToLevel irefsToLevel)
		{
			this.tile = tile;
            this.irefsToLevel = irefsToLevel;
		}

		/// <summary>
		/// Zwraca informacje o zanurzeniu statku.
		/// </summary>
		public float Depth {
			get { return this.depth; }
		}

		public virtual void StartSinking()
		{
			isSinking = true;
		}

		public virtual void StopSinking()
		{
			isSinking = false;
		}

		public bool IsSinking {
			get { return isSinking; }
		}

		public bool IsSunkDown {
			get { return isSunkDown; }
		}

		/// <summary>
		/// Tonięcie tile'a. Zwraca o ile tile zatonął, lub 0 w przypadku zakończenia tonięcia
		/// </summary>
		/// <param name="time"></param>
		/// <param name="timeUnit"></param>
		public virtual float DoSinking(float time, float timeUnit)
		{
			//koniec czasu
			if (sinkingTimeElapsed > maxSinkingTime) {
				StopSinking();
				isSunkDown = true;
				return 0;
			}

			sinkingTime += time;
			float YVal = 0;
			//aktualizacja movmentVector.Y
			YVal = (YVal >= 0) ? SinkingSpeed : System.Math.Min(YVal + waterYBreakingPower, -SinkingSpeed);
			YVal = YVal * (time / timeUnit);
			depth += YVal;
			ChangeTileDepth(tile, YVal);
			sinkingTimeElapsed += time;
			return YVal;
		}
        float YVal = 0;

		public virtual float DoSubmerge(float time, float timeUnit)
		{
			//koniec czasu
			if (submergeTimeElapsed > maxSubmergeTime) {
				StopSubmerging();
				isSubmerged = true;
				return 0;
			}

			float progress = submergeTimeElapsed/ maxSubmergeTime;
			
		
			YVal = SubmergingSpeed * Mogre.Math.Sin(progress* Mogre.Math.PI);
			
			YVal = YVal * (time / timeUnit);
			depth += YVal;
			ChangeTileDepth(tile, YVal);
			submergeTimeElapsed += time;
			return YVal;

		}

		public virtual float DoEmerge(float time, float timeUnit)
		{
			//koniec czasu
			if (emergeTimeElapsed > maxEmergeTime || depth <= 0) {				
				StopEmerging();
				isEmerged = true;			
				if(depth <0)
				{
					// latajacy okret
					ChangeTileDepth(tile, depth);
				}
				return 0;
			}
			float progress = emergeTimeElapsed / maxEmergeTime;
			
		
			YVal = EmergingSpeed * Mogre.Math.Sin(progress* Mogre.Math.PI);
		
			YVal = YVal * (time / timeUnit);
			YVal *= -1;

			depth += YVal;
			ChangeTileDepth(tile, YVal);
			emergeTimeElapsed += time;
			return YVal;

		}
		
		protected void ChangeTileDepth(LevelTile tile, float YVal)
		{
			tile.YBegin -= YVal;
			tile.YEnd -= YVal;
			foreach (Quadrangle q in tile.ColisionRectangles) {
				q.Move(0, -YVal);
			}
			tile.HitBound.Move(0, -YVal);
			
		}

		public void StartEmerging()
		{
			emergeTimeElapsed = 0;
			isSubmerged = false;
			isEmerging = true;
		}

		public void StopEmerging()
		{
            isEmerging = false;
            if(this.irefsToLevel.LevelProperties != null)
            {
                this.irefsToLevel.LevelProperties.Controller.OnShipEmerged(tile);
            }
		}

		public void StartSubmerging()
		{
			submergeTimeElapsed = 0;
			isSubmerging = true;
			isEmerged = false;
		}

		public void StopSubmerging()
		{
            isSubmerging = false;
            if (this.irefsToLevel.LevelProperties != null)
            {
                this.irefsToLevel.LevelProperties.Controller.OnShipSubmerged(tile);
            }
			
		}

		public bool IsEmerged {
			get { return isEmerged; }
		}

		public bool IsSubmerged {
			get { return isSubmerged; }
		}

		public float SubmergeTimeElapsed {
			get { return submergeTimeElapsed; }
		}

		public float SinkingTimeElapsed {
			get { return sinkingTimeElapsed; }
		}

		public bool IsSubmerging {
			get { return isSubmerging; }
		}

		public bool IsEmerging {
			get { return isEmerging; }
		}

	    public LevelTile Tile
	    {
	        get
	        {
	            return tile;   
	        }
	    }
	}
}
