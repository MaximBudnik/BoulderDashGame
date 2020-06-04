using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class DynamiteTile : ItemCollectible {
        public DynamiteTile(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            EntityEnumType = GameEntitiesEnum.DynamiteTile;
        }
        protected override void Collect(Player.Player player) {
            player.Inventory.TntQuantity++;
        }
    }
}