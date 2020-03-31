using System;

namespace ClassLibrary.ConsoleInterface {
    public class Menu : UserInterface {
        //Fields

        // private System.ConsoleColor ConsoleBackgroundColor = ConsoleColor.Red;
        private new System.ConsoleColor secondTextColor = ConsoleColor.Red;

        private string[] menuActions = {"Continue", "New game", "Settings", "Help", "Exit",};
        // private int currentMenuAction = 0;

        //private methods

        //public methods
        public void Draw(int currentMenuAction) {
            Console.Clear();
            // SetBackground(ConsoleBackgroundColor);
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

            for (int i = 0; i < menuActions.Length; i++) {
                if (i == currentMenuAction) {
                    Console.ForegroundColor = secondTextColor;
                    LogCentered(menuActions[i]);
                    Console.ForegroundColor = primaryTextColor;
                }
                else {
                    LogCentered(menuActions[i]);
                }
            }


            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 3;
            Console.WriteLine("To choose actions press W, S and Enter");
            Console.WriteLine("Version: 0.2.1(playable) (Only Help / Settings arent wotking)");
        }
    }
}