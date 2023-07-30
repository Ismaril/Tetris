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
        public const int HEIGHT_OF_GRID = 22;
        public const int SPEED_OF_PIECES_FALLING = 200;
        public const int SPEED_GUI = 50;


        public const string I_type = "I";
        public const string O_type = "O";
        public const string T_type = "T";
        public const string S_type = "S";
        public const string Z_type = "Z";
        public const string J_type = "J";
        public const string L_type = "L";


        public const int ROW_JUMP_GRID = WIDTH_OF_GRID;
        public const int ROW_JUMP_TETROMINO = 4;
        public const int LAST_ROW = HEIGHT_OF_GRID - 1;

        //  ____________ 
        // /__/__/__/__/ 
        public static readonly byte[] I_Default =
        {
            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };

        //   ______
        //  /__/__/
        // /__/__/ 
        public static readonly byte[] O_Default =
        {
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,
        };

        //   _________
        //  /__/__/__/
        //    /__/ 
        public static readonly byte[] T_Default =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };

        //      ______
        //  ___/__/__/
        // /__/__/ 
        public static readonly byte[] S_Default =
        {
            0, 0, 0, 0,
            0, 0, 1, 1,
            0, 1, 1, 0,
            0, 0, 0, 0,
        };

        //   ______
        //  /__/__/__
        //    /__/__/ 
        public static readonly byte[] Z_Default =
        {
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 0, 1, 1,
            0, 0, 0, 0,
        };

        //   _________
        //  /__/__/__/
        //       /__/ 
        public static readonly byte[] J_Default =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 0, 0, 1,
            0, 0, 0, 0,
        };

        //    _________
        //   /__/__/__/
        //  /__/ 
        public static readonly byte[] L_Default =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 1, 0, 0,
            0, 0, 0, 0,
        };
    }
}
