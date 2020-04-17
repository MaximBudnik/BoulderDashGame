using System;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities {
    public class Player : Movable {
        public int MaxHp = 10;
        public int Hp = 10;
        public string Name = "Maxim";
        public int MaxEnergy { get; } = 20;
        public int Energy = 20;
        public int CollectedDiamonds;
        public int EnergyRestoreTick = 2;
        public int ScoreMultiplier = 10;
        public int Score = 0; //diamonds and lucky boxes + are multiplied in setter
        
        private int _moveEnergyCost = 1;
        private int _moveRockEnergyCost = 5;
        
        public Player(int[] pos) {
            PositionX = pos[0];
            PositionY = pos[1];
            entityType = 0;
        }

        public new void GameLoopAction() {
            CheckWin();
            CheckLose();
            RestoreEnergy();
        }

        public void Move(string direction, int value) {
            // TODO: refactor me! i duplicate movable
            if (Energy >= _moveEnergyCost) {
                Level level = GameEngine.GameLogic.CurrentLevel;
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                if (direction == "vertical") {
                    PositionX += value;
                    if (PositionX == level.Width || PositionX == -1 || level[PositionX, PositionY].EntityType == 5 ||
                        level[PositionX, PositionY].EntityType == 3) {
                        //  level[positionX, positionY]==5 || level[positionX, positionY]==3 means that if there are rocks or walls, player cant move there
                        PositionX -= value;
                        Energy += _moveEnergyCost;
                    }
                    //checking on diamonds
                    if (level[PositionX, PositionY].EntityType == 4) {
                        CollectDiamond();
                    }
                    if (level[PositionX, PositionY].EntityType == 7) {
                        CollectLuckyBox();
                    }
                    Energy -= _moveEnergyCost;
                    level[PositionX, PositionY] = this;               
                }
                if (direction == "horizontal") {
                    PositionY += value;
                    //pushing rock
                    if (level[PositionX, PositionY].EntityType == 3) {
                        if (Energy >= _moveRockEnergyCost) {
                            GameEngine.GameLogic.RockProcessor.PushRock(PositionX, PositionY, "horizontal", value);
                            Energy -= _moveRockEnergyCost;
                        }
                    }
                    if (PositionY == level.Height || PositionY == -1 || level[PositionX, PositionY].EntityType == 5 ||
                        level[PositionX, PositionY].EntityType == 3) {
                        Energy += _moveEnergyCost;
                        PositionY -= value;
                    }
                    //checking on diamonds
                    if (level[PositionX, PositionY].EntityType == 4) {
                        CollectDiamond();
                    }
                    if (level[PositionX, PositionY].EntityType == 7) {
                        CollectLuckyBox();
                    }
                    Energy -= _moveEnergyCost;
                    level[PositionX, PositionY] = this;
                }
                GameEngine.GameLogic.DrawLevel();
            }
        }

        public void HpInEnergy() {
            Hp--;
            Energy = MaxEnergy;
            GameEngine.GameLogic.UpdatePlayerInterface();
        }

        public void Teleport() {
            if (Energy == MaxEnergy) {
                Level level = GameEngine.GameLogic.CurrentLevel;
                Energy = 0;
                Random rnd = new Random();
                int posX = rnd.Next(level.Width);
                int posY = rnd.Next(level.Height);
                level[PositionX, PositionY] = new EmptySpace(PositionX, PositionY);
                PositionX = posX;
                PositionY = posY;
                level[PositionX, PositionY] = this;
                GameEngine.GameLogic.DrawLevel();
            }
        }

        private void CollectDiamond() {
            int value = Diamond.PickUpValue;
            CollectedDiamonds += value;
            Score += value*ScoreMultiplier;
            GameEngine.GameLogic.UpdateUpperInterface();
        }

        private void CollectLuckyBox() {
            LuckyBox.PickUpBox();
            Score += LuckyBox.PickUpValue*ScoreMultiplier;
            GameEngine.GameLogic.UpdateUpperInterface();
        }

        private void CheckLose() {
            if (Hp <= 0) {
                GameEngine.GameLogic.Lose();
            }
        }

        private void CheckWin() {
            if (CollectedDiamonds >= GameEngine.GameLogic.CurrentLevel.DiamondsQuantity) {
                //TODO: must change depending on level
                GameEngine.GameLogic.Win();
            }
        }

        private void RestoreEnergy() {
            if (Energy < MaxEnergy && GameEngine.GameLogic.FrameCounter % 3 == 0) {
                Energy += EnergyRestoreTick;
                GameEngine.GameLogic.UpdatePlayerInterface();
            }
        }
    }
}