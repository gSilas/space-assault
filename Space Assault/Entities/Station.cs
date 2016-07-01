using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Entities
{
    class Station : AEntity
    {
        private float _angle;
        private bool _up;
        public int _health;
        private bool _isNotDead;

        public Station(Vector3 position, float angle)
        {
            _angle = angle;
            Position = position;
            Scale = 0.5f;
        }

        public override void Initialize()
        {
            _up = true;
            _health = 10000;
        }
        public bool IsNotDead
        {
            get { return _isNotDead; }
            protected set { _isNotDead = value; }
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/station");
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.005f;
            if (Position.Y < 1 && _up)
                Position += new Vector3(0, 0.002f, 0);
            else if (Position.Y < 0)
                _up = true;
            else
            {
                Position -= new Vector3(0, 0.002f, 0);
                _up = false;
            }
            RotationMatrix = Matrix.CreateRotationY(_angle);

            if (_health <= 0) IsNotDead = false;
        }
    }
}
