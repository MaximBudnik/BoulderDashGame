using System;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.
    InputProcessors { //dont know ifgi can do such operations. maybe i need to refactor it using delegates
    public class GameInputProcessor : InputProcessor {
        public void ProcessInput(
            ConsoleKey key,
            Func<Player> getPlayer,
            Action<GameStatusEnum> changeGameStatus
        ) {
            var player = getPlayer();
            switch (key) {
                case ConsoleKey.W:
                    player.Move(MoveDirectionEnum.Horizontal, -1);
                    player.Keyboard.W = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.S:
                    player.Move(MoveDirectionEnum.Horizontal, 1);
                    player.Keyboard.S = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.A:
                    player.Move(MoveDirectionEnum.Vertical, -1);
                    player.Keyboard.A = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.D:
                    player.Move(MoveDirectionEnum.Vertical, 1);
                    player.Keyboard.D = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.T:
                    player.Teleport();
                    player.Keyboard.T = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.Spacebar:
                    player.UseEnergyConverter();
                    player.Keyboard.Space = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.Q:
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.E:
                    player.UseDynamite();
                    player.Keyboard.E = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.R:
                    player.Attack();
                    player.Keyboard.R = KeyboardEnum.Enabled;
                    break;
                case ConsoleKey.Escape:
                    changeGameStatus(GameStatusEnum.Menu);
                    break;
            }
        }
    }
}