using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    class ShipBigExplosionSettings : ParticleSystem
    {
        public ShipBigExplosionSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "explosion";

            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(2);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 20;
            settings.MaxHorizontalVelocity = 30;

            settings.MinVerticalVelocity = -20;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0;

            settings.MinColor = new Color(255, 255, 255, 100);
            settings.MaxColor = new Color(255, 255, 255, 120);

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 20;
            settings.MaxStartSize = 30;

            settings.MinEndSize = 80;
            settings.MaxEndSize = 100;

            settings.BlendState = BlendState.Additive;
        }
    }
}
