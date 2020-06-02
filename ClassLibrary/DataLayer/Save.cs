using System;

namespace ClassLibrary.DataLayer {
    public class Save {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int LevelName { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int Hero { get; set; } = 1;
    }
}