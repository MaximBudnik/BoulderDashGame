using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable {
    public class GoldenFish : ItemCollectible {
        public GoldenFish(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            IdleFrames = 32;
            CurrentFrame = Randomizer.Random(0, 16);
            EntityEnumType = GameEntitiesEnum.GoldenFish;
            MoveWeight = 100000;
        }

        public override void BreakAction(Player.Player player) {
            _playSound(SoundFilesEnum.PickupSound);
            player.IsGoldenFishCollected = true;
        }
    }
}