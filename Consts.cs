using System.Drawing;


namespace Tetris
{
    /// <summary>
    /// This class contains all the global constants used in the game.
    /// </summary>
    public static class Consts
    {
        public const int GUI_TICK = 16;
        public const int MAIN_GRID_WIDTH = 10;
        public const int MAIN_GRID_HEIGHT = 24;
        public const int GRID_SURFACE_AREA = MAIN_GRID_WIDTH * MAIN_GRID_HEIGHT;
        public const byte ACTUAL_PLAYING_AREA = 200;
        public const int LAST_ROW = MAIN_GRID_HEIGHT - 5;
        public static readonly Color COLOR_BLACK = Color.Black;
        public const byte HIDDEN_UPPER_MAIN_GRID_INDEXES = 20;
        public const string TEXT_BLANK_SPACE = "- - - - - - ";
        public const string TEXT_BLANK_SPACE_SHORT = "- -";
    }
}
