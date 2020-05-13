using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class TntTile : ItemCollectible {
        public TntTile(int i, int j) : base(i, j) {
            EntityType = 22;
        }
        public void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().TntQuantity++;
        }
    }
}