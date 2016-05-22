using System;
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
		//Current GameStates,Drawables,Updateables has to be initialized
        //public IGameState _currentGameState;
        public GraphicsDeviceManager gm;
        public ContentManager cm;
        private Camera _camera;

        public List<IGameState> _currentGameStates;
        public List<IDrawableState> _activeDrawable;
        public List<IUpdateableState> _activeUpdateable; 

        //Constructor creates instance of mainmenu for the default gamestate
        public Controller(GraphicsDeviceManager graphics, ContentManager content)
        {
            gm = graphics;
            cm = content;
            _currentGameStates = new List<IGameState>();
            _activeDrawable = new List<IDrawableState>();
            _activeUpdateable = new List<IUpdateableState>();
            Switch(EGameStates.MainMenu);

        }

		//switches between GameStates
        public void Switch(EGameStates nextState)
        {
            switch (nextState)
            {
                case EGameStates.MainMenu:
                    MainMenu mainMenu = new MainMenu(this);
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
                    Push(highScore);
                    break;
            }

        }

        //Removes an Gamestate from Draw and Update vectors
        public void Pop(IGameState gameState)
        {
            if (gameState is IUpdateableState)
            {
                IUpdateableState updateable = gameState as IUpdateableState;
                _activeUpdateable.Remove(updateable);
            }
            if (gameState is IDrawableState)
            {
                IDrawableState drawable = gameState as IDrawableState;
                _activeDrawable.Remove(drawable);
            }
        }

        //Adds a Gamestate to the list of initialized Gamestates and adds it to update and draw vectors
        public void Push(IGameState gameState)
        {
            if (!_currentGameStates.Contains(gameState))
            {
                gameState.Initialize();
                gameState.LoadContent();
                _currentGameStates.Add(gameState);
            }
                
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
