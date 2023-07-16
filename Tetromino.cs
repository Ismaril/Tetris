
using System;

namespace Tetris
{
    public class Tetromino: Constants
    {
        private int[] indexes;
        private string type;
        private int rotationType;
        private int[] previousIndexes;   
        public Tetromino(string tetrominoType)
        {
            this.type = tetrominoType;
            this.previousIndexes = new int[4];
            this.indexes = new int[4];
            this.rotationType = 0;
        }  
        public int[] GetIndexes() { return this.indexes; }
        public int GetPositionOfRotation() {  return this.rotationType; }
        public string GetType_() { return this.type; }
        public int[] GetPreviousIndexes() { return this.previousIndexes; }

        public int[] SendNewFromTop()
        {
            switch(this.type)
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.indexes[i] = I_Default[i];
                    }
                    break;

            }
            return this.indexes;
        }

        public void MoveDown()
        {
            switch(this.type)
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.indexes[i] += ROW_JUMP;
                    }
                    break;
            } 
        }

        public void SubtractToPreviousIndexes()
        {
            this.indexes.CopyTo(this.previousIndexes, 0);

            switch (this.type)
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.previousIndexes[i] -= ROW_JUMP;
                    }
                    break;
            }
        }

        public void MoveLeft()
        {
            switch (this.type)
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.indexes[i]--;
                    }
                    break;
            }
        }

        public void MoveRight()
        {
            switch (this.type)
            {
                case I_type:
                    for (int i = 0; i < 4; i++)
                    {
                        this.indexes[i] ++;
                    }
                    break;
            }
        }

        public void RotateRight() { }
        public void RotateLeft() { }  
    }
}
