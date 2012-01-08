using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Mogre;

namespace SkyX
{
    /// <summary>
    /// 
    /// </summary>
    public unsafe class CloudLayer
    {
        #region LayerOptions

        /// <summary>
        /// Layer cloud options 
        /// </summary>
		public class LayerOptions 
		{
			/// Cloud layer height
			public float Height;
			/// Cloud layer scale
			public float Scale;
			/// Wind direction
			public Vector2 WindDirection;
			/// Time multiplier
			public float TimeMultiplier;

			/// Distance attenuation
			public float DistanceAttenuation;
			/// Detail attenuation
			public float DetailAttenuation;
			/// Normal multiplier
			public float NormalMultiplier;
			/// Cloud layer height volume(For volumetric effects on the gpu)
			public float HeightVolume;
			/// Volumetric displacement(For volumetric effects on the gpu)
			public float VolumetricDisplacement;


			/// <summary>
			/// Default constructor
			/// </summary>
			public LayerOptions()
                : this(100,0.001f,new Vector2(1,1),0.125f,0.05f,1,2,0.25f,0.01f)
			{
			}

			
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="height">Cloud layer height</param>
            /// <param name="scale">Clouds scale</param>
            /// <param name="windDirection">Clouds movement direction</param>
            /// <param name="timeMultiplier">Time multiplier factor</param>
			public LayerOptions(float height, 
				    float scale, 
					Vector2 windDirection, 
					float timeMultiplier)
                : this(height,scale,windDirection,timeMultiplier,0.05f,1,2,0.25f,0.01f)
			{
			}

			/// <summary>
            /// Constructor
			/// </summary>
            /// <param name="height">Cloud layer height</param>
            /// <param name="scale">Clouds scale</param>
            /// <param name="windDirection">Clouds movement direction</param>
            /// <param name="timeMultiplier">Time multiplier factor</param>
            /// <param name="distanceAttenuation">Distance attenuation</param>
            /// <param name="detailAttenuation">Detail attenuation</param>
            /// <param name="normalMultiplier">Normal multiplier coeficient</param>
            /// <param name="heightVolume">Height volume(For volumetric effects on the gpu)</param>
            /// <param name="volumetricDisplacement">Volumetric displacement(For volumetric effects on the gpu)</param>
            public LayerOptions(
                    float height, 
                    float scale, 
                    Vector2 windDirection, 
					float  timeMultiplier,
					float distanceAttenuation,
					float detailAttenuation,
					float normalMultiplier,
					float heightVolume,
					float volumetricDisplacement)
				
			{
                this.Height = height;
                this.Scale = scale;
                this.WindDirection = windDirection;
                this.TimeMultiplier = timeMultiplier;
                this.DistanceAttenuation = distanceAttenuation;
                this.DetailAttenuation = detailAttenuation;
				this.NormalMultiplier = normalMultiplier;
                this.HeightVolume = heightVolume;
                this.VolumetricDisplacement = volumetricDisplacement;
			}
        }

        #endregion

        internal IntPtr NativeHandle;
        private LayerOptions options;


        internal CloudLayer(IntPtr handle, LayerOptions opt)
        {
            this.NativeHandle = handle;
            this.options = opt;
           
        }

        /// <summary>
        /// 
        /// </summary>
        public LayerOptions Options
        {
            get
            {
                return options;
            }
        }
    }
}
