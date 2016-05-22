
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Space_Assault.States
{
    class MainMenu : AGameState
    {
        //#################################
        // Set Variables
        //#################################

        // General
        private Controller sc;
        private GraphicsDeviceManager gm;
        private ContentManager cm;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private bool up;
        private float angle;
        private Vector3 position;
        private Model model;
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 45, 60), new Vector3(-30, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private SpriteBatch spriteBatch;
        Texture2D background;

        //#################################
        // Constructor
        //#################################
        public MainMenu(Controller controller)
        {
            sc = controller;
            gm = sc.gm;
            cm = sc.cm;
            soundEffects = new List<SoundEffect>();
        }

        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            
            // Sound
            soundEffects.Add(cm.Load<SoundEffect>("stationSound"));
            
            // Play that can be manipulated after the fact
            var instance = soundEffects[0].CreateInstance();
            instance.IsLooped = true;
            instance.Play();

            // 3D Model
            up = true;
            angle = 0;
            position = new Vector3(0, 0, 0);
            model = cm.Load<Model>("station");
            
        }

        //#################################
        // Draw - Function
        //#################################
        public override void Draw()
        {
            DrawModel(model, world, view, projection);
        }

        //#################################
        // Update - Function
        //#################################
        public override void Update()
        {
            
            //3D Model
            angle += 0.005f;
            if (position.Y < 1 && up)
                position += new Vector3(0, 0.002f, 0);
            else if (position.Y < 0)
                up = true;
            else
            {
                position -= new Vector3(0, 0.002f, 0);
                up = false;
            }
            world = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(position);
            
        }

        //#################################
        // DrawModel - Function
        //#################################
        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
