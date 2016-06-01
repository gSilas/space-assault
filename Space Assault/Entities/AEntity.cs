﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    public abstract  class AEntity
    {
        public Model Model;
        public Vector3 Position;
        protected Matrix RotationMatrix;

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

        public abstract void Initialize();

        public void Draw()
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
