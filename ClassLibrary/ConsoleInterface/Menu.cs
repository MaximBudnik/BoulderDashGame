using System;

namespace ClassLibrary.ConsoleInterface {
    public class Menu : UserInterface {
        private new ConsoleColor secondTextColor = ConsoleColor.Red;
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Help", "Exit"};
        public void Draw(int currentMenuAction) {
            Console.Clear();
            Console.WriteLine('\n');
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            LogCentered(
                "███╗░░░███╗██╗███╗░░██╗███████╗██████╗░  ░█████╗░██████╗░██╗░░░██╗███████╗███╗░░██╗████████╗██╗░░░██╗██████╗░███████╗");
            LogCentered(
                "████╗░████║██║████╗░██║██╔════╝██╔══██╗  ██╔══██╗██╔══██╗██║░░░██║██╔════╝████╗░██║╚══██╔══╝██║░░░██║██╔══██╗██╔════╝");
            LogCentered(
                "██╔████╔██║██║██╔██╗██║█████╗░░██████╔╝  ███████║██║░░██║╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░██║░░░██║██████╔╝█████╗░░");
            LogCentered(
                "██║╚██╔╝██║██║██║╚████║██╔══╝░░██╔══██╗  ██╔══██║██║░░██║░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░██║░░░██║██╔══██╗██╔══╝░░");
            LogCentered(
                "██║░╚═╝░██║██║██║░╚███║███████╗██║░░██║  ██║░░██║██████╔╝░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░╚██████╔╝██║░░██║███████╗");
            LogCentered(
                "╚═╝░░░░░╚═╝╚═╝╚═╝░░╚══╝╚══════╝╚═╝░░╚═╝  ╚═╝░░╚═╝╚═════╝░░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░░╚═════╝░╚═╝░░╚═╝╚══════╝ ");
            Console.ForegroundColor = primaryTextColor;
            Console.WriteLine('\n');
            Console.ForegroundColor = ConsoleColor.Yellow;
            LogCentered("█▄▄ █▀▀ █▀▀ █ █▄░█   █▄█ █▀█ █░█ █▀█   ░░█ █▀█ █░█ █▀█ █▄░█ █▀▀ █▄█");
            LogCentered("█▄█ ██▄ █▄█ █ █░▀█   ░█░ █▄█ █▄█ █▀▄   █▄█ █▄█ █▄█ █▀▄ █░▀█ ██▄ ░█░");
            Console.ForegroundColor = primaryTextColor;
            Console.WriteLine('\n');
            for (int i = 0; i < _menuActions.Length; i++) {
                if (i == currentMenuAction) {
                    Console.ForegroundColor = secondTextColor;
                    LogCentered(_menuActions[i]);
                    Console.ForegroundColor = primaryTextColor;
                }
                else {
                    LogCentered(_menuActions[i]);
                }
            }
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 3;
            Console.WriteLine("To choose actions press W, S and Enter");
            Console.WriteLine("Version: 0.2.3(playable) (Only Help / Settings aren't working)");
        }
    }
}