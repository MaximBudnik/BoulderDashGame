namespace ClassLibrary.Entities {
    public class Player : Movable {
        
        public int positionX;
        public int positionY;
        
        

        public Player(int[] pos) {
            positionX = pos[0];
            positionY = pos[1];
            
        }

        public void Move(string direction, int value) {// TODO: can be refactored with delegates
            
            Level level =  GameEngine.gameLogic.CurrentLevel;
            level[positionX, positionY] = 1;
            if (direction == "vertical") {
                positionX += value;
                if (positionX == level.Width || positionX == -1 || level[positionX, positionY]==5 || level[positionX, positionY]==3)
                    positionX -= value;                          //  level[positionX, positionY]==5 || level[positionX, positionY]==3
                level[positionX, positionY] = 0;                 // means that if there are rocks or walls, player cant move there
            }
            if (direction == "horizontal") {
                positionY += value;
                if (positionY == level.Height || positionY == -1 || level[positionX, positionY]==5 || level[positionX, positionY]==3)
                    positionY -= value;
                level[positionX, positionY] = 0;
            }
            GameEngine.gameLogic.drawLevel();
        }
    }
}