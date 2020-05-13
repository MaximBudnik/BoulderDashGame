namespace ClassLibrary.Entities.Player {
    public class PlayerAnimator {
        public readonly int attackFrames = 3;
        public readonly int converterFrames = 3;
        public readonly int damageFrames = 2;
        public readonly int explosionFrames = 7;
        public readonly int IdleFrames = 4;
        public readonly int moveFrames = 4;
        public readonly int teleportFrames = 4;
        public int CurrentAnimation;
        public int FramesLimit;
        public int Reverse;
        public PlayerAnimator(int reverse) {
            FramesLimit = IdleFrames;
            Reverse = reverse;
        }

        public void SetAnimation(int currentAnimation) {
            CurrentAnimation = currentAnimation;
            switch (currentAnimation) {
                case 0:
                    FramesLimit = IdleFrames;
                    break;
                case 1:
                    FramesLimit = moveFrames;
                    break;
                case 2:
                    FramesLimit = damageFrames;
                    break;
                case 3:
                    FramesLimit = attackFrames;
                    break;
                case 4:
                    FramesLimit = explosionFrames;
                    break;
                case 5:
                    FramesLimit = teleportFrames;
                    break;
                case 6:
                    FramesLimit = converterFrames;
                    break;
            }
        }
    }
}