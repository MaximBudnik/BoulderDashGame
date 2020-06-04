namespace ClassLibrary.Entities.Player {
    public class PlayerAnimator {
        private readonly int attackFrames = 3;
        private readonly int converterFrames = 3;
        private readonly int damageFrames = 2;
        private readonly int explosionFrames = 7;
        private readonly int IdleFrames = 4;
        private readonly int moveFrames = 4;
        private readonly int teleportFrames = 4;
        public PlayerAnimationsEnum CurrentPlayerAnimationEnum;
        public int FramesLimit;
        public int Reverse;
        public PlayerAnimator(int reverse) {
            FramesLimit = IdleFrames;
            Reverse = reverse;
        }

        public void SetAnimation(PlayerAnimationsEnum currentPlayerAnimationEnum) {
            CurrentPlayerAnimationEnum = currentPlayerAnimationEnum;
            switch (currentPlayerAnimationEnum) {
                case PlayerAnimationsEnum.Idle:
                    FramesLimit = IdleFrames;
                    break;
                case PlayerAnimationsEnum.Move:
                    FramesLimit = moveFrames;
                    break;
                case PlayerAnimationsEnum.GetDamage:
                    FramesLimit = damageFrames;
                    break;
                case PlayerAnimationsEnum.Attack:
                    FramesLimit = attackFrames;
                    break;
                case PlayerAnimationsEnum.Explosion:
                    FramesLimit = explosionFrames;
                    break;
                case PlayerAnimationsEnum.Teleport:
                    FramesLimit = teleportFrames;
                    break;
                case PlayerAnimationsEnum.Converting:
                    FramesLimit = converterFrames;
                    break;
            }
        }
    }
}