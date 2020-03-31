using System;
using System.Collections.Generic;

namespace ClassLibrary.Entities.Collectable {
    public class LuckyBox : ItemCollectable {
        public LuckyBox(int i, int j) {
            entityType = 7;
            this.positionX = i;
            this.positionY = j;
        }

        public static void pickUpBox() {
            Player player = GameEngine.gameLogic.Player;
            List<int> pool = new List<int>() {
                1, 1, 1,
                2, 2, 2,
                3,3,
                4,
                5,
            };
            Random rand = new Random();
            int effect = pool[rand.Next(pool.Count)];
            // maybe i need to put this part of the logic in player
            switch (effect) {
                case 1:
                    player.CollectedDiamonds += rand.Next(10);
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
                    player.EnergyRestoreTick *=2;
                    break;
            }
        }
    }
}