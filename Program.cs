using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tetris
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

// BUGS&TODOS -----------------------------------------------------------------------------------------------------------------------
// TODO: Collision detection to the sides.
// TODO: Let tetromino fall when it arrived at top of next tetromino but you can still move it to the side where it is clear below.
// TODO: Rotation of tetrominos.
// TODO: Try to unify redraw functions.



// FIXED ----------------------------------------------------------------------------------------------------------------------------
// Let I fall on bottom - 2.
// Collision detection when tetromino falling.
// Collision detection when tetromino at top row (game ended)

