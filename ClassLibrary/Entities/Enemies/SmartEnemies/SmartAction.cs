using System;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class SmartAction {
        public Action Effect;
        public int BenefitPoints;

        public SmartAction(int benefitPoints, Action effect) {
            BenefitPoints = benefitPoints;
            Effect = effect;
        }

        public void InvokeAction() {
            Effect();
        }
        
    }
}