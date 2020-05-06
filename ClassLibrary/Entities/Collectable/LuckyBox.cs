using System;
using System.Collections.Generic;

namespace ClassLibrary.Entities.Collectable {
    public class LuckyBox : ItemCollectible {
        public LuckyBox(int i, int j):base(i, j) {
            EntityType = 7;
        }

        public new static int PickUpValue = 3;
        
        public static void Collect( Func<Player.Player> getPlayer) { //TODO: write text in xaml on pickup (impossible in console)
            Player.Player player = getPlayer();
            List<int> pool = new List<int>{
                1,1,1,1,1,
                2,2,2,2,2,
                3,3,3,3,
                4,4,
                5,
                6,6,
                7,
            };
            int effect = Randomizer.GetRandomFromList(pool);
            int tmp;
            switch (effect) {
                case 1:
                    tmp = Randomizer.Random(10);
                    player.CollectedDiamonds += tmp;
                    player.Score += tmp*player.ScoreMultiplier;
                    player.AllScores["Diamonds from lucky box"][0] += 1;
                    player.AllScores["Diamonds from lucky box"][1] += tmp*player.ScoreMultiplier;
                    break;
                case 2:
                    player.Hp = player.MaxHp;
                    break;
                case 3:
                    player.SubstractPlayerHp(Randomizer.Random(3));
                    break;
                case 4:
                    player.MaxHp = 15;
                    break;
                case 5:
                    player.EnergyRestoreTick *= 2;
                    break;
                case 6:
                    tmp = Randomizer.Random(10,30);
                    player.Score += tmp*player.ScoreMultiplier;
                    player.AllScores["Score from lucky box"][0] += 1;
                    player.AllScores["Score from lucky box"][1] += tmp*player.ScoreMultiplier;
                    break;
                case 7:
                    player.ScoreMultiplier *=Randomizer.Random(2,5);
                    break;
                default:
                    throw new Exception("Out of range in LuckyBox");
            }
        }
    }
}