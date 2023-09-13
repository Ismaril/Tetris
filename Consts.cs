using System;
using System.Collections.Generic;

namespace Tetris
{
    /// <summary>
    /// This class contains all the constants used in the game.
    /// </summary>
    public class Consts
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
        
        // Background collor for the application window
        public static readonly object COLOR_BACKGROUND = System.Drawing.Color.Black;

        // Offsets
        public const int CENTRE_OF_SCREEN_OFFSET = 740;


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
        public const int FAST_MUSIC_INDEX = 89;
        //


        // ------------------------------------------------------------------------------------------------
        // TETROMINO CONSTANTS
        public const byte MIN_NR_OF_TETROMINOS = 0;
        public const byte MAX_NR_OF_TETROMINOS = 1;
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

        //  ____________ 
        // /__/__/__/__/ 
        public static readonly byte[] I_0 =
        {
            0, 0, 0, 0,
            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
        };
        public static readonly byte[] I_1 =
{
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
        };


        //   ______
        //  /__/__/
        // /__/__/ 
        public static readonly byte[] O_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //    /__/ 
        public static readonly byte[] T_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_1 =
{
            0, 0, 1, 0,
            0, 1, 1, 0,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_2 =
{
            0, 0, 1, 0,
            0, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_3 =
{
            0, 0, 1, 0,
            0, 0, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };


        //      ______
        //  ___/__/__/
        // /__/__/ 
        public static readonly byte[] S_0 =
        {
            0, 0, 0, 0,
            0, 0, 2, 2,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] S_1 =
{
            0, 0, 2, 0,
            0, 0, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };


        //   ______
        //  /__/__/__
        //    /__/__/ 
        public static readonly byte[] Z_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };
        public static readonly byte[] Z_1 =
{
            0, 0, 0, 3,
            0, 0, 3, 3,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //       /__/ 
        public static readonly byte[] J_0 =
        {
            0, 0, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_1 =
        {
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_2 =
        {
            0, 2, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_3 =
        {
            0, 0, 2, 2,
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 0, 0, 0,
        };


        //    _________
        //   /__/__/__/
        //  /__/ 
        public static readonly byte[] L_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 3,
            0, 3, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_1 =
{
            0, 3, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_2 =
{
            0, 0, 0, 3,
            0, 3, 3, 3,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_3 =
{
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };

        /// <summary>
        /// List of 7 tetromino objects. Each object is unique tetromino with its properties.
        /// </summary>
        public static readonly List<Tetromino> tetrominos = new List<Tetromino>
        {
            new Tetromino(I_0, I_1, I_0, I_1, (byte)TetrominoType.I_type),
            new Tetromino(O_0, O_0, O_0, O_0, (byte)TetrominoType.O_type),
            new Tetromino(T_0, T_1, T_2, T_3, (byte)TetrominoType.T_type),
            new Tetromino(S_0, S_1, S_0, S_1, (byte)TetrominoType.S_type),
            new Tetromino(Z_0, Z_1, Z_0, Z_1, (byte)TetrominoType.Z_type),
            new Tetromino(J_0, J_1, J_2, J_3, (byte)TetrominoType.J_type),
            new Tetromino(L_0, L_1, L_2, L_3, (byte)TetrominoType.L_type)
        };
    }
}
