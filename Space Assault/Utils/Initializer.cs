

using System.Collections.Generic;

namespace Space_Assault.Utils
{
    public static class Initializer
    {
        /*
         *TODO add your initalize code here for eacht gamestate  
         */
        private static List<Controller.EGameStates> _loadlist = new List<Controller.EGameStates>(); 

        public static void InitalizeGameState(Controller.EGameStates state)
        {
            if (!_loadlist.Contains(state))
            {
                _loadlist.Add(state);
                switch (state)
                {
                    case Controller.EGameStates.EndlessModeScene:
                        
                        break;
                    case Controller.EGameStates.MainMenu:

                        break;
                    case Controller.EGameStates.OptionsMenu:

                        break;
                    case Controller.EGameStates.PauseMenu:

                        break;
                    case Controller.EGameStates.TutorialScene:

                        break;
                }
            }
        }
    }
}
