using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;

namespace Tetris
{
    public class Logic
    {
        //private byte[] matrix = new byte[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID];
        private List<byte> matrix = new List<byte>();
        
        public int speed = Constants.MOVEVEMENT_TICK;

        public bool moveRight = false;
        public bool moveLeft = false;
        public bool moveDownFast = false;
        public bool rotateRight = false;
        public bool rotateLeft = false; 
        public bool putTetrominoOnGrid = true; 
        public bool atGridBoundary = false;

        public bool cannotMoveRight = false;
        public bool cannotMoveLeft = false;
        public bool cannotMoveDown = false;
        
        public bool sendNextPiece = true;
        public bool removeTetromino = false;
        public bool startRemoving = false;

        public byte currentRow = 0;
        private byte nrOfSessionRows = 0;
        private bool roundEnded = false;
        private bool totalGameEnded = false;

        private int timer = 0;
        public bool gameStarted = false;

        private Tetromino tetromino = new Tetromino();
        private Tetromino I_tetromino = new Tetromino(Constants.I_0, Constants.I_1, Constants.I_0, Constants.I_1, Constants.I_type);
        private Tetromino O_tetromino = new Tetromino(Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_type);
        private Tetromino T_tetromino = new Tetromino(Constants.T_0, Constants.T_1, Constants.T_2, Constants.T_3, Constants.T_type);
        private Tetromino S_tetromino = new Tetromino(Constants.S_0, Constants.S_1, Constants.S_0, Constants.S_1, Constants.S_type);
        private Tetromino Z_tetromino = new Tetromino(Constants.Z_0, Constants.Z_1, Constants.Z_0, Constants.Z_1, Constants.Z_type);
        private Tetromino J_tetromino = new Tetromino(Constants.J_0, Constants.J_1, Constants.J_2, Constants.J_3, Constants.J_type);
        private Tetromino L_tetromino = new Tetromino(Constants.L_0, Constants.L_1, Constants.L_2, Constants.L_3, Constants.L_type);
        private List<Tetromino> tetrominos;
        private Action<List<byte>> redraw;
        private List<byte> toBeRemoved = new List<byte>();
        private List<byte> collisionDetection = new List<byte>();




        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="redrawBack"></param>
        public Logic(Action<List<byte>> redraw)
        {
            this.redraw = redraw;
            this.tetrominos = new List<Tetromino> { I_tetromino, O_tetromino, T_tetromino, S_tetromino, Z_tetromino, J_tetromino, L_tetromino };

            for(int i = 0; i < Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID; i++)
            {
                matrix.Add(0);
            }
        }

        public Tetromino Tetromino { get { return this.tetromino; } }
        public int Timer { get { return this.timer; } set { timer = value; } }

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
                    this.matrix[gridIndexOffseted] = tetrominoMatrix[i];
                    this.toBeRemoved.Add((byte)(gridIndexOffseted));
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
            foreach (byte i in this.toBeRemoved)
            {
                if (matrix.Contains(i)) continue;
                this.matrix[i] = 0;
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
                        this.collisionDetection.Add((byte)this.toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < this.collisionDetection.Count; i++)
                {
                    if ((this.matrix[collisionDetection[i] + Constants.ROW_JUMP_GRID] > 0))
                    {
                        this.collisionDetection.Clear();
                        return true;
                    }
                }
                this.collisionDetection.Clear();
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
                    if (!(this.toBeRemoved.Contains((byte)(this.toBeRemoved[i] + operator_))))
                    {
                        this.collisionDetection.Add((byte)this.toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < this.collisionDetection.Count; i++)
                {
                    if ((this.matrix[collisionDetection[i] + operator_] > 0))
                    {
                        this.collisionDetection.Clear();
                        return true;
                    }

                }
                this.collisionDetection.Clear();
            }
            return false;
        }

        public bool TetrominaHasObstacleAtNextRotation(bool checkRightside)
        {

            sbyte operator_;
            if (checkRightside) operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(this.toBeRemoved.Contains((byte)(this.toBeRemoved[i] + operator_))))
                    {
                        this.collisionDetection.Add((byte)this.toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < this.collisionDetection.Count; i++)
                {
                    if ((this.matrix[collisionDetection[i] + operator_] > 0))
                    {
                        this.collisionDetection.Clear();
                        return true;
                    }

                }
                this.collisionDetection.Clear();
            }
            return false;
        }

        /// <summary>
        /// Check if tetromino is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if(tetromino.GetType_ == Constants.I_type)
            {
                return this.currentRow == Constants.LAST_ROW - tetromino.ComputeNrOfBottomPaddingRows() + 1;
            }

            return this.currentRow == Constants.LAST_ROW - tetromino.ComputeNrOfBottomPaddingRows();


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
                this.roundEnded = true;
                throw new Exception("Game ended");
            }

        }

        public void MoveRight_UserEvent(bool canMoveRight, bool desiredRight)
        {
            //if (this.tetromino.Offset % Constants.ROW_JUMP_GRID == 9) return;
            
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                //Console.WriteLine(toBeRemoved[i] + 1 % Constants.ROW_JUMP_GRID);
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 9) return;
            }

            if (canMoveRight && desiredRight)
            {
                this.tetromino.MoveRight();

            }
        }

        public void MoveLeft_UserEvent(bool canMoveLeft, bool desiredLeft)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                //Console.WriteLine(toBeRemoved[i] - 1 % Constants.ROW_JUMP_GRID);
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 0) return;
            }

            if (canMoveLeft && desiredLeft)
            {
                this.tetromino.MoveLeft();
                
            }
        }



        private void MoveDownFaster_UserEvent(bool activate)
        {
            if (!activate) return;
            //this.PutTetrominoOnGrid(this.tetromino.Indexes, this.tetromino.Offset, putTetrominoOnGrid);
            //this.redraw(this.matrix);
            this.timer = Constants.MOVEVEMENT_TICK;
            moveDownFast = false;

        }

        public void RotateLeft_UserEvent(bool canRotateLeft, bool desiredRotationLeft)
        {
            //for (int i = 0; i < toBeRemoved.Count; i++)
            //{
            //    //Console.WriteLine(toBeRemoved[i] - 1 % Constants.ROW_JUMP_GRID);
            //    if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 0) return;
            //}

            if (canRotateLeft && desiredRotationLeft)
            {
                this.tetromino.RotateLeft();

            }
        }

        public void RotateRight_UserEvent(bool canRotateRight, bool desiredRotationRight)
        {
            //for (int i = 0; i < toBeRemoved.Count; i++)
            //{
            //    //Console.WriteLine(toBeRemoved[i] - 1 % Constants.ROW_JUMP_GRID);
            //    if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 0) return;
            //}

            if (canRotateRight && desiredRotationRight)
            {
                this.tetromino.RotateRight();

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
            if (this.timer >= Constants.MOVEVEMENT_TICK)
            {
                if (this.TetrominoHasObstacleAtNextRow() || this.TetrominoIsAtBottom())
                {
                    if (this.currentRow == 0) this.roundEnded = true;
                    sendNextPiece = true;
                    this.currentRow = 0;
                    this.toBeRemoved.Clear();
                    removeTetromino = false;
                }
                else
                {
                    removeTetromino = true;
                    this.tetromino.MoveDown();
                    this.currentRow++;
                    nrOfSessionRows++;
                    putTetrominoOnGrid = true;
                }
                this.timer = 0;
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
                this.tetromino = this.tetrominos[random.Next(0, 6)];
                if (this.tetromino.GetType_ == Constants.I_type)
                {
                    this.tetromino.Offset = 3;
                }
                else
                {
                    this.tetromino.Offset = 3 + Constants.ROW_JUMP_GRID;
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
            //matrix.ToArray();
            //for (byte i = 0; i < indexesOfCompletedRows.Count * 10; i++) matrix.Prepend<byte>(0);

        }

        public void Main__()
        {

            //GameStarted();
            //RoundEnded();
            //TotalGameEnded();

            ChoseNextTetromino();

            cannotMoveRight = TetrominaHasObstacleAtNextColumn(checkRightside: true);
            cannotMoveLeft = TetrominaHasObstacleAtNextColumn(checkRightside: false);
            cannotMoveDown = TetrominoHasObstacleAtNextRow();

            RotateLeft_UserEvent(true, rotateLeft);
            RotateRight_UserEvent(true, rotateRight);
            MoveRight_UserEvent(!cannotMoveRight, moveRight);
            MoveLeft_UserEvent(!cannotMoveLeft, moveLeft);
            MoveDownFaster_UserEvent(activate: moveDownFast);
            toBeRemoved.Clear();

            PutTetrominoOnGrid(this.tetromino.Indexes, this.tetromino.Offset, putTetrominoOnGrid);

            DefaultTetrominoMovement();

            
            redraw(matrix);

            RemoveTetrominoFromGrid(activate: removeTetromino);

            RemoveCompleteRows();


            this.moveRight = false;
            this.moveLeft = false;
            this.cannotMoveLeft = false;
            this.cannotMoveRight = false;
            
            rotateRight = false;
            rotateLeft = false;

            atGridBoundary = false;


        }
    }
}
