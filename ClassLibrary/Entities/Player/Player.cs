using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Player {
    public class Player : Movable {
        public int MaxHp { get; set; } = 10;

        public int Hp { get; set; } = 10;
        public readonly string Name;
        public readonly int MaxEnergy = 20;
        public int Energy { get; private set; } = 20;
        public int CollectedDiamonds { get; set; }
        public int EnergyRestoreTick { get; set; } = 2;
        public int ScoreMultiplier { get; set; } = 10;
        public int Score { get; set; }

        public Inventory Inventory = new Inventory();

        public readonly Dictionary<string, int[]> AllScores;

        private readonly int _moveEnergyCost = 1;
        private readonly int _moveRockEnergyCost = 5;

        private readonly int _diamondsTowWin;

        private readonly Action<int, int, string, int> _pushRock;
        private readonly Action _win;
        private readonly Action _lose;
        private readonly Func<List<StoneInDiamondConverter>> _getStoneInDiamondsConvertersList;
        private readonly Func<List<Acid>> _getAcidBlocksList;

        public Player(
            int i,
            int j,
            string name,
            Func<Level> getLevel,
            Action<int, int, string, int> pushRock,
            Action win,
            Action lose,
            int diamondsTowWin,
            Func<List<StoneInDiamondConverter>> getStoneInDiamondsConverter, Func<List<Acid>> getAcidBlocksList)
            : base(getLevel, i, j) {
            Name = name;
            _pushRock = pushRock;
            _win = win;
            _lose = lose;
            _diamondsTowWin = diamondsTowWin;
            EntityType = 0;
            AllScores = new Dictionary<string, int[]> {
                {"Collected diamonds", new[] {0, 0}},
                {"Collected lucky boxes", new[] {0, 0}},
                {"Diamonds from lucky box", new[] {0, 0}},
                {"Score from lucky box", new[] {0, 0}}
            };
            _getStoneInDiamondsConvertersList = getStoneInDiamondsConverter;
            _getAcidBlocksList = getAcidBlocksList;
            CanMove = false;
            Inventory.ArmorLevel = 10;
        }

        public new void GameLoopAction() {
            CheckWin();
            CheckLose();
            RestoreEnergy();
        }

        public void SubstractPlayerHp(int value) {
            if (Inventory.ArmorLevel>0) {
                Inventory.ArmorCellHp -= value;
                value -= Inventory.ArmorLevel;
            }
            if (Inventory.ArmorCellHp <=0) {
                Inventory.ArmorLevel--;
                Inventory.ArmorCellHp = 10;
            }
            if (value>0) {
                Hp -= value;
            }
        }

        public void Move(string direction, int value) {
            bool EnoughEnergy() {
                if (Energy >= _moveEnergyCost)
                    return true;
                return false;
            }

            bool EnoughEnergyForRock() {
                if (Energy >= _moveRockEnergyCost)
                    return true;
                return false;
            }

            bool CheckNewPosition(Level level) {
                if (
                    PositionX == level.Width || PositionX == -1
                                             ||
                                             PositionY == level.Height || PositionY == -1
                                             ||
                                             level[PositionX, PositionY].CanMove == false
                )
                    return true;
                return false;
            }

            if (EnoughEnergy()) {
                var willMove = false;
                var level = GetLevel();
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                switch (direction) {
                    case "vertical":
                        PositionX += value;
                        if (CheckNewPosition(level))
                            PositionX -= value;
                        else
                            willMove = true;
                        break;
                    case "horizontal":
                        PositionY += value;
                        if (level[PositionX, PositionY].EntityType == 3 && EnoughEnergyForRock()) {
                            _pushRock(PositionX, PositionY, "horizontal", value);
                            Energy -= _moveRockEnergyCost;
                        }
                        if (CheckNewPosition(level))
                            PositionY -= value;
                        else
                            willMove = true;
                        break;
                    default:
                        throw new Exception("Unknown move direction in Player.cs");
                }
                if (willMove) {
                    Energy -= _moveEnergyCost;
                    switch (level[PositionX, PositionY].EntityType) {
                        case 4:
                            CollectDiamond();
                            break;
                        case 7:
                            CollectLuckyBox();
                            break;
                        case 12:
                            BarrelWithSubstance.Collect(PositionX, PositionY, GetLevel,
                                _getAcidBlocksList, SubstractPlayerHp);
                            break;
                        case 20:
                            SwordTile.Collect(() => Inventory);
                            break;
                        case 21:
                            ConverterTile.Collect(() => Inventory);
                            break;
                        case 22:
                            TntTile.Collect(() => Inventory);
                            break;
                        case 23:
                            ArmorTile.Collect(() => Inventory);
                            break;
                    }
                }
                level[PositionX, PositionY] = this;
            }
        }

        public void HpInEnergy() {
            SubstractPlayerHp(1);
            Energy = MaxEnergy;
        }

        public void Teleport() {
            if (Energy < MaxEnergy) return;
            var level = GetLevel();
            Energy = 0;
            var posX = Randomizer.Random(level.Width);
            var posY = Randomizer.Random(level.Height);
            level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
            PositionX = posX;
            PositionY = posY;
            level[PositionX, PositionY] = this;
        }

        public void ConvertNearStonesInDiamonds() {
            // if (Energy < MaxEnergy) return;
            var level = GetLevel();
            if (PositionX + 1 < level.Width && level[PositionX + 1, PositionY].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX + 1, PositionY, GetLevel,
                    _getStoneInDiamondsConvertersList);
                level[PositionX + 1, PositionY] = tmp;
                _getStoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionX - 1 >= 0 && level[PositionX - 1, PositionY].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX - 1, PositionY, GetLevel,
                    _getStoneInDiamondsConvertersList);
                level[PositionX - 1, PositionY] = tmp;
                _getStoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionY + 1 < level.Height && level[PositionX, PositionY + 1].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX, PositionY + 1, GetLevel,
                    _getStoneInDiamondsConvertersList);
                level[PositionX, PositionY + 1] = tmp;
                _getStoneInDiamondsConvertersList().Add(tmp);
            }
            if (PositionY - 1 >= 0 && level[PositionX, PositionY - 1].EntityType == 3) {
                var tmp = new StoneInDiamondConverter(PositionX, PositionY - 1, GetLevel,
                    _getStoneInDiamondsConvertersList);
                level[PositionX, PositionY - 1] = tmp;
                _getStoneInDiamondsConvertersList().Add(tmp);
            }
            // Energy = 0;
        }

        private void CollectDiamond() {
            var value = ItemCollectible.PickUpValue;
            CollectedDiamonds += value;
            AllScores["Collected diamonds"][0] += 1;
            AllScores["Collected diamonds"][1] += value * ScoreMultiplier;
            Score += value * ScoreMultiplier;
        }

        private void CollectLuckyBox() {
            LuckyBox.Collect(() => this);
            var tmp = LuckyBox.PickUpValue * ScoreMultiplier;
            AllScores["Collected lucky boxes"][0] += 1;
            AllScores["Collected lucky boxes"][1] += tmp;
            Score += tmp;
        }

        private void CheckLose() {
            if (Hp <= 0)
                _lose();
        }

        private void CheckWin() {
            if (CollectedDiamonds >= _diamondsTowWin) //TODO: must change depending on level
                _win();
        }

        private int _frameCounter;
        private void RestoreEnergy() {
            _frameCounter++;
            if (Energy < MaxEnergy && _frameCounter >= 2) {
                Energy += EnergyRestoreTick;
                _frameCounter = 0;
            }
        }
    }
}