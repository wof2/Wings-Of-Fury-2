using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FSLOgreCS;
using Mogre;

namespace Wof.Controller
{
    /// <summary>
    /// Singleton odpowiadaj¹ce za zaawansowane dŸwiêki oraz dekodowanie muzyki. Wykorzystuje FreeSL.
    /// </summary>
    internal class SoundManager3D : FSLSoundManager, IDisposable
    {

        public static readonly string C_ENEMY_ENGINE_IDLE = "sounds/enemy_engineidle.ogg";
        public static readonly string C_SOLDIER_DIE_1 = "sounds/oh.ogg";
        public static readonly string C_SOLDIER_DIE_2 = "sounds/oh2.ogg";
        public static readonly string C_SOLDIER_DIE_3 = "sounds/oh3.ogg";
        public static readonly string C_SOLDIER_DIE_4 = "sounds/oh4.ogg";
        public static readonly string C_SOLDIER_DIE_5 = "sounds/oh5.ogg";


        public static readonly string C_MENU_CLICK = "sounds/click.ogg";
        public static readonly string C_MENU_CHEAT = "sounds/mario.ogg";
        public static readonly string C_STORAGE_PLANE_DESTROYED = "sounds/storage_plane.ogg";

        public static readonly string C_ENEMY_WARCRY = "sounds/banzai.wav";
        public static readonly string C_ENEMY_WARCRY2 = "sounds/banzai2.wav";
        public static readonly string C_PLANE_PASS = "sounds/plane_pass.wav"; 
        public static readonly string C_SHIP_SINKING = "sounds/ship_sinking.ogg"; 
        public static readonly string C_SHIP_SINKING_2 = "sounds/ship_sinking2.ogg";
        public static readonly string C_GUN = "sounds/machinegun.wav"; 

        public static readonly string C_MISSILE_LOCK = "sounds/missile_lock.wav";
        public static readonly string C_RAVEN = "sounds/raven.wav";
       
        
       

        private FreeSL.FSL_SOUND_SYSTEM soundSystem;
        private string currentMusic;
        public string CurrentMusic
        {
            get { return currentMusic; }
        }

        private FSLSoundObject ambientSound;

        private IDictionary<String, FSLSoundObject> ambientSounds = new Dictionary<String, FSLSoundObject>();

        protected SoundManager3D()
        {
        }

        protected class SingletonCreator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonCreator()
            {
            }

            internal static readonly SoundManager3D instance = new SoundManager3D();
        }


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public new static SoundManager3D Instance
        {
            get { return SingletonCreator.instance; }
        }
        
        
        public static void SetMusicVolume(uint volume)
        {
        	EngineConfig.MusicVolume = (int)volume;
            if (EngineConfig.MusicVolume == 0)
            {
           
            }
            if (SoundManager3D.Instance.CurrentMusic != null)
            {
                SoundManager3D.Instance.PlayAmbientMusic(SoundManager3D.Instance.CurrentMusic, 100);
            }

            
            EngineConfig.SaveEngineConfig();          
        }
        
		public static void SetSoundVolume(uint volume)
        {
           
           // FreeSL.fslSetVolume(volume / 100.0f);
        	EngineConfig.SoundVolume = (int)volume;
            foreach (FSLSoundObject soundObject in SoundManager3D.Instance.SoundObjectVector)
            {
                if (soundObject is FSLSoundEntity)
                {
                    soundObject.ApplyGain();
                }
            }



            EngineConfig.SaveEngineConfig();
        }

        public new bool InitializeSound(CameraListenerBase listener, FreeSL.FSL_SOUND_SYSTEM ss)
        {

            if (Instance.Initialized && ss == soundSystem) 
            {
                this.SetListener(listener); 
                return true;
            }

            if(Instance.Initialized)
            {
                this.ambientSounds.Clear();
                Instance.Destroy();
            }

            bool ok = base.InitializeSound(listener, ss); //InitializeSound sound system
            if (ok)
            {
                soundSystem = ss;
                StopAmbientMusic();
                currentMusic = null;
                ambientSound = null;
                return true;
            }
            return false;
        }

        public FSLSoundEntity CreateSoundEntity(String filename, SceneNode entity, bool loop, bool play)
        {
            FSLSoundEntity sound = (FSLSoundEntity)CreateSoundEntity(filename, entity, filename + "_entity" + entity.GetHashCode()+ "_Sound", loop, false);
            if (play)
            {
                sound.Play();
            }
            return sound;
        }


        public void PlayAmbientMusic(String sound, int volume)
        {
            PlayAmbientMusic(sound, volume, false);
        }

        public void PlayAmbientMusic(String sound, int volume, bool preloadOnly, bool loop)
        {
            PlayAmbientMusic(sound, volume, preloadOnly, loop, false);
        }

        /// <summary>
        /// Odgrywa dŸwiêk/muzykê jako ambient (slychaæ z tak¹ sam¹ g³oœnoœci¹ bez wzglêdu na po³o¿enie kamery)
        /// </summary>
        /// <param name="sound">plik z muzyk¹/dŸwiêkiem</param>
        /// <param name="volume">0-100</param>
        /// <param name="preloadOnly">czy tylko preloadowaæ muzykê</param>
        /// <param name="loop">zapêtlenie dziêku</param>
        /// <param name="streaming"></param>
        public void PlayAmbientMusic(String sound, int volume, bool preloadOnly, bool loop, bool streaming)
        {
            streaming = false;

            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;

            if (ambientSound == null || (ambientSound != null && !ambientSound.Name.Equals(sound + "_Ambient")))
            {
                
               /* if (ambientSound != null)
                {
                     RemoveSound(ambientSound.Name);
                     ambientSound.Destroy();
                }*/
                if(ambientSounds.ContainsKey(sound))
                {
                    ambientSound = ambientSounds[sound];
                }
                else
                {
                    ambientSound = CreateAmbientSoundMusic(sound, sound + "_Ambient", loop, streaming);
                    ambientSounds[sound] = ambientSound;
                }

               
                ambientSound.SetBaseGain(volume / 100.0f);
               // ambientSound.ApplyGain();
                //Create Ambient sound  
                if (!preloadOnly)
                {
                    ambientSound.Play();
                }
            }
            else
            {
                if (ambientSound!=null)
                {
                    ambientSound.SetBaseGain(1.0f * volume / 100);
                   
                    if (!ambientSound.IsPlaying() && !preloadOnly)
                    {
                        ambientSound.Play();
                    }
                }
                
            }
           

            currentMusic = sound;
        }


        public void PlayAmbientMusic(String sound, int volume, bool preloadOnly)
        {
            PlayAmbientMusic(sound, volume, preloadOnly, true, EngineConfig.AudioStreaming);
        }

        public void PlayAmbientMusic(String sound, bool loop)
        {
            PlayAmbientMusic(sound, 100, false, loop, true);
        }

        public void PlayAmbientMusic(String sound)
        {
            PlayAmbientMusic(sound, 100);
        }

        public void StopAmbientMusic()
        {
            if (ambientSound != null)
            {
                ambientSound.Stop();
            }
        }



        public void CreateFrameListener(Root root)
        {
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            //Add sound listener so it will update every frame
        }

        public void Dispose()
        {
            Destroy();
        }

    }
}
