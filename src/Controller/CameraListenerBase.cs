using Mogre;

namespace Wof.Controller
{
    public class CameraListenerBase : MovableObject.Listener
    {
        private Vector3? cameraLastRealPosition = null;
        private Quaternion? cameraLastRealOrientation = null;
        private Camera camera;

        public CameraListenerBase(Camera camera)
        {
            this.camera = camera;
        }

        public Vector3? CameraLastRealPosition
        {
            get { return cameraLastRealPosition; }
        }

        public Quaternion? CameraLastRealOrientation
        {
            get { return cameraLastRealOrientation; }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public bool IsReady()
        {
            return cameraLastRealPosition.HasValue && cameraLastRealOrientation.HasValue;
        }
        public override void ObjectMoved(Mogre.MovableObject o)
        {
            if(o.Equals(camera))
            {
                cameraLastRealPosition = (o as Camera).RealPosition; // clone
                cameraLastRealOrientation = (o as Camera).RealOrientation; // clone
            }
            base.ObjectMoved(o);
        }

        public override bool ObjectRendering(Mogre.MovableObject o, Mogre.Camera c)
        {
            return base.ObjectRendering(o, c);
        }
    }
}