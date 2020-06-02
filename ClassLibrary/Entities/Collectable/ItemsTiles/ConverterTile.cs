using System;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Entities.Collectable.ItemsTiles {
    public class ConverterTile : ItemCollectible {
        public ConverterTile(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.ConverterTile;
        }
        public void Collect(Func<Inventory> getPlayerInventory) {
            getPlayerInventory().StoneInDiamondsConverterQuantity++;
        }
    }
}