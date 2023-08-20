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
        readonly Logic logic;
        readonly System.Windows.Forms.Timer timer;
        List<Keys> pressedKeys = new List<Keys>();

        bool moveDownalreadypressed = false;
        bool alreadyPressedRotate = false;
        bool alreadyPressed = false;
        bool rotateRight = false;
        bool rotateLeft = false;

        int keyTimer = 0;

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
            KeyUp += new KeyEventHandler(Form1_KeyArrowsUp);
            KeyPreview = true;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            logic.Main__();
            logic.Timer += Constants.GUI_TICK;
            keyTimer += 1;
            this.Controls[240].Text = $"SCORE\n{logic.ScoreIncrementor}";

            if (logic.currentRow < 1) moveDownalreadypressed = true;
            else if(keyTimer % 2 == 0 ) moveDownalreadypressed = false;

            if (keyTimer % 5 == 0)
            {
                alreadyPressed = false;
                keyTimer = 0;
            }

            if (pressedKeys.Contains(Keys.Right) && !alreadyPressed) { logic.MoveRight = true; alreadyPressed = true; keyTimer = 0; }
            else if (pressedKeys.Contains(Keys.Left) && !alreadyPressed) { logic.MoveLeft = true; alreadyPressed = true; keyTimer = 0; }

            if (pressedKeys.Contains(Keys.Down) && !moveDownalreadypressed) { logic.MoveDownFast = true; moveDownalreadypressed = true; }
 
            if (rotateLeft && !alreadyPressedRotate) { logic.RotateLeft = true; alreadyPressedRotate = true; rotateLeft = false; }
            else if (rotateRight && !alreadyPressedRotate) { logic.RotateRight = true; alreadyPressedRotate = true; rotateRight = false; }
        }
        private void Form1_KeyArrowsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X || e.KeyCode == Keys.Z) { alreadyPressedRotate = false; return; }
            
            if (pressedKeys.Contains(e.KeyCode))
                pressedKeys.Remove(e.KeyCode);
        }

        private void Form1_KeyArrowsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z)
            
            { alreadyPressedRotate = true; rotateLeft = true; return; }

            else if (e.KeyCode == Keys.X)

            { alreadyPressedRotate = true; rotateRight = true; return; }

            else if(!pressedKeys.Contains(e.KeyCode))
            {
                pressedKeys.Add(e.KeyCode);
            } 
        }
    }
}
