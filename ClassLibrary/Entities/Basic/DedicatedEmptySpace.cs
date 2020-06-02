namespace ClassLibrary.Entities.Basic {
    public class DedicatedEmptySpace : GameEntity {
        public DedicatedEmptySpace(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.DedicatedEmptySpace;
        }
    }
}