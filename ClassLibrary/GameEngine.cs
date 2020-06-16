using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.DataLayer;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary {
    public partial class GameEngine {
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        private readonly Action _reDraw;
        public readonly DataLayer.DataLayer DataLayer = new DataLayer.DataLayer();
        public readonly GameLogic GameLogic;
        public float GuiScale = 2f;
        public GameEngine(Action reDraw) {
            _reDraw = reDraw;
            GameLogic = new GameLogic(ChangeGameStatus, () => DataLayer, RefreshSaves, _musicPlayer.PlaySound);
        }
        public GameStatusEnum GameStatus { get; private set; }
        public List<Save> Saves { get; private set; }
        
        
        
        //TODO: settings validation
        public Save NewGameSave { get; private set; } = new Save();
        public LevelRedactor LevelRedactor = new LevelRedactor();
        public List<CustomLevel> CustomLevels { get; private set; }


        
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
                Thread.Sleep(1000 / DataLayer.Settings.Fps);
                _reDraw();
            }
        }
        private void MenuGraphicsThread() {
            while (GameStatus == GameStatusEnum.Menu) {
                Thread.Sleep(1000 / (DataLayer.Settings.Fps * 2));
                _reDraw();
            }
        }

        private void ResultsGraphicsThread() {
            while (GameStatus == GameStatusEnum.WinScreen || GameStatus == GameStatusEnum.LoseScreen) {
                Thread.Sleep(1000 / DataLayer.Settings.Fps);
                _reDraw();
            }
        }
        
        private void RedactorGraphicsThread() {
            while (GameStatus == GameStatusEnum.Redactor) {
                Thread.Sleep(1000 / DataLayer.Settings.Fps);
                _reDraw();
            }
        }
        

        private void GameLogicThread() {
            while (GameStatus == GameStatusEnum.Game) {
                Thread.Sleep(10000 / DataLayer.Settings.TickRate);
                GameLogic.GameLoop();
            }
        }
        private void RefreshSaves() {
            Saves = DataLayer.GetAllGameSaves();
        }

        private void RefreshCustomLevels() {
            CustomLevels = DataLayer.GetAllCustomLevels();
        }
        
        public void Start() {
            RefreshSaves();
            RefreshCustomLevels();
            MenuGameCycle();

            void MenuGameCycle() {
                try {
                    switch (GameStatus) {
                        case GameStatusEnum.Menu:
                            _musicPlayer.PlayTheme(SoundFilesEnum.MenuTheme);
                            Parallel.Invoke(MenuGraphicsThread);
                            break;
                        case GameStatusEnum.Game:
                            _musicPlayer.PlayTheme(SoundFilesEnum.GameTheme);
                            Parallel.Invoke(GraphicsThread,
                                GameLogicThread);
                            break;
                        case GameStatusEnum.WinScreen:
                        case GameStatusEnum.LoseScreen:
                            _musicPlayer.PlayTheme(SoundFilesEnum.ResultsTheme);
                            Parallel.Invoke(ResultsGraphicsThread);
                            break;
                        case GameStatusEnum.Redactor:
                            _musicPlayer.PlayTheme(SoundFilesEnum.ResultsTheme);
                            Parallel.Invoke(RedactorGraphicsThread);

                            break;
                        default:
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
            GameLogic.CreateLevel(save.LevelName, save.Name, DataLayer.Settings.SizeX,
                DataLayer.Settings.SizeY, DataLayer.Settings.Difficulty, PlaySound);
            GameLogic.Player.Score = save.Score;
            GameLogic.Player.Hero = save.Hero;
            GameLogic.CurrentSave = save;
            GameStatus = GameStatusEnum.Game;
        }
    }
}