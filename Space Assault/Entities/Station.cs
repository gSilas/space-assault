using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    class Station: AEntity
    {
        private float _angle;
        private bool _up;

        public Station(Vector3 position, float angle)
        {
            _angle = angle;
            Position = position;
            _up = true;
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("station");
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.005f;
            if (Position.Y < 1 && _up)
                Position += new Vector3(0, 0.002f, 0);
            else if (Position.Y < 0)
                _up = true;
            else
            {
                Position -= new Vector3(0, 0.002f, 0);
                _up = false;
            }
            RotationMatrix = Matrix.CreateRotationY(_angle) * Matrix.CreateTranslation(Position);
        }
    }
}
