using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils
{
    //TODO make movable and better constructor
    public struct Camera
    {
        private float _aspectRatio;
        private float _farClipPlane;
        private float _fieldOfView;
        private float _nearClipPlane;

        private Vector3 _position;
        private Vector3 _target;
        private Vector3 _upVector;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 Target
        {
            get { return _target; }
        }

        public Vector3 UpVector
        {
            get { return _upVector; }
        }

        public Camera(float aspectRatio, float farClipPlane, float fieldOfView, float nearClipPlane, Vector3 position, Vector3 target, Vector3 upVector) : this()
        {
            _aspectRatio = aspectRatio;
            _farClipPlane = farClipPlane;
            _fieldOfView = fieldOfView;
            _nearClipPlane = nearClipPlane;
            _position = position;
            _target = target;
            _upVector = upVector;
        }

        public void updateCameraPositionTarget(Vector3 position, Vector3 target)
        {
            _position = position;
            _target = target;
        }

        public Matrix ViewMatrix
        {
            get { return Matrix.CreateLookAt(_position, _target, _upVector); }
        }

        public Matrix ProjectionMatrix
        {
            get { return Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearClipPlane, _farClipPlane); }
        }
    }
}
