
using System;

namespace Tetris
{
    public class Tetromino: Constants
    {
        private string type;
        private int rotationType;
        private int[] indexes;
        private int[] previousIndexes;
        private Action<int[]> redraw;
        private Action<int[]> redrawBack;
        

        public Tetromino(Action<int[]> redraw, Action<int[]> redrawBack)
        {
            this.type = String.Empty;
            this.previousIndexes = new int[4];
            this.indexes = new int[4];
            this.rotationType = 0;
            this.redraw = redraw;
            this.redrawBack = redrawBack;   

        }

        public int[] GetIndexes { get { return this.indexes; } }
        public int[] GetPreviousIndexes { get { return this.previousIndexes; } }
        public int GetPositionOfRotation { get { return this.rotationType; } }
        public string GetType_ { get { return this.type; } } 

        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown()
        {
            for (int i = 0; i < 4; i++)
            {
                this.indexes[i] += ROW_JUMP;
            }
        }
        
        /// <summary>
        /// Prepare index positions of former tetromino position.
        /// </summary>
        public void SubtractToPreviousIndexes()
        {
            this.indexes.CopyTo(this.previousIndexes, 0);

            for (int i = 0; i < 4; i++)
            {
                this.previousIndexes[i] -= ROW_JUMP;
            }
        }
        
        /// <summary>
        /// Check if tetromino is at left boundary of grid.
        /// </summary>
        /// <returns></returns>
        private bool AtLeftGridBoundary()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.indexes[i] % ROW_JUMP == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if tetromino is at right boundary of grid.
        /// </summary>
        /// <returns></returns>
        private bool AtRightGridBoundary()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.indexes[i] % ROW_JUMP == 9)
                {
                    return true;
                }
            }
            return false;
        }
    

        /// <summary>
        /// Move tetromino left.
        /// </summary>
        public void MoveLeft()
        {
            if (!this.AtLeftGridBoundary())
            {
                for (int i = 0; i < 4; i++)
                {
                    this.indexes[i]--;
                }
                this.redraw(this.indexes);
                this.redrawBack(this.previousIndexes);
            }
       }

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight()
        {
            if (!this.AtRightGridBoundary())
            {
                for (int i = 0; i < 4; i++)
                {
                    this.indexes[i]++;
                }
                this.redraw(this.indexes);
                this.redrawBack(this.previousIndexes);

            }

        }

        /// <summary>
        /// Rotate tetromino right.
        /// </summary>
        public void RotateRight() { }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft() { } 

        /// <summary>
        /// Prepare one random tetromino at the starting position at the top of grid.
        /// </summary>
        /// <returns>int[]</returns>
        public int[] PrepareAtStartPosition()
        {
            Random random = new Random();
            int index = random.Next(0, 0); // prepsat 0 na 6 na druhej pozici

            switch (index)
            {
                case 0:
                    this.type = I_type;
                    I_Default.CopyTo(this.indexes, 0);
                    break;
                case 1:
                    this.type = O_type;
                    O_Default.CopyTo(this.indexes, 0);
                    break;
                case 2:
                    this.type = T_type;
                    T_Default.CopyTo(this.indexes, 0);
                    break;
                case 3:
                    this.type = S_type;
                    S_Default.CopyTo(this.indexes, 0);
                    break;
                case 4:
                    this.type = Z_type;
                    Z_Default.CopyTo(this.indexes, 0);
                    break;
                case 5:
                    this.type = J_type;
                    J_Default.CopyTo(this.indexes, 0);
                    break;
                case 6:
                    this.type = L_type;
                    L_Default.CopyTo(this.indexes, 0);
                    break;

            }
            return this.indexes;
        }
    }
}
