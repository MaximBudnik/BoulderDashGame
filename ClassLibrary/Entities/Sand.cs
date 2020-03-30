namespace ClassLibrary.Entities {
    public class Sand : GameEntity {
        public Sand(int i,int j){
            this.entityType = 2;
            this.positionX = i;
            this.positionY = j;
        }
    }
}