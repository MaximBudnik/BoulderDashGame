using System;
using System.Drawing;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class EnemyWalker : SmartEnemy {
        public EnemyWalker(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp, getOutdatedPlayer) {
            EntityEnumType = GameEntitiesEnum.EnemyWalker;
            Damage = 5;
            MaxHp = 10;
            Hp = 10;
            ScoreForKill = 40;
            CurrentFrame = Randomizer.Random(0, 3);
            ConditionToMove = (level, point) =>
                level[point.X, point.Y].CanMove || level[point.X, point.Y].PathFinderMove;
        }

        public override void GameLoopAction() {
            // EnemyDamageNearTitles();
            // Move();
            MakeDecision();
        }
        
    }
}