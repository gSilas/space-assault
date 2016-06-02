using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.UI;
using System.Collections.Generic;


namespace Space_Assault.States
{
    class MainMenu : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        // General
        List<Button> Buttons = new List<Button>();

        //#################################
        // Constructor
        //#################################
        public MainMenu()
        {
            IsStopped = false;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            // Add Buttons
            int button_x = 150;
            int button_y = 150;
            int margin = 50;

            Buttons.Add(new Button("play",Global.ContentManager.Load<Texture2D>("UI/play"), new Vector2(button_x, button_y + Buttons.Count * margin)));
            Buttons.Add(new Button("tutorial", Global.ContentManager.Load<Texture2D>("UI/tutorial"), new Vector2(button_x, button_y + Buttons.Count * margin)));
            Buttons.Add(new Button("highscore", Global.ContentManager.Load<Texture2D>("UI/highscore"), new Vector2(button_x, button_y + Buttons.Count * margin)));
            Buttons.Add(new Button("credits", Global.ContentManager.Load<Texture2D>("UI/credits"), new Vector2(button_x, button_y + Buttons.Count * margin)));
            Buttons.Add(new Button("end",Global.ContentManager.Load<Texture2D>("UI/end"), new Vector2(button_x, button_y + Buttons.Count * margin)));

            /* Alternative nonTexture Button
            Buttons.Add(new Button("Arial", "StartGame", init_x, init_y));
            */
        }

        public void Kill()
        {
            IsStopped = true;
            foreach (var button in Buttons)
            {
                button.Pressed = false;
            }
        }

        public void Resume()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }
        //#################################
        // Draw - Function
        //#################################
        public void Draw(GameTime elapsedTime)
        {
            foreach (var button in Buttons)
            {
                button.Draw();
            }
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            foreach (var button in Buttons)
            {
                button.Update();

                if (button.Pressed)
                { 
                    switch (button.name)
                    {
                        case ("play"):
                            Global.Controller.Push(Controller.EGameStates.EndlessModeScene);
                            Global.Controller.Pop(Controller.EGameStates.MenuBackground);
                            Global.Controller.Pop(Controller.EGameStates.MainMenu);
                            break;
                        case ("tutorial"):
                            break;
                        case ("highscore"):
                            Global.Controller.Push(Controller.EGameStates.HighScore);
                            Global.Controller.Push(Controller.EGameStates.HighScoreEnter);
                            Global.Controller.Pop(Controller.EGameStates.MainMenu);
                            break;
                        case ("credits"):
                            break;
                        case ("end"):
                            Global.SpaceAssault.Exit();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
        }

        public bool Equals(IGameState other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IUpdateable other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IDrawableState other)
        {
            return other.GetType() == this.GetType();
        }


    }
}
