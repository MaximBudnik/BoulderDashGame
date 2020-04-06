using System;
using System.Runtime.InteropServices;

namespace ClassLibrary.ConsoleInterface {
    public class Menu : UserInterface {
        private new ConsoleColor secondTextColor = ConsoleColor.Red;
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Help", "Exit"};
        public void DrawMenu(int currentMenuAction) {
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
            Console.WriteLine("Version: 0.2.4");
        }

        public void DrawHelp() {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            LogCentered("█░█ █▀▀ █░░ █▀█");
            LogCentered("█▀█ ██▄ █▄▄ █▀▀");  
            Console.ForegroundColor = primaryTextColor;
            Console.WriteLine();
            
            Console.WriteLine(" ► To move press W, A, S, D");
            Console.WriteLine();
            Console.WriteLine(" ► You can push stones and break sand");
            Console.WriteLine();
            Console.WriteLine(" ► To complete level you must collect diamonds");
            Console.WriteLine();
            Console.WriteLine(" ► Avoid falling stones and enemies - they can kill you");
            Console.WriteLine();
            Console.WriteLine(" Press any key to continue...");
            Console.ReadKey();
            DrawMenu(3); //3 is "Help" index in _menuActions, so new  menu will be with this element current
        }
        
        public void DrawSettings() {
            Console.Clear();
            Console.WriteLine();
            LogCentered("█▀ █▀▀ ▀█▀ ▀█▀ █ █▄░█ █▀▀ █▀");
            LogCentered("▄█ ██▄ ░█░ ░█░ █ █░▀█ █▄█ ▄█");
            Console.ForegroundColor = primaryTextColor;
            Console.WriteLine();
            
            Console.WriteLine(" This feature is not implemented yet");
            Console.WriteLine();
            Console.WriteLine(" Press any key to continue...");
            Console.ReadKey();
            DrawMenu(2); //2 is "Settings" index in _menuActions, so new  menu will be with this element current
        }
    }
}