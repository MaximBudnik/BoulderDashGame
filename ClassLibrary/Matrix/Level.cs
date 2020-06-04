using System;
using System.Collections.Generic;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Collectable.ItemsTiles;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Generation;
using ClassLibrary.Entities.Player;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary.Matrix {
    public partial class Level : Matrix {
        private readonly Func<Level> _getLevel;
        private readonly Func<int> _getPlayerPositionX;
        private readonly Func<int> _getPlayerPositionY;
        private readonly Action _lose;
        private readonly string _playerName;
        private readonly Action<SoundFilesEnum> _playSound;
        private readonly Action<Player> _setPlayer;
        private readonly Action<int> _substractPlayerHp;
        private readonly Action _win;
        private readonly Action _acidGameLoopAction;
        private int _createRoomChance;
        private int _createRoomMaxSizeX = 10;
        private int _createRoomMaxSizeY = 10;
        private int _difficulty;
        private int _diggerMovesLower = 20;
        private int _diggerMovesUpper = 40;
        private List<int> _quarterPool;
        private int _roomChanceGrow = 2; // the bigger the  chance, the bigger open spaces on level will be
        public Level(int levelName, string playerName,
            Action win, Action lose,
            Func<int> getPlayerPositionX,
            Func<int> getPlayerPositionY,
            Action<int> substractPlayerHp, Action<Player> setPlayer,
            int sizeX, int sizeY, int difficulty, Action<SoundFilesEnum> playSound, Action acidGameLoopAction) {
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

            SetDifficulty(difficulty);

            matrix = new GameEntity[width, height];
            DiggerAlgorithm();
        }
        public int DiamondsQuantityToWin { get; private set; }
        public int LevelName { get; }
        public string Aim { get; } = "Collect diamonds";
        private int WalkersCount { get; set; }
        private int DiggersCount { get; set; }

        //fields for creating level
        private void SetDifficulty(int difficulty) {
            _difficulty = difficulty;
            WalkersCount = difficulty;
            DiggersCount = difficulty / 2;
            DiamondsQuantityToWin = difficulty * 2;
        }
    }

    
}