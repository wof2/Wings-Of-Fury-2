using System;
using Mogre;

namespace FSLOgreCS
{
    public class FSLListener
    {
        private Camera _renderable;
        public bool ZFlipped = true; // reversed stereo hack

        private Wof.Model.Level.Planes.Plane _plane = null;

        public Camera Renderable
        {
            get { return _renderable; }
        }
        
        public FSLListener()
        {
            _renderable = null;
        }


        public FSLListener(Camera renderable)
        {
            _renderable = renderable;
        }

        public FSLListener(Camera renderable, Wof.Model.Level.Planes.Plane plane)
        {
            _renderable = renderable;
            _plane = plane;
        }

        public void SetListener(Camera renderable, Wof.Model.Level.Planes.Plane plane)
        {
            _renderable = renderable;
            _plane = plane;
        }


        public void Update()
        {
            unsafe
            {
                if (_renderable == null || _renderable.NativePtr == null) return;
            }

            try
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
            catch (Exception)
            {

               
            }
          
        } 

        public Vector3 GetPosition()
        {
            return _renderable.Position;
        }
    }
}