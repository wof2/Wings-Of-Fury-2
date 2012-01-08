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
    public unsafe class AtmosphereManager
    {
        internal IntPtr NativeHandle;

        internal AtmosphereManager(IntPtr handle)
        {
            this.NativeHandle = handle;
        }


        /// <summary>
        /// Time information: x = time in [0, 24]h range, y = sunrise hour in [0, 24]h range, z = sunset hour in [0, 24] range
        /// </summary>
        public Vector3 Time
        {
            get
            {
                return *(((Vector3*)AtmosphereManager_GetTime(NativeHandle)));
            }
        }

        /// <summary>
        /// East position
        /// </summary>
        public Vector2 EastPosition
        {
            get
            {
                return *(((Vector2*)AtmosphereManager_GetEastPosition(NativeHandle)));
            }
        }

        /// <summary>
        /// WaveLength for RGB channels
        /// </summary>
        public Vector3 WaveLength
        {
            get
            {
                return *(((Vector3*)AtmosphereManager_GetWaveLength(NativeHandle)));
            }
        }

        /// <summary>
        /// Inner atmosphere radius
        /// </summary>
        public float InnerRadius
        {
            get
            {
                return AtmosphereManager_GetInnerRadius(NativeHandle);
            }
        }

        /// <summary>
        /// Outer atmosphere radius
        /// </summary>
        public float OuterRadius
        {
            get
            {
                return AtmosphereManager_GetOuterRadius(NativeHandle);
            }
        }

        /// <summary>
        /// Height position, in [0, 1] range, 0=InnerRadius, 1=OuterRadius
        /// </summary>
        public float HeightPosition
        {
            get
            {
                return AtmosphereManager_GetHeightPosition(NativeHandle);
            }
        }

        /// <summary>
        /// Rayleigh multiplier
        /// </summary>
        public float RayleighMultiplier
        {
            get
            {
                return AtmosphereManager_GetRayleighMultiplier(NativeHandle);
            }
        }

        /// <summary>
        /// Mie multiplier
        /// </summary>
        public float MieMultiplier
        {
            get
            {
                return AtmosphereManager_GetMieMultiplier(NativeHandle);
            }
        }

        /// <summary>
        /// Sun intensity
        /// </summary>
        public float SunIntensity
        {
            get
            {
                return AtmosphereManager_GetSunIntensity(NativeHandle);
            }
        }

        /// <summary>
        /// Phase function
        /// </summary>
        public float G
        {
            get
            {
                return AtmosphereManager_GetG(NativeHandle);
            }
        }

        /// <summary>
        /// Exposure coeficient
        /// </summary>
        public float Exposure
        {
            get
            {
                return AtmosphereManager_GetExposure(NativeHandle);
            }
        }

        /// <summary>
        /// Number of samples
        /// </summary>
        public int NumberOfSamples
        {
            get
            {
                return AtmosphereManager_GetNumberOfSamples(NativeHandle);
            }
        }
        

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetTime", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr AtmosphereManager_GetTime(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetEastPosition", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr AtmosphereManager_GetEastPosition(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetWaveLength", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr AtmosphereManager_GetWaveLength(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetInnerRadius", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetInnerRadius(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetOuterRadius", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetOuterRadius(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetHeightPosition", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetHeightPosition(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetRayleighMultiplier", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetRayleighMultiplier(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetMieMultiplier", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetMieMultiplier(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetSunIntensity", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetSunIntensity(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetG", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetG(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetExposure", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float AtmosphereManager_GetExposure(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "AtmosphereManager_GetNumberOfSamples", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AtmosphereManager_GetNumberOfSamples(IntPtr handle);

        #endregion
    }
}
