
using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetromino: Constants
    {
        private string type;
        private sbyte rotationType;
        private byte[] indexes;
        private bool movedRight = false;
        private bool movedLeft = false;
        private bool rotatedRight = false;
        private bool rotatedLeft = false;   
        private bool moveDownFaster = false;
        private byte offset;
        private List<byte[]> rotations;

        public Tetromino() { }

        public Tetromino(byte[] rotation0, byte[] rotation1, byte[] rotation2, byte[] rotation3, string type)
        {
            this.rotations = new List<byte[]> { rotation0, rotation1, rotation2, rotation3};
            this.type = String.Empty;
            this.indexes = rotation0;
            this.rotationType = 0;
        }

        public byte[] GetIndexes { get { return this.indexes; } }
        public sbyte GetPositionOfRotation { get { return this.rotationType; } }
        public string GetType_ { get { return this.type; } } 
        public byte Offset { get { return this.offset; } set { this.offset = value; } }

        public bool MovedRight { get { return this.movedRight; } set { this.movedRight = value; } }

        public bool MovedLeft { get {  return this.movedLeft; } set { this.movedLeft = value; } }

        public bool RotatedRight { get { return this.rotatedRight; } set { this.rotatedRight = value; } }

        public bool RotatedLeft { get { return this.rotatedLeft; } set { this.rotatedLeft = value; } }

        public bool MoveDownFasterP { get { return this.moveDownFaster; } set { this.moveDownFaster = value; } }

        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown()
        {
            this.offset += ROW_JUMP_GRID;
        }

        public void MoveDownFaster()
        {
            this.moveDownFaster = true;
        }

        /// <summary>
        /// Check if tetromino is at left boundary of grid.
        /// </summary>
        /// <returns></returns>
        private bool AtLeftGridBoundary()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.offset % ROW_JUMP_GRID == 0)
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
                if (this.offset % ROW_JUMP_GRID == 9)
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
            //if (!AtLeftGridBoundary())
            //{
                this.offset--;
                this.movedLeft = true;
            //}
            
        }

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight()
        {
            //if(!AtRightGridBoundary())
            //{
                this.offset++;
                this.movedRight = true;
            //}

        }

        /// <summary>
        /// Rotate tetromino right.
        /// </summary>
        public void RotateRight()
        {
            this.rotationType--;
            if (this.rotationType == -1) this.rotationType = 3;
            this.indexes = this.rotations[this.rotationType];
            this.rotatedRight = true;
        }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft()
        {
            this.rotationType++;
            if (this.rotationType == 4) this.rotationType = 0;
            this.indexes = this.rotations[this.rotationType];
            this.rotatedLeft = true;
        }
    }
}
