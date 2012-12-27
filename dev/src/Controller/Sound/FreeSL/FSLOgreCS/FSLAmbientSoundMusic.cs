using Mogre;
using Wof.Controller;

namespace FSLOgreCS
{
    /// <summary>
    /// Obiekt ktory nie zalezy od kamery i jednoczesnie jego glosnosc zalezy od EngineConfig.MusicVolume
    /// </summary>
    public class FSLAmbientSoundMusic : FSLAmbientSound
    {
        public FSLAmbientSoundMusic(string musicFile, string name, bool loop, bool streaming)
            : base(musicFile, name, loop, streaming)
        {
        }

        private bool _isOnPlaylist = false;

        public bool IsOnPlaylist
        {
            get { return _isOnPlaylist; }
            set { _isOnPlaylist = value; }
        }

        public override void ApplyGain()
        {
            SetGain(_baseGain * EngineConfig.MusicVolume / 100.0f);
        }
        public override void Update()
        { 
        	base.Update();
            if (_isOnPlaylist && _shouldBePlaying && !IsPlaying())
            {
                if (!_loop)
                {
                    LogManager.Singleton.LogMessage("Track '" + this.Name + "' finished -> ShouldLoadNextMusic");
                    // Console.WriteLine("ShouldLoadNextMusic=true : "+this.SoundFile);
                    SoundManager3D.Instance.ShouldLoadNextMusic = true;
                }

            }
        }
    }
}