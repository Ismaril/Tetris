using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Timers;

namespace Tetris
{
    public class Logic
    {

        private byte[] matrix;
        private byte currentRow;
        private byte nrOfSessionRows;
        private bool roundEnded;
        private bool totalGameEnded;
        private Tetromino tetromino;
        private Action<byte[]> redraw;
        private Action<byte[]> redrawBack;
        private List<byte> toBeRemoved;
        public int timer;
        private bool canMoveDown;
        private bool deletePrevious;

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="redrawBack"></param>
        public Logic(Action<byte[]> redraw, Action<byte[]> redrawBack)
        {

            this.matrix =  new byte[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID];
            this.tetromino = new Tetromino(redraw, redrawBack);
            this.currentRow = 0;
            this.nrOfSessionRows = 0;
            this.roundEnded = false;
            this.totalGameEnded = false;
            this.redraw = redraw;
            this.redrawBack = redrawBack;
            this.timer = 0;
            this.deletePrevious = false;
            this.toBeRemoved = new List<byte>();
        }

        public Tetromino Tetromino { get {  return this.tetromino; } }
        
        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetromino object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte x)
        {
            byte j = 0;
            for(byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                if (j % 4 == 0 && i > 0)
                {
                    j = 0;
                    x += (byte)Constants.ROW_JUMP;
                }
                if (tetrominoMatrix[i] > 0)
                {
                    this.matrix[x + j] = tetrominoMatrix[i];
                    this.toBeRemoved.Add((byte)(x + j));
                }
                j++;
            }
        }

        /// <summary>
        /// Actualises the selected matrix indexes to 0 from indexes of tetromino object.
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
        private object TetrominoHasObstacleAtNextRow()
        {
            {
                for(byte i = 0;i < this.toBeRemoved.Count; i++)
                {
                    if (this.matrix[this.toBeRemoved[i] + Constants.ROW_JUMP] > 0)
                    {
                        return true;
                    } 
                }
                return false;
            }
            throw new Exception("Incorrect exit");
        }
        
        /// <summary>
        /// Check if tetromino is still away from bottom of grid.
        /// </summary>
        /// <returns></returns>
        private object TetrominoCanFall()
        {
            return this.currentRow < Constants.LAST_ROW - 3;
        }
        
        /// <summary>
        /// Check if tetromino is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private object TetrominoIsAtBottom()
        {
            return this.currentRow == Constants.LAST_ROW  - 3;
        }

        private object TetrominaHasObstacleAtNextColumn() { throw new NotImplementedException(); }
      
        public void DrawGraphics()
        {
            this.PutTetrominoOnGrid(this.tetromino.GetIndexes, this.tetromino.X);

            // Add tetromino at new position at the screen - Redraw GUI
            this.redraw(this.matrix);
        }

        /// <summary>
        /// Notify a player that the current game round has ended. Game lost.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void RoundEnded()
        {
            Console.WriteLine("You lost");
            this.roundEnded = true;
            throw new Exception("Game ended");
        }

        public void Main__()
        {
            if (this.tetromino.MovedRight || this.tetromino.MovedLeft)
            {
                RemoveTetrominoFromGrid();
                this.redrawBack(this.matrix);
                this.DrawGraphics();
                this.tetromino.MovedLeft = false;
                this.tetromino.MovedRight = false;
            }

            if (this.timer > Constants.SPEED_OF_PIECES_FALLING)
            {
                //if ((bool)this.TetrominoHasObstacleAtNextRow())
                //{
                //    if (this.currentRow == 0) this.RoundEnded();
                //    this.currentRow = 0;
                //    this.deletePrevious = false;
                //    this.toBeRemoved.Clear();

                //}
                //else
                if ((bool)this.TetrominoIsAtBottom())
                {
                    this.currentRow = 0;
                    this.deletePrevious = false;
                    this.toBeRemoved.Clear();
                }

                else if ((bool)this.TetrominoCanFall())
                {
                    RemoveTetrominoFromGrid();
                    this.redrawBack(this.matrix);
                    this.toBeRemoved.Clear();
                    this.tetromino.MoveDown();
                    this.currentRow++;
                }

                if (this.currentRow == 0){this.tetromino.PrepareAtStartPosition();}

                this.DrawGraphics();
                this.nrOfSessionRows++;
                this.timer = 0;
            }
        }
    }
}
