using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class StoneInDiamondConverter : Expandable {
        private readonly Func<Level> _getLevel;
        private readonly Func<List<StoneInDiamondConverter>> _stoneInDiamondsConvertersList;
        private int _actionsCounter = 0;

        public StoneInDiamondConverter(int i, int j, Func<Level> getLevel,
            Func<List<StoneInDiamondConverter>> stoneInDiamondsConvertersList) : base(i, j) {
            EntityType = 10;
            _getLevel = getLevel;
            _stoneInDiamondsConvertersList = stoneInDiamondsConvertersList;
            CanMove = false;
        }

        public new void GameLoopAction() {
            if (_actionsCounter == 5) {
                SearchForStones();
            }
            if (_actionsCounter >= 10) {
                TurnIntoDiamond();
                _actionsCounter = 0;
            }
            _actionsCounter++;
        }
        private void TurnIntoDiamond() {
            var level = _getLevel();
            level[PositionX, PositionY] = new Diamond(PositionX, PositionY);
            _stoneInDiamondsConvertersList().Remove(this);
        }

        private void SearchForStones() {
            var level = _getLevel();
            if (PositionX + 1 < level.Width && level[PositionX + 1, PositionY].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX + 1, PositionY, _getLevel,
                    _stoneInDiamondsConvertersList);
                level[PositionX + 1, PositionY] = tmp;
                _stoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionX - 1 >= 0 && level[PositionX - 1, PositionY].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX - 1, PositionY, _getLevel,
                    _stoneInDiamondsConvertersList);
                level[PositionX - 1, PositionY] = tmp;
                _stoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionY + 1 < level.Height && level[PositionX, PositionY + 1].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX, PositionY + 1, _getLevel,
                    _stoneInDiamondsConvertersList);
                level[PositionX, PositionY + 1] = tmp;
                _stoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionY - 1 >= 0 && level[PositionX, PositionY - 1].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX, PositionY - 1, _getLevel,
                    _stoneInDiamondsConvertersList);
                level[PositionX, PositionY - 1] = tmp;
                _stoneInDiamondsConvertersList().Add(tmp);
            }
        }
    }
}