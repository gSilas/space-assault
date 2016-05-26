using System;
using Microsoft.Xna.Framework;

namespace Space_Assault.States
{
    public interface IUpdateableState : IEquatable<IUpdateable>
    {
        /*
        * This interface implements an Update function into a GameState
        * TODO add pause halt etc
        */
        void Update(GameTime elapsedTime);
    }
}
