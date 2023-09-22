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

        private const string MUSIC_PATH = @"../../Music/";
        private const string SOUND_ROTATE = MUSIC_PATH + @"SFX 6.mp3";
        private const string SOUND_MOVE_TO_SIDES = MUSIC_PATH + @"SFX 4.mp3";
        private const string SOUND_GAME_OVER = MUSIC_PATH + @"SFX 14.mp3";
        private const string SOUND_OBSTACLE = MUSIC_PATH + @"SFX 8.wav";
        private const string SOUND_LINE_CLEARED = MUSIC_PATH + @"SFX 11.wav";
        private const string SOUND_TETRIS = MUSIC_PATH + @"SFX TetrisClear.wav";
        private const string SOUND_NEXTLEVEL = MUSIC_PATH + @"SFX 7.wav";
        private const string SOUND_SETTINGS = MUSIC_PATH + @"SFX 2.wav";
        private const string MUSIC_BACKGROUND1_SLOW = MUSIC_PATH + @"1 - Music 1.mp3";
        private const string MUSIC_BACKGROUND2_SLOW = MUSIC_PATH + @"2 - Music 2.mp3";
        private const string MUSIC_BACKGROUND3_SLOW = MUSIC_PATH + @"3 - Music 3.mp3";
        private const string MUSIC_BACKGROUND1_FAST = MUSIC_PATH + @"8 - Track 8.mp3";
        private const string MUSIC_BACKGROUND2_FAST = MUSIC_PATH + @"9 - Track 9.mp3";
        private const string MUSIC_BACKGROUND3_FAST = MUSIC_PATH + @"10 - Track 10.mp3";
        private const string MUSIC_BACKGROUND_TETRISMASTER = MUSIC_PATH + @"6 - High Score (Tetris Master).mp3";

        private const byte NUMBER_OF_OBJECTS_NOT_TO_DISPOSE = 6;

        // ------------------------------------------------------------------------------------------------
        // FIELDS

        private byte _currentMainMusicIndex;
        private AudioFileReader _backgroundMusicReader;
        private readonly Random _random = new Random();
        private readonly SoundPlayer _obstacle = new SoundPlayer(soundLocation: SOUND_OBSTACLE);
        private readonly SoundPlayer _lineCleared = new SoundPlayer(soundLocation: SOUND_LINE_CLEARED);
        private readonly SoundPlayer _tetris = new SoundPlayer(soundLocation: SOUND_TETRIS);
        private readonly SoundPlayer _nextLevel = new SoundPlayer(soundLocation: SOUND_NEXTLEVEL);
        private readonly SoundPlayer _settings = new SoundPlayer(soundLocation: SOUND_SETTINGS);
        public enum Type
        {
            Movement,
            MainMusic,
            TetrisMaster
        }

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

        // Lists which contain objects that should be disposed
        public List<AudioFileReader> ToBeDisposedMovementAF { get; set; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposedMovementWO { get; set; } = new List<WaveOutEvent>();
        public List<AudioFileReader> ToBeDisposedMainMusicAF { get; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposedMainMusicWO { get; } = new List<WaveOutEvent>();
        public List<AudioFileReader> ToBeDisposedTetrisMasterAF { get; } = new List<AudioFileReader>();
        public List<WaveOutEvent> ToBeDisposedTetrisMasterWO { get; } = new List<WaveOutEvent>();


        // ------------------------------------------------------------------------------------------------
        // METHODS

        // For some reason, the Classes AduioFileReader and WaveOutEvent have to be instantiated again and
        // again with each call of below methods. Without that the music/sounds would just stop playing after one
        // call of each method. I don't know why is that happening...

        /// <summary>
        /// Play this music when the game is over and the player has high score.
        /// </summary>
        public void TetrisMaster()
        {
            var backgroundTetrisMaster = new AudioFileReader(MUSIC_BACKGROUND_TETRISMASTER);
            var backgroundMusicPlayerTetrisMaster = new WaveOutEvent();
            backgroundMusicPlayerTetrisMaster.Init(backgroundTetrisMaster);
            backgroundMusicPlayerTetrisMaster.Play();
            ToBeDisposedTetrisMasterAF.Add(backgroundTetrisMaster);
            ToBeDisposedTetrisMasterWO.Add(backgroundMusicPlayerTetrisMaster);
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
            ToBeDisposedMovementAF.Add(addressToSides);
            ToBeDisposedMovementWO.Add(moveToSidesUserEvent);

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
            ToBeDisposedMovementAF.Add(addressRotate);
            ToBeDisposedMovementWO.Add(rotateUserEvent);
        }
        
        /// <summary>
        /// Play Main Background music.
        /// </summary>
        /// <param name="playSlowMusic"></param>
        public void MainMusic(bool playSlowMusic = true)
        {
            DisposeMusic(music: Type.MainMusic);
            
            if (playSlowMusic)
                SlowMusic();
            else
                FastMusic();

            var backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(_backgroundMusicReader);
            backgroundMusicPlayer.Play();

            ToBeDisposedMainMusicAF.Add(_backgroundMusicReader);
            ToBeDisposedMainMusicWO.Add(backgroundMusicPlayer);
        }

        /// <summary>
        /// Helper method for Main music.
        /// </summary>
        private void SlowMusic()
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

        /// <summary>
        /// Helper method for Main music.
        /// </summary>
        private void FastMusic()
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

            ToBeDisposedMovementAF.Add(addressGameOver);
            ToBeDisposedMovementWO.Add(gameOver);
        }

        /// <summary>
        /// Dispose music.
        /// </summary>
        /// <param name="music">Based on case, select which objects should be disposed</param>
        public void DisposeMusic(Type music)
        {
            switch (music)
            {
                case Type.Movement:
                    DisposerAdvanced();
                    break;
                case Type.MainMusic:
                    DisposerBasic(ToBeDisposedMainMusicAF, ToBeDisposedMainMusicWO);
                    break;
                case Type.TetrisMaster:
                    DisposerBasic(ToBeDisposedTetrisMasterAF, ToBeDisposedTetrisMasterWO);
                    break;
            }
        }

        /// <summary>
        /// Dispose only older objects. In the past I disposed objects that were still in use.
        /// </summary>
        private void DisposerAdvanced()
        {
            if (ToBeDisposedMovementAF.Count < NUMBER_OF_OBJECTS_NOT_TO_DISPOSE)
                return ;

            for (byte i = 0; i < ToBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE; i++)
            {
                ToBeDisposedMovementAF[i].Dispose();
                ToBeDisposedMovementWO[i].Dispose();
            }

            var temp1 = new List<AudioFileReader>();
            var temp2 = new List<WaveOutEvent>();

            for (byte i = 0; i < NUMBER_OF_OBJECTS_NOT_TO_DISPOSE; i++)
            {
                temp1.Add(ToBeDisposedMovementAF[ToBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE + i]);
                temp2.Add(ToBeDisposedMovementWO[ToBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE + i]);
            }

            ToBeDisposedMovementAF.Clear();
            ToBeDisposedMovementWO.Clear();

            ToBeDisposedMovementAF = temp1.ToList();
            ToBeDisposedMovementWO = temp2.ToList();
        }


        /// <summary>
        /// Helper method for DisposeMusic.
        /// </summary>
        /// <param name="container1"></param>
        /// <param name="container2"></param>
        private void DisposerBasic(List<AudioFileReader> container1, List<WaveOutEvent> container2)
        {
            foreach (var audioFileReader in container1)
            {
                audioFileReader.Dispose();
            }

            foreach (var audioFileReader in container2)
            {
                audioFileReader.Dispose();
            }
            container1.Clear();
            container2.Clear();

        }
    }
}

