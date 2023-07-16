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

        //  ____________ 
        // /__/__/__/__/ 
        public int[] I_Default = new int[] { 3+2, 4+2, 5 + 2, 6 + 2 };

        //   ______
        //  /__/__/
        // /__/__/ 
        public int[] O_Default = new int[] { 4, 5, 4 + WIDTH_OF_GRID, 5 + WIDTH_OF_GRID };
    



    }
}
