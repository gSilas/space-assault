using Microsoft.Xna.Framework;

namespace Space_Assault.States
{
    public interface IDrawableState
    {
        /*
         * This interface implemnts a Draw function into a GameState
         * TODO add pause halt etc
         */
        void Draw(GameTime elapsedTime);
    }
}
