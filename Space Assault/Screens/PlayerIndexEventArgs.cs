using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault
{
    // Custom event argument which includes the index of the player who
    // triggered the event. This is used by the MenuEntry.Selected event.
    class PlayerIndexEventArgs : EventArgs
    {
        // Constructor.
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        // Gets the index of the player who triggered this event.
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}
