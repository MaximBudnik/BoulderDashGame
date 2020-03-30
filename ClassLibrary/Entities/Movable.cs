namespace ClassLibrary.Entities {
    public class Movable : GameEntity {
        
        

        
        public Movable(int[] pos) {
            positionX = pos[0];
            positionY = pos[1];
        }
        public Movable() {
            
        }
        
        public void Move(string direction, int value, int posX, int posY) {// TODO: can be refactored with delegates

            positionX = posX;
            positionY = posY;
            
            Level level =  GameEngine.gameLogic.CurrentLevel;
            level[positionX, positionY] = new EmptySpace(positionX,positionY); //making previous position empty
            if (direction == "vertical") {
                positionX += value;
                if (positionX == level.Width || positionX == -1)
                    positionX -= value;   
                level[positionX, positionY] = this;
                       //  level[positionX, positionY]==5 || level[positionX, positionY]==3
            }
            if (direction == "horizontal") {
                positionY += value;
                if (positionY == level.Height || positionY == -1)
                    positionY -= value;
                level[positionX, positionY] = this;
            }
            GameEngine.gameLogic.drawLevel();
        }
        
        
        
    }
}