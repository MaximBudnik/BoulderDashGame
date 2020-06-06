using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Player;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Matrix {
    public partial class Level : Matrix {
        private readonly Action _acidGameLoopAction;

        public readonly int _chanceToSpawnSmartDevil = 10;
        public readonly int _chanceToSpawnSmartDigger = 65;
        public readonly int _chanceToSpawnSmartPeaceful = 50;
        public readonly int _chanceToSpawnSmartSkeleton = 30;
        public readonly int _chanceToSpawnWalker = 100;
        private readonly Func<Level> _getLevel;
        private readonly Func<Player> _getOutdatedPlayer;
        private readonly Func<int> _getPlayerPositionX;
        private readonly Func<int> _getPlayerPositionY;
        private readonly Action _lose;
        private readonly string _playerName;
        private readonly Action<SoundFilesEnum> _playSound;
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
        private int _roomChanceGrow = 2; // the bigger the  chance, the bigger open spaces on level will be
        public Level(
            int levelName, string playerName,
            Action win, Action lose,
            Func<int> getPlayerPositionX,
            Func<int> getPlayerPositionY,
            Action<int> substractPlayerHp, Action<Player> setPlayer,
            int sizeX, int sizeY, int difficulty, Action<SoundFilesEnum> playSound, Action acidGameLoopAction,
            Func<Player> getOutdatedPlayer
        ) {
            width = sizeX; //20 for console
            height = sizeY; //65 for console
            LevelName = levelName;
            _playerName = playerName;
            _getLevel = () => this;
            _win = win;
            _lose = lose;
            _getPlayerPositionX = getPlayerPositionX;
            _getPlayerPositionY = getPlayerPositionY;
            _substractPlayerHp = substractPlayerHp;
            _setPlayer = setPlayer;
            _playSound = playSound;
            _acidGameLoopAction = acidGameLoopAction;
            _getOutdatedPlayer = getOutdatedPlayer;

            SetDifficulty(difficulty);

            matrix = new GameEntity[width, height];
            DiggerAlgorithm();
        }
        public int DiamondsQuantityToWin { get; private set; }
        public int LevelName { get; }
        public string Aim { get; } = "Collect diamonds";

        private int SmartDevilCount { get; set; }
        private int SmartSkeletonsCount { get; set; }
        private int SmartPeacefulCount { get; set; }
        private int DiggersCount { get; set; }
        private int WalkersCount { get; set; }

        //fields for creating level
        private void SetDifficulty(int difficulty) {
            DiamondsQuantityToWin = difficulty * 3;
            _difficulty = difficulty;

            while (difficulty > 0) {
                var chanceToSpawn = Randomizer.Random(101);
                if (_chanceToSpawnSmartDevil > chanceToSpawn) SmartDevilCount++;
                else if (_chanceToSpawnSmartSkeleton > chanceToSpawn) SmartSkeletonsCount++;
                else if (_chanceToSpawnSmartPeaceful > chanceToSpawn) SmartPeacefulCount++;
                else if (_chanceToSpawnSmartDigger > chanceToSpawn) DiggersCount++;
                else if (_chanceToSpawnWalker > chanceToSpawn) WalkersCount++;
                difficulty--;
            }
        }
    }
}