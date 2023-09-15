﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {

        // ------------------------------------------------------------------------------------------------
        // FIELDS
        Logic logic;
        Music music = new Music();
        Timer timer = new Timer();
        PictureBox pictureBox;
        Sprites sprites = new Sprites();
        List<Keys> pressedKeys = new List<Keys>();
        List<(string, int, int)> scoresList = new List<(string, int, int)>();
        Dictionary<int, (string, int, int)> scores = new Dictionary<int, (string, int, int)>();

        Label labelBox;
        Label labelScore = new Label();
        Label labelLevel = new Label();
        Label labelMusicOn = new Label();
        Label labelMusicOff = new Label();
        Label labelTopScore = new Label();
        Label labelLinesCleared = new Label();
        Label labelLevelSetting = new Label();
        Label labelMusicSetting = new Label();
        Label scoreScreenGratulation = new Label();
        Label scoreScreenGratulationTitle = new Label();

        string name = Consts.TEXT_BLANK_SPACE;
        string nameAdjusted = "";
        string[] names = { Consts.TEXT_BLANK_SPACE, Consts.TEXT_BLANK_SPACE, Consts.TEXT_BLANK_SPACE };

        byte shitak = 0;
        byte scoreScreenLabelboxIndex = 1;
        sbyte levelSettingInitial = 0;

        int keyTimer = 0;
        int highScore = 0;
        int counterInitialScreen = 0;

        bool screenUpdaterScreenVisible = false;
        bool moveDownalreadypressed = false;
        bool alreadyPressedRotate = false;
        bool alreadyPressed = false;
        bool rotateRight = false;
        bool rotateLeft = false;
        bool initialScreenDisplayed = false;
        bool settingsScrennDisplayed = false;
        bool playMusic = true;
        bool playGame = false;
        bool mainMusicStartedPlayin = false;
        bool logicEndedUserPressedEnter = false;


        // ------------------------------------------------------------------------------------------------
        // PROPERTIES


        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTOR
        public Form1()
        {
            // Enable double buffering and other optimizations
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer 
                | ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.UserPaint, true);

            InitializeComponent();
            InitialScreen();
            logic = new Logic(music);
        }


        // ------------------------------------------------------------------------------------------------
        // METHODS
        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor.Hide(); // Hide mouse
            FormBorderStyle = FormBorderStyle.None;  // Remove border of the window
            WindowState = FormWindowState.Maximized; // Full screen window mode

            timer.Interval = (int)Consts.GUI_TICK; // How many milliseconds before next tick.
            timer.Tick += new EventHandler(TimerTick);  // What happens when timer ticks.
            KeyDown += new KeyEventHandler(Form1_KeyArrowsDown);  // What happends when keyDown is pressed.
            KeyUp += new KeyEventHandler(Form1_KeyArrowsUp);  // What happends when keyUp is pressed.
            KeyPress += new KeyPressEventHandler(YourKeyPressEventHandler); // What happends when key is pressed.
            KeyPreview = true;
            timer.Start();
        }

        /// <summary>
        /// This method is called based on the length of the Interval. Currently this method will be called 
        /// every 16 milliseconds that means 62.5x per second. It is the main method of the game which handles
        /// every stage of the game, from the initial welcome sceen to the score screen at the end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            // Initial welcome screen (Prague pixel wallpaper)
            if (!initialScreenDisplayed)
            {
                if (counterInitialScreen > 100)
                {
                    initialScreenDisplayed = true;
                    Controls.RemoveByKey("pictureBoxInitialScreen");
                }
                counterInitialScreen++;
            }

            // Settings screen (Audio on/off, level setting)
            else if (!settingsScrennDisplayed)
            {
                SettingsScreen();
            }

            // Main game - Tetris
            else if (playGame && !logicEndedUserPressedEnter)
            {
                if (playMusic && !mainMusicStartedPlayin)
                {
                    music.ChoseMainMusic();
                    logic.MusicSlowIsPlaying = true;
                    music.MainMusicSlow();
                    mainMusicStartedPlayin = true;
                }
                logic.Main__(Redraw);
                logic.Timer += Consts.GUI_TICK;
                keyTimer += 1;

                // Update statistics during gamplay. (Label boxes on the right side of the screen)
                Controls[Consts.TEXT_TEXTBOX + Consts.TEXT_TOP_SCORE].Text =
                    $"{Consts.TEXT_TOP_SCORE}" +
                    $"\n{highScore}";

                Controls[Consts.TEXT_TEXTBOX + Consts.TEXT_SCORE].Text =
                    $"{Consts.TEXT_SCORE}" +
                    $"\n{logic.ScoreIncrementor}";

                Controls[Consts.TEXT_TEXTBOX + Consts.TEXT_LEVEL].Text =
                    $"{Consts.TEXT_LEVEL}" +
                    $"\n{logic.CurrentLevel}";

                Controls[Consts.TEXT_TEXTBOX + Consts.TEXT_LINES_CLEARED].Text =
                    $"{Consts.TEXT_LINES_CLEARED}" +
                    $"\n{logic.TotalNumberOfClearedLines}";

                // Disable the "fast movement down" of tetromino when next tetromino 
                // appears on the screen???
                if (logic.CurrentRow < 1) moveDownalreadypressed = true;
                else if (keyTimer % 2 == 0) moveDownalreadypressed = false;

                if (keyTimer % 5 == 0)
                {
                    alreadyPressed = false;
                    keyTimer = 0;
                }

                if (pressedKeys.Contains(Keys.Right) && !alreadyPressed)
                { 
                    logic.MoveRight = true;
                    alreadyPressed = true;
                    keyTimer = 0; 
                }
                else if (pressedKeys.Contains(Keys.Left) && !alreadyPressed)
                { 
                    logic.MoveLeft = true;
                    alreadyPressed = true;
                    keyTimer = 0; 
                }

                if (pressedKeys.Contains(Keys.Down) && !moveDownalreadypressed) 
                { 
                    logic.MoveDownFast = true; 
                    moveDownalreadypressed = true; 
                }

                if (rotateLeft && !alreadyPressedRotate) 
                { 
                    logic.RotateLeft = true; 
                    alreadyPressedRotate = true; 
                    rotateLeft = false; 
                }
                else if (rotateRight && !alreadyPressedRotate) 
                { 
                    logic.RotateRight = true; 
                    alreadyPressedRotate = true; 
                    rotateRight = false; 
                }
                GameEnded();
                //music.GetPositionOfMainMusic();
            }

            // Score screen (Results of players high scores)
            else if (logic.RoundEndedFlag
                && logic.ScoreIncrementor >= Consts.MINIMUM_HIGH_SCORE
                && screenUpdaterScreenVisible)
            {
                ScoreScreenUpdater();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (name.Length > 6) nameAdjusted = name.Substring(startIndex: name.Length - 6, 6);
                names[scoreScreenLabelboxIndex - 1] = nameAdjusted;
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
            else if (!pressedKeys.Contains(e.KeyCode))
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
                    //music.DisposeBackgroundMusic_NAudio();
                    music.DisposeMusic(5);
                    name = Consts.TEXT_BLANK_SPACE;
                }

            }
            else if ((settingsScrennDisplayed && !playGame) || (!settingsScrennDisplayed && logicEndedUserPressedEnter))
            {
                Controls[levelSettingInitial + 1].BackColor = Consts.COLOR_GRAY;
                if (e.KeyCode == Keys.Right) { levelSettingInitial++; music.SoundSettings(); }
                else if (e.KeyCode == Keys.Left) { levelSettingInitial--; music.SoundSettings(); }
                if (levelSettingInitial == 30) levelSettingInitial = 0;
                else if (levelSettingInitial == -1) levelSettingInitial = 29;
                Controls[levelSettingInitial + 1].BackColor = Consts.COLOR_RED;


                if (e.KeyCode == Keys.Up) { playMusic = !playMusic; music.SoundSettings(); }
                if (e.KeyCode == Keys.Down) { playMusic = !playMusic; music.SoundSettings(); }
                if (playMusic)
                {
                    Controls[32].BackColor = Consts.COLOR_RED; ;
                    Controls[33].BackColor = Consts.COLOR_GRAY;
                }
                else
                {
                    Controls[32].BackColor = Consts.COLOR_GRAY;
                    Controls[33].BackColor = Consts.COLOR_RED;
                }
                logic.CurrentLevel = (byte)levelSettingInitial;
                logic.PlayMusic = playMusic;
                music.MusicIsAllowed = playMusic;

                if (e.KeyCode == Keys.Enter)
                {
                    if (logic.RoundEndedFlag)
                    {
                        logic.ResetAllFields();

                        shitak = 0;
                    }

                    Controls.Clear();
                    playGame = true;
                    logicEndedUserPressedEnter = false;

                    logic.CurrentLevel = (byte)levelSettingInitial;
                    logic.PlayMusic = playMusic;
                    DrawGrid();
                    DrawStatisticsLabelBoxes();
                }


            }
            else if (logic.RoundEndedFlag && e.KeyCode == Keys.Enter && logic.ScoreIncrementor >= 10_000)
            {
                if (logic.ScoreIncrementor > highScore)
                {
                    highScore = logic.ScoreIncrementor;
                }
                scores[logic.ScoreIncrementor] = (Consts.TEXT_BLANK_SPACE, logic.ScoreIncrementor, logic.CurrentLevel);
                scoresList = scores.Values.ToList();
                scoresList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

                if (logic.ScoreIncrementor >= scoresList[0].Item2)
                {
                    scoreScreenLabelboxIndex = 1;
                    names[2] = names[1];
                    names[1] = names[0];
                    names[0] = Consts.TEXT_BLANK_SPACE;

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
            else if (logic.RoundEndedFlag && e.KeyCode == Keys.Enter && logic.ScoreIncrementor < 10_000)
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
            // Redraw main game matrix
            for (byte i = 0; i < matrix.Count; i++)
            {
                switch (matrix[i])
                {
                    case 0:
                        if (i < 20 || i >= 220) Controls[i].BackgroundImage = Consts.OFFGRID_COLOR;
                        else Controls[i].BackgroundImage = Consts.GRID_COLOR;
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

            // Redraw "next tetromino" matrix
            for (int i = 0; i < 16; i++)
            {
                Controls[Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID + i].BackgroundImage = Consts.OFFGRID_COLOR;
                switch (logic.TetrominoNext[i])
                {
                    case 1:
                        Controls[Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][0];
                        break;
                    case 2:
                        Controls[Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][1];
                        break;
                    case 3:
                        Controls[Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID + i].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][2];
                        break;
                }
            }

            // Labels have to be redrawn unfortunatelly here to avoid flickering when the appear for the first time on the screen.
            // When was this done in the definition of label it flickered as sentence above.
            Controls[256].ForeColor = Consts.COLOR_GRAY;
            Controls[256].BackColor = Consts.COLOR_TRANSPARENT;
            Controls[257].ForeColor = Consts.COLOR_GRAY;
            Controls[257].BackColor = Consts.COLOR_TRANSPARENT;
            Controls[258].ForeColor = Consts.COLOR_GRAY;
            Controls[258].BackColor = Consts.COLOR_TRANSPARENT;
            Controls[259].ForeColor = Consts.COLOR_GRAY;
            Controls[259].BackColor = Consts.COLOR_TRANSPARENT;

        }

        private void GameEnded()
        {
            if (!logic.SkipLogicMain || shitak == 200) return;

            for (int i = 0; i < 10; i++)
            {
                Controls[i + 20 + shitak].BackgroundImage = sprites.TetrominoSpriteCollection[logic.CurrentLevel % 10][3];
            }
            shitak += 10;
        }


        private void DrawStatisticsLabelBoxes()
        {

            // Top Score
            labelTopScore.Text = $"{Consts.TEXT_TOP_SCORE}\n0";
            labelTopScore.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelTopScore.Location = new Point(Consts.X_STATISTICS_LABEL_BOX, 87);
            labelTopScore.Name = Consts.TEXT_TEXTBOX + Consts.TEXT_TOP_SCORE;
            labelTopScore.Size = new Size(Consts.WIDTH_OF_STATISTICS_LABEL_BOX, Consts.HEIGHT_OF_STATISTICS_LABEL_BOX);
            Controls.Add(labelTopScore);

            // Score
            labelScore.Text = $"{Consts.TEXT_SCORE}\n0";
            labelScore.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelScore.Location = new Point(Consts.X_STATISTICS_LABEL_BOX, 242);
            labelScore.Name = Consts.TEXT_TEXTBOX + Consts.TEXT_SCORE;
            labelScore.Size = new Size(Consts.WIDTH_OF_STATISTICS_LABEL_BOX, Consts.HEIGHT_OF_STATISTICS_LABEL_BOX);
            Controls.Add(labelScore);

            // Level
            labelLevel.Text = $"{Consts.TEXT_LEVEL}\n0";
            labelLevel.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelLevel.Location = new Point(Consts.X_STATISTICS_LABEL_BOX, 660);
            labelLevel.Name = Consts.TEXT_TEXTBOX + Consts.TEXT_LEVEL;
            labelLevel.Size = new Size(Consts.WIDTH_OF_STATISTICS_LABEL_BOX, Consts.HEIGHT_OF_STATISTICS_LABEL_BOX);
            Controls.Add(labelLevel);

            // Lines Cleared
            labelLinesCleared.Text = $"{Consts.TEXT_LINES_CLEARED}\n0";
            labelLinesCleared.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelLinesCleared.Location = new Point(Consts.X_STATISTICS_LABEL_BOX, 814);
            labelLinesCleared.Name = Consts.TEXT_TEXTBOX + Consts.TEXT_LINES_CLEARED;
            labelLinesCleared.Size = new Size(Consts.WIDTH_OF_STATISTICS_LABEL_BOX, Consts.HEIGHT_OF_STATISTICS_LABEL_BOX);
            Controls.Add(labelLinesCleared);
        }

        public void DrawGrid()
        {
            // Create 2D matrix at GUI.
            byte counter = 0;
            for (int i = 0; i < Consts.HEIGHT_OF_GRID; i++)
            {
                for (int j = 0; j < Consts.WIDTH_OF_GRID; j++)
                {
                    pictureBox = new PictureBox();
                    ((ISupportInitialize)(pictureBox)).BeginInit();
                    SuspendLayout();
                    pictureBox.Location = new Point(
                        Consts.OFFSET_CENTRE_OF_SCREEN + j * Consts.PICTURE_BOX_LOCATION,
                        i * Consts.PICTURE_BOX_LOCATION
                        );
                    pictureBox.Name = $"{Consts.TEXT_PICTUREBOX}{counter}";
                    pictureBox.Size = new Size(Consts.PICTURE_BOX_SIZE, Consts.PICTURE_BOX_SIZE);
                    pictureBox.TabIndex = 0;
                    pictureBox.TabStop = false;
                    Controls.Add(pictureBox);
                    ((ISupportInitialize)(pictureBox)).EndInit();
                    counter++;
                }
            }

            // Create a GUI matrix which will display next
            //
            //
            //
            // .
            for (byte i = 0; i < 4; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    pictureBox = new PictureBox();
                    ((ISupportInitialize)(pictureBox)).BeginInit();
                    pictureBox.Location = new Point(1228 + j * 44, 440 + i * 44);
                    pictureBox.Name = $"{Consts.TEXT_PICTUREBOX}{counter}";
                    pictureBox.Size = new Size(40, 40);
                    pictureBox.TabIndex = 0;
                    pictureBox.TabStop = false;
                    Controls.Add(pictureBox);
                    ((ISupportInitialize)(pictureBox)).EndInit();
                    counter++;
                }
            }
        }

        public void SettingsScreen()
        {

            // level setting
            labelLevelSetting.Text = Consts.TEXT_LEVEL_SETTING;
            labelLevelSetting.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelLevelSetting.Location = new Point(820, 100);
            labelLevelSetting.Name = Consts.TEXT_LABELBOX + Consts.TEXT_LEVEL_SETTING;
            labelLevelSetting.Size = new Size(300, 90);
            labelLevelSetting.ForeColor = Consts.COLOR_GRAY;
            labelLevelSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(labelLevelSetting);

            byte counter = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    labelBox = new Label();
                    labelBox.Text = $"{counter}";
                    labelBox.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_SMALL);
                    labelBox.Location = new Point(635 + j * 66, i * 66 + 200);
                    labelBox.Name = $"{Consts.TEXT_LABELBOX}{counter}";
                    labelBox.Size = new Size(60, 60);
                    labelBox.ForeColor = Consts.COLOR_BLACK;
                    if (i + j == 0) labelBox.BackColor = Consts.COLOR_RED;
                    else labelBox.BackColor = Consts.COLOR_GRAY;
                    Controls.Add(labelBox);
                    counter++;
                }
            }


            // Music settings description
            labelMusicSetting.Text = Consts.TEXT_MUSIC_SETTING;
            labelMusicSetting.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            labelMusicSetting.Location = new Point(795, 450);
            labelMusicSetting.Name = Consts.TEXT_LABELBOX + Consts.TEXT_MUSIC_SETTING;
            labelMusicSetting.Size = new Size(340, 90);
            labelMusicSetting.ForeColor = Consts.COLOR_GRAY;
            labelMusicSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(labelMusicSetting);

            // Music ON
            labelMusicOn.Text = Consts.TEXT_ON_SETTING;
            labelMusicOn.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_SMALL);
            labelMusicOn.Location = new Point(920, 540);
            labelMusicOn.Name = Consts.TEXT_LABELBOX + Consts.TEXT_ON_SETTING;
            labelMusicOn.Size = new Size(80, 80);
            labelMusicOn.ForeColor = Consts.COLOR_BLACK;
            labelMusicOn.BackColor = Consts.COLOR_RED;
            Controls.Add(labelMusicOn);

            // music off
            labelMusicOff.Text = Consts.TEXT_OFF_SETTING;
            labelMusicOff.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_SMALL);
            labelMusicOff.Location = new Point(920, 626);
            labelMusicOff.Name = Consts.TEXT_LABELBOX + Consts.TEXT_OFF_SETTING;
            labelMusicOff.Size = new Size(80, 80);
            labelMusicOff.ForeColor = Consts.COLOR_BLACK;
            labelMusicOff.BackColor = Consts.COLOR_GRAY;
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
            pictureBox.Size = new Size(Consts.WIDTH_OF_APPLICATION_WINDOW, Consts.HEIGHT_OF_APPLICATION_WINDOW);
            //pictureBox.BackColor = Color.Red;
            this.pictureBox.Image = Image.FromFile(@"..\..\Sprites\hrad1_adjusted_tetris.png");
            Controls.Add(pictureBox);
            ((ISupportInitialize)(pictureBox)).EndInit();
        }

        public void ScoreScreenInitilaizer()
        // this has to be only initialised once, and subsequent changes can be done in loop
        {

            scoreScreenGratulationTitle.Text = Consts.TEXT_CONGRATULATIONS;
            scoreScreenGratulationTitle.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            scoreScreenGratulationTitle.Location = new Point(660, 100);
            scoreScreenGratulationTitle.Name = Consts.TEXT_LABELBOX + Consts.TEXT_CONGRATULATIONS;
            scoreScreenGratulationTitle.Size = new Size(650, 90);
            scoreScreenGratulationTitle.ForeColor = Consts.COLOR_RED;
            scoreScreenGratulationTitle.BackColor = Consts.COLOR_BLACK;
            Controls.Add(scoreScreenGratulationTitle);

            scoreScreenGratulation.Text = $"{Consts.TEXT_TETRIS_MASTER}\n{Consts.TEXT_ENTER_YOUR_NAME}";
            scoreScreenGratulation.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
            scoreScreenGratulation.Location = new Point(560, 240);
            scoreScreenGratulation.Name = Consts.TEXT_LABELBOX + Consts.TEXT_TETRIS_MASTER;
            scoreScreenGratulation.Size = new Size(800, 160);
            scoreScreenGratulation.ForeColor = Consts.COLOR_GRAY;
            scoreScreenGratulation.BackColor = Consts.COLOR_BLACK;
            Controls.Add(scoreScreenGratulation);

            byte counter = 0;
            int yOffset = 0;
            int XoFFSET = 20;
            for (byte i = 0; i < 4; i++)
            {
                // labelbox poradi
                if (i > 0)
                {
                    labelBox = new Label();
                    labelBox.Text = $"{counter}";
                    labelBox.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
                    labelBox.Location = new Point(500 + XoFFSET, 540 + yOffset);
                    labelBox.Name = $"labelBoxRow{i}";
                    labelBox.Size = new Size(90, 90);
                    labelBox.ForeColor = Consts.COLOR_GRAY;
                    labelBox.BackColor = Consts.COLOR_BLACK;
                    Controls.Add(labelBox);
                }

                // labelboxy nazev viteze
                labelBox = new Label();
                if (i == 0) { labelBox.Text = $"NAME"; }
                try
                {

                    if (i > 0) { labelBox.Text = $"{names[i - 1]}"; }
                }
                catch { labelBox.Text = Consts.TEXT_BLANK_SPACE; }

                labelBox.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
                labelBox.Location = new Point(630 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxNAME{i}";
                labelBox.Size = new Size(270, 90);
                labelBox.ForeColor = Consts.COLOR_GRAY;
                labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(labelBox);



                // labelbox skore z logic.score
                labelBox = new Label();
                if (i == 0) { labelBox.Text = $"SCORE"; }
                try
                {
                    if (i > 0) { labelBox.Text = $"{scoresList[i - 1].Item2}"; }
                }
                catch { labelBox.Text = Consts.TEXT_BLANK_SPACE; }

                labelBox.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
                labelBox.Location = new Point(930 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxSCORE{i}";
                labelBox.Size = new Size(280, 90);
                labelBox.ForeColor = Consts.COLOR_GRAY;
                labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(labelBox);

                // labelbox level z logic.level
                labelBox = new Label();
                if (i == 0) { labelBox.Text = $"LVL"; }
                try
                {
                    if (i > 0) { labelBox.Text = $"{scoresList[i - 1].Item3}"; }
                }
                catch { labelBox.Text = Consts.TEXT_BLANK_SPACE_SHORT; }
                labelBox.Font = new Font(Consts.FONT_BAUHAUS, Consts.FONT_SIZE_BIG);
                labelBox.Location = new Point(1250 + XoFFSET, 540 + yOffset);
                labelBox.Name = $"labelBoxLVL{i}";
                labelBox.Size = new Size(150, 90);
                labelBox.ForeColor = Consts.COLOR_GRAY;
                labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(labelBox);

                counter++;
                yOffset += 100;

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
 * 
 */