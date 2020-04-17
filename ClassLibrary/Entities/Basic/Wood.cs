namespace ClassLibrary.Entities.Basic {
    public class Wood : GameEntity{
        public Wood(int i, int j) {
            entityType = 9;
            PositionX = i;
            PositionY = j;
        }
    }
}