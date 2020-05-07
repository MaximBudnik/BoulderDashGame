using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class SwordTile : ItemCollectible {
        public SwordTile(int i, int j) :base(i, j){
            EntityType = 20;
        }
        public static void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().SwordLevel++;
        }
        
        public override void GameLoopAction() {
        }
    }
}