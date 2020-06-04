using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable {
    public abstract class ItemCollectible : GameEntity {
        protected readonly int PickUpValue = 1;

        protected ItemCollectible(int i, int j) : base(i, j) {
        }

        
        public override void BreakAction(Player.Player player) {
            
        }
    }
}