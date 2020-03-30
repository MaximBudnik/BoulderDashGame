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

        private GameEntity generateTile(int i, int j) {
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
                    return new EmptySpace(i,j);
                case 2:
                    return new Sand(i,j);
                case 3:
                    return new Rock(i,j);
                case 4:
                    _diamondsQuantity++;
                    return new Diamond(i,j);
                case 5:
                    return new Wall(i,j);
            }
            GameEntity gameEntity = new GameEntity(randNumber,i,j);
            return gameEntity;
        }

        public void CreateLevel() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = generateTile(i,j);
                }
            }
            GameEntity player = new GameEntity(0,5,5);
            matrix[5, 5] = player;
        }

        public int[] defaultPlayerPosition { get; }= {5, 5};// do smth with it on loading level
    }
}