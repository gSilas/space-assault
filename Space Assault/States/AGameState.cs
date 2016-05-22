
namespace Space_Assault.States
{
    public abstract class AGameState
    {
        public abstract void Update();
        public abstract void Draw();
        public virtual void Initialize(){}
        public virtual void LoadContent(){}
    }
}
