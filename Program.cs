using System;
using System.Windows.Forms;

namespace Tetris
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Fyi: Main grid and main matrix in comments are the same thing in this project.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

// refactoring tip: max 3 indentations in a method
