using System;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class SmartDevil : SmartEnemy {
        public SmartDevil(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer,
            Action<SoundFilesEnum> playSound
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp, getOutdatedPlayer, playSound) {
            EntityEnumType = GameEntitiesEnum.SmartDevil;
            Damage = 4;
            MaxHp = 14;
            Hp = 14;
            ScoreForKill = 80;
            Agression = Randomizer.Random(5, 15);
            CurrentFrame = Randomizer.Random(0, 3);
            IdleWeight = 15;
            ChasePlayerWeight = 45;
            RunFromPlayerWeight = 1;
            UseConverterWeight = 1;
            TeleportCost = 30;
            TeleportWeight = 3;
            _energy = 120;
            EnergyRestoreIdle = 5;
            PlayerDetectionRange = 30;
        }

        public override void GameLoopAction() {
            EnemyDamageNearTitles();
            MakeDecision();
        }
    }
}