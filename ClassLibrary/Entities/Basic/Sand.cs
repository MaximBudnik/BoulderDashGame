namespace ClassLibrary.Entities.Basic {
    public class Sand : GameEntity {
        public Sand(int i, int j) {
            entityType = 2;
            PositionX = i;
            PositionY = j;
        }
    }
}