using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable {
    public abstract class ItemCollectible : GameEntity {
        private readonly Action<SoundFilesEnum> _playSound;
        protected readonly int PickUpValue = 1;

        protected ItemCollectible(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j) {
            _playSound = playSound;
            MoveWeight = 65;
        }

        protected virtual void Collect(Player.Player player) { }

        public override void BreakAction(Player.Player player) {
            Collect(player);
            _playSound(SoundFilesEnum.PickupSound);
        }
    }
}