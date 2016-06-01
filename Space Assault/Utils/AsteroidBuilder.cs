using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Entities;

namespace Space_Assault.Utils
{
    class AsteroidBuilder
    {
        private Vector3 _position;
        private List<Asteroid> _asteroids;
        private TimeSpan _lastChunkTime;

        public AsteroidBuilder(Vector3 position)
        {
            _position = position;
            _asteroids = new List<Asteroid>();
            LoadContent();
            BuildChunks();
        }

        private void BuildChunks()
        {
            _asteroids.AddRange(AsteroidChunk.RandomChunks());
        }

        private void LoadContent()
        {
            AsteroidChunk.LoadContent(Global.ContentManager.Load<Model>("Models/asteroid"),_position);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var ast in _asteroids)
            {
                ast.Update(gameTime);
            }
            if(gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromSeconds(1))))
            {
                BuildChunks();
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }

        public void Draw()
        {
            foreach (var ast in _asteroids)
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
            int seed = random.Next(Enum.GetValues(typeof(PatternType)).Cast<int>().Min(),Enum.GetValues(typeof(PatternType)).Cast<int>().Max());
            return ChunkAsteroids((PatternType)seed);
        }

        public static List<Asteroid> ChunkAsteroids(PatternType t)
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
                    astPos.Z = float.Parse(coords[2]);
                    Asteroid ast = new Asteroid(astPos+_position, float.Parse(coords[3]), new Vector3(-1, 0, 1),float.Parse(coords[4]));
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
