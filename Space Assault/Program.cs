using System;

namespace Space_Assault
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Global.SpaceAssault = new SpaceAssault();
            using (var game = Global.SpaceAssault)
                game.Run();
        }
    }
#endif
}
