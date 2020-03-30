using System;

namespace ClassLibrary.Entities {
    public class Player : Movable {
        
        public Player(int[] pos){
            positionX = pos[0];
            positionY = pos[1];
            entityType = 0;
        }

        public int MaxHp { get; }= 10;
        public int Hp = 8;
        public int MaxEnergy{ get; } = 20;
        public int Energy = 20;
        public int CollectedDiamonds = 0;

        public void Move(string direction, int value) {// TODO: refactor me! i duplicate movable
            
            Level level =  GameEngine.gameLogic.CurrentLevel;
            level[positionX, positionY] = new EmptySpace(positionX,positionY);
            if (direction == "vertical") {
                positionX += value;
                if (positionX == level.Width || positionX == -1 || level[positionX, positionY].EntityType==5 ||level[positionX, positionY].EntityType==3)
                    positionX -= value;                          //  level[positionX, positionY]==5 || level[positionX, positionY]==3 means that if there are rocks or walls, player cant move there
                //checking on diamonds
                if (level[positionX, positionY].EntityType==4) {
                    collectDiamond(1);
                }
                Energy--;
                level[positionX, positionY] = this;//May cause bugs! check it out                 
            }
            if (direction == "horizontal") {
                positionY += value;
                //pushing rock
                if (level[positionX, positionY].EntityType==3) {
                    GameEngine.gameLogic.RockProcessor.PushRock(positionX, positionY, "horizontal",value);
                }
                if (positionY == level.Height || positionY == -1 || level[positionX, positionY].EntityType==5 || level[positionX, positionY].EntityType==3)
                    positionY -= value;
                //checking on diamonds
                
                Energy--;
                level[positionX, positionY] = this;
            }
            GameEngine.gameLogic.drawLevel();
        }

        public void collectDiamond(int value) {
            CollectedDiamonds+=value;
        }
    }
}