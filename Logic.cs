using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class Logic
    {

        private int[] matrix;
        private int currentRow;
        private int nrOfSessionRows;
        private bool roundEnded;
        private bool totalGameEnded;
        private Tetromino tetromino;
        private Action<int[]> redraw;
        private Action<int[]> redrawBack;

        public Logic(Action<int[]> redraw, Action<int[]> redrawBack)
        {

            this.matrix =  new int[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID];
            this.tetromino = new Tetromino(redraw, redrawBack);
            this.currentRow = 0;
            this.nrOfSessionRows = 0;
            this.roundEnded = false;
            this.totalGameEnded = false;
            this.redraw = redraw;
            this.redrawBack = redrawBack;
        }

        public Tetromino Tetromino { get {  return this.tetromino; } }
        
        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetromino object.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void PutTetrominoOnGrid(int[] tetrominoIndexes)
        {

            foreach (int i in tetrominoIndexes)
            {
                this.matrix[i] = 1;
            }
        }
        /// <summary>
        /// Actualises the selected matrix indexes to 0 from indexes of tetromino object.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void RemoveTetrominoFromGrid(int[] tetrominoIndexes)
        {

            foreach (int i in tetrominoIndexes)
            {
                this.matrix[i] = 0;
            }
        }

        private void VisualiseMatrixInConsole()
        {
            // Visualise at console.
            string matrixForPrint = String.Empty;
            for (int j = 0; j < this.matrix.Length; j++)
            {
                if (j % Constants.ROW_JUMP == 0) matrixForPrint += "\n";
                matrixForPrint += matrix[j];
            }
            Console.Write($"\r{matrixForPrint}");
        }


        /// <summary>
        /// Checks wheter there is a tetromino at next row.
        /// Checks wheter there is the botom of matrix at next row.
        /// If yes, current tetromino has to stop on current row.
        /// </summary>
        /// <returns></returns>
        private object TetrominoHasObstacleAtNextRow()
        {
            int rot = this.tetromino.GetPositionOfRotation;

            switch (this.tetromino.GetType_)
            {
                case Constants.I_type:
                    if (rot == 0)
                    {
                        foreach(int index in this.tetromino.GetIndexes)
                        {
                            if (this.matrix[index + Constants.ROW_JUMP] == 1)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    break;
            }
            return "Incorrect exit";
        }

        private object TetrominoCanFall()
        {
            return this.currentRow < Constants.LAST_ROW - 1;
        }
        
        private object TetrominoIsAtBottom()
        {
            return this.currentRow == Constants.LAST_ROW - 1;
        }

        private object TetrominaHasObstacleAtNextColumn() { throw new NotImplementedException(); }
      
        public void DrawGraphics()
        {
            // Change the state of the grid indexes at a current row from empty to having a tetromino.
            this.PutTetrominoOnGrid(this.tetromino.GetIndexes);

            // Add tetromino at new position at the screen - Redraw GUI
            this.redraw(this.tetromino.GetIndexes);

            if (this.currentRow > 0)
            {
                // Remove tetromino at old position from screen - Redraw GUI
                this.tetromino.SubtractToPreviousIndexes();
                redrawBack(this.tetromino.GetPreviousIndexes);
            }
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
            if (this.currentRow == 0) this.tetromino.PrepareAtStartPosition();

            this.DrawGraphics();
            
            if ((bool)this.TetrominoHasObstacleAtNextRow())
            {
                // Check if there is some tetromino at the top rows.
                if (this.currentRow == 0) this.RoundEnded();

                // Reset index and therefore send next piece.
                this.currentRow = 0;
            }           
            else if ((bool)this.TetrominoCanFall())
            {
                // Change the state of current indexes which have tetromino back to empty. This change will be visible in next iteration.
                this.RemoveTetrominoFromGrid(this.tetromino.GetIndexes);

                // Move the index at next row.
                this.tetromino.MoveDown();
                this.currentRow++;
            }
            else if ((bool)this.TetrominoIsAtBottom())
            {
                // Reset index and therefore send next piece.
                this.currentRow = 0;
            };
            this.nrOfSessionRows++;
        }
    }
}
