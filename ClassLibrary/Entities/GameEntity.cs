using System;

namespace ClassLibrary.Entities {
     public abstract class GameEntity {
        
        public int EntityType { get; protected set; }
        public int PositionX;
        public int PositionY;
        public bool CanMove = true;
        protected readonly int Right;
        protected readonly int Left;
        protected readonly int Bot;
        protected readonly int Top;

        protected GameEntity(int i,int j) {
            PositionX = i;
            PositionY = j;
            Right = PositionX + 1;
            Left = PositionX - 1;
            Bot = PositionY + 1;
            Top = PositionY - 1;
        }

        protected GameEntity() {
        }
       
        public virtual void  GameLoopAction() {
            // it is what action is performed by each class instance of game matrix on every gameLoop()
        }


    }
}