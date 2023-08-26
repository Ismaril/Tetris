using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using NAudio.Wave;

namespace Tetris
{
    public class Logic
    {
        //SoundPlayer soundMoveToSides = new SoundPlayer(@"../../Music/SFX 4.wav");
        //SoundPlayer soundRotate = new SoundPlayer(@"../../Music/SFX 6.wav");
        SoundPlayer soundObstacle = new SoundPlayer(@"../../Music/SFX 8.wav");
        SoundPlayer soundLineCleared = new SoundPlayer(@"../../Music/SFX 11.wav");
        //SoundPlayer soundGameOver = new SoundPlayer(@"../../Music/SFX 14.wav");
        SoundPlayer soundTetris = new SoundPlayer(@"../../Music/SFX TetrisClear.wav");

        // Initialize the background music player
        //WaveOutEvent backgroundMusicPlayer = new WaveOutEvent();
        //AudioFileReader backgroundMusicReader = new AudioFileReader(@"../../Music/1 - Music 1.wav");

        // Create the user event sound player
        //WaveOut userEventSoundPlayer = new WaveOut();
        //AudioFileReader userEventSoundReader = new AudioFileReader(@"../../Music/SFX 4.wav");
        private Music music = new Music();
        private List<byte> matrix = new List<byte>();
        public int[] movementTicksBasedOnLevel = {
            Constants.GUI_TICK * 48,
            Constants.GUI_TICK * 43,
            Constants.GUI_TICK * 38,
            Constants.GUI_TICK * 33,
            Constants.GUI_TICK * 28,
            Constants.GUI_TICK * 23,
            Constants.GUI_TICK * 18,
            Constants.GUI_TICK * 13,
            Constants.GUI_TICK * 8,
            Constants.GUI_TICK * 6,
            Constants.GUI_TICK * 5,
            Constants.GUI_TICK * 5,
            Constants.GUI_TICK * 5,
            Constants.GUI_TICK * 4,
            Constants.GUI_TICK * 4,
            Constants.GUI_TICK * 4,
            Constants.GUI_TICK * 3,
            Constants.GUI_TICK * 3,
            Constants.GUI_TICK * 3,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 2,
            Constants.GUI_TICK * 1,
        };
        public int speed;
        private bool moveRight = false;
        private bool moveLeft = false;
        private bool rotateRight = false;
        private bool rotateLeft = false;
        private bool moveDownFast = false;


        private bool atGridBoundary = false;
        private bool cannotMoveRight = false;
        private bool cannotMoveLeft = false;
        public bool cannotMoveDown = false;
        private bool cannotRotateRight = false;
        private bool cannotRotateLeft = false;

        public bool sendNextPiece = true;
        public bool putTetrominoOnGrid = true; 
        public bool startRemoving = false;
        public bool removeTetromino = false;

        private int timer = 0;
        public byte currentRow = 0;
        private byte nrOfSessionRows = 0;

        public bool gameStarted = false;
        private bool roundEnded = false;
        private bool totalGameEnded = false;
        private int scoreIncrementor = 0;
        private byte currentLevel = 0;
        private byte linesNextLevel = 0; // has to be 10 in order to continue to next level
        private byte[] tetrominoNext = new byte[16];
        private byte tetrominoNextIndex = 99;
        public bool skipLogicMain = false;

        private Tetromino tetrominoCurrent = new Tetromino();

        private Tetromino I_tetromino = new Tetromino(Constants.I_0, Constants.I_1, Constants.I_0, Constants.I_1, (byte)Constants.TetrominoType.I_type);
        private Tetromino O_tetromino = new Tetromino(Constants.O_0, Constants.O_0, Constants.O_0, Constants.O_0, (byte)Constants.TetrominoType.O_type);
        private Tetromino T_tetromino = new Tetromino(Constants.T_0, Constants.T_1, Constants.T_2, Constants.T_3, (byte)Constants.TetrominoType.T_type);
        private Tetromino S_tetromino = new Tetromino(Constants.S_0, Constants.S_1, Constants.S_0, Constants.S_1, (byte)Constants.TetrominoType.S_type);
        private Tetromino Z_tetromino = new Tetromino(Constants.Z_0, Constants.Z_1, Constants.Z_0, Constants.Z_1, (byte)Constants.TetrominoType.Z_type);
        private Tetromino J_tetromino = new Tetromino(Constants.J_0, Constants.J_1, Constants.J_2, Constants.J_3, (byte)Constants.TetrominoType.J_type);
        private Tetromino L_tetromino = new Tetromino(Constants.L_0, Constants.L_1, Constants.L_2, Constants.L_3, (byte)Constants.TetrominoType.L_type);
        private List<Tetromino> tetrominos;
        private Action<List<byte>> redraw;
        public List<byte> toBeRemoved = new List<byte>();
        public List<byte> collisionDetection = new List<byte>();

        bool removeUpToThreeLines = false;
        bool removingLinesSoundIsPlaying = false;

        Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="redrawBack"></param>
        public Logic(Action<List<byte>> redraw)
        {
            this.redraw = redraw;
            tetrominos = new List<Tetromino> { I_tetromino, O_tetromino, T_tetromino, S_tetromino, Z_tetromino, J_tetromino, L_tetromino };
            speed = movementTicksBasedOnLevel[CurrentLevel];
            for(int i = 0; i < Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID; i++) Matrix.Add(0);


            music.MainMusic(); //uncommment

            
            //userEventSoundPlayer.Init(userEventSoundReader);
            
            // Dispose of resources when done
            //backgroundMusicPlayer.Dispose();
        }

        public Tetromino Tetromino { get { return tetrominoCurrent; } }
        public int Timer { get { return timer; } set { timer = value; } }
        public bool MoveRight { get => moveRight; set => moveRight = value; }
        public bool MoveLeft { get => moveLeft; set => moveLeft = value; }
        public bool MoveDownFast { get => moveDownFast; set => moveDownFast = value; }
        public bool RotateRight { get => rotateRight; set => rotateRight = value; }
        public bool RotateLeft { get => rotateLeft; set => rotateLeft = value; }
        public int ScoreIncrementor { get => scoreIncrementor; set => scoreIncrementor = value; }
        public byte[] TetrominoNext { get => tetrominoNext; set => tetrominoNext = value; }
        public byte CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public bool RoundEnded1 { get => roundEnded; set => roundEnded = value; }
        public List<byte> Matrix { get => matrix; set => matrix = value; }


        /// <summary>
        /// Actualises the selected matrix indexes to 1 from indexes of tetrominoCurrent object.
        /// </summary>
        /// <param name="tetrominoMatrix"></param>
        private void PutTetrominoOnGrid(byte[] tetrominoMatrix, byte offset, bool active)
        {
            if(!active) return;



            byte columnRelative = 0;
            for (byte i = 0; i < tetrominoMatrix.Length; i++)
            {
                if (columnRelative % 4 == 0 && i > 0)
                {
                    columnRelative = 0;
                    offset += (byte)Constants.ROW_JUMP_GRID;
                }
                byte gridIndexOffseted = (byte)(offset + columnRelative);

                if (tetrominoMatrix[i] > 0)
                {
                    Matrix[gridIndexOffseted] = tetrominoMatrix[i];
                    toBeRemoved.Add((byte)(gridIndexOffseted));
                }
                columnRelative++;
            }
            startRemoving = true;
            for (int i = 0; i < 20; i++)
            {
                Matrix[i] = 0;
            }
        }

        /// <summary>
        /// Actualises the selected matrix indexes to 0.
        /// </summary>
        /// <param name="tetrominoIndexes"></param>
        private void RemoveTetrominoFromGrid(bool activate)
        {
            foreach (byte i in toBeRemoved)
            {
                if (Matrix.Contains(i)) continue;
                Matrix[i] = 0;
            }
             //toBeRemoved.Clear();
             removeTetromino = false;


        }

        /// <summary>
        /// Checks wheter there is a tetrominoCurrent at next row.
        /// If yes, current tetrominoCurrent has to stop on current row.
        /// </summary>
        /// <returns></returns>
        public bool TetrominoHasObstacleAtNextRow()
        {
            if (toBeRemoved.Count > 0)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + (byte)Constants.ROW_JUMP_GRID))))
                    {
                        collisionDetection.Add((byte)this.toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((Matrix[collisionDetection[i] + Constants.ROW_JUMP_GRID] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                }
                collisionDetection.Clear();
            }
            return false;
        }

        public bool TetrominaHasObstacleAtNextColumn(bool checkRightside)
        {

            sbyte operator_;
            if (checkRightside) operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (!(toBeRemoved.Contains((byte)(toBeRemoved[i] + operator_))))
                    {
                        collisionDetection.Add((byte)toBeRemoved[i]);
                    }
                }

                for (byte i = 0; i < collisionDetection.Count; i++)
                {
                    if ((Matrix[collisionDetection[i] + operator_] > 0))
                    {
                        collisionDetection.Clear();
                        return true;
                    }

                }
                collisionDetection.Clear();
            }
            return false;
        }

        public bool TetrominaHasObstacleAtNextRotation(bool checkRightRotation, byte offset)
        {
            sbyte operator_;

            if (checkRightRotation)operator_ = 1;
            else operator_ = -1;

            if (toBeRemoved.Count > 0)
            {
                sbyte rotationType = (sbyte)(tetrominoCurrent.GetPositionOfRotation + operator_);
                if (rotationType == -1) rotationType = 3;
                else if (rotationType == 4) rotationType = 0;
                byte[] tetrominoMatrix = tetrominoCurrent.Rotations[rotationType];
                byte columnRelative = 0;

                for (byte i = 0; i < tetrominoMatrix.Length; i++)
                {
                    if (columnRelative % 4 == 0 && i > 0)
                    {
                        columnRelative = 0;
                        offset += (byte)Constants.ROW_JUMP_GRID;
                    }

                    byte gridIndexOffseted = (byte)((offset + columnRelative));
                    if ((tetrominoMatrix[i] > 0 && Matrix[gridIndexOffseted] > 0)
                        || (offset % 10 == 8 || offset % 10 == 7)
                        || (tetrominoCurrent.GetType_ == (byte)Constants.TetrominoType.I_type && offset % 10 == 9))
                    {
                        collisionDetection.Clear();
                        return true;
                    }
                    columnRelative++;
                }
            }
            collisionDetection.Clear();
            return false;
        }

        /// <summary>
        /// Check if tetrominoCurrent is at bottom of the grid.
        /// </summary>
        /// <returns></returns>
        private bool TetrominoIsAtBottom()
        {
            if(tetrominoCurrent.GetType_ == (byte)Constants.TetrominoType.I_type)
            {
                return currentRow == Constants.LAST_ROW - tetrominoCurrent.ComputeNrOfBottomPaddingRows() + 1;
            }
            return currentRow == Constants.LAST_ROW - tetrominoCurrent.ComputeNrOfBottomPaddingRows();
        }

        /// <summary>
        /// Notify a player that the current game round has ended. Game lost.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void RoundEnded()
        {
            if (RoundEnded1)
            {
                //Console.WriteLine("You lost");
                RoundEnded1 = false;
                //soundGameOver.Play();//////////////////////////////////////////////////////////////////////////
                music.GameOver();
                //throw new Exception("Game ended");
                skipLogicMain = true;
            }
        }

        public void MoveRight_UserEvent(bool canMoveRight, bool desiredRight)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 9) return;
            }
            if (canMoveRight && desiredRight)
            {
                tetrominoCurrent.MoveRight();
                music.MoveToSides();


            }

        }

        public void MoveLeft_UserEvent(bool canMoveLeft, bool desiredLeft)
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (toBeRemoved[i] % Constants.ROW_JUMP_GRID == 0) return;
            }
            if (canMoveLeft && desiredLeft)
            {
                tetrominoCurrent.MoveLeft();
                music.MoveToSides();


            }
        }

        private void MoveDownFaster_UserEvent(bool activate)
        {
            if (!activate) return;
            timer = movementTicksBasedOnLevel[CurrentLevel];
            MoveDownFast = false;
        }

        public void RotateLeft_UserEvent(bool canRotateLeft, bool desiredRotationLeft)
        {
            if (canRotateLeft && desiredRotationLeft)
            {
                tetrominoCurrent.RotateLeft();
                //soundRotate.Play();
                music.Rotate();
            }
        }

        public void RotateRight_UserEvent(bool canRotateRight, bool desiredRotationRight)
        {
            if (canRotateRight && desiredRotationRight)
            {
                tetrominoCurrent.RotateRight();
                //soundRotate.Play();
                music.Rotate();
            }
        }

    //private void GameStarted()
    //{
    //    if (!gameStarted)
    //    {
    //        gameStarted = true;
    //        //this.ChoseNextTetromino();
    //        this.PutTetrominoOnGrid(this.tetrominoCurrent.Indexes, this.tetrominoCurrent.Offset);
    //        this.redraw(this.matrix);
    //    }
    //}

    private void DefaultTetrominoMovement()
        {
            if (timer >= movementTicksBasedOnLevel[CurrentLevel])
            {
                if (TetrominoHasObstacleAtNextRow() || TetrominoIsAtBottom())
                {
                    if (currentRow == 0) RoundEnded1 = true;
                    sendNextPiece = true;
                    currentRow = 0;
                    toBeRemoved.Clear();
                    removeTetromino = false;
                    soundObstacle.Play();/////////////////////////////////////////////////////////////////////////
                    //music.Obstacle();
                }
                else
                {
                    removeTetromino = true;
                    tetrominoCurrent.MoveDown();
                    currentRow++;
                    nrOfSessionRows++;
                    putTetrominoOnGrid = true;
                }
                timer = 0;
                removingLinesSoundIsPlaying = false;
            }
        }

        private void TotalGameEnded()
        {
            if (totalGameEnded) { }
        }

        /// <summary>
        /// Prepare one random tetrominoCurrent at the starting position at the top of grid.
        /// </summary>
        /// <returns>byte[]</returns>
        public void ChoseNextTetromino()
        {
            if (sendNextPiece)
            {
                Random random = new Random();
                if(tetrominoNextIndex == 99)
                {
                    tetrominoCurrent = tetrominos[random.Next(Constants.MIN_NR_OF_TETROMINOS, Constants.MAX_NR_OF_TETROMINOS)];
                 
                }
                else tetrominoCurrent = tetrominos[tetrominoNextIndex];

                if (tetrominoCurrent.GetType_ == (byte)Constants.TetrominoType.I_type)
                {
                    tetrominoCurrent.Offset = 3;
                }
                else
                {
                    tetrominoCurrent.Offset = 3 + Constants.ROW_JUMP_GRID;
                }

                tetrominoCurrent.Indexes = tetrominoCurrent.BaseRotation;
                tetrominoCurrent.rotationType = 0;
                sendNextPiece = false;

                tetrominoNextIndex = (byte)random.Next(Constants.MIN_NR_OF_TETROMINOS, Constants.MAX_NR_OF_TETROMINOS);
                Array.Copy(tetrominos[tetrominoNextIndex].BaseRotation, TetrominoNext, TetrominoNext.Length);


            }
        }

        public void RemoveCompleteRows()
        {
            List<byte> indexesOfCompletedRows = new List<byte> { };
            byte[] rows = new byte[10];

            for (byte i = 0; i < Constants.HEIGHT_OF_GRID; i++)
            {
                Array.Copy(Matrix.ToArray(), i * 10, rows, 0, 10);
                if (rows.All(x => x > 0)) indexesOfCompletedRows.Add(i);
            }

            foreach (byte row in indexesOfCompletedRows)
            {
                Matrix.RemoveRange(row*10, 10);
                Matrix.InsertRange(0, new byte[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            ComputeScore((byte)indexesOfCompletedRows.Count);

            if (indexesOfCompletedRows.Count > 0 && indexesOfCompletedRows.Count <= 3)
            {
                soundLineCleared.Play();////////////////////////////////////////////////////////////////////////
                //music.LineCleared();
                System.Threading.Thread.Sleep(450);
            }
            else if(indexesOfCompletedRows.Count == 4)
            {
                soundTetris.Play();/////////////////////////////////////////////////////////////////////////////////
                //music.Tetris();
                System.Threading.Thread.Sleep(450);
            }
            linesNextLevel += (byte)indexesOfCompletedRows.Count;
        }

        private void CheckIfContinueToNextLevel()
        {
            if(linesNextLevel >= 10)
            {
                linesNextLevel = 0;
                currentLevel++;
            }
        }

        private void ComputeScore(byte numberOfFinishedRows)
        {
            if (numberOfFinishedRows == 0) return;
            else if (numberOfFinishedRows == 1) ScoreIncrementor += (40 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 2) ScoreIncrementor += (100 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 3) ScoreIncrementor += (300 * (CurrentLevel + 1));
            else if (numberOfFinishedRows == 4) ScoreIncrementor += (1200 * (CurrentLevel + 1));
        }
        private void SetAllFlagsToFalse()
        {
            MoveRight = false;
            MoveLeft = false;
            cannotMoveLeft = false;
            cannotMoveRight = false;
            RotateRight = false;
            RotateLeft = false;
            atGridBoundary = false;
            removeUpToThreeLines = false;
        }
        private void SetCollisionFlags()
        {
            cannotMoveRight = TetrominaHasObstacleAtNextColumn(checkRightside: true);
            cannotMoveLeft = TetrominaHasObstacleAtNextColumn(checkRightside: false);
            cannotRotateRight = TetrominaHasObstacleAtNextRotation(checkRightRotation: true, offset: tetrominoCurrent.Offset);
            cannotRotateLeft = TetrominaHasObstacleAtNextRotation(checkRightRotation: false, offset: tetrominoCurrent.Offset);
            cannotMoveDown = TetrominoHasObstacleAtNextRow();
        }

        public void Main__()
        {

            //GameStarted();
            RoundEnded();
            
            if (skipLogicMain) return;
            //TotalGameEnded();
            SetCollisionFlags();
            RotateRight_UserEvent(!cannotRotateRight, RotateRight);
            RotateLeft_UserEvent(!cannotRotateLeft, RotateLeft);
            MoveRight_UserEvent(!cannotMoveRight, MoveRight);
            MoveLeft_UserEvent(!cannotMoveLeft, MoveLeft);
            MoveDownFaster_UserEvent(activate: MoveDownFast);

            toBeRemoved.Clear();
            ChoseNextTetromino();
            PutTetrominoOnGrid(this.tetrominoCurrent.Indexes, tetrominoCurrent.Offset, putTetrominoOnGrid);
            DefaultTetrominoMovement();

            redraw(Matrix);

            RemoveTetrominoFromGrid(activate: removeTetromino);
            RemoveCompleteRows();
            CheckIfContinueToNextLevel();
            SetAllFlagsToFalse();

            if (timer >= movementTicksBasedOnLevel[CurrentLevel])
                music.Dispose();


        }
    }
}
