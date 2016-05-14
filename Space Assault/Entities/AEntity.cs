
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities
{
    public abstract  class AEntity
    {
        private Model _model;
        private Vector3 _position;

        public abstract void Update();

        public void Draw(Camera camera)
        {
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.CreateWorld(_position, Vector3.UnitX, Vector3.UnitY);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
    }
}
