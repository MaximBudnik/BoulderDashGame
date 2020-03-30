using System.Collections.Generic;

namespace ClassLibrary.Entities {
    public class RockProcessor : Movable {
        public RockProcessor() {
            entityType = 3;
        }

        public void ProcessRock() {
            // TODO: i think all 2x loops can be replaced with method
            Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            List<int[]> falledRocks = new List<int[]>();
            for (int i = currentLevel.Width - 1; i >= 0; i--) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    int[] currentArray = {i, j};
                    if (
                        currentLevel[i, j] == 3 &&
                        i + 1 < currentLevel.Width &&
                        currentLevel[i + 1, j] == 1 &&
                        !falledRocks.Contains(currentArray)
                    ) {
                        Move("vertical", 1, i, j);
                        int[] current = {i - 1, j};
                        falledRocks.Add(current);
                    }
                }
            }
        }

        public void PushRock(int posX, int posY, string direction, int value) {
            Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            if (posX + value <=currentLevel.Width&& posX + value >= 0 &&currentLevel[posX, posY + value]==1) {
                Move(direction, value, posX, posY);
            }
        }
    }
}