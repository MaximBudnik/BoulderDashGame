using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ConverterTile : ItemCollectible {
        public ConverterTile(int i, int j) :base(i, j){
            EntityType = 21;
        }
        public static void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().StoneInDiamondsConverterQuantity++;
        }
    }
}