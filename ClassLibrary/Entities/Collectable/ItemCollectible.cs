namespace ClassLibrary.Entities {
    public abstract class ItemCollectible : GameEntity {

        public static readonly int PickUpValue=1;

        protected ItemCollectible(int i, int j):base(i, j) {
            
        }
        
    }
}