using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.DataLayer;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary {
    public partial class GameEngine {
        //menu 
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        private readonly Action _reDraw;
        public readonly DataInterlayer DataInterlayer = new DataInterlayer();
        public readonly GameLogic GameLogic;
        public GameEngine(Action reDraw) {
            _reDraw = reDraw;
            GameLogic = new GameLogic(ChangeGameStatus, () => DataInterlayer, RefreshSaves);
        }
        public GameStatusEnum GameStatus { get; private set; }
        public List<Save> Saves { get; private set; }

        //TODO: settings validation
        public Save NewGameSave { get; private set; } = new Save();

        public void ChangeVolume(float val) {
            _musicPlayer.ChangeVolume(val);
        }

        public void PlaySound(SoundFilesEnum name) {
            _musicPlayer.PlaySound(name);
        }

        public int GetScores() {
            return GameLogic.Player.Score;
        }
        public string GetPlayerName() {
            return GameLogic.Player.Name;
        }
        public Dictionary<string, int[]> GetAllPlayerScores() {
            return GameLogic.Player.AllScores;
        }

        public void ChangeGameStatus(GameStatusEnum status) {
            GameStatus = status;
            _musicPlayer.PlaySound(SoundFilesEnum.MenuAcceptSound);
        }

        private void GraphicsThread() {
            while (GameStatus == GameStatusEnum.Game) {
                Thread.Sleep(1000 / DataInterlayer.Settings.Fps);
                _reDraw();
            }
        }
        private void MenuGraphicsThread() {
            while (GameStatus == GameStatusEnum.Menu) {
                Thread.Sleep(1000 / (DataInterlayer.Settings.Fps * 2));
                _reDraw();
            }
        }

        private void ResultsGraphicsThread() {
            while (GameStatus == GameStatusEnum.WinScreen || GameStatus == GameStatusEnum.LoseScreen) {
                Thread.Sleep(1000 / DataInterlayer.Settings.Fps);
                _reDraw();
            }
        }

        private void GameLogicThread() {
            while (GameStatus == GameStatusEnum.Game) {
                Thread.Sleep(10000 / DataInterlayer.Settings.TickRate);
                GameLogic.GameLoop();
            }
        }
        private void RefreshSaves() {
            Saves = DataInterlayer.GetAllGameSaves();
        }
        public void Start() {
            RefreshSaves();
            MenuGameCycle();

            void MenuGameCycle() {
                try {
                    if (GameStatus == GameStatusEnum.Menu) {
                        _musicPlayer.PlayTheme(SoundFilesEnum.MenuTheme);
                        Parallel.Invoke(MenuGraphicsThread);
                    }
                    else if (GameStatus == GameStatusEnum.Game) {
                        _musicPlayer.PlayTheme(SoundFilesEnum.GameTheme);
                        Parallel.Invoke(GraphicsThread,
                            GameLogicThread);
                    }
                    else if (GameStatus == GameStatusEnum.WinScreen || GameStatus == GameStatusEnum.LoseScreen) {
                        _musicPlayer.PlayTheme(SoundFilesEnum.ResultsTheme);
                        Parallel.Invoke(ResultsGraphicsThread);
                    }
                    else {
                        throw new Exception("Unknown game status");
                    }
                    MenuGameCycle();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        private void LaunchGame(Save save) {
            GameLogic.CreateLevel(save.LevelName, save.Name, DataInterlayer.Settings.SizeX,
                DataInterlayer.Settings.SizeY, DataInterlayer.Settings.Difficulty, PlaySound);
            GameLogic.Player.Score = save.Score;
            GameLogic.Player.Hero = save.Hero;
            GameLogic.CurrentSave = save;
            GameStatus = GameStatusEnum.Game;
        }
    }
}