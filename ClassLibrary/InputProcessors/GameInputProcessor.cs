using System;
using ClassLibrary.Entities;

namespace ClassLibrary.
    InputProcessors { //dont know ifgi can do such operations. maybe i need to refactor it using delegates
    public class GameInputProcessor : InputProcessor {
        public void ProcessInput(
            ConsoleKey key,
            Func<Player> getPlayer,
            Action<int> changeGameStatus
        ) {
            Player player = getPlayer();
            switch (key) {
                case ConsoleKey.W:
                    player.Move("vertical", -1);
                    break;
                case ConsoleKey.S:
                    player.Move("vertical", 1);
                    break;
                case ConsoleKey.A:
                    player.Move("horizontal", -1);
                    break;
                case ConsoleKey.D:
                    player.Move("horizontal", 1);
                    break;
                case ConsoleKey.T:
                    player.Teleport();
                    break;
                case ConsoleKey.Spacebar:
                    player.HpInEnergy();
                    break;
                case ConsoleKey.Q:
                    player.ConvertNearStonesInDiamonds();
                    break;
                case ConsoleKey.Escape:
                    changeGameStatus(0);
                    break;
            }
        }
    }
}