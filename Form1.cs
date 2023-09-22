using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        // ------------------------------------------------------------------------------------------------
        // CONSTANTS

        // Tick after how many milliseconds should the graphics update
        private const int GUI_TICK = 16;

        // Picture box size and location (main game grid)
        private const byte PICTURE_BOX_SIZE = 40;
        private const byte PICTURE_BOX_LOCATION = PICTURE_BOX_SIZE + (PICTURE_BOX_SIZE / 10);


        // Statistics label boxes size and location and text
        private const int WIDTH_OF_STATISTICS_LABEL_BOX = 400;
        private const int HEIGHT_OF_STATISTICS_LABEL_BOX = 150;
        private const int X_STATISTICS_LABEL_BOX = 1228;

        // Colors
        private static readonly Color COLOR_GRAY = Color.FromArgb(red: 65, green: 65, blue: 65);
        private static readonly Color COLOR_RED = Color.FromArgb(248, 56, 0);
        private static readonly Color COLOR_TRANSPARENT = Color.Transparent;

        // Offsets
        private const int OFFSET_CENTRE_OF_SCREEN = 740;

        // Texts
        private const string FONT_BAUHAUS = "Bauhaus 93";
        private const float FONT_SIZE_BIG = 50;
        private const float FONT_SIZE_SMALL = 26;

        private const string TEXT_TOP_SCORE = "TOP SCORE";
        private const string TEXT_SCORE = "SCORE";
        private const string TEXT_LEVEL = "LEVEL";
        private const string TEXT_LEVEL_SHORT = "LVL";
        private const string TEXT_LINES_CLEARED = "LINES";
        private const string TEXT_CONGRATULATIONS = "CONGRATULATIONS";
        private const string TEXT_TETRIS_MASTER = "YOU ARE A TETRIS MASTER";
        private const string TEXT_ENTER_YOUR_NAME = "PLEASE ENTER YOUR NAME";
        private const string TEXT_LEVEL_SETTING = "SET LEVEL";
        private const string TEXT_MUSIC_SETTING = "SET MUSIC";
        private const string TEXT_ON_SETTING = "ON";
        private const string TEXT_OFF_SETTING = "OFF";
        private const string TEXT_BLANK_SPACE = "- - - - - - ";
        private const string TEXT_BLANK_SPACE_SHORT = "- -";
        private const string TEXT_NAME = "NAME";
        private const string TEXT_TEXTBOX = "textBox";
        private const string TEXT_LABELBOX = "labelBox";
        private const string TEXT_PICTUREBOX = "pictureBox";
        private const string TEXT_PICTUREBOX_INITIAL_SCREEN = "pictureBoxInitialScreen";

        // ------------------------------------------------------------------------------------------------
        // FIELDS
        private readonly Logic _logic;
        private readonly Music _music = new Music();
        private readonly Timer _timer = new Timer();
        private PictureBox _pictureBox;
        private readonly Sprites _sprites = new Sprites();
        private readonly List<Keys> _pressedKeys = new List<Keys>();
        private List<(string, int, int)> _scoresList = new List<(string, int, int)>();
        private readonly Dictionary<int, (string, int, int)> _scores = new Dictionary<int, (string, int, int)>();

        private Label _labelBox;
        private readonly Label _labelScore = new Label();
        private readonly Label _labelLevel = new Label();
        private readonly Label _labelMusicOn = new Label();
        private readonly Label _labelMusicOff = new Label();
        private readonly Label _labelTopScore = new Label();
        private readonly Label _labelLinesCleared = new Label();
        private readonly Label _labelLevelSetting = new Label();
        private readonly Label _labelMusicSetting = new Label();
        private readonly Label _scoreScreenGratulation = new Label();
        private readonly Label _scoreScreenGratulationTitle = new Label();

        private string _scoreScreenNameHolder = TEXT_BLANK_SPACE;
        private string _nameAdjusted = "";
        private readonly string[] _names = { TEXT_BLANK_SPACE, TEXT_BLANK_SPACE, TEXT_BLANK_SPACE };

        private byte _shitak;
        private byte _scoreScreenLabelboxIndex = 1;
        private byte _counterInitialScreen;
        private sbyte _levelSettingInitial;

        private int _keyTimer;
        private int _highScore;
        private const int COLOR0 = 0;       
        private const int COLOR1 = 1;
        private const int COLOR2 = 2;
        private const int COLOR3 = 3;

        private bool _playMusic = true;
        private bool _scoreScreenVisible;
        private bool _moveDownalreadypressed;
        private bool _alreadyPressedRotate;
        private bool _alreadyPressed;
        private bool _rotateRight;
        private bool _rotateLeft;
        private bool _initialScreenDisplayed;
        private bool _settingsScreenDisplayed;
        private bool _playGame;
        private bool _mainMusicStartedPlaying;
        private bool _logicEndedUserPressedEnter;
        private bool _handledInitialScreen;


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
            _logic = new Logic(_music);
        }


        // ------------------------------------------------------------------------------------------------
        // METHODS
        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor.Hide(); // Hide mouse
            FormBorderStyle = FormBorderStyle.None;  // Remove border of the window
            WindowState = FormWindowState.Maximized; // Full screen window mode

            _timer.Interval = (int)Consts.GUI_TICK; // How many milliseconds before next tick.
            _timer.Tick += TimerTick;  // What happens when timer ticks.
            KeyDown += Form1_KeyArrowsDown;  // What happends when keyDown is pressed.
            KeyUp += Form1_KeyArrowsUp;  // What happends when keyUp is pressed.
            KeyPress += YourKeyPressEventHandler; // What happends when key is pressed.
            KeyPreview = true;
            _timer.Start();
        }

        /// <summary>
        /// This method is called based on the length of the Interval. 
        /// Currently this method will be called every 16 milliseconds that means 62.5x per second. 
        /// It is the main method of the game which handles every stage of the game, 
        /// from the initial welcome sceen to the score screen at the end. It executes it's conditions
        /// based on corresponding flags.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            // Initial welcome screen (Prague pixel wallpaper)
            if (!_handledInitialScreen)
            {
                WelcomeScreen_Initialiser();
                CheckIfDisposeInitialScreen();
            }

            // Settings screen (Audio on/off, level setting)
            else if (!_settingsScreenDisplayed)
            {
                SettingsScreen_Initializer();
            }

            // Main game - Tetris
            else if (_playGame && !_logicEndedUserPressedEnter)
            {
                PlayMusic();
                _logic.Main__(Grid_Updater);
                UpdateLogicTimer();
                UpdateKeyboardKeyTimer();
                StatisticsLabelBoxes_Updater();

                // Disable the "fast movement down" of tetromino when next tetromino 
                // appears on the screen???
                if (_logic.CurrentRow < 1) _moveDownalreadypressed = true;
                else if (_keyTimer % 2 == 0) _moveDownalreadypressed = false;

                if (_keyTimer % 5 == 0)
                {
                    _alreadyPressed = false;
                    _keyTimer = 0;
                }

                if (_pressedKeys.Contains(Keys.Right) && !_alreadyPressed)
                {
                    _logic.MoveRight = true;
                    _alreadyPressed = true;
                    _keyTimer = 0;
                }
                else if (_pressedKeys.Contains(Keys.Left) && !_alreadyPressed)
                {
                    _logic.MoveLeft = true;
                    _alreadyPressed = true;
                    _keyTimer = 0;
                }

                if (_pressedKeys.Contains(Keys.Down) && !_moveDownalreadypressed)
                {
                    _logic.MoveDownFast = true;
                    _moveDownalreadypressed = true;
                }

                if (_rotateLeft && !_alreadyPressedRotate)
                {
                    _logic.RotateLeft = true;
                    _alreadyPressedRotate = true;
                    _rotateLeft = false;
                }
                else if (_rotateRight && !_alreadyPressedRotate)
                {
                    _logic.RotateRight = true;
                    _alreadyPressedRotate = true;
                    _rotateRight = false;
                }
                GameEndedAnimation();
                //music.GetPositionOfMainMusic();
            }

            // Score screen (Results of players high scores)
            else if (
                _logic.RoundEndedFlag
                && _logic.ScoreIncrementor >= Consts.MINIMUM_HIGH_SCORE_LIMIT
                && _scoreScreenVisible
                )
            {
                ScoreScreen_Updater();
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
            if (char.IsControl(e.KeyChar) || !_scoreScreenVisible)
                return;

            // Convert the pressed key to its ASCII character representation
            var pressedChar = e.KeyChar;

            _scoreScreenNameHolder += pressedChar;
            if (_scoreScreenNameHolder.Length > 6)
                _nameAdjusted = _scoreScreenNameHolder.Substring(startIndex: _scoreScreenNameHolder.Length - 6, 6);

            _names[_scoreScreenLabelboxIndex - 1] = _nameAdjusted;
            _music.SoundSettings();
        }

        /// <summary>
        /// This method specifies what happens when the user releases a keyboard key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyArrowsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X || e.KeyCode == Keys.Z)
            {
                _alreadyPressedRotate = false;
                return;
            }

            if (_pressedKeys.Contains(e.KeyCode))
                _pressedKeys.Remove(e.KeyCode);
        }

        /// <summary>
        /// This method speciifies what happens when the user presses a keyboard key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyArrowsDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    _alreadyPressedRotate = true;
                    _rotateLeft = true;
                    return;
                case Keys.X:
                    _alreadyPressedRotate = true;
                    _rotateRight = true;
                    return;
                default:
                {
                    if (!_pressedKeys.Contains(e.KeyCode))
                    {
                        _pressedKeys.Add(e.KeyCode);
                    }

                    break;
                }
            }

            // If player presses enter after he is finished with scorescreen,
            // corresponding objects will be reseted.
            if (_scoreScreenVisible && e.KeyCode == Keys.Enter)
            {
                ResetObjects();
                _settingsScreenDisplayed = false;
                _scoreScreenVisible = false;
                _music.DisposeMusic(music: Music.Type.TetrisMaster);
                _scoreScreenNameHolder = TEXT_BLANK_SPACE;
            }

            // Settings screen
            else if ((_settingsScreenDisplayed && !_playGame && !_scoreScreenVisible) 
                || (!_settingsScreenDisplayed && _logicEndedUserPressedEnter))
            {
                Controls[_levelSettingInitial + 1].BackColor = COLOR_GRAY;

                switch (e.KeyCode)
                {
                    case Keys.Right:
                        _levelSettingInitial++; 
                        _music.SoundSettings();
                        break;
                    case Keys.Left:
                        _levelSettingInitial--; 
                        _music.SoundSettings();
                        break;
                }

                switch (_levelSettingInitial)
                {
                    case 30:
                        ResetLevelToZero();
                        break;
                    case -1:
                        _levelSettingInitial = 29;
                        break;
                }

                Controls[_levelSettingInitial + 1].BackColor = COLOR_RED;


                switch (e.KeyCode)
                {
                    case Keys.Up:
                        _playMusic = !_playMusic;
                        _music.SoundSettings();
                        break;
                    case Keys.Down:
                        _playMusic = !_playMusic; 
                        _music.SoundSettings();
                        break;
                }

                if (_playMusic)
                {
                    Controls[32].BackColor = COLOR_RED; ;
                    Controls[33].BackColor = COLOR_GRAY;
                }
                else
                {
                    Controls[TEXT_LABELBOX + TEXT_ON_SETTING].BackColor = COLOR_GRAY;
                    Controls[TEXT_LABELBOX + TEXT_OFF_SETTING].BackColor = COLOR_RED;
                }
                _logic.CurrentLevel = (byte)_levelSettingInitial;
                _logic.PlayMusic = _playMusic;
                _music.MusicIsAllowed = _playMusic;

                if (e.KeyCode == Keys.Enter)
                {
                    if (_logic.RoundEndedFlag)
                    {
                        _logic.ResetAllFields();
                        _shitak = 0;
                    }

                    Controls.Clear();
                    _playGame = true;
                    _logicEndedUserPressedEnter = false;
                    _logic.CurrentLevel = (byte)_levelSettingInitial;
                    _logic.PlayMusic = _playMusic;
                    Grid_Initializer();
                    StatisticsLabelBoxes_Initializer();
                }
            }

            // Score screen
            else if (_logic.RoundEndedFlag && e.KeyCode == Keys.Enter && _logic.ScoreIncrementor >= 10_000)
            {
                if (_logic.ScoreIncrementor > _highScore)
                {
                    _highScore = _logic.ScoreIncrementor;
                }
                _scores[_logic.ScoreIncrementor] = (
                    TEXT_BLANK_SPACE,
                    _logic.ScoreIncrementor,
                    _logic.CurrentLevel
                    );
                _scoresList = _scores.Values.ToList();
                _scoresList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

                if (_logic.ScoreIncrementor >= _scoresList[0].Item2)
                {
                    _scoreScreenLabelboxIndex = 1;
                    _names[2] = _names[1];
                    _names[1] = _names[0];
                    _names[0] = TEXT_BLANK_SPACE;

                }
                if (_logic.ScoreIncrementor < _scoresList[0].Item2)
                {
                    _scoreScreenLabelboxIndex = 2;
                }
                try
                {
                    if (_logic.ScoreIncrementor < _scoresList[1].Item2)
                    {
                        _scoreScreenLabelboxIndex = 3;
                    }
                }
                catch { }

                ResetObjects();
                ScoreScreen_Initilaizer();
                _scoreScreenVisible = true;

            }

            // Reset cooresponding objects and return to settings screen because game ended and player
            // has not reached high score.
            else if (_logic.RoundEndedFlag && e.KeyCode == Keys.Enter && _logic.ScoreIncrementor < 10_000)
            {
                ResetObjects();
                _settingsScreenDisplayed = false;
            }

            // Exit game
            if (e.KeyCode == Keys.Escape) Application.Exit();

        }

        /// <summary>
        /// Redraw GUI based on numbers at game grid/matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Grid_Updater(List<byte> matrix)
        {
            // Redraw main game matrix
            for (byte i = 0; i < matrix.Count; i++)
            {
                switch (matrix[i])
                {
                    case 0:
                        if (i < 20 || i >= 220)
                            Controls[i].BackgroundImage = Sprites.OFFGRID_COLOR;
                        else
                            Controls[i].BackgroundImage = Sprites.GRID_COLOR;
                        break;
                    case 1:
                        Controls[i].BackgroundImage = 
                            _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR0];
                        break;
                    case 2:
                        Controls[i].BackgroundImage = 
                            _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR1];
                        break;
                    case 3:
                        Controls[i].BackgroundImage = 
                            _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR2];
                        break;
                }
            }

            // GRID = Compute control offset because widgets (Controls) in the "next tetromino" matrix were
            // created after the main game matrix.

            // Redraw "next tetromino" matrix
            for (var i = 0; i < 16; i++)
            {
                Controls[Consts.GRID + i].BackgroundImage = Sprites.OFFGRID_COLOR;
                switch (_logic.TetrominoNext[i])
                {
                    case 1:
                        Controls[Consts.GRID + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR0];
                        break;
                    case 2:
                        Controls[Consts.GRID + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR1];
                        break;
                    case 3:
                        Controls[Consts.GRID + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR2];
                        break;
                }
            }

            // Todo: move it to initialiser method?
            // Labels have to be redrawn unfortunatelly here to avoid flickering when the
            // appear for the first time on the screen.
            // When was this done in the definition of label it flickered as sentence above.
            Controls[256].ForeColor = COLOR_GRAY;
            Controls[256].BackColor = COLOR_TRANSPARENT;
            Controls[257].ForeColor = COLOR_GRAY;
            Controls[257].BackColor = COLOR_TRANSPARENT;
            Controls[258].ForeColor = COLOR_GRAY;
            Controls[258].BackColor = COLOR_TRANSPARENT;
            Controls[259].ForeColor = COLOR_GRAY;
            Controls[259].BackColor = COLOR_TRANSPARENT;

        }

        /// <summary>
        /// Play this animation when the player lost the game. It fills the game matrix with
        /// special colors.
        /// </summary>
        private void GameEndedAnimation()
        {
            if (!_logic.SkipLogicMain || _shitak == 200) return;

            for (var i = 0; i < 10; i++)
            {
                Controls[i + 20 + _shitak].BackgroundImage 
                    = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR3];
            }
            _shitak += 10;
        }

        /// <summary>
        /// This method creates labelboxes which will hold information about the 
        /// game statistics during gameplay.
        /// These labelboxes are created only once and then must be updated in the 
        /// loop by different method.
        /// </summary>
        private void StatisticsLabelBoxes_Initializer()
        {
            // Top Score
            _labelTopScore.Text = $"{TEXT_TOP_SCORE}\n0";
            _labelTopScore.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelTopScore.Location = new Point(X_STATISTICS_LABEL_BOX, 87);
            _labelTopScore.Name = TEXT_TEXTBOX + TEXT_TOP_SCORE;
            _labelTopScore.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX, 
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            Controls.Add(_labelTopScore);

            // Score
            _labelScore.Text = $"{TEXT_SCORE}\n0";
            _labelScore.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelScore.Location = new Point(X_STATISTICS_LABEL_BOX, 242);
            _labelScore.Name = TEXT_TEXTBOX + TEXT_SCORE;
            _labelScore.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX, 
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            Controls.Add(_labelScore);

            // Level
            _labelLevel.Text = $"{TEXT_LEVEL}\n0";
            _labelLevel.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelLevel.Location = new Point(X_STATISTICS_LABEL_BOX, 660);
            _labelLevel.Name = TEXT_TEXTBOX + TEXT_LEVEL;
            _labelLevel.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX,
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            Controls.Add(_labelLevel);

            // Lines Cleared
            _labelLinesCleared.Text = $"{TEXT_LINES_CLEARED}\n0";
            _labelLinesCleared.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelLinesCleared.Location = new Point(X_STATISTICS_LABEL_BOX, 814);
            _labelLinesCleared.Name = TEXT_TEXTBOX + TEXT_LINES_CLEARED;
            _labelLinesCleared.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX,
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            Controls.Add(_labelLinesCleared);
        }

        /// <summary>
        /// This method creates pictureBoxes which will be used as a game grid and to 
        /// display next tetromino. 
        /// These picture boxes are created only once and then must be updated in the 
        /// loop by different method.
        /// </summary>
        public void Grid_Initializer()
        {
            // Create 2D matrix at GUI which will be the main playboard.
            byte counter = 0;
            for (var i = 0; i < Consts.HEIGHT_OF_GRID; i++)
            {
                for (var j = 0; j < Consts.WIDTH_OF_GRID; j++)
                {
                    _pictureBox = new PictureBox();
                    ((ISupportInitialize)(_pictureBox)).BeginInit();
                    SuspendLayout();
                    _pictureBox.Location = new Point(
                        OFFSET_CENTRE_OF_SCREEN + j * PICTURE_BOX_LOCATION,
                        i * PICTURE_BOX_LOCATION
                        );
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(PICTURE_BOX_SIZE, PICTURE_BOX_SIZE);
                    _pictureBox.TabIndex = 0;
                    _pictureBox.TabStop = false;
                    Controls.Add(_pictureBox);
                    ((ISupportInitialize)(_pictureBox)).EndInit();
                    counter++;
                }
            }

            // Create a GUI matrix which will display next tetromino.
            for (byte i = 0; i < 4; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    _pictureBox = new PictureBox();
                    ((ISupportInitialize)(_pictureBox)).BeginInit();
                    _pictureBox.Location = new Point(1228 + j * 44, 440 + i * 44);
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(40, 40);
                    _pictureBox.TabIndex = 0;
                    _pictureBox.TabStop = false;
                    Controls.Add(_pictureBox);
                    ((ISupportInitialize)(_pictureBox)).EndInit();
                    counter++;
                }
            }
        }

        /// <summary>
        /// This method displays the setting options. 
        /// It is called only once and then labelboxes should be updated in the loop by 
        /// different method.
        /// </summary>
        public void SettingsScreen_Initializer()
        {
            // Level settings labelbox.
            _labelLevelSetting.Text = TEXT_LEVEL_SETTING;
            _labelLevelSetting.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelLevelSetting.Location = new Point(820, 100);
            _labelLevelSetting.Name = TEXT_LABELBOX + TEXT_LEVEL_SETTING;
            _labelLevelSetting.Size = new Size(300, 90);
            _labelLevelSetting.ForeColor = COLOR_GRAY;
            _labelLevelSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_labelLevelSetting);

            // This loop creates 30 labelboxes which will display level options, each label
            // represented by level number.
            byte counter = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    _labelBox = new Label();
                    _labelBox.Text = $"{counter}";
                    _labelBox.Font = new Font(FONT_BAUHAUS, FONT_SIZE_SMALL);
                    _labelBox.Location = new Point(635 + j * 66, i * 66 + 200);
                    _labelBox.Name = $"{TEXT_LABELBOX}{counter}";
                    _labelBox.Size = new Size(60, 60);
                    _labelBox.ForeColor = Consts.COLOR_BLACK;
                    _labelBox.BackColor = i + j == 0 ? COLOR_RED : COLOR_GRAY;
                    Controls.Add(_labelBox);
                    counter++;
                }
            }

            // Music settings description
            _labelMusicSetting.Text = TEXT_MUSIC_SETTING;
            _labelMusicSetting.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelMusicSetting.Location = new Point(795, 450);
            _labelMusicSetting.Name = TEXT_LABELBOX + TEXT_MUSIC_SETTING;
            _labelMusicSetting.Size = new Size(340, 90);
            _labelMusicSetting.ForeColor = COLOR_GRAY;
            _labelMusicSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_labelMusicSetting);

            // Music ON labelbox
            _labelMusicOn.Text = TEXT_ON_SETTING;
            _labelMusicOn.Font = new Font(FONT_BAUHAUS, FONT_SIZE_SMALL);
            _labelMusicOn.Location = new Point(920, 540);
            _labelMusicOn.Name = TEXT_LABELBOX + TEXT_ON_SETTING;
            _labelMusicOn.Size = new Size(80, 80);
            _labelMusicOn.ForeColor = Consts.COLOR_BLACK;
            _labelMusicOn.BackColor = COLOR_RED;
            Controls.Add(_labelMusicOn);

            // Music OFF labelbox
            _labelMusicOff.Text = TEXT_OFF_SETTING;
            _labelMusicOff.Font = new Font(FONT_BAUHAUS, FONT_SIZE_SMALL);
            _labelMusicOff.Location = new Point(920, 626);
            _labelMusicOff.Name = TEXT_LABELBOX + TEXT_OFF_SETTING;
            _labelMusicOff.Size = new Size(80, 80);
            _labelMusicOff.ForeColor = Consts.COLOR_BLACK;
            _labelMusicOff.BackColor = COLOR_GRAY;
            Controls.Add(_labelMusicOff);

            _settingsScreenDisplayed = true;
        }

        /// <summary>
        /// This method is called when the game starts and displayes the initial screen with the 
        /// Prague pixel wallpaper.
        /// </summary>
        public void WelcomeScreen_Initialiser()
        {
            if (_initialScreenDisplayed)
                return;

            _pictureBox = new PictureBox();
            ((ISupportInitialize)(_pictureBox)).BeginInit();
            _pictureBox.Location = new Point(0, 0);
            _pictureBox.Name = TEXT_PICTUREBOX_INITIAL_SCREEN;
            _pictureBox.Size = new Size(
                WIDTH_OF_APPLICATION_WINDOW,
                HEIGHT_OF_APPLICATION_WINDOW
            );
            _pictureBox.Image = Image.FromFile(Sprites.WALLPAPER_INITIAL_SCREEN);
            Controls.Add(_pictureBox);
            ((ISupportInitialize)(_pictureBox)).BeginInit();
            _initialScreenDisplayed = true;
        }

        /// <summary>
        /// This method is called when the game ends and the score screen is displayed. 
        /// All widgets in this method are initialised only once and then only updated in the loop.
        /// </summary>
        public void ScoreScreen_Initilaizer()
        {
            _scoreScreenGratulationTitle.Text = TEXT_CONGRATULATIONS;
            _scoreScreenGratulationTitle.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _scoreScreenGratulationTitle.Location = new Point(660, 100);
            _scoreScreenGratulationTitle.Name = TEXT_LABELBOX + TEXT_CONGRATULATIONS;
            _scoreScreenGratulationTitle.Size = new Size(650, 90);
            _scoreScreenGratulationTitle.ForeColor = COLOR_RED;
            _scoreScreenGratulationTitle.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_scoreScreenGratulationTitle);

            _scoreScreenGratulation.Text = $"{TEXT_TETRIS_MASTER}\n{TEXT_ENTER_YOUR_NAME}";
            _scoreScreenGratulation.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _scoreScreenGratulation.Location = new Point(560, 240);
            _scoreScreenGratulation.Name = TEXT_LABELBOX + TEXT_TETRIS_MASTER;
            _scoreScreenGratulation.Size = new Size(800, 160);
            _scoreScreenGratulation.ForeColor = COLOR_GRAY;
            _scoreScreenGratulation.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_scoreScreenGratulation);

            // This loop creates 4 rows. First row is only description of the rows.
            // Other 3 rows are intended to be filled with with players data.
            byte counter = 0;
            var yOffset = 0;
            const int X_OFFSET = 20;
            for (byte i = 0; i < 4; i++)
            {
                // Labelboxe - sequence of winners.
                if (i > 0)
                {
                    _labelBox = new Label();
                    _labelBox.Text = $"{counter}";
                    _labelBox.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
                    _labelBox.Location = new Point(500 + X_OFFSET, 540 + yOffset);
                    _labelBox.Name = $"labelBoxRow{i}";
                    _labelBox.Size = new Size(90, 90);
                    _labelBox.ForeColor = COLOR_GRAY;
                    _labelBox.BackColor = Consts.COLOR_BLACK;
                    Controls.Add(_labelBox);
                }

                // Todo: refactor these 3 blocks.
                // Labelboxes - names of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_NAME; }
                try
                {

                    if (i > 0) { _labelBox.Text = $"{_names[i - 1]}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE; }

                _labelBox.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
                _labelBox.Location = new Point(630 + X_OFFSET, 540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_NAME}{i}";
                _labelBox.Size = new Size(270, 90);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Labelboxes - scores of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_SCORE; }
                try
                {
                    if (i > 0) { _labelBox.Text = $"{_scoresList[i - 1].Item2}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE; }

                _labelBox.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
                _labelBox.Location = new Point(930 + X_OFFSET, 540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_SCORE}{i}";
                _labelBox.Size = new Size(280, 90);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Labelboxes - levels of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_LEVEL_SHORT; }
                try
                {
                    if (i > 0) { _labelBox.Text = $"{_scoresList[i - 1].Item3}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE_SHORT; }
                _labelBox.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
                _labelBox.Location = new Point(1250 + X_OFFSET, 540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_LEVEL_SHORT}{i}";
                _labelBox.Size = new Size(150, 90);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                counter++;
                yOffset += 100;

            }
            _music.TetrisMaster();
        }

        /// <summary>
        /// Assigns a name to corresponding control labelbox.
        /// </summary>
        public void ScoreScreen_Updater()
        {
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{1}"].Text = _names[0];
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{2}"].Text = _names[1];
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{3}"].Text = _names[2];
        }


        /// <summary>
        /// Resets the level setting to zero.
        /// </summary>
        private void ResetLevelToZero() => _levelSettingInitial = 0;

        /// <summary>
        /// Gets the current level adjusted by modulo.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentLevel() => _logic.CurrentLevel % 10;


        private void UpdateKeyboardKeyTimer() => _keyTimer += 1;


        private void UpdateLogicTimer() => _logic.Timer += Consts.GUI_TICK;
        

        private void PlayMusic()
        {
            if (!_playMusic || _mainMusicStartedPlaying)
                return;

            _music.ChoseMainMusic();
            _music.MainMusic();
            _logic.MusicSlowIsPlaying = true;
            _mainMusicStartedPlaying = true;
        }

        /// <summary>
        /// Update statistics during gamplay. (Label boxes on the right side of the screen)
        /// </summary>
        private void StatisticsLabelBoxes_Updater()
        {
            Controls[TEXT_TEXTBOX + TEXT_TOP_SCORE].Text =
                $"{TEXT_TOP_SCORE}" +
                $"\n{_highScore}";

            Controls[TEXT_TEXTBOX + TEXT_SCORE].Text =
                $"{TEXT_SCORE}" +
                $"\n{_logic.ScoreIncrementor}";

            Controls[TEXT_TEXTBOX + TEXT_LEVEL].Text =
                $"{TEXT_LEVEL}" +
                $"\n{_logic.CurrentLevel}";

            Controls[TEXT_TEXTBOX + TEXT_LINES_CLEARED].Text =
                $"{TEXT_LINES_CLEARED}" +
                $"\n{_logic.TotalNumberOfClearedLines}";
        }

        private void CheckIfDisposeInitialScreen()
        {
            if (_counterInitialScreen > Consts.INITIAL_SCRREN_VISIBILITY_LIMIT)
            {
                _handledInitialScreen = true;
                Controls[TEXT_PICTUREBOX_INITIAL_SCREEN].Dispose();
                Controls.RemoveByKey(TEXT_PICTUREBOX_INITIAL_SCREEN);
            }
            _counterInitialScreen++;
        }

        /// <summary>
        /// Resets objects to the state so that it is possible to move to next screen.
        /// </summary>
        private void ResetObjects()
        {
            ResetLevelToZero();
            _playMusic = true;
            _playGame = false;
            Controls.Clear();
            _mainMusicStartedPlaying = false;
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