using System;

namespace ClassLibrary.Entities.Collectable {
    public class Diamond : ItemCollectible {
        public Diamond(int i, int j) : base(i, j) {
            EntityEnumType = GameEntitiesEnum.Diamond;
            IdleFrames = 512;
            CurrentFrame = Randomizer.Random(0, 64);
        }

        public void Collect(Func<Player.Player> getPlayer) {
            var value = PickUpValue;
            var player = getPlayer();
            player.CollectedDiamonds += value;
            player.AllScores["Collected diamonds"][0] += 1;
            player.AllScores["Collected diamonds"][1] += player.GetScoreToAdd(value);
            player.AddScore(value);
        }
    }
}