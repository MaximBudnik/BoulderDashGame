﻿namespace ClassLibrary.Entities {
    public class EmptySpace : GameEntity {
        public EmptySpace(int i, int j) {
            entityType = 1;
            PositionX = i;
            PositionY = j;
        }
    }
}