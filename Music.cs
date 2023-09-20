using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;

namespace Tetris
{
    public class Music
    {
        // ------------------------------------------------------------------------------------------------
        // CONSTANTS

        const string MUSIC_PATH = @"../../Music/";
        const string SOUND_ROTATE = MUSIC_PATH + @"SFX 6.mp3";
        const string SOUND_MOVE_TO_SIDES = MUSIC_PATH + @"SFX 4.mp3";
        const string SOUND_GAME_OVER = MUSIC_PATH + @"SFX 14.mp3";
        const string SOUND_OBSTACLE = MUSIC_PATH + @"SFX 8.wav";
        const string SOUND_LINE_CLEARED = MUSIC_PATH + @"SFX 11.wav";
        const string SOUND_TETRIS = MUSIC_PATH + @"SFX TetrisClear.wav";
        const string SOUND_NEXTLEVEL = MUSIC_PATH + @"SFX 7.wav";
        const string SOUND_SETTINGS = MUSIC_PATH + @"SFX 2.wav";
        const string MUSIC_BACKGROUND1_SLOW = MUSIC_PATH + @"1 - Music 1.mp3";
        const string MUSIC_BACKGROUND2_SLOW = MUSIC_PATH + @"2 - Music 2.mp3";
        const string MUSIC_BACKGROUND3_SLOW = MUSIC_PATH + @"3 - Music 3.mp3";
        const string MUSIC_BACKGROUND1_FAST = MUSIC_PATH + @"8 - Track 8.mp3";
        const string MUSIC_BACKGROUND2_FAST = MUSIC_PATH + @"9 - Track 9.mp3";
        const string MUSIC_BACKGROUND3_FAST = MUSIC_PATH + @"10 - Track 10.mp3";
        const string MUSIC_BACKGROUND_TETRISMASTER = MUSIC_PATH + @"6 - High Score (Tetris Master).mp3";


        // ------------------------------------------------------------------------------------------------
        // FIELDS

        byte _currentMainMusicIndex;
        readonly Random _random = new Random();
        AudioFileReader _backgroundMusicReader;
        readonly SoundPlayer _obstacle = new SoundPlayer(soundLocation: SOUND_OBSTACLE);
        readonly SoundPlayer _lineCleared = new SoundPlayer(soundLocation: SOUND_LINE_CLEARED);
        readonly SoundPlayer _tetris = new SoundPlayer(soundLocation: SOUND_TETRIS);
        readonly SoundPlayer _nextLevel = new SoundPlayer(soundLocation: SOUND_NEXTLEVEL);
        readonly SoundPlayer _settings = new SoundPlayer(soundLocation: SOUND_SETTINGS);

        
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
        public bool MusicIsAllowed { get; set; } = true;

        // Lists for disposing movement sounds
        public List<AudioFileReader> ToBeDisposed1 { get; set; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposed2 { get; set; } = new List<WaveOutEvent>();

        // Lists for disposing main music
        public List<AudioFileReader> ToBeDisposed3 { get; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposed4 { get; } = new List<WaveOutEvent>();

        // Lists for disposing TetrisMaster music
        public List<AudioFileReader> ToBeDisposed5 { get; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposed6 { get; } = new List<WaveOutEvent>();


        // ------------------------------------------------------------------------------------------------
        // METHODS

        /// <summary>
        /// Play this music when the game is over and the player has high score.
        /// </summary>
        public void TetrisMaster()
        {
            var backgroundTetrisMaster = new AudioFileReader(MUSIC_BACKGROUND_TETRISMASTER);
            var backgroundMusicPlayerTetrisMaster = new WaveOutEvent();
            backgroundMusicPlayerTetrisMaster.Init(backgroundTetrisMaster);
            backgroundMusicPlayerTetrisMaster.Play();
            ToBeDisposed5.Add(backgroundTetrisMaster);
            ToBeDisposed6.Add(backgroundMusicPlayerTetrisMaster);
        }

        /// <summary>
        /// Play this sound when tetromino is moved to the sides.
        /// </summary>
        public void MoveToSides()
        {
            var addressToSides = new AudioFileReader(SOUND_MOVE_TO_SIDES);
            var moveToSidesUserEvent = new WaveOutEvent();
            moveToSidesUserEvent.Init(addressToSides);
            moveToSidesUserEvent.Play();
            ToBeDisposed1.Add(addressToSides);
            ToBeDisposed2.Add(moveToSidesUserEvent);

        }

        /// <summary>
        /// Play this sound when tetromino is rotated.
        /// </summary>
        public void Rotate()
        {
            var addressRotate = new AudioFileReader(SOUND_ROTATE);
            var rotateUserEvent = new WaveOutEvent();
            rotateUserEvent.Init(addressRotate);
            rotateUserEvent.Play();
            ToBeDisposed1.Add(addressRotate);
            ToBeDisposed2.Add(rotateUserEvent);
        }
        

        public void MainMusic(bool playSlowMusic = true)
        {
            DisposeMusic(case_: 3);
            
            if (playSlowMusic)
                SlowMusic();
            else
                FastMusic();

            var backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(_backgroundMusicReader);
            backgroundMusicPlayer.Play();

            ToBeDisposed3.Add(_backgroundMusicReader);
            ToBeDisposed4.Add(backgroundMusicPlayer);
        }

        public void SlowMusic()
        {
            switch (_currentMainMusicIndex)
            {
                case 0:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND1_SLOW);
                    break;
                case 1:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND2_SLOW);
                    break;
                case 2:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND3_SLOW);
                    break;
            }
        }

        public void FastMusic()
        {
            switch (_currentMainMusicIndex)
            {
                case 0:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND1_FAST);
                    break;
                case 1:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND2_FAST);
                    break;
                case 2:
                    _backgroundMusicReader = new AudioFileReader(MUSIC_BACKGROUND3_FAST);
                    break;
            }
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
        public void ChoseMainMusic() => _currentMainMusicIndex = (byte)_random.Next(0, 3);

        /// <summary>
        /// Pley this sound when the player reaches the next level.
        /// </summary>
        public void NextLevel() => _nextLevel.Play();

        /// <summary>
        /// Play this sound when tetromino arrived at obstacle.
        /// </summary>
        public void Obstacle() => _obstacle.Play(); 

        /// <summary>
        /// Play this sound when a line is cleared.
        /// </summary>
        public void LineCleared() => _lineCleared.Play();

        /// <summary>
        /// Play this sound when player clears 4 lines at once.
        /// </summary>
        public void Tetris() => _tetris.Play();

        /// <summary>
        /// Play this sound when player moves around settings.
        /// </summary>
        public void SoundSettings() => _settings.Play();

        /// <summary>
        /// Play this sound when the game is over.
        /// </summary>
        public void GameOver()
        {
            var addressGameOver = new AudioFileReader(SOUND_GAME_OVER);
            var gameOver = new WaveOutEvent();
            gameOver.Init(addressGameOver);
            gameOver.Play();

            ToBeDisposed1.Add(addressGameOver);
            ToBeDisposed2.Add(gameOver);
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

                    // Todo: still not working
                    // Dispose only the first x elements, excluding last 6. There was a problem that I was trying
                    // to dispose the sounds that were still playing.
                    byte num = 6;
                    if (ToBeDisposed1.Count < num)
                        break;
                    for (byte i = 0; i < ToBeDisposed1.Count - num; i++)
                    {
                        ToBeDisposed1[i].Dispose();
                        ToBeDisposed2[i].Dispose();
                    }

                    var temp1 = new List<AudioFileReader>();
                    var temp2 = new List<WaveOutEvent>();

                    for (byte i = 0; i < num; i++)
                    {
                        temp1.Add(ToBeDisposed1[ToBeDisposed1.Count - num + i]);
                        temp2.Add(ToBeDisposed2[ToBeDisposed1.Count - num + i]);
                    }

                    ToBeDisposed1.Clear();
                    ToBeDisposed2.Clear();

                    ToBeDisposed1 = temp1.ToList();
                    ToBeDisposed2= temp2.ToList();

                    break;
                case 3:
                    // todo: here also happend RaceOnRCWCleanup
                    foreach (var audioFileReader in ToBeDisposed3)
                    {
                        audioFileReader.Dispose();
                    }

                    foreach (var audioFileReader in ToBeDisposed4)
                    {
                        audioFileReader.Dispose();
                    }
                    ToBeDisposed3.Clear();
                    ToBeDisposed4.Clear();
                    break;
                case 5:
                    foreach (var audioFileReader in ToBeDisposed5)
                    {
                        audioFileReader.Dispose();
                    }

                    foreach (var audioFileReader in ToBeDisposed6)
                    {
                        audioFileReader.Dispose();
                    }
                    ToBeDisposed5.Clear();
                    ToBeDisposed6.Clear();
                    break;
            }
        }
    }
}

