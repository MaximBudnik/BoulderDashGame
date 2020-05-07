using System;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class StoneInDiamondConverter : Expandable {
        private int _actionsCounter = 0;

        public StoneInDiamondConverter(int i, int j, Func<Level> getLevel) : base(i, j, getLevel) {
            EntityType = 10;
           
            CanMove = false;
            ConstructorForExpand = (i, j) => {
                var level = GetLevel();
                var tmp = new StoneInDiamondConverter(i, j, GetLevel);
                level[i, j] = tmp;
            };
        }

        
        public override void GameLoopAction() {
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
        }
        
    }
}