using System.Runtime.InteropServices;

namespace FSLOgreCS
{
    public sealed class FreeSL
    {
        // Sound System
        public enum FSL_SOUND_SYSTEM
        {
            FSL_SS_EAX2, // EAX 2.0 (Direct Sound 3D)
            FSL_SS_DIRECTSOUND3D, // Direct Sound 3D
            FSL_SS_DIRECTSOUND, // Direct Sound
            FSL_SS_NVIDIA_NFORCE_2, // nVidia nForce 2
            FSL_SS_CREATIVE_AUDIGY_2, // Creative Audigy 2
            FSL_SS_MMSYSTEM, // Microsoft
            FSL_SS_ALUT, // ALUT

            FSL_SS_NOSYSTEM // no sound system
        } ;

        [DllImport("FreeSL.dll", EntryPoint = "?fslInit@@YA_NW4FSL_SOUND_SYSTEM@@@Z")]
        public static extern bool fslInit(FSL_SOUND_SYSTEM val);

        [DllImport("FreeSL.dll", EntryPoint = "?fslShutDown@@YAXXZ")]
        public static extern void fslShutDown();

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetVolume@@YAXM@Z")]
        public static extern void fslSetVolume(float gain_mult);

        [DllImport("FreeSL.dll", EntryPoint = "?fslFreeSound@@YAXI_N@Z")]
        public static extern void fslFreeSound(uint obj, bool remove_buffer);

        [DllImport("FreeSL.dll", EntryPoint = "?fslLoadSound@@YAIPBD@Z")]
        public static extern uint fslLoadSound(string strFile);

        [DllImport("FreeSL.dll", EntryPoint = "?fslStreamSound@@YAIPBD@Z")]
        public static extern uint fslStreamSound(string strFile);

        [DllImport("FreeSL.dll", EntryPoint = "?fslLoadSoundFromZip@@YAIPBD0@Z")]
        public static extern uint fslLoadSoundFromZip(string strPackage, string strFile);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundPlay@@YAXI@Z")]
        public static extern void fslSoundPlay(uint obj); // play the sound

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundRewind@@YAXI@Z")]
        public static extern void fslSoundRewind(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundStop@@YAXI@Z")]
        public static extern void fslSoundStop(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundPause@@YAXI@Z")]
        public static extern void fslSoundPause(uint obj);

       // [DllImport("FreeSL.dll", EntryPoint = "?fslSoundIsPlaying@@YA_NI@Z")]
       // public static extern bool fslSoundIsPlaying(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundIsPlaying@@YA_NI@Z")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fslSoundIsPlaying(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundIsPaused@@YA_NI@Z")]
        public static extern bool fslSoundIsPaused(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetLooping@@YAXI_N@Z")]
        public static extern void fslSoundSetLooping(uint obj, bool loop_sound);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundIsLooping@@YA_NI@Z")]
        public static extern bool fslSoundIsLooping(uint obj);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetGain@@YAXIM@Z")]
        public static extern void fslSoundSetGain(uint obj, float gain);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetSourceRelative@@YAXI_N@Z")]
        public static extern void fslSoundSetSourceRelative(uint obj, bool is_relative);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerPosition@@YAXMMM@Z")]
        public static extern void fslSetListenerPosition(float x, float y, float z);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerOrientation@@YAXMMMMMM@Z")]
        public static extern void fslSetListenerOrientation(float atx, float aty, float atz,
                                                            float upx, float upy, float upz);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetPosition@@YAXIMMM@Z")]
        public static extern void fslSoundSetPosition(uint obj, float x, float y, float z);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetMaxDistance@@YAXIM@Z")]
        public static extern void fslSoundSetMaxDistance(uint obj, float max_distance);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetReferenceDistance@@YAXIM@Z")]
        public static extern void fslSoundSetReferenceDistance(uint obj, float ref_distance);

        // Listener Environments
        public enum FSL_LISTENER_ENVIRONMENT : int
        {
            FSL_ENVIRONMENT_GENERIC,
            FSL_ENVIRONMENT_PADDEDCELL,
            FSL_ENVIRONMENT_ROOM,
            FSL_ENVIRONMENT_BATHROOM,
            FSL_ENVIRONMENT_LIVINGROOM,
            FSL_ENVIRONMENT_STONEROOM,
            FSL_ENVIRONMENT_AUDITORIUM,
            FSL_ENVIRONMENT_CONCERTHALL,
            FSL_ENVIRONMENT_CAVE,
            FSL_ENVIRONMENT_ARENA,
            FSL_ENVIRONMENT_HANGAR,
            FSL_ENVIRONMENT_CARPETEDHALLWAY,
            FSL_ENVIRONMENT_HALLWAY,
            FSL_ENVIRONMENT_STONECORRIDOR,
            FSL_ENVIRONMENT_ALLEY,
            FSL_ENVIRONMENT_FOREST,
            FSL_ENVIRONMENT_CITY,
            FSL_ENVIRONMENT_MOUNTAINS,
            FSL_ENVIRONMENT_QUARRY,
            FSL_ENVIRONMENT_PLAIN,
            FSL_ENVIRONMENT_PARKINGLOT,
            FSL_ENVIRONMENT_SEWERPIPE,
            FSL_ENVIRONMENT_UNDERWATER,
            FSL_ENVIRONMENT_DRUGGED,
            FSL_ENVIRONMENT_DIZZY,
            FSL_ENVIRONMENT_PSYCHOTIC,

            FSL_ENVIRONMENT_COUNT
        } ;

        // Structs
        [StructLayout(LayoutKind.Sequential)]
        public struct FSL_EAX_LISTENER_PROPERTIES
        {
            public long lRoom; // room effect level at low frequencies
            public long lRoomHF; // room effect high-frequency level re. low frequency level
            public float flRoomRolloffFactor; // like DS3D flRolloffFactor but for room effect
            public float flDecayTime; // reverberation decay time at low frequencies
            public float flDecayHFRatio; // high-frequency to low-frequency decay time ratio
            public long lReflections; // early reflections level relative to room effect
            public float flReflectionsDelay; // initial reflection delay time
            public long lReverb; // late reverberation level relative to room effect
            public float flReverbDelay; // late reverberation delay time relative to initial reflection
            public ulong dwEnvironment; // sets all listener properties
            public float flEnvironmentSize; // environment size in meters
            public float flEnvironmentDiffusion; // environment diffusion
            public float flAirAbsorptionHF; // change in level per meter at 5 kHz
            public ulong dwFlags; // modifies the behavior of properties
        } ;

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerEnvironment@@YAXPAUFSL_EAX_LISTENER_PROPERTIES@@@Z")]
        public static extern void fslSetListenerEnvironment(FSL_EAX_LISTENER_PROPERTIES lpData);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerEnvironmentPreset@@YAXW4FSL_LISTENER_ENVIRONMENT@@@Z")]
        public static extern void fslSetListenerEnvironmentPreset(FSL_LISTENER_ENVIRONMENT type);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerDefaultEnvironment@@YAXXZ")]
        public static extern void fslSetListenerDefaultEnvironment();

        [DllImport("FreeSL.dll", EntryPoint = "?fslGetCurrentListenerEnvironment@@YA?AUFSL_EAX_LISTENER_PROPERTIES@@XZ")
        ]
        public static extern FSL_EAX_LISTENER_PROPERTIES fslGetCurrentListenerEnvironment();

        [DllImport("FreeSL.dll", EntryPoint = "?fslLoadListenerEnvironment@@YA?AUFSL_EAX_LISTENER_PROPERTIES@@PBD@Z")]
        public static extern FSL_EAX_LISTENER_PROPERTIES fslLoadListenerEnvironment(string strFile);

        [DllImport("FreeSL.dll",
            EntryPoint = "?fslLoadListenerEnvironmentFromZip@@YA?AUFSL_EAX_LISTENER_PROPERTIES@@PBD0@Z")]
        public static extern FSL_EAX_LISTENER_PROPERTIES fslLoadListenerEnvironmentFromZip(string strPackage,
                                                                                           string strFile);

        [DllImport("FreeSL.dll", EntryPoint = "?fslCreateListenerEnvironment@@YA?AUFSL_EAX_LISTENER_PROPERTIES@@PBDI@Z")
        ]
        public static extern FSL_EAX_LISTENER_PROPERTIES fslCreateListenerEnvironment(string strData, uint size);


        [DllImport("FreeSL.dll", EntryPoint = "?fslUpdate@@YAXXZ")
       ]
        public static extern void fslUpdate();

       [DllImport("FreeSL.dll", EntryPoint = "?fslSleep@@YAXM@Z")
       ]
        public static extern void fslSleep(float time);
         
        public delegate void ErrorCallbackDelegate(string s, bool b);

        [DllImport("FreeSL.dll", EntryPoint = "?fslSetErrorCallback@@YAXP6AXPBD_N@Z@Z")
       ]
        public static extern void fslSetErrorCallback(ErrorCallbackDelegate d);


        [DllImport("FreeSL.dll", EntryPoint = "?fslSetListenerVelocity@@YAXMMM@Z")
       ]
        public static extern void fslSetListenerVelocity(float x, float y, float z);


        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetVelocity@@YAXIMMM@Z")
        ]
        public static extern void fslSoundSetVelocity(uint obj, float x, float y, float z);


        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetPitchAllSounds@@YAXM@Z")
     ]
        public static extern void fslSoundSetPitchAllSounds(float p);


        [DllImport("FreeSL.dll", EntryPoint = "?fslSoundSetPitch@@YAXIM@Z")
     ]
        public static extern void fslSoundSetPitch(uint obj, float p);

        

      
    }
}