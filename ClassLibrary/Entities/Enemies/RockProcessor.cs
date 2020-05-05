using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class RockProcessor : Enemy {
        public RockProcessor(Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        ) : base(getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = 3;
            Damage = 3;
            CanMove = false;
        }

        private readonly List<int[]> _falling = new List<int[]>();

        public new void GameLoopAction() {
            ProcessRock();
            RockDamage();
        }

        private void ProcessRock() {
            var currentLevel = GetLevel();
            var fallenRocks = new List<int[]>();
            _falling.Clear();
            for (var i = currentLevel.Width - 1; i >= 0; i--)
            for (var j = 0; j < currentLevel.Height; j++) {
                int[] currentArray = {i, j};
                if (
                    currentLevel[i, j].EntityType == 3 &&
                    i + 1 < currentLevel.Width &&
                    currentLevel[i + 1, j].EntityType == 1 &&
                    !fallenRocks.Contains(currentArray)
                ) {
                    Move("horizontal", 1, i, j);
                    int[] current = {i - 1, j};
                    fallenRocks.Add(current);
                    currentArray[0] += 1;
                    _falling.Add(currentArray);
                }
            }
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            var currentLevel = GetLevel();
            if (posX + value <= currentLevel.Height && posX + value >= 0 &&
                currentLevel[posX, posY + value].EntityType == 1)
                Move(direction, value, posX, posY);
        }

        private void RockDamage() {
            foreach (var stone in _falling) {
                var i = stone[0];
                var j = stone[1];
                if (i + 1 == GetLevel().Width) //not to overflow matrix
                    return;
                if (GetLevel()[i + 1, j].EntityType == 0) DealDamage();
            }
        }
    }
}