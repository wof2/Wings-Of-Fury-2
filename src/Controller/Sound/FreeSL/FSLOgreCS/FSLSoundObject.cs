using System.IO;

namespace FSLOgreCS
{
    public class FSLSoundObject
    {
        protected uint _sound;
        protected string _name;
        protected bool _withSound;

        protected bool _streaming;
        
        protected bool _loop;
        
        protected bool _shouldBePlaying = false;

        protected string _soundFile;
       

        public bool Streaming
        {
            get { return _streaming; }
        }

        public FSLSoundObject(string soundFile, string name, bool loop, bool streaming)
        {
            _withSound = false;
            _name = name;
            _loop = loop;
            SetSound(soundFile, loop, streaming);
        }

        public FSLSoundObject(string package, string soundFile, string name, bool loop)
        {
            _withSound = false;
            _name = name;
            _loop = loop;
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
            _streaming = streaming;
            if(!streaming) LoopSound(loop);
           _loop = loop;
           _soundFile = soundFile;
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
            _shouldBePlaying = true;
            FreeSL.fslSoundPlay(_sound);
        }

        public void Stop()
        {
            _shouldBePlaying = false;
            FreeSL.fslSoundStop(_sound);
        }

        public bool IsPlaying()
        {
            return FreeSL.fslSoundIsPlaying(_sound);

            // uwagi na bugi w streamingu w FreeSL trzeba zrobic workaround...
            /*
            if(FreeSL.fslSoundIsPlaying(_sound))
            {
                // nie wierzymy freeslowi
                return _playing;
            }
            else
            {
                // wierzymy ze dzwiek sie zakonczyl
                _playing = false;
                return false;
            }
            */
         
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
        	if(_streaming)
        	{
        		if(_loop && _shouldBePlaying && !IsPlaying())
	        	{
	        		this.SetSound(_soundFile, _loop, _streaming);
	        		
	        		Play();
	        		
	        	}
        	}
        	
        }
    }
}