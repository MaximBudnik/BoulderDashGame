namespace ClassLibrary.Entities.Basic {
    public class Wall : GameEntity {
        public Wall(int i, int j):base(i, j) {
            EntityType = 5;
            CanMove = false;
        }
    }
}