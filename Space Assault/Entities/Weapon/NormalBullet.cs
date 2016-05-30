using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    class NormalBullet : AAmmunition
    {
        public NormalBullet(Vector3 position, Vector3 direction, float travelspeed) : base(position, direction, travelspeed) {}

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/bullet");
        }
    }
}
