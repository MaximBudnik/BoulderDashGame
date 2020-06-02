using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ClassLibrary.SoundPlayer {
    public class MusicPlayer {
        private readonly string _gameTheme = Path.Combine(Environment.CurrentDirectory, @"Sounds\bullet.mp3");
        private readonly string _menuTheme = Path.Combine(Environment.CurrentDirectory, @"Sounds\trigger.mp3");
        private readonly string _resultTheme = Path.Combine(Environment.CurrentDirectory, @"Sounds\jw.mp3");
        private readonly float _startVolume = 0.15f;
        private float _currentVolume;
        private WaveOut _effectsOutput;
        private AudioFileReader _effectsReader;
        private Mp3FileReader _fileReader;
        private WaveOut _musicOutput;

        public MusicPlayer() {
            FadeOutEndNotification += OnFadeOutEndNotification;
        }

        private event FadeOutEnd FadeOutEndNotification;

        private void OnFadeOutEndNotification(SoundFilesEnum name) {
            StopTheme();
            PlayThemePrivate(name);
        }

        public void ChangeVolume(float value) {
            _currentVolume += value;
            if (_currentVolume > 1f) _currentVolume = 1f;
            if (_currentVolume < 0f) _currentVolume = 0f;
            _musicOutput.Volume = _currentVolume;
        }

        private void PlayThemePrivate(SoundFilesEnum theme) {
            try {
                _fileReader = theme switch {
                    SoundFilesEnum.MenuTheme => new Mp3FileReader(_menuTheme),
                    SoundFilesEnum.GameTheme => new Mp3FileReader(_gameTheme),
                    SoundFilesEnum.ResultsTheme => new Mp3FileReader(_resultTheme),
                    _ => throw new Exception("Unknown theme")
                };
                _musicOutput = new WaveOut {Volume = _currentVolume};
                _musicOutput.Init(_fileReader);
                FadeInVolume();
                _musicOutput.Play();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error in sound player");
            }
        }

        private void FadeInVolume() {
            var fadeIn = new Task(() => {
                _currentVolume = 0f;
                while (_currentVolume <= _startVolume) {
                    _currentVolume += 0.01f;
                    _musicOutput.Volume = _currentVolume;
                    Thread.Sleep(250);
                }
            });
            fadeIn.Start();
        }

        public void PlayTheme(SoundFilesEnum name) {
            var fadeOut = new Task(() => {
                if (_musicOutput != null)
                    while (_currentVolume > 0.01f) {
                        _currentVolume -= 0.01f;
                        _musicOutput.Volume = _currentVolume;
                        Thread.Sleep(50);
                    }
                FadeOutEndNotification?.Invoke(name);
            });
            fadeOut.Start();
        }

        public void PlaySound(SoundFilesEnum sound) {
            try {
                _effectsReader = sound switch {
                    SoundFilesEnum.MenuAcceptSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\menuAccept.wav")),
                    SoundFilesEnum.MenuClickSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\menuClick.wav")),
                    SoundFilesEnum.AttackSound => new AudioFileReader(
                        Path.Combine(Environment.CurrentDirectory, @"Sounds\sfx\attack.wav")),
                    SoundFilesEnum.BombSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\bomb.wav")),
                    SoundFilesEnum.ConverterSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\converter.wav")),
                    SoundFilesEnum.HitSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\hit.wav")),
                    SoundFilesEnum.TeleportSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\teleport.wav")),
                    SoundFilesEnum.WalkSound => new AudioFileReader(Path.Combine(Environment.CurrentDirectory,
                        @"Sounds\sfx\walk.wav")),
                    SoundFilesEnum.PickupSound => new AudioFileReader(
                        Path.Combine(Environment.CurrentDirectory, @"Sounds\sfx\pickup.wav")),
                    _ => throw new Exception("Unknown theme")
                };
                _effectsOutput = new WaveOut();
                _effectsOutput.Init(_effectsReader);
                _effectsOutput.Play();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error in sound player");
            }
        }
        private void StopTheme() {
            try {
                _musicOutput?.Stop();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        private delegate void FadeOutEnd(SoundFilesEnum name);
    }
}