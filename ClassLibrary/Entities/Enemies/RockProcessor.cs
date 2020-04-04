using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class RockProcessor : Enemy {
        public RockProcessor() {
            entityType = 3;
            _damage = 3;
        }

        private List<int[]> _fallling = new List<int[]>();

        public void ProcessRock() {
            Level currentLevel = GameEngine.GameLogic.CurrentLevel;
            List<int[]> fallenRocks = new List<int[]>();
            _fallling.Clear();
            for (int i = currentLevel.Width - 1; i >= 0; i--) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    int[] currentArray = {i, j};
                    if (
                        currentLevel[i, j].EntityType == 3 &&
                        i + 1 < currentLevel.Width &&
                        currentLevel[i + 1, j].EntityType == 1 &&
                        !fallenRocks.Contains(currentArray)
                    ) {
                        Move("vertical", 1, i, j);
                        int[] current = {i - 1, j};
                        fallenRocks.Add(current);
                        currentArray[0] += 1;
                        _fallling.Add(currentArray);
                        GameEngine.GameLogic.DrawLevel();
                    }
                }
            }
            RockDamage();
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            Level currentLevel = GameEngine.GameLogic.CurrentLevel;
            if (posX + value <= currentLevel.Height && posX + value >= 0 &&
                currentLevel[posX, posY + value].EntityType == 1) {
                Move(direction, value, posX, posY);
            }
        }

        public void RockDamage() {
            foreach (var stone in _fallling) {
                int i = stone[0];
                int j = stone[1];
                if (i + 1 == GameEngine.GameLogic.CurrentLevel.Width) //not to overflow matrix
                    return;
                if (GameEngine.GameLogic.CurrentLevel[i + 1, j].EntityType == 0) {
                    DealDamage(GameEngine.GameLogic.Player, _damage);
                }
            }
        }
    }
}