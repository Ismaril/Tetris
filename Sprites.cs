using System.Collections.Generic;
using System.Drawing;

namespace Tetris
{
    public class Sprites
    {
        // ------------------------------------------------------------------------------------------------
        // FIELDS

        private List<List<Image>> tetrominoSpriteCollection = new List<List<Image>>();

        /// <summary>
        /// Collection of lists. Each sublist contains 4 different images of a tetromino single block. 
        /// Sublists are called based on which level is current player on. Based on that, 
        /// the color of tetromino blocks change.
        /// </summary>
        public List<List<Image>> TetrominoSpriteCollection { get => tetrominoSpriteCollection; }
        

        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTOR

        /// <summary>
        /// During initialisation of this constructor, a list of lists of images is created.
        /// Constructor without parameters.
        /// </summary>
        public Sprites()
        {
            // Create a list of lists of images
            TetrominoSpriteCollection.Add(new List<Image>
            {
                Consts.BLUE_WHITE,
                Consts.BLUE,
                Consts.TURQUOISE,
                Consts.TURQUOISE_WHITE_BLUE 
            });

            TetrominoSpriteCollection.Add(new List<Image>
            {
                Consts.DARK_GREEN_WHITE,
                Consts.DARK_GREEN,
                Consts.LIGHT_GREEN,
                Consts.LIGHT_GREEN_WHITE_DARK_GREEN
            });

            TetrominoSpriteCollection.Add(new List<Image>
            { 
                Consts.PURPLE_WHITE, 
                Consts.PURPLE, 
                Consts.PINK, 
                Consts.PINK_WHITE_PURPLE 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.BLUE_WHITE,
                Consts.BLUE,
                Consts.GREEN,
                Consts.GREEN_WHITE_BLUE 
            });

            TetrominoSpriteCollection.Add(new List<Image>
            { 
                Consts.PASTEL_RED_WHITE,
                Consts.PASTEL_RED,
                Consts.PASTEL_GREEN,
                Consts.PASTEL_GREEN_WHITE_PASTEL_RED 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.PASTEL_GREEN_WHITE, 
                Consts.PASTEL_GREEN, 
                Consts.LIGHT_BLUE, 
                Consts.LIGHT_BLUE_WHITE_PASTEL_GREEN 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.RED_WHITE,
                Consts.RED, 
                Consts.GRAY,
                Consts.GRAY_WHITE_RED 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.PURPLE_BLUE_WHITE, 
                Consts.PURPLE_BLUE, 
                Consts.DARK_RED, 
                Consts.DARK_RED_WHITE_PURPLE_BLUE 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.BLUE_WHITE, 
                Consts.BLUE, 
                Consts.RED, 
                Consts.RED_WHITE_BLUE 
            });

            TetrominoSpriteCollection.Add(new List<Image> 
            { 
                Consts.RED_WHITE,
                Consts.RED, 
                Consts.ORANGE,
                Consts.ORANGE_WHITE_RED 
            });
        }
    }
}
