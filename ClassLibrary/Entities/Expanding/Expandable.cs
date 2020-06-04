using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class Expandable : GameEntity {
        protected readonly Func<Level> GetLevel;

        protected Action<int, int> ConstructorForExpand;
        protected Expandable(int i, int j, Func<Level> getLevel) : base(i, j) {
            GetLevel = getLevel;
        }
        public override void GameLoopAction() { }
        protected void Expand(Func<int, int, bool> getCondition, Action<int, int> classConstructor) {
            var level = GetLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (IsLevelCellValid(posX, posY, level.Width, level.Height) && getCondition(posX, posY))
                        classConstructor(posX, posY);
                }
        }
    }
}