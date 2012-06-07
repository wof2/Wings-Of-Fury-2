using System;
using Mogre;
using Wof.Controller;

namespace FSLOgreCS
{
    public class FSLListener
    {
        private CameraListenerBase _listener;
        public bool ZFlipped = true; // reversed stereo hack

        private Wof.Model.Level.Planes.Plane _plane = null;

        public CameraListenerBase Listener
        {
            get { return _listener; }
        }
        
        public FSLListener()
        {
            _listener = null;
        }


        public FSLListener(CameraListenerBase listener)
        {
            _listener = listener;
        }

        public FSLListener(CameraListenerBase listener, Wof.Model.Level.Planes.Plane plane)
        {
            _listener = listener;
            _plane = plane;
        }

        public void SetListener(CameraListenerBase listener, Wof.Model.Level.Planes.Plane plane)
        {
            _listener = listener;
            _plane = plane;
        }


        public void Update()
        {
            unsafe
            {
                if (_listener == null || _listener.NativePtr == null) return;
            }

            if(!_listener.IsReady())
            {
                return;
            }
            try
            {
               int zflip = (ZFlipped) ? -1 : 1; // added

               FreeSL.fslSetListenerPosition(_listener.CameraLastRealPosition.Value.x,
                                              _listener.CameraLastRealPosition.Value.y,
                                              _listener.CameraLastRealPosition.Value.z);

                Mogre.Vector3 yVec, zVec;
                yVec = _listener.CameraLastRealOrientation.Value.YAxis;
                zVec = _listener.CameraLastRealOrientation.Value.ZAxis *zflip;// change
               
                FreeSL.fslSetListenerOrientation(zVec.x, zVec.y, zVec.z, yVec.x, yVec.y, yVec.z); 

            }
            catch (Exception exception)
            {

             //   Console.WriteLine("AAAA " + exception.Message + " " + exception.InnerException + " " + exception.Source);
            }
          
        } 

       
    }
}