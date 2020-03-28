using System;
using System.Threading;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public class GameEngine {
        private static int fps = 30; //TODO: carry it out in settings.
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

            ConsoleKeyInfo c = new ConsoleKeyInfo();

            do {
                while (Console.KeyAvailable == false) {
                    Console.CursorVisible = false; //TODO: Important! I constantly hide cursor here.
                    Thread.Sleep(1000 / fps);
                }

                c = Console.ReadKey(true);

                if (!isGame) { //i just give all methods to menuInpuProcessor, so it looks on iput and performs actions
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
                if (isGame) {
                    Console.WriteLine("Zaebumba");
                    GameInputProcessor gameInputProcessor = new GameInputProcessor();
                    gameInputProcessor.processInput();
                }
            } while (c.Key != ConsoleKey.Escape); //TODO: easy and important but maybe some refactor?
            ChangeIsGame();
            Start();
        }
    }
}