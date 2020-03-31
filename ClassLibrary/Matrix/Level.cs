using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;

namespace ClassLibrary.Matrix {
    public class Level : Matrix {
        public Level(string levelName) {
            //TODO: retrieve data from json files

            width = 15;
            height = 30;
            matrix = new GameEntity[width, height];
            CreateLevel();
        }
        
        private int _diamondsQuantity=20;

        public int DiamondsQuantity {
            get => _diamondsQuantity;
        }

        private GameEntity GenerateTile(int i, int j) {
            //clown method TODO: refactor this
            List<int> pool = new List<int>() {
                //this values represent titles and probability of spawn
                1,1,1,1,
                2, 2, 2, 2,2,2,2,2,2,
                3,3,3,
                4,4,4,
                5,5,5,
                7,
            };
            Random rand = new Random();
            int randNumber = pool[rand.Next(pool.Count)];
           
            switch (randNumber) {
                case 1:
                    EmptySpace emptySpace = new EmptySpace(i,j);
                    return emptySpace;
                case 2:
                    Sand sand = new Sand(i,j);
                    return sand;
                case 3:
                    Rock rock = new Rock(i,j);
                    return rock;
                case 4:
                    // _diamondsQuantity++;//TODO: fix me: when enemies are spawned, they can decrease my number :(
                    Diamond diamond = new Diamond(i,j);
                    return diamond;
                case 5:
                    Wall wall = new Wall(i,j);
                    return wall;
                case 7:
                    LuckyBox luckyBox = new LuckyBox(i,j);
                    return luckyBox;
                case 8:
                 EnemyRandomer enemyRandomer = new EnemyRandomer(i,j); 
                 GameEngine.gameLogic.LevelEnemyRandomers.Add(enemyRandomer);
                 return enemyRandomer;
                default:
                    EmptySpace es = new EmptySpace(i,j);
                    return es; 
            }
            // GameEntity gameEntity = new GameEntity(randNumber,i,j);
            // return gameEntity;
        }

        public void CreateLevel() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = GenerateTile(i,j);
                }
            }
            GameEntity player = new GameEntity(0,5,5);
            matrix[5, 5] = player;
            
            EnemyWalker enemy = new EnemyWalker(2,25);
            matrix[2, 25] = enemy;
            GameEngine.gameLogic.LevelEnemyWalkers.Add(enemy);
            
            enemy = new EnemyWalker(10,23);
            matrix[10, 23] = enemy;
            GameEngine.gameLogic.LevelEnemyWalkers.Add(enemy);
            
            enemy = new EnemyWalker(5,10);
            matrix[5, 10] = enemy;
            GameEngine.gameLogic.LevelEnemyWalkers.Add(enemy);
            
            enemy = new EnemyWalker(6,8);
            matrix[6, 8] = enemy;
            GameEngine.gameLogic.LevelEnemyWalkers.Add(enemy);
            
        }

        public int[] defaultPlayerPosition { get; }= {5, 5};// do smth with it on loading level
    }
}