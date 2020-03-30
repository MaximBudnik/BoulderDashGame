namespace ClassLibrary.Entities {
    public class Wall : GameEntity {
        
        public Wall(int i, int j) {
            this.entityType = 5;
            this.positionX = i;
            this.positionY = j;
        }
        
    }
}