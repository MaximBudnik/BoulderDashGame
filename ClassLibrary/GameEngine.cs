using System;
using System.Threading;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public class GameEngine {
        private static int fps = 30; //TODO: carry it out in settings. Actually it must be much lover than 30!!!
        private static bool isGame = false;

        public static int currentMenuAction { get; set; } = 0;

        //delegates for menu input processor
        public delegate int MipGetOperation();

        public delegate void MipExit();

        public delegate void MipChangeIsGame();

        public delegate void MipChangeCurrentMenuAction(int i);

        static void ChangeIsGame() {
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
        
        
        
        public static void Start() {
            Menu menu = new Menu();
            menu.CreateMainMenu(currentMenuAction);
            GameLogic gameLogic = new GameLogic();
            gameLogic.CreateLevel("level1");

            ConsoleKeyInfo c = new ConsoleKeyInfo();

            //do while loop represents all actions and responses on it as in menu as in game
            do {
                //this actions perform constant times (fps) in second. It is not necessary for menu, so it can be refactored.
                //As for isGame mode, we perform gameLoop() here (ex. here enemies are moved)
                while (Console.KeyAvailable == false) {
                    Console.CursorVisible = false; //TODO: Important! I constantly hide cursor here.
                    Thread.Sleep(1000 / fps);
                    //TODO: insert game loop here!!!
                }

                c = Console.ReadKey(true); //read key without inputin it

                if (!isGame) { //i just give all methods to menuInputProcessor, so it looks on input and performs actions
                    MipExit MipExit = delegate { Environment.Exit(0); };
                    MipChangeIsGame MipChangeIsGame = delegate { ChangeIsGame(); };
                    MipChangeCurrentMenuAction MipChangeCurrentMenuAction = delegate(int i) {
                        ChangeCurrentMenuAction(i);
                        menu.CreateMainMenu(currentMenuAction);
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
                }
                if (isGame) {//if it isGame mode (we are actually playing) So here our key pressings are processed, while independent game logic
                             //is calculated in while loop
                    Console.WriteLine("Zaebumba");
                    GameInputProcessor gameInputProcessor = new GameInputProcessor();
                    gameInputProcessor.processInput();
                }
            } while (c.Key != ConsoleKey.Escape); //TODO: easy and important but maybe some refactor? + its global restart(bad)
            ChangeIsGame();
            Start();
        }
    }
}