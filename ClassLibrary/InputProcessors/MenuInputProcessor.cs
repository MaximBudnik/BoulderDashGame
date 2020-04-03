using System;

namespace ClassLibrary.InputProcessors {
    public class MenuInputProcessor : InputProcessor {
        public void processInput(
            ConsoleKey key,
            GameEngine.MipExit exit,
            GameEngine.MipChangeIsGame changeIsGame,
            GameEngine.MipChangeCurrentMenuAction changeMenuAction,
            GameEngine.MipGetOperation getOperation,
            GameEngine.MipCreateNewLevel createNewLevel
        ) {
            switch (key) {
                case ConsoleKey.W:
                    changeMenuAction(-1);
                    break;
                case ConsoleKey.S:
                    changeMenuAction(1);
                    break;
                case ConsoleKey.Enter:
                    int operation = getOperation();
                    switch (operation) {
                        case 0:
                            changeIsGame();
                            break;
                        case 1:
                            createNewLevel();
                            changeIsGame();
                            break;
                        case 4:
                            exit();
                            break;
                        //TODO: add another switch case statements
                    }
                    break;

                case ConsoleKey.Escape:
                    changeIsGame();
                    break;

                default:
                    break;
            }
        }
    }
}