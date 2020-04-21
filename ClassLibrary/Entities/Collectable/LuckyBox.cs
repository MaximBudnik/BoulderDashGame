using System;
using System.Collections.Generic;

namespace ClassLibrary.Entities.Collectable {
    public class LuckyBox : ItemCollectable {
        public LuckyBox(int i, int j) {
            entityType = 7;
            PositionX = i;
            PositionY = j;
        }

        public static new int PickUpValue = 3;
        
        public static void PickUpBox() {
            Player player = GameEngine.GameLogic.Player;
            List<int> pool = new List<int>{
                1,1,1,1,1,
                2,2,2,2,2,
                3,3,3,3,
                4,4,
                5,
                6,6,
                7,
            };
            Random rand = new Random();
            int effect = pool[rand.Next(pool.Count)];
            // maybe i need to put this part of the logic in player
            int tmp;
            switch (effect) {
                case 1:
                    tmp = rand.Next(10);
                    player.CollectedDiamonds += tmp;
                    player.Score += tmp*player.ScoreMultiplier;
                    player.allScores["Diamonds from lucky box"][0] += 1;
                    player.allScores["Diamonds from lucky box"][1] += tmp*player.ScoreMultiplier;
                    break;
                case 2:
                    player.Hp = player.MaxHp;
                    break;
                case 3:
                    player.Hp -= rand.Next(3);
                    break;
                case 4:
                    player.MaxHp = 15;
                    break;
                case 5:
                    player.EnergyRestoreTick *= 2;
                    break;
                case 6:
                    tmp = rand.Next(10,30);
                    player.Score += tmp*player.ScoreMultiplier;
                    player.allScores["Score from lucky box"][0] += 1;
                    player.allScores["Score from lucky box"][1] += tmp*player.ScoreMultiplier;
                    break;
                case 7:
                    player.ScoreMultiplier *=rand.Next(2,5);
                    break;
            }
        }
    }
}