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
    public unsafe class VCloudsManager
    {
        internal IntPtr NativeHandle;

        internal VCloudsManager(IntPtr handle)
        {
            this.NativeHandle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {
            VCloudsManager_Create(NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSinceLastFrame"></param>
        public void Update(float timeSinceLastFrame)
        {
            VCloudsManager_Update(NativeHandle, timeSinceLastFrame);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            VCloudsManager_Remove(NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Height
        {
            get
            {
                return *(((Vector2*)VCloudsManager_GetHeight(NativeHandle)));
            }
            set
            {
                VCloudsManager_SetHeight(NativeHandle, ref value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoUpdate
        {
            get
            {
                return VCloudsManager_GetAutoupdate(NativeHandle);
            }
            set
            {
                VCloudsManager_SetAutoupdate(NativeHandle, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float WindSpeed
        {
            get
            {
                return VCloudsManager_GetWindSpeed(NativeHandle);
            }
            set
            {
                VCloudsManager_SetWindSpeed(NativeHandle, value);
            }
        }

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_Create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_Create(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_Update", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_Update(IntPtr handle,float time);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_Remove", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_Remove(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_SetHeight", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_SetHeight(IntPtr handle,ref  Vector2 val);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_GetHeight", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr VCloudsManager_GetHeight(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_SetAutoupdate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_SetAutoupdate(IntPtr handle, [MarshalAs(UnmanagedType.U1)] bool val);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_GetAutoupdate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool VCloudsManager_GetAutoupdate(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_SetWindSpeed", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void VCloudsManager_SetWindSpeed(IntPtr handle, float val);

        [DllImport("SkyX.dll", EntryPoint = "VCloudsManager_GetWindSpeed", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float VCloudsManager_GetWindSpeed(IntPtr handle);

        #endregion
    }
}
