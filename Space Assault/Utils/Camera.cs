using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Utils
{
    //TODO make movable and better constructor
    public class Camera
    {
        private float _aspectRatio;
        private float _farClipPlane = 10000f;
        private float _fieldOfView = MathHelper.PiOver2;
        private float _nearClipPlane = 0.1f;

        private Vector3 _position = new Vector3(0, 100, 0);
        private Vector3 _target = Vector3.UnitZ;
        private Vector3 _upVector = Vector3.UnitZ;

        public Camera(GraphicsDeviceManager graphics)
        {
            _aspectRatio = graphics.GraphicsDevice.DisplayMode.AspectRatio;
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
