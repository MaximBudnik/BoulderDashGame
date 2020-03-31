using System;
using System.Collections.Generic;
using ClassLibrary.Entities;

namespace ClassLibrary {
    public class Level : Matrix {
        public Level(string levelName) {
            //TODO: retrieve data from json files

            width = 15;
            height = 30;
            matrix = new GameEntity[width, height];
            CreateLevel();
        }
        
        private int _diamondsQuantity=0;

        public int DiamondsQuantity {
            get => _diamondsQuantity;
        }

        private GameEntity GenerateTile(int i, int j) {
            //clown method TODO: refactor this
            List<int> pool = new List<int>() {
                //this values represent titles and probability of spawn
                1,
                2, 2, 2, 2,
                3,
                4,
                5,
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
                    _diamondsQuantity++;
                    Diamond diamond = new Diamond(i,j);
                    return diamond;
                case 5:
                    Wall wall = new Wall(i,j);
                    return wall;
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
        }

        public int[] defaultPlayerPosition { get; }= {5, 5};// do smth with it on loading level
    }
}