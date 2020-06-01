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
            if (RightX < level.Width && getCondition(RightX, PositionY)) classConstructor(RightX, PositionY);
            if (LeftX >= 0 && getCondition(LeftX, PositionY)) classConstructor(LeftX, PositionY);
            if (BotY < level.Height && getCondition(PositionX, BotY)) classConstructor(PositionX, BotY);
            if (TopY >= 0 && getCondition(PositionX, TopY)) classConstructor(PositionX, TopY);
        }
    }
}