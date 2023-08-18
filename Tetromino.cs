
using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetromino : Constants
    {
        private string type;
        public sbyte rotationType;
        private byte[] indexes;
        private byte offset;
        private List<byte[]> rotations;
        public byte[] baseRotation;

        public Tetromino() { }

        public Tetromino(byte[] rotation0, byte[] rotation1, byte[] rotation2, byte[] rotation3, string type)
        {
            this.baseRotation = rotation0;
            this.rotations = new List<byte[]> { rotation0, rotation1, rotation2, rotation3 };
            this.type = type;
            this.indexes = baseRotation;
            this.rotationType = 0;
        }

        public byte[] Indexes { get { return this.indexes; } set { this.indexes = value; } }
        public sbyte GetPositionOfRotation { get { return this.rotationType; } }
        public string GetType_ { get { return this.type; } }
        public byte Offset { get { return this.offset; } set { this.offset = value; } }

        public List<byte[]> Rotations { get {  return this.rotations; } }

        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown()
        {
            this.offset += ROW_JUMP_GRID;
        }

        /// <summary>
        /// Move tetromino left.
        /// </summary>
        public void MoveLeft(){ this.offset--; }

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight(){ this.offset++; }

        /// <summary>
        /// Rotate tetromino right.
        /// </summary>
        public void RotateRight()
        {
            this.rotationType++;
            if (this.rotationType == 4) this.rotationType = 0;
            this.indexes = this.rotations[this.rotationType];
            //this.rotatedRight = true;
        }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft()
        {
            this.rotationType--;
            if (this.rotationType == -1) this.rotationType = 3;
            this.indexes = this.rotations[this.rotationType];
            //this.rotatedLeft = true;
        }

        public byte ComputeNrOfBottomPaddingRows()
        {
            byte result = 0;
            bool hasAPieceAtSecondLastRow = false;
            bool hasAPieceAtLastRow = false;

            for(int i = 0; i < this.indexes.Length;i++)
            {
                if (i < 8) continue;
                else if (i < 12 && indexes[i] > 0 && !hasAPieceAtSecondLastRow) hasAPieceAtSecondLastRow = true;
                else if (i >= 12 && indexes[i] > 0 && !hasAPieceAtLastRow) hasAPieceAtLastRow = true;
            }
            if (hasAPieceAtSecondLastRow) result++;
            if(hasAPieceAtLastRow) result++;

            return result;
        }
    }
}
