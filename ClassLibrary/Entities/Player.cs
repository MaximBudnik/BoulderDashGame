﻿using System;

namespace ClassLibrary.Entities {
    public class Player : Movable {
        
        public Player(int[] pos){
            positionX = pos[0];
            positionY = pos[1];
            entityType = 0;
            
            
            // Hp = MaxHp;
            // Energy = MaxEnergy;
            // CollectedDiamonds = 0;
        }

        public int MaxHp { get; }= 10;
        public int Hp = 10;
        public int MaxEnergy{ get; } = 20;
        public int Energy = 20;
        public int CollectedDiamonds;
        private int _energyRestoreTick = 2;
        private int _moveEnergyCost = 1;
        private int _moveRockEnergyCost = 5;

        public new void  GameLoopAction() {
            CheckWin();
            CheckLose();

            if (Energy < MaxEnergy && GameEngine.gameLogic.FrameCounter % 5 == 0) {
                Energy += _energyRestoreTick;
                GameEngine.gameLogic.drawLevel();
            }
        }
        
        
        public void Move(string direction, int value) {// TODO: refactor me! i duplicate movable
            if (Energy >= _moveEnergyCost) {
                Level level =  GameEngine.gameLogic.CurrentLevel;
            level[positionX, positionY] = new EmptySpace(positionX,positionY);
            if (direction == "vertical") {
                positionX += value;
                if (positionX == level.Width || positionX == -1 || level[positionX, positionY].EntityType == 5 ||
                    level[positionX, positionY].EntityType == 3) {
                    //  level[positionX, positionY]==5 || level[positionX, positionY]==3 means that if there are rocks or walls, player cant move there
                    positionX -= value;  
                    Energy+=_moveEnergyCost;
                }
                   
                //checking on diamonds
                if (level[positionX, positionY].EntityType==4) {
                    CollectDiamond(1);
                }
                Energy-=_moveEnergyCost;
                level[positionX, positionY] = this;//May cause bugs! check it out                 
            }
            if (direction == "horizontal") {
                positionY += value;
                //pushing rock
                if (level[positionX, positionY].EntityType==3) {
                    if (Energy >= _moveRockEnergyCost) {
                        GameEngine.gameLogic.RockProcessor.PushRock(positionX, positionY, "horizontal",value);
                        // level[positionX, positionY].PushRock("horizontal", value);
                        Energy -= _moveRockEnergyCost;
                    }
                }
                if (positionY == level.Height || positionY == -1 || level[positionX, positionY].EntityType == 5 ||
                    level[positionX, positionY].EntityType == 3) {
                    Energy += _moveEnergyCost;
                    positionY -= value;
                }
                //checking on diamonds
                if (level[positionX, positionY].EntityType==4) {
                    CollectDiamond(1);
                }
                Energy -= _moveEnergyCost;
                level[positionX, positionY] = this;
            }
            GameEngine.gameLogic.drawLevel();
            }
        }

        public void HpInEnergy() {
            Hp--;
            Energy = MaxEnergy;
            GameEngine.gameLogic.drawLevel();
        }

        private void CollectDiamond(int value) {
            CollectedDiamonds+=value;
        }

        private void CheckLose() {
            if (Hp <= 0) {
                GameEngine.gameLogic.Lose();
            }
        }
        
        private void CheckWin() {
            if (CollectedDiamonds == GameEngine.gameLogic.CurrentLevel.DiamondsQuantity) {//TODO: must change depending on level
                GameEngine.gameLogic.Win();
            }
        }
    }
}