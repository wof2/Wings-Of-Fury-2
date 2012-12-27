using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        public static readonly string C_SHIP_SUBMERGING = "sounds/ship_submerging.ogg";
        public static readonly string C_SHIP_EMERGING = "sounds/ship_emerging.ogg";

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

   


        public void SetMusicVolume(uint volume)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Changing music volume to: " + volume);

        	EngineConfig.MusicVolume = (int)volume;
            if (EngineConfig.MusicVolume == 0)
            {
           
            }
            if (SoundManager3D.Instance.CurrentMusic != null)
            {
                //SoundManager3D.Instance.CurrentMusic
               
                ambientSound.SetBaseGain(1.0f * volume / 100.0f);
               // SoundManager3D.Instance.PlayAmbientMusic(SoundManager3D.Instance.CurrentMusic, EngineConfig.MusicVolume);
            }

            
            EngineConfig.SaveEngineConfig();          
        }
        
		public static void SetSoundVolume(uint volume)
        {
           
           // FreeSL.fslSetVolume(volume / 100.0f);
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Changing sound volume to: "+volume);

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
                SoundManager3D.Instance.UpdaterRunning = true;
                return true;
            }
            return false;
        }

        public FSLSoundEntity CreateSoundEntity(String filename, SceneNode entity, bool loop, bool play)
        {
            FSLSoundEntity sound = (FSLSoundEntity)CreateSoundEntity(filename, entity, filename + "_entity" + entity.Name+ "_" + entity.GetHashCode() + "_Sound", loop, false);
            if (play)
            {
                sound.Play();
            }
            return sound;
        }

        private Thread soundPreloaderWorker;

        public void PrepareMusic(String sound, bool loop)
        {
            SoundPreloader preloader = new SoundPreloader(sound, loop);
            if (soundPreloaderWorker != null && !IsSoundPreloadWorkerReady)
            {
                KillPreoloadWorker();
            }
            soundPreloaderWorker = new Thread(new ThreadStart(preloader.Work));
            soundPreloaderWorker.Start();

        }

        public bool IsSoundPreloadWorkerReady
        {
            get {
                return soundPreloaderWorker != null &&
                       (soundPreloaderWorker.ThreadState == ThreadState.Stopped ||
                        soundPreloaderWorker.ThreadState == ThreadState.Unstarted); }
        }

        public bool ShouldLoadNextMusic
        {
            get { return _shouldLoadNextMusic; }
            set
            {
                if (value == true)
                {
                    if (IsSoundPreloadWorkerReady)
                    {
                        _shouldLoadNextMusic = value;
                    }
                }
                else
                {
                    _shouldLoadNextMusic = value;
                }
               
            }

        }

        private bool _shouldLoadNextMusic = false;

        private bool readyToPlayPreloadedMusic = false;
        public void SetReadyToPlayPreloadedMusic()
        {
            readyToPlayPreloadedMusic = true;
        }

        internal class SoundPreloader
        {
            protected string sound;
            protected bool loop;

            public SoundPreloader(String sound, bool loop)
            {
                this.sound = sound;
                this.loop = loop;
            }

            public void Work()
            {
                FSLSoundObject obj = Instance.CreateAmbientSoundMusic(sound, sound + "_Ambient", loop, false);
                Instance.OnSoundPreloaded(sound, obj, loop);
            }
        }

        public void OnSoundPreloaded(string sound, FSLSoundObject obj, bool loop)
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Sound preloaded: " + sound);
            ambientSounds[sound] = obj;
            
            while (!readyToPlayPreloadedMusic)
            {
                Thread.Sleep(100);
            }
			 LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Playback of sound: " + sound);
            
            PlayAmbientMusic(sound, EngineConfig.MusicVolume, false, loop, false);
            readyToPlayPreloadedMusic = false;
        }


        /// <summary>
        /// Odgrywa dŸwiêk/muzykê jako ambient (slychaæ z tak¹ sam¹ g³oœnoœci¹ bez wzglêdu na po³o¿enie kamery)
        /// </summary>
        /// <param name="sound">plik z muzyk¹/dŸwiêkiem</param>
        /// <param name="volume">0-100</param>
        /// <param name="preloadOnly">czy tylko preloadowaæ muzykê</param>
        /// <param name="loop">zapêtlenie dziêku</param>
        /// <param name="streaming"></param>
        public FSLSoundObject PlayAmbientMusic(String sound, int volume, bool preloadOnly, bool loop, bool streaming)
        {
           // streaming = false;

            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return null;

            try 
            {
            	
            
	            if (ambientSound == null || (ambientSound != null && !ambientSound.Name.Equals(sound + "_Ambient")))
	            {
	
	                // stop old sound
	                if (!preloadOnly && (ambientSound != null && !ambientSound.Name.Equals(sound + "_Ambient") && ambientSound.IsPlaying()))
	                {
	                    ambientSound.Stop();
	                    
	                }
	                
	                if (ambientSound != null)
	                {
	                     RemoveSound(ambientSound.Name);
                         ambientSounds.Remove(ambientSound.SoundFile);
	                }
	
	
	                if(ambientSounds.ContainsKey(sound))
	                {
	                    ambientSound = ambientSounds[sound];
	                }
	                else
	                {
	                    ambientSound = CreateAmbientSoundMusic(sound, sound + "_Ambient", loop, streaming);
	                    ambientSounds[sound] = ambientSound;
	                }
	             
	                ambientSound.SetBaseGain(1.0f * volume / 100.0f);
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
	                    ambientSound.SetBaseGain(1.0f * volume / 100.0f);
	                   
	                    if (!ambientSound.IsPlaying() && !preloadOnly)
	                    {
	                        ambientSound.Play();
	                    }
	                }
	                
	            }
	           
	
	            currentMusic = sound;
            }
            catch(Exception ex) {            	
            	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Error while trying to play ambient music:"+ ex.ToString());            	
            }

            return ambientSound;
        }


        public void PlayAmbientMusic(String sound, int volume, bool loop, bool preloadOnly)
        {
            PlayAmbientMusic(sound, volume, preloadOnly, loop, EngineConfig.AudioStreaming);
        }

        public void PlayAmbientMusic(String sound, bool loop)
        {
            PlayAmbientMusic(sound, EngineConfig.MusicVolume, false, loop, EngineConfig.AudioStreaming);
        }


        public void StopAmbientMusic()
        {
            if (ambientSound != null)
            {
                ambientSound.Stop();
                if (EngineConfig.AudioStreaming) // streaming nie pozwala na pauzowanie
                {
                    RemoveSound(ambientSound.Name);
                    ambientSounds.Remove(ambientSound.SoundFile);

                    ambientSound = null;
                }
            }

         
        }



        public void CreateFrameListener(Root root)
        {
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            //Add sound listener so it will update every frame
        }

        protected void KillPreoloadWorker()
        {
            if (soundPreloaderWorker != null)
            {
                try
                {
                    soundPreloaderWorker.Abort();
                }
                catch (Exception)
                {
                }

                while (soundPreloaderWorker.ThreadState != ThreadState.Stopped && soundPreloaderWorker.ThreadState != ThreadState.Aborted)
                {
                    Thread.Sleep(100);
                }
                soundPreloaderWorker = null;
            }

        }
        public override void Destroy()
        {
            KillPreoloadWorker();
          
            base.Destroy();
        }

        public void Dispose()
        {
            Destroy();
        }

    }
}
