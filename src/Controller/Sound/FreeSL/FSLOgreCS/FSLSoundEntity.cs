using Mogre;
using Wof.Controller;

namespace FSLOgreCS
{
    public class FSLSoundEntity : FSLSoundObject
    {
        protected SceneNode _renderable;

        public FSLSoundEntity(string soundFile, SceneNode renderable, string name, bool loop, bool streaming)
            : base(soundFile, name, loop, streaming)
        {
            _renderable = renderable;
            SetReferenceDistance(80.0f);
        }

        public FSLSoundEntity(string package, string soundFile, SceneNode renderable, string name, bool loop)
            : base(package, soundFile, name, loop)
        {
            _renderable = renderable;
            SetReferenceDistance(80.0f);
        }

        public void SetRenderable(SceneNode renderable)
        {
            _renderable = renderable;
        }

        public override void Update()
        {
        	base.Update();
           // if (_renderable != null)
            {
                FreeSL.fslSoundSetPosition(_sound,
                                           _renderable._getDerivedPosition().x,
                                           _renderable._getDerivedPosition().y,
                                           _renderable._getDerivedPosition().z);
            }
        }
        
        public override void Play()
        {
        	base.Play();
        }

        public void SetMaxDistance(float distance)
        {
            FreeSL.fslSoundSetMaxDistance(_sound, distance);
        }

        public void SetReferenceDistance(float distance)
        {
            FreeSL.fslSoundSetReferenceDistance(_sound, distance);
        }
    }
}