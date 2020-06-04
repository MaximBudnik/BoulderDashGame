using System;
using System.Drawing;
using ClassLibrary.Entities.Basic;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public partial class SmartEnemy {
        protected void AddEnergy() { }
        protected void IdleAction() { }
        protected void PowerfulAttack() { }
        protected void ChasePlayer() {
            Move();
            Log($"Bot decided to move");
        }
        protected void RunFromPlayer() {
            
        }
        protected void RegenerateHp() { }
        protected void UseShield() { }
        protected void UseDynamite() { }
        protected void UseAcid() { }
        protected void Teleport() { }
        protected void Move() {
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