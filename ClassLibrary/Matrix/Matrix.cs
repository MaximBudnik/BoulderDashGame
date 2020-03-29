namespace ClassLibrary {
    public class Matrix {
        protected  int[,] matrix;

        protected int width;

        public int Width {
            get => this.width;
        }

        protected int height;

        public int Height {
            get => this.height;
        }
        
        
        public int this[int x, int y] {
            get => matrix[x, y];
            set => matrix[x, y] = value;
        }
    }
}