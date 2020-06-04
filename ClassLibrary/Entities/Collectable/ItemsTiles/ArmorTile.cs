using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ArmorTile : ItemCollectible {
        public ArmorTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.ArmorTile;
        }

        public override void BreakAction(Player.Player player) {
            player.Inventory.ArmorLevel++;
        }
    }
}