using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Space_Assault.Entities
{
    class Drone : AEntity
    {

        public Drone(Vector3 position, float angle)
        {
            Position = position;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            ///here I would load a model if I had one >_>
            //Model = cm.Load<Model>("Models/drone");
        }

        public override void Update(GameTime gameTime)
        {
            //mousehandler stuff
        }
    }
}
