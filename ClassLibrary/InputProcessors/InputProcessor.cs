using System;

namespace ClassLibrary.InputProcessors {
    public class InputProcessor{
        
        public virtual void processInput(
            ConsoleKey key,
            GameEngine.MipExit exit,
            GameEngine.MipGetOperation getOperation,
            GameEngine.MipChangecurrentMenuAction changeCurrentMenuAction,
            GameEngine.MipChangeIsGame changeIsGame
            ) {

            switch (key) {
                case ConsoleKey.W:
                    changeCurrentMenuAction(-1);
                    break;
                case ConsoleKey.S:
                    changeCurrentMenuAction(1);
                    break;
                case ConsoleKey.Enter:
                    string operation = getOperation();

                    switch (operation) {
                        case "EXIT":
                            exit();
                            break;
                        case "CONTINUE":
                            changeIsGame();
                            break;
                        //TODO: add another switch case statements
                    }
                            
                    break;
                default:
                    break;
            }
        }

    }
}