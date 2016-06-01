using System;
using System.Collections.Generic;
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
            MenuBackground,
            MainMenu,
            EndlessModeScene,
            TutorialScene,
            PauseMenu,
            OptionsMenu,
            HighScore
        }
        //Current GameStates,Drawables,Updateables has to be initialized
        //public IGameState _currentGameState;

        private bool _updateClear;
        private bool _drawClear;

        private List<IGameState> _currentGameStates;
        private List<IDrawableState> _activeDrawable;
        private List<IUpdateableState> _activeUpdateable;
        private List<IDrawableState> _inactiveDrawable;
        private List<IUpdateableState> _inactiveUpdateable;
        private List<IDrawableState> _removeDrawable;
        private List<IUpdateableState> _removeUpdateable;

        //Constructor creates instance of mainmenu for the default gamestate
        public Controller()
        {
            _currentGameStates = new List<IGameState>();
            _activeDrawable = new List<IDrawableState>();
            _activeUpdateable = new List<IUpdateableState>();
            _inactiveDrawable = new List<IDrawableState>();
            _inactiveUpdateable = new List<IUpdateableState>();
            _removeDrawable = new List<IDrawableState>();
            _removeUpdateable = new List<IUpdateableState>();
            _updateClear = false;
            _drawClear = false;
            Push(EGameStates.MenuBackground);
            Push(EGameStates.MainMenu);

        }

        //Removes an Gamestate from Draw and Update vectors
        public void Pop(EGameStates gameState)
        {
            IGameState state = Switch(gameState);

            foreach (var states in _currentGameStates)
            {
                if (state.Equals(states))
                {
                    state = states;
                    break;
                }
            }

            Console.WriteLine("Pop!\n");
            state.Kill();
            if (state is IUpdateableState)
            {
                IUpdateableState updateable = state as IUpdateableState;
                _removeUpdateable.Add(updateable);
                _updateClear = true;
            }
            if (state is IDrawableState)
            {
                IDrawableState drawable = state as IDrawableState;
                _removeDrawable.Add(drawable);
                _drawClear = true;
            }
        }

        public IGameState Switch(EGameStates gameState)
        {
            IGameState state = new MainMenu();
            switch (gameState)
            {
                case EGameStates.MainMenu:
                    state = new MainMenu();
                    break;
                case EGameStates.EndlessModeScene:
                    state = new EndlessMode();
                    break;
                case EGameStates.TutorialScene:
                    break;
                case EGameStates.PauseMenu:
                    break;
                case EGameStates.OptionsMenu:
                    break;
                case EGameStates.HighScore:
                    state = new HighScore();
                    break;
                case EGameStates.MenuBackground:
                    state = new MenuBackground();
                    break;
                default:
                    state = new MainMenu();
                    break;
            }
            return state;
        }

        //Adds a Gamestate to the list of initialized Gamestates and adds it to update and draw vectors
        public void Push(EGameStates gameState)
        {
            IGameState state = Switch(gameState);

            if (!_currentGameStates.Contains(state))
            {
                Console.WriteLine("Creating new gameState!\n");
                state.Initialize();
                state.LoadContent();
                _currentGameStates.Add(state);

                if (state is IUpdateableState)
                {
                    IUpdateableState updateable = state as IUpdateableState;
                    _inactiveUpdateable.Add(updateable);
                    _drawClear = true;
                    Console.WriteLine("Added Updateable!\n");
                }
                if (state is IDrawableState)
                {
                    IDrawableState drawable = state as IDrawableState;
                    _inactiveDrawable.Add(drawable);
                    _updateClear = true;
                    Console.WriteLine("Added Drawable!\n");
                }
            }
            else
            {
                Console.WriteLine("State already existed! Added state to Update and Draw List!\n");
                state.Resume();
                if (state is IDrawableState)
                {
                    IDrawableState drawable = state as IDrawableState;
                    if (_activeDrawable.Contains(drawable))
                    {
                        return;
                    }
                    else
                    {
                        IGameState dstate = _currentGameStates.Find((state.Equals));
                        IDrawableState ddrawable = dstate as IDrawableState;
                        _inactiveDrawable.Add(ddrawable);
                        _drawClear = true;
                    }
                }
                if (state is IUpdateableState)
                {
                    IUpdateableState updateable = state as IUpdateableState;
                    if (_activeUpdateable.Contains(updateable))
                    {
                        return;
                    }
                    else
                    {
                        IGameState ustate = _currentGameStates.Find((state.Equals));
                        IUpdateableState uupdateable = ustate as IUpdateableState;
                        _inactiveUpdateable.Add(uupdateable);
                        _updateClear = true;
                    }
                }
            }
        }

        public void Update(GameTime elapsedTime)
        {
            if (_activeUpdateable.Count == 0 && (elapsedTime.ElapsedGameTime.Seconds % 30) == 0)
                Console.WriteLine("Nothing to update! \n");

            foreach (var updateable in _activeUpdateable)
            {
                updateable.Update(elapsedTime);
                //Console.WriteLine("Update! \n");
            }
            if (_updateClear)
            {
                foreach (var rem in _removeUpdateable)
                {
                    _activeUpdateable.Remove(rem);
                }
                _activeUpdateable.AddRange(_inactiveUpdateable);
                _inactiveUpdateable.Clear();
                _removeUpdateable.Clear();
                _updateClear = false;
                Console.WriteLine("Updateclear done! \n");
            }
        }

        public void Draw(GameTime elapsedTime)
        {
            if (_activeDrawable.Count == 0 && (elapsedTime.ElapsedGameTime.Seconds % 30) == 0)
                Console.WriteLine("Nothing to draw! \n");

            foreach (var drawable in _activeDrawable)
            {
                drawable.Draw(elapsedTime);

            }
            if (_drawClear)
            {

                foreach (var rem in _removeDrawable)
                {
                    _activeDrawable.Remove(rem);
                }
                _activeDrawable.AddRange(_inactiveDrawable);
                _inactiveDrawable.Clear();
                _removeDrawable.Clear();
                _drawClear = false;
                Console.WriteLine("Drawclear done! \n");
            }
        }

    }
}
