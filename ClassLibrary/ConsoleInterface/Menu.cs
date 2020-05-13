using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.DataLayer;

namespace ClassLibrary.ConsoleInterface {
    public class Menu : UserInterface {
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Scores", "Help", "Exit"};
        private readonly ConsoleColor _secondTextColor = ConsoleColor.Red;
        public void DrawMenu(int currentMenuAction) {
            Console.Clear();
            SkipLine();
            ChangeForegroundColor(ConsoleColor.DarkYellow);
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
            ChangeForegroundColor(primaryTextColor);
            SkipLine();
            ChangeForegroundColor(ConsoleColor.Yellow);
            LogCentered("█▄▄ █▀▀ █▀▀ █ █▄░█   █▄█ █▀█ █░█ █▀█   ░░█ █▀█ █░█ █▀█ █▄░█ █▀▀ █▄█");
            LogCentered("█▄█ ██▄ █▄█ █ █░▀█   ░█░ █▄█ █▄█ █▀▄   █▄█ █▄█ █▄█ █▀▄ █░▀█ ██▄ ░█░");
            ChangeForegroundColor(primaryTextColor);
            SkipLine();
            for (var i = 0; i < _menuActions.Length; i++)
                if (i == currentMenuAction) {
                    ChangeForegroundColor(_secondTextColor);
                    LogCentered(_menuActions[i]);
                    ChangeForegroundColor(primaryTextColor);
                }
                else {
                    LogCentered(_menuActions[i]);
                }
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 3;
            Console.WriteLine("To choose actions press W, S and Enter");
            Console.WriteLine("Version: 0.6.2 Console edition");
        }

        public void DrawHelp() {
            Console.Clear();
            SkipLine();
            ChangeForegroundColor(ConsoleColor.DarkGreen);
            LogCentered("█░█ █▀▀ █░░ █▀█");
            LogCentered("█▀█ ██▄ █▄▄ █▀▀");
            ChangeForegroundColor(primaryTextColor);
            SkipLine();
            Console.WriteLine(" ► To move press W, A, S, D");
            SkipLine();
            Console.WriteLine(" ► You can push stones and break sand");
            SkipLine();
            Console.WriteLine(" ► To complete level you must collect diamonds");
            SkipLine();
            Console.WriteLine(" ► Avoid falling stones and enemies - they can kill you");
            SkipLine();
            Console.WriteLine(" Press any key to continue...");
        }

        public void DrawSettings() {
            Console.Clear();
            SkipLine();
            LogCentered("█▀ █▀▀ ▀█▀ ▀█▀ █ █▄░█ █▀▀ █▀");
            LogCentered("▄█ ██▄ ░█░ ░█░ █ █░▀█ █▄█ ▄█");
            ChangeForegroundColor(primaryTextColor);
            SkipLine();
            Console.WriteLine(" This feature is not implemented yet");
            SkipLine();
            Console.WriteLine(" Press any key to continue...");
        }

        public void DrawSaves(List<Save> saves) {
            Console.Clear();
            ChangeForegroundColor(ConsoleColor.DarkBlue);
            SkipLine();
            LogCentered("█▀▀ █░█ █▀█ █▀█ █▀ █▀▀   █▀ ▄▀█ █░█ █▀▀");
            LogCentered("█▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   ▄█ █▀█ ▀▄▀ ██▄");
            SkipLine();
            ChangeForegroundColor(ConsoleColor.Blue);
            DrawLine();
            var i = 1;
            foreach (var save in saves) {
                Console.WriteLine();
                Console.Write($" {i}. Player name: {save.Name} Level: {save.LevelName} Score: {save.Score} \n");
                Console.WriteLine();
                DrawLine();
                i++;
            }
            Console.WriteLine("\n");
            Console.Write(" Enter name of player:");
            ChangeForegroundColor(ConsoleColor.Red);
        }

        public void DrawNewGame() {
            Console.Clear();
            ChangeForegroundColor(ConsoleColor.Magenta);
            SkipLine();
            LogCentered("╭━━━┳━━━━┳━━━┳━━━┳━━━━╮╭━╮╱╭┳━━━┳╮╭╮╭╮╭━━━┳━━━┳━╮╭━┳━━━╮");
            LogCentered("┃╭━╮┃╭╮╭╮┃╭━╮┃╭━╮┃╭╮╭╮┃┃┃╰╮┃┃╭━━┫┃┃┃┃┃┃╭━╮┃╭━╮┃┃╰╯┃┃╭━━╯");
            LogCentered("┃╰━━╋╯┃┃╰┫┃╱┃┃╰━╯┣╯┃┃╰╯┃╭╮╰╯┃╰━━┫┃┃┃┃┃┃┃╱╰┫┃╱┃┃╭╮╭╮┃╰━━╮");
            LogCentered("╰━━╮┃╱┃┃╱┃╰━╯┃╭╮╭╯╱┃┃╱╱┃┃╰╮┃┃╭━━┫╰╯╰╯┃┃┃╭━┫╰━╯┃┃┃┃┃┃╭━━╯");
            LogCentered("┃╰━╯┃╱┃┃╱┃╭━╮┃┃┃╰╮╱┃┃╱╱┃┃╱┃┃┃╰━━╋╮╭╮╭╯┃╰┻━┃╭━╮┃┃┃┃┃┃╰━━╮");
            LogCentered("╰━━━╯╱╰╯╱╰╯╱╰┻╯╰━╯╱╰╯╱╱╰╯╱╰━┻━━━╯╰╯╰╯╱╰━━━┻╯╱╰┻╯╰╯╰┻━━━╯");
            SkipLine();
            ChangeForegroundColor(ConsoleColor.DarkGreen);
            LogCentered("░░░░░▄▄▄▄▄░▄░▄░▄░▄░░░░░░░▄█▀▀▀░░░░░░░░▀▀▀█▄░░░░░░░░░░░░░▄░░▄░▀█▄░░");
            LogCentered("▄▄▄▄██▄████▀█▀█▀█▀██▄░░▄███▄▄░░▀▄██▄▀░░▄▄███▄░▄████████▄██▄██▄██░░");
            LogCentered("▀▄▀▄▀▄████▄█▄█▄█▄█████░▀██▄▄▄▄████████▄▄▄▄██▀░█████████████▄████▌░");
            LogCentered("▒▀▀▀▀▀▀▀▀██▀▀▀▀██▀▒▄██░░░▄▄▄▄██████████▄▄▄▄░░░▌████████████▀▀▀▀▀░░");
            LogCentered("▒▒▒▒▒▒▒▒▀▀▒▒▒▒▀▀▄▄██▀▒▒░▐▐▀▐▀░▀██████▀░▀▌▀▌▌░▀▒▐█▄▐█▄▐█▄▐█▄▒░▒░▒░▒");
            SkipLine();
            ChangeForegroundColor(ConsoleColor.DarkYellow);
            DrawLine();
            Console.Write(" Please enter your name: ");
            ChangeForegroundColor(ConsoleColor.Red);
        }

        public void DrawScores(SortedDictionary<int, string> results) {
            Console.Clear();
            SkipLine();
            ChangeForegroundColor(ConsoleColor.DarkRed);
            LogCentered("█▄▄ █▀▀ █▀ ▀█▀   █▀ █▀▀ █▀█ █▀█ █▀▀ █▀");
            LogCentered("█▄█ ██▄ ▄█ ░█░   ▄█ █▄▄ █▄█ █▀▄ ██▄ ▄█");
            SkipLine();
            ChangeForegroundColor(ConsoleColor.Red);
            LogCentered("───────────────▄▄───▐█");
            LogCentered("───▄▄▄───▄██▄──█▀───█─▄");
            LogCentered("─▄██▀█▌─██▄▄──▐█▀▄─▐█▀");
            LogCentered("▐█▀▀▌───▄▀▌─▌─█─▌──▌─▌");
            LogCentered("▌▀▄─▐──▀▄─▐▄─▐▄▐▄─▐▄─▐▄");
            ChangeForegroundColor(primaryTextColor);
            SkipLine();

            var i = 1;
            foreach (var key in results.Reverse()) {
                LogCentered($"{i}. {key.Key} {key.Value} \n");
                i++;
            }
        }
    }
}