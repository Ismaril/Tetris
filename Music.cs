using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Music
    {
        bool musicIsAllowed = true;

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

        WaveOut rotateUserEvent = new WaveOut();
        WaveOut moveToSidesUserEvent = new WaveOut();
        WaveOutEvent backgroundMusicPlayerSlow = new WaveOutEvent();
        WaveOutEvent backgroundMusicPlayerFast = new WaveOutEvent();
        WaveOut gameOver = new WaveOut();

        SoundPlayer obstacle = new SoundPlayer(soundObstacle);
        SoundPlayer lineCleared = new SoundPlayer(soundLineCleared);
        SoundPlayer tetris = new SoundPlayer(soundTetris);
        SoundPlayer nextLevel = new SoundPlayer(soundNextLevel);
        SoundPlayer settings = new SoundPlayer(soundSettings);

        

        // Initialize the background music player
        static readonly AudioFileReader backgroundMusicReader1 = new AudioFileReader(backgroundMusic1_Slow);
        static readonly AudioFileReader backgroundMusicReader2 = new AudioFileReader(backgroundMusic2_Slow);
        static readonly AudioFileReader backgroundMusicReader3 = new AudioFileReader(backgroundMusic3_Slow);
        static readonly AudioFileReader backgroundMusicReader10 = new AudioFileReader(backgroundMusic1_Fast);
        static readonly AudioFileReader backgroundMusicReader20 = new AudioFileReader(backgroundMusic2_Fast);
        static readonly AudioFileReader backgroundMusicReader30 = new AudioFileReader(backgroundMusic3_Fast);

        List<AudioFileReader> slowBackgroundMusic = new List<AudioFileReader>() { backgroundMusicReader1, backgroundMusicReader2, backgroundMusicReader3 };
        List<AudioFileReader> fastBackgroundMusic = new List<AudioFileReader>() { backgroundMusicReader10, backgroundMusicReader20, backgroundMusicReader30 };

        Random random = new Random();

        byte currentMainMusicIndex = 0;

        bool fastMusicPlaying = false;
        bool slowMusicPlaying = false;

        public bool MusicIsAllowed { get => musicIsAllowed; set => musicIsAllowed = value; }

        //WaveOut obstacle = new WaveOut();
        //WaveOut lineCleared = new WaveOut();
        //WaveOut tetris = new WaveOut();

        /// <summary>
        /// Constructor
        /// </summary>
        public Music() { }

        public void MoveToSides()
        {
            AudioFileReader addressToSides = new AudioFileReader(soundMoveToSides);

            moveToSidesUserEvent.Init(addressToSides);
            moveToSidesUserEvent.Play();
        }

        public void Rotate()
        {
            AudioFileReader addressRotate = new AudioFileReader(soundRotate);

            rotateUserEvent.Init(addressRotate);
            rotateUserEvent.Play();
        }

        public void MainMusicSlow()
        {
                backgroundMusicPlayerFast.Stop();
                backgroundMusicPlayerFast.Dispose();
                //fastMusicPlaying = false;
                //slowMusicPlaying = true;
                backgroundMusicPlayerSlow.Init(slowBackgroundMusic[currentMainMusicIndex]);
                backgroundMusicPlayerSlow.Play();

        }

        public void MainMusicFast()
        {
 
                backgroundMusicPlayerSlow.Stop();
                backgroundMusicPlayerSlow.Dispose();
                //slowMusicPlaying = false;
                //fastMusicPlaying = true;
                backgroundMusicPlayerFast.Init(fastBackgroundMusic[currentMainMusicIndex]);
                backgroundMusicPlayerFast.Play();

        }
        public void InitialiseBackgroundMusic()
        {
            backgroundMusicPlayerSlow.Init(slowBackgroundMusic[currentMainMusicIndex]);

            backgroundMusicPlayerFast.Init(fastBackgroundMusic[currentMainMusicIndex]);

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

        public void ChoseMainMusic()
        {
            currentMainMusicIndex = (byte)random.Next(0, 3);

        }

        public void NextLevel() => nextLevel.Play();

        public void Obstacle() => obstacle.Play(); 

        public void LineCleared() => lineCleared.Play();

        public void Tetris() => tetris.Play();

        public void SoundSettings() => settings.Play();

        public void GameOver()
        {
            AudioFileReader addressGameOver = new AudioFileReader(soundGameOver);

            gameOver.Init(addressGameOver);
            gameOver.Play();
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
        public void DisposeBackgroundMusic_NAudio() { backgroundMusicPlayerSlow.Dispose(); backgroundMusicPlayerFast.Dispose(); }
        
    }
}
