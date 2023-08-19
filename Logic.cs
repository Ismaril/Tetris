using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;

namespace Tetris
{
    public class Logic
    {
        private List<byte> matrix = new List<byte>();
        
        public int speed = Constants.MOVEVEMENT_TICK;

        private bool moveRight = false;
        private bool moveLeft = false;
        private bool rotateRight = false;
        private bool rotateLeft = false;
        private bool moveDownFast = false;


        private bool atGridBoundary = false;
        private bool cannotMoveRight = false;
        private bool cannotMoveLeft = false;
        public bool cannotMoveDown = false;
        private bool cannotRotateRight = false;
        private bool cannotRotateLeft = false;

        public bool sendNextPiece = true;
        public bool putTetrominoOnGrid = true; 
        public bool startRemoving = false;
        public bool removeTetromino = false;

        private int timer = 0;
        public byte currentRow = 0;
        private byte nrOfSessionRows = 0;

        public bool gameStarted = false;
        private bool roundEnded = false;
        private bool totalGameEnded = false;

        private Tetromino tetromino = new Tetromino();
        private Tetromino I_tetromino = new Tetromino(Constants.I_0, Constants.I_1, Constants.I_0, Constants.I_1, (byte)Constants.TetrominoType.I_type);
        private Tetromino O_tetromino = new Tetromino(Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_0, (byte)Constants.TetrominoType.O_type);
        private Tetromino T_tetromino = new Tetromino(Constants.T_0, Constants.T_1, Constants.T_2, Constants.T_3, (byte)Constants.TetrominoType.T_type);
        private Tetromino S_tetromino = new Tetromino(Constants.S_0, Constants.S_1, Constants.S_0, Constants.S_1, (byte)Constants.TetrominoType.S_type);
        private Tetromino Z_tetromino = new Tetromino(Constants.Z_0, Constants.Z_1, Constants.Z_0, Constants.Z_1, (byte)Constants.TetrominoType.Z_type);
        private Tetromino J_tetromino = new Tetromino(Constants.J_0, Constants.J_1, Constants.J_2, Constants.J_3, (byte)Constants.TetrominoType.J_type);
        private Tetromino L_tetromino = new Tetromino(Constants.L_0, Constants.L_1, Constants.L_2, Constants.L_3, (byte)Constants.TetrominoType.L_type);
        private List<Tetromino> tetrominos;
        private Action<List<byte>> redraw;
        public List<byte> toBeRemoved = new List<byte>();
        public List<byte> collisionDetection = new List<byte>();

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="redrawBack"></param>
        public Logic(Action<List<byte>> redraw)
        {
            this.redraw = redraw;
            tetrominos = new List<Tetromino> { I_tetromino, O_tetromino, T_tetromino, S_tetromino, Z_tetromino, J_tetromino, L_tetromino };

            for(int i = 0; i < Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID; i++)
            {
                matrix.Add(0);
            }
        }

        public Tetromino Tetromino { get { return tetromino; } }
        public int Timer { get { return timer; } set { timer = value; } }
        public bool MoveRight { get => moveRight; set => moveRight = value; }
        public bool MoveLeft { get => moveLeft; set => moveLeft = value; }
        public bool MoveDownFast { get => moveDownFast; set => moveDownFast = value; }
        public bool RotateRight { get => rotateRight; set => rotateRight = value; }
        public bool RotateLeft { get => rotateLeft; set => rotateLeft = value; }

        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetromino object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte offset, bool active)
        {
            if(!active) return;

            byte columnRelative = 0;
            for (byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Constants.ROW_JUMP_GRID;
                }
                byte gridIndexOffseted = (byte)(offset + columnRelative);

                if (tetrominoMatrix[i] > 0)
                {
                    matrix[gridIndexOffseted] = tetrominoMatrix[i];
                    toBeRemoved.Add((byte)(gridIndexOffseted));
                }
                columnRelative++;
            }
            startRemoving = true;
        }

        /// <summary>
        /// Actualises the selected matrix indexes to 0.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void RemoveTetrominoFromGrid(bool activate)
        {
            foreach (byte i in toBeRemoved)
            {
                if (matrix.Contains(i)) continue;
                matrix[i] = 0;
            }
             //toBeRemoved.Clear();
             removeTetromino = false;


        }

        /// <summary>
        /// Checks wheter there is a tetromino at next row.
        /// If yes, current tetromino has to stop on current row.
        /// </summary>
        /// <returns></returns>
        public bool TetrominoHasObstacleAtNextRow()
        {
            if (toBeRemoved.Count > 0)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + (byte)Constants.ROW_JUMP_GRID))))
                    {
                        collisionDetection.Add((byte)this.toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((matrix[collisionDetection[i] + Constants.ROW_JUMP_GRID] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                }
                collisionDetection.Clear();
            }
            return false;
        }

        public bool TetrominaHasObstacleAtNextColumn(bool checkRightside)
        {

            sbyte operator_;
            if (checkRightside) operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + operator_))))
                    {
                        collisionDetection.Add((byte)toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((matrix[collisionDetection[i] + operator_] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }

                }
                collisionDetection.Clear();
            }
            return false;
        }

        public bool TetrominaHasObstacleAtNextRotation(bool checkRightRotation, byte offset)
        {
            sbyte operator_;

            if (checkRightRotation)operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                sbyte rotationType = (sbyte)(tetromino.GetPositionOfRotation + operator_);
                if (rotationType == -1) rotationType = 3;
                else if (rotationType == 4) rotationType = 0;
                byte[] tetrominoMatrix = tetromino.Rotations[rotationType];
                byte columnRelative = 0;

                for (byte i = 0; i < tetrominoMatrix.Length; i++)
                {
                    if (columnRelative % 4 == 0 && i > 0)
                    {
                        columnRelative = 0;
                        offset += (byte)Constants.ROW_JUMP_GRID;
                    }

                    byte gridIndexOffseted = (byte)((offset + columnRelative));
                    if ((tetrominoMatrix[i] > 0 && matrix[gridIndexOffseted] > 0)
                        || (offset % 10 == 8 || offset % 10 == 7)
                        || (tetromino.GetType_ == (byte)Constants.TetrominoType.I_type && offset % 10 == 9))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                    columnRelative++;
                }
            }
            collisionDetection.Clear();
            return false;
        }

        /// <summary>
        /// Check if tetromino is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if(tetromino.GetType_ == (byte)Constants.TetrominoType.I_type)
            {
                return currentRow == Constants.LAST_ROW - tetromino.ComputeNrOfBottomPaddingRows() + 1;
            }
            return currentRow == Constants.LAST_ROW - tetromino.ComputeNrOfBottomPaddingRows();
        }

        /// <summary>
        /// Notify a player that the current game round has ended. Game lost.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void RoundEnded()
        {
            if (roundEnded)
            {
                //Console.WriteLine("You lost");
                roundEnded = false;
                throw new Exception("Game ended");
            }
        }

        public void MoveRight_UserEvent(bool canMoveRight, bool desiredRight)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 9) return;
            }
            if (canMoveRight && desiredRight) tetromino.MoveRight();
        }

        public void MoveLeft_UserEvent(bool canMoveLeft, bool desiredLeft)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 0) return;
            }
            if (canMoveLeft && desiredLeft) tetromino.MoveLeft();
        }

        private void MoveDownFaster_UserEvent(bool activate)
        {
            if (!activate) return;
            timer = Constants.MOVEVEMENT_TICK;
            MoveDownFast = false;
        }

        public void RotateLeft_UserEvent(bool canRotateLeft, bool desiredRotationLeft)
        {
            if (canRotateLeft && desiredRotationLeft)
            {
                tetromino.RotateLeft();
            }
        }

        public void RotateRight_UserEvent(bool canRotateRight, bool desiredRotationRight)
        {
            if (canRotateRight && desiredRotationRight)
            {
                tetromino.RotateRight();
            }
        }

    //private void GameStarted()
    //{
    //    if (!gameStarted)
    //    {
    //        gameStarted = true;
    //        //this.ChoseNextTetromino();
    //        this.PutTetrominoOnGrid(this.tetromino.Indexes, this.tetromino.Offset);
    //        this.redraw(this.matrix);
    //    }
    //}

    private void DefaultTetrominoMovement()
        {
            if (timer >= Constants.MOVEVEMENT_TICK)
            {
                if (TetrominoHasObstacleAtNextRow() || TetrominoIsAtBottom())
                {
                    if (currentRow == 0) roundEnded = true;
                    sendNextPiece = true;
                    currentRow = 0;
                    toBeRemoved.Clear();
                    removeTetromino = false;
                }
                else
                {
                    removeTetromino = true;
                    tetromino.MoveDown();
                    currentRow++;
                    nrOfSessionRows++;
                    putTetrominoOnGrid = true;
                }
                timer = 0;
            }
        }

        private void TotalGameEnded()
        {
            if (totalGameEnded) { }
        }

        /// <summary>
        /// Prepare one random tetromino at the starting position at the top of grid.
        /// </summary>
        /// <returns>byte[]</returns>
        public void ChoseNextTetromino()
        {
            if (sendNextPiece)
            {
                Random random = new Random();
                tetromino = tetrominos[random.Next(Constants.MIN_NR_OF_TETROMINOS, Constants.MAX_NR_OF_TETROMINOS)];
                if (tetromino.GetType_ == (byte)Constants.TetrominoType.I_type)
                {
                    tetromino.Offset = 3;
                }
                else
                {
                    tetromino.Offset = 3 + Constants.ROW_JUMP_GRID;
                }
                tetromino.Indexes = tetromino.baseRotation;
                tetromino.rotationType = 0;
                sendNextPiece = false;
            }
        }

        public void RemoveCompleteRows()
        {
            List<byte> indexesOfCompletedRows = new List<byte> { };
            byte[] rows = new byte[10];

            for (byte i = 0; i < Constants.HEIGHT_OF_GRID; i++)
            {
                Array.Copy(matrix.ToArray(), i * 10, rows, 0, 10);
                if (rows.All(x => x > 0)) indexesOfCompletedRows.Add(i);
            }

            foreach (byte row in indexesOfCompletedRows)
            {
                matrix.RemoveRange(row*10, 10);
                matrix.InsertRange(0, new byte[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
        }

        private void SetAllFlagsToFalse()
        {
            MoveRight = false;
            MoveLeft = false;
            cannotMoveLeft = false;
            cannotMoveRight = false;
            RotateRight = false;
            RotateLeft = false;
            atGridBoundary = false;
        }

        public void Main__()
        {
            //GameStarted();
            RoundEnded();
            //TotalGameEnded();

            cannotMoveRight = TetrominaHasObstacleAtNextColumn(checkRightside: true);
            cannotMoveLeft = TetrominaHasObstacleAtNextColumn(checkRightside: false);
            cannotRotateRight = TetrominaHasObstacleAtNextRotation(checkRightRotation: true,offset: tetromino.Offset);
            cannotRotateLeft = TetrominaHasObstacleAtNextRotation(checkRightRotation: false, offset: tetromino.Offset);
            //cannotMoveDown = TetrominoHasObstacleAtNextRow();

            RotateRight_UserEvent(!cannotRotateRight, RotateRight);
            RotateLeft_UserEvent(!cannotRotateLeft, RotateLeft);
            MoveRight_UserEvent(!cannotMoveRight, MoveRight);
            MoveLeft_UserEvent(!cannotMoveLeft, MoveLeft);
            MoveDownFaster_UserEvent(activate: MoveDownFast);

            toBeRemoved.Clear();
            ChoseNextTetromino();
            PutTetrominoOnGrid(this.tetromino.Indexes, tetromino.Offset, putTetrominoOnGrid);
            DefaultTetrominoMovement();
            redraw(matrix);
            RemoveTetrominoFromGrid(activate: removeTetromino);
            RemoveCompleteRows();
            SetAllFlagsToFalse();
        }
    }
}
