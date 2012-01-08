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
    public unsafe class MoonManager
    {
        internal IntPtr NativeHandle;
        private BillboardSet billboardSet;
        private SceneNode sceneNode;

        internal MoonManager(IntPtr handle)
        {
            this.NativeHandle = handle;
        }

        /*

        /// <summary>
        /// 
        /// </summary>
        public BillboardSet MoonBillboard
        {
            get
            {
                if (billboardSet == null)
                {
                    IntPtr handle = MoonManager_GetMoonBillboard(NativeHandle);
                    billboardSet = ReflectionHelper.Construct<BillboardSet>(new object[] { handle });
                }

                return billboardSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SceneNode MoonSceneNode
        {
            get
            {
                if (sceneNode == null)
                {
                    sceneNode = (Mogre.SceneNode) MoonManager_GetMoonSceneNode(NativeHandle);
                    IntPtr handle = MoonManager_GetMoonSceneNode(NativeHandle);
                    sceneNode = ReflectionHelper.Construct<SceneNode>(new object[] { handle });
                }

                return sceneNode;
            }
        }

        */
        
        /// <summary>
        /// 
        /// </summary>
        public float MoonSize
        {
            get
            {
                return MoonManager_GetMoonSize(NativeHandle);
            }
            set
            {
                MoonManager_SetMoonSize(NativeHandle,value);
            }
        }

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "MoonManager_GetMoonBillboard", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MoonManager_GetMoonBillboard(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MoonManager_GetMoonSceneNode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MoonManager_GetMoonSceneNode(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "MoonManager_SetMoonSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MoonManager_SetMoonSize(IntPtr handle,float value);

        [DllImport("SkyX.dll", EntryPoint = "MoonManager_GetMoonSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float MoonManager_GetMoonSize(IntPtr handle);


        #endregion
    }
}
