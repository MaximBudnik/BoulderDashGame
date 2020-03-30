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

        private GameEntity generateTile() {
            //clown method TODO: refactor this
            List<int> numbers = new List<int>() {
                //this values represent titles and probability of spawn
                1,
                2, 2, 2, 2,
                3,
                4,
                5,
            };
            Random rand = new Random();
            int randNumber = numbers[rand.Next(numbers.Count)];
            GameEntity gameEntity = new GameEntity(randNumber);
            return gameEntity;
        }

        public void CreateLevel() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = generateTile();
                }
            }
            GameEntity player = new GameEntity(0);
            matrix[5, 5] = player;
        }

        public int[] defaultPlayerPosition { get; }= {5, 5};// do smth with it on loading level
    }
}