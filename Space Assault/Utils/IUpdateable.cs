using Microsoft.Xna.Framework;

namespace Space_Assault.Utils
{
    public interface IUpdateableState
    {
        void Update(GameTime elapsedTime);
    }
}
