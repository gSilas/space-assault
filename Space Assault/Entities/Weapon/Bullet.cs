using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Space_Assault.Entities.Weapon
{
    class Bullet : AEntity
    {
       // private Vector3 _position;
        private Vector3 _direction;
        private Vector3 _travelspeed;

        public Bullet(Vector3 position, Vector3 direction, Vector3 travelspeed)
        {
            Position = position;
            _direction = direction;
            _travelspeed = travelspeed;

            _direction = direction;
            _direction.Normalize();
            _travelspeed = travelspeed;

            //kollision später
            //worldMatrix = Matrix.CreateWorld(base.Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(0.05f);
        }
        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            //Model = cm.Load<Model>("Models/asteroid");
        }

        public override void Update(GameTime gameTime)
        {

            //RotationMatrix = Matrix.Identity;

            Position += _direction;
        }

    }
}
