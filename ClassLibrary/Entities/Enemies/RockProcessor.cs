using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class RockProcessor : Enemy {
        public RockProcessor(Func<Level> getLevel, Action drawLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action updatePlayerInterface,
            Action<int> changePlayerHp
        ) : base(getLevel, drawLevel, updatePlayerInterface, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = 3;
            Damage = 3;
        }

        private readonly List<int[]> _fallling = new List<int[]>();

        public void ProcessRock() {
            var currentLevel = GetLevel();
            var fallenRocks = new List<int[]>();
            _fallling.Clear();
            for (var i = currentLevel.Width - 1; i >= 0; i--)
            for (var j = 0; j < currentLevel.Height; j++) {
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
                }
            }
            DrawLevel();
            RockDamage();
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            var currentLevel = GetLevel();
            if (posX + value <= currentLevel.Height && posX + value >= 0 &&
                currentLevel[posX, posY + value].EntityType == 1)
                Move(direction, value, posX, posY);
        }

        private void RockDamage() {
            foreach (var stone in _fallling) {
                var i = stone[0];
                var j = stone[1];
                if (i + 1 == GetLevel().Width) //not to overflow matrix
                    return;
                if (GetLevel()[i + 1, j].EntityType == 0) DealDamage();
            }
        }
    }
}