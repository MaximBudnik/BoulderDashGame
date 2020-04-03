using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.ConsoleInterface {
    public class GameInterface : UserInterface {
        private Dictionary<int, string> _sprites = new Dictionary<int, string>(5) {
            {0, "☺"},
            {1, " "},
            {2, "·"},
            {3, "○"},
            {4, "×"},
            {5, "│"},
            {6, "*"},
            {7, "?"},
            {8, "^"}
        };

        // private int interfaceMultiplier = 2; //should be retrieved from settings
        private new System.ConsoleColor secondTextColor = ConsoleColor.DarkCyan;
        private int topHeight = 2;
        private int bottomHeight = 3;

        private void drawSprites(Level level, int i) {
            for (int j = 0; j < level.Height; j++) {
                int item = level[i, j].EntityType;
                switch (item) {
                    // TODO: actually can be refactored. Add changeColor method
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Magenta;

                        Console.Write(_sprites[0]);
                        Console.Write(_sprites[0]);

                        Console.ForegroundColor = primaryTextColor;
                        // Console.ForegroundColor = primaryTextColor;
                        break;
                    case 1:
                        Console.Write(_sprites[1]);
                        Console.Write(_sprites[1]);
                        // Console.ForegroundColor = primaryTextColor;
                        break;

                    case 2:
                        Console.Write(_sprites[2]);
                        Console.Write(_sprites[2]);

                        break;
                    case 3:
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(_sprites[3]);
                        Console.Write(_sprites[3]);
                        Console.ForegroundColor = primaryTextColor;
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(_sprites[4]);
                        Console.Write(_sprites[4]);

                        Console.ForegroundColor = primaryTextColor;
                        break;
                    case 5:
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(_sprites[5]);
                        Console.Write(_sprites[5]);
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case 6:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(_sprites[6]);
                        Console.Write(_sprites[6]);
                        Console.ForegroundColor = primaryTextColor;
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case 7:
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.Write(_sprites[7]);
                        Console.Write(_sprites[7]);
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case 8:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(_sprites[8]);
                        Console.Write(_sprites[8]);
                        Console.ForegroundColor = primaryTextColor;
                        break;
                }
            }
            Console.WriteLine(' ');
        }

        public string getSprite(int i) {
            return _sprites[i];
        }

        public void Draw(GameLogic.GiGetCurrentLevel currentLevel) {
            Console.Clear();
            Level level = currentLevel();
            for (int i = 0; i < level.Width; i++) {
                //TODO: refactor me pls
                drawSprites(level, i);
                drawSprites(level, i);
            }
        }

        public void newDraw(GameLogic.GiGetCurrentLevel currentLevel) {
            Level level = currentLevel();
            int origCol = 0;
            int origRow = 3;
            for (int i = 0; i < level.Width; i++) {
                for (int j = 0; j < level.Height; j++) {
                    Console.SetCursorPosition(2 * j, (2 * i + origRow)); //2 - height of top ui
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j, (2 * i + origRow));
                    drawSprite(level[i, j].EntityType);
                    Console.SetCursorPosition(2 * j, (2 * i + origRow + 1));
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j, (2 * i + origRow + 1));
                    drawSprite(level[i, j].EntityType);
                    Console.SetCursorPosition(2 * j + 1, (2 * i + origRow));
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j + 1, (2 * i + origRow));
                    drawSprite(level[i, j].EntityType);
                    Console.SetCursorPosition(2 * j + 1, (2 * i + origRow + 1));
                    Console.Write(" ");
                    Console.SetCursorPosition(2 * j + 1, (2 * i + origRow + 1));
                    drawSprite(level[i, j].EntityType);
                }
            }
        }
        private void drawSprite(int item) {
            switch (item) {
                // TODO: actually can be refactored. Add changeColor method
                case 0:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(_sprites[0]);
                    Console.ForegroundColor = primaryTextColor;
                    // Console.ForegroundColor = primaryTextColor;
                    break;
                case 1:
                    Console.Write(_sprites[1]);
                    // Console.ForegroundColor = primaryTextColor;
                    break;

                case 2:
                    Console.Write(_sprites[2]);
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(_sprites[3]);
                    Console.ForegroundColor = primaryTextColor;
                    Console.BackgroundColor = ConsoleBackgroundColor;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(_sprites[4]);

                    Console.ForegroundColor = primaryTextColor;
                    break;
                case 5:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write(_sprites[5]);
                    Console.BackgroundColor = ConsoleBackgroundColor;
                    break;
                case 6:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(_sprites[6]);
                    Console.ForegroundColor = primaryTextColor;
                    Console.BackgroundColor = ConsoleBackgroundColor;
                    break;
                case 7:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Write(_sprites[7]);
                    Console.BackgroundColor = ConsoleBackgroundColor;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(_sprites[8]);
                    Console.ForegroundColor = primaryTextColor;
                    break;
            }
        }

        public void DrawPlayerInterface(int diamondsAll, int diamondsCollected, int maxEnergy, int currentEnergy,
            int hpMax, int currentHp) {
            void writePart(string symbol, int fill, int all, ConsoleColor primary) {
                Console.ForegroundColor = primary;
                for (int i = 0; i < fill; i++) {
                    Console.Write(symbol);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (int i = 0; i < all - fill; i++) {
                    Console.Write(symbol);
                }
                Console.Write("   ");
                Console.ForegroundColor = secondTextColor;
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - bottomHeight;
            Console.WriteLine();
            Console.WriteLine();
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - bottomHeight;
            for (int i = 0; i < consoleWidth; i++) {
                Console.Write("─");
            }
            string name = "Maxim";

            Console.Write(" Name: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name}   ");
            Console.ForegroundColor = secondTextColor;
            Console.Write("HP: ");
            writePart("♥", currentHp, hpMax, ConsoleColor.Red);
            Console.Write("Energy: ");
            writePart("■", currentEnergy, maxEnergy, ConsoleColor.DarkYellow);
            Console.Write("Diamonds: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{diamondsCollected}/{diamondsAll}   ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("I/A: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("n/a");
            Console.ForegroundColor = primaryTextColor;
        }

        public void DrawUpperInterface(string levelName, int score, string aim) {
            Console.WriteLine();
            Console.WriteLine();
            Console.CursorTop = 0;
            Console.ForegroundColor = secondTextColor;
            Console.Write(" Level: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{levelName}   ");
            Console.ForegroundColor = secondTextColor;
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{score}   ");
            Console.ForegroundColor = secondTextColor;
            Console.Write("Aim: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{aim}   ");
            Console.ForegroundColor = secondTextColor;
            Console.Write("\n");
            for (int i = 0; i < consoleWidth; i++) {
                Console.Write("─");
            }
            Console.ForegroundColor = primaryTextColor;
        }
    }
}