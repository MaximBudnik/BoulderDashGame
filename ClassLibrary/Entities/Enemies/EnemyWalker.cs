using System;
using System.Collections.Generic;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyWalker : Enemy {
        public EnemyWalker(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = 6;
            Damage = 5;
            Hp = 10;

            //forms
            CurrentFrame = Randomizer.Random(0, 3);
        }
        public new void GameLoopAction() {
            EnemyMovement();
            EnemyDamageNearTitles();
        }

        private void EnemyMovement() {
            var level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var moves = new List<string>();

            if (playerPosX < PositionX && Left < level.Width &&
                level[Left, PositionY].CanMove)
                moves.Add("left");
            if (playerPosX > PositionX && Right >= 0 && level[Right, PositionY].CanMove)
                moves.Add("right");
            if (playerPosY > PositionY && Bot < level.Height &&
                level[PositionX, Bot].CanMove)
                moves.Add("down");
            if (playerPosY < PositionY && Top >= 0 && level[PositionX, Top].CanMove)
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