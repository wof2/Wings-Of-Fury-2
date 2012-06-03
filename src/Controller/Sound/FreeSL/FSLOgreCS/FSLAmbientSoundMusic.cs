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

        public FSLAmbientSoundMusic(string package, string musicFile, string name, bool loop)
            : base(package, musicFile, name, loop)
        {
        }

        public override void ApplyGain()
        {
            SetGain(_baseGain * EngineConfig.MusicVolume / 100.0f);
        }
        public override void Update()
        { 
        	base.Update();
            if(this.IsPlaying())
            {
            
             //   FreeSL.fslUpdate();
                //FreeSL.fslSleep(0.01f);
            }
        }
    }
}