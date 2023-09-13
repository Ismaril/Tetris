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

        static readonly string path = @"../../Music/";

        static readonly string soundRotate = path + @"SFX 6.mp3";
        static readonly string soundMoveToSides = path + @"SFX 4.mp3";
        static readonly string soundGameOver = path + @"SFX 14.mp3";
        static readonly string soundObstacle = path + @"SFX 8.wav";
        static readonly string soundLineCleared = path + @"SFX 11.wav";
        static readonly string soundTetris = path + @"SFX TetrisClear.wav";
        static readonly string soundNextLevel = path + @"SFX 7.wav";
        static readonly string soundSettings = path + @"SFX 2.wav";

        static readonly string backgroundMusic1_Slow = path + @"1 - Music 1.mp3";
        static readonly string backgroundMusic2_Slow = path + @"2 - Music 2.mp3";
        static readonly string backgroundMusic3_Slow = path + @"3 - Music 3.mp3";
        static readonly string backgroundMusic1_Fast = path + @"8 - Track 8.mp3";
        static readonly string backgroundMusic2_Fast = path + @"9 - Track 9.mp3";
        static readonly string backgroundMusic3_Fast = path + @"10 - Track 10.mp3";
        static readonly string backgroundMusicTetrisMaster = path + @"6 - High Score (Tetris Master).mp3";



        WaveOutEvent backgroundMusicPlayerSlow = new WaveOutEvent();
        WaveOutEvent backgroundMusicPlayerFast = new WaveOutEvent();
        WaveOutEvent backgroundMusicPlayerTetrisMaster = new WaveOutEvent();

        SoundPlayer obstacle = new SoundPlayer(soundObstacle);
        SoundPlayer lineCleared = new SoundPlayer(soundLineCleared);
        SoundPlayer tetris = new SoundPlayer(soundTetris);
        SoundPlayer nextLevel = new SoundPlayer(soundNextLevel);
        SoundPlayer settings = new SoundPlayer(soundSettings);

        // Initialize the background music player
        static AudioFileReader backgroundMusicReader1 = new AudioFileReader(backgroundMusic1_Slow);
        static AudioFileReader backgroundMusicReader2 = new AudioFileReader(backgroundMusic2_Slow);
        static AudioFileReader backgroundMusicReader3 = new AudioFileReader(backgroundMusic3_Slow);
        static AudioFileReader backgroundMusicReader10 = new AudioFileReader(backgroundMusic1_Fast);
        static AudioFileReader backgroundMusicReader20 = new AudioFileReader(backgroundMusic2_Fast);
        static AudioFileReader backgroundMusicReader30 = new AudioFileReader(backgroundMusic3_Fast);
        static AudioFileReader backgroundTetrisMaster = new AudioFileReader(backgroundMusicTetrisMaster);



        List<AudioFileReader> slowBackgroundMusic = new List<AudioFileReader>() { backgroundMusicReader1, backgroundMusicReader2, backgroundMusicReader3 };
        List<AudioFileReader> fastBackgroundMusic = new List<AudioFileReader>() { backgroundMusicReader10, backgroundMusicReader20, backgroundMusicReader30 };

        public List<AudioFileReader> toBeDisposed1 = new List<AudioFileReader>();
        public List<WaveOutEvent> toBeDisposed2 = new List<WaveOutEvent>();

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
            backgroundMusicPlayerTetrisMaster.Init(backgroundTetrisMaster);
            backgroundMusicPlayerTetrisMaster.Play();
        }

        /// <summary>
        /// Play this sound when tetromino is moved to the sides.
        /// </summary>
        public void MoveToSides()
        {
            AudioFileReader addressToSides = new AudioFileReader(soundMoveToSides);
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
            AudioFileReader addressRotate = new AudioFileReader(soundRotate);
            WaveOutEvent rotateUserEvent = new WaveOutEvent();
            rotateUserEvent.Init(addressRotate);
            rotateUserEvent.Play();

            toBeDisposed1.Add(addressRotate);
            toBeDisposed2.Add(rotateUserEvent);
        }

        /// <summary>
        /// This is main music. It is played during the game and when played tetrominos have not reached the certain treshold of the game matrix.
        /// </summary>
        public void MainMusicSlow()
        {
                backgroundMusicPlayerFast.Stop();
                backgroundMusicPlayerFast.Dispose();
                backgroundMusicPlayerSlow.Init(slowBackgroundMusic[currentMainMusicIndex]);
                backgroundMusicPlayerSlow.Play();
        }

        /// <summary>
        /// This is main music. It is played during the game and when played tetrominos have reached the certain treshold of the game matrix.
        /// </summary>
        public void MainMusicFast()
        {
                backgroundMusicPlayerSlow.Stop();
                backgroundMusicPlayerSlow.Dispose();
                backgroundMusicPlayerFast.Init(fastBackgroundMusic[currentMainMusicIndex]);
                backgroundMusicPlayerFast.Play();
        }

        // Not implemented yet
        public void GetPositionOfMainMusic()
        {
            // 63556920
            // 63551992
            if (musicIsAllowed)
            {
                if (backgroundMusicPlayerSlow.GetPosition() > (long)10000000)
                {
                    MainMusicSlow();
                }
                Console.WriteLine(backgroundMusicPlayerSlow.GetPosition());
            }
        }

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
            AudioFileReader addressGameOver = new AudioFileReader(soundGameOver);
            WaveOutEvent gameOver = new WaveOutEvent();
            gameOver.Init(addressGameOver);
            gameOver.Play();

            toBeDisposed1.Add(addressGameOver);
            toBeDisposed2.Add(gameOver);
        }

        /// <summary>
        /// Free up memory resources.
        /// </summary>
        public void DisposeSFX_NAudio()
        {
            //rotateUserEvent.Dispose();
            //moveToSidesUserEvent.Dispose();
            //gameOver.Dispose();
            obstacle.Dispose();
            lineCleared.Dispose();
            tetris.Dispose();
            nextLevel.Dispose();
            settings.Dispose();
        }

        /// <summary>
        /// Free up memory resources.
        /// </summary>
        public void DisposeBackgroundMusic_NAudio()
        {
            //Todo: When the same music is called again, it plays where it left off. It should start from the beginning.
            backgroundMusicPlayerSlow.Stop();
            backgroundMusicPlayerSlow.Dispose();
            backgroundMusicPlayerFast.Stop();
            backgroundMusicPlayerFast.Dispose();
            backgroundMusicPlayerTetrisMaster.Stop();
            backgroundMusicPlayerTetrisMaster.Dispose();

            backgroundMusicReader1.Dispose();
            backgroundMusicReader2.Dispose();
            backgroundMusicReader3.Dispose();
            backgroundMusicReader10.Dispose();
            backgroundMusicReader20.Dispose();
            backgroundMusicReader30.Dispose();
            backgroundTetrisMaster.Dispose();
            ChoseMainMusic();
        }
    }
}

// Todo: Music from naudio playes where it left off. It should start from the beginning. Moreover short sounds are played only once. After another call they are not played.
