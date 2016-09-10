using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Utils
{
    class AsteroidBuilder
    {
        private Model _model;
        public List<Asteroid> _asteroidList;
        private Random _rand;
        ParticleSystem shinyParticles;
        private int _asteroidNumber;

        public AsteroidBuilder(int number)
        {
            _asteroidList = new List<Asteroid>();
            _rand = new Random();
            shinyParticles = new AsteroidTrailSettings();
            _asteroidNumber = number;
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
                if (ast.IsShiny)
                {
                    shinyParticles.AddParticle(ast.Position,-ast.Direction*ast.Speed);
                }
            }
            if (_asteroidList.Count < _asteroidNumber)
            {
                Console.WriteLine("here come dat boi");
                    int movespeed;
                    double angle;
                    int size = _rand.Next(0, 11);
                    int noise;
                    int astAngle;
                    int shinyness;
                    while (_asteroidList.Count < _asteroidNumber)
                    {
                        movespeed = _rand.Next(20, 100);
                        astAngle = _rand.Next(-360, 360);
                        noise = _rand.Next(0, 90);
                        shinyness = _rand.Next(0, 21);
                        angle = _rand.NextDouble() * Math.PI * 2;
                        Vector3 position = new Vector3(Global.MapSpawnRadius * (float)Math.Cos(angle), 0, Global.MapSpawnRadius * (float)Math.Sin(angle));
                        Vector3 direction = new Vector3(Global.MapSpawnRadius * (float)Math.Cos(angle + 180d), 0, Global.MapSpawnRadius * (float)Math.Sin(angle + 180d)) - position;
                        direction.Normalize();
                        Asteroid ast;
                        if (shinyness == 10)
                        {
                            ast = new Asteroid(_model, position, astAngle, direction, (float)movespeed / 100, true);
                        }
                        else
                        {
                            ast = new Asteroid(_model, position, astAngle, direction, (float)movespeed / 100, false);
                        }
                        ast.LoadContent();
                       _asteroidList.Add(ast);
                    }
            }
            shinyParticles.Update(gameTime);
        }
        public void Draw()
        {
            foreach (var ast in _asteroidList)
            {
                shinyParticles.Draw();
                ast.Draw(Global.AsteroidColor);
            }
        }
    }
}
