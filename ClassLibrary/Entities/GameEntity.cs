using System;

namespace ClassLibrary.Entities {
     public abstract class GameEntity {
        
        public int EntityType { get; protected set; }
        public int PositionX;
        public int PositionY;

        public GameEntity(int i,int j) {
            PositionX = i;
            PositionY = j;
        }
        
        public GameEntity() {
        }
       
        public void GameLoopAction() {
            // it is what action is performed by each class instance of game matrix on every gameLoop()
        }


    }
}