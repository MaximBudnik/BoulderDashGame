using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Matrix {
    public class Level : Matrix {
        public int DiamondsQuantity { get; } = 20; //TODO: change
        public int LevelName { get; }
        public string Aim { get; } = "Collect diamonds";

        private int WalkersCount { get; set; }
        private int DiggersCount { get; set; }


        //fields for creating level
        public string LevelType { get; private set; } = "red";
        private List<int> _quarterPool;
        private int _createRoomChance; // 0/5/10/100
        private int _roomChanceGrow = 5; // the bigger the  chance, the bigger open spaces on level will be
        private int _diggerMovesLower = 20;
        private int _diggerMovesUpper = 40;
        private int _createRoomMaxSizeX = 10;
        private int _createRoomMaxSizeY = 10;

        private string playerName;
        private Func<Level> getLevel;
        private Action win;
        private Action lose;
        private Func<int> getPlayerPositionX;
        private Func<int> getPlayerPositionY;
        private Action<int> substractPlayerHp;
        private Action<Player> setPlayer;

        public Level(int levelName, string playerName,
            Func<Level> getLevel,
            Action win, Action lose,
            Func<int> getPlayerPositionX,
            Func<int> getPlayerPositionY,
            Action<int> substractPlayerHp, Action<Player> setPlayer
        ) {
            //TODO: now choose the size of the level from starting game/random
            width = 24; //20 for console
            height = 38; //65 for console
            LevelName = levelName;
            this.playerName = playerName;
            this.getLevel = getLevel;
            this.win = win;
            this.lose = lose;
            this.getPlayerPositionX = getPlayerPositionX;
            this.getPlayerPositionY = getPlayerPositionY;
            this.substractPlayerHp = substractPlayerHp;
            this.setPlayer = setPlayer;
            WalkersCount = 4;
            DiggersCount = 3;
            matrix = new GameEntity[width, height];
            DiggerAlgorithm();
        }

        private void DiggerAlgorithm() {
            //in general generation depends on:
            //    _roomChanceGrow
            //    diggerMoves
            //     9), where size of setted rooms are setted
            //    secondary generation

            //1) All field is filled with walls
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                matrix[i, j] = fillOneTitle(i, j, 5);
            //2)digger is spawned in random position+ it will be start position for player
            var rand = new Random();
            var startPosX = rand.Next(0, Width);
            var startPosY = rand.Next(0, Height);
            var diggerPoxX = startPosX;
            var diggerPoxY = startPosY;

            //2.1) randomize values for level generation to be sure every level is unique
            RandomizeSeedValues();
            //3) the quantity of digger moves is determined
            //Digger moves through field in different positions and leaves behind empty space
            var diggerMoves = rand.Next(_diggerMovesLower, _diggerMovesUpper);
            //4) flag makes sure that he changes axis of movement after he gets every position
            var flag = false;
            //5) quarters are created to make sure that digger wiil be in all parts of field     
            //12 - quarters pos
            //34
            var currentQuarter = CheckQuarter(diggerPoxX, diggerPoxY);
            //From quarter pool the next quarter(from which the next point will be chosen) is chosen
            _quarterPool = new List<int> {
                currentQuarter
            };

            //6) loop that performs all operations 
            for (var i = 0; i < diggerMoves; i++) {
                //7) determine next point to dig
                // it depends on quarter chosen from the pool
                var number = rand.Next(rand.Next(_quarterPool.Count));
                var nextQuarter = _quarterPool[number];
                FillQuarterPool(nextQuarter);

                //8)Here random point is randomed
                var randomX = rand.Next(0, Width);
                var randomY = rand.Next(0, Height);
                while (CheckQuarter(randomX, randomY) != nextQuarter) {
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
                    CreateRoom(_createRoomChance, diggerPoxX, diggerPoxY, _createRoomMaxSizeX, _createRoomMaxSizeY);
                    //chance of creating room always growing
                    _createRoomChance += _roomChanceGrow;
                }
                //after digger reaches end point? he creates room
                CreateRoom(100, diggerPoxX, diggerPoxY);
                flag = !flag;
            }

            //secondary generation

            //TODO: optimize following processes
            // here i fill matrix with hand-made objects
            var numberOfFeatures = rand.Next(0, 4);

            while (numberOfFeatures > 0) {
                var feature = rand.Next(1, 5);
                var randomI = rand.Next(0, Width - 6);
                var randomJ = rand.Next(0, Height - 6);
                for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    if (i == randomI && j == randomJ) {
                        var sizeX = rand.Next(5, Width - i - 1);
                        var sizeY = rand.Next(5, Height - j - 1);
                        switch (feature) {
                            case 1:
                                CreateColumnHall(i, j, sizeX, sizeY);
                                break;
                            case 2:
                                CreateMegaRoom(i, j, sizeX, sizeY);
                                break;
                            case 3:
                                CreateBox(i, j, sizeX, sizeY);
                                break;
                            case 4:
                                CreateCorridorVertical();
                                break;
                            case 5:
                                CreateCorridorHorizontal();
                                break;
                        }
                    }
                numberOfFeatures--;
            }

            //the we need to replace walls and empty spaces with different blocks, spawn keys, doors,+enemies
            //here i work with out place(walls and their surroundings)
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (matrix[i, j].EntityType == 5) {
                    // i make wal sand but only if it doesnt border wit empty space
                    //so player just cant get out the map so easily
                    matrix[i, j] = fillOneTitle(i, j, 2);
                    if (i + 1 < Width && (matrix[i + 1, j].EntityType == 1 || matrix[i + 1, j].EntityType == 7 ||
                                          matrix[i + 1, j].EntityType == 9))
                        matrix[i, j] = fillOneTitle(i, j, 5);
                    else if (i - 1 >= 0 && (matrix[i - 1, j].EntityType == 1 || matrix[i - 1, j].EntityType == 7 ||
                                            matrix[i - 1, j].EntityType == 9))
                        matrix[i, j] = fillOneTitle(i, j, 5);
                    else if (j + 1 < Height &&
                             (matrix[i, j + 1].EntityType == 1 || matrix[i, j + 1].EntityType == 7 ||
                              matrix[i, j + 1].EntityType == 9))
                        matrix[i, j] = fillOneTitle(i, j, 5);
                    else if (j - 1 >= 0 && (matrix[i, j - 1].EntityType == 1 || matrix[i, j - 1].EntityType == 7 ||
                                            matrix[i, j - 1].EntityType == 9))
                        matrix[i, j] = fillOneTitle(i, j, 5);
                }

            
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //create enemies in random points
            while (WalkersCount > 0) {
                var posX = rand.Next(width);
                var posY = rand.Next(height);
                if (matrix[posX, posY].EntityType == 7) {
                    var enemy = new EnemyWalker(posX, posY, getLevel, getPlayerPositionX,
                        getPlayerPositionY, substractPlayerHp);
                    matrix[posX, posY] = enemy;
                    WalkersCount--;
                }
            }
            
            while (DiggersCount > 0) {
                var posX = rand.Next(width);
                var posY = rand.Next(height);
                if (matrix[posX, posY].EntityType == 7) {
                    var enemy = new EnemyDigger(posX, posY, getLevel, getPlayerPositionX,
                        getPlayerPositionY, substractPlayerHp);
                    matrix[posX, posY] = enemy;
                    DiggersCount--;
                }
            }
            
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

            // here i fill the rest of empty space that was created be rooms 
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (matrix[i, j].EntityType == 7) {
                    var pool = new List<int> {
                        //this values represent titles and probability of spawn
                        8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
                        3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                        4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                        7, 7,
                        12,
                        20,
                        21,
                        22,
                        23,
                    };
                    matrix[i, j] = fillOneTitle(i, j, innerEntitySpawner(pool));
                }
                else if (matrix[i, j].EntityType == 1) {
                    var pool = new List<int> {
                        //this values represent titles and probability of spawn
                        8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
                        1, 1, 1, 1, 1, 1, 1,
                        4, 4, 4, 4, 4,
                        7, 7,
                        12
                    };
                    matrix[i, j] = fillOneTitle(i, j, innerEntitySpawner(pool));
                }
                else if (matrix[i, j].EntityType == 101) {
                    matrix[i, j] = fillOneTitle(i, j, 1);
                }

            var player = new Player(startPosX, startPosY, playerName,
                getLevel, win, lose, DiamondsQuantity);
            matrix[startPosX, startPosY] = player;
            setPlayer(player);
        }

        private void RandomizeSeedValues() {
            var rnd = new Random();
            var roomChancePercents = new List<int> {0, 5, 10, 100};
            _createRoomChance = roomChancePercents[rnd.Next(roomChancePercents.Count)]; // 0/5/10/100
            _roomChanceGrow = rnd.Next(0, 8); // the bigger the  chance, the bigger open spaces on level will be
            _diggerMovesLower = rnd.Next(10, 20);
            _diggerMovesUpper = rnd.Next(30, 50);
            _createRoomMaxSizeX = rnd.Next(7, 15);
            _createRoomMaxSizeY = rnd.Next(7, 15);
            var levelTypes = new List<string> {"default", "blue", "red"};
            LevelType = levelTypes[rnd.Next(levelTypes.Count)];
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
            var rand = new Random();
            var rnd = rand.Next(0, 100);

            if (rnd <= chance) {
                var roomSizeX = rand.Next(0, maxSizeX);
                var roomSizeY = rand.Next(0, maxSizeY);
                while (!(posX + roomSizeX < Width && posX - roomSizeX >= 0 && posY + roomSizeY <= Height &&
                         posY - roomSizeY >= 0)) {
                    roomSizeX = rand.Next(0, maxSizeX);
                    roomSizeY = rand.Next(0, maxSizeY);
                }
                var startposX = posX - roomSizeX / 2;
                var startposY = posY - roomSizeY / 2;
                for (var i = startposX; i < startposX + roomSizeX; i++)
                for (var j = startposY; j < startposY + roomSizeY; j++)
                    matrix[i, j] = fillOneTitle(i, j, 7);
                _createRoomChance = 0;
            }
        }
        private void straightDig(bool flag, ref int diggerPoxX, ref int diggerPoxY, int randomX, int randomY) {
            if (flag) {
                if (randomX > diggerPoxX)
                    diggerPoxX++;
                else if (randomX < diggerPoxX) diggerPoxX--;
            }
            else {
                if (randomY > diggerPoxY)
                    diggerPoxY++;
                else if (randomY < diggerPoxY) diggerPoxY--;
            }
        }
        private int CheckQuarter(int posY, int posX) {
            if (posX >= 0 && posX < height / 2 && posY >= 0 && posY < width / 2) return 1;
            if (posX >= 0 && posX < height / 2 && posY >= width / 2 && posY < width) return 3;
            if (posX >= height / 2 && posX < height && posY >= 0 && posY < width / 2) return 2;
            if (posX >= height / 2 && posX < height && posY >= width / 2 && posY < width) return 4;
            return 1;
        }
        private void FillQuarterPool(int quarter) {
            _quarterPool.Remove(quarter);
            _quarterPool.AddRange(new List<int> {1, 2, 3, 4});
        }
        private int innerEntitySpawner(List<int> pool) {
            var rand = new Random();
            var randNumber = pool[rand.Next(pool.Count)];
            return randNumber;
        }

        private GameEntity fillOneTitle(int i, int j, Func<Level> getLevel,
            Action<int> changePlayerHp, int entityType) {
            switch (entityType) {
                case 3:
                    var rock = new Rock(i, j, getLevel, changePlayerHp);
                    return rock;
                default:
                    var es = new EmptySpace(i, j);
                    return es;
            }
        }

        private GameEntity fillOneTitle(int i, int j, int entityType) {
            switch (entityType) {
                case 1:
                    var emptySpace = new EmptySpace(i, j);
                    return emptySpace;
                case 2:
                    var sand = new Sand(i, j);
                    return sand;
                case 3:
                    var rock = fillOneTitle(i, j, getLevel,substractPlayerHp, entityType);
                    return rock;
                case 4:
                    var diamond = new Diamond(i, j);
                    return diamond;
                case 5:
                    var wall = new Wall(i, j);
                    return wall;
                case 7:
                    var luckyBox = new LuckyBox(i, j);
                    return luckyBox;
                case 8:
                    var sandTranclucent = new SandTranclucent(i, j);
                    return sandTranclucent;
                case 9:
                    var wood = new Wood(i, j);
                    return wood;
                case 12:
                    var barrel = new BarrelWithSubstance(i, j);
                    return barrel;
                case 20:
                    var sword = new SwordTile(i, j);
                    return sword;
                case 21:
                    var converter = new ConverterTile(i, j);
                    return converter;
                case 22:
                    var tnt = new TntTile(i, j);
                    return tnt;
                case 23:
                    var armor = new ArmorTile(i, j);
                    return armor;
                case 101:
                    var dedicatedEmptySpace = new DedicatedEmptySpace(i, j);
                    return dedicatedEmptySpace;
                default:
                    var es = new EmptySpace(i, j);
                    return es;
            }
        }
        private void CreateColumnHall(int i, int j, int sizeX, int sizeY) {
            var flag = true;
            for (var k = i; k < i + sizeX; k++) {
                for (var l = j; l < j + sizeY; l++)
                    if (l % 2 == 0 && flag)
                        matrix[k, l] = fillOneTitle(k, l, 5);
                    else
                        matrix[k, l] = fillOneTitle(k, l, 1);
                flag = !flag;
            }
        }
        private void CreateMegaRoom(int i, int j, int sizeX, int sizeY) {
            for (var k = i; k < i + sizeX; k++)
            for (var l = j; l < j + sizeY; l++)
                matrix[k, l] = fillOneTitle(k, l, 7);
        }
        private void CreateBox(int i, int j, int sizeX, int sizeY) {
            for (var k = i; k < i + sizeX; k++)
            for (var l = j; l < j + sizeY; l++)
                if (k == i + sizeX - sizeX / 2 && (l == j || l == j + sizeY - 1) ||
                    l == j + sizeY - sizeY / 2 && (k == i || k == i + sizeX - 1))
                    matrix[k, l] = fillOneTitle(k, l, 8);
                else if (k == i || k == i + sizeX - 1 || l == j || l == j + sizeY - 1)
                    matrix[k, l] = fillOneTitle(k, l, 5);
                else
                    matrix[k, l] = fillOneTitle(k, l, 7);
        }
        private void CreateCorridorHorizontal() {
            var rnd = new Random();
            var row = rnd.Next(0, Width);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (i == row)
                    matrix[i, j] = fillOneTitle(i, j, 7);
        }
        private void CreateCorridorVertical() {
            var rnd = new Random();
            var col = rnd.Next(0, Height);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (j == col)
                    matrix[i, j] = fillOneTitle(i, j, 7);
        }
    }
}