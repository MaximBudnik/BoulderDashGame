using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Enemies.SmartEnemies {
    public partial class SmartEnemy {
        public void AddEnergy(int value) {
            _energy += value;
        }
        protected void IdleAction() {
            _energy += 5;
        }
        protected void ChasePlayer() {
            var level = GetLevel();
            Point dest;
            try {
                dest = GetNextPosition(level, GetPlayerPosX(), GetPlayerPosY());
            }
            catch (Exception) {
                Log($"Bot cant chase player");
                _actionList.RemoveAll(x => x.Effect==ChasePlayer);
                InvokeBestAction();
                return;
            }
            Move(level, dest);
            Log($"Bot decided to chase player");
        }
        protected void RunFromPlayer() {
            var level = GetLevel();
            var playerPosX = GetPlayerPosX();
            var playerPosY = GetPlayerPosY();
            Point dest = new Point(PositionX,PositionY);
            var distanceToPlayer = GetDistanceToPlayer(playerPosX,playerPosY);
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++) {
                if (!(x == 0 || y == 0)) continue;
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (!IsLevelCellValid(posX, posY, level.Width, level.Height)) continue;
                if (level[posX, posY].CanMove &&
                    distanceToPlayer < GetDistanceToPlayer(posX, posY, playerPosX, playerPosY)) {
                    dest.X = posX;
                    dest.Y = posY;
                }
            }
            if (dest.X == PositionX && dest.Y == PositionY) {
                Log($"Bot cant run from player");
                _actionList.RemoveAll(x => x.Effect==RunFromPlayer);
                InvokeBestAction();
                return;
            } 
            Move(level, dest);
            Log($"Bot decided to run from player");
        }
        protected void RegenerateHp() {
            _energy -= RegenerateHpCost;
            Hp++;
        }
        protected void UseShield() {
            _energy -= UseShieldCost;
            IsShieldActive = true;
        }
        protected void UseDynamite() {
            _playSound(SoundFilesEnum.BombSound);
            _energy -= UseDynamiteCost;
            var level = GetLevel();
            double dmg = 0;

            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++) {
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (!IsLevelCellValid(posX, posY, level.Width, level.Height)) continue;
                if (level[posX, posY] is Enemy enemy) enemy.SubstractEnemyHp(Convert.ToInt32(dmg));
                else if (level[posX, posY] is Player.Player player) player.SubstractPlayerHp(Convert.ToInt32(dmg));
                else {
                    level[posX, posY] = new EmptySpace(posX, posY);
                }
                dmg += DynamiteTileDamage;
            }
        }

        protected void UseConverter() {
            _playSound(SoundFilesEnum.ConverterSound);

            _energy -= UseConverterCost;
            var level = GetLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (IsLevelCellValid(posX, posY, level.Width, level.Height) &&
                        level[posX, posY].EntityEnumType == GameEntitiesEnum.Rock) {
                        var tmp = new StoneInDiamondConverter(posX, posY, GetLevel, _playSound);
                        level[posX, posY] = tmp;
                    }
                }
        }
        protected void Teleport() {
            _playSound(SoundFilesEnum.TeleportSound);

            _energy -= TeleportCost;
            var level = GetLevel();
            _energy = 0;
            int posX;
            int posY;
            do {
                posX = Randomizer.Random(level.Width);
                posY = Randomizer.Random(level.Height);
            } while (Math.Abs(PositionX - posX) + Math.Abs(PositionY - posY) > TeleportRange);

            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            var pathfinder = new Pathfinder();
            var path = pathfinder.FindPath(PositionX, PositionY, posX, posY, level, (l, p) => true);
            var task = new Task(() => {
                foreach (var point in path) {
                    PositionX = point.X;
                    PositionY = point.Y;
                    var temp = level[PositionX, PositionY];
                    level[PositionX, PositionY] = this;
                    Thread.Sleep(50);
                    level[PositionX, PositionY] = temp;
                }
                level[PositionX, PositionY] = this;
            });
            task.Start();
            
        }
        protected void Move(Level level, Point dest) {
            if (!level[dest.X, dest.Y].CanMove) return;
            level[dest.X, dest.Y].BreakAction(this);
            level[dest.X, dest.Y] = this;
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            PositionX = dest.X;
            PositionY = dest.Y;
        }
    }
}