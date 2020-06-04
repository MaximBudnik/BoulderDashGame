using System;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
    public class Movable : GameEntity {
        protected readonly Func<Level> GetLevel;
        public int MaxHp { get; set; }

        protected Movable(Func<Level> getLevel, int i, int j) : base(i, j) {
            GetLevel = getLevel;
        }
        public int Hp { get; set; }
        public override void GameLoopAction() { }
        public virtual void Move(MoveDirectionEnum direction, int value) {
            var level = GetLevel();
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY); //making previous position empty
            switch (direction) {
                case MoveDirectionEnum.Horizontal:
                    PositionY += value;
                    if (!IsValid(level)) PositionY -= value;
                    break;
                case MoveDirectionEnum.Vertical:
                    PositionX += value;
                    if (!IsValid(level)) PositionX -= value;
                    break;
                default:
                    throw new Exception("Unknown move direction in Movable.cs");
            }
            level[PositionX, PositionY] = this;
        }

        public override void BreakAction(Player.Player player) { }

        private bool IsValid(Level level) {
            return IsLevelCellValid(PositionX, PositionY, level.Width, level.Height);
        }
    }
}