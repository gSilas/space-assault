using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using System.Collections.Generic;

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

        public ParticleSystem TrailParticles;
        public List<Trail> trail;

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
            if (Collider3D.BoundingFrustumIntersection(this))
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        _world = effect.World = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(_scale);
                        effect.View = Global.Camera.ViewMatrix;
                        effect.Projection = Global.Camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
