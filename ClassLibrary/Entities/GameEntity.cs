using System;

namespace ClassLibrary.Entities {
    public class GameEntity {
        public GameEntity(int entityType, int positionX, int positionY) {
            this.entityType = entityType;
            PositionX = positionX;
            PositionY = positionY;
        }
        public GameEntity() { }

        public void GameLoopAction() {
            // it is what action is performed by each class instance of game matrix on every gameLoop()
        }

        protected int entityType;

        public int EntityType {
            get => entityType;
        }

        public int PositionX;
        public int PositionY;
    }
}