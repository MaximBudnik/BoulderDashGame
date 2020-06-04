using System;
using System.Drawing;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public abstract class Enemy : Movable {
        private readonly Action<int> _changePlayerHp;
        private readonly Pathfinder _pathfinder;
        private readonly Func<int> GetPlayerPosX;
        private readonly Func<int> GetPlayerPosY;
        protected Func<Level, Point, bool> ConditionToMove;
        protected int Damage;
        public int ScoreForKill = 20;

        protected Enemy(
            int i,
            int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX,
            Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        ) : base(getLevel, i, j) {
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
            Action<int> changePlayerHp
        ) : base(getLevel, i, j) {
            _changePlayerHp = changePlayerHp;
            CanMove = false;
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

        protected Point GetNextPosition(Level level) {
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var path = _pathfinder.FindPath(PositionX, PositionY, playerPosX, playerPosY, level, ConditionToMove) ??
                       throw new ArgumentNullException(
                           "FindPath(PositionX, PositionY, playerPosX, playerPosY)");
            return path[1];
        }
    }
}