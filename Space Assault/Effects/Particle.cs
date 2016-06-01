using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Space_Assault.Effects
{
    public class Particle
    {
        // The position of the particle
        public Vector2 position { get; set; }
        // Where to move the particle
        public Vector2 direction { get; set; }
        // The lifetime of the particle
        public int lifeTime { get; set; }
        // The texture that will be drawn to represent the particle
        public Texture2D texture { get; set; }
        // The rotation of the particle
        public float rotation { get; set; }
        // The rotation rate
        public float rotationRate { get; set; }
        // The color of the particle
        public Color color { get; set; }
        // The fading of the particle
        public float fadeValue { get; set; }
        // The fading rate of the particle
        // they can dissapear/apper smoothly with this
        public float fadeRate { get; set; }
        // The size of the particle   
        public float size { get; set; }
        // The size rate of the particle to make them bigger/smaller
        // over time
        public float sizeRate { get; set; }


        public Particle(Texture2D texture, Vector2 position, Vector2 direction,
            float rotation, float rotationRate, Color color, float fadeValue, float fadeRate,
            float size, float sizeRate, int lifeTime)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.rotationRate = rotationRate;
            this.color = color;
            this.fadeValue = fadeValue;
            this.fadeRate = fadeRate;
            this.size = size;
            this.lifeTime = lifeTime;
        }

        public void Update()
        {
            lifeTime--;
            position += direction;
            rotation += rotationRate;
            fadeValue += fadeRate;
            size += sizeRate;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color * fadeValue,
                rotation, new Vector2(0, 0), size, SpriteEffects.None, 0f);
        }
    }
}
