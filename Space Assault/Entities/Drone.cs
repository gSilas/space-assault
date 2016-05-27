
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _direction;
        private float _speed;
        private Vector3 rotationXYZ;
        private Matrix rotationMatrix;


        public Drone(Vector3 position)
        {
            Position = position;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/asteroid");
        }

        public override void Update(GameTime gameTime)
        {
            //mousehandler stuff
        }

        public void turn(Vector3 direction)
        {
            /*
            rotationXYZ.Normalize();
            rotationMatrix = Matrix.Identity;

            rotationMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(rotationXYZ.X))
                * Matrix.CreateRotationY(MathHelper.ToRadians(rotationXYZ.Y))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationXYZ.Z));
            */
        }

        public void turn(Vector3 direction)
        {

        }
    }
}
