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
        private List<Keys> pressedKeys = new List<Keys>();
        bool processedIt = false;
        int keyTimer = 0;
        bool rotateRight = false;
        bool rotateLeft = false;

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
            keyTimer += Constants.GUI_TICK;

            if (logic.currentRow < 1)
            {
                moveDwnalreadypressed = true;

            }

            else if(keyTimer % 32 == 0 ) moveDwnalreadypressed = false;

            if (keyTimer % 144 == 0 )
            {
                alreadyPressed = false;
                processedIt = false;
                //alreadyPressedRotate = false;
                keyTimer = 0;
            }

            // Check for specific key combinations
            //if (pressedKeys.Contains(Keys.Right) && pressedKeys.Contains(Keys.X) && !processedIt)
            //{
            //    logic.moveRight = true;
            //    logic.rotateLeft = true;
            //    processedIt = true;
            //}
            if (pressedKeys.Contains(Keys.Right) && !processedIt) { logic.moveRight = true; processedIt = true; }
            else if (pressedKeys.Contains(Keys.Left) && !processedIt) { logic.moveLeft = true; processedIt = true; }

            if (pressedKeys.Contains(Keys.Down) && !moveDwnalreadypressed) { logic.moveDownFast = true; moveDwnalreadypressed = true; }
            
            if (rotateLeft) { logic.rotateLeft = true; alreadyPressedRotate = true; rotateLeft = false; }
            else if (rotateRight) { logic.rotateRight = true; alreadyPressedRotate = true; rotateRight = false; }
        }
        private void Form1_KeyArrowsUp(object sender, KeyEventArgs e)
        //{
        //    alreadyPressedRotate = false;
        //}
        {
            if (e.KeyCode == Keys.X || e.KeyCode == Keys.Z) { alreadyPressedRotate = false; return; }

            if (pressedKeys.Contains(e.KeyCode))
                pressedKeys.Remove(e.KeyCode);
        }

        private void Form1_RotateInput(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

            }
        }

        private void Form1_KeyArrowsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z)
            
            { alreadyPressedRotate = true; rotateRight = true; return; }

            else if (e.KeyCode == Keys.X)

            { alreadyPressedRotate = true; rotateLeft = true; return; }



            else if(!pressedKeys.Contains(e.KeyCode))
            {
                //alreadyPressed = true;
                pressedKeys.Add(e.KeyCode);
                //alreadyPressedRotate = true;
                //moveDwnalreadypressed = true;
            }
                
        }
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Right:
        //            if (!alreadyPressed)
        //            {
        //                Console.WriteLine("Right");
        //                logic.moveRight = true;
        //                alreadyPressed = true;
        //            }
        //            break;

        //        case Keys.Left:
        //            if (!alreadyPressed)
        //            {
        //                logic.moveLeft = true;
        //                alreadyPressed = true;
        //            }
        //            break;

        //        case Keys.Down:
        //            if (!moveDwnalreadypressed)
        //            {
        //                logic.moveDownFast = true;
        //                moveDwnalreadypressed = true;
        //            }
        //            break;

        //        case Keys.Z:
        //            if (!alreadyPressedRotate)
        //            {
        //                Console.WriteLine("Rotate");
        //                logic.rotateRight = true;
        //                alreadyPressedRotate = true;
        //            }
        //            break;

        //        case Keys.X:
        //            if (!alreadyPressedRotate)
        //            {
        //                logic.rotateLeft = true;
        //                alreadyPressedRotate = true;
        //            }
        //            break;
        //    }
        //}
    }
}
