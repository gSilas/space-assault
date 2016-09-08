using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    /// <summary>
    /// Custom particle system for creating a flame effect.
    /// </summary>
    class ShipRingExplosionSettings : ParticleSystem
    {
        public ShipRingExplosionSettings()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "explosion";

            settings.MaxParticles = 5000;

            settings.Duration = TimeSpan.FromSeconds(0.2);

            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 20, 0);

            settings.MinColor = new Color(255, 255, 255, 10);
            settings.MaxColor = new Color(255, 255, 255, 40);

            settings.MinStartSize = 30;
            settings.MaxStartSize = 30;

            settings.MinEndSize = 10;
            settings.MaxEndSize = 10;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
