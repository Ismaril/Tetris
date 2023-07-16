
using System;

namespace Tetris
{
    public class Tetromino: Constants
    {
        private string type;
        private int[] indexes;
        private int rotationType;
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

        public int[] GetIndexes { get { return indexes; } }
        public int[] GetPreviousIndexes { get { return previousIndexes; } }
        public int GetPositionOfRotation { get { return this.rotationType; } }
        public string GetType_ { get { return this.type; } } 

        public int[] SendNewFromTop()
        {
            return this.indexes;
        }

        public void MoveDown()
        {
            for (int i = 0; i < 4; i++)
            {
                this.indexes[i] += ROW_JUMP;
            }
        }

        public void SubtractToPreviousIndexes()
        {
            this.indexes.CopyTo(this.previousIndexes, 0);

            for (int i = 0; i < 4; i++)
            {
                this.previousIndexes[i] -= ROW_JUMP;
            }
        }

        public void MoveLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                this.indexes[i]--;
            }
            this.redraw(this.indexes);
            this.redrawBack(this.previousIndexes);
        }

        public void MoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                this.indexes[i] ++;
            }
            this.redraw(this.indexes);
            this.redrawBack(this.previousIndexes);
        }

        public void RotateRight() { }
        public void RotateLeft() { } 

        public void ChoseNewType() // Sjednotit s send from top?
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
        }
    }
}
