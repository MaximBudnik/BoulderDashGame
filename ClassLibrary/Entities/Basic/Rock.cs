using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Rock : GameEntity {
        public Rock(int i, int j) :base(i, j){
            EntityType = 3;
        }
    }
}