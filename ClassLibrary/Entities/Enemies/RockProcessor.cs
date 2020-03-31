using System.Collections.Generic;

namespace ClassLibrary.Entities {
    public class RockProcessor : Enemy {
        public RockProcessor() {
            entityType = 3;
            _damage = 3;
        }
        
        private List<int[]> fallling = new List<int[]>();
        
        public void ProcessRock() {
            // TODO: i think all 2x loops can be replaced with method
            Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            List<int[]> falledRocks = new List<int[]>();
            fallling.Clear();
            for (int i = currentLevel.Width - 1; i >= 0; i--) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    int[] currentArray = {i, j};
                    if (
                        currentLevel[i, j].EntityType == 3 &&
                        i + 1 < currentLevel.Width &&
                        currentLevel[i + 1, j].EntityType == 1 &&
                        !falledRocks.Contains(currentArray)
                    ) {
                        Move("vertical", 1, i, j);
                        int[] current = {i - 1, j};
                        falledRocks.Add(current);
                        currentArray[0] += 1;
                        fallling.Add(currentArray);
                    }
                }
            }
            RockDamage();
        }

        public void PushRock(int posX, int posY, string direction, int value) {
            Level currentLevel = GameEngine.gameLogic.CurrentLevel;
            if (posX + value <=currentLevel.Height&& posX + value >= 0 &&currentLevel[posX, posY + value].EntityType==1) {
                Move(direction, value, posX, posY);
            }
        }

        public void RockDamage() {
            foreach (var stone in fallling) {
                int i = stone[0];
                int j = stone[1];
                if(i+1 ==GameEngine.gameLogic.CurrentLevel.Width)//not to overflow matrix
                    return;
                if (GameEngine.gameLogic.CurrentLevel[i + 1, j].EntityType == 0) {
                    DealDamage(GameEngine.gameLogic.Player, _damage);
                }
            }
        }
        
    }
}