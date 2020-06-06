using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ArmorTile : ItemCollectible {
        public ArmorTile(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            EntityEnumType = GameEntitiesEnum.ArmorTile;
            MoveWeight = 40;
        }

        protected override void Collect(Player.Player player) {
            player.Inventory.ArmorLevel++;
        }
    }
}