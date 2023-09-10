using System.Collections.Generic;

namespace Tetris
{
    public class Tetromino
    {
        // Fields ------------------------------------------------------------------------------
        readonly List<byte[]> rotations;
        readonly byte[] baseRotation;
        readonly byte type;
        sbyte rotationType;
        byte[] indexes;
        byte offset;


        // Constructors ------------------------------------------------------------------------
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Tetromino() { }

        /// <summary>
        /// Constructor which takes in a list of rotations and a type of tetromino. This class is a template for a tetromino objects.
        /// </summary>
        /// <param name="rotation0"></param>
        /// <param name="rotation1"></param>
        /// <param name="rotation2"></param>
        /// <param name="rotation3"></param>
        /// <param name="type"> Type of tetromino </param>
        public Tetromino(
            byte[] rotation0,
            byte[] rotation1,
            byte[] rotation2,
            byte[] rotation3,
            byte type)
        {
            baseRotation = rotation0;
            rotations = new List<byte[]> { rotation0, rotation1, rotation2, rotation3 };
            indexes = baseRotation;
            rotationType = 0;
            this.type = type;
        }


        // Properties-------------------------------------------------------------------------
        /// <summary>
        /// 4x4 matrix of bytes which represents the shape of tetromino.
        /// </summary>
        public byte[] Indexes { get { return indexes; } set { indexes = value; } }

        /// <summary>
        /// Returns a number between 0 and 3 which represents the rotation of tetromino. Check the Consts.cs file for more info.
        /// </summary>
        public sbyte CurrentRotation { get { return rotationType; } set { rotationType = value; } }

        /// <summary>
        /// Returns the type of tetromino.
        /// </summary>
        public byte GetType_ { get { return this.type; } }

        /// <summary>
        /// Returns the offset of tetromino with regards to the matrix. With offset we specify where to start drawing the tetromino. Starting with the top left corner of the tetromino matrix.
        /// </summary>
        public byte Offset { get { return offset; } set { offset = value; } }

        /// <summary>
        /// Returns a list of four 4x4 matrixes - all possible rotations of tetromino.
        /// </summary>
        public List<byte[]> Rotations { get {  return rotations; } }

        /// <summary>
        /// Base rotation of tetromino. This is the starting rotation of tetromino when the tetromino is put to grid for the first time.
        /// </summary>
        public byte[] BaseRotation { get => baseRotation;}


        // Methods ----------------------------------------------------------------------------
        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown(int rowJumpGrid) => offset += (byte)rowJumpGrid;

        /// <summary>
        /// Move tetromino left.
        /// </summary>
        public void MoveLeft() => offset--; 

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight() => offset++;

        /// <summary>
        /// Rotate tetromino right.
        /// </summary>
        public void RotateRight()
        {
            rotationType++;
            if (rotationType == 4) rotationType = 0;
            indexes = rotations[rotationType];
        }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft()
        {
            rotationType--;
            if (rotationType == -1) rotationType = 3;
            indexes = rotations[rotationType];
        }

        /// <summary>
        /// Due to the game matrix having extra two rows below the 20th line, compute how many rows earlier should the tetromino stop at the bottom of game matrix.
        /// </summary>
        /// <returns></returns>
        public byte ComputeNrOfBottomPaddingRows()
        {
            byte result = 0;
            bool hasAPieceAtSecondLastRow = false;
            bool hasAPieceAtLastRow = false;
            for(int i = 0; i < indexes.Length;i++)
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
