using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Music
    {
        static readonly string path = @"../../Music/";

        static readonly string soundRotate = path + @"SFX 6.mp3";
        static readonly string soundMoveToSides = path + @"SFX 4.mp3";
        static readonly string soundGameOver = path + @"SFX 14.mp3";

        static readonly string soundObstacle = path + @"SFX 8.wav";
        static readonly string soundLineCleared = path + @"SFX 11.wav";
        static readonly string soundTetris = path + @"SFX TetrisClear.wav";
        static readonly string soundNextLevel = path + @"SFX 7.wav";

        static readonly string backgroudMusic1_Slow = path + @"1 - Music 1.mp3";
        static readonly string backgroudMusic2_Slow = path + @"2 - Music 2.mp3";
        static readonly string backgroudMusic3_Slow = path + @"3 - Music 3.mp3";

        static readonly string backgroudMusic1_Fast = path + @"8 - Music 8.mp3";
        static readonly string backgroudMusic2_Fast = path + @"9 - Music 9.mp3";
        static readonly string backgroudMusic3_Fast = path + @"10 - Music 10.mp3";

        WaveOut rotateUserEvent = new WaveOut();
        WaveOut moveToSidesUserEvent = new WaveOut();
        WaveOutEvent backgroundMusicPlayer = new WaveOutEvent();
        WaveOut gameOver = new WaveOut();

        SoundPlayer obstacle = new SoundPlayer(soundObstacle);
        SoundPlayer lineCleared = new SoundPlayer(soundLineCleared);
        SoundPlayer tetris = new SoundPlayer(soundTetris);
        SoundPlayer nextLevel = new SoundPlayer(soundNextLevel);


        //WaveOut obstacle = new WaveOut();
        //WaveOut lineCleared = new WaveOut();
        //WaveOut tetris = new WaveOut();

        /// <summary>
        /// Constructor
        /// </summary>
        public Music() { }

        public void MoveToSides()
        {
            AudioFileReader address = new AudioFileReader(soundMoveToSides);
            moveToSidesUserEvent.Init(address);
            moveToSidesUserEvent.Play();
        }

        public void Rotate()
        {
            AudioFileReader address = new AudioFileReader(soundRotate);
            rotateUserEvent.Init(address);
            rotateUserEvent.Play();
        }

        public void MainMusic()
        {
            // Initialize the background music player
            AudioFileReader backgroundMusicReader = new AudioFileReader(backgroudMusic1_Slow);
            // Start playing the background music (looped)
            backgroundMusicPlayer.Init(backgroundMusicReader);
            backgroundMusicPlayer.Play();
        }

        public void NextLevel()
        {
            nextLevel.Play();
        }

        public void Obstacle()
        {
            //AudioFileReader address = new AudioFileReader(soundObstacle);
            //obstacle.Init(address);
            obstacle.Play();
        }

        public void LineCleared()
        {
            //AudioFileReader address = new AudioFileReader(soundLineCleared);
            //lineCleared.Init(address);
            lineCleared.Play();
        }
        public void GameOver()
        {
            AudioFileReader address = new AudioFileReader(soundGameOver);
            gameOver.Init(address);
            gameOver.Play();
        }

        public void Tetris()
        {
        //    AudioFileReader address = new AudioFileReader(soundTetris);
        //    tetris.Init(address);
            tetris.Play();
        }

        /// <summary>
        /// Free up memory resources.
        /// </summary>
        public void DisposeSFX_NAudio()
        {
            rotateUserEvent.Dispose();
            moveToSidesUserEvent.Dispose();
            //obstacle.DisposeSFX_NAudio();
            //lineCleared.DisposeSFX_NAudio();
            gameOver.Dispose();
            //tetris.DisposeSFX_NAudio();
        }

        /// <summary>
        /// Free up memory resources.
        /// </summary>
        public void DisposeBackgroundMusic_NAudio()
        {
            backgroundMusicPlayer.Dispose();
        }
    }
}
