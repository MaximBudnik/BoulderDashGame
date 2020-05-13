using System;
using System.IO;
using NAudio.Wave;

namespace ClassLibrary {
    public class SoundPlayer {
        private readonly string _menuTheme = Path.Combine(Environment.CurrentDirectory, @"Sounds\trigger.mp3");
        private readonly string _gameTheme = Path.Combine(Environment.CurrentDirectory, @"Sounds\bullet.mp3");
        private float _volume = 0.5f;
        private Mp3FileReader _fileReader;
        private WaveOut _output;
        public void ChangeVolume(float value) {
            _volume += value;
            if (_volume >1f) {
                _volume = 1f;
            }
            if (_volume <0f) {
                _volume = 0f;
            }
            _output.Volume = _volume;
        }

        public void PlayTheme(string name) {
            try {
                switch (name) {
                    case "menu":
                        _fileReader = new Mp3FileReader(_menuTheme);
                        break;
                    case "game":
                        _fileReader = new Mp3FileReader(_gameTheme);
                        break;
                    default:
                        throw new Exception("Unknown theme");
                }
                _output = new WaveOut {Volume = _volume};
                _output.Volume = _volume;
                _output.Init(_fileReader);
                _output.Play();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error in sound player");
            }
        }

        private void PauseTheme() {
            _output?.Pause();
        }
        private void ResumeTheme() {
            _output?.Resume();
        }
        
        public void StopTheme() {
            try {
                _output?.Stop();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args) { }
    }
}