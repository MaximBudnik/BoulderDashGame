using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
    public class Movable : GameEntity {
        public Movable(int[] pos) {
            PositionX = pos[0];
            PositionY = pos[1];
        }
        protected Movable() { }

        protected void Move(string direction, int value, int posX, int posY) {
            PositionX = posX;
            PositionY = posY;

            Level level = GameEngine.GameLogic.CurrentLevel;
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY); //making previous position empty
            if (direction == "vertical") {
                PositionX += value;
                if (PositionX == level.Width || PositionX == -1)
                    PositionX -= value;
                level[PositionX, PositionY] = this;
            }
            if (direction == "horizontal") {
                PositionY += value;
                if (PositionY == level.Height || PositionY == -1)
                    PositionY -= value;
                level[PositionX, PositionY] = this;
            }
        }
    }
}