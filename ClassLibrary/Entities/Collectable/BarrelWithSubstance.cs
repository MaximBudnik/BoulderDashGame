using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Collectable {
    public class BarrelWithSubstance : ItemCollectible {


        public BarrelWithSubstance(int i, int j) : base(i, j) {
            EntityType = 12;
        }

        public static void Collect(int i, int j, Func<Level> getLevel, Func<List<Acid>> getAcidBlocksList,
            Action<int> changePlayerHp) {
            var tmp = new Acid(i, j, getLevel, getAcidBlocksList, changePlayerHp);
            getLevel()[i, j] = tmp;
            getAcidBlocksList().Add(tmp);
        }
    }
}