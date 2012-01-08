using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Mogre;

namespace SkyX
{
    public unsafe class SkyManager : IDisposable
    {
        /// <summary>Occurs when the manager is being disposed.</summary>
        public event EventHandler Disposed;

        protected SceneManager manager;
        protected Camera camera;

        private MeshManager meshManager;
        private GPUManager gpuManager;
        private MoonManager moonManager;
        private AtmosphereManager atmosphereManager;
        private CloudsManager cloudsManager;
        private VCloudsManager vCloudsManager;

        internal IntPtr NativeHandle;

        public SkyManager(SceneManager manager, Camera camera)
        {
            this.manager = manager;
            this.camera = camera;

            NativeHandle = New_Manager(manager.NativePtr, camera.NativePtr);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {

                    if (Disposed != null)
                    {
                        Disposed(this, EventArgs.Empty);
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {
            Manager_Create(NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            Manager_Remove(NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSinceLastFrame"></param>
        public void Update(float timeSinceLastFrame)
        {
            Manager_Update(NativeHandle, timeSinceLastFrame);
        }

        /// <summary>
        /// Gets or Sets the time multiplier
        /// <remarks>
        /// The time multiplier can be a negative number, 0 will disable auto-updating
        /// For setting a custom time of day, check: AtmosphereManager.Options.Time
        /// </remarks>
        /// </summary>
        public float TimeMultiplier
        {
            get
            {
                return Manager_GetTimeMultiplier(NativeHandle);
            }
            set
            {
                Manager_SetTimeMultiplier(NativeHandle, value);
            }
        }

        /// <summary>
        /// Gets time offset
        /// </summary>
        public float TimeOffset
        {
            get
            {
                return Manager_GetTimeOffset(NativeHandle);
            }

        }

        /// <summary>
        /// Gets or Sets the lighting mode
        /// <remarks>
        /// SkyX is designed for true HDR rendering, but there're a lot of applications
        /// that doesn't use HDR rendering, due to this a little exponential tone-mapping 
        /// algoritm is applied to SkyX materials if LDR is selected. (See: AtmosphereManager::Options::Exposure)
        /// Select HDR if your app is designed for true HDR rendering.
        /// </remarks>
        /// </summary>
        public LightingMode LightingMode
        {
            get
            {
                return Manager_GetLightingMode(NativeHandle);
            }
            set
            {
                Manager_SetLightingMode(NativeHandle, value);
            }
        }

        /// <summary>
        /// Gets or Sets the starfield enabled/disabled
        /// </summary>
        public bool StarfieldEnabled
        {
            get
            {
                return Manager_GetStarfieldEnabled(NativeHandle);
            }
            set
            {
                Manager_SetStarfieldEnabled(NativeHandle, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GPUManager GPUManager
        {
            get
            {
                //Cache params
                if (gpuManager == null)
                {
                    gpuManager = new GPUManager(Manager_GetGPUManager(NativeHandle));
                }

                return gpuManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MoonManager MoonManager
        {
            get
            {
                //Cache params
                if (moonManager == null)
                {
                    moonManager = new MoonManager(Manager_GetMoonManager(NativeHandle));
                }

                return moonManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MeshManager MeshManager
        {
            get
            {
                //Cache params
                if (meshManager == null)
                {
                    meshManager = new MeshManager(Manager_GetMeshManager(NativeHandle));
                }

                return meshManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AtmosphereManager AtmosphereManager
        {
            get
            {
                //Cache params
                if (atmosphereManager == null)
                {
                    atmosphereManager = new AtmosphereManager(Manager_GetAtmosphereManager(NativeHandle));
                }

                return atmosphereManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CloudsManager CloudsManager
        {
            get
            {
                //Cache params
                if (cloudsManager == null)
                {
                    cloudsManager = new CloudsManager(Manager_GetCloudsManager(NativeHandle));
                }

                return cloudsManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public VCloudsManager VCloudsManager
        {
            get
            {
                //Cache params
                if (vCloudsManager == null)
                {
                    vCloudsManager = new VCloudsManager(Manager_GetVCloudsManager(NativeHandle));
                }

                return vCloudsManager;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public SceneManager SceneManager
        {
            get
            {
                return manager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "New_Manager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr New_Manager(void* manager, void* camera);

        [DllImport("SkyX.dll", EntryPoint = "Manager_Create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_Create(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_Remove", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_Remove(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_Update", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_Update(IntPtr handle,float time);

        [DllImport("SkyX.dll", EntryPoint = "Manager_SetTimeMultiplier", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_SetTimeMultiplier(IntPtr handle, float time);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetTimeMultiplier", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Manager_GetTimeMultiplier(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetTimeOffset", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Manager_GetTimeOffset(IntPtr handle);


        [DllImport("SkyX.dll", EntryPoint = "Manager_SetLightingMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_SetLightingMode(IntPtr handle, LightingMode value);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetLightingMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern LightingMode Manager_GetLightingMode(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_SetStarfieldEnabled", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Manager_SetStarfieldEnabled(IntPtr handle, [MarshalAs(UnmanagedType.U1)]bool value);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("SkyX.dll", EntryPoint = "Manager_GetStarfieldEnabled", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Manager_GetStarfieldEnabled(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetGPUManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetGPUManager(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetMoonManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetMoonManager(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetMeshManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetMeshManager(IntPtr handle);


        [DllImport("SkyX.dll", EntryPoint = "Manager_GetAtmosphereManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetAtmosphereManager(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetCloudsManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetCloudsManager(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "Manager_GetVCloudsManager", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Manager_GetVCloudsManager(IntPtr handle);

        #endregion
    }
}
