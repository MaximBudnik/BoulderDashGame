using ClassLibrary.Entities;
using ClassLibrary.Matrix;

namespace ClassLibrary.DataLayer {
    public class CustomLevel {
        // public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int SizeY { get; set; } = 20;
        public int SizeX { get; set; } = 20;
        public GameModesEnum Aim { get; set; } = GameModesEnum.CollectDiamonds;
        public GameEntitiesEnum[,] Map { get; set; } = null;
    }
}