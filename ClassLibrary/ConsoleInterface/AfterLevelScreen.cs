using System;
using System.Collections.Generic;
using System.Threading;

namespace ClassLibrary.ConsoleInterface {
    public class AfterLevelScreen : UserInterface {
        private ConsoleColor lose = ConsoleColor.Red;
        private ConsoleColor win = ConsoleColor.Green;

        public void DrawGameLose() {
            Console.Clear();
            Console.ForegroundColor = lose;
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            LogCentered("░██████╗░░█████╗░███╗░░░███╗███████╗  ░█████╗░██╗░░░██╗███████╗██████╗░");
            LogCentered("██╔════╝░██╔══██╗████╗░████║██╔════╝  ██╔══██╗██║░░░██║██╔════╝██╔══██╗");
            LogCentered("██║░░██╗░███████║██╔████╔██║█████╗░░  ██║░░██║╚██╗░██╔╝█████╗░░██████╔╝");
            LogCentered("██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░  ██║░░██║░╚████╔╝░██╔══╝░░██╔══██╗");
            LogCentered("██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░  ██║░░██║░╚████╔╝░██╔══╝░░██╔══██╗");
            LogCentered("╚██████╔╝██║░░██║██║░╚═╝░██║███████╗  ╚█████╔╝░░╚██╔╝░░███████╗██║░░██║");
            LogCentered("░╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝  ░╚════╝░░░░╚═╝░░░╚══════╝╚═╝░░╚═╝");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            LogCentered("Press any key to return in menu...");
            Thread.Sleep(3000);
            Console.ReadKey();
            GameEngine.ChangeIsGame();
            Console.ForegroundColor = primaryTextColor;
        }
        public void DrawGameWin(int score, Dictionary<string, int[]> allScores) {
            Console.Clear();
            Console.ForegroundColor = win;
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            LogCentered("██╗░░░░░███████╗██╗░░░██╗███████╗██╗░░░░░  ░█████╗░░█████╗░███╗░░░███╗██████╗░██╗░░░░░███████╗████████╗███████╗██████╗░");
            LogCentered("██║░░░░░██╔════╝██║░░░██║██╔════╝██║░░░░░  ██╔══██╗██╔══██╗████╗░████║██╔══██╗██║░░░░░██╔════╝╚══██╔══╝██╔════╝██╔══██╗");
            LogCentered("██║░░░░░█████╗░░╚██╗░██╔╝█████╗░░██║░░░░░  ██║░░╚═╝██║░░██║██╔████╔██║██████╔╝██║░░░░░█████╗░░░░░██║░░░█████╗░░██║░░██║");
            LogCentered("██║░░░░░██╔══╝░░░╚████╔╝░██╔══╝░░██║░░░░░  ██║░░██╗██║░░██║██║╚██╔╝██║██╔═══╝░██║░░░░░██╔══╝░░░░░██║░░░██╔══╝░░██║░░██║");
            LogCentered("███████╗███████╗░░╚██╔╝░░███████╗███████╗  ╚█████╔╝╚█████╔╝██║░╚═╝░██║██║░░░░░███████╗███████╗░░░██║░░░███████╗██████╔╝");
            LogCentered("╚══════╝╚══════╝░░░╚═╝░░░╚══════╝╚══════╝  ░╚════╝░░╚════╝░╚═╝░░░░░╚═╝╚═╝░░░░░╚══════╝╚══════╝░░░╚═╝░░░╚══════╝╚═════╝░");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.ForegroundColor = primaryTextColor;
            LogCentered($"Your score: {score}");
            LogCentered($"Level score\n");
            foreach (var key in allScores) {
                LogCentered($"{key.Key}...x{key.Value[0]}...{key.Value[1]}\n");
            }
            Console.WriteLine("\n\n\n");
            
            LogCentered("Press Enter to play next level or press any key to return in menu...");
            Console.ForegroundColor = primaryTextColor;
        }
    }
}