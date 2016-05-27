
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _direction;
        private Vector3 _speed;
        private Vector3 rotationXYZ;
        private Matrix rotationMatrix;


        public Drone(Vector3 position, float angle)
        {
            Position = position;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            ///here I would load a model if I had one >_>
            //Model = cm.Load<Model>("Models/drone");
        }

        public override void Update(GameTime gameTime)
        {
            //mousehandler stuff
        }

        public void turn(Vector3 direction)
        {
            rotationXYZ.Normalize();
            rotationMatrix = Matrix.Identity;

            rotationMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(rotationXYZ.X))
                * Matrix.CreateRotationY(MathHelper.ToRadians(rotationXYZ.Y))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationXYZ.Z));
        }

        public void move()
        {

        }
    }
}
