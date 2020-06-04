using System;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Collectable {
    public class Diamond : ItemCollectible {
        public Diamond(int i, int j, Action<SoundFilesEnum> playSound) : base(i, j, playSound) {
            EntityEnumType = GameEntitiesEnum.Diamond;
            IdleFrames = 512;
            CurrentFrame = Randomizer.Random(0, 64);
        }

        protected override void Collect(Player.Player player) {
            var value = PickUpValue;
            player.CollectedDiamonds += value;
            player.AllScores["Collected diamonds"][0] += 1;
            player.AllScores["Collected diamonds"][1] += player.GetScoreToAdd(value);
            player.AddScore(value);
        }
    }
}