using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Enemies.SmartEnemies;
using ClassLibrary.Entities.Generation;
using ClassLibrary.Entities.Player;

namespace ClassLibrary.Matrix {
    public partial class Level {
        private void DiggerAlgorithm() {
            //in general generation depends on:
            //    _roomChanceGrow
            //    diggerMoves
            //     9), where size of setted rooms are setted
            //    secondary generation
            //1) All field is filled with walls
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.FutureWallsAndSand);
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
            var isMoveHorizontal = false;
            //5) quarters are created to make sure that digger will be in all parts of field     
            //12 - quarters pos
            //34
            var currentQuarter = CheckQuarter(diggerPoxX, diggerPoxY);
            //From quarter pool the next quarter(from which the next point will be chosen) is chosen
            _quarterPool = new List<int> {
                currentQuarter
            };
            //6) loop that performs all operations 
            MainGeneration(diggerMoves, rand, isMoveHorizontal, diggerPoxX, diggerPoxY);
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
                _getLevel, _win, _lose, _playSound, _setPlayer, DiamondsQuantityToWin) {ScoreMultiplier = _difficulty};
            matrix[startPosX, startPosY] = player;
            _setPlayer(player);
        }
        private void FillEmptySpace() {
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (matrix[i, j].EntityEnumType == GameEntitiesEnum.FutureRooms) {
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
                    matrix[i, j] = FillOneTitle(i, j, (GameEntitiesEnum) Randomizer.GetRandomFromList(pool));
                }
                else if (matrix[i, j].EntityEnumType == GameEntitiesEnum.FutureCorridors) {
                    var pool = new List<int> {
                        //this values represent titles and probability of spawn
                        8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
                        1, 1, 1, 1, 1, 1, 1,
                        4, 4, 4, 4, 4,
                        7, 7,
                        12
                    };
                    matrix[i, j] = FillOneTitle(i, j, (GameEntitiesEnum) Randomizer.GetRandomFromList(pool));
                }
                else if (matrix[i, j].EntityEnumType == GameEntitiesEnum.DedicatedEmptySpace) {
                    matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.EmptySpace);
                }
        }
        private void CreateEnemies(Random rand) {
            while (WalkersCount > 0) {
                var posX = rand.Next(width);
                var posY = rand.Next(height);
                if (matrix[posX, posY].EntityEnumType == GameEntitiesEnum.FutureRooms) {
                    var enemy = new EnemyWalker(posX, posY, _getLevel, _getPlayerPositionX,
                        _getPlayerPositionY, _substractPlayerHp,_getOutdatedPlayer);
                    matrix[posX, posY] = enemy;
                    WalkersCount--;
                }
            }
            
            //TODO: not forget enable this

            // while (DiggersCount > 0) {
            //     var posX = rand.Next(width);
            //     var posY = rand.Next(height);
            //     if (matrix[posX, posY].EntityEnumType == GameEntitiesEnum.FutureRooms) {
            //         var enemy = new EnemyDigger(posX, posY, _getLevel, _getPlayerPositionX,
            //             _getPlayerPositionY, _substractPlayerHp);
            //         matrix[posX, posY] = enemy;
            //         DiggersCount--;
            //     }
            // }
        }
        private void SetWalls() {
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (matrix[i, j].EntityEnumType == GameEntitiesEnum.FutureWallsAndSand) {
                    // i make wal sand but only if it doesnt border wit empty space
                    //so player just cant get out the map so easily
                    matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.Sand);
                    if (i + 1 < Width && (matrix[i + 1, j].EntityEnumType == GameEntitiesEnum.FutureCorridors ||
                                          matrix[i + 1, j].EntityEnumType == GameEntitiesEnum.FutureRooms))
                        matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.Wall);
                    else if (i - 1 >= 0 && (matrix[i - 1, j].EntityEnumType == GameEntitiesEnum.FutureCorridors ||
                                            matrix[i - 1, j].EntityEnumType == GameEntitiesEnum.FutureRooms))
                        matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.Wall);
                    else if (j + 1 < Height &&
                             (matrix[i, j + 1].EntityEnumType == GameEntitiesEnum.FutureCorridors ||
                              matrix[i, j + 1].EntityEnumType == GameEntitiesEnum.FutureRooms))
                        matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.Wall);
                    else if (j - 1 >= 0 && (matrix[i, j - 1].EntityEnumType == GameEntitiesEnum.FutureCorridors ||
                                            matrix[i, j - 1].EntityEnumType == GameEntitiesEnum.FutureRooms))
                        matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.Wall);
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
        private void MainGeneration(int diggerMoves, Random rand, bool isMoveHorizontal, int diggerPosX,
            int diggerPosY) {
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
                var pathfinder = new Pathfinder();
                var path = pathfinder.FindPath(diggerPosX, diggerPosY, randomX, randomY, this, (l, p) => true);

                //9) digger starts moving

                while (path.Count > 0) {
                    //making previous tile empty space
                    matrix[diggerPosX, diggerPosY] =
                        FillOneTitle(diggerPosX, diggerPosY, GameEntitiesEnum.FutureCorridors);
                    //moves next tile
                    diggerPosX = path.First().X;
                    diggerPosY = path.First().Y;
                    //has a chance to create new room
                    CreateRoom(_createRoomChance, diggerPosX, diggerPosY, _createRoomMaxSizeX, _createRoomMaxSizeY);
                    //chance of creating room is always growing
                    _createRoomChance += _roomChanceGrow;
                    path.Remove(path.First());
                }
                //after digger reaches end point, he creates room
                CreateRoom(100, diggerPosX, diggerPosY);
                isMoveHorizontal = !isMoveHorizontal;
            }
        }
        private void RandomizeSeedValues() {
            var rnd = new Random();
            var roomChancePercents = new List<int> {0, 5, 10, 100};
            _createRoomChance = roomChancePercents[rnd.Next(roomChancePercents.Count)];
            _roomChanceGrow = rnd.Next(0, 4); // the bigger the  chance, the bigger open spaces on level will be
            _diggerMovesLower = rnd.Next(10, 20);
            _diggerMovesUpper = rnd.Next(30, 40);
            _createRoomMaxSizeX = rnd.Next(7, 13);
            _createRoomMaxSizeY = rnd.Next(7, 13);
        }
        private bool CheckMoved(bool isMoveHorizontal, int digX, int digY, int targetX, int targetY) {
            if (isMoveHorizontal) return digX != targetX;
            return digY != targetY;
        }
        private void CreateRoom(int chance, int posX, int posY, int maxSizeX = 20, int maxSizeY = 8) {
            var rand = new Random();
            var rnd = rand.Next(0, 100);

            if (rnd > chance) return;
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
                matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.FutureRooms);
            _createRoomChance = 0;
        }
        private void StraightDig(bool isMoveHorizontal, ref int diggerPosX, ref int diggerPosY, int randomX,
            int randomY) {
            if (isMoveHorizontal) {
                if (randomX > diggerPosX)
                    diggerPosX++;
                else if (randomX < diggerPosX) diggerPosX--;
            }
            else {
                if (randomY > diggerPosY)
                    diggerPosY++;
                else if (randomY < diggerPosY) diggerPosY--;
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
        private GameEntity FillOneTitle(int i, int j, Func<Level> getLevel,
            Action<int> changePlayerHp, GameEntitiesEnum entityType) {
            switch (entityType) {
                case GameEntitiesEnum.Rock:
                    var rock = new Rock(i, j, getLevel, changePlayerHp);
                    return rock;
                default:
                    var es = new EmptySpace(i, j);
                    return es;
            }
        }
        private GameEntity FillOneTitle(int i, int j, GameEntitiesEnum entityType) {
            switch (entityType) {
                case GameEntitiesEnum.EmptySpace:
                    return new EmptySpace(i, j);
                case GameEntitiesEnum.Sand:
                    return new Sand(i, j);
                case GameEntitiesEnum.Rock:
                    return FillOneTitle(i, j, _getLevel, _substractPlayerHp, entityType);
                case GameEntitiesEnum.Diamond:
                    return new Diamond(i, j, _playSound);
                case GameEntitiesEnum.Wall:
                    return new Wall(i, j);
                case GameEntitiesEnum.LuckyBox:
                    return new LuckyBox(i, j, _playSound);
                case GameEntitiesEnum.SandTranslucent:
                    return new SandTranclucent(i, j);
                case GameEntitiesEnum.Barrel:
                    return new BarrelWithSubstance(i, j, _acidGameLoopAction, _getLevel, _substractPlayerHp,
                        _playSound);
                case GameEntitiesEnum.SwordTile:
                    return new SwordTile(i, j, _playSound);
                case GameEntitiesEnum.ConverterTile:
                    return new ConverterTile(i, j, _playSound);
                case GameEntitiesEnum.DynamiteTile:
                    return new DynamiteTile(i, j, _playSound);
                case GameEntitiesEnum.ArmorTile:
                    return new ArmorTile(i, j, _playSound);
                case GameEntitiesEnum.DedicatedEmptySpace:
                    return new DedicatedEmptySpace(i, j);
                case GameEntitiesEnum.FutureCorridors:
                    return new FutureCorridors(i, j);
                case GameEntitiesEnum.FutureRooms:
                    return new FutureRooms(i, j);
                case GameEntitiesEnum.FutureWallsAndSand:
                    return new FutureWallsAndSand(i, j);
                default:
                    throw new Exception("Unknown EntityType in generation");
            }
        }
        private void CreateColumnHall(int i, int j, int sizeX, int sizeY) {
            var willWallBePlaced = true;
            for (var k = i; k < i + sizeX; k++) {
                for (var l = j; l < j + sizeY; l++)
                    if (l % 2 == 0 && willWallBePlaced)
                        matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.Wall);
                    else
                        matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.FutureRooms);
                willWallBePlaced = !willWallBePlaced;
            }
        }
        private void CreateMegaRoom(int i, int j, int sizeX, int sizeY) {
            for (var k = i; k < i + sizeX; k++)
            for (var l = j; l < j + sizeY; l++)
                matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.FutureRooms);
        }
        private void CreateBox(int i, int j, int sizeX, int sizeY) {
            for (var k = i; k < i + sizeX; k++)
            for (var l = j; l < j + sizeY; l++)
                if (k == i + sizeX - sizeX / 2 && (l == j || l == j + sizeY - 1) ||
                    l == j + sizeY - sizeY / 2 && (k == i || k == i + sizeX - 1))
                    matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.SandTranslucent);
                else if (k == i || k == i + sizeX - 1 || l == j || l == j + sizeY - 1)
                    matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.Wall);
                else
                    matrix[k, l] = FillOneTitle(k, l, GameEntitiesEnum.FutureRooms);
        }
        private void CreateCorridorHorizontal() {
            var rnd = new Random();
            var row = rnd.Next(0, Width);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (i == row)
                    matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.FutureCorridors);
        }
        private void CreateCorridorVertical() {
            var rnd = new Random();
            var col = rnd.Next(0, Height);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (j == col)
                    matrix[i, j] = FillOneTitle(i, j, GameEntitiesEnum.FutureCorridors);
        }
    }
}