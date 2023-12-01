using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Logic
    {
        // -----------------------------------------------------------------------------------------
        // CONSTANTS

        // After how many multiples of GUI_TICK should the tetromino move down
        private static readonly int[] LEVEL_SPEED_LIMITS = {
            Consts.GUI_TICK * 48, // Level 0
            Consts.GUI_TICK * 43,
            Consts.GUI_TICK * 38,
            Consts.GUI_TICK * 33,
            Consts.GUI_TICK * 28,
            Consts.GUI_TICK * 23, // Level 5
            Consts.GUI_TICK * 18,
            Consts.GUI_TICK * 13,
            Consts.GUI_TICK * 8,
            Consts.GUI_TICK * 6,
            Consts.GUI_TICK * 5,  // Level 10
            Consts.GUI_TICK * 5,
            Consts.GUI_TICK * 5,
            Consts.GUI_TICK * 4,
            Consts.GUI_TICK * 4,
            Consts.GUI_TICK * 4,  // Level 15
            Consts.GUI_TICK * 3,
            Consts.GUI_TICK * 3,
            Consts.GUI_TICK * 3,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,  // Level 20
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,  // Level 25
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 2,
            Consts.GUI_TICK * 1,  // Level 30
        };

        private const byte MIN_NR_OF_TETROMINOS = 0;
        private const byte MAX_NR_OF_TETROMINOS = 7;
        private const byte NOT_YET_CHOSEN_TETROMINO_INDEX = 255;
        private const byte FAST_MUSIC_INDEX = 89;
        private const int SCORE_ONE_LINE = 40;
        private const int SCORE_TWO_LINES = 100;
        private const int SCORE_THREE_LINES = 300;
        private const int SCORE_FOUR_LINES = 1200;
        private readonly byte[] EMPTY_MAIN_GRID_ROW = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private const byte NUMBER_OF_LINES_TO_NEXT_LEVEL_LIMIT = 10;


        // -----------------------------------------------------------------------------------------
        // FIELDS
        private Tetromino _tetrominoCurrent = new Tetromino();

        private readonly Music _music;
        private readonly Random _random = new Random();
        private readonly List<byte> _toBeRemoved = new List<byte>();
        private readonly List<byte> _collisionDetection = new List<byte>();
        private readonly List<byte> _indexesOfCompletedRows = new List<byte> { };
        private readonly byte[] _rows = new byte[10];
        
        private byte _linesNextLevel; // has to be 10 in order to continue to next level
        private byte _tetrominoNextIndex = NOT_YET_CHOSEN_TETROMINO_INDEX;
        private bool _sendNextPiece = true;
        private bool _musicFastIsPlaying;


        // -----------------------------------------------------------------------------------------
        // CONSTRUCTORS 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="music"></param>
        public Logic(Music music)
        {
            this._music = music;
            for (var i = 0; i < Consts.GRID_SURFACE_AREA; i++)
                Matrix.Add(0);
        }

        // -----------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Counter which is used to determine when to move tetrominoCurrent down. 
        /// It gets incremented by every GUI_TICK.
        /// Once the timer equals or exceeds the movementTicksBasedOnLevel, 
        /// the tetrominoCurrent can move down.
        /// </summary>
        public int Timer { get; set; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move right.
        /// </summary>
        public bool MoveRight { get; set; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move left.
        /// </summary>
        public bool MoveLeft { get; set; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move down faster.
        /// </summary>
        public bool MoveDownFast { get; set; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should rotate right.
        /// </summary>
        public bool RotateRight { get; set; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should rotate left.
        /// </summary>
        public bool RotateLeft { get; set; }

        /// <summary>
        /// Counter which holds the score of the player. It gets incremented by every 
        /// finished row.
        /// </summary>
        public int PlayersScore { get; set; }

        /// <summary>
        /// 4x4 grid which holds the tetromino which will be used in the next round at 
        /// main matrix. 
        /// During current round, this array is used to display the next tetromino.
        /// </summary>
        public byte[] TetrominoNext { get; set; } = new byte[16];

        /// <summary>
        /// Level of the game. It gets incremented by every 10 finished rows. 
        /// It is used to determine the speed of the game.
        /// </summary>
        public byte CurrentLevel { get; set; }

        /// <summary>
        /// Boolean which controls if the game should end. 
        /// It gets set to true when the tetrominoCurrent reaches the top of the grid.
        /// </summary>
        public bool RoundEndedFlag { get; set; }

        /// <summary>
        /// 1D array which holds the main matrix of the game. 
        /// It holds all the tetrominos which were put to the grid.
        /// </summary>
        public List<byte> Matrix { get; set; } = new List<byte>();

        /// <summary>
        /// Holds the number of cleared lines. It gets incremented by every finished row. 
        /// The number is used later by GUI to display the number of all cleared lines.
        /// </summary>
        public int TotalNumberOfClearedLines { get; set; }

        /// <summary>
        /// Flag which controls whether to play music.
        /// </summary>
        public bool PlayMusic { get; set; }

        /// <summary>
        /// Flag which controls whether slow background music is playing.
        /// </summary>
        public bool MusicSlowIsPlaying { get; set; }

        /// <summary>
        /// Controls at which row is tetromino during its lifetime on main matrix. 
        /// Row gets always incremented by 1 when tetromino moves down 
        /// (Tetromino moves by 10 index positions on matrix, when moving down a row).
        /// </summary>
        public byte CurrentRow { get; set; }

        /// <summary>
        /// Boolean flag which controls if whole game logic should be skipped. 
        /// It gets set to true when the game is paused and other parts of the program
        /// like settings, or score screen are visible.
        /// </summary>
        public bool SkipLogicMain { get; set; }


        // -----------------------------------------------------------------------------------------
        // METHODS

        /// <summary>
        /// Actualizes the selected matrix indexes to 1 from indexes of tetrominoCurrent object.
        /// </summary>
        private void PutTetrominoOnGrid()
        {
            byte offsetColumn = 0;
            byte offsetRow = _tetrominoCurrent.Offset;

            for (byte i = 0; i < _tetrominoCurrent.Indexes.Length; i++)
            {
                var offset = ComputeOffset(i, ref offsetColumn, ref offsetRow);

                // Put non zero tetromino indexes at main matrix. (Actual shape of tetromino)
                if (_tetrominoCurrent.Indexes[i] > 0)
                {
                    Matrix[offset] = _tetrominoCurrent.Indexes[i];
                    _toBeRemoved.Add(offset);  // These indexes will be later removed.
                }
                offsetColumn++;
            }

            // With below code tetromino is not drawn outside of the matrix,
            // when rotated during first two rows at matrix.
            for (var i = 0; i < Consts.HIDDEN_UPPER_MAIN_GRID_INDEXES; i++)
                Matrix[i] = 0;
        }

        /// <summary>
        /// Actualizes the selected matrix indexes to 0.
        /// </summary>
        private void RemoveTetrominoFromGrid()
        {
            foreach (byte i in _toBeRemoved)
                Matrix[i] = 0;
        }

        /// <summary>
        /// Checks whether there is a tetromino at next row.
        /// If yes, current tetrominoCurrent has to stop on current row.
        /// </summary>
        public bool TetrominoHasObstacleAtNextRow()
        {
            if (_toBeRemoved.Count == 0)
                return false;

            // Exclude indexes which will overlap with tetrominoCurrent
            // when tetrominoCurrent moves down.
            for (byte i = 0; i < Tetromino.NUMBER_OF_SUBBLOCKS; i++)
                if (!_toBeRemoved.Contains((byte)(_toBeRemoved[i] + Consts.MAIN_GRID_WIDTH)))
                    _collisionDetection.Add(_toBeRemoved[i]);

            // Check if there are no tetrominos at next row.
            for (byte i = 0; i < _collisionDetection.Count; i++)
            {
                if (Matrix[_collisionDetection[i] + Consts.MAIN_GRID_WIDTH] == 0)
                    continue;
                _collisionDetection.Clear();
                return true;
            }
            _collisionDetection.Clear();
            return false;
        }

        /// <summary>
        /// Check if tetrominoCurrent has obstacle at next column.
        /// If yes, tetromino must not move to desired side.
        /// </summary>
        /// <param name="checkRightside">
        /// If yes, check movement to right else check movement to left
        /// </param>
        public bool TetrominoHasNotObstacleAtNextColumn(bool checkRightside)
        {
            if (_toBeRemoved.Count == 0)
                return true;

            var operator_ = checkRightside ? 1 : -1;

            // Exclude indexes which will overlap with tetrominoCurrent
            // when tetrominoCurrent moves to side.
            for (byte i = 0; i < 4; i++)
                if (!(_toBeRemoved.Contains((byte)(_toBeRemoved[i] + operator_))))
                    _collisionDetection.Add(_toBeRemoved[i]);

            try
            {
                // Check if there are no tetrominos at next column.
                for (byte i = 0; i < _collisionDetection.Count; i++)
                {
                    if (Matrix[_collisionDetection[i] + operator_] == 0)
                        continue;
                    _collisionDetection.Clear();
                    return false;
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                // User moved I-type tetromino on the most upper index to left boundary.
                // This resulted to check index position which is negative.
                // Therefore there is no need for boundary check meaning no action required,
                // continue.
            }
            _collisionDetection.Clear();
            return true;
        }

        /// <summary>
        /// Check whether tetrominoCurrent has obstacle at next rotation.
        /// This method is able to handle checking for both sides (left and right).
        /// </summary>
        /// <param name="checkRightRotation">
        /// If yes, check right rotation else check left rotation
        /// </param>
        /// <returns></returns>
        public bool TetrominoHasNotObstacleAtNextRotation(bool checkRightRotation)
        {
            if (_toBeRemoved.Count == 0)
                return true;

            var operator_ = checkRightRotation ? 1 : -1;
            byte offsetRow = _tetrominoCurrent.Offset;

            // Compute hypothetical rotation of tetrominoCurrent.
            var hypotheticalRotationType = (sbyte)(_tetrominoCurrent.CurrentRotation + operator_);
            hypotheticalRotationType = Tetromino.ResetRotation(hypotheticalRotationType);

            // Put artificially tetrominoMatrixRotated on grid and check if there are no collisions.
            byte[] tetrominoMatrixRotated = _tetrominoCurrent.Rotations[hypotheticalRotationType];
            byte offsetColumn = 0;
            for (byte i = 0; i < tetrominoMatrixRotated.Length; i++)
            {
                var offset = ComputeOffset(i, ref offsetColumn, ref offsetRow);

                if ((tetrominoMatrixRotated[i] > 0 && Matrix[offset] > 0)
                    // Checks if tetromino would be out off left & right boundaries of main matrix.
                    || (offsetRow % Consts.MAIN_GRID_WIDTH == 8 || offsetRow % Consts.MAIN_GRID_WIDTH == 7)
                    || (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I
                        && offsetRow % Consts.MAIN_GRID_WIDTH == 9))
                    return false;
                offsetColumn++;
            }
            return true;
        }

        /// <summary>
        /// Helper method for PutTetrominoOnGrid() and RemoveTetrominoFromGrid().
        /// </summary>
        /// <param name="i"></param>
        /// <param name="offsetColumn"></param>
        /// <param name="offsetRow"></param>
        /// <returns></returns>
        private static byte ComputeOffset(byte i, ref byte offsetColumn, ref byte offsetRow)
        {
            if (offsetColumn % Tetromino.GRID_WIDTH == 0 && i > 0)
            {
                offsetColumn = 0;
                offsetRow += (byte)Consts.MAIN_GRID_WIDTH;
            }

            var offset = (byte)(offsetRow + offsetColumn);
            return offset;
        }

        /// <summary>
        /// Check if tetrominoCurrent is at bottom of the main matrix.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I)
                return CurrentRow 
                    == Consts.LAST_ROW - _tetrominoCurrent.ComputeNrOfBottomPaddingRows() + 1;
            return CurrentRow 
                == Consts.LAST_ROW - _tetrominoCurrent.ComputeNrOfBottomPaddingRows();
        }

        /// <summary>
        /// Game lost. This flag makes sure that the game logic is not executed anymore.
        /// </summary>
        private void RoundEnded()
        {
            if (!RoundEndedFlag)
                return;

            _music.GameOver();
            _music.DisposeMusic(music: Music.Type.MainMusic);
            SkipLogicMain = true;
        }

        /// <summary>
        /// Moves tetromino to right if possible.
        /// </summary>
        /// <param name="desiredRight">
        /// Holds bool whether player requested movement to right.
        /// </param>
        public void MoveRight_UserEvent(bool desiredRight)
        {
            // Make sure that tetromino does not move out of right boundary of matrix.
            if (_toBeRemoved.Any(item => item % Consts.MAIN_GRID_WIDTH == 9))
                return;

            if (!desiredRight)
                return;

            if(!TetrominoHasNotObstacleAtNextColumn(checkRightside: true))
                return;

            _tetrominoCurrent.MoveRight();
            _music.MoveToSides();
        }

        /// <summary>
        /// Moves tetromino to left if possible.
        /// </summary>
        /// <param name="desiredLeft">
        /// Holds bool whether player requested movement to left.
        /// </param>
        public void MoveLeft_UserEvent(bool desiredLeft)
        {
            // Make sure that tetromino does not move out of left boundary of matrix.
            if (_toBeRemoved.Any(t => t % Consts.MAIN_GRID_WIDTH == 0))
                return;

            if (!desiredLeft)
                return;

            if (!TetrominoHasNotObstacleAtNextColumn(checkRightside: false))
                return;

            _tetrominoCurrent.MoveLeft();
            _music.MoveToSides();
        }

        /// <summary>
        /// Moves tetromino down faster by setting the timer equal to limit. 
        /// By this the game logic does not have to wait for the timer to reach the limit
        /// incrementally.
        /// </summary>
        /// <param name="activate"></param>
        private void MoveDownFaster_UserEvent(bool activate)
        {
            if (!activate)
                return;

            Timer = LEVEL_SPEED_LIMITS[CurrentLevel];
            MoveDownFast = false;
        }

        /// <summary>
        /// Rotate tetromino to left if possible.
        /// </summary>
        /// <param name="desiredRotationLeft">
        /// Holds bool whether player requested rotation to left.
        /// </param>
        public void RotateLeft_UserEvent(bool desiredRotationLeft)
        {
            if (!desiredRotationLeft) 
                return;

            if(!TetrominoHasNotObstacleAtNextRotation(checkRightRotation: false))
                return;

            _tetrominoCurrent.RotateLeft();
            _music.Rotate();
        }

        /// <summary>
        /// Rotate tetromino to right if possible.
        /// </summary>
        /// <param name="desiredRotationRight">
        /// Holds bool whether player requested rotation to right.
        /// </param>
        public void RotateRight_UserEvent(bool desiredRotationRight)
        {
            if (!desiredRotationRight)
                return;

            if(!TetrominoHasNotObstacleAtNextRotation(checkRightRotation: true))
                return;

            _tetrominoCurrent.RotateRight();
            _music.Rotate();
        }

        /// <summary>
        /// Default movement of tetrominoCurrent.
        /// It tries to move tetrominoCurrent down by one row.
        /// </summary>
        private void DefaultTetrominoMovement()
        {
            if (Timer < LEVEL_SPEED_LIMITS[CurrentLevel])
                return;

            if (TetrominoHasObstacleAtNextRow() || TetrominoIsAtBottom())
            {
                if (CurrentRow == 0)
                    RoundEndedFlag = true;
                _sendNextPiece = true;
                CurrentRow = 0;
                _toBeRemoved.Clear();
                _music.Obstacle();
            }
            else
            {
                _tetrominoCurrent.MoveDown(rowJumpGrid: Consts.MAIN_GRID_WIDTH);
                CurrentRow++;
            }
            Timer = 0;
        }

        /// <summary>
        /// Play music according to how many indexes of matrix are filled. 
        /// When the tetrominos are close to the top, fast music starts playing else slow music.
        /// </summary>
        private void PlayMusicAccordingToFilledGrid()
        {
            if (!PlayMusic) return;

            // This variable just count how many indexes at the top of matrix are filled with zero
            // values.
            byte checkEmptySpaces = 0;

            // Loops through the upper part of main matrix and checks how many indexes are filled
            // with non zero values (tetrominos are present).
            for (byte i = 0; i < Matrix.Count; i++)
            {
                switch (i <= FAST_MUSIC_INDEX)
                {
                    case true when Matrix[i] == 0 && !MusicSlowIsPlaying:
                        checkEmptySpaces++;
                        continue;
                    case true when Matrix[i] > 0 && !_musicFastIsPlaying:
                        _music.MainMusic(playSlowMusic: false);
                        _musicFastIsPlaying = true;
                        MusicSlowIsPlaying = false;
                        return;
                }
            }

            if (checkEmptySpaces <= FAST_MUSIC_INDEX)
                return;

            _music.MainMusic();
            MusicSlowIsPlaying = true;
            _musicFastIsPlaying = false;
        }

        /// <summary>
        /// Prepare one random tetrominoCurrent at the starting position at the top of grid.
        /// </summary>
        public void ChoseNextTetromino()
        {
            if (!_sendNextPiece) 
                return;

            if (_tetrominoNextIndex == NOT_YET_CHOSEN_TETROMINO_INDEX)
                _tetrominoCurrent = Tetromino.TETROMINOS[_random.Next(
                    MIN_NR_OF_TETROMINOS,
                    MAX_NR_OF_TETROMINOS
                    )];
            else
                _tetrominoCurrent = Tetromino.TETROMINOS[_tetrominoNextIndex];

            if (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I)
                _tetrominoCurrent.Offset = 3;
            else
                _tetrominoCurrent.Offset = 3 + Consts.MAIN_GRID_WIDTH;

            // This will reset the tetrominoCurrent to its base rotation. Without this,
            // new tetromino which would start at top of grid would have the same rotation as the
            // previous one.
            _tetrominoCurrent.Indexes = _tetrominoCurrent.BaseRotation;
            _tetrominoCurrent.CurrentRotation = 0;

            _sendNextPiece = false;
            _tetrominoNextIndex = (byte)_random.Next(MIN_NR_OF_TETROMINOS, MAX_NR_OF_TETROMINOS);
            Array.Copy(
                sourceArray: Tetromino.TETROMINOS[_tetrominoNextIndex].BaseRotation,
                destinationArray: TetrominoNext, 
                length: TetrominoNext.Length
                );
        }

        /// <summary>
        /// Removes completed rows from main matrix, meaning row which is fully filled with 
        /// tetrominos.
        /// </summary>
        public void RemoveCompleteRows()
        {
            // Goes row by row through main matrix and checks if all indexes are filled with
            // non zero values.
            for (byte i = 0; i < Consts.MAIN_GRID_HEIGHT; i++)
            {
                Array.Copy(
                    sourceArray: Matrix.ToArray(),
                    sourceIndex: i * Consts.MAIN_GRID_WIDTH,
                    destinationArray: _rows,
                    destinationIndex: 0,
                    length: Consts.MAIN_GRID_WIDTH);
                if (_rows.All(x => x > 0))
                    _indexesOfCompletedRows.Add(i);
            }

            if (_indexesOfCompletedRows.Count == 0)
                return;

            // Remove completed rows and insert new empty rows at the top of matrix.
            foreach (byte row in _indexesOfCompletedRows)
            {
                Matrix.RemoveRange(index: row * Consts.MAIN_GRID_WIDTH, count: Consts.MAIN_GRID_WIDTH);
                Matrix.InsertRange(index: 0, collection: EMPTY_MAIN_GRID_ROW);
            }

            ComputeScore((byte)_indexesOfCompletedRows.Count);

            // Play music according to number of cleared lines.
            if (_indexesOfCompletedRows.Count > 0 && _indexesOfCompletedRows.Count <= 3)
                _music.LineCleared();
            else if (_indexesOfCompletedRows.Count == 4)
                _music.Tetris();

            // This sleep should be replaced by some animation when lines are cleared.
            System.Threading.Thread.Sleep(450);

            // Add numbers to statistic
            _linesNextLevel += (byte)_indexesOfCompletedRows.Count;
            TotalNumberOfClearedLines += (byte)_indexesOfCompletedRows.Count;

            // Clear resources.
            Array.Clear(array: _rows, index: 0, length: _rows.Length);
            _indexesOfCompletedRows.Clear();
        }

        /// <summary>
        /// Checks if the player has reached the next level.
        /// </summary>
        private void CheckIfContinueToNextLevel()
        {
            if (!(_linesNextLevel >= NUMBER_OF_LINES_TO_NEXT_LEVEL_LIMIT))
                return;

            _linesNextLevel = 0;
            CurrentLevel++;
            _music.NextLevel();
        }

        /// <summary>
        /// Computes the score of the player based on number of finished rows.
        /// </summary>
        /// <param name="numberOfFinishedRows"></param>
        private void ComputeScore(byte numberOfFinishedRows)
        {
            switch (numberOfFinishedRows)
            {
                case 0:
                    return;
                // This math formula is taken from original Tetris game.
                case 1:
                    PlayersScore += (SCORE_ONE_LINE * (CurrentLevel + 1));
                    break;
                case 2:
                    PlayersScore += (SCORE_TWO_LINES * (CurrentLevel + 1));
                    break;
                case 3:
                    PlayersScore += (SCORE_THREE_LINES * (CurrentLevel + 1));
                    break;
                case 4:
                    PlayersScore += (SCORE_FOUR_LINES * (CurrentLevel + 1));
                    break;
            }
        }

        /// <summary>
        /// Set desired, move, rotate flags to false.
        /// </summary>
        private void SetTetrominoFlagsFalse()
        {
            MoveLeft = false;
            MoveRight = false;
            RotateLeft = false;
            RotateRight = false;
        }

        /// <summary>
        /// Reset all fields to their default values.
        /// </summary>
        public void ResetAllFields()
        {
            Matrix.Clear();
            
            for (var i = 0; i < Consts.GRID_SURFACE_AREA; i++)
                Matrix.Add(0);

            SkipLogicMain = false;
            RoundEndedFlag = false;
            _musicFastIsPlaying = false;
            SetTetrominoFlagsFalse();
            MoveDownFast = false;
            _sendNextPiece = true;
            Timer = 0;
            CurrentRow = 0;
            PlayersScore = 0;
            TotalNumberOfClearedLines = 0;
            _tetrominoNextIndex = NOT_YET_CHOSEN_TETROMINO_INDEX;
            _linesNextLevel = 0;
        }
        
        /// <summary>
        /// Main method which controls the game logic.
        /// </summary>
        /// <param name="redraw"></param>
        public void Main__(Action<List<byte>> redraw)
        {
            if (SkipLogicMain)
                return;

            _music.DisposeMusic(music: Music.Type.Movement);
            PlayMusicAccordingToFilledGrid();
            RoundEnded();
            RotateRight_UserEvent(RotateRight);
            RotateLeft_UserEvent(RotateLeft);
            MoveRight_UserEvent(MoveRight);
            MoveLeft_UserEvent(MoveLeft);
            MoveDownFaster_UserEvent(activate: MoveDownFast);
            _toBeRemoved.Clear();
            ChoseNextTetromino();
            PutTetrominoOnGrid();
            DefaultTetrominoMovement();
            redraw(Matrix);
            RemoveTetrominoFromGrid();
            RemoveCompleteRows();
            CheckIfContinueToNextLevel();
            SetTetrominoFlagsFalse();
        }
    }
}