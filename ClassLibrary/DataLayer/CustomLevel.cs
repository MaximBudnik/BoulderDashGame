using System;
using ClassLibrary.Entities;
using ClassLibrary.Matrix;

namespace ClassLibrary.DataLayer {
    public class CustomLevel {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int SizeX { get; set; } = 10;
        public int SizeY { get; set; } = 10;
        public GameModesEnum Aim { get; set; } = GameModesEnum.CollectDiamonds;

        public GameEntitiesEnum[,] Map { get; set; } = null;
    }
}