using System;

namespace ClassLibrary.InputProcessors {
    public class MenuInputProcessor : InputProcessor {
        public void processInput(
            ConsoleKey key,
            GameEngine.MipExit exit,
            GameEngine.MipChangeIsGame changeIsGame,
            GameEngine.MipChangeCurrentMenuAction changeMenuAction,
            GameEngine.MipGetOperation getOperation,
            GameEngine.MipCreateNewGame createNewGame,
            GameEngine.MipDrawHelp drawHelp,
            GameEngine.MipDrawSettings drawSettings,
            GameEngine.MipShowBestScores showBestScores,
            GameEngine.MipDrawContinue drawContinue
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
                            drawContinue();
                            break;
                        case 1:
                            createNewGame();
                            changeIsGame();
                            break;
                        case 2:
                            drawSettings();
                            break;
                        case 3:
                            showBestScores();
                            break;
                        case 4:
                            drawHelp();
                            break;
                        case 5:
                            exit();
                            break;
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