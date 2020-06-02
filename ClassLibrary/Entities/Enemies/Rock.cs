using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Rock : Enemy {
        private bool _isFalling;
        public Rock(int i, int j, Func<Level> getLevel,
            Action<int> changePlayerHp
        ) : base(i, j, getLevel, changePlayerHp) {
            EntityEnumType = GameEntitiesEnum.Rock;
            Damage = 3;
            CanMove = false;
            Hp = 1000;
        }
        public override void GameLoopAction() {
            ProcessRock();
            RockDamage();
        }

        private void ProcessRock() {
            var currentLevel = GetLevel();
            _isFalling = false;

            if (RightX >= currentLevel.Width ||
                currentLevel[RightX, PositionY].EntityEnumType != GameEntitiesEnum.EmptySpace ||
                _isFalling) return;
            Move("horizontal", 1, PositionX, PositionY);
            _isFalling = true;
        }
        public void PushRock(int posX, int posY, string direction, int value) {
            var currentLevel = GetLevel();
            bool b = posX + value <= currentLevel.Height && posX + value >= 0 &&
                     currentLevel[posX, posY + value].EntityEnumType == GameEntitiesEnum.EmptySpace;
            if (b) {
                Move(direction, value, posX, posY);
            }
        }

        private void RockDamage() {
            //not to overflow matrix
            if (RightX == GetLevel().Width) return;
            if (_isFalling && GetLevel()[RightX, PositionY].EntityEnumType == GameEntitiesEnum.Player) DealDamage();
        }
    }
}