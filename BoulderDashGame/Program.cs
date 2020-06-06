using System;
using ClassLibrary;

namespace BoulderDashGame {
    internal class Program {
        private static void Main(string[] args) {
            try {
                var gameEngine = new GameEngineConsole();
                gameEngine.Start();
            }
            catch (Exception e) {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured. Restart app and contact developer.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.Data);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }
    }
}