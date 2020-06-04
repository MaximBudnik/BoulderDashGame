using System;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Collectable {
    public class BarrelWithSubstance : ItemCollectible {
        public BarrelWithSubstance(
            int i, int j, 
            Action acidGameLoopAction,
            Func<Level> getLevel,
            Action<int> changePlayerHp
            ) : base(i, j) {
            _acidGameLoopAction = acidGameLoopAction;
            EntityEnumType = GameEntitiesEnum.Barrel;
            _changePlayerHp = changePlayerHp;
            _getLevel = getLevel;
        }
        
        private readonly Action<int> _changePlayerHp;
        private readonly Func<Level> _getLevel;

        
        private readonly Action _acidGameLoopAction;
        private bool WillReplace => 33 >= Randomizer.Random(100);

        public override void BreakAction(Player.Player player) {
            var level = _getLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (IsLevelCellValid(posX, posY, level.Width, level.Height) && WillReplace)
                        level[posX, posY] = new Acid(posX, posY, _getLevel, _changePlayerHp, _acidGameLoopAction);
                }
        }
    }
}