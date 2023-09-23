using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
// DONE (Repeating background music not implemented)
namespace Tetris
{
    public class Music
    {
        // ------------------------------------------------------------------------------------------------
        // CONSTANTS

        private const string PATH = @"../../Music/";
        private const string SOUND_ROTATE = PATH + @"SFX 6.mp3";
        private const string SOUND_MOVE_TO_SIDES = PATH + @"SFX 4.mp3";
        private const string SOUND_GAME_OVER = PATH + @"SFX 14.mp3";
        private const string SOUND_OBSTACLE = PATH + @"SFX 8.wav";
        private const string SOUND_LINE_CLEARED = PATH + @"SFX 11.wav";
        private const string SOUND_TETRIS = PATH + @"SFX TetrisClear.wav";
        private const string SOUND_NEXTLEVEL = PATH + @"SFX 7.wav";
        private const string SOUND_SETTINGS = PATH + @"SFX 2.wav";
        private const string MUSIC_BACKGROUND1_SLOW = PATH + @"1 - Music 1.mp3";
        private const string MUSIC_BACKGROUND2_SLOW = PATH + @"2 - Music 2.mp3";
        private const string MUSIC_BACKGROUND3_SLOW = PATH + @"3 - Music 3.mp3";
        private const string MUSIC_BACKGROUND1_FAST = PATH + @"8 - Track 8.mp3";
        private const string MUSIC_BACKGROUND2_FAST = PATH + @"9 - Track 9.mp3";
        private const string MUSIC_BACKGROUND3_FAST = PATH + @"10 - Track 10.mp3";
        private const string MUSIC_BACKGROUND_TETRISMASTER = PATH + @"6 - Tetris Master.mp3";

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

        // Lists which will contain objects that should be disposed.
        private List<AudioFileReader> _toBeDisposedMovementAF = new List<AudioFileReader>();
        private List<WaveOutEvent> _toBeDisposedMovementWO = new List<WaveOutEvent>();
        private List<AudioFileReader> _toBeDisposedMainMusicAF = new List<AudioFileReader>();
        private List<WaveOutEvent> _toBeDisposedMainMusicWO = new List<WaveOutEvent>();
        private List<AudioFileReader> _toBeDisposedTetrisMasterAF = new List<AudioFileReader>();
        private List<WaveOutEvent> _toBeDisposedTetrisMasterWO = new List<WaveOutEvent>();


        // ------------------------------------------------------------------------------------------------
        // ENUM
        /// <summary>
        /// Groups of music based on their type.
        /// </summary>
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
        // In this class there have to be used 2 different libraries for playing music. One is NAudio and
        // the other is System.Media. System.Media is unable to play mp3 files and two sounds at the same
        // time, so I had to use NAudio. However, NAudio has to be managed by the programmer, which means
        // that the programmer has to dispose objects that are no longer in use. System.Media does that
        // automatically.

        // ------------------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Controls if music is allowed or not.
        /// </summary>
        public bool MusicIsAllowed { get; set; } = true;


        // ------------------------------------------------------------------------------------------------
        // METHODS

        // For some reason, the Classes AduioFileReader and WaveOutEvent have to be instantiated again and
        // again with each call of below methods. Without that the music/sounds would just stop playing
        // after one call of each method. I don't know why is that happening...

        /// <summary>
        /// Play this music when the game is over and the player has high score.
        /// </summary>
        public void TetrisMaster()
        {
            var backgroundTetrisMaster = new AudioFileReader(MUSIC_BACKGROUND_TETRISMASTER);
            var backgroundMusicPlayerTetrisMaster = new WaveOutEvent();
            backgroundMusicPlayerTetrisMaster.Init(backgroundTetrisMaster);
            backgroundMusicPlayerTetrisMaster.Play();
            _toBeDisposedTetrisMasterAF.Add(backgroundTetrisMaster);
            _toBeDisposedTetrisMasterWO.Add(backgroundMusicPlayerTetrisMaster);
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
            _toBeDisposedMovementAF.Add(addressToSides);
            _toBeDisposedMovementWO.Add(moveToSidesUserEvent);

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
            _toBeDisposedMovementAF.Add(addressRotate);
            _toBeDisposedMovementWO.Add(rotateUserEvent);
        }
        
        /// <summary>
        /// Play Main Background music.
        /// </summary>
        /// <param name="playSlowMusic">If yes play slow music else play fast music.</param>
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
            _toBeDisposedMainMusicAF.Add(_backgroundMusicReader);
            _toBeDisposedMainMusicWO.Add(backgroundMusicPlayer);
        }

        /// <summary>
        /// Play this sound when the game is over.
        /// </summary>
        public void GameOver()
        {
            var addressGameOver = new AudioFileReader(SOUND_GAME_OVER);
            var gameOver = new WaveOutEvent();
            gameOver.Init(addressGameOver);
            gameOver.Play();
            _toBeDisposedMovementAF.Add(addressGameOver);
            _toBeDisposedMovementWO.Add(gameOver);
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

        public void GetPositionOfMainMusic()
        {
            throw new NotImplementedException();

            // 1st music length 63556920
            // 2sn music length 63551992
            // 3rd music length

            if (!MusicIsAllowed)
                return;
        }

        /// <summary>
        /// Chose randomly the main music. There are 3 main songs.
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
                    DisposerBasic(_toBeDisposedMainMusicAF, _toBeDisposedMainMusicWO);
                    break;
                case Type.TetrisMaster:
                    DisposerBasic(_toBeDisposedTetrisMasterAF, _toBeDisposedTetrisMasterWO);
                    break;
            }
        }

        /// <summary>
        /// Dispose only older objects. In the past I disposed objects that were still in use and that
        /// crashed the program.
        /// </summary>
        private void DisposerAdvanced()
        {
            if (_toBeDisposedMovementAF.Count < NUMBER_OF_OBJECTS_NOT_TO_DISPOSE)
                return ;

            for (byte i = 0; i < _toBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE; i++)
            {
                _toBeDisposedMovementAF[i].Dispose();
                _toBeDisposedMovementWO[i].Dispose();
            }

            var temp1 = new List<AudioFileReader>();
            var temp2 = new List<WaveOutEvent>();

            for (byte i = 0; i < NUMBER_OF_OBJECTS_NOT_TO_DISPOSE; i++)
            {
                temp1.Add(_toBeDisposedMovementAF[_toBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE + i]);
                temp2.Add(_toBeDisposedMovementWO[_toBeDisposedMovementAF.Count - NUMBER_OF_OBJECTS_NOT_TO_DISPOSE + i]);
            }

            _toBeDisposedMovementAF.Clear();
            _toBeDisposedMovementWO.Clear();

            _toBeDisposedMovementAF = temp1.ToList();
            _toBeDisposedMovementWO = temp2.ToList();
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

            foreach (var waveOutEvent in container2)
            {
                waveOutEvent.Dispose();
            }
            container1.Clear();
            container2.Clear();

        }
    }
}

