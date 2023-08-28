using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Sprites
    {
        private List<List<Image>> tetrominoSpriteCollection = new List<List<Image>>();

        public static readonly string pathToSprites = @"../../Sprites/";

        public readonly Image GRID_COLOR = Image.FromFile(pathToSprites + @"grid.png");
        public readonly Image OFFGRID_COLOR = Image.FromFile(pathToSprites + @"offgrid.png");

        public static readonly Image BLUE_WHITE = Image.FromFile(pathToSprites + @"blue_white.png");
        public static readonly Image BLUE = Image.FromFile(pathToSprites + @"blue.png");
        public static readonly Image TURQUOISE = Image.FromFile(pathToSprites + @"turquoise.png");
        public static readonly Image TURQUOISE_WHITE_BLUE = Image.FromFile(pathToSprites + @"turquoise_white_blue.png");

        public static readonly Image DARK_GREEN_WHITE = Image.FromFile(pathToSprites + @"dark_green_white.png");
        public static readonly Image DARK_GREEN = Image.FromFile(pathToSprites + @"dark_green.png");
        public static readonly Image LIGHT_GREEN = Image.FromFile(pathToSprites + @"light_green.png");
        public static readonly Image LIGHT_GREEN_WHITE_DARK_GREEN = Image.FromFile(pathToSprites + @"light_green_white_dark_green.png");

        public static readonly Image PURPLE_WHITE = Image.FromFile(pathToSprites + @"purple_white.png");
        public static readonly Image PURPLE = Image.FromFile(pathToSprites + @"purple.png");
        public static readonly Image PINK = Image.FromFile(pathToSprites + @"pink.png");
        public static readonly Image PINK_WHITE_PURPLE = Image.FromFile(pathToSprites + @"pink_white_purple.png");

        public static readonly Image GREEN = Image.FromFile(pathToSprites + @"green.png");
        public static readonly Image GREEN_WHITE_BLUE = Image.FromFile(pathToSprites + @"green_white_blue.png");

        public static readonly Image PASTEL_RED_WHITE = Image.FromFile(pathToSprites + @"pastel_red_white.png");
        public static readonly Image PASTEL_RED = Image.FromFile(pathToSprites + @"pastel_red.png");
        public static readonly Image PASTEL_GREEN = Image.FromFile(pathToSprites + @"pastel_green.png");
        public static readonly Image PASTEL_GREEN_WHITE_PASTEL_RED = Image.FromFile(pathToSprites + @"pastel_green_white_pastel_red.png");

        public static readonly Image PASTEL_GREEN_WHITE = Image.FromFile(pathToSprites + @"pastel_green_white.png");
        public static readonly Image LIGHT_BLUE = Image.FromFile(pathToSprites + @"light_blue.png");
        public static readonly Image LIGHT_BLUE_WHITE_PASTEL_GREEN = Image.FromFile(pathToSprites + @"light_blue_white_pastel_green.png");

        public static readonly Image RED_WHITE = Image.FromFile(pathToSprites + @"red_white.png");
        public static readonly Image RED = Image.FromFile(pathToSprites + @"red.png");
        public static readonly Image GRAY = Image.FromFile(pathToSprites + @"gray.png");
        public static readonly Image GRAY_WHITE_RED = Image.FromFile(pathToSprites + @"gray_white_red.png");

        public static readonly Image PURPLE_BLUE_WHITE = Image.FromFile(pathToSprites + @"purple_blue_white.png");
        public static readonly Image PURPLE_BLUE = Image.FromFile(pathToSprites + @"purple_blue.png");
        public static readonly Image DARK_RED = Image.FromFile(pathToSprites + @"dark_red.png");
        public static readonly Image DARK_RED_WHITE_PURPLE_BLUE = Image.FromFile(pathToSprites + @"dark_red_white_purple_blue.png");

        public static readonly Image RED_WHITE_BLUE = Image.FromFile(pathToSprites + @"red_white_blue.png");

        public static readonly Image ORANGE = Image.FromFile(pathToSprites + @"orange.png");
        public static readonly Image ORANGE_WHITE_RED = Image.FromFile(pathToSprites + @"orange_white_red.png");


        public List<List<Image>> TetrominoSpriteCollection { get => tetrominoSpriteCollection;}

        /// <summary>
        /// Constructor
        /// </summary>
        public Sprites()
        {
            TetrominoSpriteCollection.Add(new List<Image> { BLUE_WHITE, BLUE, TURQUOISE, TURQUOISE_WHITE_BLUE });
            TetrominoSpriteCollection.Add(new List<Image> { DARK_GREEN_WHITE, DARK_GREEN, LIGHT_GREEN, LIGHT_GREEN_WHITE_DARK_GREEN });
            TetrominoSpriteCollection.Add(new List<Image> { PURPLE_WHITE, PURPLE, PINK, PINK_WHITE_PURPLE });
            TetrominoSpriteCollection.Add(new List<Image> { BLUE_WHITE, BLUE, GREEN, GREEN_WHITE_BLUE });
            TetrominoSpriteCollection.Add(new List<Image> { PASTEL_RED_WHITE, PASTEL_RED, PASTEL_GREEN, PASTEL_GREEN_WHITE_PASTEL_RED });
            TetrominoSpriteCollection.Add(new List<Image> { PASTEL_GREEN_WHITE, PASTEL_GREEN, LIGHT_BLUE, LIGHT_BLUE_WHITE_PASTEL_GREEN });
            TetrominoSpriteCollection.Add(new List<Image> { RED_WHITE, RED, GRAY, GRAY_WHITE_RED });
            TetrominoSpriteCollection.Add(new List<Image> { PURPLE_BLUE_WHITE, PURPLE_BLUE, DARK_RED, DARK_RED_WHITE_PURPLE_BLUE });
            TetrominoSpriteCollection.Add(new List<Image> { BLUE_WHITE, BLUE, RED, RED_WHITE_BLUE });
            TetrominoSpriteCollection.Add(new List<Image> { RED_WHITE, RED, ORANGE, ORANGE_WHITE_RED });

        }
    }
}
