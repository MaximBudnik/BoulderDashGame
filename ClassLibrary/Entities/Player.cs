using System;

namespace ClassLibrary.Entities {
    public class Player : Movable {
        
        public Player(int[] pos){
            positionX = pos[0];
            positionY = pos[1];
            entityType = 0;
        }

        public void Move(string direction, int value) {// TODO: can be refactored with delegates
            
            Level level =  GameEngine.gameLogic.CurrentLevel;
            level[positionX, positionY] = 1;
            if (direction == "vertical") {
                positionX += value;
                if (positionX == level.Width || positionX == -1 || level[positionX, positionY]==5 ||level[positionX, positionY]==3)
                    positionX -= value;                          //  level[positionX, positionY]==5 || level[positionX, positionY]==3 means that if there are rocks or walls, player cant move there

                level[positionX, positionY] = entityType;                 
            }
            if (direction == "horizontal") {
                positionY += value;
                if (level[positionX, positionY]==3) {
                    GameEngine.gameLogic.RockProcessor.PushRock(positionX, positionY, "horizontal",value);
                }
                if (positionY == level.Height || positionY == -1 || level[positionX, positionY]==5 || level[positionX, positionY]==3)
                    positionY -= value;
                level[positionX, positionY] = entityType;
            }
            GameEngine.gameLogic.drawLevel();
        }
    }
}