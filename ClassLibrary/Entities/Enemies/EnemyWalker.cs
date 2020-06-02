using System;
using System.Drawing;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyWalker : Enemy {
        public EnemyWalker(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityEnumType = GameEntitiesEnum.EnemyWalker;
            Damage = 5;
            Hp = 10;
            ScoreForKill = 40;
            CurrentFrame = Randomizer.Random(0, 3);
            ConditionToMove = (level, point) =>
                level[point.X, point.Y].CanMove || level[point.X, point.Y].PathFinderMove;
        }

        public new void GameLoopAction() {
            EnemyDamageNearTitles();
            Move();
        }

        private void Move() {
            var level = GetLevel();
            Point dest;
            try {
                dest = GetNextPosition(level);
            }
            catch (Exception) {
                return;
            }
            if (level[dest.X, dest.Y].CanMove) {
                level[dest.X, dest.Y] = this;
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                PositionX = dest.X;
                PositionY = dest.Y;
            }
        }
    }
}