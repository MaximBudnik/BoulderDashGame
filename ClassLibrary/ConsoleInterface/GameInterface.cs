using System;
using System.Collections.Generic;

namespace ClassLibrary {
    public class GameInterface : UserInterface {
        private Dictionary<int, string> _sprites = new Dictionary<int, string>(5) {
            {0, "P"},
            {1, " "},
            {2, "-"},
            {3, "O"},
            {4, "D"},
            {5, "D"},
        };

        // public GameInterface() {
        //     _sprites.Add(0, " ");
        //     _sprites.Add(1, "P");
        //     _sprites.Add(2, "-");
        //     _sprites.Add(3, "O");
        //     _sprites.Add(4, "D");
        // }

        public void Draw(GameLogic.GiGetCurrentLevel currentLevel) {
            Console.Clear();
            Level level = currentLevel();
            for (int i = 0; i < level.Width; i++) {
                for (int j = 0; j < level.Height; j++) {
                    int item = level[i, j];
                    switch (item) {
                        case 0:
                            // Console.ForegroundColor = ConsoleBackgroundColor;
                            // Console.Write('0');
                            // Console.ForegroundColor = primaryTextColor;
                            Console.Write(_sprites[0]);
                            break;
                        case 2:
                            Console.Write(_sprites[2]);
                            break;
                        case 3:
                            Console.Write(_sprites[3]);
                            break;
                        case 4:
                            Console.Write(_sprites[4]);
                            break;
                        case 5:
                            Console.Write(_sprites[5]);
                            break;
                    }
                }
                Console.WriteLine(' ');
            }
        }
    }
}