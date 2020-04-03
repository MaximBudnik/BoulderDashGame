namespace ClassLibrary.Entities {
    public class Wall : GameEntity {
        public Wall(int i, int j) {
            entityType = 5;
            PositionX = i;
            PositionY = j;
        }
    }
}