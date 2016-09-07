using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils.Particle
{
    public class Trail
    {
        const float trailParticlesPerSecond = 150;

        ParticleEmitter trailEmitter;
        Vector3 position = Vector3.Zero;


        // Constructs a new Trail
        public Trail(ParticleSystem TrailParticles)
        {
            // Use the particle emitter helper to output our trail particles.
            trailEmitter = new ParticleEmitter(TrailParticles,
                                               trailParticlesPerSecond, position);
        }


        // Updates the Trail
        public void Update(GameTime gameTime, Vector3 position)
        {
            // Update the particle emitter, which will create our particle trail.
            trailEmitter.Update(gameTime, position);
        }
    }
}
