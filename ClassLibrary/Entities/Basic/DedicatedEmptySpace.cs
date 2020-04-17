namespace ClassLibrary.Entities.Basic {
    public class DedicatedEmptySpace : GameEntity {
        public DedicatedEmptySpace(int i, int j) {
            entityType = 101;
            PositionX = i;
            PositionY = j;
        }
    }
}