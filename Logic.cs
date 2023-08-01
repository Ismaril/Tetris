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
        private Action<byte[]> redraw;
        private List<byte> toBeRemoved = new List<byte>();
        private int timer = 0;
        List<byte> collisionDetectionDown = new List<byte>();
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
                    this.toBeRemoved.Add((byte)(offset + columnRelative));
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
            foreach (int i in this.toBeRemoved)
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
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(this.toBeRemoved.Contains((byte)(this.toBeRemoved[i] + (byte)Constants.ROW_JUMP_GRID))))
                    {
                        this.collisionDetectionDown.Add(this.toBeRemoved[i]);
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
            throw new Exception("Incorrect exit");
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

        private object TetrominaHasObstacleAtNextColumn() { throw new NotImplementedException(); }
      
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
            if (this.tetromino.MovedRight || this.tetromino.MovedLeft)
            {
                this.RemoveTetrominoFromGrid();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
                this.tetromino.MovedLeft = false;
                this.tetromino.MovedRight = false;
            }
        }

        private void ArrowKeysMoveDownFaster_UserEvent() { }

        private void ArrowKeysRotate_UserEvent() { }

        private void GameStarted()
        {
            if (!gameStarted)
            {
                gameStarted = true;
                this.tetromino.PrepareAtStartPosition();
                this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.Offset);
                this.redraw(this.matrix);
            }
        }

        private void AutomaticShitgoingdownrenamethis(int tick)
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

                if (this.currentRow == 0) { this.tetromino.PrepareAtStartPosition(); }

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
            AutomaticShitgoingdownrenamethis(tick: Constants.MOVEVEMENT_TICK);


        }
    }
}
