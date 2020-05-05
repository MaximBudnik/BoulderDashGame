namespace ClassLibrary.Entities.Basic {
    public class Rock : GameEntity {
        public Rock(int i, int j) :base(i, j){
            EntityType = 3;
            CanMove = false;
        }
    }
}