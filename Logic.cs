using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Logic
    {
        // Fields ------------------------------------------------------------------------------
        Music music;
        Random random = new Random();
        List<byte> matrix = new List<byte>();
        Tetromino tetrominoCurrent = new Tetromino();
        List<byte> toBeRemoved = new List<byte>();
        List<byte> collisionDetection = new List<byte>();
        byte[] tetrominoNext = new byte[16];
        
        int timer = 0;
        byte currentRow = 0;
        byte currentLevel = 0;
        byte linesNextLevel = 0; // has to be 10 in order to continue to next level
        int scoreIncrementor = 0;
        byte tetrominoNextIndex = 99; // just a random number which means that we have not yet chosen the next tetromino
        int totalNumberOfClearedLines = 0;
        
        bool sendNextPiece = true;
        bool putTetrominoOnGrid = true;
        bool skipLogicMain = false;
        bool moveRight = false;
        bool moveLeft = false;
        bool rotateRight = false;
        bool rotateLeft = false;
        bool moveDownFast = false;
        bool cannotMoveRight = false;
        bool cannotMoveLeft = false;
        bool cannotRotateRight = false;
        bool cannotRotateLeft = false;
        bool roundEnded = false;
        bool musicFastIsPlaying = false;
        bool musicSlowIsPlaying = true;
        bool playMusic = false;


        // Constructors ------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="music"></param>
        public Logic(Music music)
        {
            this.Music = music;
            for (int i = 0; i < Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID; i++) Matrix.Add(0);
        }

        // Properties --------------------------------------------------------------------------
        /// <summary>
        /// Counter which is used to determine when to move tetrominoCurrent down. It gets incremented by every GUI_TICK.
        /// Once the timer equals or exceeds the movementTicksBasedOnLevel, the tetrominoCurrent moves down.
        /// </summary>
        public int Timer { get { return timer; } set { timer = value; } }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move right.
        /// </summary>
        public bool MoveRight { get => moveRight; set => moveRight = value; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move left.
        /// </summary>
        public bool MoveLeft { get => moveLeft; set => moveLeft = value; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should move down faster.
        /// </summary>
        public bool MoveDownFast { get => moveDownFast; set => moveDownFast = value; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should rotate right.
        /// </summary>
        public bool RotateRight { get => rotateRight; set => rotateRight = value; }

        /// <summary>
        /// Boolean which determines if the tetrominoCurrent should rotate left.
        /// </summary>
        public bool RotateLeft { get => rotateLeft; set => rotateLeft = value; }

        /// <summary>
        /// Counter which holds the score of the player. It gets incremented by every finished row.
        /// </summary>
        public int ScoreIncrementor { get => scoreIncrementor; set => scoreIncrementor = value; }

        /// <summary>
        /// 4x4 grid which holds the tetromino which will be used in the next round at main matrix. During current round, this array is used to display the next tetromino.
        /// </summary>
        public byte[] TetrominoNext { get => tetrominoNext; set => tetrominoNext = value; }

        /// <summary>
        /// Level of the game. It gets incremented by every 10 finished rows. It is used to determine the speed of the game.
        /// </summary>
        public byte CurrentLevel { get => currentLevel; set => currentLevel = value; }

        /// <summary>
        /// Boolean which controls if the game should end. It gets set to true when the tetrominoCurrent reaches the top of the grid.
        /// </summary>
        public bool RoundEndedFlag { get => roundEnded; set => roundEnded = value; }

        /// <summary>
        /// 1D array which holds the main matrix of the game. It holds all the tetrominos which were put to the grid.
        /// </summary>
        public List<byte> Matrix { get => matrix; set => matrix = value; }

        /// <summary>
        /// Holds the number of cleared lines. It gets incremented by every finished row. The number is used later by GUI to display the number of all cleared lines.
        /// </summary>
        public int TotalNumberOfClearedLines { get => totalNumberOfClearedLines; set => totalNumberOfClearedLines = value; }


        // TODO: Check if these properties are needed here
        internal Music Music { get => music; set => music = value; }
        public bool PlayMusic { get => playMusic; set => playMusic = value; }
        public bool MusicSlowIsPlaying { get => musicSlowIsPlaying; set => musicSlowIsPlaying = value; }
        // --------------------------------------------------------------------------


        /// <summary>
        /// Controls at which row is tetromino during its lifetime on main matrix. Row gets always incremented by 1 when tetromino moves down (When tetromino moves by 10 index positions on matrix.
        /// </summary>
        public byte CurrentRow { get => currentRow; set => currentRow = value; }

        /// <summary>
        /// Boolean flag which controls if whole game logic should be skipped. It gets set to true when the game is paused and other parts of the program like settings or score screen are visible.
        /// </summary>
        public bool SkipLogicMain { get => skipLogicMain; set => skipLogicMain = value; }


        // Methods ----------------------------------------------------------------------------
        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetrominoCurrent object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte offset, bool active)
        {
            if (!active) return;

            // ColumnRelative is number between 0 and 4 which represents the column of tetrominoMatrix. Values between 0 and 3 are actually used to determine the column of tetrominoMatrix. At number 4 the index is reseted to 0.
            byte columnRelative = 0; 
            for (byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                // If we are at the end of the row of tetrominoMatrix (4x4 grid, meaning every 4th index), add +10 to offset which will be used at main matrix.
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Consts.ROW_JUMP_GRID;
                }
                byte gridIndexOffseted = (byte)(offset + columnRelative);

                // Put subpart of tetromino on grid if the subpart actually holds some value in tetrominoMatrix.
                if (tetrominoMatrix[i] > 0)
                {
                    Matrix[gridIndexOffseted] = tetrominoMatrix[i];
                    toBeRemoved.Add((byte)(gridIndexOffseted));  // These indexes will be later removed.
                }
                columnRelative++;
            }

            // Set indexes below index 20 to zero, so that tetromino is not drawn outside of the matrix, when rotated during first two rows at matrix.
            for (int i = 0; i < 20; i++)
            {
                Matrix[i] = 0;
            }
        }

        /// <summary>
        /// Actualises the selected matrix indexes to 0.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void RemoveTetrominoFromGrid()
        {
            foreach (byte i in toBeRemoved)
            {
                if (Matrix.Contains(i)) continue; // Checks if there is no overlap between former and current tetromino position. TODO: Check if this is needed, I think it is not. ============================================================
                Matrix[i] = 0;
            }
        }

        /// <summary>
        /// Checks wheter there is a tetrominoCurrent at next row.
        /// If yes, current tetrominoCurrent has to stop on current row.
        /// </summary>
        /// <returns></returns>
        public bool TetrominoHasObstacleAtNextRow()
        {
            if (toBeRemoved.Count > 0)
            {
                // Check for collision detection only at those indexes, which will not overlap when the tetrominoCurrent moves down.
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + (byte)Consts.ROW_JUMP_GRID))))
                        collisionDetection.Add((byte)this.toBeRemoved[i]);
                }

                // Check if at next main matrix row, there are some indexes non zero (there are some former tetrominos).
                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((Matrix[collisionDetection[i] + Consts.ROW_JUMP_GRID] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                }
                collisionDetection.Clear();
            }
            return false;
        }

        /// <summary>
        /// Check if tetrominoCurrent has obstacle at next column.
        /// If yes, tetromino must not move to desired side.
        /// This method is able to handle checking for both sides (left and right).
        /// This method is very similar to TetrominoHasObstacleAtNextRow(). It is possible to merge these two methods into one. (TODO)
        /// </summary>
        /// <param name="checkRightside">If yes, check movement to right else check movement to left</param>
        /// <returns></returns>
        public bool TetrominaHasObstacleAtNextColumn(bool checkRightside)
        {
            sbyte operator_;
            if (checkRightside) operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                // Check for collision detection only at those indexes, which will not overlap when the tetrominoCurrent moves to side.
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + operator_))))
                        collisionDetection.Add((byte)toBeRemoved[i]);
                }

                // Check if at next main matrix column, there are some indexes non zero (there are some former tetrominos).
                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((Matrix[collisionDetection[i] + operator_] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                }
                collisionDetection.Clear();
            }
            return false;
        }

        /// <summary>
        /// Check whether tetrominoCurrent has obstacle at next rotation.
        /// This method is able to handle checking for both sides (left and right).
        /// </summary>
        /// <param name="checkRightRotation">If yes, check right rotation else check left rotation</param>
        /// <param name="offset">Offset where to start putting indexes of tetrominoMatrix </param>
        /// <returns></returns>
        public bool TetrominaHasObstacleAtNextRotation(bool checkRightRotation, byte offset)
        {
            sbyte operator_;
            if (checkRightRotation) operator_ = 1;
            else operator_ = -1;
            if (toBeRemoved.Count > 0)
            {
                // Compute hypothetical rotation of tetrominoCurrent.
                sbyte hypotheticRotationType = (sbyte)(tetrominoCurrent.CurrentRotation + operator_);

                // Adjust rotation type if is out of bounds.
                if (hypotheticRotationType == -1) hypotheticRotationType = 3;
                else if (hypotheticRotationType == 4) hypotheticRotationType = 0;

                // Put artifitially tetrominoMatrixRotated on grid and check if there are no collisions.
                byte[] tetrominoMatrixRotated = tetrominoCurrent.Rotations[hypotheticRotationType];
                byte columnRelative = 0;
                for (byte i = 0; i < tetrominoMatrixRotated.Length; i++)
                {
                    if (columnRelative % 4 == 0 && i > 0)
                    {
                        columnRelative = 0;
                        offset += (byte)Consts.ROW_JUMP_GRID;
                    }
                    byte gridIndexOffseted = (byte)((offset + columnRelative));

                    if ((tetrominoMatrixRotated[i] > 0 && Matrix[gridIndexOffseted] > 0)
                        // Checks if tetromino would be out off left & right boundaries of matrix
                        || (offset % 10 == 8 || offset % 10 == 7)
                        // Checks if tetromino would be out off left & right boundaries of matrix but applies for I type tetromino
                        || (tetrominoCurrent.GetType_ == (byte)Consts.TetrominoType.I_type && offset % 10 == 9))
                    {
                        //collisionDetection.Clear(); Might be possible to delete ====================================================
                        return true;
                    }
                    columnRelative++;
                }
            }
            //collisionDetection.Clear(); Might be possible to delete ====================================================
            return false;
        }

        /// <summary>
        /// Check if tetrominoCurrent is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if (tetrominoCurrent.GetType_ == (byte)Consts.TetrominoType.I_type)
                return CurrentRow == Consts.LAST_ROW - tetrominoCurrent.ComputeNrOfBottomPaddingRows() + 1;
            return CurrentRow == Consts.LAST_ROW - tetrominoCurrent.ComputeNrOfBottomPaddingRows();
        }

        /// <summary>
        /// Game lost.
        /// </summary>
        private void RoundEnded()
        {
            if (RoundEndedFlag)
            {
                Music.GameOver();
                SkipLogicMain = true;
            }
        }

        public void MoveRight_UserEvent(bool canMoveRight, bool desiredRight)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
                if (toBeRemoved[i] % Consts.ROW_JUMP_GRID == 9) return;
            if (canMoveRight && desiredRight)
            {
                tetrominoCurrent.MoveRight();
                Music.MoveToSides();
            }
        }

        public void MoveLeft_UserEvent(bool canMoveLeft, bool desiredLeft)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
                if (toBeRemoved[i] % Consts.ROW_JUMP_GRID == 0) return;
            if (canMoveLeft && desiredLeft)
            {
                tetrominoCurrent.MoveLeft();
                Music.MoveToSides();
            }
        }

        private void MoveDownFaster_UserEvent(bool activate)
        {
            if (!activate) return;
            timer = Consts.movementTicksBasedOnLevel[CurrentLevel];
            MoveDownFast = false;
        }

        public void RotateLeft_UserEvent(bool canRotateLeft, bool desiredRotationLeft)
        {
            if (canRotateLeft && desiredRotationLeft)
            {
                tetrominoCurrent.RotateLeft();
                Music.Rotate();
            }
        }

        public void RotateRight_UserEvent(bool canRotateRight, bool desiredRotationRight)
        {
            if (canRotateRight && desiredRotationRight)
            {
                tetrominoCurrent.RotateRight();
                Music.Rotate();
            }
        }

        private void DefaultTetrominoMovement()
        {
            if (timer >= Consts.movementTicksBasedOnLevel[CurrentLevel])
            {
                if (TetrominoHasObstacleAtNextRow() || TetrominoIsAtBottom())
                {
                    if (CurrentRow == 0) RoundEndedFlag = true;
                    sendNextPiece = true;
                    CurrentRow = 0;
                    toBeRemoved.Clear();
                    Music.Obstacle();
                }
                else
                {
                    tetrominoCurrent.MoveDown(rowJumpGrid: Consts.ROW_JUMP_GRID);
                    CurrentRow++;
                    putTetrominoOnGrid = true;
                }
                timer = 0;
            }
        }

        private void PlayMusicAccordingToFilledGrid()
        {
            if (!PlayMusic) return;

            byte checkEmptySpaces = 0;

            for (byte i = 0; i < matrix.Count; i++)
            {
                if (toBeRemoved.Contains(i))
                {
                    continue;
                };

                if (i < 80 && matrix[i] == 0 && !MusicSlowIsPlaying)
                {
                    checkEmptySpaces++;
                    continue;

                }
                else if (i < 80 && matrix[i] > 0 && !musicFastIsPlaying)
                {
                    music.MainMusicFast();
                    musicFastIsPlaying = true;
                    MusicSlowIsPlaying = false;
                    return;
                }

            }
            if (checkEmptySpaces > 79)
            {
                music.MainMusicSlow();
                MusicSlowIsPlaying = true;
                musicFastIsPlaying = false;
            }
        }

        /// <summary>
        /// Prepare one random tetrominoCurrent at the starting position at the top of grid.
        /// </summary>
        public void ChoseNextTetromino()
        {
            if (sendNextPiece)
            {
                // If no tetromino was chosen yet (game has just started), chose one based on number 99. Otherwise chose a tetromino which was determined in previous round.
                if (tetrominoNextIndex == 99)
                    tetrominoCurrent = Consts.tetrominos[random.Next(Consts.MIN_NR_OF_TETROMINOS, Consts.MAX_NR_OF_TETROMINOS)];
                else tetrominoCurrent = Consts.tetrominos[tetrominoNextIndex];

                // Adjust starting position of tetrominoCurrent based on its type, on playing grid.
                if (tetrominoCurrent.GetType_ == (byte)Consts.TetrominoType.I_type)
                    tetrominoCurrent.Offset = 3;
                else
                    tetrominoCurrent.Offset = 3 + Consts.ROW_JUMP_GRID;

                // This will reset the tetrominoCurrent to its base rotation. Without this, new tetromino which would start at top of grid would have the same rotation as the previous one.
                tetrominoCurrent.Indexes = tetrominoCurrent.BaseRotation;
                tetrominoCurrent.CurrentRotation = 0;

                sendNextPiece = false;
                tetrominoNextIndex = (byte)random.Next(Consts.MIN_NR_OF_TETROMINOS, Consts.MAX_NR_OF_TETROMINOS);
                Array.Copy(Consts.tetrominos[tetrominoNextIndex].BaseRotation, TetrominoNext, TetrominoNext.Length);
            }
        }

        public void RemoveCompleteRows()
        {
            List<byte> indexesOfCompletedRows = new List<byte> { };
            byte[] rows = new byte[10];

            for (byte i = 0; i < Consts.HEIGHT_OF_GRID; i++)
            {
                Array.Copy(Matrix.ToArray(), i * 10, rows, 0, 10);
                if (rows.All(x => x > 0)) indexesOfCompletedRows.Add(i);
            }

            foreach (byte row in indexesOfCompletedRows)
            {
                Matrix.RemoveRange(row * 10, 10);
                Matrix.InsertRange(0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            ComputeScore((byte)indexesOfCompletedRows.Count);

            if (indexesOfCompletedRows.Count > 0 && indexesOfCompletedRows.Count <= 3)
            {
                Music.LineCleared();
                System.Threading.Thread.Sleep(450);
            }
            else if (indexesOfCompletedRows.Count == 4)
            {
                Music.Tetris();
                System.Threading.Thread.Sleep(450);
            }
            linesNextLevel += (byte)indexesOfCompletedRows.Count;
            TotalNumberOfClearedLines += (byte)indexesOfCompletedRows.Count;
        }

        private void CheckIfContinueToNextLevel()
        {
            if (linesNextLevel >= 10)
            {
                linesNextLevel = 0;
                currentLevel++;
                Music.NextLevel();
            }
        }

        private void ComputeScore(byte numberOfFinishedRows)
        {
            if (numberOfFinishedRows == 0) return;
            else if (numberOfFinishedRows == 1) ScoreIncrementor += (40 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 2) ScoreIncrementor += (100 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 3) ScoreIncrementor += (300 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 4) ScoreIncrementor += (1200 * (CurrentLevel + 1));
        }

        private void SetAllFlagsToFalse()
        {
            MoveRight = false;
            MoveLeft = false;
            cannotMoveLeft = false;
            cannotMoveRight = false;
            RotateRight = false;
            RotateLeft = false;
        }

        private void SetCollisionFlags()
        {
            cannotMoveRight = TetrominaHasObstacleAtNextColumn(checkRightside: true);
            cannotMoveLeft = TetrominaHasObstacleAtNextColumn(checkRightside: false);
            cannotRotateRight = TetrominaHasObstacleAtNextRotation(checkRightRotation: true, offset: tetrominoCurrent.Offset);
            cannotRotateLeft = TetrominaHasObstacleAtNextRotation(checkRightRotation: false, offset: tetrominoCurrent.Offset);
        }

        public void Main__(Action<List<byte>> redraw)
        {

            if (SkipLogicMain) return;
            PlayMusicAccordingToFilledGrid();
            RoundEnded();
            SetCollisionFlags();
            RotateRight_UserEvent(!cannotRotateRight, RotateRight);
            RotateLeft_UserEvent(!cannotRotateLeft, RotateLeft);
            MoveRight_UserEvent(!cannotMoveRight, MoveRight);
            MoveLeft_UserEvent(!cannotMoveLeft, MoveLeft);
            MoveDownFaster_UserEvent(activate: MoveDownFast);
            toBeRemoved.Clear();
            ChoseNextTetromino();
            PutTetrominoOnGrid(this.tetrominoCurrent.Indexes, tetrominoCurrent.Offset, putTetrominoOnGrid);
            DefaultTetrominoMovement();
            redraw(Matrix);
            RemoveTetrominoFromGrid();
            RemoveCompleteRows();
            CheckIfContinueToNextLevel();
            SetAllFlagsToFalse();

            //if (timer >= Consts.movementTicksBasedOnLevel[CurrentLevel])
                //Music.DisposeSFX_NAudio();

        }


        public void ResetAllFields()
        {
            matrix = new List<byte>();
            for (int i = 0; i < Consts.WIDTH_OF_GRID * Consts.HEIGHT_OF_GRID; i++) Matrix.Add(0);
            moveRight = false;
            moveLeft = false;
            rotateRight = false;
            rotateLeft = false;
            moveDownFast = false;
            cannotMoveRight = false;
            cannotMoveLeft = false;
            cannotRotateRight = false;
            cannotRotateLeft = false;
            sendNextPiece = true;
            putTetrominoOnGrid = true;
            roundEnded = false;
            RoundEndedFlag = false;
            musicFastIsPlaying = false;
            musicSlowIsPlaying = true;
            tetrominoNextIndex = 99;
            currentLevel = 0;
            totalNumberOfClearedLines = 0;
            timer = 0;
            CurrentRow = 0;
            scoreIncrementor = 0;
            linesNextLevel = 0;
            SkipLogicMain = false;
            playMusic = false;
        }
    }
}
