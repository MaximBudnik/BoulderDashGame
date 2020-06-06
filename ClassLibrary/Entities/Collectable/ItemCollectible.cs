using System;
using ClassLibrary.Entities.Enemies.SmartEnemies;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable {
    public abstract class ItemCollectible : GameEntity {
        private readonly Action<SoundFilesEnum> _playSound;
        protected readonly int PickUpValue = 1;

        protected ItemCollectible(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j) {
            _playSound = playSound;
            MoveWeight = 60;
        }

        protected virtual void Collect(Player.Player player) { }
        protected virtual void Collect(SmartEnemy enemy) { }


        public override void BreakAction(Player.Player player) {
            Collect(player);
            _playSound(SoundFilesEnum.PickupSound);
        }
        
        public override void BreakAction(SmartEnemy enemy) {
            enemy.AddEnergy(PickUpValue * 10);
            Collect(enemy);
        }
    }
}