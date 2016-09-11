using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    class Planet : AEntity
    {
        private float _angle;
        public Planet(Vector3 position, float angle)
        {
           _angle = angle;
           Position = position;
           Scale = 0.5f;
            //_angle += 0.0025f;
        }

        public override void LoadContent()
        {
           Model = Global.ContentManager.Load<Model>("Models/planet");
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.0025f;

            RotationMatrix = Matrix.CreateRotationY(_angle);
        }
        public virtual void Draw(Color entityColor)
        {
            if (Collider3D.BoundingFrustumIntersection(this))
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.DiffuseColor = entityColor.ToVector3();
                        effect.SpecularColor = entityColor.ToVector3();

                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.DiffuseColor = Color.LightGoldenrodYellow.ToVector3(); // a light
                        effect.DirectionalLight0.Direction = Vector3.Normalize(Position - new Vector3(0, 1000, 0));  // coming along the x-axis
                        effect.DirectionalLight0.SpecularColor = Color.DarkGoldenrod.ToVector3(); // with highlights

                        effect.PreferPerPixelLighting = true;

                        RotationMatrix = Matrix.CreateWorld(Position, RotationMatrix.Forward, Vector3.Up);
                        effect.World = RotationMatrix * Matrix.CreateScale(Scale);
                        effect.View = Global.Camera.ViewMatrix;
                        effect.Projection = Global.Camera.ProjectionMatrix;

                        World = effect.World;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}