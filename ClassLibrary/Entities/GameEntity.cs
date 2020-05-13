namespace ClassLibrary.Entities {
    public abstract class GameEntity {
        public static readonly int FormsSize = 14;
        public bool CanMove = true;

        public int CurrentFrame = 0;

        //win forms
        public int IdleFrames = 4;
        public int PositionX;
        public int PositionY;
        protected GameEntity(int i, int j) {
            PositionX = i;
            PositionY = j;
        }
        protected GameEntity() { }
        public int EntityType { get; protected set; }
        protected int Right => PositionX + 1;
        protected int Left => PositionX - 1;
        protected int Bot => PositionY + 1;
        protected int Top => PositionY - 1;

        public virtual void GameLoopAction() { }
    }
}