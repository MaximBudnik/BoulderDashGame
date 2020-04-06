using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;

namespace ClassLibrary.Matrix {
    public class Level : Matrix {
        public int DiamondsQuantity { get; } = 20;
        public int[] PlayerPosition = {0, 0}; // do smth with it on loading level
        public string LevelName { get; } = "level_name";
        public string Aim { get; } = "Collect diamonds";
        public int WalkersCount { get; } = 0;

        //fields for creating level
        private List<int> _quarterPool;
        private int createRoomChance = 0;

        public Level(string levelName) {
            //TODO: retrieve data from json files
            width = 20;
            height = 65;
            LevelName = levelName;
            WalkersCount = 6;
            matrix = new GameEntity[width, height];
            DiggerAlgorithm();
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

        public void DiggerAlgorithm() {
            //How it works?
            //1) All field is filled with walls
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    matrix[i, j] = fillOneTitle(i, j, 5);
                }
            }
            //2)digger is spawned in random position+ it will be start position for player
            Random rand = new Random();
            int startPosX = rand.Next(0, Width);
            int startPosY = rand.Next(0, Height);
            int diggerPoxX = startPosX;
            int diggerPoxY = startPosY;
            //3) the quantity of digger moves is determined
            //Digger moves through field in different positions and leaves behind empty space
            int diggerMoves = rand.Next(20, 40);
            //4) flag makes sure that he changes axis of movement after he gets every position
            bool flag = false;
            //5) quarters are created to make sure that digger wiil be in all parts of field     
            //12 - quarters pos
            //34
            int currentQuarter;
            currentQuarter = checkQuarter(diggerPoxX, diggerPoxY);
            //From quarter pool the next quarter(from which the next point will be chosen) is chosen
            _quarterPool = new List<int>() {
                currentQuarter
            };

            //6) loop that performs all operations 
            for (int i = 0; i < diggerMoves; i++) {
                //7) determine next point to dig

                int number = rand.Next(rand.Next(_quarterPool.Count));
                int nextQuarter = _quarterPool[number];
                fillQuarterPool(nextQuarter);

                int randomX = rand.Next(0, Width);
                int randomY = rand.Next(0, Height);
                while (checkQuarter(randomX, randomY) != nextQuarter) {
                    randomX = rand.Next(0, Width);
                    randomY = rand.Next(0, Height);
                }

                //8)move until digger position and posToMove are the same
                while (checkMoved(flag, diggerPoxX, diggerPoxY, randomX, randomY)) {
                    matrix[diggerPoxX, diggerPoxY] = fillOneTitle(diggerPoxX, diggerPoxY, 1);
                    straightDig(flag, ref diggerPoxX, ref diggerPoxY, randomX, randomY);
                }
                createRoom(100, diggerPoxX, diggerPoxY);
                flag = !flag;
            }

            GameEntity player = new GameEntity(0, startPosX, startPosY);
            matrix[startPosX, startPosY] = player;
            PlayerPosition[0] = startPosX;
            PlayerPosition[1] = startPosY;
        }

        private bool checkMoved(bool flag, int digX, int digY, int ranX, int ranY) {
            if (flag) {
                if (digX == ranX)
                    return false;
                return true;
            }
            if (digY == ranY)
                return false;
            return true;
        }

        private void createRoom(int chance, int posX, int posY) {
            Random rand = new Random();
            int roomSize = rand.Next(0, 4)*2;
            while (!(posX+roomSize<Width&&posX-roomSize>=0&&posY+roomSize<=Height&&posY-roomSize>=0)) {
                roomSize = rand.Next(0, 15);
            }
            int startposX = posX - roomSize / 2;
            int startposY = posY - roomSize / 2;
            for (int i = startposX; i < startposX+roomSize; i++) {
                for (int j = startposY; j < startposY+roomSize; j++) {
                    matrix[i, j] = fillOneTitle(i, j, 0);
                }
            }
        }

        private void straightDig(bool flag, ref int diggerPoxX, ref int diggerPoxY, int randomX, int randomY) {
            if (flag) {
                if (randomX > diggerPoxX) {
                    diggerPoxX++;
                }
                else if (randomX < diggerPoxX) {
                    diggerPoxX--;
                }
            }
            else {
                if (randomY > diggerPoxY) {
                    diggerPoxY++;
                }
                else if (randomY < diggerPoxY) {
                    diggerPoxY--;
                }
            }
        }

        private int checkQuarter(int posY, int posX) {
            if ((posX >= 0 && posX < height / 2) && (posY >= 0 && posY < width / 2)) {
                return 1;
            }
            if ((posX >= 0 && posX < height / 2) && (posY >= width / 2 && posY < width)) {
                return 3;
            }
            if ((posX >= height / 2 && posX < height) && (posY >= 0 && posY < width / 2)) {
                return 2;
            }
            if ((posX >= height / 2 && posX < height) && (posY >= width / 2 && posY < width)) {
                return 4;
            }
            return 1;
        }
        private void fillQuarterPool(int quarter) {
            _quarterPool.Remove(quarter);
            switch (quarter) {
                case 1:
                    _quarterPool.AddRange(new List<int>() {1, 2, 2, 3, 3,});
                    break;
                case 2:
                    _quarterPool.AddRange(new List<int>() {1, 1, 2, 4, 4});

                    break;
                case 3:
                    _quarterPool.AddRange(new List<int>() {1,3, 4, 4});
                    break;
                case 4:
                    _quarterPool.AddRange(new List<int>() { 2, 2, 3, 3, 4});
                    break;
            }
        }

        private GameEntity fillOneTitle(int i, int j, int entityType) {
            switch (entityType) {
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
    }
}