using System;
using System.Collections.Generic;
using System.Text;

namespace SkyX
{
    /// <summary>
    /// Lighting mode enumeration
    ///SkyX is designed for true HDR rendering, but there're a lot of applications
    ///that doesn't use HDR rendering, due to this a little exponential tone-mapping 
    ///algoritm is applied to SkyX materials if LM_LDR is selected. (See: AtmosphereManager::Options::Exposure)
    ///Select LM_HDR if your app is designed for true HDR rendering.
    /// </summary>
    public enum LightingMode
    {
        /// <summary>Low dynamic range</summary>
        LDR = 0,
        /// <summary>High dynamic range</summary>
        HDR = 1
    }
}
