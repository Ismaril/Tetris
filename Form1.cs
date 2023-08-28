using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Color A = Color.FromArgb(red: 65, green: 65, blue: 65);
        Color B = Color.Transparent;
        Sprites sprites = new Sprites();
        PictureBox pictureBox;
        List<Keys> pressedKeys = new List<Keys>();
        Label labelScore = new Label();
        Label labelTopScore = new Label();
        Label labelLevel = new Label();
        Label labelLinesCleared = new Label();

        readonly Logic logic;
        readonly System.Windows.Forms.Timer timer;

        byte shitak = 0;
        int keyTimer = 0;
        int counterInitialScreen = 0;

        bool moveDownalreadypressed = false;
        bool alreadyPressedRotate = false;
        bool alreadyPressed = false;
        bool rotateRight = false;
        bool rotateLeft = false;
        bool initialScreenDisplayed = false;
        bool labelBoxesDrawn = false;
        bool gridDrawn = false;

        public Form1()
        {
            // Enable double buffering and other optimizations
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitializeComponent();
            logic = new Logic();
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
            if (!initialScreenDisplayed)
            {
                DrawGrid();
                DrawLabelBoxes();
                InitialScreen();
            }
            else if (initialScreenDisplayed && labelBoxesDrawn && gridDrawn && counterInitialScreen >= 100)
            {
                logic.Main__(Redraw);
                logic.Timer += Constants.GUI_TICK;
                keyTimer += 1;
                Controls[257].Text = $"SCORE\n{logic.ScoreIncrementor}";
                Controls[258].Text = $"LEVEL\n{logic.CurrentLevel}";
                Controls[259].Text = $"LINES\n{logic.TotalNumberOfClearedLines}";

                if (logic.currentRow < 1) moveDownalreadypressed = true;
                else if (keyTimer % 2 == 0) moveDownalreadypressed = false;

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

                GameEnded();
                counterInitialScreen = 200;
            }
            counterInitialScreen++;
            
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
            {
                alreadyPressedRotate = true;
                rotateLeft = true;
                return;
            }
            else if (e.KeyCode == Keys.X)
            {
                alreadyPressedRotate = true;
                rotateRight = true;
                return;
            }
            else if(!pressedKeys.Contains(e.KeyCode))
            {
                pressedKeys.Add(e.KeyCode);
            } 
        }

        /// <summary>
        /// Redraw GUI based on numbers at game grid/matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Redraw(List<byte> matrix)
        {
            for (byte i = 0; i < matrix.Count; i++)
            {
                switch (matrix[i])
                {
                    case 0:
                        if (i < 20 || i >= 220) Controls[i].BackgroundImage = sprites.OFFGRID_COLOR;
                        else Controls[i].BackgroundImage = sprites.GRID_COLOR;
                        break;
                    case 1:
                        Controls[i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][0];
                        break;
                    case 2:
                        Controls[i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][1];
                        break;
                    case 3:
                        Controls[i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][2];
                        break;
                }
            }

            for (int i = 0; i < 16; i++)
            {
                Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = sprites.OFFGRID_COLOR;
                switch (logic.TetrominoNext[i])
                {
                    case 1:
                        Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][0];
                        break;
                    case 2:
                        Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][1];
                        break;
                    case 3:
                        Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][2];
                        break;
                }
            }

            // Labels have to be redrawn unfortunatelly here to avoid flickering when the appear for the first time on the screen.
            // When was this done in the definition of label it flickered as sentence above.
            Controls[256].ForeColor = A;
            Controls[256].BackColor = B;
            Controls[257].ForeColor = A;
            Controls[257].BackColor = B;
            Controls[258].ForeColor = A;
            Controls[258].BackColor = B;
            Controls[259].ForeColor = A;
            Controls[259].BackColor = B;

        }

        private void GameEnded()
        {
            if (!logic.skipLogicMain || shitak == 200) return;

            for (int i = 0; i < 10; i++)
            {
                Controls[i+20+shitak].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel%10][3];
            }
            shitak += 10;
        }

        private void DrawLabelBoxes()
        {

            // Top Score
            labelTopScore.Text = "TOP SCORE\n0";
            labelTopScore.Font = new Font("Bauhaus 93", 50);
            labelTopScore.Location = new Point(1228, 87);
            labelTopScore.Name = "textBoxTopScore";
            labelTopScore.Size = new Size(400, 150);
            Controls.Add(labelTopScore);

            // Score
            labelScore.Text = "SCORE\n0";
            labelScore.Font = new Font("Bauhaus 93", 50);
            labelScore.Location = new Point(1228, 242);
            labelScore.Name = "textBoxScore";
            labelScore.Size = new Size(400, 150);
            Controls.Add(labelScore);

            // Level
            labelLevel.Text = "LEVEL\n0";
            labelLevel.Font = new Font("Bauhaus 93", 50);
            labelLevel.Location = new Point(1228, 660);
            labelLevel.Name = "textBoxLevel";
            labelLevel.Size = new Size(400, 150);
            Controls.Add(labelLevel);

            // Lines Cleared
            labelLinesCleared.Text = "LINES\n0";
            labelLinesCleared.Font = new Font("Bauhaus 93", 50);
            labelLinesCleared.Location = new Point(1228, 814);
            labelLinesCleared.Name = "textBoxLinesCleared";
            labelLinesCleared.Size = new Size(400, 150);
            Controls.Add(labelLinesCleared);

            labelBoxesDrawn = true;
        }

        public void DrawGrid()
        {
            // Create 2D matrix at GUI.
            byte counter = 0;
            for (int i = 0; i < Constants.HEIGHT_OF_GRID; i++)
            {
                for (int j = 0; j < Constants.WIDTH_OF_GRID; j++)
                {
                    pictureBox = new PictureBox();
                    ((ISupportInitialize)(pictureBox)).BeginInit();
                    SuspendLayout();
                    pictureBox.Location = new Point(Constants.CENTRE_OF_SCREEN_OFFSET + j * 44, i * 44);
                    pictureBox.Name = $"pictureBox{counter}";
                    pictureBox.Size = new Size(40, 40);
                    pictureBox.TabIndex = 0;
                    pictureBox.TabStop = false;
                    Controls.Add(pictureBox);
                    ((ISupportInitialize)(pictureBox)).EndInit();
                    counter++;
                }
            }

            // Create a GUI matrix which will display next tetrominoCurrent.
            for (byte i = 0; i < 4; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    pictureBox = new PictureBox();
                    ((ISupportInitialize)(pictureBox)).BeginInit();
                    pictureBox.Location = new Point(1228 + j * 44, 440 + i * 44);
                    pictureBox.Name = $"pictureBox{counter}";
                    pictureBox.Size = new Size(40, 40);
                    pictureBox.TabIndex = 0;
                    pictureBox.TabStop = false;
                    Controls.Add(pictureBox);
                    ((ISupportInitialize)(pictureBox)).EndInit();
                    counter++;
                }
                gridDrawn = true;
            }
        }

        public void InitialScreen()
        {
            // Initial Screen
            pictureBox = new PictureBox();
            ((ISupportInitialize)(pictureBox)).BeginInit();
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = $"pictureBoxInitialScreen";
            pictureBox.Size = new Size(Constants.WIDTH_OF_APPLICATION_WINDOW, Constants.HEIGHT_OF_APPLICATION_WINDOW);
            pictureBox.BackColor = Color.Black;
            //this.pictureBox.Image = Image.FromFile(@"C:/Users/lazni/Downloads/Miriam Leone.jpg");
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            Controls.Add(pictureBox);
            ((ISupportInitialize)(pictureBox)).EndInit();
            initialScreenDisplayed = true;
        }
    }
}
