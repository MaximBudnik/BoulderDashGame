using System;

namespace ClassLibrary.InputProcessors {
    public class MenuInputProcessor : InputProcessor {
        public void ProcessInput(
            ConsoleKey key,
            Action exit,
            Action<GameStatusEnum> changeGameStatus,
            Action<int> changeMenuAction,
            Func<int> getOperation,
            Action createNewGame,
            Action drawHelp,
            Action drawSettings,
            Action showBestScores,
            Action drawContinue
        ) {
            switch (key) {
                case ConsoleKey.W:
                    changeMenuAction(-1);
                    break;
                case ConsoleKey.S:
                    changeMenuAction(1);
                    break;
                case ConsoleKey.Enter:
                    var operation = getOperation();
                    switch (operation) {
                        case 0:
                            drawContinue();
                            break;
                        case 1:
                            createNewGame();
                            changeGameStatus(GameStatusEnum.Game);
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
                    changeGameStatus(GameStatusEnum.Game);
                    break;
            }
        }
    }
}