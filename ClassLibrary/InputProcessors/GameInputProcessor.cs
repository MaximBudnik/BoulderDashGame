using System;
using ClassLibrary.Entities;

namespace ClassLibrary.InputProcessors {//dont know ifgi can do such operations. maybe i need to refactor it using delegates
    public class GameInputProcessor : InputProcessor {

        public void processInput(
            ConsoleKey key
        ) {


            switch (key) {
                case ConsoleKey.W:
                    GameEngine.gameLogic.Player.Move("vertical",-1);
                    break;
                case ConsoleKey.S:
                    GameEngine.gameLogic.Player.Move("vertical",1);
                    break;
                case ConsoleKey.A:
                    GameEngine.gameLogic.Player.Move("horizontal",-1);
                    break;
                case ConsoleKey.D:
                    GameEngine.gameLogic.Player.Move("horizontal",1);
                    break;
                case ConsoleKey.T:
                    GameEngine.gameLogic.Player.Teleport();
                    break;
                case ConsoleKey.Spacebar:
                    GameEngine.gameLogic.Player.HpInEnergy();
                    break;
                default:
                    break;
            }
        }
    }
}