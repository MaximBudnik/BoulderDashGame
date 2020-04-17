using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Transactions;

namespace ClassLibrary.ConsoleInterface {
    public class Menu : UserInterface {
        private new ConsoleColor secondTextColor = ConsoleColor.Red;
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Scores", "Help", "Exit",};
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
            DrawMenu(4); //3 is "Help" index in _menuActions, so new  menu will be with this element current
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

        public void DrawSaves(List<save> saves) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n");
            LogCentered("█▀▀ █░█ █▀█ █▀█ █▀ █▀▀   █▀ ▄▀█ █░█ █▀▀");
            LogCentered("█▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   ▄█ █▀█ ▀▄▀ ██▄");
            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            DrawLine();
            foreach (var save in saves) {
                Console.Write($"{save.id} {save.name} {save.levelName} {save.score} \n");
                DrawLine();
            }
            Console.WriteLine("\n");
            Console.ForegroundColor = primaryTextColor;
            Console.Write("Enter id of save:");
            Console.ForegroundColor = ConsoleColor.Red;

        }

        public void DrawNewGame() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine('\n');
            LogCentered("╭━━━┳━━━━┳━━━┳━━━┳━━━━╮╭━╮╱╭┳━━━┳╮╭╮╭╮╭━━━┳━━━┳━╮╭━┳━━━╮");
            LogCentered("┃╭━╮┃╭╮╭╮┃╭━╮┃╭━╮┃╭╮╭╮┃┃┃╰╮┃┃╭━━┫┃┃┃┃┃┃╭━╮┃╭━╮┃┃╰╯┃┃╭━━╯");
            LogCentered("┃╰━━╋╯┃┃╰┫┃╱┃┃╰━╯┣╯┃┃╰╯┃╭╮╰╯┃╰━━┫┃┃┃┃┃┃┃╱╰┫┃╱┃┃╭╮╭╮┃╰━━╮");
            LogCentered("╰━━╮┃╱┃┃╱┃╰━╯┃╭╮╭╯╱┃┃╱╱┃┃╰╮┃┃╭━━┫╰╯╰╯┃┃┃╭━┫╰━╯┃┃┃┃┃┃╭━━╯");
            LogCentered("┃╰━╯┃╱┃┃╱┃╭━╮┃┃┃╰╮╱┃┃╱╱┃┃╱┃┃┃╰━━╋╮╭╮╭╯┃╰┻━┃╭━╮┃┃┃┃┃┃╰━━╮");
            LogCentered("╰━━━╯╱╰╯╱╰╯╱╰┻╯╰━╯╱╰╯╱╱╰╯╱╰━┻━━━╯╰╯╰╯╱╰━━━┻╯╱╰┻╯╰╯╰┻━━━╯");
            Console.WriteLine('\n');
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            LogCentered("░░░░░▄▄▄▄▄░▄░▄░▄░▄░░░░░░░▄█▀▀▀░░░░░░░░▀▀▀█▄░░░░░░░░░░░░░▄░░▄░▀█▄░░");
            LogCentered("▄▄▄▄██▄████▀█▀█▀█▀██▄░░▄███▄▄░░▀▄██▄▀░░▄▄███▄░▄████████▄██▄██▄██░░");
            LogCentered("▀▄▀▄▀▄████▄█▄█▄█▄█████░▀██▄▄▄▄████████▄▄▄▄██▀░█████████████▄████▌░");
            LogCentered("▒▀▀▀▀▀▀▀▀██▀▀▀▀██▀▒▄██░░░▄▄▄▄██████████▄▄▄▄░░░▌████████████▀▀▀▀▀░░");
            LogCentered("▒▒▒▒▒▒▒▒▀▀▒▒▒▒▀▀▄▄██▀▒▒░▐▐▀▐▀░▀██████▀░▀▌▀▌▌░▀▒▐█▄▐█▄▐█▄▐█▄▒░▒░▒░▒");
            Console.WriteLine('\n');
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            DrawLine();
            Console.Write(" Please enter your name:");
            Console.ForegroundColor = ConsoleColor.Red;

        }
        
        public void DrawScores(SortedDictionary<int, string> results) {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            LogCentered("█▄▄ █▀▀ █▀ ▀█▀   █▀ █▀▀ █▀█ █▀█ █▀▀ █▀");
            LogCentered("█▄█ ██▄ ▄█ ░█░   ▄█ █▄▄ █▄█ █▀▄ ██▄ ▄█");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            LogCentered("───────────────▄▄───▐█");
            LogCentered("───▄▄▄───▄██▄──█▀───█─▄");
            LogCentered("─▄██▀█▌─██▄▄──▐█▀▄─▐█▀");
            LogCentered("▐█▀▀▌───▄▀▌─▌─█─▌──▌─▌");
            LogCentered("▌▀▄─▐──▀▄─▐▄─▐▄▐▄─▐▄─▐▄");
            Console.ForegroundColor = primaryTextColor;
            Console.WriteLine();

            try {
                int i = 1;
                foreach (KeyValuePair<int, string> key in results.Reverse()) {
                    LogCentered($"{i}. {key.Key} {key.Value} \n");
                    i++;
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey();
                DrawMenu(3); //2 is "Settings" index in _menuActions, so new  menu will be with this element current
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read file with best scores");
                Console.WriteLine(e.Message);
            }
        }
    }
}