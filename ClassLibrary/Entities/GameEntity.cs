using System;

namespace ClassLibrary.Entities {
     public abstract class GameEntity {

         //win forms
         public int idleFrames = 4;
         public static int formsSize = 14;
         public int currentFrame=0;
         
        public int EntityType { get; protected set; }
        public int PositionX;
        public int PositionY;
        public bool CanMove = true;

        protected int Right => PositionX + 1;
        protected int Left => PositionX - 1;
        protected int Bot => PositionY + 1;
        protected int Top => PositionY - 1;

        protected GameEntity(int i,int j) {
            PositionX = i;
            PositionY = j;
        }

        protected GameEntity() {
        }

        public virtual void GameLoopAction() {
            
        }
        
        


    }
}