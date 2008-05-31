using System.Collections.Generic;
using Mogre;

namespace FSLOgreCS
{
    public class FSLSoundManager
    {
        #region Variables

        private List<FSLSoundObject> _soundObjectVector = new List<FSLSoundObject>();
        private bool _initSound;
        private FSLListener _listener;

        #endregion

        #region Singleton Stuff

        private FSLSoundManager()
        {
            _initSound = false;
            _listener = null;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static FSLSoundManager Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonCreator()
            {
            }

            internal static readonly FSLSoundManager instance = new FSLSoundManager();
        }

        #endregion

        #region Private Members

        private FSLSoundObject AddSound(FSLSoundObject sound)
        {
            _soundObjectVector.Add(sound);
            return sound;
        }

        #endregion

        public bool InitializeSound(Camera listener, FreeSL.FSL_SOUND_SYSTEM soundSystem)
        {
            _listener = new FSLListener(listener);
            if (_initSound)
                return true;

            if (!FreeSL.fslInit(soundSystem)) //Change if desire
                return false;

            _initSound = true;
            return true;
        }

        public void ShutDown()
        {
            FreeSL.fslShutDown();
            _initSound = false;
        }

        public float Volume
        {
            set { FreeSL.fslSetVolume(value); }
        }

        public bool Initialized
        {
            get { return _initSound; }
        }

        public void RemoveSound(string name)
        {
            FSLSoundObject sound = GetSound(name);
            if (sound == null)
                return;
            else
                sound = null;
        }

        public FSLSoundObject GetSound(string name)
        {
            if (_soundObjectVector.Count == 0)
                return null;
            foreach (FSLSoundObject sound in _soundObjectVector)
            {
                if (sound.Name == name)
                    return sound;
            }
            return null;
        }

        public void UpdateSoundObjects()
        {
            if (!_initSound)
                return;
            _listener.Update();
            foreach (FSLSoundObject sound in _soundObjectVector)
            {
                sound.Update();
            }
        }

        public void SetListener(Camera listener)
        {
            _listener = new FSLListener(listener);
        }

        public FSLListener GetListener()
        {
            return _listener;
        }


        public FSLSoundObject CreateAmbientSound(string soundFile, string name, bool loop, bool streaming)
        {
            return AddSound(new FSLAmbientSound(soundFile, name, loop, streaming));
        }

        public FSLSoundObject CreateSoundEntity(string soundFile, IRenderable renderable, string name, bool loop,
                                                bool streaming)
        {
            return AddSound(new FSLSoundEntity(soundFile, renderable, name, loop, streaming));
        }

        public FSLSoundObject CreateAmbientSound(string package, string soundFile, string name, bool loop)
        {
            return AddSound(new FSLAmbientSound(package, soundFile, name, loop));
        }

        public FSLSoundObject CreateSoundEntity(string package, string soundFile, IRenderable renderable, string name,
                                                bool loop)
        {
            return AddSound(new FSLSoundEntity(package, soundFile, renderable, name, loop));
        }

        public bool FrameStarted(FrameEvent evt)
        {
            UpdateSoundObjects();
            return true;
        }

        public void Destroy()
        {
            if (_soundObjectVector.Count != 0)
            {
                foreach (FSLSoundObject sound in _soundObjectVector)
                {
                    sound.Destroy();
                }
                _soundObjectVector.Clear();
            }
            if (_listener != null)
                _listener = null;
            if (_initSound)
                ShutDown();
        }

        #region Environment Functions

        /// <summary>
        /// Sets the listener environment.
        /// </summary>
        /// <param name="prop">The prop.</param>
        public void SetListenerEnvironment(FreeSL.FSL_EAX_LISTENER_PROPERTIES prop)
        {
            FreeSL.fslSetListenerEnvironment(prop);
        }

        /// <summary>
        /// Sets the listener environment preset.
        /// </summary>
        /// <param name="type">The type.</param>
        public void SetListenerEnvironmentPreset(FreeSL.FSL_LISTENER_ENVIRONMENT type)
        {
            FreeSL.fslSetListenerEnvironmentPreset(type);
        }

        /// <summary>
        /// Sets the listener default environment.
        /// </summary>
        public void SetListenerDefaultEnvironment()
        {
            FreeSL.fslSetListenerDefaultEnvironment();
        }

        /// <summary>
        /// Gets the current listener environment.
        /// </summary>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES GetCurrentListenerEnvironment()
        {
            return FreeSL.fslGetCurrentListenerEnvironment();
        }

        /// <summary>
        /// Loads the listener environment.
        /// </summary>
        /// <param name="strFile">The STR file.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES LoadListenerEnvironment(string strFile)
        {
            return FreeSL.fslLoadListenerEnvironment(strFile);
        }

        /// <summary>
        /// Loads the listener environment from zip.
        /// </summary>
        /// <param name="strFile">The STR file.</param>
        /// <param name="strPackage">The STR package.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES LoadListenerEnvironmentFromZip(string strFile, string strPackage)
        {
            return FreeSL.fslLoadListenerEnvironmentFromZip(strPackage, strFile);
        }

        /// <summary>
        /// Creates the listener environment.
        /// </summary>
        /// <param name="strData">The STR data.</param>
        /// <param name="Size">The size.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES CreateListenerEnvironment(string strData, uint Size)
        {
            return FreeSL.fslCreateListenerEnvironment(strData, Size);
        }

        #endregion
    }
}