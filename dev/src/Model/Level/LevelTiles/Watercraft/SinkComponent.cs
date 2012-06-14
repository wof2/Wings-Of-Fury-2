using System;
using System.Collections.Generic;
using System.Text;
using Wof.Model.Level.Common;

namespace Wof.Model.Level.LevelTiles.Watercraft
{
    public class SinkComponent : ISinkComponent
    {

        protected LevelTile tile;

        private bool isSubmerging;


        /// <summary>
        /// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
        /// </summary>
        public static float SinkingSpeed = 1.15f;





        /// <summary>
        /// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
        /// </summary>
        public static float SubmergingSpeed = 4.15f;


        /// <summary>
        /// Określa z jaką prędkością statek będzie tonąć. Wyrażona jako liczba dodatnia.
        /// </summary>
        public static float EmergingSpeed = 2.0f;


        /// <summary>
        /// Czas od momentu rozbicia statku do momentu zakończenia tonięcia.
        /// Wyrażony w ms.
        /// </summary>
        private const float wreckTime = 30000;


        /// <summary>
        /// Wysokość nad poziomem morza. Standardowo 0. Przy tonięciu < 0
        /// </summary>
        protected float depth = 0.0f;

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
        private float wreckTimeElapsed = 0;


        /// <summary>
        /// Moc hamowania wody w pionie (do tonięcia).
        /// </summary>
        private readonly float waterYBreakingPower = SinkingSpeed*0.5f;

        private float submergeTimeElapsed;
        private bool isSubmerged;
        private float submergeTime;


        private float sinkingTime;
        private float emergeTimeElapsed;
        private float emergeTime;
        private bool isEmerging;


        public SinkComponent(LevelTile tile)
        {
            this.tile = tile;
        }

        /// <summary>
        /// Zwraca informacje o zanurzeniu statku.
        /// </summary>
        public float Depth
        {
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

        public bool IsSinking
        {
            get { return isSinking; }
        }

        public bool IsSunkDown
        {
            get { return isSunkDown; }
        }

        /// <summary>
        /// Tonięcie tile'a. Zwraca o ile tile zatonął, lub 0 w przypadku zakończenia tonięcia
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        public virtual float DoSinking(float time, float timeUnit)
        {
            if (wreckTimeElapsed > wreckTime) //koniec czasu
            {
                StopSinking();
                isSunkDown = true;
                return 0;
            }

            sinkingTime +=  time;
            float YVal = 0;
            //aktualizacja movmentVector.Y
            YVal = (YVal >= 0)
                       ? SinkingSpeed
                       : System.Math.Min(YVal + waterYBreakingPower, -SinkingSpeed);
            YVal = YVal*(time/timeUnit);
            depth += YVal;
            tile.YBegin -= YVal;
            tile.YEnd -= YVal;
            foreach (Quadrangle q in tile.ColisionRectangles)
            {
                q.Move(0, -YVal);
            }
            tile.HitBound.Move(0, -YVal);


            wreckTimeElapsed += time;

            return YVal;
        }


        public virtual float DoSubmerge(float time, float timeUnit)
        {
            if (submergeTimeElapsed > submergeTime) //koniec czasu
            {
                StopSubmerging();
                isSubmerged = true;
                return 0;
            }
            submergeTime +=time;

            float YVal = 0;
            //aktualizacja movmentVector.Y
            YVal = (YVal >= 0)
                                   ? SubmergingSpeed
                                   : System.Math.Min(YVal + waterYBreakingPower, -SubmergingSpeed);
            YVal = YVal * (time / timeUnit);
            depth += YVal;
            tile.YBegin -= YVal;
            tile.YEnd -= YVal;
            foreach (Quadrangle q in tile.ColisionRectangles)
            {
                q.Move(0, -YVal);
            }
            tile.HitBound.Move(0, -YVal);
            
            return YVal;

        }

        public virtual float DoEmerge(float time, float timeUnit)
        {
            if (emergeTimeElapsed > emergeTime) //koniec czasu
            {
                StopEmerging();
               // isSubmerged = false;
                return 0;
            }
            emergeTime += time;

            float YVal = 0;
            //aktualizacja movmentVector.Y
            YVal = (YVal >= 0)
                                   ? EmergingSpeed
                                   : System.Math.Min(YVal + waterYBreakingPower, -EmergingSpeed);
            YVal = YVal * (time / timeUnit);
            YVal *= -1;

            depth += YVal;
            tile.YBegin -= YVal;
            tile.YEnd -= YVal;
            foreach (Quadrangle q in tile.ColisionRectangles)
            {
                q.Move(0, -YVal);
            }
            tile.HitBound.Move(0, -YVal);

            return YVal;

        }

        public void StartEmerging()
        {
            isEmerging = true;
        }

        public void StopEmerging()
        {
            isEmerging = false;
        }

        public void StartSubmerging()
        {
            isSubmerging = true;
        }

        public void StopSubmerging()
        {
            isSubmerging = false;
        }

        public float SinkingTime
        {
            get { return sinkingTime; }
        }

        public float SubmergeTime
        {
            get { return submergeTime; }
        }

        public bool IsSubmerged
        {
            get { return isSubmerged; }
        }

        public float SubmergeTimeElapsed
        {
            get { return submergeTimeElapsed; }
        }

        public bool IsSubmerging
        {
            get { return isSubmerging; }
        }

        public bool IsEubmerging
        {
            get { return isEmerging; }
        }

        
    }
}