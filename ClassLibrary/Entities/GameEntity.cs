namespace ClassLibrary.Entities {
    public abstract class GameEntity {
        public static readonly int FormsSize = 14;
        public bool CanMove = true;
        public bool PathFinderMove = false;
        public int CurrentFrame = 0;
        public int IdleFrames = 4;
        public int PositionX;
        public int PositionY;
        protected GameEntity(int i, int j) {
            PositionX = i;
            PositionY = j;
        }
        public GameEntities EntityType { get; protected set; }
        protected int RightX => PositionX + 1;
        protected int LeftX => PositionX - 1;
        protected int BotY => PositionY + 1;
        protected int TopY => PositionY - 1;

        public virtual void GameLoopAction() { }
    }
}