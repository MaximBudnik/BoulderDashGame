namespace ClassLibrary.Entities.Collectable {
    public class Diamond : ItemCollectible{
        
        public Diamond(int i, int j) :base(i, j){
            EntityType = 4;
        }
    }
}