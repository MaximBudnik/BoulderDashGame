using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public abstract class Enemy : Movable {
        protected int Damage;
        protected readonly Func<int> GetPlayerPosX;
        protected readonly Func<int> GetPlayerPosY;
        private readonly Action<int> _changePlayerHp;

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
        }
        protected Enemy(
            Func<Level> getLevel,
            Func<int> getPlayerPosX,
            Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        ) : base(getLevel) {
            GetPlayerPosX = getPlayerPosX;
            GetPlayerPosY = getPlayerPosY;
            _changePlayerHp = changePlayerHp;
            CanMove = false;
        }

        protected void EnemyDamageNearTitles() {
            int playerPosX = GetPlayerPosX();
            int playerPosY = GetPlayerPosY();
            bool one = Math.Abs(PositionX - playerPosX) == 1 && playerPosY == PositionY;
            bool two = Math.Abs(PositionY - playerPosY) == 1 && playerPosX == PositionX;
            if (one || two) {
                DealDamage();
            }
        }
        
        protected void DealDamage() {
            _changePlayerHp(Damage);
        }
    }
}