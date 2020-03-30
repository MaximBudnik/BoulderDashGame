using System;

namespace ClassLibrary.Entities {
    public class GameEntity {

        public GameEntity(int entityType, int positionX, int positionY) {
            this.entityType = entityType;
            this.positionX = positionX;
            this.positionY = positionY;
        }
        public GameEntity() {
            
        }

        public void GameLoopAction() {// it is what action is performed by each class instance of game matrix on every gameLoop()

        }
        
        
        protected int entityType;
        public int EntityType {
            get => entityType;
        }

        protected int positionX;
        protected int positionY;

    }
}