using System;
using System.Drawing;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyDigger : Enemy {
        public EnemyDigger(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityEnumType = GameEntitiesEnum.EnemyDigger;
            Damage = 2;
            Hp = 5;
            ScoreForKill = 30;
            ConditionToMove = (level, point) => true;
            CurrentFrame = Randomizer.Random(0, 4);
        }
        public override void GameLoopAction() {
            EnemyDamageNearTitles();
            EnemyMovement();
        }

        private void EnemyMovement() {
            var level = GetLevel();
            Point dest;
            try {
                dest = GetNextPosition(level);
            }
            catch (Exception) {
                return;
            }
            if (!(level[dest.X, dest.Y] is Player.Player)) {
                level[dest.X, dest.Y] = this;
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                PositionX = dest.X;
                PositionY = dest.Y;
            }
        }
    }
}