using Mogre;

namespace FSLOgreCS
{
    public class FSLListener
    {
        private Camera _renderable;

        public FSLListener()
        {
            _renderable = null;
        }

        public FSLListener(Camera renderable)
        {
            _renderable = renderable;
        }

        public void SetListener(Camera renderable)
        {
            _renderable = renderable;
        }

        public void Update()
        {
            FreeSL.fslSetListenerPosition(_renderable.Position.x,
                                          _renderable.Position.y,
                                          _renderable.Position.z);
            Vector3 yVec, zVec;
            yVec = _renderable.Orientation.YAxis;
            zVec = _renderable.Orientation.ZAxis;
            FreeSL.fslSetListenerOrientation(zVec.x, zVec.y, zVec.z, yVec.x, yVec.y, yVec.z);
        }

        public Vector3 GetPosition()
        {
            return _renderable.Position;
        }
    }
}