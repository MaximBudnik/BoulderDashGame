using System;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Collectable {
    public class BarrelWithSubstance : ItemCollectible {
        public BarrelWithSubstance(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.Barrel;
        }

        private bool WillReplace => 33 >= Randomizer.Random(100);

        public void Collect(Func<Level> getLevel, Action<int> changePlayerHp) {
            var level = getLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (IsLevelCellValid(posX, posY, level.Width, level.Height) && WillReplace)
                        level[posX, posY] = new Acid(posX, posY, getLevel, changePlayerHp);
                }
        }
    }
}