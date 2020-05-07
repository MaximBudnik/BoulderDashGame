namespace ClassLibrary.Entities.Basic {
    public class DedicatedEmptySpace : GameEntity {
        public DedicatedEmptySpace(int i, int j):base(i, j) {
            EntityType = 101;
        }
        public override void GameLoopAction() {
        }
    }
}