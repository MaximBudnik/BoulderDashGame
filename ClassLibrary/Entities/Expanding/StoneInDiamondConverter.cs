using System;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Expanding {
    public class StoneInDiamondConverter : Expandable {
        private int _actionsCounter;
        private readonly Action<SoundFilesEnum> _playSound;

        public StoneInDiamondConverter(int i, int j, Func<Level> getLevel, Action<SoundFilesEnum> playSound) : base(i,
            j, getLevel) {
            _playSound = playSound;
            EntityEnumType = GameEntitiesEnum.Converter;
            CanMove = false;
            ConstructorForExpand = (i, j) => {
                var level = GetLevel();
                var tmp = new StoneInDiamondConverter(i, j, GetLevel, _playSound);
                level[i, j] = tmp;
            };
        }

        public override void GameLoopAction() {
            if (_actionsCounter == 1)
                Expand((i, j) => GetLevel()[i, j].EntityEnumType == GameEntitiesEnum.Rock, ConstructorForExpand);
            if (_actionsCounter >= 2) {
                TurnIntoDiamond();
                _actionsCounter = 0;
            }
            _actionsCounter++;
        }
        private void TurnIntoDiamond() {
            var level = GetLevel();
            level[PositionX, PositionY] = new Diamond(PositionX, PositionY, _playSound);
        }
    }
}