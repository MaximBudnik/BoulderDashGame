using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyDigger : Enemy {
        public EnemyDigger(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = GameEntities.EnemyDigger;
            Damage = 2;
            Hp = 5;
            ScoreForKill = 30;

            //forms
            CurrentFrame = Randomizer.Random(0, 4);
        }
        public new void GameLoopAction() {
            EnemyDamageNearTitles();
            EnemyMovement();
        }

        
        private void EnemyMovement() {
            var level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var moves = new List<string>();

            if (playerPosX < PositionX && LeftX < level.Width &&
                level[LeftX, PositionY].EntityType != GameEntities.Player)
                moves.Add("left");
            if (playerPosX > PositionX && RightX >= 0 && level[RightX, PositionY].EntityType != GameEntities.Player)
                moves.Add("right");
            if (playerPosY > PositionY && BotY < level.Height &&
                level[PositionX, BotY].EntityType != GameEntities.Player)
                moves.Add("down");
            if (playerPosY < PositionY && TopY >= 0 && level[PositionX, TopY].EntityType != GameEntities.Player)
                moves.Add("up");
            moves.Add("hold");
            var action = Randomizer.GetRandomFromList(moves);
            switch (action) {
                case "hold":
                    return;
                case "up":
                    Move("vertical", -1, PositionX, PositionY);
                    break;
                case "down":
                    Move("vertical", 1, PositionX, PositionY);
                    break;
                case "right":
                    Move("horizontal", 1, PositionX, PositionY);
                    break;
                case "left":
                    Move("horizontal", -1, PositionX, PositionY);
                    break;
            }
        }
    }
}