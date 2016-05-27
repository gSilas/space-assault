
using System;

namespace Space_Assault.States
{
    public interface IGameState : IEquatable<IGameState>
    {
        bool IsStopped { get; set; }
        void Initialize();
        void LoadContent();
        void Kill();
        void Resume();
    }
}