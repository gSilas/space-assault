
using Microsoft.Xna.Framework;

namespace Space_Assault.Utils
{
    //TODO make movable and better constructor
    public struct Camera
    {
        private float _aspectRatio;
        private float _farClipPlane;
        private float _fieldOfView;
        private float _nearClipPlane;

        private Vector3 _position;
        public Vector3 Target;
        private Vector3 _upVector;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Camera(float aspectRatio, float farClipPlane, float fieldOfView, float nearClipPlane, Vector3 position, Vector3 target, Vector3 upVector) : this()
        {
            _aspectRatio = aspectRatio;
            _farClipPlane = farClipPlane;
            _fieldOfView = fieldOfView;
            _nearClipPlane = nearClipPlane;
            _position = position;
            Target = target;
            _upVector = upVector;
        }

        public Matrix ViewMatrix
        {
            get { return Matrix.CreateLookAt(_position, Target, _upVector); }
        }

        public Matrix ProjectionMatrix
        {
            get { return Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearClipPlane, _farClipPlane); }
        }
    }
}
