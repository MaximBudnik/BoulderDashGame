namespace ClassLibrary.Entities.Collectable {
    public class Diamond : GameEntity{
        public Diamond(int i, int j) {
            entityType = 4;
            this.positionX = i;
            this.positionY = j;
        }
    }
}