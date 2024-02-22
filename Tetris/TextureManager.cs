using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Enumerables that represent the different block colours
    /// </summary>
    public enum BlockColor
    {
        EMPTY,
        BLUE,
        RED,
        GREEN,
        CYAN,
        ORANGE,
        PURPLE,
        YELLOW,
    }

    /// <summary>
    /// Texture manager class that handles the textures of the Tetris blocks
    /// </summary>
    public class TextureManager
    {
        // Path to the texture images
        private static readonly string ASSETS_PATH = "Assets/";
        // Texture varaibles
        private static Texture EmptyBlockTexture;
        private static Texture blueBlockTexture;
        private static Texture redBlockTexture;
        private static Texture greenBlockTexture;
        private static Texture cyanBlockTexture;
        private static Texture orangeBlockTexture;
        private static Texture purpleBlockTexture;
        private static Texture yellowBlockTexture;

        // Map of Color to appropriate texture
        public static Dictionary<BlockColor, Texture> textureCache = new Dictionary<BlockColor, Texture>();

        // Load the appropriate images into the textures
        public static void LoadTextures()
        {
            EmptyBlockTexture = new Texture(ASSETS_PATH + "TileEmpty.png");
            blueBlockTexture = new Texture(ASSETS_PATH + "TileBlue.png");
            redBlockTexture = new Texture(ASSETS_PATH + "TileRed.png");
            greenBlockTexture = new Texture(ASSETS_PATH + "TileGreen.png");
            cyanBlockTexture = new Texture(ASSETS_PATH + "TileCyan.png");
            orangeBlockTexture = new Texture(ASSETS_PATH + "TileOrange.png");
            purpleBlockTexture = new Texture(ASSETS_PATH + "TilePurple.png");
            yellowBlockTexture = new Texture(ASSETS_PATH + "TileYellow.png");

            setTextures();
        }

        // Map the Colors to the appropriate texture
        private static void setTextures()
        {
            textureCache.Add(BlockColor.EMPTY, EmptyBlockTexture);
            textureCache.Add(BlockColor.BLUE, blueBlockTexture);
            textureCache.Add(BlockColor.RED, redBlockTexture);
            textureCache.Add(BlockColor.GREEN, greenBlockTexture);
            textureCache.Add(BlockColor.CYAN, cyanBlockTexture);
            textureCache.Add(BlockColor.ORANGE, orangeBlockTexture);
            textureCache.Add(BlockColor.PURPLE, purpleBlockTexture);
            textureCache.Add(BlockColor.YELLOW, yellowBlockTexture);
        }
    }
}
