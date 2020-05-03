using System;
using System.Collections.Generic;
using System.Threading;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Matrix;

namespace ClassLibrary {
    public class GameLogic {
        private RockProcessor _rockProcessor;
        public Level CurrentLevel { get; private set; }
        public Player Player { get; private set; }
        private int _gameTickCounter;
        private List<EnemyWalker> _levelEnemyWalkers;
        public Save CurrentSave = null;

        private Action<int> _changeGameStatus;
        private Func<DataInterlayer> _getDataLayer;
        
        private void SubstractPlayerHp(int value) {
            Player.Hp -= value;
        }

        private void setPlayer(Player pl) {
            Player = pl;
        }

        public void CreateLevel(int levelName, string playerName, Action<int> changeGameStatus, Func<DataInterlayer> getDataLayer) {
            _changeGameStatus = changeGameStatus;
            _getDataLayer = getDataLayer;
            _gameTickCounter = 0;
            _rockProcessor = new RockProcessor(() => CurrentLevel, () => Player.PositionX,
                () => Player.PositionY, SubstractPlayerHp);
            _levelEnemyWalkers = new List<EnemyWalker>();
            CurrentLevel = new Level(
                levelName, playerName,
                () => CurrentLevel,
                (i, j, direction, value) => _rockProcessor.PushRock(i, j, direction, value),
                Win,
                Lose,
                () => Player.PositionX,
                () => Player.PositionY,
                SubstractPlayerHp, 
                _levelEnemyWalkers,
                setPlayer
            );
        }

        public void GameLoop() {
            Player.GameLoopAction();
            foreach (var enemy in _levelEnemyWalkers)
                enemy.GameLoopAction();
            _rockProcessor.ProcessRock();
            if (_gameTickCounter == 100) _gameTickCounter = 0;
            _gameTickCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
        }

        private void Win() {
            _changeGameStatus(2);
            Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            dataInterlayer.ChangeGameSave(CurrentSave, CurrentLevel.LevelName, Player.Score);
            _changeGameStatus(0);
            Console.ReadLine();
        }

        private void Lose() {
            _changeGameStatus(3);
            Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            dataInterlayer.AddBestScore(Player.Name, Player.Score);
            dataInterlayer.DeleteGameSave(CurrentSave.Id);
            _changeGameStatus(0);
            Console.ReadLine();
        }
    }
}