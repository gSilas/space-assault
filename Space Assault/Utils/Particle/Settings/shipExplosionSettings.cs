using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    /// <summary>
    /// Custom particle system for creating the fiery part of the explosions.
    /// </summary>
    class ShipExplosionSettings : ParticleSystem
    {
        public ShipExplosionSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "explosion";

            settings.MaxParticles = 100;

            settings.Duration = TimeSpan.FromSeconds(0.4);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 20;
            settings.MaxHorizontalVelocity = 30;

            settings.MinVerticalVelocity = -20;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0;

            settings.MinColor = new Color(155, 255, 155, 80);
            settings.MaxColor = new Color(205, 255, 200, 100);

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
