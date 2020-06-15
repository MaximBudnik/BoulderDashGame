using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public partial class SmartEnemy : Enemy {
        //Need to be disabled in console
        private static readonly bool UseDebugger = true;
        private readonly List<SmartAction> _actionList = new List<SmartAction>();
        private readonly Func<Player.Player> _getOutdatedPlayer;
        private readonly Action<SoundFilesEnum> _playSound;
        private readonly int TeleportRange = 20;
        protected int Agression;
        protected int ChasePlayerWeight = 30;
        protected int Energy;
        protected int EnergyRestoreIdle = 5;
        protected int IdleWeight = 20;
        protected int PlayerDetectionRange = 20;
        protected int RegenerateHpCost = 10;
        protected int RegenerateHpWeight = 150;
        protected int RunFromPlayerWeight = 55;
        protected int TeleportCost = 40;
        protected int TeleportWeight = 3;
        protected int UseConverterCost = 30;
        protected int UseConverterWeight = 10;
        protected int UseDynamiteCost = 40;
        protected int UseDynamiteWeight = 5;
        protected int UseShieldCost = 40;
        protected int UseShieldWeight = 10;
        protected SmartEnemy(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer, Action<SoundFilesEnum> playSound)
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            _getOutdatedPlayer = getOutdatedPlayer;
            _playSound = playSound;
            Agression = Randomizer.Random(1, 11);
        }

        public bool IsShieldActive { get; private set; }

        protected void MakeDecision() {
            Log($"Enemy position: {PositionX}:{PositionY} Agression: {Agression} Energy: {Energy}");
            _actionList.Clear();
            var level = GetLevel();
            var player = _getOutdatedPlayer();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var distanceToPlayer = GetDistanceToPlayer(playerPosX, playerPosY);

            CalculateChasePlayerBenefit(distanceToPlayer);
            CalculateRunFromPlayerBenefit(player.Inventory.SwordLevel, player.Hp, distanceToPlayer);
            CalculateRegenerateHpBenefit();
            CalculateIdleBenefit();
            CalculateUseShieldBenefit();
            CalculateUseConverterBenefit(level);
            CalculateTeleportBenefit(distanceToPlayer);
            CalculateUseDynamiteBenefit(distanceToPlayer);

            InvokeBestAction();
            Log("----------------------------------------------------");
        }
        private void InvokeBestAction() {
            _actionList.OrderBy(e => e.BenefitPoints).Reverse().First().InvokeAction();
        }
        private void CalculateIdleBenefit() {
            var result = IdleWeight / Agression;
            Log($"IdleBenefit: {result}");
            _actionList.Add(new SmartAction(result, IdleAction));
        }
        public override void SubstractEnemyHp(int value) {
            if (IsShieldActive) {
                IsShieldActive = !IsShieldActive;
                return;
            }
            base.SubstractEnemyHp(value);
        }
        private void CalculateChasePlayerBenefit(int distanceToPlayer) {
            int result;
            if (PlayerDetectionRange < distanceToPlayer) result = 0;
            else if (distanceToPlayer <= 1) result = 0;
            else result = Agression * ChasePlayerWeight / distanceToPlayer;
            Log($"ChasePlayerBenefit: {result}");
            _actionList.Add(new SmartAction(result, ChasePlayer));
        }
        private void CalculateRunFromPlayerBenefit(int playerSwordLevel, int playerHp, int distanceToPlayer) {
            var result = (playerSwordLevel + playerHp / 2 - Hp - Agression) * RunFromPlayerWeight / distanceToPlayer;
            Log($"RunFromPlayerBenefit: {result}");
            _actionList.Add(new SmartAction(result, RunFromPlayer));
        }
        private void CalculateRegenerateHpBenefit() {
            if (MaxHp == Hp) {
                Log("Enemy has full hp");
                return;
            }
            if (Energy < RegenerateHpCost) {
                Log("Enemy dont have energy for hp regeneration");
                return;
            }
            var result = RegenerateHpWeight / (MaxHp - Hp);
            Log($"RegenerateHpBenefit: {result}");
            _actionList.Add(new SmartAction(result, RegenerateHp));
        }
        private void CalculateUseShieldBenefit() {
            if (IsShieldActive) {
                Log("Shield is already active");
                return;
            }
            if (Energy < UseShieldCost) {
                Log("Not enought energy for shield");
                return;
            }
            var result = (Energy - Agression) * UseShieldWeight;
            Log($"UseShieldBenefit: {result}");
            _actionList.Add(new SmartAction(result, UseShield));
        }
        private void CalculateUseDynamiteBenefit(int distanceToPlayer) {
            if (Energy < UseDynamiteCost) {
                Log("Not enough energy for using dynamite");
                return;
            }
            var result = 0;
            if (IsShieldActive) result += UseDynamiteWeight;
            if (distanceToPlayer < 2) result += Agression * UseDynamiteWeight;
            Log($"UseDynamiteBenefit: {result}");
            _actionList.Add(new SmartAction(result, UseDynamite));
        }
        private void CalculateUseConverterBenefit(Level level) {
            if (Energy < UseConverterCost) {
                Log("Not enough energy for using converter");
                return;
            }
            var result = 0;
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (IsLevelCellValid(posX, posY, level.Width, level.Height) &&
                        level[posX, posY].EntityEnumType == GameEntitiesEnum.Rock)
                        result += 1;
                }
            result *= UseConverterWeight;
            Log($"UseConverterBenefit: {result}");
            _actionList.Add(new SmartAction(result, UseConverter));
        }
        private void CalculateTeleportBenefit(int distanceToPlayer) {
            if (Energy < TeleportCost) {
                Log("Not enough energy for teleport");
                return;
            }
            var result = distanceToPlayer * TeleportWeight;
            Log($"TeleportBenefit: {result}");
            _actionList.Add(new SmartAction(result, Teleport));
        }

        private int GetDistanceToPlayer(int selfX, int selfY, int playerX, int playerY) {
            return Math.Abs(selfX - playerX) + Math.Abs(selfY - playerY);
        }

        private int GetDistanceToPlayer(int playerX, int playerY) {
            return GetDistanceToPlayer(PositionX, PositionY, playerX, playerY);
        }
        private static void Log(string s) {
            if (UseDebugger) Console.WriteLine(s);
        }
    }
}