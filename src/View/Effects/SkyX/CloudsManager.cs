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
    public unsafe class CloudsManager
    {
        internal IntPtr NativeHandle;
        private List<CloudLayer> layers;
        internal CloudsManager(IntPtr handle)
        {
            this.NativeHandle = handle;
            layers = new List<CloudLayer>();
        }

        /// <summary>
        /// 
        /// </summary>
        public CloudLayer Add(CloudLayer.LayerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            IntPtr handle = CloudsManager_Add(NativeHandle, options.Height, options.Scale, options.WindDirection, options.TimeMultiplier,
                                             options.DistanceAttenuation, options.DetailAttenuation, options.NormalMultiplier, options.HeightVolume, options.VolumetricDisplacement);

            if (handle == IntPtr.Zero)
            {
                LogManager.Singleton.LogMessage("Error adding new CloudLayer");
            }

            CloudLayer result = new CloudLayer(handle, options);
            layers.Add(result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public void Remove(CloudLayer layer)
        {
            if(!layers.Contains(layer))
            {
                throw new ArgumentException("Layer not found into list.");
            }

            CloudsManager_Remove(NativeHandle, layer.NativeHandle);
            layers.Remove(layer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public void Unregister(CloudLayer layer)
        {
            if (!layers.Contains(layer))
            {
                throw new ArgumentException("Layer not found into list.");
            }

            CloudsManager_Unregister(NativeHandle, layer.NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAll()
        {
            CloudsManager_RemoveAll(NativeHandle);
            layers.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterAll()
        {
            CloudsManager_RegisterAll(NativeHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnregisterAll()
        {
            CloudsManager_UnregisterAll(NativeHandle);
        }
        
        #region PINVOKE
        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_Add", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr CloudsManager_Add(IntPtr handle, float height,float scale,Vector2 windDirection,
                                          float timeMultiplier,float distanceAttenuation,float detailAttenuation,float normalMultiplier,
                                          float heightVolume,float volumetricDisplacement);

        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_Remove", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CloudsManager_Remove(IntPtr handle, IntPtr layerHandle);

        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_Unregister", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CloudsManager_Unregister(IntPtr handle, IntPtr layerHandle);

        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_RemoveAll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CloudsManager_RemoveAll(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_RegisterAll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CloudsManager_RegisterAll(IntPtr handle);

        [DllImport("SkyX.dll", EntryPoint = "CloudsManager_UnregisterAll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CloudsManager_UnregisterAll(IntPtr handle);

        #endregion
    }
}
