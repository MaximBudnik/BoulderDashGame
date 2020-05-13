using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ArmorTile : ItemCollectible {
        public ArmorTile(int i, int j) : base(i, j) {
            EntityType = 23;
        }

        public void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().ArmorLevel++;
        }
    }
}