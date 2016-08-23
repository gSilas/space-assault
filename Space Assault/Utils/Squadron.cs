using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class Squadron
    {
        public List<AEnemys> _ships;
        public Vector3 _position;
        public SquadronFormation _formType;
        public EnemyType _type;
        public bool SquadronIntelligence;

        public enum SquadronFormation
        {
            Arrow,
            Circle,
            Line,
            Snake
        }

        public enum EnemyType
        {
            Fighter,
            Bomber
        }

        public Squadron(Vector3 position, SquadronFormation formType, uint numShips, EnemyType type)
        {
            SquadronIntelligence = true;
            _position = position;
            _ships = new List<AEnemys>();
            _formType = formType;
            _type = type;
            switch (formType)
            {
                case SquadronFormation.Arrow:
                    createArrow(numShips);
                    break;
                case SquadronFormation.Circle:
                    createCircle(numShips);
                    break;
                case SquadronFormation.Line:
                    createLine(numShips);
                    break;
                case SquadronFormation.Snake:
                    createSnake(numShips);
                    break;
                default:
                    createArrow(numShips);
                    break;
            }
        }

        private void createArrow(uint numShips)
        {

            _ships.Add(new EnemyFighter(_position));
            uint k = 1;
            uint parallels = numShips / 2;

            for (int i = 1; i <= parallels; i++)
            {
                _ships.Add(new EnemyFighter(_ships[0].Position + _ships[0].RotationMatrix.Forward * 20 * i + _ships[0].RotationMatrix.Left * 15 * i));
                k++;
                if (k < numShips)
                {
                    _ships.Add(new EnemyFighter(_ships[0].Position + _ships[0].RotationMatrix.Forward * 20 * i + _ships[0].RotationMatrix.Left * -15 * i));
                    k++;
                }
            }

            foreach (var ship in _ships)
                ship.LoadContent();
        }

        private void createSnake(uint numShips)
        {
            _ships.Add(new EnemyFighter(_position));
            for (int i = 0; i < numShips; i++)
            {
                _ships.Add(new EnemyFighter(_position + new Vector3(0, 0, -30 * i)));
            }

            foreach (var ship in _ships)
                ship.LoadContent();
        }

        private void createLine(uint numShips)
        {
            _ships.Add(new EnemyFighter(_position));
            for (int i = 0; i < numShips; i++)
            {
                _ships.Add(new EnemyFighter(_position + new Vector3(-20 * i, 0, 0)));
            }

            foreach (var ship in _ships)
                ship.LoadContent();
        }

        private void createCircle(uint numShips)
        {
            throw new NotImplementedException();
        }

        public void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {

            switch (_type)
            {
                case EnemyType.Fighter:
                    IntelligenceFighter(gameTime, targetPosition, ref bulletList);
                    break;
                case EnemyType.Bomber:
                    IntelligenceBomber(gameTime, targetPosition, ref bulletList);
                    break;
                default:
                    IntelligenceFighter(gameTime, targetPosition, ref bulletList);
                    break;
            }

        }

        private void IntelligenceFighter(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            double distanceToTarget = (_ships[0].Position - targetPosition).Length();

            if (_formType == SquadronFormation.Snake)
            {
                _ships[0].Intelligence(gameTime, targetPosition, ref bulletList);
                for (int i = 1; i < _ships.Count; i++)
                {
                    _ships[i].FlyToPoint(_ships[i - 1].Position - _ships[i - 1].RotationMatrix.Forward * -20);
                }
            }
            else if (_formType == SquadronFormation.Arrow)
            {
                
                _ships[0].FlyToPoint(targetPosition);

                int k = 1;
                int parallels = _ships.Count / 2;

                for (int i = 1; i <= parallels; i++)
                {
                    _ships[k].FlyToPoint(_ships[0].Position + _ships[0].RotationMatrix.Forward * 20 * i + _ships[0].RotationMatrix.Left * 15 * i);
                    k++;
                    if (k < _ships.Count)
                    {
                        _ships[k].FlyToPoint(_ships[0].Position + _ships[0].RotationMatrix.Forward * 20 * i + _ships[0].RotationMatrix.Left * -15 * i);
                        k++;
                    }
                }

                //ship.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, ship.gunMakeDmg, ship.Position, ship.RotationMatrix, ref bulletList); 

            }

        }


        private void IntelligenceBomber(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {

        }
    }
}
