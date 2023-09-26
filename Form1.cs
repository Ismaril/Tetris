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
        // CONSTANTS

        // Picture box size and location (main game grid)
        private const byte PICTURE_BOX_SIZE = 40;
        private const byte PICTURE_BOX_LOCATION = PICTURE_BOX_SIZE + (PICTURE_BOX_SIZE / 10);

        // Statistics label boxes size and location
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

        // Limits
        private const int MINIMUM_HIGH_SCORE_LIMIT = 10_000;
        private const byte INITIAL_SCREEN_VISIBILITY_LIMIT = 100;
        private const byte LIMIT_OF_CHARACTERS_IN_NAME = 6;
        private byte END_GAME_ANIMATION_COUNTER_LIMIT = 200;



        // ------------------------------------------------------------------------------------------------
        // FIELDS
        private PictureBox _pictureBox;
        private List<(string, int, int)> _scoresList = new List<(string, int, int)>();
        
        private readonly Logic _logic;
        private readonly Music _music = new Music();
        private readonly Timer _timer = new Timer();
        private readonly Sprites _sprites = new Sprites();
        private readonly List<Keys> _pressedKeys = new List<Keys>();
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

        private byte _endGameAnimationCounter;
        private byte _scoreScreenLabelboxIndex = 1;
        private byte _counterInitialScreen;
        private sbyte _currentLevelSetting;

        private int _keyTimer;
        private int _topScore;
        private const int COLOR0 = 0;       
        private const int COLOR1 = 1;
        private const int COLOR2 = 2;
        private const int COLOR3 = 3;

        private bool _playMusic = true;
        private bool _scoreScreenVisible;
        private bool _moveDownAlreadyPressed;
        private bool _rotateAlreadyPressed;
        private bool _moveToSidesAlreadyPressed;
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
            KeyDown += KeyArrowsDown;
            KeyUp += KeyArrowsUp;
            KeyPress += ScoreScreenKeyPressEventHandler;
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
                LogicTimer_Updater();
                KeyboardKeyTimer_Updater();
                StatisticsLabelBoxes_Updater();
                FastTetrominoMovement_Limiter();
                MovementToSidesKeyPress_Limiter();
                RotateKeyPress_Limiter();
                GameEndedAnimation();
                //music.GetPositionOfMainMusic();
            }

            // Score screen (Results of players high scores)
            else if (_logic.RoundEndedFlag
                     && _logic.PlayersScore >= MINIMUM_HIGH_SCORE_LIMIT
                     && _scoreScreenVisible)
            {
                ScoreScreen_Updater();
            }
        }

        /// <summary>
        /// For rotate keys (Z and X) to work properly, they have to be pressed and released in
        /// order for them to work again.
        /// </summary>
        private void RotateKeyPress_Limiter()
        {
            if (_rotateLeft && !_rotateAlreadyPressed)
            {
                _logic.RotateLeft = true;
                _rotateAlreadyPressed = true;
                _rotateLeft = false;
            }
            else if (_rotateRight && !_rotateAlreadyPressed)
            {
                _logic.RotateRight = true;
                _rotateAlreadyPressed = true;
                _rotateRight = false;
            }
        }

        /// <summary>
        /// Limit the speed of the keyboard input based on counter. Without this method
        /// tetromino would move too fast to sides.
        /// </summary>
        private void MovementToSidesKeyPress_Limiter()
        {
            if (_keyTimer % 5 == 0)
            {
                _moveToSidesAlreadyPressed = false;
                _keyTimer = 0;
            }

            if (_pressedKeys.Contains(Keys.Right) && !_moveToSidesAlreadyPressed)
            {
                _logic.MoveRight = true;
                _moveToSidesAlreadyPressed = true;
                _keyTimer = 0;
            }
            else if (_pressedKeys.Contains(Keys.Left) && !_moveToSidesAlreadyPressed)
            {
                _logic.MoveLeft = true;
                _moveToSidesAlreadyPressed = true;
                _keyTimer = 0;
            }
        }

        /// <summary>
        /// Disable the fast movement down of tetromino when next tetromino 
        /// appears on the screen.
        /// </summary>
        private void FastTetrominoMovement_Limiter()
        {
            // Disable fast movement down when tetromino appears on the screen. Applies only
            // when tetromino is at first row.
            if (_logic.CurrentRow < 1)
                _moveDownAlreadyPressed = true;

            // Increased speed of tetromino movement down.
            else if (_keyTimer % 2 == 0)
                _moveDownAlreadyPressed = false;

            // Fast movement allowed if conditions met
            if (!_pressedKeys.Contains(Keys.Down) || _moveDownAlreadyPressed)
                return;
            _logic.MoveDownFast = true;
            _moveDownAlreadyPressed = true;
        }


        /// <summary>
        /// This method takes users input from keyboard and updates the corresponding labelboxes during
        /// display of the score screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreScreenKeyPressEventHandler(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a valid character (not a control key)
            if (char.IsControl(e.KeyChar) || !_scoreScreenVisible)
                return;

            // Convert the pressed key to its ASCII character representation
            var pressedChar = e.KeyChar;

            _scoreScreenNameHolder += pressedChar;
            if (_scoreScreenNameHolder.Length > LIMIT_OF_CHARACTERS_IN_NAME)
                _nameAdjusted = _scoreScreenNameHolder.Substring(
                    startIndex: _scoreScreenNameHolder.Length - LIMIT_OF_CHARACTERS_IN_NAME, 
                    length: LIMIT_OF_CHARACTERS_IN_NAME
                    );

            _names[_scoreScreenLabelboxIndex - 1] = _nameAdjusted;
            _music.SoundSettings();
        }

        /// <summary>
        /// This method specifies what happens when the user releases a keyboard key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyArrowsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X || e.KeyCode == Keys.Z)
            {
                _rotateAlreadyPressed = false;
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
        private void KeyArrowsDown(object sender, KeyEventArgs e)
        {
            SetCorrespondingFlagsBasedOnPressedKeyboardKey(e);

            // If player presses enter after he is finished with scorescreen,
            // corresponding objects will be reseted.
            if (_scoreScreenVisible && e.KeyCode == Keys.Enter)
            {
                ResetObjectsAfterScoreScreenEnded();
            }

            // Settings screen
            else if ((_settingsScreenDisplayed && !_playGame && !_scoreScreenVisible) 
                || (!_settingsScreenDisplayed && _logicEndedUserPressedEnter))
            {
                Controls[_currentLevelSetting + 1].BackColor = COLOR_GRAY;
                SetLevelDifficultyBasedOnKeypress(e);
                ResetLevelSettingIfOutOfBounds();
                Controls[_currentLevelSetting + 1].BackColor = COLOR_RED;
                SetMusicOnOffBasedOnKeyPress(e);
                AdjustColorsOfMusicLabelBoxes();
                _music.MusicIsAllowed = _playMusic;

                if (e.KeyCode == Keys.Enter)
                    ResetObjectsAfterSettingsScreenEnded();
            }

            // Score screen
            else if (_logic.RoundEndedFlag 
                     && e.KeyCode == Keys.Enter 
                     && _logic.PlayersScore >= MINIMUM_HIGH_SCORE_LIMIT)
            {
                SetTopScore();
                SetNameLeadeboardBasedOnScoreLeaderboard();
                ResetObjects();
                ScoreScreen_Initilaizer();
                _scoreScreenVisible = true;

            }

            // Reset cooresponding objects and return to settings screen because game ended and player
            // has not reached high score.
            else if (_logic.RoundEndedFlag 
                     && e.KeyCode == Keys.Enter 
                     && _logic.PlayersScore < MINIMUM_HIGH_SCORE_LIMIT)
            {
                ResetObjects();
                _settingsScreenDisplayed = false;
            }

            ExitProgram(e);
        }

        private void SetNameLeadeboardBasedOnScoreLeaderboard()
        {
            _scores[_logic.PlayersScore] = (TEXT_BLANK_SPACE, _logic.PlayersScore, _logic.CurrentLevel);
            _scoresList = _scores.Values.ToList();
            _scoresList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            if (_logic.PlayersScore >= _scoresList[0].Item2)
            {
                _scoreScreenLabelboxIndex = 1;
                _names[2] = _names[1];
                _names[1] = _names[0];
                _names[0] = TEXT_BLANK_SPACE;
            }

            if (_logic.PlayersScore < _scoresList[0].Item2)
                _scoreScreenLabelboxIndex = 2;

            try
            {
                if (_logic.PlayersScore < _scoresList[1].Item2)
                    _scoreScreenLabelboxIndex = 3;
            }
            catch
            {
            }
        }

        private void SetTopScore()
        {
            if (_logic.PlayersScore > _topScore)
                _topScore = _logic.PlayersScore;
        }

        private void ResetObjectsAfterSettingsScreenEnded()
        {
            if (_logic.RoundEndedFlag)
            {
                _logic.ResetAllFields();
                _endGameAnimationCounter = 0;
            }

            Controls.Clear();
            _playGame = true;
            _logicEndedUserPressedEnter = false;
            _logic.CurrentLevel = (byte)_currentLevelSetting;
            _logic.PlayMusic = _playMusic;
            Grid_Initializer();
            StatisticsLabelBoxes_Initializer();
        }

        private static void ExitProgram(KeyEventArgs e)
        {
            // Exit game
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        /// <summary>
        /// Adjust colors of music labelboxes based on whether music is on or off.
        /// </summary>
        private void AdjustColorsOfMusicLabelBoxes()
        {
            if (_playMusic)
            {
                Controls[TEXT_LABELBOX + TEXT_ON_SETTING].BackColor = COLOR_RED;
                Controls[TEXT_LABELBOX + TEXT_OFF_SETTING].BackColor = COLOR_GRAY;
            }
            else
            {
                Controls[TEXT_LABELBOX + TEXT_ON_SETTING].BackColor = COLOR_GRAY;
                Controls[TEXT_LABELBOX + TEXT_OFF_SETTING].BackColor = COLOR_RED;
            }
        }

        private void SetMusicOnOffBasedOnKeyPress(KeyEventArgs e)
        {
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
        }

        private void ResetLevelSettingIfOutOfBounds()
        {
            switch (_currentLevelSetting)
            {
                case 30: // One index above range
                    ResetLevelToZero();
                    break;
                case -1: // One index below range
                    _currentLevelSetting = 29;
                    break;
            }
        }

        private void SetLevelDifficultyBasedOnKeypress(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    _currentLevelSetting++;
                    _music.SoundSettings();
                    break;
                case Keys.Left:
                    _currentLevelSetting--;
                    _music.SoundSettings();
                    break;
            }
        }
        
        private void SetCorrespondingFlagsBasedOnPressedKeyboardKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    _rotateAlreadyPressed = true;
                    _rotateLeft = true;
                    return;
                case Keys.X:
                    _rotateAlreadyPressed = true;
                    _rotateRight = true;
                    return;
                default:
                {
                    if (!_pressedKeys.Contains(e.KeyCode))
                        _pressedKeys.Add(e.KeyCode);
                    break;
                }
            }
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
                        if (i < Consts.HIDDEN_UPPER_MAIN_GRID_INDEXES 
                            || i >= Consts.HIDDEN_UPPER_MAIN_GRID_INDEXES + Consts.ACTUAL_PLAYING_AREA)
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
            for (var i = 0; i < Tetromino.GRID_SURFACE_AREA; i++)
            {
                Controls[Consts.GRID_SURFACE_AREA + i].BackgroundImage = Sprites.OFFGRID_COLOR;
                switch (_logic.TetrominoNext[i])
                {
                    case 1:
                        Controls[Consts.GRID_SURFACE_AREA + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR0];
                        break;
                    case 2:
                        Controls[Consts.GRID_SURFACE_AREA + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR1];
                        break;
                    case 3:
                        Controls[Consts.GRID_SURFACE_AREA + i].BackgroundImage 
                            = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR2];
                        break;
                }
            }
        }

        /// <summary>
        /// Play this animation when the player lost the game. It fills the game matrix with
        /// special colors.
        /// </summary>
        private void GameEndedAnimation()
        {
            if (!_logic.SkipLogicMain || _endGameAnimationCounter == END_GAME_ANIMATION_COUNTER_LIMIT)
                return;

            for (var i = 0; i < Consts.MAIN_GRID_WIDTH; i++)
                Controls[i + Consts.HIDDEN_UPPER_MAIN_GRID_INDEXES + _endGameAnimationCounter].BackgroundImage 
                    = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR3];
            _endGameAnimationCounter += Consts.MAIN_GRID_WIDTH;
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
            _labelTopScore.Location = new Point(x: X_STATISTICS_LABEL_BOX, y: 87);
            _labelTopScore.Name = TEXT_TEXTBOX + TEXT_TOP_SCORE;
            _labelTopScore.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX, 
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            _labelTopScore.ForeColor = COLOR_GRAY;
            _labelTopScore.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelTopScore);

            // Score
            _labelScore.Text = $"{TEXT_SCORE}\n0";
            _labelScore.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelScore.Location = new Point(x: X_STATISTICS_LABEL_BOX, y: 242);
            _labelScore.Name = TEXT_TEXTBOX + TEXT_SCORE;
            _labelScore.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX, 
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            _labelScore.ForeColor = COLOR_GRAY;
            _labelScore.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelScore);

            // Level
            _labelLevel.Text = $"{TEXT_LEVEL}\n0";
            _labelLevel.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelLevel.Location = new Point(x: X_STATISTICS_LABEL_BOX, y: 660);
            _labelLevel.Name = TEXT_TEXTBOX + TEXT_LEVEL;
            _labelLevel.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX,
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            _labelLevel.ForeColor = COLOR_GRAY;
            _labelLevel.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelLevel);

            // Lines Cleared
            _labelLinesCleared.Text = $"{TEXT_LINES_CLEARED}\n0";
            _labelLinesCleared.Font = new Font(FONT_BAUHAUS, FONT_SIZE_BIG);
            _labelLinesCleared.Location = new Point(x: X_STATISTICS_LABEL_BOX, y: 814);
            _labelLinesCleared.Name = TEXT_TEXTBOX + TEXT_LINES_CLEARED;
            _labelLinesCleared.Size = new Size(
                WIDTH_OF_STATISTICS_LABEL_BOX,
                HEIGHT_OF_STATISTICS_LABEL_BOX
                );
            _labelLinesCleared.ForeColor = COLOR_GRAY;
            _labelLinesCleared.BackColor = COLOR_TRANSPARENT;
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
            for (var i = 0; i < Consts.MAIN_GRID_HEIGHT; i++)
            {
                for (var j = 0; j < Consts.MAIN_GRID_WIDTH; j++)
                {
                    _pictureBox = new PictureBox();
                    ((ISupportInitialize)(_pictureBox)).BeginInit();
                    SuspendLayout();
                    _pictureBox.Location = new Point(       
                        x:OFFSET_CENTRE_OF_SCREEN + j * PICTURE_BOX_LOCATION,
                        y:i * PICTURE_BOX_LOCATION
                        );
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(width:PICTURE_BOX_SIZE, height:PICTURE_BOX_SIZE);
                    _pictureBox.TabIndex = 0;
                    _pictureBox.TabStop = false;
                    Controls.Add(_pictureBox);
                    ((ISupportInitialize)(_pictureBox)).EndInit();
                    counter++;
                }
            }

            // Create a GUI matrix which will display next tetromino.
            for (byte i = 0; i < Tetromino.GRID_WIDTH; i++)
            {
                for (byte j = 0; j < Tetromino.GRID_HEIGHT; j++)
                {
                    _pictureBox = new PictureBox();
                    ((ISupportInitialize)(_pictureBox)).BeginInit();
                    _pictureBox.Location = new Point(x: 1228 + j * 44, y: 440 + i * 44);
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(width: 40, height: 40);
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
            _labelLevelSetting.Font = new Font(familyName:FONT_BAUHAUS, emSize:FONT_SIZE_BIG);
            _labelLevelSetting.Location = new Point(x: 820, y: 100);
            _labelLevelSetting.Name = TEXT_LABELBOX + TEXT_LEVEL_SETTING;
            _labelLevelSetting.Size = new Size(width: 300, height: 90);
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
                    _labelBox.Font = new Font(familyName:FONT_BAUHAUS, emSize:FONT_SIZE_SMALL);
                    _labelBox.Location = new Point(x: 635 + j * 66, y: i * 66 + 200);
                    _labelBox.Name = $"{TEXT_LABELBOX}{counter}";
                    _labelBox.Size = new Size(width:60, height:60);
                    _labelBox.ForeColor = Consts.COLOR_BLACK;
                    _labelBox.BackColor = i + j == 0 ? COLOR_RED : COLOR_GRAY;
                    Controls.Add(_labelBox);
                    counter++;
                }
            }

            // Music settings description
            _labelMusicSetting.Text = TEXT_MUSIC_SETTING;
            _labelMusicSetting.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelMusicSetting.Location = new Point(x:795, y:450);
            _labelMusicSetting.Name = TEXT_LABELBOX + TEXT_MUSIC_SETTING;
            _labelMusicSetting.Size = new Size(width:340, height:90);
            _labelMusicSetting.ForeColor = COLOR_GRAY;
            _labelMusicSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_labelMusicSetting);

            // Music ON labelbox
            _labelMusicOn.Text = TEXT_ON_SETTING;
            _labelMusicOn.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_SMALL);
            _labelMusicOn.Location = new Point(x:920, y: 540);
            _labelMusicOn.Name = TEXT_LABELBOX + TEXT_ON_SETTING;
            _labelMusicOn.Size = new Size(width: 80, height: 80);
            _labelMusicOn.ForeColor = Consts.COLOR_BLACK;
            _labelMusicOn.BackColor = COLOR_RED;
            Controls.Add(_labelMusicOn);

            // Music OFF labelbox
            _labelMusicOff.Text = TEXT_OFF_SETTING;
            _labelMusicOff.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_SMALL);
            _labelMusicOff.Location = new Point(x:920, y: 626);
            _labelMusicOff.Name = TEXT_LABELBOX + TEXT_OFF_SETTING;
            _labelMusicOff.Size = new Size(width: 80, height: 80);
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
            _pictureBox.Location = new Point(x: 0, y: 0);
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
            _scoreScreenGratulationTitle.Font = new Font(familyName:FONT_BAUHAUS, emSize:FONT_SIZE_BIG);
            _scoreScreenGratulationTitle.Location = new Point(x:660, y:100);
            _scoreScreenGratulationTitle.Name = TEXT_LABELBOX + TEXT_CONGRATULATIONS;
            _scoreScreenGratulationTitle.Size = new Size(width: 650, height: 90);
            _scoreScreenGratulationTitle.ForeColor = COLOR_RED;
            _scoreScreenGratulationTitle.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_scoreScreenGratulationTitle);

            _scoreScreenGratulation.Text = $"{TEXT_TETRIS_MASTER}\n{TEXT_ENTER_YOUR_NAME}";
            _scoreScreenGratulation.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _scoreScreenGratulation.Location = new Point(x:560, y:240);
            _scoreScreenGratulation.Name = TEXT_LABELBOX + TEXT_TETRIS_MASTER;
            _scoreScreenGratulation.Size = new Size(width: 800, height: 160);
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
                // Label-boxes - sequence of winners.
                if (i > 0)
                {
                    _labelBox = new Label();
                    _labelBox.Text = $"{counter}";
                    _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                    _labelBox.Location = new Point(x:500 + X_OFFSET, y:540 + yOffset);
                    _labelBox.Name = $"labelBoxRow{i}";
                    _labelBox.Size = new Size(width: 90, height: 90);
                    _labelBox.ForeColor = COLOR_GRAY;
                    _labelBox.BackColor = Consts.COLOR_BLACK;
                    Controls.Add(_labelBox);
                }

                // Todo: refactor these 3 blocks.
                // Label-boxes - names of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_NAME; }
                try
                {

                    if (i > 0) { _labelBox.Text = $"{_names[i - 1]}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE; }

                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(x: 630 + X_OFFSET, y: 540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_NAME}{i}";
                _labelBox.Size = new Size(width: 270, height: 90);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Label-boxes - scores of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_SCORE; }
                try
                {
                    if (i > 0) { _labelBox.Text = $"{_scoresList[i - 1].Item2}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE; }

                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(x: 930 + X_OFFSET, y: 540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_SCORE}{i}";
                _labelBox.Size = new Size(width: 280, height: 90);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Label-boxes - levels of winners.
                _labelBox = new Label();
                if (i == 0) { _labelBox.Text = TEXT_LEVEL_SHORT; }
                try
                {
                    if (i > 0) { _labelBox.Text = $"{_scoresList[i - 1].Item3}"; }
                }
                catch { _labelBox.Text = TEXT_BLANK_SPACE_SHORT; }
                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(x:1250 + X_OFFSET, y:540 + yOffset);
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_LEVEL_SHORT}{i}";
                _labelBox.Size = new Size(width: 150, height: 90);
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
        private void ResetLevelToZero() => _currentLevelSetting = 0;

        /// <summary>
        /// Gets the current level adjusted by modulo.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentLevel() => _logic.CurrentLevel % 10;

        /// <summary>
        /// Update the counter which is used later with other method to limit
        /// the speed of tetromino movement to the sides.
        /// </summary>
        private void KeyboardKeyTimer_Updater() => _keyTimer += 1;

        /// <summary>
        /// Update the logic clock/timer with each tick. Once the timer reaches a certain value,
        /// logic of default tetromino movement down will be executed.
        /// </summary>
        private void LogicTimer_Updater() => _logic.Timer += Consts.GUI_TICK;
        
        /// <summary>
        /// If user wishes to play music, this method will play the music.
        /// </summary>
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
                $"\n{_topScore}";

            Controls[TEXT_TEXTBOX + TEXT_SCORE].Text =
                $"{TEXT_SCORE}" +
                $"\n{_logic.PlayersScore}";

            Controls[TEXT_TEXTBOX + TEXT_LEVEL].Text =
                $"{TEXT_LEVEL}" +
                $"\n{_logic.CurrentLevel}";

            Controls[TEXT_TEXTBOX + TEXT_LINES_CLEARED].Text =
                $"{TEXT_LINES_CLEARED}" +
                $"\n{_logic.TotalNumberOfClearedLines}";
        }

        private void CheckIfDisposeInitialScreen()
        {
            if (_counterInitialScreen > INITIAL_SCREEN_VISIBILITY_LIMIT)
            {
                _handledInitialScreen = true;
                Controls[TEXT_PICTUREBOX_INITIAL_SCREEN].Dispose();
                Controls.RemoveByKey(TEXT_PICTUREBOX_INITIAL_SCREEN);
            }
            _counterInitialScreen++;
        }


        private void ResetObjectsAfterScoreScreenEnded()
        {
            ResetObjects();
            _settingsScreenDisplayed = false;
            _scoreScreenVisible = false;
            _music.DisposeMusic(music: Music.Type.TetrisMaster);
            _scoreScreenNameHolder = TEXT_BLANK_SPACE;
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