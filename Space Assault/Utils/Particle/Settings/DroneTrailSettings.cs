
using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils.Particle.Settings
{
    /// <summary>
    /// Custom particle system for leaving smoke trails behind the rocket projectiles.
    /// </summary>
    class DroneTrailSettings : ParticleSystem
    {
        public DroneTrailSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 500;

            settings.Duration = TimeSpan.FromSeconds(0.5);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = new Color(155, 255, 155, 70);
            settings.MaxColor = new Color(205, 255, 200, 100);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 40;
            settings.MaxStartSize = 42;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;
        }
    }
}
