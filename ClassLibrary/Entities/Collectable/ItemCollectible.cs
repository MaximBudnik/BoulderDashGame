namespace ClassLibrary.Entities.Collectable {
    public abstract class ItemCollectible : GameEntity {
        protected static readonly int PickUpValue = 1;
        protected ItemCollectible(int i, int j) : base(i, j) { }
    }
}