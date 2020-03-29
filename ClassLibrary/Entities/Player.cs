namespace ClassLibrary.Entities {
    public class Player : Movable {
        
        public int positionX;
        public int positionY;
        
        

        public Player(int[] pos) {
            positionX = pos[0];
            positionY = pos[1];
            
        }

        public void movePlayer(string direction, int value) {
            
        }
        
    }
}