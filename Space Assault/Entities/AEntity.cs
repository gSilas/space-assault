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

                        effect.DirectionalLight0.DiffuseColor = Color.LightGoldenrodYellow.ToVector3(); // a red light
                        effect.DirectionalLight0.Direction = new Vector3(-1, -1, 0);  // coming along the x-axis
                        effect.DirectionalLight0.SpecularColor = Color.DarkGoldenrod.ToVector3(); // with green highlights

                        effect.PreferPerPixelLighting = true;
                        _world = effect.World = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(_scale);
                        effect.View = Global.Camera.ViewMatrix;
                        effect.Projection = Global.Camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        public virtual void Draw(Effect effectShader)
        {
            if (Collider3D.BoundingFrustumIntersection(this))
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effectShader;
                        _world = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(_scale);
                        part.Effect.Parameters["World"].SetValue(_world);
                        part.Effect.Parameters["View"].SetValue(Global.Camera.ViewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(Global.Camera.ProjectionMatrix);
                        part.Effect.Parameters["AmbientColor"].SetValue(Color.Pink.ToVector4());
                        part.Effect.Parameters["AmbientIntensity"].SetValue(1f);
                    }
                    mesh.Draw();
                    
                }
            }
        }

    }
}
