using System;
using System.Threading;
using ClassLibrary.ConsoleInterface;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public static class GameEngine {
        private static readonly int fps = 5; //TODO: carry it out in settings. Actually it must be much lover than 30!!!
        private static bool _isGame;

        private static int _currentMenuAction;
        private static readonly int MenuItems = 6;

        public static void ChangeIsGame() {
            _isGame = !_isGame;
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
        public static readonly GameLogic GameLogic = new GameLogic();
        public static readonly DataInterlayer DataInterlayer = new DataInterlayer();
        private static readonly MenuInputProcessor MenuInputProcessor = new MenuInputProcessor();


        public static void Start() {
            // SoundPlayer soundPlayer = new SoundPlayer(); //TODO: dont forget to enable music on build!
            // soundPlayer.playMusic();
            Menu.DrawMenu(_currentMenuAction);
            ConsoleKeyInfo c;
            MenuGameCycle();

            void MenuGameCycle() {
                Menu.DrawMenu(_currentMenuAction);
                while (!_isGame) {
                    c = Console.ReadKey(true);
                    MenuInputProcessor.ProcessInput(
                        c.Key,
                        () => { Environment.Exit(0); },
                        ChangeIsGame,
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
                            var currentSave = DataInterlayer.saves[DataInterlayer.saves.Count - 1];
                            GameLogic.CreateLevel(currentSave.levelName, currentSave.name);
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
                            var results = DataInterlayer.getBestSccores();
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
                            var saves = DataInterlayer.saves;
                            Menu.DrawSaves(saves);
                            var id = Console.ReadLine();
                            foreach (var save in saves)
                                if (save.id == int.Parse(id)) {
                                    ResumeGame(save);
                                    ChangeIsGame();
                                }
                        }
                    );
                }
                if (_isGame) {
                    Console.Clear();
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
                        var gameInputProcessor = new GameInputProcessor();
                        gameInputProcessor.ProcessInput(c.Key);
                        GameLogic.GameLoop();
                    } while (_isGame);
                }
                MenuGameCycle();
            }
        }
        public static void ResumeGame(save save) {
            GameLogic.CreateLevel(save.levelName, save.name);
            GameLogic.Player.Score = save.score;
            GameLogic.CurrentSave = save;
        }
        
        
    }
}