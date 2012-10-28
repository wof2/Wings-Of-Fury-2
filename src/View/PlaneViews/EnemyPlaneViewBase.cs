using System;
using FSLOgreCS;
using Mogre;
using Wof.Controller;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    public abstract class EnemyPlaneViewBase : PlaneView
    {
        protected FSLSoundObject engineSound = null;
        protected FSLSoundObject warCrySound = null;
        protected FSLSoundObject warCrySound2 = null;
        protected FSLSoundObject gunSound = null;
        protected Random random;
       
        public EnemyPlaneViewBase(Plane plane, IFrameWork frameWork, SceneNode parentNode, String name)
            : base(plane, frameWork, parentNode, name)
        {
        }

        public void PlayGunSound()
        {
            //LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "START");
            if (EngineConfig.SoundEnabled && !gunSound.IsPlaying())
            {
                // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, " -NEW LOOP");
                gunSound.SetBaseGain(1.0f);
                gunSound.Play();
                //SoundManager3D.Instance.UpdateSoundObjects();
            }
        }

        public void StopGunSound()
        {
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "STOP");
            if (EngineConfig.SoundEnabled) gunSound.Stop();
        }

        public void PlayWarcry()
        {
           
            if (random.Next(0, 101) > 50)
            {
                if (EngineConfig.SoundEnabled && !warCrySound.IsPlaying()) warCrySound.Play();
            }
            else
            {
                if (EngineConfig.SoundEnabled && !warCrySound2.IsPlaying()) warCrySound2.Play();
            }

        }

        public override void Destroy()
        {
            base.Destroy();
            if (engineSound != null)
            {
                SoundManager3D.Instance.RemoveSound(engineSound.Name);
                engineSound.Destroy();
                engineSound = null;
            }


            if (warCrySound != null)
            {
                SoundManager3D.Instance.RemoveSound(warCrySound.Name);
                warCrySound.Destroy();
                warCrySound = null;
            }


            if (warCrySound2 != null)
            {
                SoundManager3D.Instance.RemoveSound(warCrySound2.Name);
                warCrySound2.Destroy();
                warCrySound2 = null;
            }

            if (gunSound != null)
            {
                SoundManager3D.Instance.RemoveSound(gunSound.Name);
                gunSound.Destroy();
                gunSound = null;
            }

        }


        public void LoopEngineSound()
        {
            if (EngineConfig.SoundEnabled && !engineSound.IsPlaying())
            {
                engineSound.SetBaseGain(0.3f);
                engineSound.Play();
                //SoundManager3D.Instance.UpdateSoundObjects();
            }
        }

        public void StopEngineSound()
        {
            if (EngineConfig.SoundEnabled) engineSound.Stop();
        }
    }
}