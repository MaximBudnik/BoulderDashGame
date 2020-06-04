namespace ClassLibrary.Entities {
    public abstract class GameEntity {
        public static readonly int FormsSize = 14;
        public bool CanMove = true;
        public int CurrentFrame = 0;
        public int IdleFrames = 4;
        public bool PathFinderMove = false;
        public int PositionX;
        public int PositionY;
        protected GameEntity(int i, int j) {
            PositionX = i;
            PositionY = j;
        }
        public GameEntitiesEnum EntityEnumType { get; protected set; }
        protected int RightX => PositionX + 1;
        protected int LeftX => PositionX - 1;
        protected int BotY => PositionY + 1;
        protected int TopY => PositionY - 1;

        public virtual void GameLoopAction() { }

        public virtual void BreakAction(Player.Player player) {
            
        }
        
        public virtual void BreakAction() {
            
        }
        
        public static bool IsLevelCellValid(int x, int y, int width, int height) {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}