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
        };
        // private int interfaceMultiplier = 2; //should be retrieved from settings
        private new ConsoleColor secondTextColor = ConsoleColor.DarkCyan;
        private int _topHeight = 2;
        private int bottomHeight = 3;

        public void NewDraw(GameLogic.GiGetCurrentLevel currentLevel) {//TODO: optimization
            Level level = currentLevel();
            int origRow =_topHeight;
            for (int i = 0; i < level.Width; i++) {
                for (int j = 0; j < level.Height; j++) {
                    
                            int tmp = level[i, j].EntityType;
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
            switch (item) {
                case 0:
                    WriteColorBack(_sprites[0],ConsoleColor.Magenta);
                    break;
                case 1:
                    WriteColorBack(_sprites[1]);
                    break;
                case 2:
                    WriteColorBack(_sprites[2],ConsoleColor.DarkYellow,ConsoleColor.Yellow);
                    break;
                case 3:
                    WriteColorBack(_sprites[3],ConsoleColor.Gray,ConsoleColor.DarkGray);
                    break;
                case 4:
                    WriteColorBack(_sprites[4],ConsoleColor.Cyan);
                    break;
                case 5:
                    WriteColorBack(_sprites[5], primaryTextColor,ConsoleColor.Gray);
                    break;
                case 6:
                    WriteColorBack(_sprites[6],ConsoleColor.Black,ConsoleColor.DarkRed);
                    break;
                case 7:
                    WriteColorBack(_sprites[7],primaryTextColor,ConsoleColor.DarkYellow);
                    break;
                case 8:
                    WriteColorBack(_sprites[2], ConsoleColor.DarkGray);
                    break;
            }
        }


        
        public void DrawPlayerInterface(int diamondsAll, int diamondsCollected, int maxEnergy, int currentEnergy,
            int hpMax, int currentHp, string name) {
            void WritePart(string symbol, int fill, int all, ConsoleColor primary) {
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
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - bottomHeight;
            for (int i = 0; i < consoleWidth; i++) {
                Console.Write("─");
            }

            Console.Write(" Name: ");
            WriteForeground($"{name}   ",ConsoleColor.White,secondTextColor);
            Console.Write("HP: ");
            WritePart("♥", currentHp, hpMax, ConsoleColor.Red);
            Console.Write("Energy: ");
            WritePart("■", currentEnergy, maxEnergy, ConsoleColor.DarkYellow);
            Console.Write("Diamonds: ");
            WriteForeground($"{diamondsCollected}/{diamondsAll}   ",ConsoleColor.Cyan,secondTextColor);
            Console.Write("I/A: ");
            WriteForeground("n/a",ConsoleColor.Green,secondTextColor);
            Console.ForegroundColor = primaryTextColor;
        }

        public void DrawUpperInterface(string levelName, int score, string aim) {
            Console.CursorTop = 0;
            Console.WriteLine();
            Console.WriteLine();
            Console.CursorTop = 0;
            Console.ForegroundColor = secondTextColor;
            Console.Write(" Level: ");
            WriteForeground($"{levelName}   ",ConsoleColor.White,secondTextColor);
            Console.Write("Score: ");
            WriteForeground($"{score}   ",ConsoleColor.White,secondTextColor);
            Console.Write("Aim: ");
            WriteForeground($"{aim}   ",ConsoleColor.Blue,secondTextColor);
            Console.Write("\n");
            for (int i = 0; i < consoleWidth; i++) {
                Console.Write("─");
            }
            Console.ForegroundColor = primaryTextColor;
        }
    }
}