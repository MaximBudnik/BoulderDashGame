using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.ConsoleInterface;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public static class GameEngine {
        private const int
            GameLogicTickRate = 1; //TODO: carry it out in settings. Actually it must be much lover than 30!!!

        private static int _gameStatus; // 0 - menu; 1 - game; 2 - win screen; 3 - lose screen

        private static int _currentMenuAction;
        private static readonly int MenuItems = 6;

        public static void ChangeGameStatus(int i) {
            if (i >= 0 && i < 4)
                _gameStatus = i;
            else
                throw new Exception("Unknown game status");
        }

        private static void ChangeCurrentMenuAction(int i) {
            if (_currentMenuAction < MenuItems && _currentMenuAction >= 0) {
                _currentMenuAction += i;
                if (_currentMenuAction == MenuItems)
                    _currentMenuAction = 0;
                else if (_currentMenuAction == -1) _currentMenuAction = MenuItems - 1;
            }
        }

        private static readonly Menu Menu = new Menu();
        private static readonly GameLogic GameLogic = new GameLogic();
        private static readonly DataInterlayer DataInterlayer = new DataInterlayer();
        private static readonly MenuInputProcessor MenuInputProcessor = new MenuInputProcessor();
        private static readonly GameInterface GameInterface = new GameInterface();
        private static readonly AfterLevelScreen AfterLevelScreen = new AfterLevelScreen();
        private static readonly GameInputProcessor GameInputProcessor = new GameInputProcessor();

        private static void GraphicsThread() {
            var currentLevel = GameLogic.CurrentLevel;
            var player = GameLogic.Player;
            while (_gameStatus == 1) {
                GameInterface.DrawUpperInterface(currentLevel.LevelName, player.Score, currentLevel.Aim);
                GameInterface.DrawPlayerInterface(currentLevel.DiamondsQuantity, player.CollectedDiamonds,
                    player.MaxEnergy, player.Energy, player.MaxHp, player.Hp, player.Name);
                GameInterface.NewDraw(() => currentLevel);
            }
            if (_gameStatus == 2) AfterLevelScreen.DrawGameWin(player.Score, player.AllScores);
            else if (_gameStatus == 3) AfterLevelScreen.DrawGameLose();
        }

        private static void GameLogicThread() {
            while (_gameStatus == 1) {
                Console.CursorVisible = false;
                Thread.Sleep(1000 / GameLogicTickRate);
                GameLogic.GameLoop();
            }
        }

        private static void InputThread() {
            while (_gameStatus == 1) {
                var c = Console.ReadKey(true);
                GameInputProcessor.ProcessInput(c.Key, () => GameLogic.Player, ChangeGameStatus);
            }
        }

        public static void Start() {
            // SoundPlayer soundPlayer = new SoundPlayer(); //TODO: dont forget to enable music on build!
            // soundPlayer.playMusic();
            Menu.DrawMenu(_currentMenuAction);
            MenuGameCycle();

            void MenuGameCycle() {
                Menu.DrawMenu(_currentMenuAction);
                while (_gameStatus == 0) {
                    var c = Console.ReadKey(true);
                    MenuInputProcessor.ProcessInput(
                        c.Key,
                        () => { Environment.Exit(0); },
                        ChangeGameStatus,
                        i => {
                            ChangeCurrentMenuAction(i);
                            Menu.DrawMenu(_currentMenuAction);
                        },
                        () => _currentMenuAction,
                        () => {
                            Menu.DrawNewGame();
                            var name = Console.ReadLine();
                            DataInterlayer.AddGameSave(name);
                            DataInterlayer.GetGameSaves();
                            var currentSave = DataInterlayer.Saves[^1];
                            GameLogic.CreateLevel(currentSave.LevelName, currentSave.Name, ChangeGameStatus,
                                () => DataInterlayer);
                            GameLogic.CurrentSave = currentSave;
                        },
                        () => {
                            Menu.DrawHelp();
                            Console.ReadKey();
                            Menu.DrawMenu(
                                4); //3 is "Help" index in _menuActions, so new  menu will be with this element current
                        },
                        () => {
                            Menu.DrawSettings();
                            Console.ReadKey();
                            Menu.DrawMenu(
                                2); //2 is "Settings" index in _menuActions, so new  menu will be with this element current
                        },
                        () => {
                            var results = DataInterlayer.GetBestScores();
                            Menu.DrawScores(results);
                            try {
                                Console.ReadKey();
                                Menu.DrawMenu(
                                    3); //2 is "Settings" index in _menuActions, so new  menu will be with this element current
                            }
                            catch (Exception e) {
                                Console.WriteLine("Unable to read file with best scores");
                                Console.WriteLine(e.Message);
                            }
                        },
                        () => {
                            DataInterlayer.GetGameSaves();
                            var saves = DataInterlayer.Saves;
                            Menu.DrawSaves(saves);
                            var id = Console.ReadLine();
                            foreach (var save in saves)
                                if (save.Id == int.Parse(id ?? throw new Exception("Id not found"))) {
                                    ResumeGame(save);
                                    ChangeGameStatus(1);
                                }
                        }
                    );
                }
                if (_gameStatus == 1) {
                    Console.Clear();
                    Parallel.Invoke(GraphicsThread, GameLogicThread, InputThread);
                }
                MenuGameCycle();
            }
        }
        private static void ResumeGame(Save save) {
            GameLogic.CreateLevel(save.LevelName, save.Name, ChangeGameStatus, () => DataInterlayer);
            GameLogic.Player.Score = save.Score;
            GameLogic.CurrentSave = save;
        }
    }
}