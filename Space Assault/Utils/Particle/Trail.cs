using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils.Particle
{
    public class Trail
    {
        const float trailParticlesPerSecond = 150;

        ParticleEmitter trailEmitter;
        Vector3 position = Vector3.Zero;
        ParticleSystem _trailSystem;


        // Constructs a new Trail
        public Trail(ParticleSystem TrailSettings)
        {
            // Use the particle emitter helper to output our trail particles.
            trailEmitter = new ParticleEmitter(TrailSettings,
                                               trailParticlesPerSecond, position);
            _trailSystem = TrailSettings;
        }


        // Updates the Trail
        public void Update(GameTime gameTime, Vector3 position)
        {
            // Update the particle emitter, which will create our particle trail.
            trailEmitter.Update(gameTime, position);
            _trailSystem.Update(gameTime);
        }

        // Draw the Trail
        public void Draw()
        {
            _trailSystem.Draw();
        }

    }
}
