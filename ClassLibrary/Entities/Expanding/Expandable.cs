using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class Expandable : GameEntity {
        protected Expandable(int i, int j, Func<Level> getLevel) : base(i, j) {
            GetLevel = getLevel;
        }
        protected readonly Func<Level> GetLevel;

        protected Expandable(Func<Level> getLevel) {
            GetLevel = getLevel;
        }
        
        protected Action<int, int> ConstructorForExpand;

        protected void Expand(Func<int, int, bool> getCondition, Action<int, int> classConstructor) {
            var level = GetLevel();
            if (Right < level.Width && getCondition(Right,PositionY)) classConstructor(Right, PositionY);
            if (Left >= 0 && getCondition(Left,PositionY)) classConstructor(Left, PositionY);
            if (Bot < level.Height && getCondition(PositionX,Bot)) classConstructor(PositionX, Bot);
            if (Top >= 0 && getCondition(PositionX,Top)) classConstructor(PositionX, Top);
        }
    }
}