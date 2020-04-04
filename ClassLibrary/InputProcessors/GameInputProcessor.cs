using System;
using ClassLibrary.Entities;

namespace ClassLibrary.InputProcessors {//dont know ifgi can do such operations. maybe i need to refactor it using delegates
    public class GameInputProcessor : InputProcessor {

        public void processInput(
            ConsoleKey key
        ) {


            switch (key) {
                case ConsoleKey.W:
                    GameEngine.GameLogic.Player.Move("vertical",-1);
                    break;
                case ConsoleKey.S:
                    GameEngine.GameLogic.Player.Move("vertical",1);
                    break;
                case ConsoleKey.A:
                    GameEngine.GameLogic.Player.Move("horizontal",-1);
                    break;
                case ConsoleKey.D:
                    GameEngine.GameLogic.Player.Move("horizontal",1);
                    break;
                case ConsoleKey.T:
                    GameEngine.GameLogic.Player.Teleport();
                    break;
                case ConsoleKey.Spacebar:
                    GameEngine.GameLogic.Player.HpInEnergy();
                    break;
                case ConsoleKey.Escape:
                    GameEngine.ChangeIsGame();
                    break;
                default:
                    break;
            }
        }
    }
}