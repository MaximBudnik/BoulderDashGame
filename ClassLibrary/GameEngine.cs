using System;
using System.Threading;
using ClassLibrary.InputProcessors;

namespace ClassLibrary {
    public class GameEngine {

        private static int fps = 30; //TODO: carry it out in settings.
        private static bool isGame = false;

        // static System.Media.SoundPlayer Player = new SoundPlayer("menuTheme.wav");

        //delegates for menu input processor
        public delegate void MipChangecurrentMenuAction(int i);

        public delegate string MipGetOperation();

        public delegate void MipExit();

        public delegate void MipChangeIsGame();

        static void ChangeIsGame() {
            isGame = !isGame;
        }

        public static void Start() {

            Menu menu = new Menu();
            menu.СreateMainMenu();


            MipChangecurrentMenuAction MipChangecurrentMenuAction = delegate(int i) {
                menu.ChangecurrentMenuAction(i);
            };
            MipGetOperation MipGetOperation = delegate { return menu.GetOperation(); };
            MipExit MipExit = delegate { Environment.Exit(0); };
            MipChangeIsGame MipChangeIsGame = delegate { ChangeIsGame(); };

            ConsoleKeyInfo c = new ConsoleKeyInfo();

            do {
                while (Console.KeyAvailable == false) {
                    Console.CursorVisible = false; //TODO: Important! I constantly hide cousror here.
                    Thread.Sleep(1000 / fps);
                }

                c = Console.ReadKey(true);

                if (!isGame) {
                    InputProcessor inputProcessor = new InputProcessor();
                    inputProcessor.processInput(c.Key, MipExit, MipGetOperation, MipChangecurrentMenuAction, MipChangeIsGame);
                }
                else if (isGame) { }

            } while (c.Key != ConsoleKey.Escape);


        }
    }
}