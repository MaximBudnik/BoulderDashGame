using System;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class SmartAction {
        public readonly int BenefitPoints;
        public readonly Action Effect;

        public SmartAction(int benefitPoints, Action effect) {
            BenefitPoints = benefitPoints;
            Effect = effect;
        }

        public void InvokeAction() {
            Effect();
        }
    }
}