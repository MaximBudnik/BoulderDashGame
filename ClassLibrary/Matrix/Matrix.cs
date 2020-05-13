using ClassLibrary.Entities;

namespace ClassLibrary.Matrix {
    public class Matrix {
        protected int height;

        protected GameEntity[,] matrix;
        protected int width;
        public Matrix(int width, int height) {
            matrix = new GameEntity[width, height];
        }
        protected Matrix() { }

        public int Width => width;

        public int Height => height;

        public GameEntity this[int x, int y] {
            get => matrix[x, y];
            set => matrix[x, y] = value;
        }
    }
}