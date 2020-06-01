using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class SwordTile : ItemCollectible {
        public SwordTile(int i, int j) : base(i, j) {
            EntityType = GameEntities.SwordTile;
        }
        public void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().SwordLevel++;
        }
    }
}