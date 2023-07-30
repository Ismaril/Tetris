
using System;

namespace Tetris
{
    public class Tetromino: Constants
    {
        private string type;
        private byte rotationType;
        private byte[] indexes;
        private bool movedRight = false;
        private bool movedLeft = false;
        private byte offset;
        

        public Tetromino()
        {
            this.type = String.Empty;
            this.indexes = new byte[16];
            this.rotationType = 0;
        }

        public byte[] GetIndexes { get { return this.indexes; } }
        public byte GetPositionOfRotation { get { return this.rotationType; } }
        public string GetType_ { get { return this.type; } } 
        public byte Offset { get { return this.offset; } set { this.offset = value; } }

        public bool MovedRight { get { return this.movedRight; } set { this.movedRight = value; } }

        public bool MovedLeft { get {  return this.movedLeft; } set { this.movedLeft = value; } }

        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown()
        {
            this.offset += ROW_JUMP_GRID;
        }
        
        /// <summary>
        /// Check if tetromino is at left boundary of grid.
        /// </summary>
        /// <returns></returns>
        private bool AtLeftGridBoundary()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.indexes[i] % ROW_JUMP_GRID == 0)
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
                if (this.indexes[i] % ROW_JUMP_GRID == 9)
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
            this.offset--;
            this.movedLeft = true;
        }

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight()
        {
            this.offset++;
            this.movedRight = true;
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
        /// <returns>byte[]</returns>
        public byte[] PrepareAtStartPosition()
        {
            Random random = new Random();
            int index = random.Next(0, 6); 
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
            this.offset = 3;
            return this.indexes;
        }
    }
}
