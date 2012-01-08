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
    public unsafe class MeshManager
    {
        internal IntPtr NativeHandle;

        internal MeshManager(IntPtr handle)
        {
            this.NativeHandle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smoothSkydomeFading"></param>
        /// <param name="skydomeFadingPercent"></param>
        public void SetSkydomeFadingParameters(bool smoothSkydomeFading, float skydomeFadingPercent)
        {
            MeshManager_SetSkydomeFadingParameters(NativeHandle, smoothSkydomeFading, skydomeFadingPercent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smoothSkydomeFading"></param>
        public void SetSkydomeFadingParameters(bool smoothSkydomeFading)
        {
            this.SetSkydomeFadingParameters(smoothSkydomeFading, 0.05f);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SmoothSkydomeFading
        {
            get
            {
                return MeshManager_GetSmoothSkydomeFading(NativeHandle);
            }
            set
            {
                this.SetSkydomeFadingParameters(value, this.SkydomeFadingPercent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float SkydomeFadingPercent
        {
            get
            {
                return MeshManager_GetSkydomeFadingPercent(NativeHandle);
            }
            set
            {
                this.SetSkydomeFadingParameters(this.SmoothSkydomeFading,value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Steps
        {
            get
            {
                return MeshManager_GetSteps(NativeHandle);
            }
            set
            {
                MeshManager_SetGeometryParameters(NativeHandle, value, this.Circles);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Circles
        {
            get
            {
                return MeshManager_GetCircles(NativeHandle);
            }
            set
            {
                MeshManager_SetGeometryParameters(NativeHandle, this.Steps, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float SkyDomeRadius
        {
            get
            {
                return MeshManager_GetSkydomeRadius(NativeHandle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String MaterialName
        {
            get
            {
                return ReflectionHelper.IntPtrToString( MeshManager_GetMaterialName(NativeHandle) );
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                MeshManager_SetMaterialName(NativeHandle, value);
            }
        }

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "MeshManager_SetSkydomeFadingParameters", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MeshManager_SetSkydomeFadingParameters(IntPtr handle,[MarshalAs(UnmanagedType.U1)]bool smoothSkydomeFading,float skydomeFadingPercent);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetSmoothSkydomeFading", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool MeshManager_GetSmoothSkydomeFading(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetSkydomeFadingPercent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float MeshManager_GetSkydomeFadingPercent(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetSteps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MeshManager_GetSteps(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetCircles", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MeshManager_GetCircles(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetSkydomeRadius", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float MeshManager_GetSkydomeRadius(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_SetGeometryParameters", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MeshManager_SetGeometryParameters(IntPtr handle,int val1,int val2);


        [DllImport("SkyX.dll", EntryPoint = "MeshManager_SetMaterialName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MeshManager_SetMaterialName(IntPtr handle, string value);

        [DllImport("SkyX.dll", EntryPoint = "MeshManager_GetMaterialName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MeshManager_GetMaterialName(IntPtr handle);


        #endregion
    }
}
