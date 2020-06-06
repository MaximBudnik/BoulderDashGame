using System;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class SmartPeaceful : SmartEnemy {
        public SmartPeaceful(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer,
            Action<SoundFilesEnum> playSound
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp, getOutdatedPlayer, playSound) {
            EntityEnumType = GameEntitiesEnum.SmartPeaceful;
            Damage = 1;
            MaxHp = 8;
            Hp = 8;
            ScoreForKill = 30;
            Agression = Randomizer.Random(1, 8);
            CurrentFrame = Randomizer.Random(0, 3);
            ChasePlayerWeight = 25;
            RunFromPlayerWeight = 65;
            RegenerateHpWeight = 160;
            UseShieldCost = 30;
            UseShieldWeight = 15;
            UseDynamiteWeight = 10;
            UseConverterCost = 30;
            UseConverterWeight = 15;
        }

        public override void GameLoopAction() {
            EnemyDamageNearTitles();
            MakeDecision();
        }
    }
}