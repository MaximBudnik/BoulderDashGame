using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
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
        public readonly Dictionary<string, int[]> AllScores;

        private readonly int _moveEnergyCost = 1;
        private readonly int _moveRockEnergyCost = 5;

        private readonly int _diamondsTowWin;

        private readonly Action<int, int, string, int> _pushRock;
        private readonly Action _win;
        private readonly Action _lose;

        public Player(
            int i,
            int j,
            string name,
            Func<Level> getLevel,
            Action<int, int, string, int> pushRock,
            Action win,
            Action lose,
            int diamondsTowWin)
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
        }

        public new void GameLoopAction() {
            CheckWin();
            CheckLose();
            RestoreEnergy();
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
                                             level[PositionX, PositionY].EntityType == 5 ||
                                             level[PositionX, PositionY].EntityType == 3
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
                    }
                }
                level[PositionX, PositionY] = this;
            }
        }

        public void HpInEnergy() {
            Hp--;
            Energy = MaxEnergy;
        }

        public void Teleport() {
            if (Energy >= MaxEnergy) {
                var level = GetLevel();
                Energy = 0;
                var posX = Randomizer.Random(level.Width);
                var posY = Randomizer.Random(level.Height);
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                PositionX = posX;
                PositionY = posY;
                level[PositionX, PositionY] = this;
            }
        }

        private void CollectDiamond() {
            int value = ItemCollectible.PickUpValue;
            CollectedDiamonds += value;
            AllScores["Collected diamonds"][0] += 1;
            AllScores["Collected diamonds"][1] += value * ScoreMultiplier;
            Score += value * ScoreMultiplier;
        }

        private void CollectLuckyBox() {
            LuckyBox.PickUpBox(() => this);
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