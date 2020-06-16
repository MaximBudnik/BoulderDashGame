using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Entities.Player {
    public class Player : AdvancedLogic {
        private readonly int _adrenalineOnCombo = 100;
        private readonly int _adrenalineTickReduction = 5;
        private readonly int _attackEnergyCost = 5;
        private readonly int _diamondsTowWin;
        private readonly Action _lose;
        private readonly int _moveEnergyCost = 1;
        private readonly int _shootCost = 5;
        private readonly int _moveRockEnergyCost = 5;
        private readonly Action<SoundFilesEnum> _playSound;
        private readonly Action<Player> _setPlayer;
        private readonly int _teleportRange = 20;
        private readonly Action _win;
        public readonly Dictionary<string, int[]> AllScores;
        private readonly int DefaultArmorCellHp = 10;
        public readonly Inventory Inventory = new Inventory();
        public readonly Keyboard Keyboard = new Keyboard();
        public readonly AchievementsController AchievementsController = new AchievementsController();
        public readonly int MaxEnergy = 20;
        public readonly string Name;

        public Player(
            int i,
            int j,
            string name,
            Func<Level> getLevel,
            Action win,
            Action lose,
            Action<SoundFilesEnum> playSound,
            Action<Player> setPlayer,
            int diamondsTowWin)
            : base(getLevel, i, j) {
            Name = name;
            _win = win;
            _lose = lose;
            _playSound = playSound;
            _setPlayer = setPlayer;
            MaxHp = 10;
            Hp = MaxHp;
            _diamondsTowWin = diamondsTowWin;
            EntityEnumType = GameEntitiesEnum.Player;
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

        public MoveDirectionExtended LastMove = MoveDirectionExtended.Right;        

        
        public int Energy { get; private set; } = 20;

        public int Adrenaline { get; private set; }
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

        public override void GameLoopAction() {
            AchievementsController.CheckAchievements(this);
            CheckWin();
            CheckLose();
            RestoreEnergy();
            AdrenalineEffect();
            _setPlayer(this);
        }

        public void SubstractPlayerHp(int value) {
            if (Inventory.ArmorLevel > 0) {
                Inventory.ArmorCellHp -= value;
                value -= Inventory.ArmorLevel;
            }
            if (Inventory.ArmorCellHp <= 0) {
                Inventory.ArmorLevel--;
                Inventory.ArmorCellHp = DefaultArmorCellHp;
            }
            if (value > 0) Hp -= value;
            _playSound(SoundFilesEnum.HitSound);
            SetAnimation(PlayerAnimationsEnum.GetDamage);
        }

        private void SubstractPlayerHp(int value, PlayerAnimationsEnum animationEnum) {
            SubstractPlayerHp(value);
            SetAnimation(animationEnum);
        }
        
        public override void Move(MoveDirectionEnum direction, int value) {
            if (!EnoughEnergy()) return;
            SetAnimation(PlayerAnimationsEnum.Move);
            var level = GetLevel();
            var willMove = false;
            switch (direction) {
                case MoveDirectionEnum.Horizontal:
                    var newPositionX = PositionX + value;
                    if (IsNewPositionSuitable(level, newPositionX, PositionY)) {
                        MakePreviousPositionEmpty(level);
                        PositionX = newPositionX;
                        willMove = true;
                    }
                    break;
                case MoveDirectionEnum.Vertical:
                    var newPositionY = PositionY + value;

                    if (level[PositionX, newPositionY] is Rock && Energy > _moveRockEnergyCost) {
                        Energy -= _moveRockEnergyCost;
                        level[PositionX, newPositionY].BreakAction(this);
                    }

                    if (IsNewPositionSuitable(level, PositionX, newPositionY)) {
                        MakePreviousPositionEmpty(level);
                        PositionY = newPositionY;
                        willMove = true;
                    }
                    break;
            }
            if (!willMove) return;
            _playSound(SoundFilesEnum.WalkSound);
            Energy -= _moveEnergyCost;
            level[PositionX, PositionY].BreakAction(this);
            level[PositionX, PositionY] = this;
        }
        private void MakePreviousPositionEmpty(Level level) {
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
        }

        private bool IsNewPositionSuitable(Level level, int posX, int posY) {
            return IsLevelCellValid(posX, posY, level.Width, level.Height) &&
                   level[posX, posY].CanMove;
        }
        private bool EnoughEnergy() {
            return Energy >= _moveEnergyCost;
        }
        public void UseEnergyConverter() {
            if (Inventory.StoneInDiamondsConverterQuantity > 0) {
                Energy = MaxEnergy;
                Inventory.StoneInDiamondsConverterQuantity--;
            }
        }
        public void Teleport() {
            if (Energy < MaxEnergy) return;
            _playSound(SoundFilesEnum.TeleportSound);
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
                foreach (var point in path) {
                    SetAnimation(PlayerAnimationsEnum.Teleport);
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
        public void ConvertNearStonesInDiamonds() {
            if (Inventory.StoneInDiamondsConverterQuantity == 0) return;
            _playSound(SoundFilesEnum.ConverterSound);
            SetAnimation(PlayerAnimationsEnum.Converting);
            Inventory.StoneInDiamondsConverterQuantity--;
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
            Energy /= 2;
        }
        public void Attack() {
            if (Energy < _attackEnergyCost) return;
            _playSound(SoundFilesEnum.AttackSound);
            SetAnimation(PlayerAnimationsEnum.Attack);
            Energy -= _attackEnergyCost;
            var level = GetLevel();
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++) {
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (IsLevelCellValid(posX, posY, level.Width, level.Height) && level[posX, posY] is Enemy) {
                    var tmp = (Enemy) level[posX, posY];
                    tmp.SubstractEnemyHp(Inventory.SwordLevel);
                    if (tmp.Hp <= 0) {
                        level[tmp.PositionX, tmp.PositionY] = new Diamond(PositionX, PositionY, _playSound);
                        Adrenaline += tmp.ScoreForKill;
                        AddScore(tmp.ScoreForKill);
                        AllScores["Killed enemies"][0] += 1;
                        AllScores["Killed enemies"][1] += GetScoreToAdd(tmp.ScoreForKill);
                    }
                }
            }
        }

        public void Shoot() {
            if (Energy < _shootCost) return;
            Energy -= _shootCost;
            var level = GetLevel();
            switch (LastMove) {
                case MoveDirectionExtended.Top:
                    level[PositionX,PositionY] = new Candy(GetLevel,PositionX,PositionY, MoveDirectionEnum.Horizontal, -1, SubstractPlayerHp);
                    break;
                case MoveDirectionExtended.Right:
                    level[PositionX,PositionY] = new Candy(GetLevel,PositionX,PositionY, MoveDirectionEnum.Vertical, 1, SubstractPlayerHp);
                    break;
                case MoveDirectionExtended.Bot:
                    level[PositionX,PositionY] = new Candy(GetLevel,PositionX,PositionY, MoveDirectionEnum.Horizontal, 1, SubstractPlayerHp);
                    break;
                case MoveDirectionExtended.Left:
                    level[PositionX,PositionY] = new Candy(GetLevel,PositionX,PositionY, MoveDirectionEnum.Vertical, -1, SubstractPlayerHp);
                    break;
            }
            Task task = new Task(() => {
                Thread.Sleep(50);
                level[PositionX, PositionY] = this;
            });
            task.Start();
        }
        
        public void UseDynamite() {
            if (Inventory.TntQuantity == 0) return;
            _playSound(SoundFilesEnum.BombSound);
            Inventory.TntQuantity--;
            var level = GetLevel();
            double dmg = 0;

            for (var x = -2; x < 3; x++)
            for (var y = -2; y < 3; y++) {
                if (x == 0 && y == 0 || (x == -2 || x == 2) && (y == -2 || y == 2)) continue;
                var posX = x + PositionX;
                var posY = y + PositionY;
                if (!IsLevelCellValid(posX, posY, level.Width, level.Height)) continue;
                if (level[posX, posY] is Enemy enemy) enemy.SubstractEnemyHp(Convert.ToInt32(dmg));
                else
                    level[posX, posY] = new EmptySpace(posX, posY);
                dmg += DynamiteTileDamage;
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

        private void AdrenalineEffect() {
            if (Adrenaline > 0) {
                if (Adrenaline > 50) RestoreEnergy();
                if (Adrenaline > 75) RestoreEnergy();
                if (Adrenaline > 100) {
                    Hp++;
                    Adrenaline -= 30;
                }
                RestoreEnergy();
                Adrenaline -= _adrenalineTickReduction;
                if (Adrenaline < 0) Adrenaline = 0;
            }
        }

        public void CheckAdrenalineCombo() {
            if (Keyboard.A == KeyboardEnum.Enabled && Keyboard.D == KeyboardEnum.Enabled) {
                Adrenaline += _adrenalineOnCombo;
                SubstractPlayerHp(1);
            }
        }

        public void SetAnimation(PlayerAnimationsEnum currentAnimationEnum) {
            PlayerAnimator.SetAnimation(currentAnimationEnum);
        }
    }
}