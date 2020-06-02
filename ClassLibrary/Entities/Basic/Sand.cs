namespace ClassLibrary.Entities.Basic {
    public class Sand : GameEntity {
        public Sand(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.Sand;
        }
    }
}