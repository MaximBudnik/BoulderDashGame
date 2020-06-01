using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public partial class EnemyWalker : Enemy {
        public EnemyWalker(int i, int j,
            Func<Level> getLevel,
            Func<int> getPlayerPosX, Func<int> getPlayerPosY,
            Action<int> changePlayerHp
        )
            : base(i, j, getLevel, getPlayerPosX, getPlayerPosY, changePlayerHp) {
            EntityType = GameEntities.EnemyWalker;
            Damage = 5;
            Hp = 10;
            ScoreForKill = 40;
            //forms
            CurrentFrame = Randomizer.Random(0, 3);
        }
        public new void GameLoopAction() {
            EnemyDamageNearTitles();
            Moving();
        }

        private void EnemyMovement() {
            var level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            var moves = new List<string>();

            if (playerPosX < PositionX && LeftX < level.Width &&
                level[LeftX, PositionY].CanMove)
                moves.Add("left");
            if (playerPosX > PositionX && RightX >= 0 && level[RightX, PositionY].CanMove)
                moves.Add("right");
            if (playerPosY > PositionY && BotY < level.Height &&
                level[PositionX, BotY].CanMove)
                moves.Add("down");
            if (playerPosY < PositionY && TopY >= 0 && level[PositionX, TopY].CanMove)
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

        private Level level;

        private void Moving() {
            level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();

            List<Point> path;

            try {
                path = FindPath(PositionX, PositionY, playerPosX, playerPosY) ??
                       throw new ArgumentNullException(
                           "FindPath(PositionX, PositionY, playerPosX, playerPosY)");
            }
            catch (Exception e) {
                return;
            }
            var dest = path[1];
            level[dest.X, dest.Y] = this;
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            PositionX = dest.X;
            PositionY = dest.Y;
        }
    }
}