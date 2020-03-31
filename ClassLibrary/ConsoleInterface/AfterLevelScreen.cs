using System;

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
            LogCentered("Press Esc to return in menu...");
            Console.ForegroundColor = primaryTextColor;
        }
        public void DrawGameWin() {
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
            LogCentered("Press Esc to return in menu...");
            Console.ForegroundColor = primaryTextColor;
        }
    }
}