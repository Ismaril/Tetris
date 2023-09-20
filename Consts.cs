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
        // GUI CONSTANTS

        // Matrix shape (main game grid)
        public const int WIDTH_OF_GRID = 10;
        public const int HEIGHT_OF_GRID = 24;
        
        // Picture box size and location (main game grid)
        public const byte PICTURE_BOX_SIZE = 40;
        public const byte PICTURE_BOX_LOCATION = PICTURE_BOX_SIZE + (PICTURE_BOX_SIZE / 10);
        
        // Application window size
        public const int WIDTH_OF_APPLICATION_WINDOW = 1920;
        public const int HEIGHT_OF_APPLICATION_WINDOW = 1080;

        // Statistics label boxes size and location and text
        public const int WIDTH_OF_STATISTICS_LABEL_BOX = 400;
        public const int HEIGHT_OF_STATISTICS_LABEL_BOX = 150;
        public const int X_STATISTICS_LABEL_BOX = 1228;

        // Colors
        public static readonly Color COLOR_BLACK = Color.Black;
        public static readonly Color COLOR_GRAY = Color.FromArgb(red: 65, green: 65, blue: 65);
        public static readonly Color COLOR_RED = Color.FromArgb(248, 56, 0);
        public static readonly Color COLOR_TRANSPARENT = Color.Transparent;

        // Offsets
        public const int OFFSET_CENTRE_OF_SCREEN = 740;

        // Texts
        public const string FONT_BAUHAUS = "Bauhaus 93";
        public const float FONT_SIZE_BIG = 50;
        public const float FONT_SIZE_SMALL = 26;

        public const string TEXT_TOP_SCORE = "TOP SCORE";
        public const string TEXT_SCORE = "SCORE";
        public const string TEXT_LEVEL = "LEVEL";
        public const string TEXT_LEVEL_SHORT = "LVL";
        public const string TEXT_LINES_CLEARED = "LINES";
        public const string TEXT_CONGRATULATIONS = "CONGRATULATIONS";
        public const string TEXT_TETRIS_MASTER = "YOU ARE A TETRIS MASTER";
        public const string TEXT_ENTER_YOUR_NAME = "PLEASE ENTER YOUR NAME";
        public const string TEXT_LEVEL_SETTING = "SET LEVEL";
        public const string TEXT_MUSIC_SETTING = "SET MUSIC";
        public const string TEXT_ON_SETTING = "ON";
        public const string TEXT_OFF_SETTING = "OFF";
        public const string TEXT_BLANK_SPACE = "- - - - - - ";
        public const string TEXT_BLANK_SPACE_SHORT = "- -";
        public const string TEXT_NAME = "NAME";
        public const string TEXT_TEXTBOX = "textBox";
        public const string TEXT_LABELBOX = "labelBox";
        public const string TEXT_PICTUREBOX = "pictureBox";
        public const string TEXT_PICTUREBOX_INITIAL_SCREEN = "pictureBoxInitialScreen";






        // ------------------------------------------------------------------------------------------------
        // SPRITES CONSTANTS

        public const string WALLPAPER_INITIAL_SCREEN = @"..\..\Sprites\hrad1_adjusted_tetris.png";

        public static readonly string SPRITES_PATH = @"../../Sprites/";
        public static readonly Image GRID_COLOR = Image.FromFile(SPRITES_PATH + "grid.png");
        public static readonly Image OFFGRID_COLOR = Image.FromFile(SPRITES_PATH + "offgrid.png");
        public static readonly Image BLUE_WHITE = Image.FromFile(SPRITES_PATH + "blue_white.png");
        public static readonly Image BLUE = Image.FromFile(SPRITES_PATH + "blue.png");
        public static readonly Image TURQUOISE = Image.FromFile(SPRITES_PATH + "turquoise.png");
        public static readonly Image TURQUOISE_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "turquoise_white_blue.png");
        public static readonly Image DARK_GREEN_WHITE = Image.FromFile(SPRITES_PATH + "dark_green_white.png");
        public static readonly Image DARK_GREEN = Image.FromFile(SPRITES_PATH + "dark_green.png");
        public static readonly Image LIGHT_GREEN = Image.FromFile(SPRITES_PATH + "light_green.png");
        public static readonly Image LIGHT_GREEN_WHITE_DARK_GREEN = Image.FromFile(SPRITES_PATH + "light_green_white_dark_green.png");
        public static readonly Image PURPLE_WHITE = Image.FromFile(SPRITES_PATH + "purple_white.png");
        public static readonly Image PURPLE = Image.FromFile(SPRITES_PATH + "purple.png");
        public static readonly Image PINK = Image.FromFile(SPRITES_PATH + "pink.png");
        public static readonly Image PINK_WHITE_PURPLE = Image.FromFile(SPRITES_PATH + "pink_white_purple.png");
        public static readonly Image GREEN = Image.FromFile(SPRITES_PATH + "green.png");
        public static readonly Image GREEN_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "green_white_blue.png");
        public static readonly Image PASTEL_RED_WHITE = Image.FromFile(SPRITES_PATH + "pastel_red_white.png");
        public static readonly Image PASTEL_RED = Image.FromFile(SPRITES_PATH + "pastel_red.png");
        public static readonly Image PASTEL_GREEN = Image.FromFile(SPRITES_PATH + "pastel_green.png");
        public static readonly Image PASTEL_GREEN_WHITE_PASTEL_RED = Image.FromFile(SPRITES_PATH + "pastel_green_white_pastel_red.png");
        public static readonly Image PASTEL_GREEN_WHITE = Image.FromFile(SPRITES_PATH + "pastel_green_white.png");
        public static readonly Image LIGHT_BLUE = Image.FromFile(SPRITES_PATH + "light_blue.png");
        public static readonly Image LIGHT_BLUE_WHITE_PASTEL_GREEN = Image.FromFile(SPRITES_PATH + "light_blue_white_pastel_green.png");
        public static readonly Image RED_WHITE = Image.FromFile(SPRITES_PATH + "red_white.png");
        public static readonly Image RED = Image.FromFile(SPRITES_PATH + "red.png");
        public static readonly Image GRAY = Image.FromFile(SPRITES_PATH + "gray.png");
        public static readonly Image GRAY_WHITE_RED = Image.FromFile(SPRITES_PATH + "gray_white_red.png");
        public static readonly Image PURPLE_BLUE_WHITE = Image.FromFile(SPRITES_PATH + "purple_blue_white.png");
        public static readonly Image PURPLE_BLUE = Image.FromFile(SPRITES_PATH + "purple_blue.png");
        public static readonly Image DARK_RED = Image.FromFile(SPRITES_PATH + "dark_red.png");
        public static readonly Image DARK_RED_WHITE_PURPLE_BLUE = Image.FromFile(SPRITES_PATH + "dark_red_white_purple_blue.png");
        public static readonly Image RED_WHITE_BLUE = Image.FromFile(SPRITES_PATH + "red_white_blue.png");
        public static readonly Image ORANGE = Image.FromFile(SPRITES_PATH + "orange.png");
        public static readonly Image ORANGE_WHITE_RED = Image.FromFile(SPRITES_PATH + "orange_white_red.png");


        // ------------------------------------------------------------------------------------------------
        // SPEEDS OF THE GAME

        // Tick after how many milliseconds should the graphics update
        public const int GUI_TICK = 16;

        // After how many multiples of GUI_TICK should the tetromino move down
        public static readonly int[] movementTicksBasedOnLevel = {
            GUI_TICK * 48, // Level 0
            GUI_TICK * 43,
            GUI_TICK * 38,
            GUI_TICK * 33,
            GUI_TICK * 28,
            GUI_TICK * 23, // Level 5
            GUI_TICK * 18,
            GUI_TICK * 13,
            GUI_TICK * 8,
            GUI_TICK * 6,
            GUI_TICK * 5,  // Level 10
            GUI_TICK * 5,
            GUI_TICK * 5,
            GUI_TICK * 4,
            GUI_TICK * 4,
            GUI_TICK * 4,  // Level 15
            GUI_TICK * 3,
            GUI_TICK * 3,
            GUI_TICK * 3,
            GUI_TICK * 2,
            GUI_TICK * 2,  // Level 20
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 2,  // Level 25
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 2,
            GUI_TICK * 1,  // Level 30
        };


        // Unresolved yet
        public const int ROW_JUMP_GRID = WIDTH_OF_GRID;
        public const int LAST_ROW = HEIGHT_OF_GRID - 5;
        public const int MINIMUM_HIGH_SCORE_LIMIT = 10_000;
        public const byte INITIAL_SCRREN_VISIBILITY_LIMIT = 100;
        public const int FAST_MUSIC_INDEX = 89;
        //


        // ------------------------------------------------------------------------------------------------
        // TETROMINO CONSTANTS
        public const byte MIN_NR_OF_TETROMINOS = 1;
        public const byte MAX_NR_OF_TETROMINOS = 7;
        public const int ROW_JUMP_TETROMINO = 4;
        public const int SCORE_ONE_LINE = 40;
        public const int SCORE_TWO_LINES = 100;
        public const int SCORE_THREE_LINES = 300;
        public const int SCORE_FOUR_LINES = 1200;

        /// <summary>
        /// The type of tetrominos. There are 7 types of tetrominos.
        /// </summary>
        public enum TetrominoType
        {
            I_type,
            O_type,
            T_type,
            S_type,
            Z_type,
            J_type,
            L_type
        }


    }
}
