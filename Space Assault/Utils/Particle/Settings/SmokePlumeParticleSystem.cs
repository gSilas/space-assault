using Microsoft.Xna.Framework;
using System;

namespace SpaceAssault.Utils.Particle.Settings
{
    class SmokePlumeParticleSystem : ParticleSystem
    {
        public SmokePlumeParticleSystem()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "smoke";

            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(10);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 15;

            settings.MinVerticalVelocity = 10;
            settings.MaxVerticalVelocity = 20;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = new Vector3(-20, -5, 0);

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 4;
            settings.MaxStartSize = 7;

            settings.MinEndSize = 35;
            settings.MaxEndSize = 140;
        }
    }
}
