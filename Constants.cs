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
        public const int HEIGHT_OF_GRID = 20;
        public const int SPEED_OF_PIECES_FALLING = 100;
        public const int SPEED_GUI = 30;


        public const string I_type = "I";
        public const string O_type = "O";
        public const string T_type = "T";
        public const string S_type = "S";
        public const string Z_type = "Z";
        public const string J_type = "J";
        public const string L_type = "L";


        public const int ROW_JUMP = WIDTH_OF_GRID;
        public const int LAST_ROW = HEIGHT_OF_GRID - 1;



        // ----------------------------------------------------------------------------------
        // INDEX POSITIONS OF TETROMINOS WHEN THEY APPEAR AS A FRESH PIECE AT THE TOP OF GRID
        // ----------------------------------------------------------------------------------

        // Adjustable index positions based on width of grid
        public const int POS_0 = WIDTH_OF_GRID / 2 - 2;
        public const int POS_1 = POS_0 + 1;
        public const int POS_2 = POS_0 + 2;
        public const int POS_3 = POS_0 + 3;



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
