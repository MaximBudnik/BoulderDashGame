using System;
using System.Collections.Generic;

namespace ClassLibrary {
    public class Level : Matrix {
        public Level(string levelName) {
            //TODO: retrieve data from json files

            width = 10;
            height = 20;
            matrix = new int[width, height];
            CreateLevel();
        }

        private int generateTile() {
            //clown method TODO: refactor this
            List<int> numbers = new List<int>() {
                //this values represent titles and probability of spawn
                1,
                2, 2, 2, 2,
                3, 3,
                4, 4,
                5,
            };
            Random rand = new Random();
            int randNumber = numbers[rand.Next(numbers.Count)];
            return randNumber;
        }

        public void CreateLevel() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = generateTile();
                }
            }
            matrix[5, 5] = 0;
        }

        public int[] defaultPlayerPosition { get; }= {5, 5};// do smth with it on loading level
    }
}