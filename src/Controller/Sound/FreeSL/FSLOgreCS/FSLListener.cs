using Mogre;

namespace FSLOgreCS
{
    public class FSLListener
    {
        private Camera _renderable;
        public bool ZFlipped = true; // reversed stereo hack
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
            int zflip = (ZFlipped) ? -1 : 1; // added
          
            FreeSL.fslSetListenerPosition(_renderable.RealPosition.x,
                                          _renderable.RealPosition.y,
                                          _renderable.RealPosition.z); 
           
            Mogre.Vector3 yVec, zVec;
            yVec = _renderable.RealOrientation.YAxis;
            zVec = _renderable.RealOrientation.ZAxis * zflip;// change

            FreeSL.fslSetListenerOrientation(zVec.x, zVec.y, zVec.z, yVec.x, yVec.y, yVec.z); 
        } 

        public Vector3 GetPosition()
        {
            return _renderable.Position;
        }
    }
}