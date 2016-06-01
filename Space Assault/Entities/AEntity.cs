using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    public abstract  class AEntity
    {
        private Model _model;
        private Vector3 _position;
        private Matrix _rotationMatrix = Matrix.Identity;

        public Model Model
        {
            get { return _model;}
            protected set { _model = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            protected set { _position = value; }
        }

        public Matrix RotationMatrix
        {
            get { return _rotationMatrix; }
            protected set { _rotationMatrix = value; }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

        public abstract void Initialize();

        public virtual void Draw()
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = RotationMatrix*Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
                    effect.View = Global.Camera.ViewMatrix;
                    effect.Projection = Global.Camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
