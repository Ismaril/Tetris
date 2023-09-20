using System.Collections.Generic;
using static Tetris.Consts;

namespace Tetris
{
    public class Tetromino
    {
        // ------------------------------------------------------------------------------------------------
        // FIELDS
        //  ____________ 
        // /__/__/__/__/ 
        public static readonly byte[] I_0 =
        {
            0, 0, 0, 0,
            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
        };
        public static readonly byte[] I_1 =
{
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
        };


        //   ______
        //  /__/__/
        // /__/__/ 
        public static readonly byte[] O_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //    /__/ 
        public static readonly byte[] T_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_1 =
{
            0, 0, 1, 0,
            0, 1, 1, 0,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_2 =
{
            0, 0, 1, 0,
            0, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] T_3 =
{
            0, 0, 1, 0,
            0, 0, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };


        //      ______
        //  ___/__/__/
        // /__/__/ 
        public static readonly byte[] S_0 =
        {
            0, 0, 0, 0,
            0, 0, 2, 2,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] S_1 =
{
            0, 0, 2, 0,
            0, 0, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };


        //   ______
        //  /__/__/__
        //    /__/__/ 
        public static readonly byte[] Z_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };
        public static readonly byte[] Z_1 =
{
            0, 0, 0, 3,
            0, 0, 3, 3,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //       /__/ 
        public static readonly byte[] J_0 =
        {
            0, 0, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_1 =
        {
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_2 =
        {
            0, 2, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] J_3 =
        {
            0, 0, 2, 2,
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 0, 0, 0,
        };


        //    _________
        //   /__/__/__/
        //  /__/ 
        public static readonly byte[] L_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 3,
            0, 3, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_1 =
{
            0, 3, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_2 =
{
            0, 0, 0, 3,
            0, 3, 3, 3,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        public static readonly byte[] L_3 =
{
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };
        
        /// <summary>
        /// List of 7 tetromino objects. Each object is unique tetromino with its properties.
        /// </summary>
        public static readonly List<Tetromino> TETROMINOS = new List<Tetromino>
        {
            new Tetromino(I_0, I_1, I_0, I_1, (byte)TetrominoType.I_type),
            new Tetromino(O_0, O_0, O_0, O_0, (byte)TetrominoType.O_type),
            new Tetromino(T_0, T_1, T_2, T_3, (byte)TetrominoType.T_type),
            new Tetromino(S_0, S_1, S_0, S_1, (byte)TetrominoType.S_type),
            new Tetromino(Z_0, Z_1, Z_0, Z_1, (byte)TetrominoType.Z_type),
            new Tetromino(J_0, J_1, J_2, J_3, (byte)TetrominoType.J_type),
            new Tetromino(L_0, L_1, L_2, L_3, (byte)TetrominoType.L_type)
        };

        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTORS
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Tetromino() { }

        /// <summary>
        /// Constructor which takes in a list of rotations and a type of tetromino. 
        /// This class is a template for a tetromino objects.
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
            BaseRotation = rotation0;
            Rotations = new List<byte[]> { rotation0, rotation1, rotation2, rotation3 };
            Indexes = BaseRotation;
            CurrentRotation = 0;
            this.GetType_ = type;
        }


        // ------------------------------------------------------------------------------------------------
        // PROPERTIES
        /// <summary>
        /// 4x4 matrix of bytes which represents the shape of tetromino.
        /// </summary>
        public byte[] Indexes { get; set; }

        /// <summary>
        /// Returns a number between 0 and 3 which represents the rotation of tetromino. 
        /// Check the Consts.cs file for more info.
        /// </summary>
        public sbyte CurrentRotation { get; set; }

        /// <summary>
        /// Returns the type of tetromino.
        /// </summary>
        public byte GetType_ { get; }

        /// <summary>
        /// Returns the offset of tetromino with regards to the matrix. 
        /// With offset we specify where to start drawing the tetromino. 
        /// Starting with the top left corner of the tetromino matrix.
        /// </summary>
        public byte Offset { get; set; }

        /// <summary>
        /// Returns a list of four 4x4 matrixes - all possible rotations of tetromino.
        /// </summary>
        public List<byte[]> Rotations { get; }

        /// <summary>
        /// Base rotation of tetromino. 
        /// This is the starting rotation of tetromino when the tetromino is put to grid for the first time.
        /// </summary>
        public byte[] BaseRotation { get; }


        // ------------------------------------------------------------------------------------------------
        // METHODS
        /// <summary>
        /// Move tetromino at next row.
        /// </summary>
        public void MoveDown(int rowJumpGrid) => Offset += (byte)rowJumpGrid;

        /// <summary>
        /// Move tetromino left.
        /// </summary>
        public void MoveLeft() => Offset--; 

        /// <summary>
        /// Move tetromino right.
        /// </summary>
        public void MoveRight() => Offset++;

        /// <summary>
        /// Rotate tetromino right.
        /// </summary>
        public void RotateRight()
        {
            CurrentRotation++;
            if (CurrentRotation == 4) CurrentRotation = 0;
            Indexes = Rotations[CurrentRotation];
        }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft()
        {
            CurrentRotation--;
            if (CurrentRotation == -1) CurrentRotation = 3;
            Indexes = Rotations[CurrentRotation];
        }

        /// <summary>
        /// Due to the game matrix having extra two rows below the 20th line, 
        /// compute how many rows earlier should the tetromino stop at the bottom of game matrix.
        /// </summary>
        /// <returns></returns>
        public byte ComputeNrOfBottomPaddingRows()
        {
            byte result = 0;
            var hasAPieceAtSecondLastRow = false;
            var hasAPieceAtLastRow = false;
            for (var i = 0; i < Indexes.Length;i++)
            {
                
                if (i < 8)
                    continue;
                else if (i < 12 && Indexes[i] > 0 && !hasAPieceAtSecondLastRow)
                    hasAPieceAtSecondLastRow = true;
                else if (i >= 12 && Indexes[i] > 0 && !hasAPieceAtLastRow)
                    hasAPieceAtLastRow = true;
            }
            if (hasAPieceAtSecondLastRow)
                result++;
            if(hasAPieceAtLastRow)
                result++;
            return result;
        }
    }
}
