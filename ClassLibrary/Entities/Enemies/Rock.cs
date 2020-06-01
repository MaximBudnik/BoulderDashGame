using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Rock : Enemy {
        private bool _isFalling;
        public Rock(int i, int j, Func<Level> getLevel,
            Action<int> changePlayerHp
        ) : base(i, j, getLevel, changePlayerHp) {
            EntityType = GameEntities.Rock;
            Damage = 3;
            CanMove = false;
            Hp = 1000;
        }
        public new void GameLoopAction() {
            ProcessRock();
            RockDamage();
        }

        private void ProcessRock() {
            var currentLevel = GetLevel();
            _isFalling = false;

            if (RightX >= currentLevel.Width || currentLevel[RightX, PositionY].EntityType != GameEntities.EmptySpace ||
                _isFalling) return;
            Move("horizontal", 1, PositionX, PositionY);
            _isFalling = true;
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            var currentLevel = GetLevel();
            if (posX + value <= currentLevel.Height && posX + value >= 0 &&
                currentLevel[posX, posY + value].EntityType == GameEntities.EmptySpace)
                Move(direction, value, posX, posY);
        }

        private void RockDamage() {
            if (RightX == GetLevel().Width) //not to overflow matrix
                return;
            if (_isFalling && GetLevel()[RightX, PositionY].EntityType == 0) DealDamage();
        }
    }
}