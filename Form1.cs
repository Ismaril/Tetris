using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        string name = "- - - - - - ";
        string nameAdjusted = "";
        string[] names = {"- - - - - - ", "- - - - - - " , "- - - - - - "};


        int highScore = 0;
        Dictionary<int, (string, int, int)> scores = new Dictionary<int, (string, int, int)>();
        List<(string, int, int)> scoresList = new List<(string, int, int)>();
        byte scoreScreenLabelboxIndex = 1;

        Color A = Color.FromArgb(red: 65, green: 65, blue: 65);
        Color B = Color.Transparent;
        Sprites sprites = new Sprites();
        PictureBox pictureBox;
        Music music;
        Label labelBox;
        List<Keys> pressedKeys = new List<Keys>();
        Label labelScore = new Label();
        Label labelTopScore = new Label();
        Label labelLevel = new Label();
        Label labelLinesCleared = new Label();

        Label labelLevelSetting = new Label();
        Label labelMusicSetting = new Label();
        Label labelMusicOn = new Label();
        Label labelMusicOff = new Label();
        Label scoreScreenGratulation = new Label();
        Label scoreScreenGratulationTitle = new Label();

        Logic logic;
        readonly System.Windows.Forms.Timer timer;

        byte shitak = 0;
        int keyTimer = 0;
        int counterInitialScreen = 0;

        bool screenUpdaterScreenVisible = false;
        bool moveDownalreadypressed = false;
        bool alreadyPressedRotate = false;
        bool alreadyPressed = false;
        bool rotateRight = false;
        bool rotateLeft = false;
        bool initialScreenDisplayed = false;
        bool labelBoxesDrawn = false;
        bool gridDrawn = false;

        bool settingsScrennDisplayed = false;
        bool playMusic = true;
        sbyte levelSettingInitial = 0;
        bool playGame = false;
        bool mainMusicStartedPlayin = false;

        bool logicEndedUserPressedEnter = false;

        internal Music Music { get => music; set => music = value; }

        public Form1()
        {
            // Enable double buffering and other optimizations
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitializeComponent();
            Music = new Music();
            logic = new Logic(Music);
            timer = new System.Windows.Forms.Timer();
            InitialScreen(); //----------------------------------------------- uncomment
            //ScoreScreenInitilaizer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor.Hide(); // Hide mouse
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized; // Full screen window mode

            timer.Interval = (int)(Constants.GUI_TICK);
            timer.Tick += new EventHandler(TimerTick);
            KeyDown += new KeyEventHandler(Form1_KeyArrowsDown);
            KeyUp += new KeyEventHandler(Form1_KeyArrowsUp);
            KeyPress += new KeyPressEventHandler(YourKeyPressEventHandler);
            KeyPreview = true;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!initialScreenDisplayed)
            {
                if (counterInitialScreen > 100)
                {
                    initialScreenDisplayed = true;
                    //Controls[0].Dispose();
                    Controls.RemoveByKey("pictureBoxInitialScreen");
                    Console.WriteLine(Controls.ContainsKey("pictureBoxInitialScreen"));
                    //Console.WriteLine(Controls[0].Disposing);
                }
                counterInitialScreen++;
            }
            else if (!settingsScrennDisplayed)
            {
                SettingsScreen();



            }
            else if (playGame && !logicEndedUserPressedEnter)
            {
                if (playMusic && !mainMusicStartedPlayin)
                {
                    //music.InitialiseBackgroundMusic();
                    music.ChoseMainMusic();
                    logic.MusicSlowIsPlaying = true;
                    music.MainMusicSlow();
                    mainMusicStartedPlayin = true;
                }
                logic.Main__(Redraw);
                logic.Timer += Constants.GUI_TICK;
                keyTimer += 1;
                Controls[256].Text = $"TOP SCORE\n{highScore}";
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
                //music.GetPositionOfMainMusic();
            }
            else if (logic.RoundEnded1 && logic.ScoreIncrementor >= 10_000 && screenUpdaterScreenVisible)
            {
                ScoreScreenUpdater();

            }
        }

        private void YourKeyPressEventHandler(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a valid character (not a control key)
            if (!char.IsControl(e.KeyChar) && screenUpdaterScreenVisible)
            {
                // Convert the pressed key to its ASCII character representation
                char pressedChar = e.KeyChar;

                // Now you have the ASCII character representation in the 'pressedChar' variable
                // You can use 'pressedChar' in your code
                name += pressedChar;
                if(name.Length > 6) nameAdjusted = name.Substring(startIndex: name.Length - 6, 6);
                names[scoreScreenLabelboxIndex-1] = nameAdjusted;
                music.SoundSettings();
                Console.WriteLine(name);
            }
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

            if (screenUpdaterScreenVisible)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    levelSettingInitial = 0;
                    playMusic = true;
                    playGame = false;
                    Controls.Clear();
                    settingsScrennDisplayed = false;
                    mainMusicStartedPlayin = false;
                    screenUpdaterScreenVisible = false;
                    music.DisposeBackgroundMusic_NAudio();
                    name = "- - - - - - ";
                }

            }
            else if ((settingsScrennDisplayed && !playGame) || (!settingsScrennDisplayed && logicEndedUserPressedEnter))
            {
                Controls[levelSettingInitial+1].BackColor = A;
                if (e.KeyCode == Keys.Right) {levelSettingInitial++;music.SoundSettings(); }
                else if (e.KeyCode == Keys.Left) {levelSettingInitial--; music.SoundSettings(); }
                if(levelSettingInitial == 30) levelSettingInitial = 0;
                else if(levelSettingInitial == -1) levelSettingInitial = 29;
                Controls[levelSettingInitial+1].BackColor = Color.FromArgb(248, 56, 0);


                if (e.KeyCode == Keys.Up) { playMusic = !playMusic; music.SoundSettings(); }
                if (e.KeyCode == Keys.Down) { playMusic = !playMusic; music.SoundSettings(); }
                if(playMusic)
                {
                    Controls[32].BackColor = Color.FromArgb(248, 56, 0); ;
                    Controls[33].BackColor = A;
                }
                else
                {
                    Controls[32].BackColor = A;
                    Controls[33].BackColor = Color.FromArgb(248, 56, 0);
                }
                logic.CurrentLevel = (byte)levelSettingInitial;
                logic.PlayMusic = playMusic;
                music.MusicIsAllowed = playMusic;

                if (e.KeyCode == Keys.Enter)
                {
                    if (logic.RoundEnded1)
                    {
                        logic.ResetAllFields();
                        music.DisposeBackgroundMusic_NAudio();
                        shitak = 0;
                    }
                    
                    Controls.Clear();
                    playGame = true;
                    logicEndedUserPressedEnter = false;
                    
                    logic.CurrentLevel = (byte)levelSettingInitial;
                    logic.PlayMusic = playMusic;
                    DrawGrid();
                    DrawLabelBoxes();
                }


            }
            else if(logic.RoundEnded1 && e.KeyCode == Keys.Enter && logic.ScoreIncrementor >= 10_000)
            {
                if (logic.ScoreIncrementor > highScore)
                {
                    highScore = logic.ScoreIncrementor;
                }
                scores[logic.ScoreIncrementor] = ("- - - - - -", logic.ScoreIncrementor, logic.CurrentLevel);
                //scores.OrderByDescending(c => c.Key);
                scoresList = scores.Values.ToList();
                //scoresList.OrderByDescending(c => c.Item2);
                scoresList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

                if (logic.ScoreIncrementor >= scoresList[0].Item2)
                {
                    scoreScreenLabelboxIndex = 1;
                    names[2] = names[1];
                    names[1] = names[0];
                    names[0] = "- - - - - -";

                }
                if (logic.ScoreIncrementor < scoresList[0].Item2)
                {
                    scoreScreenLabelboxIndex = 2;
                }
                try
                {
                    if (logic.ScoreIncrementor < scoresList[1].Item2)
                    {
                        scoreScreenLabelboxIndex = 3;
                    }
                }
                catch { }


                

                levelSettingInitial = 0;
                playMusic = true;
                playGame = false;
                Controls.Clear();
                ScoreScreenInitilaizer();
                mainMusicStartedPlayin = false;
                screenUpdaterScreenVisible = true;

            }
            else if (logic.RoundEnded1 && e.KeyCode == Keys.Enter && logic.ScoreIncrementor < 10_000)
            {
                levelSettingInitial = 0;
                playMusic = true;
                playGame = false;
                Controls.Clear();
                settingsScrennDisplayed = false;
                mainMusicStartedPlayin = false;
            }

            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
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

        public void SettingsScreen()
        {

            // level setting
            labelLevelSetting.Text = "SET LEVEL";
            labelLevelSetting.Font = new Font("Bauhaus 93", 50);
            labelLevelSetting.Location = new Point(820, 100);
            labelLevelSetting.Name = "labelBoxlevelSetting";
            labelLevelSetting.Size = new Size(300, 90);
            labelLevelSetting.ForeColor = A;
            labelLevelSetting.BackColor = Color.Black;
            Controls.Add(labelLevelSetting);

            byte counter = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    labelBox = new Label();
                    labelBox.Text = $"{counter}";
                    labelBox.Font = new Font("Bauhaus 93", 26);
                    labelBox.Location = new Point(635 + j * 66, i * 66 + 200);
                    labelBox.Name = $"labelBox{counter}";
                    labelBox.Size = new Size(60, 60);
                    labelBox.ForeColor = Color.Black;
                    if(i+j==0) labelBox.BackColor = Color.FromArgb(248, 56, 0);
                    else labelBox.BackColor = A;
                    Controls.Add(labelBox);
                    counter++;
                }
            }


            // Music settings description
            labelMusicSetting.Text = "SET MUSIC";
            labelMusicSetting.Font = new Font("Bauhaus 93", 50);
            labelMusicSetting.Location = new Point(795, 450);
            labelMusicSetting.Name = "labelBoxLinesCleared";
            labelMusicSetting.Size = new Size(340, 90);
            labelMusicSetting.ForeColor = A;
            labelMusicSetting.BackColor = Color.Black;
            Controls.Add(labelMusicSetting);

            // Music ON
            labelMusicOn.Text = "ON";
            labelMusicOn.Font = new Font("Bauhaus 93", 26);
            labelMusicOn.Location = new Point(920, 540);
            labelMusicOn.Name = "labelBoxLevel";
            labelMusicOn.Size = new Size(80, 80);
            labelMusicOn.ForeColor = System.Drawing.Color.Black;
            labelMusicOn.BackColor = Color.FromArgb(248, 56, 0);
            Controls.Add(labelMusicOn);

            // music off
            labelMusicOff.Text = "OFF";
            labelMusicOff.Font = new Font("Bauhaus 93", 26);
            labelMusicOff.Location = new Point(920, 626);
            labelMusicOff.Name = "labelBoxLinesCleared";
            labelMusicOff.Size = new Size(80, 80);
            labelMusicOff.ForeColor = System.Drawing.Color.Black;
            labelMusicOff.BackColor = A;
            Controls.Add(labelMusicOff);
            settingsScrennDisplayed = true;
        }

        public void InitialScreen()
        {
            // Initial Screen
            pictureBox = new PictureBox();
            ((ISupportInitialize)(pictureBox)).BeginInit();
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = $"pictureBoxInitialScreen";
            pictureBox.Size = new Size(Constants.WIDTH_OF_APPLICATION_WINDOW, Constants.HEIGHT_OF_APPLICATION_WINDOW);
            //pictureBox.BackColor = Color.Red;
            this.pictureBox.Image = Image.FromFile(@"..\..\Sprites\hrad1_adjusted_tetris.png");
            Controls.Add(pictureBox);
            ((ISupportInitialize)(pictureBox)).EndInit();
        }

        public void ScoreScreenInitilaizer()
        // this has to be only initialised once, and subsequent changes can be done in loop
        {

            scoreScreenGratulationTitle.Text = "CONGRATULATIONS";
            scoreScreenGratulationTitle.Font = new Font("Bauhaus 93", 50);
            scoreScreenGratulationTitle.Location = new Point(660, 100);
            scoreScreenGratulationTitle.Name = "labelBoxLevel";
            scoreScreenGratulationTitle.Size = new Size(650, 90);
            scoreScreenGratulationTitle.ForeColor = Color.FromArgb(248, 56, 0);
            scoreScreenGratulationTitle.BackColor = System.Drawing.Color.Black;
            Controls.Add(scoreScreenGratulationTitle);

            scoreScreenGratulation.Text = "YOU ARE A TETRIS MASTER\nPLEASE ENTER YOUR NAME";
            scoreScreenGratulation.Font = new Font("Bauhaus 93", 50);
            scoreScreenGratulation.Location = new Point(560, 240);
            scoreScreenGratulation.Name = "labelBoxLevel";
            scoreScreenGratulation.Size = new Size(800, 160);
            scoreScreenGratulation.ForeColor = Color.FromArgb(65, 65, 65);
            scoreScreenGratulation.BackColor = System.Drawing.Color.Black;
            Controls.Add(scoreScreenGratulation);

            byte counter = 0;
            int yOffset = 0;
            int XoFFSET = 20;
            for (byte i = 0; i < 4; i++)
            {
                // labelbox poradi
                if(i > 0)
                {
                    labelBox = new Label();
                    labelBox.Text = $"{counter}";
                    labelBox.Font = new Font("Bauhaus 93", 50);
                    labelBox.Location = new Point(500 + XoFFSET, 540 + yOffset);
                    labelBox.Name = $"labelBoxRow{i}";
                    labelBox.Size = new Size(90, 90);
                    labelBox.ForeColor = Color.FromArgb(65, 65, 65);
                    labelBox.BackColor = Color.Black;
                    Controls.Add(labelBox);
                }

                // labelboxy nazev viteze
                labelBox = new Label();
                if (i == 0) { labelBox.Text = $"NAME"; }
                try
                {

                    if (i > 0) { labelBox.Text = $"{names[i-1]}"; }
                }
                catch { labelBox.Text = "- - - - - -"; }

                labelBox.Font = new Font("Bauhaus 93", 50);
                labelBox.Location = new Point(630 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxNAME{i}";
                labelBox.Size = new Size(270, 90);
                labelBox.ForeColor = Color.FromArgb(65, 65, 65);
                labelBox.BackColor = Color.Black;
                Controls.Add(labelBox);



                // labelbox skore z logic.score
                labelBox = new Label();
                if (i == 0) { labelBox.Text = $"SCORE"; }
                try
                {
                    if (i > 0) { labelBox.Text = $"{scoresList[i-1].Item2}"; }
                }
                catch { labelBox.Text = "- - - - - -"; }

                labelBox.Font = new Font("Bauhaus 93", 50);
                labelBox.Location = new Point(930 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxSCORE{i}";
                labelBox.Size = new Size(280, 90);
                labelBox.ForeColor = Color.FromArgb(65, 65, 65);
                labelBox.BackColor = Color.Black;
                Controls.Add(labelBox);

                // labelbox level z logic.level
                labelBox = new Label();
                    if (i == 0) { labelBox.Text = $"LVL"; }
                try
                {
                    if (i > 0) { labelBox.Text = $"{scoresList[i - 1].Item3}"; }
                }
                catch { labelBox.Text = "- -"; }
                labelBox.Font = new Font("Bauhaus 93", 50);
                labelBox.Location = new Point(1250 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxLVL{i}";
                labelBox.Size = new Size(150, 90);
                labelBox.ForeColor = Color.FromArgb(65, 65, 65);
                labelBox.BackColor = Color.Black;
                Controls.Add(labelBox);

                counter++;
                yOffset+=100;

            }
            music.TetrisMaster();
        }

        public void ScoreScreenUpdater()
        {

            Controls[$"labelBoxNAME{1}"].Text = names[0];
            Controls[$"labelBoxNAME{2}"].Text = names[1];
            Controls[$"labelBoxNAME{3}"].Text = names[2];
        }
    }
}

/*
 * Enable condition ScoreScreenInitilaizer if Enter and logic.gameended. and score is bigger than 10000
 * Clear all controls.
 * If bool scorescreen, execute the ScoreScreenInitilaizer in TickMethod.
 * Create controls of ScoreScreenInitilaizer.
 * Play bacground music scorescreen and let sfx play with each keystroke.
 * Check for keyboard user input and update the labelbox, condition for 6 characters max.
 * Add there option do lete with backspace or delete keyword.
 * If enter, stop checking for user input and change the color of name labelbox, assign the highscore to some genereal class.
 * If next enter clear Controls and call settings screen.
 * 
 */