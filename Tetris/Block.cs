using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{ 
    /// <summary>
    /// Tetris block class
    /// </summary>
    public class Block
    {
        // Texture of the block class
        private Texture texture;
        public Texture Texture
        {
            get
            {
                return texture;
            }
            set
            {
                // Set the texture and the Block's sprite to the set texture
                texture = value;
                sprite = new Sprite(this.texture);
            }
        }
        // Sprite of the block
        private Sprite sprite;
        public Sprite Sprite { get { return sprite; } set { sprite = value; } }
        // Size of the block in pixels
        private Vector2u size;
        public Vector2u Size { get { return size; } set { size = value; SetScale(); } }
        // Color of the block
        private BlockColor color;
        public BlockColor Color { 
            get 
            { 
                return color; 
            }
            set
            {
                // Set a new texture based on set color and set the Scale and Position appropraitely
                color = value;
                Texture = TextureManager.textureCache[this.color];
                SetScale();
                setPosition();
            }
        }

        // Position of the block on the screen
        private Vector2f position;
        public Vector2f Position { get => position; 
            set
            {
                position = value;
                setPosition();
            }
        }

        /// <summary>
        /// Instantiates a Block object with color and size in pixels
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Size"></param>
        public Block(BlockColor Color, Vector2u Size)
        {
            this.Color = Color;
            this.Size = Size;
        }

        // Sets the scale of the sprite to a desired dimension in pixels using the size of the texture
        private void SetScale()
        {
            Sprite.Scale = new Vector2f(getScalePercentage(Size.X, Texture.Size.X), getScalePercentage(Size.Y, Texture.Size.Y));
        }

        // Sets the position of the Sprite
        private void setPosition()
        {
            Sprite.Position = Position;
        }
        // Get an appropriate scale percentage based on desired pixels and the actual pixels of the texture
        private float getScalePercentage(float desiredPixels, float texturePixels)
        {
            return desiredPixels / texturePixels;
        }
        // Draw the block to the screen
        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
        }
    }
}
