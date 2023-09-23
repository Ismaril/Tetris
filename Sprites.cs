using System.Collections.Generic;
using System.Drawing;
// DONE
namespace Tetris
{
    public class Sprites
    {
        // ------------------------------------------------------------------------------------------------
        // CONSTANTS

        public static readonly string SPRITES_PATH = @"../../Sprites/";
        public const string WALLPAPER_INITIAL_SCREEN = @"..\..\Sprites\Praha.png";
        public static readonly Image GRID_COLOR = Image.FromFile(SPRITES_PATH + "grid.png");
        public static readonly Image OFFGRID_COLOR = Image.FromFile(SPRITES_PATH + "offgrid.png");

        private readonly Image BLUE_WHITE = Image.FromFile(SPRITES_PATH + "blue_white.png");
        private readonly Image BLUE = Image.FromFile(SPRITES_PATH + "blue.png");
        private readonly Image TURQUOISE = Image.FromFile(SPRITES_PATH + "turquoise.png");
        private readonly Image TURQUOISE_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "turquoise_white_blue.png");
        private readonly Image DARK_GREEN_WHITE = Image.FromFile(SPRITES_PATH + "dark_green_white.png");
        private readonly Image DARK_GREEN = Image.FromFile(SPRITES_PATH + "dark_green.png");
        private readonly Image LIGHT_GREEN = Image.FromFile(SPRITES_PATH + "light_green.png");
        private readonly Image LIGHT_GREEN_WHITE_DARK_GREEN = Image.FromFile(SPRITES_PATH + "light_green_white_dark_green.png");
        private readonly Image PURPLE_WHITE = Image.FromFile(SPRITES_PATH + "purple_white.png");
        private readonly Image PURPLE = Image.FromFile(SPRITES_PATH + "purple.png");
        private readonly Image PINK = Image.FromFile(SPRITES_PATH + "pink.png");
        private readonly Image PINK_WHITE_PURPLE = Image.FromFile(SPRITES_PATH + "pink_white_purple.png");
        private readonly Image GREEN = Image.FromFile(SPRITES_PATH + "green.png");
        private readonly Image GREEN_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "green_white_blue.png");
        private readonly Image PASTEL_RED_WHITE = Image.FromFile(SPRITES_PATH + "pastel_red_white.png");
        private readonly Image PASTEL_RED = Image.FromFile(SPRITES_PATH + "pastel_red.png");
        private readonly Image PASTEL_GREEN = Image.FromFile(SPRITES_PATH + "pastel_green.png");
        private readonly Image PASTEL_GREEN_WHITE_PASTEL_RED = Image.FromFile(SPRITES_PATH + "pastel_green_white_pastel_red.png");
        private readonly Image PASTEL_GREEN_WHITE = Image.FromFile(SPRITES_PATH + "pastel_green_white.png");
        private readonly Image LIGHT_BLUE = Image.FromFile(SPRITES_PATH + "light_blue.png");
        private readonly Image LIGHT_BLUE_WHITE_PASTEL_GREEN = Image.FromFile(SPRITES_PATH + "light_blue_white_pastel_green.png");
        private readonly Image RED_WHITE = Image.FromFile(SPRITES_PATH + "red_white.png");
        private readonly Image RED = Image.FromFile(SPRITES_PATH + "red.png");
        private readonly Image GRAY = Image.FromFile(SPRITES_PATH + "gray.png");
        private readonly Image GRAY_WHITE_RED = Image.FromFile(SPRITES_PATH + "gray_white_red.png");
        private readonly Image PURPLE_BLUE_WHITE = Image.FromFile(SPRITES_PATH + "purple_blue_white.png");
        private readonly Image PURPLE_BLUE = Image.FromFile(SPRITES_PATH + "purple_blue.png");
        private readonly Image DARK_RED = Image.FromFile(SPRITES_PATH + "dark_red.png");
        private readonly Image DARK_RED_WHITE_PURPLE_BLUE = Image.FromFile(SPRITES_PATH + "dark_red_white_purple_blue.png");
        private readonly Image RED_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "red_white_blue.png");
        private readonly Image ORANGE = Image.FromFile(SPRITES_PATH + "orange.png");
        private readonly Image ORANGE_WHITE_RED = Image.FromFile(SPRITES_PATH + "orange_white_red.png");


        // ------------------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Each sublist contains 4 different images of a tetromino single block. 
        /// Sub-lists are called based on which level is current player on. Based on that, 
        /// the color of tetromino blocks change.
        /// </summary>
        public List<List<Image>> TetrominoBlocks { get; } = new List<List<Image>>();


        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTOR

        /// <summary>
        /// During initialization of this constructor, tetromino blocks sprites list is created.
        /// </summary>
        public Sprites()
        {
            // Create a list of lists of images
            TetrominoBlocks.Add(new List<Image>
            {
                BLUE_WHITE,
                BLUE,
                TURQUOISE,
                TURQUOISE_WHITE_BLUE
            });

            TetrominoBlocks.Add(new List<Image>
            {
                DARK_GREEN_WHITE,
                DARK_GREEN,
                LIGHT_GREEN,
                LIGHT_GREEN_WHITE_DARK_GREEN
            });

            TetrominoBlocks.Add(new List<Image>
            {
                PURPLE_WHITE,
                PURPLE,
                PINK,
                PINK_WHITE_PURPLE
            });

            TetrominoBlocks.Add(new List<Image>
            {
                BLUE_WHITE,
                BLUE,
                GREEN,
                GREEN_WHITE_BLUE
            });

            TetrominoBlocks.Add(new List<Image>
            {
                PASTEL_RED_WHITE,
                PASTEL_RED,
                PASTEL_GREEN,
                PASTEL_GREEN_WHITE_PASTEL_RED
            });

            TetrominoBlocks.Add(new List<Image>
            {
                PASTEL_GREEN_WHITE,
                PASTEL_GREEN,
                LIGHT_BLUE,
                LIGHT_BLUE_WHITE_PASTEL_GREEN
            });

            TetrominoBlocks.Add(new List<Image>
            {
                RED_WHITE,
                RED,
                GRAY,
                GRAY_WHITE_RED
            });

            TetrominoBlocks.Add(new List<Image>
            {
                PURPLE_BLUE_WHITE,
                PURPLE_BLUE,
                DARK_RED,
                DARK_RED_WHITE_PURPLE_BLUE
            });

            TetrominoBlocks.Add(new List<Image>
            {
                BLUE_WHITE,
                BLUE,
                RED,
                RED_WHITE_BLUE
            });

            TetrominoBlocks.Add(new List<Image>
            {
                RED_WHITE,
                RED,
                ORANGE,
                ORANGE_WHITE_RED
            });
        }
    }
}
