
using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetromino : Constants
    {
        private readonly List<byte[]> rotations;
        private readonly byte type;
        private byte[] baseRotation;
        public sbyte rotationType;
        private byte[] indexes;
        private byte offset;

        public Tetromino() { }

        public Tetromino(byte[] rotation0, byte[] rotation1, byte[] rotation2, byte[] rotation3, byte type)
        {
            this.BaseRotation = rotation0;
            this.rotations = new List<byte[]> { rotation0, rotation1, rotation2, rotation3 };
            this.type = type;
            this.indexes = BaseRotation;
            this.rotationType = 0;
        }

        public byte[] Indexes { get { return this.indexes; } set { this.indexes = value; } }
        public sbyte GetPositionOfRotation { get { return this.rotationType; } }
        public byte GetType_ { get { return type; } }
        public byte Offset { get { return this.offset; } set { this.offset = value; } }
        public List<byte[]> Rotations { get {  return this.rotations; } }
        public byte[] BaseRotation { get => baseRotation; set => baseRotation = value; }

        /// <summary>
        /// Move tetrominoCurrent at next row.
        /// </summary>
        public void MoveDown()
        {
            this.offset += ROW_JUMP_GRID;
        }

        /// <summary>
        /// Move tetrominoCurrent left.
        /// </summary>
        public void MoveLeft(){ this.offset--; }

        /// <summary>
        /// Move tetrominoCurrent right.
        /// </summary>
        public void MoveRight(){ this.offset++; }

        /// <summary>
        /// Rotate tetrominoCurrent right.
        /// </summary>
        public void RotateRight()
        {
            this.rotationType++;
            if (this.rotationType == 4) this.rotationType = 0;
            this.indexes = this.rotations[this.rotationType];
        }

        /// <summary>
        /// Rotate tetrominoCurrent left.
        /// </summary>
        public void RotateLeft()
        {
            this.rotationType--;
            if (this.rotationType == -1) this.rotationType = 3;
            this.indexes = this.rotations[this.rotationType];
        }

        /// <summary>
        /// Due to the game matrix having extra two rows below the 20th line, compute how many rows earlier should the tetrominoCurrent matrix stop at the bottom of game matrix.
        /// </summary>
        /// <returns></returns>
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
