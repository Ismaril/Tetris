using System.Collections.Generic;
// DONE
namespace Tetris
{
    public class Tetromino
    {
        // -----------------------------------------------------------------------------------------
        // CONSTANS
        public const byte GRID_WIDTH = 4;
        public const byte GRID_HEIGHT = GRID_WIDTH;
        public const byte GRID_SURFACE_AREA = GRID_WIDTH * GRID_HEIGHT;
        public const byte NUMBER_OF_SUBBLOCKS = 4;


        // -----------------------------------------------------------------------------------------
        // FIELDS
        //  ____________ 
        // /__/__/__/__/ 
        private static readonly byte[] I_0 =
        {
            0, 0, 0, 0,
            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
        };
        private static readonly byte[] I_1 =
        {
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
        };


        //   ______
        //  /__/__/
        // /__/__/ 
        private static readonly byte[] O_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //    /__/ 
        private static readonly byte[] T_0 =
        {
            0, 0, 0, 0,
            0, 1, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] T_1 =
        {
            0, 0, 1, 0,
            0, 1, 1, 0,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };
        static readonly byte[] T_2 =
        {
            0, 0, 1, 0,
            0, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] T_3 =
        {
            0, 0, 1, 0,
            0, 0, 1, 1,
            0, 0, 1, 0,
            0, 0, 0, 0,
        };


        //      ______
        //  ___/__/__/
        // /__/__/ 
        private static readonly byte[] S_0 =
        {
            0, 0, 0, 0,
            0, 0, 2, 2,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] S_1 =
        {
            0, 0, 2, 0,
            0, 0, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };


        //   ______
        //  /__/__/__
        //    /__/__/ 
        private static readonly byte[] Z_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };
        private static readonly byte[] Z_1 =
        {
            0, 0, 0, 3,
            0, 0, 3, 3,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };


        //   _________
        //  /__/__/__/
        //       /__/ 
        private static readonly byte[] J_0 =
        {
            0, 0, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 2,
            0, 0, 0, 0,
        };
        private static readonly byte[] J_1 =
        {
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 2, 2, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] J_2 =
        {
            0, 2, 0, 0,
            0, 2, 2, 2,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] J_3 =
        {
            0, 0, 2, 2,
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 0, 0, 0,
        };


        //    _________
        //   /__/__/__/
        //  /__/ 
        private static readonly byte[] L_0 =
        {
            0, 0, 0, 0,
            0, 3, 3, 3,
            0, 3, 0, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] L_1 =
        {
            0, 3, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] L_2 =
        {
            0, 0, 0, 3,
            0, 3, 3, 3,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };
        private static readonly byte[] L_3 =
        {
            0, 0, 3, 0,
            0, 0, 3, 0,
            0, 0, 3, 3,
            0, 0, 0, 0,
        };

        /// <summary>
        /// The type of tetrominos. There are 7 types of tetrominos.
        /// </summary>
        public enum Type
        {
            I,
            O,
            T,
            S,
            Z,
            J,
            L
        }

        /// <summary>
        /// List of 7 tetromino objects. Each object is unique tetromino with its properties.
        /// </summary>
        public static readonly List<Tetromino> TETROMINOS = new List<Tetromino>
        {
            new Tetromino(I_0, I_1, I_0, I_1, (byte)Type.I),
            new Tetromino(O_0, O_0, O_0, O_0, (byte)Type.O),
            new Tetromino(T_0, T_1, T_2, T_3, (byte)Type.T),
            new Tetromino(S_0, S_1, S_0, S_1, (byte)Type.S),
            new Tetromino(Z_0, Z_1, Z_0, Z_1, (byte)Type.Z),
            new Tetromino(J_0, J_1, J_2, J_3, (byte)Type.J),
            new Tetromino(L_0, L_1, L_2, L_3, (byte)Type.L)
        };

        // -----------------------------------------------------------------------------------------
        // CONSTRUCTORS
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Tetromino() { }

        /// <summary>
        /// Constructor which takes in a list of rotations and a type of tetromino. 
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


        // -----------------------------------------------------------------------------------------
        // PROPERTIES
        /// <summary>
        /// 4x4 matrix of bytes which represents the shape of tetromino.
        /// </summary>
        public byte[] Indexes { get; set; }

        /// <summary>
        /// Returns a number between 0 and 3 which represents the rotation of tetromino. 
        /// </summary>
        public sbyte CurrentRotation { get; set; }

        /// <summary>
        /// Returns the type of tetromino.
        /// </summary>
        public byte GetType_ { get; }

        /// <summary>
        /// Returns the offset of tetromino with regards to the matrix. 
        /// With offset we specify where to start drawing the tetromino at main grid. 
        /// </summary>
        public byte Offset { get; set; }

        /// <summary>
        /// Returns a list of four 4x4 matrixes - all possible rotations of tetromino.
        /// </summary>
        public List<byte[]> Rotations { get; }

        /// <summary>
        /// Base rotation of tetromino. 
        /// This is the starting rotation of tetromino when the tetromino is put to grid for the 
        /// first time.
        /// </summary>
        public byte[] BaseRotation { get; }


        // -----------------------------------------------------------------------------------------
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
            CurrentRotation = ResetRotation(CurrentRotation);
            Indexes = Rotations[CurrentRotation];
        }

        /// <summary>
        /// Rotate tetromino left.
        /// </summary>
        public void RotateLeft()
        {
            CurrentRotation--;
            CurrentRotation = ResetRotation(CurrentRotation);
            Indexes = Rotations[CurrentRotation];

        }

        /// <summary>
        /// Helper method.
        /// Adjust rotation if out of bounds.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static sbyte ResetRotation(sbyte rotation)
        {
            switch (rotation)
            {
                case 4:
                    return 0;
                case -1:
                    return 3;
            }
            return rotation;
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
                if (i < 12 && Indexes[i] > 0 && !hasAPieceAtSecondLastRow)
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
