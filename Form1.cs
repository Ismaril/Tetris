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
        bool alreadyPressedRotate = false;
        bool moveDwnalreadypressed = false;
        bool alreadyPressed = false;

        public Form1()
        {
            InitializeComponent();
            logic = new Logic(this.Redraw);
            timer = new System.Windows.Forms.Timer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Interval = (int)(Constants.GUI_TICK);
            timer.Tick += new EventHandler(TimerTick);
            KeyDown += new KeyEventHandler(Form1_KeyArrowsDown);
            //KeyDown += new KeyEventHandler(Form1_RotateInput);

            KeyUp += new KeyEventHandler(Form1_KeyArrowsUp);
            KeyPreview = true;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            logic.Main__();
            logic.Timer += Constants.GUI_TICK;
            if (logic.currentRow == 0 && moveDwnalreadypressed)
            {
                moveDwnalreadypressed = true;
                alreadyPressed = false;

            }
            else moveDwnalreadypressed = false;

            if (logic.Timer >= Constants.GUI_TICK*5) alreadyPressed = false;
            
        }
        private void Form1_KeyArrowsUp(object sender, KeyEventArgs e)
        {
            alreadyPressedRotate = false;
        }

        private void Form1_RotateInput(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

            }
        }

        private void Form1_KeyArrowsDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (!alreadyPressed)
                    {
                        Console.WriteLine("Right");
                        logic.moveRight = true;
                        alreadyPressed = true;
                    }
                    break;

                case Keys.Left:
                    if (!alreadyPressed)
                    {
                        logic.moveLeft = true;
                        alreadyPressed = true;
                    }
                    break;

                case Keys.Down:
                    if (!moveDwnalreadypressed)
                    {
                        logic.moveDownFast = true;
                        moveDwnalreadypressed = true;
                    }
                    break;

                case Keys.Z:
                    if (!alreadyPressedRotate)
                    {
                        Console.WriteLine("Rotate");
                        logic.rotateRight = true;
                        alreadyPressedRotate = true;
                    }
                    break;

                case Keys.X:
                    if (!alreadyPressedRotate)
                    {
                        logic.rotateLeft = true;
                        alreadyPressedRotate = true;
                    }
                    break;
            }


        }
    }
}
