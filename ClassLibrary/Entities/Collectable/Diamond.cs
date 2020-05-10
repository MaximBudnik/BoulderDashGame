using System;

namespace ClassLibrary.Entities.Collectable {
    public class Diamond : ItemCollectible {
        public Diamond(int i, int j) : base(i, j) {
            EntityType = 4;
            idleFrames = 512;
            currentFrame = Randomizer.Random(0, 64);
        }

        public new void Collect(Func<Player.Player> getPlayer) {
            var value = PickUpValue;
            var player = getPlayer();
            player.CollectedDiamonds += value;
            player.AllScores["Collected diamonds"][0] += 1;
            player.AllScores["Collected diamonds"][1] += player.GetScoreToAdd(value);
            player.AddScore(value);
        }
    }
}