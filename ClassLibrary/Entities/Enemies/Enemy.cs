using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public abstract class Enemy : Movable {
        private readonly Action<int> _changePlayerHp;
        protected readonly Func<int> GetPlayerPosX;
        protected readonly Func<int> GetPlayerPosY;
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
            var one = Math.Abs(PositionX - playerPosX) == 1 && playerPosY == PositionY;
            var two = Math.Abs(PositionY - playerPosY) == 1 && playerPosX == PositionX;
            if (one || two) DealDamage();
        }

        protected void DealDamage() {
            _changePlayerHp(Damage);
        }
    }
}