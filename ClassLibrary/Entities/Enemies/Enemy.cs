using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public abstract class Enemy : Movable {
        protected int Damage;
        protected readonly Func<int> GetPlayerPosX;
        protected readonly Func<int> GetPlayerPosY;
        protected readonly Action<int> ChangePlayerHp;

        protected Enemy(
            int i,
            int j,
            Func<Level> getLevel,
            Action drawLevel,
            Action updatePlayerInterface,
            Func<int> getPlayerPosX,
            Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        ) : base(getLevel, drawLevel, updatePlayerInterface, i, j) {
            GetPlayerPosX = getPlayerPosX;
            GetPlayerPosY = getPlayerPosY;
            ChangePlayerHp = changePlayerHp;
        }
        protected Enemy(
            Func<Level> getLevel,
            Action drawLevel,
            Action updatePlayerInterface,
            Func<int> getPlayerPosX,
            Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        ) : base(getLevel, drawLevel, updatePlayerInterface) {
            GetPlayerPosX = getPlayerPosX;
            GetPlayerPosY = getPlayerPosY;
            ChangePlayerHp = changePlayerHp;
        }

        protected void EnemyDefaultDamage() {
            int playerPosX = GetPlayerPosX();
            int playerPosY = GetPlayerPosY();
            bool one = Math.Abs(PositionX - playerPosX) == 1 && playerPosY == PositionY;
            bool two = Math.Abs(PositionY - playerPosY) == 1 && playerPosX == PositionX;
            if (one || two) {
                DealDamage();
            }
        }

        protected void DealDamage() {
            ChangePlayerHp(Damage);
            UpdatePlayerInterface();
        }
        
    }
}