using System;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
    public class Movable : GameEntity {
        protected readonly Func<Level> GetLevel;
        protected Movable(Func<Level> getLevel, int i, int j) : base(i, j) {
            GetLevel = getLevel;
        }
        public int Hp { get; set; }
        public new void GameLoopAction() { }
        protected virtual void Move(string direction, int value, int posX, int posY) {
            PositionX = posX;
            PositionY = posY;
            var level = GetLevel();
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY); //making previous position empty
            var isValid = !IsLevelCellValid(PositionX, PositionY, level.Width, level.Height);
            switch (direction) {
                case "horizontal":
                    PositionX += value;
                    if (isValid) PositionX -= value;
                    break;
                case "vertical":
                    PositionY += value;
                    if (isValid) PositionY -= value;
                    break;
                default:
                    throw new Exception("Unknown move direction in Movable.cs");
            }
            level[PositionX, PositionY] = this;
        }
    }
}