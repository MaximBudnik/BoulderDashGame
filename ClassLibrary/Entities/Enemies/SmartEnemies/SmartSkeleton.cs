using System;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public class SmartSkeleton : SmartEnemy {
        public SmartSkeleton(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp, Func<Player.Player> getOutdatedPlayer,
            Action<SoundFilesEnum> playSound
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp, getOutdatedPlayer, playSound) {
            EntityEnumType = GameEntitiesEnum.SmartSkeleton;
            Damage = 3;
            MaxHp = 10;
            Hp = 10;
            ScoreForKill = 40;
            CurrentFrame = Randomizer.Random(0, 3);
        }

        public override void GameLoopAction() {
            EnemyDamageNearTitles();
            MakeDecision();
        }
    }
}