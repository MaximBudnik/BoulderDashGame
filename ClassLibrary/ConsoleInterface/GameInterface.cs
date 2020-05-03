using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.ConsoleInterface {
    public class GameInterface : UserInterface {
        private readonly Dictionary<int, string> _sprites = new Dictionary<int, string> {
            {0, "☺"},
            {1, " "},
            {2, "·"},
            {3, "○"},
            {4, "×"},
            {5, "│"},
            {6, "*"},
            {7, "?"},
            {8, "·"},
            {9, "/"},
            {203, "•"}
        };

        // private int interfaceMultiplier = 2; //should be retrieved from settings
        private readonly ConsoleColor _secondTextColor = ConsoleColor.DarkCyan;
        private readonly int _topHeight = 2;
        private int _bottomHeight = 3;
        private Level _currLevel;
        public void NewDraw(Func<Level> getLevel) {
            //TODO: optimization
            _currLevel = getLevel();
            int origRow = _topHeight;
            for (int i = 0; i < _currLevel.Width; i++) {
                for (int j = 0; j < _currLevel.Height; j++) {
                    int tmp = _currLevel[i, j].EntityType;
                    Console.SetCursorPosition(2 * j, (2 * i + origRow));
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j, (2 * i + origRow));
                    DrawSprite(tmp);
                    DrawSprite(tmp);

                    Console.SetCursorPosition(2 * j, (2 * i + origRow + 1));
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j, (2 * i + origRow + 1));
                    DrawSprite(tmp);
                    DrawSprite(tmp);
                }
            }
        }
        private void DrawSprite(int item) {
            switch (_currLevel.LevelType) {
                case "default":
                    DrawSpriteDefault(item);
                    break;
                case "red":
                    DrawSpriteRed(item);
                    break;
                case "blue":
                    DrawSpriteBlue(item);
                    break;
            }
        }

        private void DrawSpriteDefault(int item) {
            switch (item) {
                case 0:
                    WriteColorBack(_sprites[0], ConsoleColor.Magenta);
                    break;
                case 1:
                    WriteColorBack(_sprites[1]);
                    break;
                case 2:
                    WriteColorBack(_sprites[2], ConsoleColor.DarkYellow, ConsoleColor.Yellow);
                    break;
                case 3:
                    WriteColorBack(_sprites[3], ConsoleColor.Gray, ConsoleColor.DarkGray);
                    break;
                case 4:
                    WriteColorBack(_sprites[4], ConsoleColor.Cyan);
                    break;
                case 5:
                    WriteColorBack(_sprites[5], primaryTextColor, ConsoleColor.Gray);
                    break;
                case 6:
                    WriteColorBack(_sprites[6], ConsoleColor.Black, ConsoleColor.DarkRed);
                    break;
                case 7:
                    WriteColorBack(_sprites[7], primaryTextColor, ConsoleColor.DarkYellow);
                    break;
                case 8:
                    WriteColorBack(_sprites[8], ConsoleColor.DarkGray);
                    break;
                case 9:
                    WriteColorBack(_sprites[9], ConsoleColor.Gray, ConsoleColor.DarkGray);
                    break;
            }
        }

        private void DrawSpriteRed(int item) {
            switch (item) {
                case 0:
                    WriteColorBack(_sprites[0], ConsoleColor.Magenta);
                    break;
                case 1:
                    WriteColorBack(_sprites[1]);
                    break;
                case 2:
                    WriteColorBack(_sprites[2], ConsoleColor.Yellow, ConsoleColor.Red);
                    break;
                case 3:
                    WriteColorBack(_sprites[203], ConsoleColor.DarkRed, ConsoleColor.Yellow);
                    break;
                case 4:
                    WriteColorBack(_sprites[4], ConsoleColor.White);
                    break;
                case 5:
                    WriteColorBack(_sprites[5], ConsoleColor.Black, ConsoleColor.White);
                    break;
                case 6:
                    WriteColorBack(_sprites[6], ConsoleColor.White, ConsoleColor.DarkRed);
                    break;
                case 7:
                    WriteColorBack(_sprites[7], ConsoleColor.Black, ConsoleColor.DarkYellow);
                    break;
                case 8:
                    WriteColorBack(_sprites[8], ConsoleColor.DarkYellow);
                    break;
                case 9:
                    WriteColorBack(_sprites[9], ConsoleColor.DarkRed, ConsoleColor.Yellow);
                    break;
            }
        }

        private void DrawSpriteBlue(int item) {
            switch (item) {
                case 0:
                    WriteColorBack(_sprites[0], ConsoleColor.Magenta, ConsoleColor.Gray);
                    break;
                case 1:
                    WriteColorBack(_sprites[1], primaryTextColor, ConsoleColor.Gray);
                    break;
                case 2:
                    WriteColorBack(_sprites[2], ConsoleColor.DarkBlue, ConsoleColor.Blue);
                    break;
                case 3:
                    WriteColorBack(_sprites[3], ConsoleColor.DarkCyan, ConsoleColor.Cyan);
                    break;
                case 4:
                    WriteColorBack(_sprites[4], ConsoleColor.Red, ConsoleColor.Gray);
                    break;
                case 5:
                    WriteColorBack(_sprites[5], ConsoleColor.Blue, ConsoleColor.Black);
                    break;
                case 6:
                    WriteColorBack(_sprites[6], ConsoleColor.White, ConsoleColor.DarkRed);
                    break;
                case 7:
                    WriteColorBack(_sprites[7], ConsoleColor.Blue, ConsoleColor.DarkYellow);
                    break;
                case 8:
                    WriteColorBack(_sprites[8], ConsoleColor.Black, ConsoleColor.Gray);
                    break;
                case 9:
                    WriteColorBack(_sprites[9], ConsoleColor.DarkCyan, ConsoleColor.Cyan);
                    break;
            }
        }

        public void DrawPlayerInterface(int diamondsAll, int diamondsCollected, int maxEnergy, int currentEnergy,
            int hpMax, int currentHp, string name) {
            void WritePart(string symbol, int fill, int all, ConsoleColor primary) {
                ChangeForegroundColor(primary);
                for (int i = 0; i < fill; i++) {
                    Console.Write(symbol);
                }
                ChangeForegroundColor(ConsoleColor.DarkGray);
                for (int i = 0; i < all - fill; i++) {
                    Console.Write(symbol);
                }
                Console.Write("   ");
                ChangeForegroundColor(_secondTextColor);
            }
            ChangeForegroundColor(ConsoleColor.DarkCyan);
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - _bottomHeight;
            Console.WriteLine();
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - _bottomHeight;
            DrawLine();
            Console.Write(" Name: ");
            WriteForeground($"{name}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("HP: ");
            WritePart("♥", currentHp, hpMax, ConsoleColor.Red);
            Console.Write("Energy: ");
            WritePart("■", currentEnergy, maxEnergy, ConsoleColor.DarkYellow);
            Console.Write("Diamonds: ");
            WriteForeground($"{diamondsCollected}/{diamondsAll}   ", ConsoleColor.Cyan, _secondTextColor);
            Console.Write("I/A: ");
            WriteForeground("n/a", ConsoleColor.Green, _secondTextColor);
            ChangeForegroundColor(primaryTextColor);
        }

        public void DrawUpperInterface(int levelName, int score, string aim) {
            Console.CursorTop = 0;
            SkipLine();
            Console.CursorTop = 0;
            ChangeForegroundColor(_secondTextColor);
            Console.Write(" Level: ");
            WriteForeground($"Level {levelName}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("Score: ");
            WriteForeground($"{score}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("Aim: ");
            WriteForeground($"{aim}   ", ConsoleColor.Blue, _secondTextColor);
            Console.Write("\n");
            DrawLine();
            ChangeForegroundColor(primaryTextColor);
        }
    }
}