using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class StoneInDiamondConverter : Expandable {
        private readonly Func<List<StoneInDiamondConverter>> _getStoneInDiamondsConvertersList;
        private int _actionsCounter = 0;

        public StoneInDiamondConverter(int i, int j, Func<Level> getLevel,
            Func<List<StoneInDiamondConverter>> getStoneInDiamondsConvertersList) : base(i, j, getLevel) {
            EntityType = 10;
            _getStoneInDiamondsConvertersList = getStoneInDiamondsConvertersList;
            CanMove = false;
            ConstructorForExpand = ( i,  j) => { 
                var level = GetLevel();
                var tmp = new StoneInDiamondConverter(i, j, GetLevel, _getStoneInDiamondsConvertersList);
                level[i, j] = tmp;
                _getStoneInDiamondsConvertersList().Add(tmp);};
        }

        
        public new void GameLoopAction() {
            if (_actionsCounter == 1) {
                Expand(( i, j)=>GetLevel()[i, j].EntityType == 3, ConstructorForExpand);
            }
            if (_actionsCounter >= 2) {
                TurnIntoDiamond();
                _actionsCounter = 0;
            }
            _actionsCounter++;
        }
        private void TurnIntoDiamond() {
            var level = GetLevel();
            level[PositionX, PositionY] = new Diamond(PositionX, PositionY);
            _getStoneInDiamondsConvertersList().Remove(this);
        }
        
    }
}