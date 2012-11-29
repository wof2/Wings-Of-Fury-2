using System;
using System.IO;
using Mogre;
using Wof.Controller;

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
        protected bool _playing = false;

        private string _soundFile;


        protected float _baseGain = 1.0f;

        public bool Streaming
        {
            get { return _streaming; }
        }

        public FSLSoundObject(string soundFile, string name, bool loop, bool streaming)
        {
            _withSound = false;
            _name = name;
            _loop = loop;
            try
            {
                SetSound(soundFile, loop, streaming);
            }
            catch(Exception ex)
            {
                
            }
            
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
                Console.WriteLine("Destroying: " + this._soundFile);
                FreeSL.fslSoundSetGain(_sound, 0.1f);
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
            {
                Console.WriteLine("Streaming sound: " + soundFile);
                _sound = FreeSL.fslStreamSound(soundFile);
            }
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

        public string SoundFile
        {
            get { return _soundFile; }
        }

        public virtual void Play()
        {
            if(EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
            // dzwieki powinny miec ustawiona lokalna glosnosc (niezalezna od muzyki) przed rozpoczeciem pierwszego odtworzenia
          //  SetGain(GetBaseGain() * EngineConfig.SoundVolume / 100.0f);
            ApplyGain();
            try{
            	FreeSL.fslSoundPlay(_sound);
            }
            catch(Exception ex) {
            	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Exception while trying to play sound: " + _soundFile +" ("+ex.Message+") " + ex.StackTrace);
             
            	
            }
            _shouldBePlaying = true;
            _playing = true;
          
        }

        public void Stop()
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
            _shouldBePlaying = false;
            FreeSL.fslSoundStop(_sound);
            _playing = false;
        }

        public bool IsPlaying()
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return false;
            //return FreeSL.fslSoundIsPlaying(_sound);

            // uwagi na bugi w streamingu w FreeSL trzeba zrobic workaround...
            
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
            
         
        }

        public void Pause()
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
            FreeSL.fslSoundPause(_sound);
        }

        public bool IsPaused()
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return false;
            return FreeSL.fslSoundIsPaused(_sound);
        }

        public void LoopSound(bool loop)
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
            FreeSL.fslSoundSetLooping(_sound, loop);
        }
        /*
        public bool IsLooping()
        {
            return FreeSL.fslSoundIsLooping(_sound);
        }
        */

        public float GetBaseGain()
        {
            return _baseGain;
        }
        public void SetBaseGain(float baseGain)
        {
           // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "SETTING BASE GAIN: " + this.Name + "Base: " + baseGain+ "\nStack trace:"+System.Environment.StackTrace);
            this._baseGain = baseGain;
            ApplyGain();
        }

        protected void SetGain(float gain)
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
            //LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "SETTING GAIN: " + this.Name + " Base: " + this.GetBaseGain() + ", final gain:" + gain);
               
            FreeSL.fslSoundSetGain(_sound, gain);
        }

        public virtual void ApplyGain()
        {
            SetGain(_baseGain * EngineConfig.SoundVolume / 100.0f);
        }

        public virtual void Update()
        {
            if (EngineConfig.SoundSystem == FreeSL.FSL_SOUND_SYSTEM.FSL_SS_NOSYSTEM) return;
        	if(_streaming)
        	{
        		if(_shouldBePlaying && !IsPlaying())
	        	{
                    if (_loop)
                    {
                        this.SetSound(_soundFile, _loop, _streaming);
                        ApplyGain();
                        Play();
                    }
	        	}
        	}
        	
        }
    }
}