using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class SwordTile : ItemCollectible {
        public SwordTile(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            EntityEnumType = GameEntitiesEnum.SwordTile;
        }
        protected override void Collect(Player.Player player) {
            player.Inventory.SwordLevel++;
        }
    }
}