using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Rock : Enemy {
        public Rock(int i, int j, Func<Level> getLevel,
            Action<int> changePlayerHp
        ) : base( i, j, getLevel, changePlayerHp) {
            EntityType = 3;
            Damage = 3;
            CanMove = false;
        }
        private bool _isFalling = false;
        public new void GameLoopAction() {
            ProcessRock();
            RockDamage();
        }

        private void ProcessRock() {
            var currentLevel = GetLevel();
            _isFalling = false;

            if (
                PositionX + 1 < currentLevel.Width &&
                currentLevel[PositionX + 1, PositionY].EntityType == 1 &&
                !_isFalling
            ) {
                Move("horizontal", 1, PositionX, PositionY);
                _isFalling = true;
            }
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            var currentLevel = GetLevel();
            if (posX + value <= currentLevel.Height && posX + value >= 0 &&
                currentLevel[posX, posY + value].EntityType == 1)
                Move(direction, value, posX, posY);
        }

        private void RockDamage() {
            if (PositionX + 1 == GetLevel().Width) //not to overflow matrix
                return;
            if ( _isFalling && GetLevel()[PositionX + 1, PositionY].EntityType == 0) DealDamage();
        }
    }
}