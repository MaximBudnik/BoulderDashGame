using System;

namespace ClassLibrary.Entities {
    public class Rock : Movable {
        public Rock(int i, int j) {
            entityType = 3;
            this.positionX = i;
            this.positionY = j;
        }

        private void ProcessRock() {
            // Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            // if (
            //     positionX + 1 < currentLevel.Width &&
            //     currentLevel[positionX + 1, positionY].EntityType == 1 
            // ) {
            //     Move("vertical", 1, positionX, positionY);
            // }
            Console.WriteLine(positionX);
        }

        public new void GameLoopAction() {
            ProcessRock();
        }

        public void PushRock(string direction, int value) {
            Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            if (positionX + value <= currentLevel.Width && positionX + value >= 0 &&
                currentLevel[positionX, positionY + value].EntityType == 1) {
                Move(direction, value, positionX, positionY);
            }
        }
    }
}