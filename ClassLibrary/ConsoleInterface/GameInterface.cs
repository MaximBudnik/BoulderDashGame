using System;
using System.Collections.Generic;

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

        private int interfaceMultiplier = 2; //should be retrieved from settings
        
        private void drawSprites(Level level, int i) {
            for (int j = 0; j < level.Height; j++) {
                int item = level[i, j];
                switch (item) {
                    // TODO: actually can be refactored. Add changeColor method
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Blue;

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

        public void Draw(GameLogic.GiGetCurrentLevel currentLevel) {
            Console.Clear();
            Level level = currentLevel();
            for (int i = 0; i < level.Width; i++) {//TODO: refactor me pls
                drawSprites(level, i);
                drawSprites(level, i);
            }
        }
    }
}