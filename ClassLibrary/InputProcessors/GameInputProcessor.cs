using System;
using ClassLibrary.Entities;

namespace ClassLibrary.InputProcessors {
    public class GameInputProcessor : InputProcessor {
        
        delegate void MovePlayer(string message, int value);
        
        public void processInput(
            ConsoleKey key
        ) {
            
            MovePlayer MovePlayer = delegate(string message, int value) {   };

            switch (key) {
                case ConsoleKey.W:

                    break;
                case ConsoleKey.S:

                    break;

                case ConsoleKey.A:

                    break;
                case ConsoleKey.D:

                    break;
                default:
                    break;
            }
        }
    }
}