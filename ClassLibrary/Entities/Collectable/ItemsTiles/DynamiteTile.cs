using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class DynamiteTile : ItemCollectible {
        public DynamiteTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.DynamiteTile;
        }
        public override void BreakAction(Player.Player player) {
            player.Inventory.TntQuantity++;
        }
    }
}