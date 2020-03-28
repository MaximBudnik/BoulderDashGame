using System;
using System.Threading;


namespace ClassLibrary {
    public class GameEngine {

        private static int fps = 30;
        
        private static bool isGame = false;

        public static void Start() {
            Menu menu = new Menu();
            menu.СreateMainMenu();
            ConsoleKeyInfo c;
            do {
                while (Console.KeyAvailable == false)
                    Thread.Sleep(1000/fps);
           
                c = Console.ReadKey(true);
                Console.WriteLine("You pressed the '{0}' key.", c.Key);
            } while(c.Key != ConsoleKey.Escape);
        }
    }
}