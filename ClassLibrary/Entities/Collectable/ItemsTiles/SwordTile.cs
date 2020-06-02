using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class SwordTile : ItemCollectible {
        public SwordTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.SwordTile;
        }
        public void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().SwordLevel++;
        }
    }
}