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

        public EnemyWalker(int posX, int posY) {
            PositionX = posX;
            PositionY = posY;
            entityType = 6;
            _damage = 5;
        }

        private void EnemyMovement() {
            Level level = GameEngine.GameLogic.CurrentLevel;
            int playerPosX = GameEngine.GameLogic.Player.PositionX;
            int playerPosY = GameEngine.GameLogic.Player.PositionY;
            List<string> moves = new List<string>();

            if (playerPosX < PositionX && PositionX - 1 < level.Width &&
                (level[PositionX - 1, PositionY].EntityType == 1 || level[PositionX - 1, PositionY].EntityType == 2 || level[PositionX - 1, PositionY].EntityType == 4)) {
                moves.Add("up");
            }
            if (playerPosX > PositionX && PositionX + 1 >= 0 &&
                (level[PositionX + 1, PositionY].EntityType == 1 || level[PositionX + 1, PositionY].EntityType == 2 || level[PositionX + 1, PositionY].EntityType == 4)) {
                moves.Add("down");
            }
            if (playerPosY > PositionY && PositionY + 1 < level.Height &&
                (level[PositionX, PositionY + 1].EntityType == 1 || level[PositionX, PositionY + 1].EntityType == 2|| level[PositionX, PositionY + 1].EntityType == 4)) {
                moves.Add("right");
            }
            if (playerPosY < PositionY && PositionY - 1 >= 0 &&
                (level[PositionX, PositionY - 1].EntityType == 1 || level[PositionX, PositionY - 1].EntityType == 2|| level[PositionX, PositionY - 1].EntityType == 4)) {
                moves.Add("left");
            }
            moves.Add("hold");
            Random rnd = new Random();
            int number = rnd.Next(rnd.Next(moves.Count));
            string action = moves[number];
            switch (action) {
                case "up":
                    Move("vertical", -1, PositionX, PositionY);
                    GameEngine.GameLogic.DrawLevel();
                    break;
                case "down":
                    Move("vertical", 1, PositionX, PositionY);
                    GameEngine.GameLogic.DrawLevel();
                    break;
                case "right":
                    Move("horizontal", 1, PositionX, PositionY);
                    GameEngine.GameLogic.DrawLevel();
                    break;
                case "left":
                    Move("horizontal", -1, PositionX, PositionY);
                    GameEngine.GameLogic.DrawLevel();
                    break;
                case "hold":
                    break;
            }
        }
    }
}