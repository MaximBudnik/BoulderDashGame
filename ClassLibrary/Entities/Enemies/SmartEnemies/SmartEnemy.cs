using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public partial class SmartEnemy : Enemy {
        protected SmartEnemy(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer)
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            GetOutdatedPlayer = getOutdatedPlayer;
            PlayerDetectionRange = 15;
            Agression = Randomizer.Random(1, 101);
        }

        protected Func<Player.Player> GetOutdatedPlayer;
        protected readonly int PlayerDetectionRange;
        protected readonly int Agression;
        protected readonly int Energy = 0;
        protected List<SmartAction> ActionList;

        protected void MakeDecision() {
            Log($"Enemy position: {PositionX}:{PositionY}");
            Log($"Agression: {Agression}");

            ActionList = new List<SmartAction>();
            var level = GetLevel();
            var player = GetOutdatedPlayer();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var distanceToPlayer = GetDistanceToPlayer(playerPosX, playerPosY);

            GetChasePlayerBenefit(playerPosX, playerPosY, distanceToPlayer);
            GetRunFromPlayerBenefit(player.Inventory.SwordLevel, player.Hp, distanceToPlayer);

            ActionList.OrderBy(e => e.BenefitPoints).First().InvokeAction();
            Log("");
        }

        protected int GetIdleBenefit() {
            return 1;
        }

        protected int GetPowerfulAttackBenefit() {
            return 1;
        }

        protected int ChasePlayerWeight = 30;
        protected void GetChasePlayerBenefit(int playerPosX, int playerPosY, int distanceToPlayer) {
            //TODO: Important, even if enemy cant reach player, priority is big
            int result;
            if (PlayerDetectionRange < distanceToPlayer) result = 0;
            else result = Agression * ChasePlayerWeight / distanceToPlayer;
            Log($"ChasePlayerBenefit: {result}");
            ActionList.Add(new SmartAction(result, ChasePlayer));
        }

        protected int RunFromPlayerWeight = 80;

        protected void GetRunFromPlayerBenefit(int playerSwordLevel, int playerHp, int distanceToPlayer) {
            int result;
            result = (playerSwordLevel + playerHp / 2 - Hp) * RunFromPlayerWeight / distanceToPlayer;
            Log($"RunFromPlayerBenefit: {result}");
            ActionList.Add(new SmartAction(result, RunFromPlayer));
        }

        protected int GetRegenerateHpBenefit() {
            return 1;
        }

        protected int GetUseShieldBenefit() {
            return 1;
        }

        protected int GetUseDynamiteBenefit() {
            return 1;
        }

        protected int GetUseAcidBenefit() {
            return 1;
        }

        protected int GetTeleportBenefit() {
            return 1;
        }

        private int GetDistanceToPlayer(int playerX, int playerY) {
            return Math.Abs(PositionX - playerX) + Math.Abs(PositionY - playerY);
        }

        protected void Log(string s) {
            Console.WriteLine(s);
        }
    }
}