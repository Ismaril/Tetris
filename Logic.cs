using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tetris
{
    public class Logic
    {
        // ------------------------------------------------------------------------------------------------
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

        // ------------------------------------------------------------------------------------------------
        // FIELDS

        private readonly Music _music;
        private readonly Random _random = new Random();
        private readonly List<byte> _toBeRemoved = new List<byte>();
        private readonly List<byte> _collisionDetection = new List<byte>();
        private readonly List<byte> _indexesOfCompletedRows = new List<byte> { };
        private readonly byte[] _rows = new byte[10];
        private Tetromino _tetrominoCurrent = new Tetromino();
        private byte _linesNextLevel; // has to be 10 in order to continue to next level
        private byte _tetrominoNextIndex = 99; // just a random number which means that we have not yet chosen the next tetromino
        private bool _sendNextPiece = true;
        private bool _putTetrominoOnGrid = true;
        private bool _canMoveRight;
        private bool _canMoveLeft;
        private bool _canRotateRight;
        private bool _canRotateLeft;
        private bool _musicFastIsPlaying;


        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTORS 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="music"></param>
        public Logic(Music music)
        {
            this._music = music;
            for (var i = 0; i < Consts.GRID; i++) Matrix.Add(0);
        }

        // ------------------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Counter which is used to determine when to move tetrominoCurrent down. 
        /// It gets incremented by every GUI_TICK.
        /// Once the timer equals or exceeds the movementTicksBasedOnLevel, the tetrominoCurrent moves down.
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
        /// Counter which holds the score of the player. It gets incremented by every finished row.
        /// </summary>
        public int ScoreIncrementor { get; set; }

        /// <summary>
        /// 4x4 grid which holds the tetromino which will be used in the next round at main matrix. 
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
        /// Flag which controls whether musicslow is playing.
        /// </summary>
        public bool MusicSlowIsPlaying { get; set; }

        /// <summary>
        /// Controls at which row is tetromino during its lifetime on main matrix. 
        /// Row gets always incremented by 1 when tetromino moves down 
        /// (When tetromino moves by 10 index positions on matrix).
        /// </summary>
        public byte CurrentRow { get; set; }

        /// <summary>
        /// Boolean flag which controls if whole game logic should be skipped. 
        /// It gets set to true when the game is paused and other parts of the program like settings,
        /// or score screen are visible.
        /// </summary>
        public bool SkipLogicMain { get; set; }


        // ------------------------------------------------------------------------------------------------
        // METHODS

        /// <summary>
        /// Actualizes the selected matrix indexes to 1 from indexes of tetrominoCurrent object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte offset, bool active)
        {
            if (!active) return;

            // ColumnRelative is number between 0 and 4 which represents the column of tetrominoMatrix.
            // Values between 0 and 3 are actually used to determine the column of tetrominoMatrix.
            // At number 4 the index is reseted to 0.
            byte columnRelative = 0;
            for (byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                // If we are at the end of the row of tetrominoMatrix (4x4 grid, meaning every 4th index),
                // add +10 to offset which will be used at main matrix.
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Consts.ROW_JUMP_GRID;
                }
                var gridIndexOffset = (byte)(offset + columnRelative);

                // Put sub-part of tetromino on grid if the sub-part actually
                // holds some value in tetrominoMatrix.
                if (tetrominoMatrix[i] > 0)
                {
                    Matrix[gridIndexOffset] = tetrominoMatrix[i];
                    _toBeRemoved.Add((byte)(gridIndexOffset));  // These indexes will be later removed.
                }
                columnRelative++;
            }

            // Set indexes below index 20 to zero, so that tetromino is not drawn outside of the matrix,
            // when rotated during first two rows at matrix.
            for (var i = 0; i < 20; i++)
            {
                Matrix[i] = 0;
            }
        }

        /// <summary>
        /// Actualizes the selected matrix indexes to 0.
        /// </summary>
        private void RemoveTetrominoFromGrid()
        {
            foreach (var i in _toBeRemoved)
            {
                Matrix[i] = 0;
            }
        }

        /// <summary>
        /// Checks whether there is a tetrominoCurrent at next row.
        /// If yes, current tetrominoCurrent has to stop on current row.
        /// </summary>
        /// <returns></returns>
        public bool TetrominoHasObstacleAtNextRow()
        {
            if (_toBeRemoved.Count == 0)
                return false;   

            // Check for collision detection only at those indexes,
            // which will not overlap when the tetrominoCurrent moves down.
            for (byte i = 0; i < 4; i++)
            {
                if (!_toBeRemoved.Contains((byte)(_toBeRemoved[i] + (byte)Consts.ROW_JUMP_GRID)))
                    _collisionDetection.Add((byte)_toBeRemoved[i]);
            }

            // Check if at next main matrix row, there are some indexes non zero
            // (there are some former tetrominos).
            for (byte i = 0; i < _collisionDetection.Count; i++)
            {
                if (Matrix[_collisionDetection[i] + Consts.ROW_JUMP_GRID] == 0)
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
        /// This method is able to handle checking for both sides (left and right).
        /// This method is very similar to TetrominoHasObstacleAtNextRow(). 
        /// It is possible to merge these two methods into one. (TODO)
        /// </summary>
        /// <param name="checkRightside">If yes, check movement to right else check movement to left</param>
        /// <returns></returns>
        public bool TetrominoHasNotObstacleAtNextColumn(bool checkRightside)
        {
            if (_toBeRemoved.Count == 0)
                return true;

            sbyte operator_;
            if (checkRightside) operator_ = 1;
            else operator_ = -1;

            // Check for collision detection only at those indexes,
            // which will not overlap when the tetrominoCurrent moves to side.
            for (byte i = 0; i < 4; i++)
            {
                if (!(_toBeRemoved.Contains((byte)(_toBeRemoved[i] + operator_))))
                    _collisionDetection.Add(_toBeRemoved[i]);
            }

            try
            {
                // Check if at next main matrix column,
                // there are some indexes non zero (there are some former tetrominos).
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
                // User moved I-type tetromino on the most upper index to left boundary. This resulted to check
                // index position which is negative. Therefore there is no need for boundary check/
                // meaning no action required, continue.
            }
            
            _collisionDetection.Clear();
            return true;
        }

        /// <summary>
        /// Check whether tetrominoCurrent has obstacle at next rotation.
        /// This method is able to handle checking for both sides (left and right).
        /// </summary>
        /// <param name="checkRightRotation">If yes, check right rotation else check left rotation</param>
        /// <param name="offset">Offset where to start putting indexes of tetrominoMatrix </param>
        /// <returns></returns>
        public bool TetrominoHasNotObstacleAtNextRotation(bool checkRightRotation, byte offset)
        {
            if (_toBeRemoved.Count == 0)
                return true;

            var operator_ = checkRightRotation ? 1 : -1;
            
            // Compute hypothetical rotation of tetrominoCurrent.
            sbyte hypotheticalRotationType = (sbyte)(_tetrominoCurrent.CurrentRotation + operator_);


            switch (hypotheticalRotationType)
            {
                // Adjust rotation type if is out of bounds.
                case -1:
                    hypotheticalRotationType = 3;
                    break;
                case 4:
                    hypotheticalRotationType = 0;
                    break;
            }


            // Put artificially tetrominoMatrixRotated on grid and check if there are no collisions.
            byte[] tetrominoMatrixRotated = _tetrominoCurrent.Rotations[hypotheticalRotationType];
            byte columnRelative = 0;
            for (byte i = 0; i < tetrominoMatrixRotated.Length; i++)
            {
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Consts.ROW_JUMP_GRID;
                }
                var gridIndexOffseted = (byte)(offset + columnRelative);

                if ((tetrominoMatrixRotated[i] > 0 && Matrix[gridIndexOffseted] > 0)
                    // Checks if tetromino would be out off left & right boundaries of matrix
                    || (offset % 10 == 8 || offset % 10 == 7)
                    // Checks if tetromino would be out off left & right boundaries of matrix
                    // but applies for I type tetromino
                    || (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I && offset % 10 == 9))
                    return false;
                columnRelative++;
            }
            return true;
        }

        /// <summary>
        /// Check if tetrominoCurrent is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I)
                return CurrentRow == Consts.LAST_ROW - _tetrominoCurrent.ComputeNrOfBottomPaddingRows() + 1;
            return CurrentRow == Consts.LAST_ROW - _tetrominoCurrent.ComputeNrOfBottomPaddingRows();
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
        /// <param name="canMoveRight">Holds bool whether it is possible to move right.</param>
        /// <param name="desiredRight">Holds bool whether player requested movement to right.</param>
        public void MoveRight_UserEvent(bool canMoveRight, bool desiredRight)
        {
            // Make sure that tetromino does not move out of right boundary of matrix.
            if (_toBeRemoved.Any(item => item % Consts.ROW_JUMP_GRID == 9))
                return;

            if (!canMoveRight || !desiredRight)
                return;

            _tetrominoCurrent.MoveRight();
            _music.MoveToSides();
        }

        /// <summary>
        /// Moves tetromino to left if possible.
        /// </summary>
        /// <param name="canMoveLeft">Holds bool whether it is possible to move left.</param>
        /// <param name="desiredLeft">Holds bool whether player requested movement to left.</param>
        public void MoveLeft_UserEvent(bool canMoveLeft, bool desiredLeft)
        {
            // Make sure that tetromino does not move out of left boundary of matrix.
            if (_toBeRemoved.Any(t => t % Consts.ROW_JUMP_GRID == 0))
                return;

            if (!canMoveLeft || !desiredLeft)
                return;

            _tetrominoCurrent.MoveLeft();
            _music.MoveToSides();
        }

        /// <summary>
        /// Moves tetromino down faster by setting the timer equal to limit. 
        /// By this the game logic does not have to wait for the timer to reach the limit incrementally.
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
        /// <param name="canRotateLeft">Holds bool whether it is possible to rotate left.</param>
        /// <param name="desiredRotationLeft">Holds bool whether player requested rotation to left.</param>
        public void RotateLeft_UserEvent(bool canRotateLeft, bool desiredRotationLeft)
        {
            if (!canRotateLeft || !desiredRotationLeft) 
                return;

            _tetrominoCurrent.RotateLeft();
            _music.Rotate();
        }

        /// <summary>
        /// Rotate tetromino to right if possible.
        /// </summary>
        /// <param name="canRotateRight">Holds bool whether it is possible to rotate right.</param>
        /// <param name="desiredRotationRight">Holds bool whether player requested rotation to right.</param>
        public void RotateRight_UserEvent(bool canRotateRight, bool desiredRotationRight)
        {
            if (!canRotateRight || !desiredRotationRight)
                return;

            _tetrominoCurrent.RotateRight();
            _music.Rotate();
        }

        /// <summary>
        /// Default movement of tetrominoCurrent. It tryes to move tetrominoCurrent down by one row.
        /// </summary>
        private void DefaultTetrominoMovement()
        {
            // Execute only if timer is equal or greater than limit specified for each level.
            if (Timer < LEVEL_SPEED_LIMITS[CurrentLevel])
                return;

            // If tetrominoCurrent has obstacle at next row or is at bottom of grid,
            // leave tetrominoCurrent on current row and chose next tetrominoCurrent.
            if (TetrominoHasObstacleAtNextRow() || TetrominoIsAtBottom())
            {
                if (CurrentRow == 0) RoundEndedFlag = true;
                _sendNextPiece = true;
                CurrentRow = 0;
                _toBeRemoved.Clear();
                _music.Obstacle();
                //music.DisposeMusic(case_: 1);
            }
            // Move tetrominoCurrent down by one row.
            else
            {
                _tetrominoCurrent.MoveDown(rowJumpGrid: Consts.ROW_JUMP_GRID);
                CurrentRow++;
                _putTetrominoOnGrid = true;
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

            // This variable just count how many indxes at the top of matrix are filled with zero
            // values.
            byte checkEmptySpaces = 0;

            // Loops through the upper part of main matrix and checks how many indexes are filled
            // with non zero values (tetrominos are present).
            for (byte i = 0; i < Matrix.Count; i++)
            {
                //Todo: Check why is this block here.
                if (_toBeRemoved.Contains(i))
                    continue;

                switch (i <= Consts.FAST_MUSIC_INDEX)
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

            if (checkEmptySpaces <= Consts.FAST_MUSIC_INDEX)
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

            // If no tetromino was chosen yet (game has just started), chose one based on number 99.
            // Otherwise chose a tetromino which was determined in previous round.
            if (_tetrominoNextIndex == 99)
                _tetrominoCurrent = Tetromino.TETROMINOS[_random.Next(
                    Consts.MIN_NR_OF_TETROMINOS, 
                    Consts.MAX_NR_OF_TETROMINOS
                )];
            else
                _tetrominoCurrent = Tetromino.TETROMINOS[_tetrominoNextIndex];

            // Adjust starting position of tetrominoCurrent based on its type, on playing grid.
            if (_tetrominoCurrent.GetType_ == (byte)Tetromino.Type.I)
                _tetrominoCurrent.Offset = 3;
            else
                _tetrominoCurrent.Offset = 3 + Consts.ROW_JUMP_GRID;

            // This will reset the tetrominoCurrent to its base rotation. Without this,
            // new tetromino which would start at top of grid would have the same rotation as the previous one.
            _tetrominoCurrent.Indexes = _tetrominoCurrent.BaseRotation;
            _tetrominoCurrent.CurrentRotation = 0;

            _sendNextPiece = false;
            _tetrominoNextIndex = (byte)_random.Next(Consts.MIN_NR_OF_TETROMINOS, Consts.MAX_NR_OF_TETROMINOS);
            Array.Copy(Tetromino.TETROMINOS[_tetrominoNextIndex].BaseRotation, TetrominoNext, TetrominoNext.Length);
        }

        /// <summary>
        /// Removes completed rows from main matrix, meaning row which is fully filled with tetrominos.
        /// </summary>
        public void RemoveCompleteRows()
        {
            // Goes row by row through main matrix and checks if all indexes are filled with non zero values.
            for (byte i = 0; i < Consts.HEIGHT_OF_GRID; i++)
            {
                Array.Copy(
                    sourceArray: Matrix.ToArray(),
                    sourceIndex: i * 10,
                    destinationArray: _rows,
                    destinationIndex: 0,
                    length: 10);
                if (_rows.All(x => x > 0)) _indexesOfCompletedRows.Add(i);
            }

            // Skip the rest of the code if no row were completed.
            if (_indexesOfCompletedRows.Count == 0) return;

            // Remove completed rows and insert new empty rows at the top of matrix.
            foreach (byte row in _indexesOfCompletedRows)
            {
                Matrix.RemoveRange(index: row * 10, count: 10);
                Matrix.InsertRange(index: 0, collection: new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }

            ComputeScore((byte)_indexesOfCompletedRows.Count);

            // Play music according to number of cleared lines.
            if (_indexesOfCompletedRows.Count > 0
                && _indexesOfCompletedRows.Count <= 3)
                _music.LineCleared();

            else if (_indexesOfCompletedRows.Count == 4)
                _music.Tetris();

            // This sleep should be replaced by some animation when lines are cleared.
            System.Threading.Thread.Sleep(450);

            // Add numbers to statistic
            _linesNextLevel += (byte)_indexesOfCompletedRows.Count;
            TotalNumberOfClearedLines += (byte)_indexesOfCompletedRows.Count;

            // Clear resources.
            Array.Clear(_rows, 0, _rows.Length);
            _indexesOfCompletedRows.Clear();
        }

        /// <summary>
        /// Checks if the player has reached the next level.
        /// </summary>
        private void CheckIfContinueToNextLevel()
        {
            if (!(_linesNextLevel >= 10))
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
                    ScoreIncrementor += (Consts.SCORE_ONE_LINE * (CurrentLevel + 1));
                    break;
                case 2:
                    ScoreIncrementor += (Consts.SCORE_TWO_LINES * (CurrentLevel + 1));
                    break;
                case 3:
                    ScoreIncrementor += (Consts.SCORE_THREE_LINES * (CurrentLevel + 1));
                    break;
                case 4:
                    ScoreIncrementor += (Consts.SCORE_FOUR_LINES * (CurrentLevel + 1));
                    break;
            }
        }

        /// <summary>
        /// Set desired, move, rotate flags to false.
        /// </summary>
        private void SetTetrominoFlagsToFalse()
        {
            MoveRight = false;
            MoveLeft = false;
            _canMoveLeft = false;
            _canMoveRight = false;
            RotateRight = false;
            RotateLeft = false;
            _canRotateLeft = false;
            _canRotateRight = false;
        }

        /// <summary>
        /// Check for potential collisions of moving tetromino agains already placed 
        /// tetrominos and set flags accordingly.
        /// </summary>
        private void SetCollisionFlags()
        {
            _canMoveRight = TetrominoHasNotObstacleAtNextColumn(checkRightside: true);
            _canMoveLeft = TetrominoHasNotObstacleAtNextColumn(checkRightside: false);
            _canRotateRight = TetrominoHasNotObstacleAtNextRotation(checkRightRotation: true, offset: _tetrominoCurrent.Offset);
            _canRotateLeft = TetrominoHasNotObstacleAtNextRotation(checkRightRotation: false, offset: _tetrominoCurrent.Offset);
        }

        // Todo: Check if this method is needed. It might be possible to reinitialise the class instead.
        /// <summary>
        /// Reset all fields to their default values.
        /// </summary>
        public void ResetAllFields()
        {
            Matrix.Clear();
            for (var i = 0; i < Consts.GRID; i++)
                Matrix.Add(0);
            MoveRight = false;
            MoveLeft = false;
            RotateRight = false;
            RotateLeft = false;
            MoveDownFast = false;
            _canMoveRight = false;
            _canMoveLeft = false;
            _canRotateRight = false;
            _canRotateLeft = false;
            _sendNextPiece = true;
            _putTetrominoOnGrid = true;
            RoundEndedFlag = false;
            _musicFastIsPlaying = false;
            _tetrominoNextIndex = 99;
            Timer = 0;
            CurrentRow = 0;
            ScoreIncrementor = 0;
            _linesNextLevel = 0;
            SkipLogicMain = false;
        }

        /// <summary>
        /// Main method which controls the game logic.
        /// </summary>
        /// <param name="redraw"></param>
        public void Main__(Action<List<byte>> redraw)
        {
            // Todo: There is a merge of tetrominos when tetromino moves diagonally down. Happens both at slow and button down pressed.
            // Todo: I was able to rotate a tetromino out of matrix boundaries. Happend with I type and T type, meaning bbly all are fucked.
            // Todo; It was possible to rotate out of bottom with T type.

            if (SkipLogicMain)
                return;

            _music.DisposeMusic(music: Music.Type.Movement);
            PlayMusicAccordingToFilledGrid();
            RoundEnded();
            SetCollisionFlags();
            RotateRight_UserEvent(_canRotateRight, RotateRight);
            RotateLeft_UserEvent(_canRotateLeft, RotateLeft);
            MoveRight_UserEvent(_canMoveRight, MoveRight);
            MoveLeft_UserEvent(_canMoveLeft, MoveLeft);
            MoveDownFaster_UserEvent(activate: MoveDownFast);
            _toBeRemoved.Clear();
            ChoseNextTetromino();
            PutTetrominoOnGrid(this._tetrominoCurrent.Indexes, _tetrominoCurrent.Offset, _putTetrominoOnGrid);
            DefaultTetrominoMovement();
            redraw(Matrix);
            RemoveTetrominoFromGrid();
            RemoveCompleteRows();
            CheckIfContinueToNextLevel();
            SetTetrominoFlagsToFalse();
        }
    }
}
