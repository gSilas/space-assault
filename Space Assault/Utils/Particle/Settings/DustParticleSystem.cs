using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    /// <summary>
    /// Custom particle system for creating the fiery part of the explosions.
    /// </summary>
    class DustParticleSystem : ParticleSystem
    {
        public DustParticleSystem()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "dust";

            settings.MaxParticles = 50;

            settings.Duration = TimeSpan.FromSeconds(0.4);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 20;
            settings.MaxHorizontalVelocity = 30;

            settings.MinVerticalVelocity = -20;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0;

            settings.MinColor = Color.White;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 40;
            settings.MaxStartSize = 50;

            settings.MinEndSize = 80;
            settings.MaxEndSize = 100;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
