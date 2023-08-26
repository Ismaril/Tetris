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
        public const int GUI_TICK = 16;

        public const byte PICTURE_BOX_SIZE = 10;
        public const byte PICTURE_BOX_LOCATION = PICTURE_BOX_SIZE + (PICTURE_BOX_SIZE / 10);
        public static readonly object COLOR_BACKGROUND = System.Drawing.Color.Black;
        public const int FORM_HEIGHT = 200;
        public const int FORM_WIDTH = 100;
        public const byte MIN_NR_OF_TETROMINOS = 0;
        public const byte MAX_NR_OF_TETROMINOS = 7;
        public const int CENTRE_OF_SCREEN_OFFSET = 725;
        public const int ROW_JUMP_GRID = WIDTH_OF_GRID;
        public const int ROW_JUMP_TETROMINO = 4;
        public const int LAST_ROW = HEIGHT_OF_GRID - 5;
        public const int WIDTH_OF_APPLICATION_WINDOW = 1920;
        public const int HEIGHT_OF_APPLICATION_WINDOW = 1080;

        public static readonly System.Drawing.Image STYLE1_COLOR1 = System.Drawing.Image.FromFile(@"../../Sprites/style1_color1.png");
        public static readonly System.Drawing.Image STYLE1_COLOR2 = System.Drawing.Image.FromFile(@"../../Sprites/style1_color2.png");
        public static readonly System.Drawing.Image STYLE1_COLOR3 = System.Drawing.Image.FromFile(@"../../Sprites/style1_color3.png");
        public static readonly System.Drawing.Image GAME_OVER_GRID_COLOR = System.Drawing.Image.FromFile(@"../../Sprites/style1_color4.png");
        public static readonly System.Drawing.Image GRID_COLOR = System.Drawing.Image.FromFile(@"../../Sprites/grid_color1.png");
        public static readonly System.Drawing.Image OFFGRID_COLOR = System.Drawing.Image.FromFile(@"../../Sprites/offgrid_color.png");

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
    }
}
