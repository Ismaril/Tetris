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
    public class Logic: Constants
    {

        public int[] matrix;
        public Tetromino tetromino;
        int currentRow;
        int nrOfSessionRows;
        public bool roundEnded;
        public bool totalGameEnded;
        Action<int[]> redraw;
        Action<int[]> redrawBack;

        public Logic(Action<int[]> redraw, Action<int[]> redrawBack)
        {
            this.matrix =  new int[WIDTH_OF_GRID * HEIGHT_OF_GRID];
            this.tetromino = new Tetromino(tetrominoType: I_type);
            this.currentRow = 0;
            this.nrOfSessionRows = 0;
            this.roundEnded = false;
            this.totalGameEnded = false;
            this.redraw = redraw;
            this.redrawBack = redrawBack;
        }
        
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
                if (j % ROW_JUMP == 0) matrixForPrint += "\n";
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
        private object CheckObstacleAtNextRow()
        {
            int rot = this.tetromino.GetPositionOfRotation();

            switch (this.tetromino.GetType_())
            {
                case I_type:
                    if (rot == 0)
                    {
                        foreach(int index in this.tetromino.GetIndexes())
                        {
                            if (this.matrix[index + ROW_JUMP] == 1)
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


        private void UserInput()
        {
            var keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow: Console.WriteLine("UP"); break;
                case ConsoleKey.DownArrow: Console.WriteLine("Down"); break;
                case ConsoleKey.LeftArrow: Console.WriteLine("Left"); break;
                case ConsoleKey.RightArrow: Console.WriteLine("Right"); break;

            }
            //Thread.Sleep(100);
        }
        
        public void StartSendingPieces()
        {
            if (this.nrOfSessionRows == 0)
            {
                this.tetromino.SendNewFromTop();
            }
        }

        public void MoveLeft()
        {
            switch (this.tetromino.GetType_())
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.tetromino.GetIndexes()[i]--;
                    }
                    break;
            }
            this.redraw(this.tetromino.GetIndexes());
            this.redrawBack(this.tetromino.GetPreviousIndexes());
        }

        public void MoveRight()
        {
            switch (this.tetromino.GetType_())
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.tetromino.GetIndexes()[i]++;
                    }
                    break;
            }
            this.redraw(this.tetromino.GetIndexes());
            this.redrawBack(this.tetromino.GetPreviousIndexes());
        }

        public void MoveDown()
        {
            switch (this.tetromino.GetType_())
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.tetromino.GetIndexes()[i] += ROW_JUMP;
                    }
                    break;
            }
            this.redraw(this.tetromino.GetIndexes());
            this.redrawBack(this.tetromino.GetPreviousIndexes());
        }


        public void Main__()
        {

            if (this.nrOfSessionRows == 0)
            {
                this.tetromino.SendNewFromTop();
            }

            // Change the state of the indexes at a current row from empty to having a tetromino.
            this.PutTetrominoOnGrid(this.tetromino.GetIndexes());
            this.redraw(this.tetromino.GetIndexes());

            
            if (this.currentRow > 0)
            {
                this.tetromino.SubtractToPreviousIndexes();
                redrawBack(this.tetromino.GetPreviousIndexes());

            }
            

            //UserInput();
            //this.VisualiseMatrixInConsole(); 
            //Thread.Sleep(SPEED_OF_PIECES_FALLING);

            //Console.Clear();


            // Check if there is some tetromino at next row.
            
            if ((bool)CheckObstacleAtNextRow())
            {
                // Check if there is some tetromino at the top rows.
                if (this.currentRow == 0)
                {
                    //Console.Clear();
                    Console.WriteLine("You lost");
                    this.roundEnded = true;
                    throw new Exception("Game ended");
                }
                // Reset index and therefore send next piece.
                this.tetromino.SendNewFromTop();
                this.currentRow = 0;
            }
            
            
            // If this condition is True, tetromino can freely fall at next row.

            //PRIDAT ELSE IF!
            else if (this.currentRow < LAST_ROW - 1)
            {
                // Change the state of current indexes which have tetromino back to empty. This change will be visible in next iteration.
                this.RemoveTetrominoFromGrid(this.tetromino.GetIndexes());
                

                // Move the index and next row.
                this.tetromino.MoveDown();
                


                this.currentRow++;
            }

            // Check if a tetromino is at the bottom row.
            else if (this.currentRow == LAST_ROW - 1)
            {
                // Reset index and therefore send next piece.
                this.tetromino.SendNewFromTop();
                this.currentRow = 0;
            };


            this.nrOfSessionRows++;

            //Console.ReadLine();

        }

    }
}
