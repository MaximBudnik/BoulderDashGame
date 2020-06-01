using System;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Collectable {
    public class BarrelWithSubstance : ItemCollectible {
        public BarrelWithSubstance(int i, int j) : base(i, j) {
            EntityType = GameEntities.Barrel;
        }

        private bool WillReplace => 33 >= Randomizer.Random(100);
        
        public void Collect(Func<Level> getLevel, Action<int> changePlayerHp) {
            var level = getLevel();
            if (RightX < level.Width && WillReplace)
                level[RightX, PositionY] = new Acid(RightX, PositionY, getLevel, changePlayerHp);
            if (LeftX >= 0 && WillReplace)
                level[LeftX, PositionY] = new Acid(LeftX, PositionY, getLevel, changePlayerHp);
            if (BotY < level.Height && WillReplace)
                level[PositionX, BotY] = new Acid(PositionX, BotY, getLevel, changePlayerHp);
            if (TopY >= 0 && WillReplace)
                level[PositionX, TopY] = new Acid(PositionX, TopY, getLevel, changePlayerHp);
        }
    }
}