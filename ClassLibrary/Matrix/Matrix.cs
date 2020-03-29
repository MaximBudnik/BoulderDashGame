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

        public Matrix(int width, int height ) {
            matrix = new int[width, height];
        }
        
        public Matrix(string name) {
            //does nothing, just for Level constructor existence
        }
        public Matrix() {
            //the same
        }
        
        public int this[int x, int y] {
            get => matrix[x, y];
            set => matrix[x, y] = value;
        }
    }
}