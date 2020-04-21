using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using ClassLibrary.ConsoleInterface;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public static class GameEngine {
        private static int fps = 5; //TODO: carry it out in settings. Actually it must be much lover than 30!!!
        private static bool _isGame = false;

        private static int CurrentMenuAction;
        private static int _menuItems = 6;

        //delegates for menu input processor
        public delegate int MipGetOperation();

        public delegate void MipExit();

        public delegate void MipChangeIsGame();

        public delegate void MipChangeCurrentMenuAction(int i);

        public delegate void MipDrawHelp();

        public delegate void MipDrawSettings();

        public delegate void MipCreateNewGame();

        public delegate void MipShowBestScores();

        public delegate void MipDrawContinue();

        public static void ChangeIsGame() {
            _isGame = !_isGame;
        }

        public static void createGame() {
            
        }
        static void ChangeCurrentMenuAction(int i) {
            if (CurrentMenuAction < _menuItems && CurrentMenuAction >= 0) {
                CurrentMenuAction += i;
                if (CurrentMenuAction == _menuItems) {
                    CurrentMenuAction = 0;
                }
                else if (CurrentMenuAction == -1) {
                    CurrentMenuAction = _menuItems - 1;
                }
            }
        }

        //delegates for game input processor

        private static readonly Menu Menu = new Menu();
        public static readonly GameLogic GameLogic = new GameLogic();
        public static DataInterlayer dataInterlayer = new DataInterlayer();

        public static void Start() {
            // SoundPlayer soundPlayer = new SoundPlayer(); //TODO: dont forget to enable music on build!
            // soundPlayer.playMusic();
            Menu.DrawMenu(CurrentMenuAction);
            ConsoleKeyInfo c = new ConsoleKeyInfo();
            Frame();

            void Frame() {
                Menu.DrawMenu(CurrentMenuAction);
                if (!_isGame) {
                    do {
                        c = Console.ReadKey(true); //read key without imputing it

                        //i just give all methods to menuInputProcessor, so it looks on input and performs actions
                        void MipExit() {
                            Environment.Exit(0);
                        }
                        void MipChangeIsGame() {
                            ChangeIsGame();
                        }
                        void MipChangeCurrentMenuAction(int i) {
                            ChangeCurrentMenuAction(i);
                            Menu.DrawMenu(CurrentMenuAction);
                        }

                        void MipDrawHelp() {
                            Menu.DrawHelp();
                        }

                        void MipDrawSettings() {
                            Menu.DrawSettings();
                        }

                        int MipGetOperation() {
                            return CurrentMenuAction;
                        }
                        void MipCreateNewGame() {
                            Menu.DrawNewGame();
                            string name = Console.ReadLine();
                            dataInterlayer.AddGameSave(name);
                            dataInterlayer.GetGameSaves();
                            save currentSave = dataInterlayer.saves[dataInterlayer.saves.Count - 1];
                            GameLogic.CreateLevel(currentSave.levelName);
                            GameLogic.Player.Name = currentSave.name;
                            GameLogic.currentSave = currentSave;
                        }

                        void MipShowBestScores() {
                            SortedDictionary<int, string> results = dataInterlayer.getBestSccores();
                            Menu.DrawScores(results);
                        }

                        void MipDrawContinue() {
                            dataInterlayer.GetGameSaves();
                            List<save> saves = dataInterlayer.saves;
                            Menu.DrawSaves(saves);
                            string id = Console.ReadLine();
                            foreach (var save in saves) {
                                if (save.id == Int32.Parse(id)) {
                                    ResumeGame(save);
                                    MipChangeIsGame();
                                }
                            }
                        }

                        MenuInputProcessor menuInputProcessor = new MenuInputProcessor();
                        menuInputProcessor.processInput(
                            c.Key,
                            MipExit,
                            MipChangeIsGame,
                            MipChangeCurrentMenuAction,
                            MipGetOperation,
                            MipCreateNewGame,
                            MipDrawHelp,
                            MipDrawSettings,
                            MipShowBestScores,
                            MipDrawContinue
                        );
                    } while (!_isGame);
                }
                if (_isGame) {
                    Console.Clear();
                    // gameLogic.updateUpperInterface();
                    // GameLogic.drawLevel();
                    // gameLogic.updatePlayerInterface();
                    do {
                        //do while loop represents all actions and responses on it as in menu as in game

                        while (Console.KeyAvailable == false) {
                            //this actions perform constant times (fps) in second.
                            //As for isGame mode, we perform gameLoop() here (ex. here enemies are moved)
                            Console.CursorVisible = false; //TODO: Important! I constantly hide cursor here.
                            Thread.Sleep(1000 / fps);
                            GameLogic.GameLoop();
                        }

                        c = Console.ReadKey(true);
                        //if it isGame mode (we are actually playing) So here our key pressings are processed, while independent game logic
                        //is calculated in while loop

                        //IMPORTANT: we need to call gameLoop also while we input smth or save input out of the loop
                        GameInputProcessor gameInputProcessor = new GameInputProcessor();
                        gameInputProcessor.processInput(c.Key);
                        GameLogic.GameLoop();
                    } while (_isGame);
                }
                Frame();
            }
        }
        public static void ResumeGame(save save) {
            GameLogic.CreateLevel(save.levelName);
            GameLogic.Player.Name = save.name;
            GameLogic.Player.Score = save.score;
            GameLogic.currentSave = save;
        }
    }
}