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
            if (Right < level.Width && 33 >= Randomizer.Random(100)) {
                level[Right, PositionY]  = new Acid(Right, PositionY, getLevel, changePlayerHp);
            }
            if (Left >= 0 && 33 >= Randomizer.Random(100)) {
                level[Left, PositionY] = new Acid(Left, PositionY, getLevel,  changePlayerHp);
            }
            if (Bot < level.Height && 33 >= Randomizer.Random(100)) {
                level[PositionX, Bot] = new Acid(PositionX, Bot, getLevel,  changePlayerHp);
            }
            if (Top >= 0 && 33 >= Randomizer.Random(100)) {
                level[PositionX, Top]  = new Acid(PositionX, Top, getLevel,  changePlayerHp);
            }
        }
    }
}