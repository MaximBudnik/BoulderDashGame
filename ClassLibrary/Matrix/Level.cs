using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Matrix {
    public class Level : Matrix {
        private readonly Func<Level> _getLevel;
        private readonly Func<int> _getPlayerPositionX;
        private readonly Func<int> _getPlayerPositionY;
        private readonly Action _lose;
        private readonly string _playerName;
        private readonly Action<Player> _setPlayer;
        private readonly Action<int> _substractPlayerHp;
        private readonly Action _win;
        private int _createRoomChance;
        private int _createRoomMaxSizeX = 10;
        private int _createRoomMaxSizeY = 10;
        private int _difficulty;
        private int _diggerMovesLower = 20;
        private int _diggerMovesUpper = 40;
        private List<int> _quarterPool;
        private int _roomChanceGrow = 5; // the bigger the  chance, the bigger open spaces on level will be
        public Level(int levelName, string playerName,
            Func<Level> getLevel,
            Action win, Action lose,
            Func<int> getPlayerPositionX,
            Func<int> getPlayerPositionY,
            Action<int> substractPlayerHp, Action<Player> setPlayer,
            int sizeX, int sizeY, int difficulty
        ) {
            width = sizeX; //20 for console
            height = sizeY; //65 for console
            LevelName = levelName;
            _playerName = playerName;
            _getLevel = getLevel;
            _win = win;
            _lose = lose;
            _getPlayerPositionX = getPlayerPositionX;
            _getPlayerPositionY = getPlayerPositionY;
            _substractPlayerHp = substractPlayerHp;
            _setPlayer = setPlayer;

            SetDifficulty(difficulty);

            matrix = new GameEntity[width, height];
            DiggerAlgorithm();
        }
        public int DiamondsQuantity { get; set; }
        public int LevelName { get; }
        public string Aim { get; } = "Collect diamonds";
        private int WalkersCount { get; set; }
        private int DiggersCount { get; set; }

        //fields for creating level
        public string LevelType { get; private set; } = "red";
        private void SetDifficulty(int difficulty) {
            _difficulty = difficulty;
            WalkersCount = difficulty;
            DiggersCount = difficulty / 2;
            DiamondsQuantity = difficulty * 2;
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
            //5) quarters are created to make sure that digger will be in all parts of field     
            //12 - quarters pos
            //34
            var currentQuarter = CheckQuarter(diggerPoxX, diggerPoxY);
            //From quarter pool the next quarter(from which the next point will be chosen) is chosen
            _quarterPool = new List<int> {
                currentQuarter
            };
            //6) loop that performs all operations 
            MainGeneration(diggerMoves, rand, flag, diggerPoxX, diggerPoxY);
            //secondary generation
            //TODO: optimize following processes
            // here i fill matrix with hand-made objects
            SecondaryGeneration(rand);
            //the we need to replace walls and empty spaces with different blocks, spawn keys, doors,+enemies
            //here i work with out place(walls and their surroundings)
            SetWalls();
            //create enemies in random points
            CreateEnemies(rand);
            // here i fill the rest of empty space that was created be rooms 
            FillEmptySpace();

            var player = new Player(startPosX, startPosY, _playerName,
                _getLevel, _win, _lose, DiamondsQuantity);
            player.ScoreMultiplier = _difficulty;
            matrix[startPosX, startPosY] = player;
            _setPlayer(player);
        }
        private void FillEmptySpace() {
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
                        23
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
        }
        private void CreateEnemies(Random rand) {
            while (WalkersCount > 0) {
                var posX = rand.Next(width);
                var posY = rand.Next(height);
                if (matrix[posX, posY].EntityType == 7) {
                    var enemy = new EnemyWalker(posX, posY, _getLevel, _getPlayerPositionX,
                        _getPlayerPositionY, _substractPlayerHp);
                    matrix[posX, posY] = enemy;
                    WalkersCount--;
                }
            }

            while (DiggersCount > 0) {
                var posX = rand.Next(width);
                var posY = rand.Next(height);
                if (matrix[posX, posY].EntityType == 7) {
                    var enemy = new EnemyDigger(posX, posY, _getLevel, _getPlayerPositionX,
                        _getPlayerPositionY, _substractPlayerHp);
                    matrix[posX, posY] = enemy;
                    DiggersCount--;
                }
            }
        }
        private void SetWalls() {
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
        }
        private void SecondaryGeneration(Random rand) {
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
        }
        private void MainGeneration(int diggerMoves, Random rand, bool flag, int diggerPoxX, int diggerPoxY) {
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
                var startPosX = posX - roomSizeX / 2;
                var startPosY = posY - roomSizeY / 2;
                for (var i = startPosX; i < startPosX + roomSizeX; i++)
                for (var j = startPosY; j < startPosY + roomSizeY; j++)
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
                    var rock = fillOneTitle(i, j, _getLevel, _substractPlayerHp, entityType);
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