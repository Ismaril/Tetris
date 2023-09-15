using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Forms;

namespace Tetris
{
    public class Music
    {
        // ------------------------------------------------------------------------------------------------
        // FIELDS

        Random random = new Random();
        bool musicIsAllowed = true;
        byte currentMainMusicIndex = 0;

        SoundPlayer obstacle = new SoundPlayer(Consts.SOUND_OBSTACLE);
        SoundPlayer lineCleared = new SoundPlayer(Consts.SOUND_LINE_CLEARED);
        SoundPlayer tetris = new SoundPlayer(Consts.SOUND_TETRIS);
        SoundPlayer nextLevel = new SoundPlayer(Consts.SOUND_NEXTLEVEL);
        SoundPlayer settings = new SoundPlayer(Consts.SOUND_SETTINGS);

        AudioFileReader backgroundMusicReader;

        public List<AudioFileReader> toBeDisposed1 = new List<AudioFileReader>();
        public List<WaveOutEvent> toBeDisposed2 = new List<WaveOutEvent>();

        public List<AudioFileReader> toBeDisposed3 = new List<AudioFileReader>();
        public List<WaveOutEvent> toBeDisposed4 = new List<WaveOutEvent>();

        public List<AudioFileReader> toBeDisposed5 = new List<AudioFileReader>();
        public List<WaveOutEvent> toBeDisposed6 = new List<WaveOutEvent>();


        // ------------------------------------------------------------------------------------------------
        // CONSTRUCTOR
        /// <summary>
        /// Constructor
        /// </summary>
        public Music() { }


        // ------------------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Controls if music is allowed or not.
        /// </summary>
        public bool MusicIsAllowed { get => musicIsAllowed; set => musicIsAllowed = value; }


        // ------------------------------------------------------------------------------------------------
        // METHODS

        /// <summary>
        /// Play this music when the game is over and the player has high score.
        /// </summary>
        public void TetrisMaster()
        {
            AudioFileReader backgroundTetrisMaster = new AudioFileReader(Consts.MUSIC_BACKGROUND_TETRISMASTER);
            WaveOutEvent backgroundMusicPlayerTetrisMaster = new WaveOutEvent();

            backgroundMusicPlayerTetrisMaster.Init(backgroundTetrisMaster);
            backgroundMusicPlayerTetrisMaster.Play();
            toBeDisposed5.Add(backgroundTetrisMaster);
            toBeDisposed6.Add(backgroundMusicPlayerTetrisMaster);
        }

        /// <summary>
        /// Play this sound when tetromino is moved to the sides.
        /// </summary>
        public void MoveToSides()
        {
            AudioFileReader addressToSides = new AudioFileReader(Consts.SOUND_MOVE_TO_SIDES);
            WaveOutEvent moveToSidesUserEvent = new WaveOutEvent();

            moveToSidesUserEvent.Init(addressToSides);
            moveToSidesUserEvent.Play();

            toBeDisposed1.Add(addressToSides);
            toBeDisposed2.Add(moveToSidesUserEvent);

        }

        /// <summary>
        /// Play this sound when tetromino is rotated.
        /// </summary>
        public void Rotate()
        {
            AudioFileReader addressRotate = new AudioFileReader(Consts.SOUND_ROTATE);
            WaveOutEvent rotateUserEvent = new WaveOutEvent();
            rotateUserEvent.Init(addressRotate);
            rotateUserEvent.Play();

            toBeDisposed1.Add(addressRotate);
            toBeDisposed2.Add(rotateUserEvent);
        }

        /// <summary>
        /// This is main music. 
        /// It is played during the game and when played tetrominos 
        /// have not reached the certain treshold of the game matrix.
        /// </summary>
        public void MainMusicSlow()
        {

            DisposeMusic(case_: 3);

            switch (currentMainMusicIndex)
            {
                case 0:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND1_SLOW);
                    break;
                case 1:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND2_SLOW);
                    break;
                case 2:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND3_SLOW);
                    break;
            }
            WaveOutEvent backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(backgroundMusicReader);
            backgroundMusicPlayer.Play();

            toBeDisposed3.Add(backgroundMusicReader);
            toBeDisposed4.Add(backgroundMusicPlayer);
        }

        /// <summary>
        /// This is main music. 
        /// It is played during the game and when played tetrominos 
        /// have reached the certain treshold of the game matrix.
        /// </summary>
        public void MainMusicFast()
        {
            DisposeMusic(case_: 3);

            switch (currentMainMusicIndex)
            {
                case 0:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND1_FAST);
                    break;
                case 1:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND2_FAST);
                    break;
                case 2:
                    backgroundMusicReader = new AudioFileReader(Consts.MUSIC_BACKGROUND3_FAST);
                    break;
            }
            WaveOutEvent backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(backgroundMusicReader);
            backgroundMusicPlayer.Play();

            toBeDisposed3.Add(backgroundMusicReader);
            toBeDisposed4.Add(backgroundMusicPlayer);
        }

        // Not implemented yet
        //public void GetPositionOfMainMusic()
        //{
        //    // 63556920
        //    // 63551992
        //    if (musicIsAllowed)
        //    {
        //        if (backgroundMusicPlayerSlow.GetPosition() > (long)10000000)
        //        {
        //            MainMusicSlow();
        //        }
        //        Console.WriteLine(backgroundMusicPlayerSlow.GetPosition());
        //    }
        //}

        /// <summary>
        /// Chose a randomly main music. There are 3 main songs.
        /// </summary>
        public void ChoseMainMusic() => currentMainMusicIndex = (byte)random.Next(0, 3);

        /// <summary>
        /// Pley this sound when the player reaches the next level.
        /// </summary>
        public void NextLevel() => nextLevel.Play();

        /// <summary>
        /// Play this sound when tetromino arrived at obstacle.
        /// </summary>
        public void Obstacle() => obstacle.Play(); 

        /// <summary>
        /// Play this sound when a line is cleared.
        /// </summary>
        public void LineCleared() => lineCleared.Play();

        /// <summary>
        /// Play this sound when player clears 4 lines at once.
        /// </summary>
        public void Tetris() => tetris.Play();

        /// <summary>
        /// Play this sound when player moves around settings.
        /// </summary>
        public void SoundSettings() => settings.Play();

        /// <summary>
        /// Play this sound when the game is over.
        /// </summary>
        public void GameOver()
        {
            AudioFileReader addressGameOver = new AudioFileReader(Consts.SOUND_GAME_OVER);
            WaveOutEvent gameOver = new WaveOutEvent();
            gameOver.Init(addressGameOver);
            gameOver.Play();

            toBeDisposed1.Add(addressGameOver);
            toBeDisposed2.Add(gameOver);
        }

        /// <summary>
        /// Dispose music.
        /// </summary>
        /// <param name="case_">Based on case, select which objects should be disposed</param>
        public void DisposeMusic(byte case_)
        {
            switch (case_)
            {
                case 1:
                    foreach (AudioFileReader audioFileReader in toBeDisposed1)
                    {
                        audioFileReader.Dispose();
                    }

                    foreach (WaveOutEvent audioFileReader in toBeDisposed2)
                    {
                        audioFileReader.Dispose();
                    }
                    toBeDisposed1.Clear();
                    toBeDisposed2.Clear();
                    break;
                case 3:
                    foreach (AudioFileReader audioFileReader in toBeDisposed3)
                    {
                        audioFileReader.Dispose();
                    }

                    foreach (WaveOutEvent audioFileReader in toBeDisposed4)
                    {
                        audioFileReader.Dispose();
                    }
                    toBeDisposed3.Clear();
                    toBeDisposed4.Clear();
                    break;
                case 5:
                    foreach (AudioFileReader audioFileReader in toBeDisposed5)
                    {
                        audioFileReader.Dispose();
                    }

                    foreach (WaveOutEvent audioFileReader in toBeDisposed6)
                    {
                        audioFileReader.Dispose();
                    }
                    toBeDisposed5.Clear();
                    toBeDisposed6.Clear();
                    break;
            }
        }
    }
}

// Todo: There is an exception when disposing the music but music is still playing. (Reproducible)
