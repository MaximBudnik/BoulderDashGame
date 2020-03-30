using System;
using System.ComponentModel.Design;
using System.Threading;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public class GameEngine {
        private static int fps = 8; //TODO: carry it out in settings. Actually it must be much lover than 30!!!
        private static bool isGame = false;

        public static int currentMenuAction { get; set; } = 0;

        //delegates for menu input processor
        public delegate int MipGetOperation();

        public delegate void MipExit();

        public delegate void MipChangeIsGame();

        public delegate void MipChangeCurrentMenuAction(int i);

        public static void ChangeIsGame() {
            isGame = !isGame;
        }
        static void ChangeCurrentMenuAction(int i) {
            if (currentMenuAction < 5 && currentMenuAction >= 0) {
                //5 is number of items in menu Location: ConsoleInterface/Menu.cs TODO: refactor this.
                currentMenuAction += i;
                if (currentMenuAction == 5) {
                    currentMenuAction = 0;
                }
                else if (currentMenuAction == -1) {
                    currentMenuAction = 5 - 1;
                }
            }
        }

        //delegates for game input processor

        public static Menu menu = new Menu();
        public static GameLogic gameLogic = new GameLogic();

        public static void Start() {
            menu.Draw(currentMenuAction);
            gameLogic.CreateLevel("level1");//TODO: create level from menu
            ConsoleKeyInfo c = new ConsoleKeyInfo();
            Frame();

            void Frame() {
                menu.Draw(currentMenuAction);
                if (!isGame) {
                    do {
                        c = Console.ReadKey(true); //read key without imputing it

                        //i just give all methods to menuInputProcessor, so it looks on input and performs actions
                        MipExit MipExit = delegate { Environment.Exit(0); };
                        MipChangeIsGame MipChangeIsGame = delegate { ChangeIsGame(); };
                        MipChangeCurrentMenuAction MipChangeCurrentMenuAction = delegate(int i) {
                            ChangeCurrentMenuAction(i);
                            menu.Draw(currentMenuAction);
                        };
                        MipGetOperation MipGetOperation = delegate { return currentMenuAction; };

                        MenuInputProcessor menuInputProcessor = new MenuInputProcessor();
                        menuInputProcessor.processInput(
                            c.Key,
                            MipExit,
                            MipChangeIsGame,
                            MipChangeCurrentMenuAction,
                            MipGetOperation
                        );
                    } while (!isGame);
                }
                if (isGame) {
                    Console.Clear();
                    do {
                        //do while loop represents all actions and responses on it as in menu as in game

                        while (Console.KeyAvailable == false) {
                            //this actions perform constant times (fps) in second.
                            //As for isGame mode, we perform gameLoop() here (ex. here enemies are moved)
                            Console.CursorVisible = false; //TODO: Important! I constantly hide cursor here.
                            Thread.Sleep(1000 / fps);
                            gameLogic.GameLoop();
                            //TODO: insert game loop here!!!
                        }

                        c = Console.ReadKey(true);
                        //if it isGame mode (we are actually playing) So here our key pressings are processed, while independent game logic
                        //is calculated in while loop
                        
                        
                        //IMPORTANT: we need to call gameLoop also while we input smth or save input out of the loop
                        GameInputProcessor gameInputProcessor = new GameInputProcessor();
                        gameInputProcessor.processInput(c.Key);
                        // gameLogic.GameLoop();
                    } while (c.Key != ConsoleKey.Escape);// TODO: carry it in iunput processor
                    ChangeIsGame();
                }
                Frame();
            }
        }
    }
}