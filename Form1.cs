using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public Form1()
        {
            InitializeComponent();
            this.logic = new Logic(this.Redraw, this.RedrawBack);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyArrows);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (Constants.SPEED_OF_PIECES_FALLING);

            
            timer.Tick += new EventHandler(timerTick);
            timer.Start();

        }
        private void timerTick(object sender, EventArgs e)
        {
            this.logic.Main__() ;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

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
