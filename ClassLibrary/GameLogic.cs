using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities.Basic;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;

namespace ClassLibrary {
    public class GameLogic {
        private RockProcessor _rockProcessor;
        public Level CurrentLevel { get; private set; }
        public Player Player { get; private set; }
        private int _gameTickCounter;
        private List<EnemyWalker> _levelEnemyWalkers;
        private List<StoneInDiamondConverter> _stonesInDiamondsConverters;
        private List<Acid> _acidBlocksList;
        private int _chanceToClearAcidBlocksList = 0;
        public Save CurrentSave = null;

        private Action<int> _changeGameStatus;
        private Func<DataInterlayer> _getDataLayer;

        private void SetPlayer(Player pl) {
            Player = pl;
        }

        private void SubstractPlayerHp(int i) {
            Player.SubstractPlayerHp(i);
        }

        public void CreateLevel(int levelName, string playerName, Action<int> changeGameStatus,
            Func<DataInterlayer> getDataLayer) {
            _changeGameStatus = changeGameStatus;
            _getDataLayer = getDataLayer;
            _gameTickCounter = 0;
            _rockProcessor = new RockProcessor(() => CurrentLevel, () => Player.PositionX,
                () => Player.PositionY, SubstractPlayerHp);
            _levelEnemyWalkers = new List<EnemyWalker>();
            _stonesInDiamondsConverters = new List<StoneInDiamondConverter>();
            _acidBlocksList = new List<Acid>();
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
                _stonesInDiamondsConverters,
                _acidBlocksList,
                SetPlayer
            );
        }

        public void GameLoop() {
            Player.GameLoopAction();
            foreach (var enemy in _levelEnemyWalkers.ToList())
                enemy.GameLoopAction();
            foreach (var converter in _stonesInDiamondsConverters.ToList()) {
                converter.GameLoopAction();
            }
            foreach (var acidBlock in _acidBlocksList.ToList()) {
                acidBlock.GameLoopAction();
            }

            if (_acidBlocksList.Count > 0) {
                _chanceToClearAcidBlocksList += _acidBlocksList.Count / 2;
                if (_chanceToClearAcidBlocksList >= Randomizer.Random(100)) {
                    foreach (var acidBlock in _acidBlocksList)
                        CurrentLevel[acidBlock.PositionX, acidBlock.PositionY] =
                            new Rock(acidBlock.PositionX, acidBlock.PositionY);
                    _acidBlocksList.Clear();
                    _chanceToClearAcidBlocksList = 0;
                }
            }

            _rockProcessor.GameLoopAction();
            if (_gameTickCounter == 100) _gameTickCounter = 0;
            _gameTickCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
        }

        private void Win() {
            _changeGameStatus(2);
            Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            CurrentSave.LevelName = CurrentLevel.LevelName;
            CurrentSave.Score = Player.Score;
            CurrentSave.LevelName += 1;
            dataInterlayer.ChangeGameSave(CurrentSave);
            _changeGameStatus(0);
            Console.ReadLine();
        }

        private void Lose() {
            _changeGameStatus(3);
            Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            dataInterlayer.AddBestScore(Player.Name, Player.Score);
            dataInterlayer.DeleteGameSave(CurrentSave);
            _changeGameStatus(0);
            Console.ReadLine();
        }
    }
}