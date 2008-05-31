using System.IO;

namespace FSLOgreCS
{
    public class FSLSoundObject
    {
        protected uint _sound;
        protected string _name;
        protected bool _withSound;

        public FSLSoundObject(string soundFile, string name, bool loop, bool streaming)
        {
            _withSound = false;
            _name = name;
            SetSound(soundFile, loop, streaming);
        }

        public FSLSoundObject(string package, string soundFile, string name, bool loop)
        {
            _withSound = false;
            _name = name;
            SetSound(package, soundFile, loop);
        }

        public void Destroy()
        {
            RemoveSound();
        }


        public void RemoveSound()
        {
            if (_withSound)
            {
                FreeSL.fslFreeSound(_sound, true);
                _withSound = false;
            }
        }

        public void SetSound(string soundFile, bool loop, bool streaming)
        {
            RemoveSound();
            if (File.Exists(soundFile) == false)
                throw new FileNotFoundException("The sound file at : " + soundFile + " does not exist.");
            if (streaming)
                _sound = FreeSL.fslStreamSound(soundFile);
            else
                _sound = FreeSL.fslLoadSound(soundFile);

            LoopSound(loop);
            _withSound = true;
        }

        public void SetSound(string package, string soundFile, bool loop)
        {
            RemoveSound();
            if (File.Exists(package) == false)
                throw new FileNotFoundException("The sound file at : " + soundFile + " does not exist.");
            FreeSL.fslLoadSoundFromZip(package, soundFile);
            LoopSound(loop);
            _withSound = true;
        }

        public bool HasSound()
        {
            return _withSound;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void Play()
        {
            FreeSL.fslSoundPlay(_sound);
        }

        public void Stop()
        {
            FreeSL.fslSoundStop(_sound);
        }

        public bool IsPlaying()
        {
            return FreeSL.fslSoundIsPlaying(_sound);
        }

        public void Pause()
        {
            FreeSL.fslSoundPause(_sound);
        }

        public bool IsPaused()
        {
            return FreeSL.fslSoundIsPaused(_sound);
        }

        public void LoopSound(bool loop)
        {
            FreeSL.fslSoundSetLooping(_sound, loop);
        }

        public bool IsLooping()
        {
            return FreeSL.fslSoundIsLooping(_sound);
        }

        public void SetGain(float gain)
        {
            FreeSL.fslSoundSetGain(_sound, gain);
        }

        public virtual void Update()
        {
        }
    }
}