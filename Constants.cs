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
        public const int SPEED_OF_PIECES_FALLING = 1000;


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
        public static readonly int[] I_Default = { POS_0, POS_1, POS_2, POS_3 };

        //   ______
        //  /__/__/
        // /__/__/ 
        public static readonly int[] O_Default = {POS_1, POS_2, POS_1 + WIDTH_OF_GRID, POS_2 + WIDTH_OF_GRID };

        //   _________
        //  /__/__/__/
        //    /__/ 
        public static readonly int[] T_Default = { POS_1, POS_2, POS_2 + WIDTH_OF_GRID, POS_3 };

        //      ______
        //  ___/__/__/
        // /__/__/ 
        public static readonly int[] S_Default = {POS_1 + WIDTH_OF_GRID, POS_2 + WIDTH_OF_GRID, POS_2, POS_3 };

        //   ______
        //  /__/__/__
        //    /__/__/ 
        public static readonly int[] Z_Default = {POS_1, POS_2, POS_2 + WIDTH_OF_GRID, POS_3 + WIDTH_OF_GRID};

        //   _________
        //  /__/__/__/
        //       /__/ 
        public static readonly int[] J_Default = { POS_1, POS_2, POS_3, POS_3 + WIDTH_OF_GRID };

        //    _________
        //   /__/__/__/
        //  /__/ 
        public static readonly int[] L_Default = {POS_1 + WIDTH_OF_GRID, POS_1, POS_2, POS_3 };
        
    }
}
