using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class TntTile : ItemCollectible {
        public TntTile(int i, int j) :base(i, j){
            EntityType = 22;
        }
        public static void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().TntQuantity++;
        }
        
        public override void GameLoopAction() {
        }
    }
}