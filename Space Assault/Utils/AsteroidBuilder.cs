using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class AsteroidBuilder
    {
        private Model _model;
        public List<Asteroid> _asteroidList;
        private List<Asteroid> _asteroidsToAdd;
        private TimeSpan _lastChunkTime;
        private Random _rand;

        public AsteroidBuilder()
        {
            _asteroidList = new List<Asteroid>();
            _asteroidsToAdd = new List<Asteroid>();
            _rand = new Random();
        }

        public void LoadContent()
        {
           _model = Global.ContentManager.Load<Model>("Models/asteroid2");
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        { 
            foreach (var ast in _asteroidList)
            {
                ast.Update(gameTime);
            }
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromSeconds(Global.AsteroidSpawnTime))))
            {
                _lastChunkTime = gameTime.TotalGameTime;
                Chunk(targetPosition);
            }
        }

        private void Chunk(Vector3 targetPosition)
        {

            int movespeed;
            /*
            int zdist;
            int xoffset;
            int yoffset;
            for (int i = 0; i < 15; i++)
            {
                zdist = _rand.Next(-200,200);
                xoffset = _rand.Next(-35, 35);
                movespeed = _rand.Next(30, 100);

                Vector3 position = new Vector3();
                position.X = targetPosition.X + 550 + xoffset;
                position.Z = targetPosition.Z + zdist;
                position.Y = 0;

                Vector3 direction = new Vector3(targetPosition.X - 300 + xoffset, 0, targetPosition.Z + zdist) - position;
                direction.Y = 0;
                direction.Normalize();

                int angle = _rand.Next(-360, 360);
                Asteroid ast = new Asteroid(_model, position, angle, direction, (float)movespeed/100, _lastChunkTime);
                ast.LoadContent();
                _asteroidsToAdd.Add(ast);
            }
            */
            double angle;
            int size = _rand.Next(0, 11);
            int noise;
            int astAngle;
            for (int i = 0; i < size; i++)
            {
                movespeed = _rand.Next(10, 100);
                astAngle = _rand.Next(-360, 360);
                noise = _rand.Next(0, 90);
                angle = _rand.NextDouble() * Math.PI * 2;
                Vector3 position = new Vector3(Global.MapSpawnRadius * (float)Math.Cos(angle), 0, Global.MapSpawnRadius * (float)Math.Sin(angle));
                Vector3 direction = new Vector3(Global.MapSpawnRadius * (float)Math.Cos(angle + 180d), 0, Global.MapSpawnRadius * (float)Math.Sin(angle + 180d)) - position;
                direction.Normalize();
                Asteroid ast = new Asteroid(_model, position, astAngle, direction, (float)movespeed / 100);
                ast.LoadContent();
                _asteroidsToAdd.Add(ast);
            }
            _asteroidList.AddRange(_asteroidsToAdd);
            _asteroidsToAdd.Clear();
        }

        public void Draw()
        {
            foreach (var ast in _asteroidList)
            {
                ast.Draw();
            }
        }
    }
}
