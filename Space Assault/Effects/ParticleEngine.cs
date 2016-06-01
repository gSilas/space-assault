using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Effects
{
    /// <summary>
    /// The particle engine class that handles all the particles
    /// </summary>
    public class ParticleEngine
    {
        //A random number generator to add realism
        private Random random;
        //The emitter of the particles
        public Vector2 EmitterLocation { get; set; }
        //The pools of particles handled by the system
        private List<Particle> particles;
        //The texture used for the particles
        private Texture2D texture;

        public ParticleEngine(Texture2D texture, Vector2 location)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle>();
            random = new Random();
        }

        //Method that handles the generation of particles and defines 
        //its behaviour
        private Particle GenerateNewParticle()
        {
            //Create the particles at the emitter and  
            //move it around randomly just a bit
            Vector2 position = EmitterLocation + new Vector2(
                    (float)(random.NextDouble() * 10 - 10),
                    (float)(random.NextDouble() * 10 - 10));
            //Just a random direction. Effectively, they
            //go in every direction
            Vector2 direction = new Vector2(
                (float)(random.NextDouble() * 2 - 1),
                (float)(random.NextDouble() * 2 - 1));
            // A random life time constrained to a minimum and maximum value
            int lifeTime = 600 + random.Next(400);

            // The particles are rotated so that they seem to come
            // from a point in the infinite. No need to rotate them over time
            float rotation = -1 * (float)Math.Atan2(direction.X, direction.Y);
            float rotationRate = 0;
            // A color in the blueish tones
            Color color = new Color(0f, (float)random.NextDouble(), 1f);
            // An initial random size and decreasing over time
            float size = (float)random.NextDouble() / 4;
            float sizeRate = -0.01f;
            // A 0 fade value to start (so they don't pop in)
            // and an increase over time
            float fadeValue = 0f;
            float fadeRate = 0.001f * (float)random.NextDouble();

            return new Particle(texture, position, direction,
                rotation, rotationRate, color, fadeValue,
                fadeRate, size, sizeRate, lifeTime);
        }

        public void Update()
        {
            //Number of particles added to the system every update
            int total = 20;
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            //We remove the particles that reach their life time
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].lifeTime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
