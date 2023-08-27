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
        public static readonly Image PASTEL_GREEN = Image.FromFile(pathToSprites + @"pastel_green.png");
        public static readonly Image PASTEL_GREEN_WHITE_DARK_GREEN = Image.FromFile(pathToSprites + @"pastel_green_white_dark_green.png");

        public List<List<Image>> TetrominoSpriteCollection { get => tetrominoSpriteCollection;}

        /// <summary>
        /// Constructor
        /// </summary>
        public Sprites()
        {
            TetrominoSpriteCollection.Add(new List<Image> { BLUE_WHITE, BLUE, TURQUOISE, TURQUOISE_WHITE_BLUE });
            TetrominoSpriteCollection.Add(new List<Image> { DARK_GREEN_WHITE, DARK_GREEN, PASTEL_GREEN, PASTEL_GREEN_WHITE_DARK_GREEN });

        }

    }
}
