using System;
using System.Collections.Generic;

namespace ClassLibrary.Entities.Player {
    public class AchievementsController {

        public readonly Queue<AchievementsEnum> AchievementsQueue = new Queue<AchievementsEnum>();
        private int _achievementScore = 100;

        

        public AchievementsController() {
            AchievementsQueue.Enqueue(AchievementsEnum.Armored);
            AchievementsQueue.Enqueue(AchievementsEnum.Bomberman);
            AchievementsQueue.Enqueue(AchievementsEnum.Butcher);
            AchievementsQueue.Enqueue(AchievementsEnum.Rage);
            AchievementsQueue.Enqueue(AchievementsEnum.CandyLauncher);
            AchievementsQueue.Enqueue(AchievementsEnum.ItsShiny);


        }
        
        public void CheckAchievements(Player player) {
            
        }
        
    }
}