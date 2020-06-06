using System;
using System.Drawing;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public abstract class Enemy : AdvancedLogic {
        private readonly Action<int> _changePlayerHp;
        private readonly Pathfinder _pathfinder;
        protected readonly Func<int> GetPlayerPosX;
        protected readonly Func<int> GetPlayerPosY;

        protected Func<Level, Point, bool> ConditionToMove = (level, point) =>
            level[point.X, point.Y].CanMove || level[point.X, point.Y].PathFinderMove;

        protected int Damage;
        public int ScoreForKill = 20;

        protected Enemy(
            int i,
            int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX,
            Func<int> getPlayerPosY,
            Action<int> changePlayerHp) : base(getLevel, i, j) {
            GetPlayerPosX = getPlayerPosX;
            GetPlayerPosY = getPlayerPosY;
            _changePlayerHp = changePlayerHp;
            CanMove = false;
            PathFinderMove = true;
            _pathfinder = new Pathfinder();
        }
        protected Enemy(
            int i,
            int j,
            Func<Level> getLevel,
            Action<int> changePlayerHp) : base(getLevel, i, j) {
            _changePlayerHp = changePlayerHp;
            CanMove = false;
        }

        public virtual void SubstractEnemyHp(int value) {
            Hp -= value;
        }

        protected void EnemyDamageNearTitles() {
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var nearbyX = Math.Abs(PositionX - playerPosX) == 1 && playerPosY == PositionY;
            var nearbyY = Math.Abs(PositionY - playerPosY) == 1 && playerPosX == PositionX;
            if (nearbyX || nearbyY) DealDamage();
        }

        protected void DealDamage() {
            _changePlayerHp(Damage);
        }

        protected Point GetNextPosition(Level level, int targetX, int targetY) {
            var path = _pathfinder.FindPath(PositionX, PositionY, targetX, targetY, level, ConditionToMove) ??
                       throw new ArgumentNullException(
                           nameof(level));
            return path[1];
        }
    }
}