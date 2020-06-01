namespace ClassLibrary.Entities.Player {
    public partial class PlayerAnimator {
        private readonly int attackFrames = 3;
        private readonly int converterFrames = 3;
        private readonly int damageFrames = 2;
        private readonly int explosionFrames = 7;
        private readonly int IdleFrames = 4;
        private readonly int moveFrames = 4;
        private readonly int teleportFrames = 4;
        public PlayerAnimations CurrentPlayerAnimation;
        public int FramesLimit;
        public int Reverse;
        public PlayerAnimator(int reverse) {
            FramesLimit = IdleFrames;
            Reverse = reverse;
        }

        public void SetAnimation(PlayerAnimations currentPlayerAnimation) {
            CurrentPlayerAnimation = currentPlayerAnimation;
            switch (currentPlayerAnimation) {
                case PlayerAnimations.Idle:
                    FramesLimit = IdleFrames;
                    break;
                case PlayerAnimations.Move:
                    FramesLimit = moveFrames;
                    break;
                case PlayerAnimations.GetDamage:
                    FramesLimit = damageFrames;
                    break;
                case PlayerAnimations.Attack:
                    FramesLimit = attackFrames;
                    break;
                case PlayerAnimations.Explosion:
                    FramesLimit = explosionFrames;
                    break;
                case PlayerAnimations.Teleport:
                    FramesLimit = teleportFrames;
                    break;
                case PlayerAnimations.Converting:
                    FramesLimit = converterFrames;
                    break;
            }
        }
    }

    
}