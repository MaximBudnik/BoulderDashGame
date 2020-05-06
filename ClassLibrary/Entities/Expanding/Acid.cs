using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Expanding {
    public class Acid : Expandable { //must be refactored because it inherits logic of enemies and expandable
        
        private Func<List<Acid>> _getAcidBlocksList;
        private int _actionsCounter = 0;
        private readonly Action<int> _changePlayerHp;
        private readonly int _damage;

        public Acid(int i, int j, Func<Level> getLevel,
            Func<List<Acid>> getAcidBlocksList, Action<int> changePlayerHp) : base(i, j, getLevel) {
            EntityType = 11;
            _getAcidBlocksList = getAcidBlocksList;
            _changePlayerHp = changePlayerHp;
            _damage = 2;
            ConstructorForExpand = ( i,  j) => { 
                var level = GetLevel();
                var tmp = new Acid(i, j, GetLevel, _getAcidBlocksList, _changePlayerHp);
                level[i, j] = tmp;
                _getAcidBlocksList().Add(tmp);};
        }

        public new void GameLoopAction() {
            Level level = GetLevel();
            if (level[PositionX, PositionY].EntityType == 0) DealDamage();
            if (_actionsCounter == 3) {
                Expand(( i, j)=>level[i, j].CanMove, ConstructorForExpand);
            }
            if (_actionsCounter >= 4) {
                TurnIntoRock();
                _actionsCounter = 0;
            }
            _actionsCounter++;
        }
        private void DealDamage() {
            _changePlayerHp(_damage);
        }
        
        private void TurnIntoRock() {
            var level = GetLevel();
            level[PositionX, PositionY] = new Rock(PositionX, PositionY);
            _getAcidBlocksList().Remove(this);
        }
    }
}