using System;
using ClassLibrary.Entities;

namespace ClassLibrary.DataLayer {
    public class CustomLevel {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public GameEntitiesEnum[,] Map { get; set; } = null;
    }
}