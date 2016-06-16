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
        private Vector3 _position;
        public List<Asteroid> Asteroids;
        private TimeSpan _lastChunkTime;

        public AsteroidBuilder(Vector3 position)
        {
            _position = position;
            Asteroids = new List<Asteroid>();
        }

        private void BuildChunks()
        {
            Asteroids.AddRange(AsteroidChunk.RandomChunks());
        }

        public void LoadContent()
        {
            AsteroidChunk.LoadContent(Global.ContentManager.Load<Model>("Models/asteroid"), _position);
            BuildChunks();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var ast in Asteroids)
            {
                ast.Update(gameTime);
            }
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromSeconds(3))))
            {
                BuildChunks();
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }

        public void Draw()
        {
            foreach (var ast in Asteroids)
            {
                ast.Draw();
            }
        }
    }

    internal static class AsteroidChunk
    {
        private static Model _model;
        private static Vector3 _position;

        public static List<Asteroid> RandomChunks()
        {
            Random random = new Random();
            int seed = random.Next(Enum.GetValues(typeof(PatternType)).Cast<int>().Min(), Enum.GetValues(typeof(PatternType)).Cast<int>().Max() + 1);
            return ChunkAsteroids();
        }

        public static List<Asteroid> CircleChunkAsteroids()
        {
            List<Asteroid> astList = new List<Asteroid>();
            for (double i =0; i < 2*Math.PI; i=i+(1.0d/6.0d)*Math.PI)
            {               
                Vector3 astPos = _position;
                astPos.X = ((float)(astPos.X+ 100 * Math.Cos(i)));
                astPos.Z = ((float)(astPos.Z+ 100 * Math.Sin(i)));
                Asteroid ast = new Asteroid(astPos, MathHelper.ToDegrees(90), new Vector3(-1, 0, 1), 2f);
                ast.Initialize();
                ast.LoadContent(_model);
                astList.Add(ast);
            }
            return astList;
        }

        public static List<Asteroid> HyperbolicChunkAsteroids()
        {
            List<Asteroid> astList = new List<Asteroid>();
            for (double i = 0; i < 2 * Math.PI; i = i + (1.0d / 6.0d) * Math.PI)
            {
                Vector3 astPos = _position;
                astPos.X = ((float)(astPos.X + 55 * Math.Sinh(i)));
                astPos.Z = ((float)(astPos.Z + 55 * Math.Sin(i)));
                Asteroid ast = new Asteroid(astPos, MathHelper.ToDegrees(90), new Vector3(-1, 0, 1), 2f);
                ast.Initialize();
                ast.LoadContent(_model);
                astList.Add(ast);
            }
            return astList;
        }

        public static List<Asteroid> StrangeFieldChunkAsteroids()
        {
            List<Asteroid> astList = new List<Asteroid>();
            for (double i = 0; i < 2 * Math.PI; i = i + (1.0d / 6.0d) * Math.PI)
            {
                Vector3 astPos = _position;
                astPos.X = ((float)(astPos.X + 100 * Math.Tan(i)));
                astPos.Z = ((float)(astPos.Z + 100 * Math.Atan(i)));
                Asteroid ast = new Asteroid(astPos, MathHelper.ToDegrees(90), new Vector3(-1, 0, 1), 2f);
                ast.Initialize();
                ast.LoadContent(_model);
                astList.Add(ast);
            }
            return astList;
        }

        public static List<Asteroid> StringChunkAsteroids(PatternType t)
        {
            switch (t)
            {
                case PatternType.StarPattern:
                    return (MakeStringToChunk(System.IO.File.ReadAllLines(@"AsteroidPatterns\starpattern.txt"), t));
                case PatternType.CirclePattern:
                    return (MakeStringToChunk(System.IO.File.ReadAllLines(@"AsteroidPatterns\circlepattern.txt"), t));
                case PatternType.FieldPattern:
                    return (MakeStringToChunk(System.IO.File.ReadAllLines(@"AsteroidPatterns\fieldpattern.txt"), t));
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }

        public static void LoadContent(Model model, Vector3 position)
        {
            _model = model;
            _position = position;
        }

        private static List<Asteroid> MakeStringToChunk(string[] lines, PatternType t)
        {
            List<Asteroid> astList = new List<Asteroid>();
            foreach (string line in lines)
            {
                string[] coords = line.Split(' ');
                Vector3 astPos = new Vector3();
                astPos.X = float.Parse(coords[0]);
                astPos.Y = float.Parse(coords[1]);
                astPos.Z = float.Parse(coords[2]) * (int)(t + 1);
                Asteroid ast = new Asteroid(astPos + _position, float.Parse(coords[3]), new Vector3(-1, 0, 1), 2f);
                ast.Initialize();
                ast.LoadContent(_model);
                astList.Add(ast);
            }
            return astList;
        }

        public enum PatternType
        {
            StarPattern,
            CirclePattern,
            FieldPattern
        }
    }
}
