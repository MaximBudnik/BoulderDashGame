using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ConverterTile : ItemCollectible {
        public ConverterTile(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            EntityEnumType = GameEntitiesEnum.ConverterTile;
        }
        protected override void Collect(Player.Player player) {
            player.Inventory.StoneInDiamondsConverterQuantity++;
        }
    }
}