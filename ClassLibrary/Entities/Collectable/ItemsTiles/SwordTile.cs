using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class SwordTile : ItemCollectible {
        public SwordTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.SwordTile;
        }
        public override void BreakAction(Player.Player player) {
            player.Inventory.SwordLevel++;
        }
    }
}