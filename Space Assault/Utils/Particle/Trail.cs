using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils.Particle
{
    /// <summary>
    /// This class demonstrates how to combine several different particle systems
    /// to build up a more sophisticated composite effect. It implements a rocket
    /// projectile, which arcs up into the sky using a ParticleEmitter to leave a
    /// steady stream of trail particles behind it. After a while it explodes,
    /// creating a sudden burst of explosion and smoke particles.
    /// </summary>
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
