using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.View.Effects
{
    public interface WaterTrailGenerator
    {

        bool IsReadyForLastWaterTrail
        {
            get;
        }

        float LastWaterTrailTime
        {
            get;
            set;
        }

    }
}
