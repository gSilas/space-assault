using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Entities.Weapon
{
    public class Bullet : AEntity
    {
        private Vector3 _direction;
        private float _travelspeed;
        public int Bulletlife;

        public Bullet(Vector3 position, Matrix droneRotateMatrix, float travelspeed, Model model)
        {
            Position = position;
            _direction = droneRotateMatrix.Forward;
            RotationMatrix = droneRotateMatrix;
            _travelspeed = travelspeed;
            Model = model;
            Bulletlife = 500;
        }
        public override void Update(GameTime gameTime)
        {
            Position -= _direction*_travelspeed;
            Bulletlife--;
           
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
