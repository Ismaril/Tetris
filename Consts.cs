using System;
using System.Collections.Generic;
using System.Drawing;


namespace Tetris
{


    /// <summary>
    /// This class contains all the constants used in the game.
    /// </summary>
    public static class Consts
    {









        // ------------------------------------------------------------------------------------------------
        // SPEEDS OF THE GAME

        // Tick after how many milliseconds should the graphics update
        public const int GUI_TICK = 16;

        // Matrix shape (main game grid)
        public const int WIDTH_OF_GRID = 10;
        public const int HEIGHT_OF_GRID = 24;
        public const int GRID = WIDTH_OF_GRID * HEIGHT_OF_GRID;


        // Unresolved yet
        public const int ROW_JUMP_GRID = WIDTH_OF_GRID;
        public const int LAST_ROW = HEIGHT_OF_GRID - 5;
        public const int MINIMUM_HIGH_SCORE_LIMIT = 10_000;
        public const byte INITIAL_SCRREN_VISIBILITY_LIMIT = 100;
        public const int FAST_MUSIC_INDEX = 89;
        public static readonly Color COLOR_BLACK = Color.Black;
        //


        // ------------------------------------------------------------------------------------------------
        // TETROMINO CONSTANTS
        public const byte MIN_NR_OF_TETROMINOS = 0;
        public const byte MAX_NR_OF_TETROMINOS = 7;
        public const int ROW_JUMP_TETROMINO = 4;
        public const int SCORE_ONE_LINE = 40;
        public const int SCORE_TWO_LINES = 100;
        public const int SCORE_THREE_LINES = 300;
        public const int SCORE_FOUR_LINES = 1200;




    }
}
