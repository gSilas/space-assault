using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;

namespace SpaceAssault.Entities
{
    public abstract class AEntity
    {
        private Model _model;
        private Vector3 _position;
        private Matrix _rotationMatrix = Matrix.Identity;
        private Matrix _world;
        private float _scale = 1;
        private BoundingSphere[] _spheres;

        public Trail _trail;

        public Model Model
        {
            get { return _model; }
            protected set { _model = value; }
        }

        public BoundingSphere[] Spheres
        {
            get { return _spheres; }
            protected set { _spheres = value; }
        }

        public float Scale
        {
            get { return _scale; }
            protected set { _scale = value; }
        }

        public Matrix World
        {
            get { return _world; }
            protected set { _world = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Matrix RotationMatrix
        {
            get { return _rotationMatrix; }
            protected set { _rotationMatrix = value; }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

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

                        effect.DirectionalLight1.Enabled = true;
                        effect.DirectionalLight1.DiffuseColor = Global.PlanetColor.ToVector3(); // a light
                        effect.DirectionalLight1.Direction = Vector3.Normalize(Position - new Vector3(-1000f, -2000f, -1000f));  // coming along the x-axis
                        effect.DirectionalLight1.SpecularColor = Global.PlanetColor.ToVector3(); // with highlights

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
        public virtual void Draw(Color entityColor, float alphaValue)
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
                        effect.Alpha = alphaValue;

                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.DiffuseColor = Color.LightGoldenrodYellow.ToVector3(); // a light
                        effect.DirectionalLight0.Direction = Vector3.Normalize(Position - new Vector3(0, 1000, 0));  // coming along the x-axis
                        effect.DirectionalLight0.SpecularColor = Color.DarkGoldenrod.ToVector3(); // with highlights

                        effect.DirectionalLight1.Enabled = true;
                        effect.DirectionalLight1.DiffuseColor = Color.Red.ToVector3(); // a light
                        effect.DirectionalLight1.Direction = Vector3.Normalize(Position - new Vector3(-1000f, -2000f, -1000f));  // coming along the x-axis
                        effect.DirectionalLight1.SpecularColor = Color.DarkRed.ToVector3(); // with highlights

                        effect.PreferPerPixelLighting = true;
                        RotationMatrix = Matrix.CreateWorld(Position, RotationMatrix.Forward, Vector3.Up);
                        effect.World = RotationMatrix * Matrix.CreateScale(_scale);
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
