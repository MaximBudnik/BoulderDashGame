using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyWalker : Enemy {
        public new void GameLoopAction() {
            EnemyMovement();
            EnemyDefaultDamage();
        }

        public EnemyWalker(int i, int j,
            Func<Level> getLevel, Action drawLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action updatePlayerInterface,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, drawLevel, updatePlayerInterface, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = 6;
            Damage = 5;
        }

        private void EnemyMovement() {
            var level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var moves = new List<string>();

            if (playerPosX < PositionX && PositionX - 1 < level.Width &&
                level[PositionX - 1, PositionY].EntityType != 3 && level[PositionX - 1, PositionY].EntityType != 5)
                moves.Add("up");
            if (playerPosX > PositionX && PositionX + 1 >= 0 && level[PositionX + 1, PositionY].EntityType != 3 &&
                level[PositionX + 1, PositionY].EntityType != 5)
                moves.Add("down");
            if (playerPosY > PositionY && PositionY + 1 < level.Height &&
                level[PositionX, PositionY + 1].EntityType != 3 && level[PositionX, PositionY + 1].EntityType != 5)
                moves.Add("right");
            if (playerPosY < PositionY && PositionY - 1 >= 0 && level[PositionX, PositionY - 1].EntityType != 3 &&
                level[PositionX, PositionY - 1].EntityType != 5)
                moves.Add("left");
            moves.Add("hold");
            string action = Randomizer.GetRandomFromList(moves);
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
            DrawLevel();
        }
    }
}