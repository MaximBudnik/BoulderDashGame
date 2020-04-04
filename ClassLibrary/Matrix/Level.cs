using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;

namespace ClassLibrary.Matrix {
    public class Level : Matrix {
        public int DiamondsQuantity { get; } = 20;
        public int[] DefaultPlayerPosition { get; } = {5, 5}; // do smth with it on loading level
        public string LevelName { get; } = "level_name";
        public string Aim { get; } = "Collect diamonds";
        public int WalkersCount { get; } = 0;

        public Level(string levelName) {
            //TODO: retrieve data from json files
            width = 15;
            height = 30;
            LevelName = levelName;
            WalkersCount = 6;
            matrix = new GameEntity[width, height];
            CreateLevel();
        }

        private GameEntity GenerateTile(int i, int j) {
            List<int> pool = new List<int>() {
                //this values represent titles and probability of spawn
                1, 1, 1, 1,
                2, 2, 2, 2, 2, 2, 2, 2, 2,
                3, 3, 3,
                4, 4, 4,
                5, 5, 5,
                7,
            };
            Random rand = new Random();
            int randNumber = pool[rand.Next(pool.Count)];

            switch (randNumber) {
                case 1:
                    EmptySpace emptySpace = new EmptySpace(i, j);
                    return emptySpace;
                case 2:
                    Sand sand = new Sand(i, j);
                    return sand;
                case 3:
                    Rock rock = new Rock(i, j);
                    return rock;
                case 4:
                    Diamond diamond = new Diamond(i, j);
                    return diamond;
                case 5:
                    Wall wall = new Wall(i, j);
                    return wall;
                case 7:
                    LuckyBox luckyBox = new LuckyBox(i, j);
                    return luckyBox;
                default:
                    EmptySpace es = new EmptySpace(i, j);
                    return es;
            }
        }

        public void CreateLevel() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = GenerateTile(i, j);
                }
            }
            GameEntity player = new GameEntity(0, 5, 5);
            matrix[5, 5] = player;
            //here i crate enemies in random points
            for (int i = 0; i < WalkersCount; i++) {
                Random rand = new Random();
                int posX = rand.Next(width);
                int posY = rand.Next(height);
                EnemyWalker enemy = new EnemyWalker(posX, posY);
                matrix[posX, posY] = enemy;
                GameEngine.GameLogic.LevelEnemyWalkers.Add(enemy);
            }
        }
    }
}