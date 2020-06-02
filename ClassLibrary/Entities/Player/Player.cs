using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Player {
    public class Player : Movable {
        private readonly int _attackEnergyCost = 6;
        private readonly int _diamondsTowWin;
        private readonly Action _lose;
        private readonly int _moveEnergyCost = 1;
        private readonly int _moveRockEnergyCost = 5;
        private readonly Action<SoundFilesEnum> _playSound;
        private readonly int _teleportRange = 20;
        private readonly Action _win;
        public readonly Dictionary<string, int[]> AllScores;
        private readonly double DynamiteTileDamage = 0.3;
        public readonly Inventory Inventory = new Inventory();
        public readonly Keyboard Keyboard = new Keyboard();
        private readonly int MaxEnergy = 20;
        public readonly string Name;

        public Player(
            int i,
            int j,
            string name,
            Func<Level> getLevel,
            Action win,
            Action lose,
            Action<SoundFilesEnum> playSound,
            int diamondsTowWin)
            : base(getLevel, i, j) {
            Name = name;
            _win = win;
            _lose = lose;
            _playSound = playSound;
            Hp = MaxHp;
            _diamondsTowWin = diamondsTowWin;
            EntityEnumType = 0;
            AllScores = new Dictionary<string, int[]> {
                {"Collected diamonds", new[] {0, 0}},
                {"Collected lucky boxes", new[] {0, 0}},
                {"Diamonds from lucky box", new[] {0, 0}},
                {"Score from lucky box", new[] {0, 0}},
                {"Killed enemies", new[] {0, 0}}
            };
            CanMove = false;
            PathFinderMove = true;
            //TODO: delete thiS features (its only for testing)
            PlayerAnimator = new PlayerAnimator(1);
            Inventory.ArmorLevel = 5;
            Inventory.SwordLevel = 5;
            Inventory.TntQuantity = 5;
            Inventory.StoneInDiamondsConverterQuantity = 5;
        }
        public int MaxHp { get; set; } = 10;
        public int Energy { get; private set; } = 20;
        public int CollectedDiamonds { get; set; }
        public int EnergyRestoreTick { get; set; } = 1;
        public int ScoreMultiplier { get; set; } = 10;
        public int Score { get; set; }
        public int Hero { get; set; }
        public PlayerAnimator PlayerAnimator { get; }

        public int GetScoreToAdd(int value) {
            return value * ScoreMultiplier;
        }
        //TODO: add scores for picking inventory and killing enemies
        public void AddScore(int value) {
            Score += value * ScoreMultiplier;
        }

        public new void GameLoopAction() {
            CheckWin();
            CheckLose();
            RestoreEnergy();
        }

        public void SubstractPlayerHp(int value) {
            if (Inventory.ArmorLevel > 0) {
                Inventory.ArmorCellHp -= value;
                value -= Inventory.ArmorLevel;
            }
            if (Inventory.ArmorCellHp <= 0) {
                Inventory.ArmorLevel--;
                Inventory.ArmorCellHp = 10;
            }
            if (value > 0) Hp -= value;
            _playSound(SoundFilesEnum.HitSound);
            SetAnimation(PlayerAnimationsEnum.GetDamage);
        }

        private void SubstractPlayerHp(int value, PlayerAnimationsEnum animationEnum) {
            if (Inventory.ArmorLevel > 0) {
                Inventory.ArmorCellHp -= value;
                value -= Inventory.ArmorLevel;
            }
            if (Inventory.ArmorCellHp <= 0) {
                Inventory.ArmorLevel--;
                Inventory.ArmorCellHp = 10;
            }
            if (value > 0) Hp -= value;
            _playSound(SoundFilesEnum.HitSound);
            SetAnimation(animationEnum);
        }

        public void Move(string direction, int value) {
            SetAnimation(PlayerAnimationsEnum.Move);
            if (!EnoughEnergy()) return;
            var willMove = false;
            var level = GetLevel();
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            switch (direction) {
                case "vertical":
                    PositionX += value;
                    if (!IsNewPositionSuitable(level))
                        PositionX -= value;
                    else
                        willMove = true;
                    break;
                case "horizontal":
                    PositionY += value;
                    if (IsLevelCellValid(PositionX, PositionY, level.Width, level.Height) &&
                        level[PositionX, PositionY].EntityEnumType == GameEntitiesEnum.Rock && EnoughEnergyForRock()) {
                        ((Rock) level[PositionX, PositionY]).PushRock(PositionX, PositionY, "horizontal", value);
                        Energy -= _moveRockEnergyCost;
                    }
                    if (!IsNewPositionSuitable(level)) PositionY -= value;
                    else willMove = true;
                    break;
                default:
                    throw new Exception("Unknown move direction in Player.cs");
            }
            if (willMove) {
                Energy -= _moveEnergyCost;
                switch (level[PositionX, PositionY].EntityEnumType) {
                    case GameEntitiesEnum.Diamond:
                        ((Diamond) level[PositionX, PositionY]).Collect(() => this);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                    case GameEntitiesEnum.LuckyBox:
                        ((LuckyBox) level[PositionX, PositionY]).Collect(() => this);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                    case GameEntitiesEnum.Barrel:
                        ((BarrelWithSubstance) level[PositionX, PositionY]).Collect(GetLevel, SubstractPlayerHp);
                        break;
                    case GameEntitiesEnum.SwordTile:
                        ((SwordTile) level[PositionX, PositionY]).Collect(() => Inventory);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                    case GameEntitiesEnum.ConverterTile:
                        ((ConverterTile) level[PositionX, PositionY]).Collect(() => Inventory);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                    case GameEntitiesEnum.DynamiteTile:
                        ((DynamiteTile) level[PositionX, PositionY]).Collect(() => Inventory);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                    case GameEntitiesEnum.ArmorTile:
                        ((ArmorTile) level[PositionX, PositionY]).Collect(() => Inventory);
                        _playSound(SoundFilesEnum.PickupSound);
                        break;
                }
            }
            level[PositionX, PositionY] = this;
        }
        private bool IsNewPositionSuitable(Level level) {
            return IsLevelCellValid(PositionX, PositionY, level.Width, level.Height) &&
                   level[PositionX, PositionY].CanMove;
        }
        private bool EnoughEnergyForRock() {
            return Energy >= _moveRockEnergyCost;
        }
        private bool EnoughEnergy() {
            return Energy >= _moveEnergyCost;
        }
        public void HpInEnergy() {
            SubstractPlayerHp(1);
            Energy = MaxEnergy;
        }
        public void Teleport() {
            if (Energy < MaxEnergy) return;
            var level = GetLevel();
            Energy = 0;
            int posX;
            int posY;
            do {
                posX = Randomizer.Random(level.Width);
                posY = Randomizer.Random(level.Height);
            } while (Math.Abs(PositionX - posX) + Math.Abs(PositionY - posY) > _teleportRange);

            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            var pathfinder = new Pathfinder();
            var path = pathfinder.FindPath(PositionX, PositionY, posX, posY, level, (l, p) => true);
            var task = new Task(() => {
                GameEntity temp;
                foreach (var point in path) {
                    SetAnimation(PlayerAnimationsEnum.Teleport);
                    PositionX = point.X;
                    PositionY = point.Y;
                    temp = level[PositionX, PositionY];
                    level[PositionX, PositionY] = this;
                    Thread.Sleep(50);
                    level[PositionX, PositionY] = temp;
                }
                level[PositionX, PositionY] = this;
            });
            task.Start();
        }
        public void ConvertNearStonesInDiamonds() {
            if (Inventory.StoneInDiamondsConverterQuantity == 0) return;
            SetAnimation(PlayerAnimationsEnum.Converting);
            Inventory.StoneInDiamondsConverterQuantity--;
            var level = GetLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
                if (x == 0 || y == 0) {
                    var posX = x + PositionX;
                    var posY = y + PositionY;
                    if (level[posX, posY].EntityEnumType == GameEntitiesEnum.Rock) {
                        var tmp = new StoneInDiamondConverter(posX, posY, GetLevel);
                        level[posX, posY] = tmp;
                    }
                }
            Energy = Energy / 2;
        }
        public void Attack() {
            if (Energy < _attackEnergyCost) return;
            SetAnimation(PlayerAnimationsEnum.Attack);
            Energy -= _attackEnergyCost;
            var level = GetLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++) {
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (IsLevelCellValid(posX, posY, level.Width, level.Height) && level[posX, posY] is Enemy) {
                    var tmp = (Enemy) level[posX, posY];
                    tmp.Hp -= Inventory.SwordLevel;
                    if (tmp.Hp <= 0) {
                        level[tmp.PositionX, tmp.PositionY] = new Diamond(PositionX, PositionY);
                        AddScore(tmp.ScoreForKill);
                        AllScores["Killed enemies"][0] += 1;
                        AllScores["Killed enemies"][1] += GetScoreToAdd(tmp.ScoreForKill);
                    }
                }
            }
        }
        public void UseDynamite() {
            if (Inventory.TntQuantity == 0) return;
            Inventory.TntQuantity--;
            var level = GetLevel();
            double dmg = 0;

            for (var x = -2; x < 3; x++)
            for (var y = -2; y < 3; y++) {
                if (x == 0 && y == 0 || (x == -2 || x == 2) && (y == -2 || y == 2)) continue;
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (IsLevelCellValid(posX, posY, level.Width, level.Height)) {
                    level[posX, posY] = new EmptySpace(RightX, PositionY);
                    dmg += DynamiteTileDamage;
                }
            }
            Energy = Energy / 2;
            SubstractPlayerHp(Convert.ToInt32(dmg), PlayerAnimationsEnum.Explosion);
        }
        private void CheckLose() {
            if (Hp <= 0)
                _lose();
        }
        private void CheckWin() {
            if (CollectedDiamonds >= _diamondsTowWin)
                _win();
        }
        private void RestoreEnergy() {
            if (Energy < MaxEnergy) Energy += EnergyRestoreTick;
        }

        public void SetAnimation(PlayerAnimationsEnum currentAnimationEnum) {
            PlayerAnimator.SetAnimation(currentAnimationEnum);
        }
    }
}