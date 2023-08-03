using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Logic logic;
        System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
            this.logic = new Logic(this.Redraw);
            this.timer = new System.Windows.Forms.Timer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer.Interval = (int)(Constants.GUI_TICK);
            this.timer.Tick += new EventHandler(TimerTick);
            this.KeyDown += new KeyEventHandler(Form1_KeyArrows);
            this.KeyPreview = true;
            this.timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.logic.Main__() ;
            this.logic.Timer += Constants.GUI_TICK;
        }

        private void Form1_KeyArrows(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Right:
                    this.logic.Tetromino.MoveRight();
                    break;
                case Keys.Left:
                    this.logic.Tetromino.MoveLeft();
                    break;
                case Keys.Down:
                    this.logic.Tetromino.MoveDownFaster();
                    break;
                case Keys.Z:
                    this.logic.Tetromino.RotateRight();
                    break;
                case Keys.X:
                    this.logic.Tetromino.RotateLeft();
                    break;
            }
        }
    }
}
