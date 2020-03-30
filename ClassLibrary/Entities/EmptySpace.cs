namespace ClassLibrary.Entities {
    public class EmptySpace : GameEntity{
        
        public EmptySpace(int i, int j) {
            this.entityType = 1;
            this.positionX = i;
            this.positionY = j;
        }
    }
}