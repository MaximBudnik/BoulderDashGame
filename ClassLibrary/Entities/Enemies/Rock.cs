using System;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Rock : Enemy {
        public Rock(int i, int j) {
            entityType = 3;
            PositionX = i;
            PositionY = j;
        }
    }
}