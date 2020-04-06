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

        private int _createRoomChance = 0;

        // 0/5/10/100
        private int _roomChanceGrow = 5; // the bigger the  chance, the bigger open spaces on level will be

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

        private void DiggerAlgorithm() {
            //in general generation depends on:
            //    _roomChanceGrow
            //    diggerMoves
            //     9), where size of setted rooms are setted
            //    secondary generation

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
            int diggerMoves = rand.Next(30, 50);
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
                // it depends on quarter chosen from the pool
                int number = rand.Next(rand.Next(_quarterPool.Count));
                int nextQuarter = _quarterPool[number];
                fillQuarterPool(nextQuarter);

                //8)Here random point is randomed
                int randomX = rand.Next(0, Width);
                int randomY = rand.Next(0, Height);
                while (checkQuarter(randomX, randomY) != nextQuarter) {
                    randomX = rand.Next(0, Width);
                    randomY = rand.Next(0, Height);
                }

                //9) digger starts moving
                while (checkMoved(flag, diggerPoxX, diggerPoxY, randomX, randomY)) {
                    //making previous tile empty space
                    matrix[diggerPoxX, diggerPoxY] = fillOneTitle(diggerPoxX, diggerPoxY, 1);
                    //moves next tile
                    straightDig(flag, ref diggerPoxX, ref diggerPoxY, randomX, randomY);
                    //has a chance to create new room
                    CreateRoom(_createRoomChance, diggerPoxX, diggerPoxY, 10, 4);
                    //chance of creating room always growing
                    _createRoomChance += _roomChanceGrow;
                }
                //after digger reaches end point? he creates room
                CreateRoom(100, diggerPoxX, diggerPoxY);
                flag = !flag;
            }

            //secondary generation

            //the we need to replace walls and empty spaces with different blocks, spawn keys, doors,+enemies
            //here i work with out place(walls and their surroundings)
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (matrix[i, j].EntityType == 5) {
                        // i make wal sand but only if it doesnt border wit empty space
                        //so player just cant get out the map so easily
                        matrix[i, j] = fillOneTitle(i, j, 2);
                        if (i + 1 < Width && (matrix[i + 1, j].EntityType == 1 || matrix[i + 1, j].EntityType == 7)) {
                            matrix[i, j] = fillOneTitle(i, j, 5);
                        }
                        else if (i - 1 >= 0 && (matrix[i - 1, j].EntityType == 1 || matrix[i - 1, j].EntityType == 7)) {
                            matrix[i, j] = fillOneTitle(i, j, 5);
                        }
                        else if (j + 1 < Height &&
                                 (matrix[i, j + 1].EntityType == 1 || matrix[i, j + 1].EntityType == 7)) {
                            matrix[i, j] = fillOneTitle(i, j, 5);
                        }
                        else if (j - 1 >= 0 && (matrix[i, j - 1].EntityType == 1 || matrix[i, j - 1].EntityType == 7)) {
                            matrix[i, j] = fillOneTitle(i, j, 5);
                        }
                    }
                }
            }
            //TODO: optimize following processes
            // here i fill matrix with hand-made objects
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (i==7&&j==7) {
                        columnHall(i, j,6,20);
                    }
                }
            }
            
            // here i fill the rest of empty space that was created be rooms 
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (matrix[i, j].EntityType == 7) {
                        List<int> pool = new List<int>() {
                            //this values represent titles and probability of spawn
                            8, 8, 8, 8, 8, 8, 8, 8, 8,
                            3, 3, 3, 3,
                            4, 4, 4,
                            7,
                        };
                        matrix[i, j] = fillOneTitle(i, j, innerEntitySpawner(pool));
                    }
                }
            }
            // fill empty space created be corridors(digger moves)
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (matrix[i, j].EntityType == 1) {
                        List<int> pool = new List<int>() {
                            //this values represent titles and probability of spawn
                            8, 8, 8, 8, 8, 8, 8, 8, 8,
                            1, 1, 1, 1,
                            4, 4, 4,
                            7,
                        };
                        matrix[i, j] = fillOneTitle(i, j, innerEntitySpawner(pool));
                    }
                }
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
        private void CreateRoom(int chance, int posX, int posY, int maxSizeX = 20, int maxSizeY = 8) {
            Random rand = new Random();
            int rnd = rand.Next(0, 100);

            if (rnd <= chance) {
                int roomSizeX = rand.Next(0, maxSizeX);
                int roomSizeY = rand.Next(0, maxSizeY);
                while (!(posX + roomSizeX < Width && posX - roomSizeX >= 0 && posY + roomSizeY <= Height &&
                         posY - roomSizeY >= 0)) {
                    roomSizeX = rand.Next(0, maxSizeX);
                    roomSizeY = rand.Next(0, maxSizeY);
                }
                int startposX = posX - roomSizeX / 2;
                int startposY = posY - roomSizeY / 2;
                for (int i = startposX; i < startposX + roomSizeX; i++) {
                    for (int j = startposY; j < startposY + roomSizeY; j++) {
                        matrix[i, j] = fillOneTitle(i, j, 7);
                    }
                }
                _createRoomChance = 0;
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
            _quarterPool.AddRange(new List<int>() {1, 2, 3, 4});
        }
        private int innerEntitySpawner(List<int> pool) {
            Random rand = new Random();
            int randNumber = pool[rand.Next(pool.Count)];
            return randNumber;
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
                case 8:
                    SandTranclucent sandTranclucent = new SandTranclucent(i, j);
                    return sandTranclucent;
                default:
                    EmptySpace es = new EmptySpace(i, j);
                    return es;
            }
        }
        private void createCross() {
            
        }
        private void columnHall(int i,int j,int sizeX,int sizeY) {
            for (int k = i; k < i+sizeX; k++) {
                if (k%2==0) {
                    for (int l = 0; l < j+sizeY; l++) {
                        if (l % 2 == 0) {
                            matrix[k, l] = fillOneTitle(k, l, 5);
                        }
                        else {
                            matrix[k, l] = fillOneTitle(k, l, 1);
                        }
                    }
                }
            }
        }
        private void createMegaRoom() {
            
        }
        private void createCircleRoom() {
            
        }
    }
}