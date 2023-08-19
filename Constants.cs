using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Constants
    {
        public const int WIDTH_OF_GRID = 10;
        public const int HEIGHT_OF_GRID = 24;
        public const int MOVEVEMENT_TICK = 256;
        public const int GUI_TICK = 16;
        public const byte PICTURE_BOX_SIZE = 10;
        public const byte PICTURE_BOX_LOCATION = PICTURE_BOX_SIZE + (PICTURE_BOX_SIZE / 10);
        public static readonly object COLOR_BACKGROUND = System.Drawing.Color.DimGray;
        public static readonly object COLOR_GRID = System.Drawing.Color.Black;
        public static readonly object COLOR_TETROMINO = System.Drawing.Color.Black;
        public const int FORM_HEIGHT = 200;
        public const int FORM_WIDTH = 100;
        public const byte MIN_NR_OF_TETROMINOS = 0;
        public const byte MAX_NR_OF_TETROMINOS = 7;
        public const int CENTRE_OF_SCREEN_OFFSET = 725;


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

        //public const string I_type = "I";
        //public const string O_type = "O";
        //public const string T_type = "T";
        //public const string S_type = "S";
        //public const string Z_type = "Z";
        //public const string J_type = "J";
        //public const string L_type = "L";


        public const int ROW_JUMP_GRID = WIDTH_OF_GRID;
        public const int ROW_JUMP_TETROMINO = 4;
        public const int LAST_ROW = HEIGHT_OF_GRID - 5;


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
            0, 2, 2, 0,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //    /__/ 
        public static readonly byte[] T_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 3,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_1 =
{
            0, 0, 3, 0,
            0, 3, 3, 0,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_2 =
{
            0, 0, 3, 0,
            0, 3, 3, 3,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_3 =
{
            0, 0, 3, 0,
            0, 0, 3, 3,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };


        //      ______
        //  ___/__/__/
        // /__/__/ 
        public static readonly byte[] S_0 =
        {
            0, 0, 0, 0,
            0, 0, 4, 4,
            0, 4, 4, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] S_1 =
{
            0, 0, 4, 0,
            0, 0, 4, 4,
            0, 0, 0, 4,
            0, 0, 0, 0,
        };


        //   ______
        //  /__/__/__
        //    /__/__/ 
        public static readonly byte[] Z_0 =
        {
            0, 0, 0, 0,
            0, 5, 5, 0,
            0, 0, 5, 5,
            0, 0, 0, 0,
        };
        public static readonly byte[] Z_1 =
{
            0, 0, 0, 5,
            0, 0, 5, 5,
            0, 0, 5, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //       /__/ 
        public static readonly byte[] J_0 =
        {
            0, 0, 0, 0,
            0, 6, 6, 6,
            0, 0, 0, 6,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_1 =
        {
            0, 0, 6, 0,
            0, 0, 6, 0,
            0, 6, 6, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_2 =
        {
            0, 6, 0, 0,
            0, 6, 6, 6,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_3 =
        {
            0, 0, 6, 6,
            0, 0, 6, 0,
            0, 0, 6, 0,
            0, 0, 0, 0,
        };


        //    _________
        //   /__/__/__/
        //  /__/ 
        public static readonly byte[] L_0 =
        {
            0, 0, 0, 0,
            0, 7, 7, 7,
            0, 7, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_1 =
{
            0, 7, 7, 0,
            0, 0, 7, 0,
            0, 0, 7, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_2 =
{
            0, 0, 0, 7,
            0, 7, 7, 7,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_3 =
{
            0, 0, 7, 0,
            0, 0, 7, 0,
            0, 0, 7, 7,
            0, 0, 0, 0,
        };
    }
}
