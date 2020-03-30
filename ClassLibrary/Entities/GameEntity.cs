namespace ClassLibrary.Entities {
    public class GameEntity {

        public GameEntity(int entityType) {
            this.entityType = entityType;
        }
        public GameEntity() {
            
        }
        
        
        protected int entityType;
        public int EntityType {
            get => entityType;
        }

        protected int positionX;
        protected int positionY;

    }
}