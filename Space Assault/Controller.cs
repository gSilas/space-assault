using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        //public IGameState _currentGameState;
        public GraphicsDeviceManager gm;
        public ContentManager cm;
        private Camera _camera;

        public Stack<IGameState> _currentGameStates;
        public List<IDrawableState> _activeDrawable;
        public List<IUpdateableState> _activeUpdateable; 

        //Constructor creates instance of mainmenu for the default gamestate
        public Controller(GraphicsDeviceManager graphics, ContentManager content)
        {
            gm = graphics;
            cm = content;
            _currentGameStates = new Stack<IGameState>();
            _activeDrawable = new List<IDrawableState>();
            _activeUpdateable = new List<IUpdateableState>();
            Switch(EGameStates.MainMenu);

        }

		//switches between GameStates
        protected void Switch(EGameStates nextState)
        {
            switch (nextState)
            {
                case EGameStates.MainMenu:
                    MainMenu mainMenu = new MainMenu(this);
                    mainMenu.Initialize();
                    mainMenu.LoadContent();
                    Push(mainMenu);
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
                    HighScoreList highScore = new HighScoreList();
                    highScore.Initialize();
                    Push(highScore);
                    break;
            }

        }

        protected void Pop()
        {
            IGameState state = _currentGameStates.Pop();
            if (state is IUpdateableState)
            {
                IUpdateableState updateable = state as IUpdateableState;
                _activeUpdateable.Remove(updateable);
            }
            if (state is IDrawableState)
            {
                IDrawableState drawable = state as IDrawableState;
                _activeDrawable.Remove(drawable);
            }
        }


        protected void Push(IGameState gameState)
        {
            _currentGameStates.Push(gameState);

            if (gameState is IUpdateableState)
            {
                IUpdateableState updateable = gameState as IUpdateableState;
                _activeUpdateable.Add(updateable);
            }
            if (gameState is IDrawableState)
            {
                IDrawableState drawable= gameState as IDrawableState;
                _activeDrawable.Add(drawable);
            }
        }

        public void Update(GameTime elapsedTime)
        {
            foreach (var updateable in _activeUpdateable)
            {
                updateable.Update(elapsedTime);
            }
        }

        public void Draw(GameTime elapsedTime)
        {
            foreach (var drawable in _activeDrawable)
            {
                drawable.Draw(elapsedTime);
            }
        }

    }
}
