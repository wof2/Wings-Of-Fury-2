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
    public unsafe class GPUManager
    {
        internal IntPtr NativeHandle;


        internal GPUManager(IntPtr handle)
        {
            this.NativeHandle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="atmosphereRadius"></param>
        /// <param name="blendType"></param>
        public void AddGroundPass(Pass pass, float atmosphereRadius, SceneBlendType blendType)
        {
            IntPtr nativePassHandle = ReflectionHelper.GetFieldPointer(pass, "_native");
            GPUManager_AddGroundPass(NativeHandle, nativePassHandle, atmosphereRadius, blendType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="atmosphereRadius"></param>
        public void AddGroundPass(Pass pass, float atmosphereRadius)
        {
            this.AddGroundPass(pass, atmosphereRadius, SceneBlendType.SBT_ADD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        public void AddGroundPass(Pass pass)
        {
            this.AddGroundPass(pass, 0.0f, SceneBlendType.SBT_ADD);
        }

        /// <summary>
        /// 
        /// </summary>
        public String MoonMaterialName
        {
            get
            {
                return "SkyX_Moon";
            }
        }

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "GPUManager_AddGroundPass", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GPUManager_AddGroundPass(IntPtr handle,IntPtr passHandle,float val,Mogre.SceneBlendType blendType);

        #endregion
    }
}
