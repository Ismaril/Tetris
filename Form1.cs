using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        // -----------------------------------------------------------------------------------------
        // CONSTANTS

        private const byte MAIN_GRID_WIDGET_SIZE = 40;
        private const byte MUSIC_SETTINGS_WIDGET_SIZE = 80;
        private const byte SETTINGS_SIZE = 60;
        private const byte WIDGET_GENERAL_SIZE = 90;

        private const byte PICTURE_BOX_LOCATION = MAIN_GRID_WIDGET_SIZE + MAIN_GRID_WIDGET_SIZE / 10;
        private const byte SETTINGS_LOCATION = SETTINGS_SIZE + SETTINGS_SIZE / 10;

        private const int STATISTICS_WIDTH = 400;
        private const int WINNERS_LVL_WIDTH = 150;
        private const int SCORESCREEN_WIDTH = 650;
        private const int TETRIS_MASTER_WIDTH = 800;
        private const int LEVEL_SETTINGS_WIDTH = 300;
        private const int WINNERS_NAME_WIDTH = 270;
        private const int WINNERS_SCORE_WIDTH = 280;
        private const int MUSIC_SETTING_WIDGET_WIDTH = 500;
        private const int TETRIS_MASTER_HEIGHT = 160;
        private const int STATISTICS_HEIGHT = 150;

        private const int X_STATISTICS = 1228;
        private const int X_MUSIC_SETTING = 920;
        private const int X_MUSIC_SETTING_DESCR = 795;
        private const int X_LEVEL_SETTINGS = 820;
        private const int X_SCORESCREEN = 660;
        private const int X_TETRIS_MASTER = 560;
        private const int X_WINNERS_SEQUENCE = 500;
        private const int X_WINNERS_NAME = 630;
        private const int X_WINNERS_SCORE = 930;
        private const int X_WINNERS_LVL = 1250;

        private const int Y_LEVEL_SETTINGS = 100;
        private const int Y_TOP_SCORE = 87;
        private const int Y_SCORE = 242;
        private const int Y_LEVEL = 660;
        private const int Y_MUSIC_ON = 540;
        private const int Y_MUSIC_SETTING = 450;
        private const int Y_MUSIC_OFF = 626;
        private const int Y_SCORE_SCREEN = 540;
        private const int Y_LINES_CLEARED = 814;
        private const int Y_SCORESCREEN = 100;
        private const int Y_TETRIS_MASTER = 240;


        private const int X_OFFSET_SETTINGS_LABELBOX = 635;
        private const int X_OFFSET_LABELBOX_SCORESCREEN = 20;
        private const int X_OFFSET_NEXT_TETROMINO = 1228;
        private const int X_OFFSET_CENTRE_OF_SCREEN = 740;

        private const int Y_OFFSET_NEXT_TETROMINO = 440;
        private const int Y_OFFSET_SETTINGS = 200;
        
        private static readonly Color COLOR_GRAY = Color.FromArgb(red: 65, green: 65, blue: 65);
        private static readonly Color COLOR_RED = Color.FromArgb(248, 56, 0);
        private static readonly Color COLOR_TRANSPARENT = Color.Transparent;

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
        private const string TEXT_SCORE_SCREEN_CURSOR = "   < >";
        private const string TEXT_NAME = "NAME";
        private const string TEXT_TEXTBOX = "textBox";
        private const string TEXT_LABELBOX = "labelBox";
        private const string TEXT_PICTUREBOX = "pictureBox";
        private const string TEXT_PICTUREBOX_INITIAL_SCREEN = "pictureBoxInitialScreen";

        private const int MINIMUM_HIGH_SCORE_LIMIT = 10_000;
        private const byte INITIAL_SCREEN_VISIBILITY_LIMIT = 100;
        private const byte CHARACTERS_IN_NAME_LIMIT = 6;
        private const byte END_GAME_ANIMATION_COUNTER_LIMIT = 200;

        private const byte NUMBER_OF_SCORES_SCREEN_ROWS = 4;
        private const int COLOR0 = 0;
        private const int COLOR1 = 1;
        private const int COLOR2 = 2;
        private const int COLOR3 = 3;

        // -----------------------------------------------------------------------------------------
        // FIELDS
        private PictureBox _pictureBox;
        private readonly Logic _logic;
        private readonly Music _music = new Music();
        private readonly Timer _timer = new Timer();
        private readonly Sprites _sprites = new Sprites();
        private readonly List<Keys> _pressedKeys = new List<Keys>();

        private Label _labelBox;
        private readonly Label _labelScore = new Label();
        private readonly Label _labelLevel = new Label();
        private readonly Label _labelMusicOn = new Label();
        private readonly Label _labelMusicOff = new Label();
        private readonly Label _labelTopScore = new Label();
        private readonly Label _labelLinesCleared = new Label();
        private readonly Label _labelLevelSetting = new Label();
        private readonly Label _labelMusicSetting = new Label();
        private readonly Label _scoreScreenTetrisMaster = new Label();
        private readonly Label _scoreScreenGratulationTitle = new Label();

        private string _scoreScreenNameHolder = Consts.TEXT_BLANK_SPACE;
        private string _nameAdjusted = "";

        private byte _endGameAnimationCounter;
        private byte _scoreScreenLabelboxIndex;
        private byte _counterInitialScreen;
        private sbyte _currentLevelSetting;

        private int _keyTimer;
        private int _topScore;

        private bool _playMusic = true;
        private bool _scoreScreenDisplayed;
        private bool _initialScreenDisplayed;
        private bool _settingsScreenDisplayed;
        private bool _moveDownAlreadyPressed;
        private bool _rotateAlreadyPressed;
        private bool _moveToSidesAlreadyPressed;
        private bool _rotateRight;
        private bool _rotateLeft;
        private bool _playGame;
        private bool _mainMusicStartedPlaying;
        private bool _logicEndedUserPressedEnter;
        private bool _handledInitialScreen;

        private ScoreRow  FirstScoreBoard = new ScoreRow();
        private ScoreRow  SecondScoreBoard = new ScoreRow();
        private ScoreRow  ThirdScoreBoard = new ScoreRow();


        // -----------------------------------------------------------------------------------------
        // PROPERTIES


        // -----------------------------------------------------------------------------------------
        // CONSTRUCTOR
        public Form1()
        {
            InitializeComponent();
            _logic = new Logic(_music);
        }

        // -----------------------------------------------------------------------------------------
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
        /// from the initial welcome screen to the score screen at the end. It executes 
        /// it's conditions based on corresponding flags.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            // Initial welcome screen (Prague pixel wallpaper)
            if (!_handledInitialScreen)
            {
                WelcomeScreen_Initializer();
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
            }

            // Score screen (Results of players high scores)
            else if (_logic.RoundEndedFlag
                     && _logic.PlayersScore >= MINIMUM_HIGH_SCORE_LIMIT
                     && _scoreScreenDisplayed)
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
        /// This method takes users input from keyboard and updates the corresponding 
        /// label-boxes during display of the score screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreScreenKeyPressEventHandler(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a valid character (not a control key)
            if (char.IsControl(e.KeyChar) || !_scoreScreenDisplayed)
                return;

            // Convert the pressed key to its ASCII character representation
            var pressedChar = e.KeyChar;

            _scoreScreenNameHolder += pressedChar;
            if (_scoreScreenNameHolder.Length > CHARACTERS_IN_NAME_LIMIT)
                _nameAdjusted = _scoreScreenNameHolder.Substring(
                    startIndex: _scoreScreenNameHolder.Length - CHARACTERS_IN_NAME_LIMIT, 
                    length: CHARACTERS_IN_NAME_LIMIT
                    ).ToUpper();
            switch (_scoreScreenLabelboxIndex)
            {
                case 0:
                    FirstScoreBoard.PlayerName = _nameAdjusted;
                    break;
                case 1:
                    SecondScoreBoard.PlayerName = _nameAdjusted;
                    break;
                case 2:
                    ThirdScoreBoard.PlayerName = _nameAdjusted;
                    break;
                case 255:
                    return;
            }
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
        /// This method specifies what happens when the user presses a keyboard key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyArrowsDown(object sender, KeyEventArgs e)
        {
            SetCorrespondingFlagsBasedOnPressedKeyboardKey(e);

            // If player presses enter after he is finished with score-screen,
            // corresponding objects will be reseted.
            if (_scoreScreenDisplayed && e.KeyCode == Keys.Enter)
            {
                ResetObjectsAfterScoreScreenEnded();
            }

            // Settings screen
            else if (
                (!_playGame
                 && _settingsScreenDisplayed
                 && !_scoreScreenDisplayed) 

                || 
                
                (!_settingsScreenDisplayed 
                 && _logicEndedUserPressedEnter)
                )
            {
                Controls[_currentLevelSetting + 1].BackColor = COLOR_GRAY;
                SetLevelDifficultyBasedOnKeypress(e);
                ResetLevelSettingIfOutOfBounds();
                Controls[_currentLevelSetting + 1].BackColor = COLOR_RED;
                EnableDisableMusicBasedOnKeyPress(e);
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
                SetScoreBoardBasedOnPlayersScore();
                ResetObjects();
                ScoreScreen_Initializer();
                _scoreScreenDisplayed = true;

            }

            // Reset corresponding objects and return to settings screen because game ended
            // and player has not reached high score.
            else if (_logic.RoundEndedFlag 
                     && e.KeyCode == Keys.Enter 
                     && _logic.PlayersScore < MINIMUM_HIGH_SCORE_LIMIT)
            {
                ResetObjects();
                _settingsScreenDisplayed = false;
            }

            // Exit game
            if(e.KeyCode == Keys.Escape)
                ExitProgram(e);
        }

        /// <summary>
        /// This method adjusts the score-board based on the score of player.
        /// This function will be called only if the player has reached high score.
        /// </summary>
        private void SetScoreBoardBasedOnPlayersScore()
        {
            // If players score is higher than the first score on the score-board
            if (_logic.PlayersScore >= FirstScoreBoard.Score)
            {
                ThirdScoreBoard.Score = SecondScoreBoard.Score;
                ThirdScoreBoard.Level = SecondScoreBoard.Level;
                ThirdScoreBoard.PlayerName = SecondScoreBoard.PlayerName;

                SecondScoreBoard.Score = FirstScoreBoard.Score;
                SecondScoreBoard.Level = FirstScoreBoard.Level;
                SecondScoreBoard.PlayerName = FirstScoreBoard.PlayerName;

                FirstScoreBoard.Score = _logic.PlayersScore;
                FirstScoreBoard.Level = _logic.CurrentLevel.ToString();
                FirstScoreBoard.PlayerName = TEXT_SCORE_SCREEN_CURSOR;

                _scoreScreenLabelboxIndex = 0;
            }

            // If players score is higher than the second score on the score-board
            else if ( _logic.PlayersScore < FirstScoreBoard.Score
                && _logic.PlayersScore >= SecondScoreBoard.Score)
            {
                ThirdScoreBoard.Score = SecondScoreBoard.Score;
                ThirdScoreBoard.Level = SecondScoreBoard.Level;
                ThirdScoreBoard.PlayerName = SecondScoreBoard.PlayerName;

                SecondScoreBoard.Score = _logic.PlayersScore;
                SecondScoreBoard.Level = _logic.CurrentLevel.ToString();
                SecondScoreBoard.PlayerName = TEXT_SCORE_SCREEN_CURSOR;

                _scoreScreenLabelboxIndex = 1;
            }

            // If players score is higher than the third score on the score-board
            else if(_logic.PlayersScore < SecondScoreBoard.Score
                && _logic.PlayersScore >= ThirdScoreBoard.Score)
            {
                ThirdScoreBoard.Score = _logic.PlayersScore;
                ThirdScoreBoard.Level = _logic.CurrentLevel.ToString();
                ThirdScoreBoard.PlayerName = TEXT_SCORE_SCREEN_CURSOR;

                _scoreScreenLabelboxIndex = 2;
            }
            else
            {
                _scoreScreenLabelboxIndex = 255;
            }
        }

        /// <summary>
        /// Set new top score if players score is higher than previous top score. 
        /// The top score is then displayed during gameplay at GUI, so that the player 
        /// of next round knows what he has to beat.
        /// </summary>
        private void SetTopScore()
        {
            if (_logic.PlayersScore > _topScore)
                _topScore = _logic.PlayersScore;
        }

        /// <summary>
        /// Resets objects to their base values.
        /// </summary>
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

        /// <summary>
        /// Completely shuts down application.
        /// </summary>
        /// <param name="e"></param>
        private static void ExitProgram(KeyEventArgs e) => Application.Exit();


        /// <summary>
        /// Adjust colors of music label-boxes based on whether music is on or off.
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

        /// <summary>
        /// Based on key-up / key-down, either enable or disable music.
        /// </summary>
        /// <param name="e"></param>
        private void EnableDisableMusicBasedOnKeyPress(KeyEventArgs e)
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

        /// <summary>
        /// There are default setting of 30 levels.
        /// Set the level back to 0 or 29 if the number gets incremented or decremented
        /// out of the range.
        /// </summary>
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

        /// <summary>
        /// Based on left / right arrow keypress adjust level (difficulty)
        /// </summary>
        /// <param name="e"></param>
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
            
            // Redraw "next tetromino" matrix.
            // Index in Controls is offseted because there are other controls before.
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
            if (!_logic.SkipLogicMain
                || _endGameAnimationCounter == END_GAME_ANIMATION_COUNTER_LIMIT)
                return;

            for (var i = 0; i < Consts.MAIN_GRID_WIDTH; i++)
                Controls[i + Consts.HIDDEN_UPPER_MAIN_GRID_INDEXES
                           + _endGameAnimationCounter].BackgroundImage 
                    = _sprites.TetrominoBlocks[GetCurrentLevel()][COLOR3];
            _endGameAnimationCounter += Consts.MAIN_GRID_WIDTH;
        }

        /// <summary>
        /// This method creates label-boxes which will hold information about the 
        /// game statistics during game-play.
        /// These label-boxes are created only once and then must be updated in the 
        /// loop by different method.
        /// </summary>
        private void StatisticsLabelBoxes_Initializer()
        {
            // Top Score
            _labelTopScore.Text = $"{TEXT_TOP_SCORE}\n0";
            _labelTopScore.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelTopScore.Location = new Point(x: X_STATISTICS, y: Y_TOP_SCORE);
            _labelTopScore.Name = TEXT_TEXTBOX + TEXT_TOP_SCORE;
            _labelTopScore.Size = new Size(
                STATISTICS_WIDTH, 
                STATISTICS_HEIGHT
                );
            _labelTopScore.ForeColor = COLOR_GRAY;
            _labelTopScore.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelTopScore);

            // Score
            _labelScore.Text = $"{TEXT_SCORE}\n0";
            _labelScore.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelScore.Location = new Point(x: X_STATISTICS, y: Y_SCORE);
            _labelScore.Name = TEXT_TEXTBOX + TEXT_SCORE;
            _labelScore.Size = new Size(
                STATISTICS_WIDTH, 
                STATISTICS_HEIGHT
                );
            _labelScore.ForeColor = COLOR_GRAY;
            _labelScore.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelScore);

            // Level
            _labelLevel.Text = $"{TEXT_LEVEL}\n0";
            _labelLevel.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelLevel.Location = new Point(x: X_STATISTICS, y: Y_LEVEL);
            _labelLevel.Name = TEXT_TEXTBOX + TEXT_LEVEL;
            _labelLevel.Size = new Size(
                STATISTICS_WIDTH,
                STATISTICS_HEIGHT
                );
            _labelLevel.ForeColor = COLOR_GRAY;
            _labelLevel.BackColor = COLOR_TRANSPARENT;
            Controls.Add(_labelLevel);

            // Lines Cleared
            _labelLinesCleared.Text = $"{TEXT_LINES_CLEARED}\n0";
            _labelLinesCleared.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelLinesCleared.Location = new Point(x: X_STATISTICS, y: Y_LINES_CLEARED);
            _labelLinesCleared.Name = TEXT_TEXTBOX + TEXT_LINES_CLEARED;
            _labelLinesCleared.Size = new Size(
                STATISTICS_WIDTH,
                STATISTICS_HEIGHT
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
            // Create 2D matrix at GUI which will be the main play-board.
            byte counter = 0;
            for (var i = 0; i < Consts.MAIN_GRID_HEIGHT; i++)
            {
                for (var j = 0; j < Consts.MAIN_GRID_WIDTH; j++)
                {
                    _pictureBox = new PictureBox();
                    ((ISupportInitialize)(_pictureBox)).BeginInit();
                    SuspendLayout();
                    _pictureBox.Location = new Point(       
                        x:X_OFFSET_CENTRE_OF_SCREEN + j * PICTURE_BOX_LOCATION,
                        y:i * PICTURE_BOX_LOCATION
                        );
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(
                        width:MAIN_GRID_WIDGET_SIZE,
                        height:MAIN_GRID_WIDGET_SIZE
                        );
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
                    _pictureBox.Location = new Point(
                        x: X_OFFSET_NEXT_TETROMINO + j * PICTURE_BOX_LOCATION,
                        y: Y_OFFSET_NEXT_TETROMINO + i * PICTURE_BOX_LOCATION
                        );
                    _pictureBox.Name = $"{TEXT_PICTUREBOX}{counter}";
                    _pictureBox.Size = new Size(
                        width: MAIN_GRID_WIDGET_SIZE, 
                        height: MAIN_GRID_WIDGET_SIZE
                        );
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
        /// It is called only once and then label-boxes should be updated in the loop by 
        /// different method.
        /// </summary>
        public void SettingsScreen_Initializer()
        {
            // Level settings label-box.
            _labelLevelSetting.Text = TEXT_LEVEL_SETTING;
            _labelLevelSetting.Font = new Font(familyName:FONT_BAUHAUS, emSize:FONT_SIZE_BIG);
            _labelLevelSetting.Location = new Point(x: X_LEVEL_SETTINGS, y: Y_LEVEL_SETTINGS);
            _labelLevelSetting.Name = TEXT_LABELBOX + TEXT_LEVEL_SETTING;
            _labelLevelSetting.Size = new Size(
                width: LEVEL_SETTINGS_WIDTH, 
                height: WIDGET_GENERAL_SIZE
                );
            _labelLevelSetting.ForeColor = COLOR_GRAY;
            _labelLevelSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_labelLevelSetting);

            // This loop creates 30 label-boxes which will display level options, each label
            // represented by level number.
            byte counter = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    _labelBox = new Label();
                    _labelBox.Text = $"{counter}";
                    _labelBox.Font = new Font(familyName:FONT_BAUHAUS, emSize:FONT_SIZE_SMALL);
                    _labelBox.Location = new Point(
                        x: X_OFFSET_SETTINGS_LABELBOX + j * SETTINGS_LOCATION,
                        y: Y_OFFSET_SETTINGS + i * SETTINGS_LOCATION);
                    _labelBox.Name = $"{TEXT_LABELBOX}{counter}";
                    _labelBox.Size = new Size(width:SETTINGS_SIZE, height:SETTINGS_SIZE);
                    _labelBox.ForeColor = Consts.COLOR_BLACK;
                    _labelBox.BackColor = i + j == 0 ? COLOR_RED : COLOR_GRAY;
                    Controls.Add(_labelBox);
                    counter++;
                }
            }

            // Music settings description
            _labelMusicSetting.Text = TEXT_MUSIC_SETTING;
            _labelMusicSetting.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
            _labelMusicSetting.Location = new Point(x:X_MUSIC_SETTING_DESCR, y:Y_MUSIC_SETTING);
            _labelMusicSetting.Name = TEXT_LABELBOX + TEXT_MUSIC_SETTING;
            _labelMusicSetting.Size = new Size(
                width:MUSIC_SETTING_WIDGET_WIDTH, 
                height:WIDGET_GENERAL_SIZE
                );
            _labelMusicSetting.ForeColor = COLOR_GRAY;
            _labelMusicSetting.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_labelMusicSetting);


            // Music ON label-box
            _labelMusicOn.Text = TEXT_ON_SETTING;
            _labelMusicOn.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_SMALL);
            _labelMusicOn.Location = new Point(x: X_MUSIC_SETTING, y: Y_MUSIC_ON);
            _labelMusicOn.Name = TEXT_LABELBOX + TEXT_ON_SETTING;
            _labelMusicOn.Size = new Size(
                width: MUSIC_SETTINGS_WIDGET_SIZE,
                height: MUSIC_SETTINGS_WIDGET_SIZE
                );
            _labelMusicOn.ForeColor = Consts.COLOR_BLACK;
            _labelMusicOn.BackColor = COLOR_RED;
            Controls.Add(_labelMusicOn);

            // Music OFF label-box
            _labelMusicOff.Text = TEXT_OFF_SETTING;
            _labelMusicOff.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_SMALL);
            _labelMusicOff.Location = new Point(x: X_MUSIC_SETTING, y: Y_MUSIC_OFF);
            _labelMusicOff.Name = TEXT_LABELBOX + TEXT_OFF_SETTING;
            _labelMusicOff.Size = new Size(
                width: MUSIC_SETTINGS_WIDGET_SIZE,
                height: MUSIC_SETTINGS_WIDGET_SIZE
                );
            _labelMusicOff.ForeColor = Consts.COLOR_BLACK;
            _labelMusicOff.BackColor = COLOR_GRAY;
            Controls.Add(_labelMusicOff);

            _settingsScreenDisplayed = true;
        }

        /// <summary>
        /// This method is called when the game starts and displays the initial 
        /// screen with the Prague pixel wallpaper.
        /// </summary>
        public void WelcomeScreen_Initializer()
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
        /// All widgets in this method are initialized only once and then only updated 
        /// in the loop.
        /// </summary>
        public void ScoreScreen_Initializer()
        {
            _scoreScreenGratulationTitle.Text = TEXT_CONGRATULATIONS;
            _scoreScreenGratulationTitle.Font = new Font(
                familyName:FONT_BAUHAUS, 
                emSize:FONT_SIZE_BIG
                );
            _scoreScreenGratulationTitle.Location = new Point(
                x: X_SCORESCREEN, 
                y: Y_SCORESCREEN
                );
            _scoreScreenGratulationTitle.Name = TEXT_LABELBOX + TEXT_CONGRATULATIONS;
            _scoreScreenGratulationTitle.Size = new Size(
                width: SCORESCREEN_WIDTH, 
                height: WIDGET_GENERAL_SIZE
                );
            _scoreScreenGratulationTitle.ForeColor = COLOR_RED;
            _scoreScreenGratulationTitle.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_scoreScreenGratulationTitle);

            _scoreScreenTetrisMaster.Text = $"{TEXT_TETRIS_MASTER}\n{TEXT_ENTER_YOUR_NAME}";
            _scoreScreenTetrisMaster.Font = new Font(
                familyName: FONT_BAUHAUS, 
                emSize: FONT_SIZE_BIG
                );
            _scoreScreenTetrisMaster.Location = new Point(
                x: X_TETRIS_MASTER, 
                y: Y_TETRIS_MASTER
                );
            _scoreScreenTetrisMaster.Name = TEXT_LABELBOX + TEXT_TETRIS_MASTER;
            _scoreScreenTetrisMaster.Size = new Size(
                width: TETRIS_MASTER_WIDTH, 
                height: TETRIS_MASTER_HEIGHT
                );
            _scoreScreenTetrisMaster.ForeColor = COLOR_GRAY;
            _scoreScreenTetrisMaster.BackColor = Consts.COLOR_BLACK;
            Controls.Add(_scoreScreenTetrisMaster);

            // This loop creates 4 rows. FirstScoreBoard row is only description of the rows.
            // Other 3 rows are intended to be filled with with players data.
            byte counter = 0;
            var yOffset = 0;
            for (byte i = 0; i < NUMBER_OF_SCORES_SCREEN_ROWS; i++)
            {
                // Label-boxes - sequence of winners.
                if (i > 0)
                {
                    _labelBox = new Label();
                    _labelBox.Text = $"{counter}";
                    _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                    _labelBox.Location = new Point(
                        x: X_WINNERS_SEQUENCE + X_OFFSET_LABELBOX_SCORESCREEN, 
                        y: Y_SCORE_SCREEN + yOffset
                        );
                    _labelBox.Name = $"labelBoxRow{i}";
                    _labelBox.Size = new Size(
                        width: WIDGET_GENERAL_SIZE, 
                        height: WIDGET_GENERAL_SIZE
                        );
                    _labelBox.ForeColor = COLOR_GRAY;
                    _labelBox.BackColor = Consts.COLOR_BLACK;
                    Controls.Add(_labelBox);
                }

                // Label-boxes - names of winners.
                _labelBox = new Label();
                switch (i)
                {
                    case 0:
                        _labelBox.Text = TEXT_NAME;
                        break;
                    case 1:
                        _labelBox.Text = FirstScoreBoard.PlayerName;
                        break;
                    case 2:
                        _labelBox.Text = SecondScoreBoard.PlayerName;
                        break;
                    case 3:
                        _labelBox.Text = ThirdScoreBoard.PlayerName;
                        break;
                }
                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(
                    x: X_WINNERS_NAME + X_OFFSET_LABELBOX_SCORESCREEN,
                    y: Y_SCORE_SCREEN + yOffset
                    );
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_NAME}{i}";
                _labelBox.Size = new Size(
                    width: WINNERS_NAME_WIDTH, 
                    height: WIDGET_GENERAL_SIZE
                    );
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Label-boxes - scores of winners.
                _labelBox = new Label();
                switch (i)
                {
                    case 0:
                        _labelBox.Text = TEXT_SCORE;
                        break;
                    case 1:
                        _labelBox.Text = FirstScoreBoard.Score > 0 
                            ? FirstScoreBoard.Score.ToString() : Consts.TEXT_BLANK_SPACE ;
                        break;
                    case 2:
                        _labelBox.Text = SecondScoreBoard.Score > 0 
                            ? SecondScoreBoard.Score.ToString() : Consts.TEXT_BLANK_SPACE;
                        break;
                    case 3:
                        _labelBox.Text = ThirdScoreBoard.Score > 0 
                            ? ThirdScoreBoard.Score.ToString() : Consts.TEXT_BLANK_SPACE;
                        break;
                }
                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(
                    x: X_WINNERS_SCORE + X_OFFSET_LABELBOX_SCORESCREEN,
                    y: Y_SCORE_SCREEN + yOffset
                    );
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_SCORE}{i}";
                _labelBox.Size = new Size(width: WINNERS_SCORE_WIDTH, height: WIDGET_GENERAL_SIZE);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                // Label-boxes - levels of winners.
                _labelBox = new Label();
                switch (i)
                {
                    case 0:
                        _labelBox.Text = TEXT_LEVEL_SHORT;
                        break;
                    case 1:
                        _labelBox.Text = FirstScoreBoard.Level;
                        break;
                    case 2:
                        _labelBox.Text = SecondScoreBoard.Level;
                        break;
                    case 3:
                        _labelBox.Text = ThirdScoreBoard.Level;
                        break;
                }
                _labelBox.Font = new Font(familyName: FONT_BAUHAUS, emSize: FONT_SIZE_BIG);
                _labelBox.Location = new Point(
                    x: X_WINNERS_LVL + X_OFFSET_LABELBOX_SCORESCREEN,
                    y:Y_SCORE_SCREEN + yOffset
                    );
                _labelBox.Name = $"{TEXT_LABELBOX}{TEXT_LEVEL_SHORT}{i}";
                _labelBox.Size = new Size(width: WINNERS_LVL_WIDTH, height: WIDGET_GENERAL_SIZE);
                _labelBox.ForeColor = COLOR_GRAY;
                _labelBox.BackColor = Consts.COLOR_BLACK;
                Controls.Add(_labelBox);

                counter++;
                yOffset += 100;

            }
            _music.TetrisMaster();
        }

        /// <summary>
        /// Updates names to corresponding control label-boxes.
        /// </summary>
        public void ScoreScreen_Updater()
        {
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{1}"].Text = FirstScoreBoard.PlayerName;
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{2}"].Text = SecondScoreBoard.PlayerName;
            Controls[$"{TEXT_LABELBOX}{TEXT_NAME}{3}"].Text = ThirdScoreBoard.PlayerName;
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
        /// Update statistics during game-play. (Label boxes on the right side of the screen)
        /// </summary>
        private void StatisticsLabelBoxes_Updater()
        {
            Controls[TEXT_TEXTBOX + TEXT_TOP_SCORE].Text =
                TEXT_TOP_SCORE
                + $"\n{_topScore}";

            Controls[TEXT_TEXTBOX + TEXT_SCORE].Text =
                TEXT_SCORE
                + $"\n{_logic.PlayersScore}";

            Controls[TEXT_TEXTBOX + TEXT_LEVEL].Text =
                TEXT_LEVEL 
                + $"\n{_logic.CurrentLevel}";

            Controls[TEXT_TEXTBOX + TEXT_LINES_CLEARED].Text =
                TEXT_LINES_CLEARED 
                + $"\n{_logic.TotalNumberOfClearedLines}";
        }

        /// <summary>
        /// This function checks whether to end and dispose initial screen wallpaper
        /// after certain time has passed.
        /// </summary>
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

        /// <summary>
        /// Reset objects to their base value.
        /// </summary>
        private void ResetObjectsAfterScoreScreenEnded()
        {
            ResetObjects();
            _settingsScreenDisplayed = false;
            _scoreScreenDisplayed = false;
            _music.DisposeMusic(music: Music.Type.TetrisMaster);
            _scoreScreenNameHolder = Consts.TEXT_BLANK_SPACE;
        }

        /// <summary>
        /// Resets objects to their base value.
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
