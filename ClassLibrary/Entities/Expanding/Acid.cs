using System;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class Acid : Expandable {
        private readonly Action<int> _changePlayerHp;
        //must be refactored because it inherits logic of enemies and expandable

        private int _actionsCounter;

        public Acid(int i, int j, Func<Level> getLevel, Action<int> changePlayerHp) : base(i, j, getLevel) {
            EntityType = GameEntities.Acid;
            _changePlayerHp = changePlayerHp;
            CanMove = false;
            ConstructorForExpand = (i, j) => {
                var level = GetLevel();
                var tmp = new Acid(i, j, GetLevel, _changePlayerHp);
                level[i, j] = tmp;
            };
        }

        public override void GameLoopAction() {
            var level = GetLevel();
            if (_actionsCounter == 3) Expand((i, j) => level[i, j].CanMove, ConstructorForExpand);
            if (_actionsCounter >= 4) {
                TurnIntoRock();
                _actionsCounter = 0;
            }
            _actionsCounter++;
        }

        public void TurnIntoRock() {
            var level = GetLevel();
            level[PositionX, PositionY] = new Rock(PositionX, PositionY, GetLevel, _changePlayerHp);
        }
    }
}