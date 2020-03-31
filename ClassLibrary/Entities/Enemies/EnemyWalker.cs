using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class EnemyWalker : Enemy {
        
        
        public void GameLoopAction() {
            EnemyMovement();
            EnemyDefaultDamage();
        }

        public EnemyWalker(int posX, int posY) {
            positionX = posX;
            positionY = posY;
            entityType = 6;
            _damage = 5;
        }



        private void EnemyMovement() {
            Level level = GameEngine.gameLogic.CurrentLevel;
            int playerPosX = GameEngine.gameLogic.Player.positionX;
            int playerPosY = GameEngine.gameLogic.Player.positionY;
            List<string> moves = new List<string>();
            
            if ( playerPosX < positionX && positionX-1 < level.Width && (level[positionX-1,positionY].EntityType==1||level[positionX-1,positionY].EntityType==2)) {
                moves.Add("up");
            }
            if( playerPosX > positionX && positionX+1 >=0 && (level[positionX+1,positionY].EntityType==1||level[positionX+1,positionY].EntityType==2)){
                moves.Add("down");
            }
            if (playerPosY > positionY && positionY+1 < level.Height && (level[positionX,positionY+1].EntityType==1||level[positionX,positionY+1].EntityType==2)) {
                moves.Add("right");
            }
            if ( playerPosY < positionY && positionY-1 >=0 && (level[positionX,positionY-1].EntityType==1||level[positionX,positionY-1].EntityType==2)) {
                moves.Add("left");
            }
            moves.Add("hold");
            Random rnd = new Random();
            int number = rnd.Next(rnd.Next(moves.Count));
            string action = moves[number];
            switch (action) {
                case "up":
                    Move("vertical", -1,positionX,positionY);
                    break;
                case "down": 
                    Move("vertical", 1,positionX,positionY);
                    break;
                case "right": 
                    Move("horizontal", 1,positionX,positionY);
                    break;
                case "left": 
                    Move("horizontal", -1,positionX,positionY);
                    break;
                case "hold": 
                    break;
            }
        }
    }
}