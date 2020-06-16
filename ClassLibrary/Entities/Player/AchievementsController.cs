using System;
using System.Collections.Generic;

namespace ClassLibrary.Entities.Player {
    public class AchievementsController {
        public readonly Queue<AchievementsEnum> AchievementsQueue = new Queue<AchievementsEnum>();
        public readonly List<AchievementsEnum> Achieved = new List<AchievementsEnum>();
        private int _achievementScore = 100;

        public bool WasBombUsed = false;
        public bool WasCandyLaunched = false;
        
        public void CheckAchievements(Player player) {
            if (!Achieved.Contains(AchievementsEnum.Armored) && player.Inventory.ArmorLevel >= 5)
                AchievementsQueue.Enqueue(AchievementsEnum.Armored);

            if (!Achieved.Contains(AchievementsEnum.ItsShiny) && player.CollectedDiamonds > 0)
                AchievementsQueue.Enqueue(AchievementsEnum.ItsShiny);

            if (!Achieved.Contains(AchievementsEnum.Rage) && player.Adrenaline >= 100)
                AchievementsQueue.Enqueue(AchievementsEnum.Rage);
            
            if (!Achieved.Contains(AchievementsEnum.Bomberman) && WasBombUsed)
                AchievementsQueue.Enqueue(AchievementsEnum.Bomberman);
            
            if (!Achieved.Contains(AchievementsEnum.CandyLauncher) && WasCandyLaunched)
                AchievementsQueue.Enqueue(AchievementsEnum.CandyLauncher);
            
            if (!Achieved.Contains(AchievementsEnum.Butcher) && player.KilledEnemies>=3)
                AchievementsQueue.Enqueue(AchievementsEnum.Butcher);
        }
    }
}