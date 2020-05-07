namespace ClassLibrary.Entities.Basic {
    public class EmptySpace : GameEntity {
        public EmptySpace(int i, int j):base(i, j) {
            EntityType = 1;
        }
        public override void GameLoopAction() {
        }
    }
}