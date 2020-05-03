using System;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
    public class Movable : GameEntity {
        protected Movable(Func<Level> getLevel, int i, int j ) : base(i, j) {
            GetLevel = getLevel;
        }

        protected Movable(Func<Level> getLevel) {
            GetLevel = getLevel;
        }

        protected readonly Func<Level> GetLevel;

        protected void Move(string direction, int value, int posX, int posY) {
            PositionX = posX;
            PositionY = posY;

            Level level = GetLevel();
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY); //making previous position empty

            switch (direction) {
                case "vertical":
                    PositionX += value;
                    if (PositionX == level.Width || PositionX == -1) PositionX -= value;
                    break;
                case "horizontal":
                    PositionY += value;
                    if (PositionY == level.Height || PositionY == -1) PositionY -= value;
                    break;
                default:
                    throw new Exception("Unknown move direction in Movable.cs");
            }
            level[PositionX, PositionY] = this;
        }
    }
}