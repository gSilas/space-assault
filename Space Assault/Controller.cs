using Space_Assault.States;
using Space_Assault.Utils;

namespace Space_Assault
{
    public class Controller
    {
		// Holds all possible gamestates
        public enum EGameStates
        {
            MainMenu,
            EndlessModeScene,
            TutorialScene,
            PauseMenu,
            OptionsMenu,
            HighScoreList
        }
		//Current GameState has to be initialized
        private AGameState _currentGameState;
        private Camera _camera;

		//Constructor creates instance of mainmenu for the default gamestate
        public Controller()
        {
            _currentGameState = new HighScoreList();
        }

        public void Update()
        {
            _currentGameState.Update();
        }

        public void Draw()
        {
            _currentGameState.Draw();
        }

		//switches between GameStates
        protected void NextGameState(EGameStates nextState)
        {
            switch (nextState)
            {
                case EGameStates.MainMenu:
					_currentGameState = new MainMenu();

                    break;
				case EGameStates.EndlessModeScene:

                    break;
				case EGameStates.TutorialScene:

                    break;
				case EGameStates.PauseMenu:

                    break;
				case EGameStates.OptionsMenu:

                    break;
                case EGameStates.HighScoreList:
                    _currentGameState = new HighScoreList();

                    break;
            }
            _currentGameState.Initialize();
        }

    }
}
