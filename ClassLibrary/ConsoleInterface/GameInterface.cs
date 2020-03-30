using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClassLibrary {
    public class GameInterface : UserInterface {
        private Dictionary<int, string> _sprites = new Dictionary<int, string>(5) {
            {0, "☺"},
            {1, " "},
            {2, "·"},
            {3, "○"},
            {4, "×"},
            {5, "│"},
        };

        // private int interfaceMultiplier = 2; //should be retrieved from settings
        private protected new  System.ConsoleColor secondTextColor = ConsoleColor.DarkCyan;
        
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
                }
            }
            Console.WriteLine(' ');
        }

        public string getSprite(int i ) {
            return _sprites[i];
        }

        public void Draw(GameLogic.GiGetCurrentLevel currentLevel, int diamondsCollected, int maxEnergy,int currentEnergy) {
            Console.Clear();
            DrawUpperInterface();
            Level level = currentLevel();
            for (int i = 0; i < level.Width; i++) {//TODO: refactor me pls
                drawSprites(level, i);
                drawSprites(level, i);
            }
            DrawPlayerInterface(diamondsCollected,maxEnergy,currentEnergy);
        }

        private void DrawPlayerInterface(int diamondsCollected, int maxEnergy, int currentEnergy) {

            void writePart(string symbol, int fill, int all, ConsoleColor primary) {
                Console.ForegroundColor = primary;
                for (int i = 0; i < fill; i++) {
                    
                    Console.Write(symbol);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (int i = 0; i < all-fill; i++) {
                    Console.Write(symbol);
                }
                Console.Write("   ");
                Console.ForegroundColor = secondTextColor;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 3;
            for (int i = 0; i < consoleWidth; i++) {
                Console.Write("─");
            }
            string name = "Maxim";
            int diamondsAll = 15;
            int currentHp = 8;
            int hpMax = 10;
            Console.Write($" Name: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name}   ");
            Console.ForegroundColor = secondTextColor;
            Console.Write ("HP: ");
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

        private void DrawUpperInterface() {
            string levelName = "Random level";
            int score = 2150;
            string aim = "Collect all diamonds";
            
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