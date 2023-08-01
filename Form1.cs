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
            if (e.KeyCode == Keys.Right)
            {
                this.logic.Tetromino.MoveRight();
            }
            else if (e.KeyCode == Keys.Left)
            {
                this.logic.Tetromino.MoveLeft();
            }
            //else if ( e.KeyCode == Keys.Down)
            //{
           //     this.logic.Tetromino.MoveDown();
           // }
        }

    }
}
