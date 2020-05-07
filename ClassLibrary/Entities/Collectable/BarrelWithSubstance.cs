using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Collectable {
    public class BarrelWithSubstance : ItemCollectible {


        public BarrelWithSubstance(int i, int j) : base(i, j) {
            EntityType = 12;
        }

        public override void GameLoopAction() {
        }
        
        public void Collect(Func<Level> getLevel, Action<int> changePlayerHp) {
            var level = getLevel();
            if (Right < level.Width) {
                level[Right, PositionY]  = new Acid(Right, PositionY, getLevel, changePlayerHp);
            }
            if (Left >= 0 ) {
                level[Left, PositionY] = new Acid(Left, PositionY, getLevel,  changePlayerHp);
            }
            if (Bot < level.Height ) {
                level[PositionX, Bot] = new Acid(PositionX, Bot, getLevel,  changePlayerHp);
            }
            if (Top >= 0 ) {
                level[PositionX, Top]  = new Acid(PositionX, Top, getLevel,  changePlayerHp);
            }
        }
    }
}