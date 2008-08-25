using System;
using System.Collections.Generic;
using System.Text;
using FSLOgreCS;
using Mogre;

namespace Wof.Controller
{
    /// <summary>
    /// Singleton odpowiadaj¹ce za zaawansowane dŸwiêki oraz dekodowanie muzyki. Wykorzystuje FreeSL.
    /// </summary>
    internal class SoundManager3D : FSLSoundManager
    {

        public static readonly string C_ENEMY_ENGINE_IDLE = "sounds/enemy_engineidle.wav";
        public static readonly string C_SOLDIER_DIE_1 = "sounds/oh.wav";
        public static readonly string C_SOLDIER_DIE_2 = "sounds/oh2.wav";
        public static readonly string C_MENU_CLICK = "sounds/click.wav";

        private FreeSL.FSL_SOUND_SYSTEM soundSystem;
        private string currentMusic;
        public string CurrentMusic
        {
            get { return currentMusic; }
        }

        private FSLSoundObject ambientSound;

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


        public new bool InitializeSound(Camera camera, FreeSL.FSL_SOUND_SYSTEM ss)
        {

            if (Instance.Initialized && ss == soundSystem) 
            {
                this.SetListener(camera); 
                return true;
            }

            if(Instance.Initialized) Instance.Destroy();
            
            bool ok = base.InitializeSound(camera, ss); //InitializeSound sound system
            if (ok)
            {
                soundSystem = ss;
                currentMusic = null;
                ambientSound = null;
                return true;
            }
            return false;
        }

        public FSLSoundEntity CreateSoundEntity(String filename, IRenderable entity, bool loop, bool play)
        {
            FSLSoundEntity sound = (FSLSoundEntity)CreateSoundEntity(filename, entity, filename + "_entity" + entity.GetHashCode()+ "_Sound", loop, false);
            if (play)
            {
                sound.Play();
            }
            return sound;
        }


        public void PlayAmbient(String sound, int volume)
        {
            PlayAmbient(sound, volume, false);
        }

        /// <summary>
        /// Odgrywa dŸwiêk/muzykê jako ambient (slychaæ z tak¹ sam¹ g³oœnoœci¹ bez wzglêdu na po³o¿enie kamery)
        /// </summary>
        /// <param name="sound">plik z muzyk¹/dŸwiêkiem</param>
        /// <param name="volume">0-100</param>
        /// <param name="preloadOnly">czy tylko preloadowaæ muzykê</param>
        /// <param name="loop">zapêtlenie dziêku</param>
        public void PlayAmbient(String sound, int volume, bool preloadOnly, bool loop)
        {

            if (ambientSound == null || (ambientSound != null && !ambientSound.Name.Equals(sound + "_Ambient")))
            {
                
                if (ambientSound != null)
                {
                     RemoveSound(ambientSound.Name);
                     ambientSound.Destroy();
                }

                ambientSound = CreateAmbientSound(sound, sound + "_Ambient", loop, false);
                //Create Ambient sound  
                if (!preloadOnly) ambientSound.Play();
            }
            else
            {
                if (!ambientSound.IsPlaying() && !preloadOnly) ambientSound.Play();
            }
            ambientSound.SetGain(1.0f * volume / 100);

            currentMusic = sound;
        }


        public void PlayAmbient(String sound, int volume, bool preloadOnly)
        {
            PlayAmbient(sound, volume, preloadOnly, true);
        }


        public void PlayAmbient(String sound)
        {
            PlayAmbient(sound, EngineConfig.MusicVolume);
        }

        public void StopAmbient()
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
