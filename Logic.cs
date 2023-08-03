using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Logic
    {
        private byte[] matrix = new byte[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID];
        private byte currentRow = 0;
        private byte nrOfSessionRows = 0;
        private bool roundEnded = false;
        private bool totalGameEnded = false;
        private Tetromino tetromino = new Tetromino();
        private Tetromino I_tetromino = new Tetromino(Constants.I_0, Constants.I_1, Constants.I_0, Constants.I_1, Constants.I_type);
        private Tetromino O_tetromino = new Tetromino(Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_type);
        private Tetromino T_tetromino = new Tetromino(Constants.T_0, Constants.T_1, Constants.T_2, Constants.T_3, Constants.T_type);
        private Tetromino S_tetromino = new Tetromino(Constants.S_0, Constants.S_1, Constants.S_0, Constants.S_1, Constants.S_type);
        private Tetromino Z_tetromino = new Tetromino(Constants.Z_0, Constants.Z_1, Constants.Z_0, Constants.Z_1, Constants.Z_type);
        private Tetromino J_tetromino = new Tetromino(Constants.J_0, Constants.J_1, Constants.J_2, Constants.J_3, Constants.J_type);
        private Tetromino L_tetromino = new Tetromino(Constants.L_0, Constants.L_1, Constants.L_2, Constants.L_3, Constants.L_type);
        private List<Tetromino> tetrominos;
        private Action<byte[]> redraw;
        private List<sbyte> toBeRemoved = new List<sbyte>();
        private int timer = 0;
        List<byte> collisionDetectionDown = new List<byte>();  // prejmenovat
        public bool gameStarted = false;


        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="redrawBack"></param>
        public Logic(Action<byte[]> redraw)
        {
            this.redraw = redraw;
            this.tetrominos = new List<Tetromino>{ I_tetromino, O_tetromino, T_tetromino, S_tetromino, Z_tetromino, J_tetromino, L_tetromino };
        }

        public Tetromino Tetromino { get {  return this.tetromino; } }
        public int Timer { get { return this.timer; } set { timer = value; } }
        
        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetromino object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte offset)
        {
            byte columnRelative = 0;
            for(byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Constants.ROW_JUMP_GRID;
                }
                if (tetrominoMatrix[i] > 0)
                {
                    this.matrix[offset + columnRelative] = tetrominoMatrix[i];
                    this.toBeRemoved.Add((sbyte)(offset + columnRelative));
                }
                columnRelative++;
            }
        }

        /// <summary>
        /// Actualises the selected matrix indexes to 0.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void RemoveTetrominoFromGrid()
        {
            foreach (byte i in this.toBeRemoved)
            {
                this.matrix[i] = 0;
            }
        }

        /// <summary>
        /// Checks wheter there is a tetromino at next row.
        /// If yes, current tetromino has to stop on current row.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoHasObstacleAtNextRow()
        {

            for (byte i = 0; i < 4; i++)
            {
                if (!(this.toBeRemoved.Contains((sbyte)(this.toBeRemoved[i] + (sbyte)Constants.ROW_JUMP_GRID))))
                {
                    this.collisionDetectionDown.Add((byte)this.toBeRemoved[i]);
                }
            }

            for (byte i = 0; i < this.collisionDetectionDown.Count; i++)
            {
                if ((this.matrix[collisionDetectionDown[i] + Constants.ROW_JUMP_GRID] > 0))
                {
                    this.collisionDetectionDown.Clear();
                    return true;
                } 
            }
            this.collisionDetectionDown.Clear();
            return false;

        }
                
        /// <summary>
        /// Check if tetromino is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if (this.tetromino.GetType_ == Constants.I_type)
            {
                return this.currentRow == Constants.LAST_ROW - 3;
            }
            else
            {
                return this.currentRow == Constants.LAST_ROW - 4;
            }

        }

        private bool TetrominaHasObstacleAtNextColumn(bool IsMovingRight)
        {
            sbyte operator_;
            if (IsMovingRight) operator_ = 1;
            else operator_ = -1;

            for (byte i = 0; i < 4; i++)
            {
                if (!(this.toBeRemoved.Contains((sbyte)(this.toBeRemoved[i] + (sbyte)operator_))))
                {
                    this.collisionDetectionDown.Add((byte)this.toBeRemoved[i]);
                }
            }

            for (byte i = 0; i < this.collisionDetectionDown.Count; i++)
            {
                if ((this.matrix[(sbyte)collisionDetectionDown[i] + operator_] > 0))
                {
                    this.collisionDetectionDown.Clear();
                    return true;
                }
            }
            this.collisionDetectionDown.Clear();
            return false;

        }

      
        /// <summary>
        /// Notify a player that the current game round has ended. Game lost.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void RoundEnded()
        {
            if (roundEnded)
            {
                Console.WriteLine("You lost");
                this.roundEnded = true;
                throw new Exception("Game ended");
            }

        }

        private void ArrowKeysMoveToSides_UserEvent()
        {
            if ((this.tetromino.MovedRight || this.tetromino.MovedLeft))
                //&& !TetrominaHasObstacleAtNextColumn(this.tetromino.MovedRight))
            {
                this.RemoveTetrominoFromGrid();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
                this.tetromino.MovedLeft = false;
                this.tetromino.MovedRight = false;
            }
        }

        private void ArrowKeysMoveDownFaster_UserEvent()
        {
            if (this.tetromino.MoveDownFasterP)
            {
                this.RemoveTetrominoFromGrid();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
                this.tetromino.MoveDownFasterP = false;
                this.timer = Constants.MOVEVEMENT_TICK;
            }
        }

        private void ArrowKeysRotate_UserEvent()
        {
            if (this.tetromino.RotatedRight || this.tetromino.RotatedLeft)
            {
                this.RemoveTetrominoFromGrid();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
                this.tetromino.RotatedRight = false;
                this.tetromino.RotatedLeft = false;
            }
        }

        private void GameStarted()
        {
            if (!gameStarted)
            {
                gameStarted = true;
                this.PrepareAtStartPosition();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
            }
        }

        private void DefaultTetrominoMovement(int tick)
        {
            if (this.timer >= tick)
            {
                if (this.TetrominoHasObstacleAtNextRow() || this.TetrominoIsAtBottom())
                {
                    if (this.currentRow == 0) this.roundEnded = true;
                    this.currentRow = 0;
                    this.toBeRemoved.Clear();
                }
                else
                {
                    this.RemoveTetrominoFromGrid();
                    this.redraw(this.matrix);
                    this.toBeRemoved.Clear();
                    this.tetromino.MoveDown();
                    this.currentRow++;
                }

                if (this.currentRow == 0) { this.PrepareAtStartPosition(); }
                
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
                this.nrOfSessionRows++;
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
        public byte[] PrepareAtStartPosition()
        {
            Random random = new Random();
            this.tetromino = this.tetrominos[random.Next(0, 6)];
            this.tetromino.Offset = 3;
            return this.tetromino.GetIndexes;
        }

        public void Main__()
        {
            // State of game
            GameStarted();
            RoundEnded();
            TotalGameEnded();

            // User events
            ArrowKeysMoveToSides_UserEvent();
            ArrowKeysMoveDownFaster_UserEvent();
            ArrowKeysRotate_UserEvent();

            // Tetromino just fualling
            DefaultTetrominoMovement(tick: Constants.MOVEVEMENT_TICK);

        }
    }
}
