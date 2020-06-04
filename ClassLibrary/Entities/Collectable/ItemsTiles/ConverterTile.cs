using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ConverterTile : ItemCollectible {
        public ConverterTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.ConverterTile;
        }
        public override void BreakAction(Player.Player player) {
            player.Inventory.StoneInDiamondsConverterQuantity++;
        }
    }
}