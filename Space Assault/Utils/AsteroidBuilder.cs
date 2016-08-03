using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class AsteroidBuilder
    {
        private Model _model;
        public List<Asteroid> Asteroids;
        private List<Asteroid> addList;
        private TimeSpan _lastChunkTime;
        private Random rand;

        public AsteroidBuilder()
        {
            Asteroids = new List<Asteroid>();
            addList = new List<Asteroid>();
            rand = new Random();
        }

        public void LoadContent()
        {
           _model = Global.ContentManager.Load<Model>("Models/asteroid2");
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        { 
            foreach (var ast in Asteroids)
            {
                ast.Update(gameTime);
            }
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromMilliseconds(1000))))
            {
                Chunk(targetPosition);
               _lastChunkTime = gameTime.TotalGameTime;
            }
        }

        private void Chunk(Vector3 targetPosition)
        {
            int zdist;
            int xoffset;
            for (int i = 0; i < 15; i++)
            {
                zdist = rand.Next(-200,200);
                xoffset = rand.Next(-35, 35);

                Vector3 position = new Vector3();
                position.X = targetPosition.X + 300 + xoffset;
                position.Z = targetPosition.Z + zdist;
                position.Y = 0f;

                Vector3 direction = new Vector3(targetPosition.X - 300 + xoffset, 0, targetPosition.Z + zdist) - position;
                direction.Y = 0;
                direction.Normalize();

                int angle = rand.Next(-360, 360);
                Asteroid ast = new Asteroid(position, angle, direction, 0.9f);
                ast.Initialize();
                ast.LoadContent(_model);
                addList.Add(ast);
            }
            Asteroids.AddRange(addList);
            addList.Clear();
        }

        public void Draw()
        {
            foreach (var ast in Asteroids)
            {
                ast.Draw();
            }
        }
    }
}
