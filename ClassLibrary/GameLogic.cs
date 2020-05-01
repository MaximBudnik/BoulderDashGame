using System;
using System.Collections.Generic;
using System.Threading;
using ClassLibrary.ConsoleInterface;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Matrix;

namespace ClassLibrary {
    public class GameLogic {
        private GameInterface _gameInterface;
        private RockProcessor _rockProcessor;
        private AfterLevelScreen _afterLevelScreen;
        private Level _currentLevel;
        public Player Player;
        private int _frameCounter;
        private int _endScreen;
        public List<EnemyWalker> LevelEnemyWalkers;
        public save CurrentSave = null;

        private void SubstractPlayerHp(int value) {
            Player.Hp -= value;
        }

        public void CreateLevel(int levelName, string playerName) {
            _endScreen = 0;
            _frameCounter = 0;
            _gameInterface = new GameInterface();
            _rockProcessor = new RockProcessor(() => _currentLevel, DrawLevel, () => Player.PositionX,
                () => Player.PositionY, UpdatePlayerInterface, SubstractPlayerHp);
            _afterLevelScreen = new AfterLevelScreen();
            LevelEnemyWalkers = new List<EnemyWalker>();
            _currentLevel = new Level(
                levelName, playerName,
                () => _currentLevel,
                DrawLevel,
                UpdateUpperInterface,
                UpdatePlayerInterface,
                (i, j, direction, value) => _rockProcessor.PushRock(i, j, direction, value),
                Win,
                Lose,
                () => Player.PositionX,
                () => Player.PositionY,
                SubstractPlayerHp
            );
        }

        private void DrawLevel() {
            _gameInterface.NewDraw(() => _currentLevel);
        }

        private void UpdatePlayerInterface() {
            _gameInterface.DrawPlayerInterface(_currentLevel.DiamondsQuantity, Player.CollectedDiamonds,
                Player.MaxEnergy, Player.Energy, Player.MaxHp, Player.Hp, Player.Name);
        }
        private void UpdateUpperInterface() {
            _gameInterface.DrawUpperInterface(_currentLevel.LevelName, Player.Score, _currentLevel.Aim);
        }

        public void GameLoop() {
            if (_endScreen == 0) {
                if (_frameCounter == 0) {
                    UpdateUpperInterface();
                    UpdatePlayerInterface();
                    DrawLevel();
                }

                Player.GameLoopAction();
                if (_frameCounter % 10 == 0) //processing enemies
                    for (var i = 0; i < LevelEnemyWalkers.Count; i++)
                        LevelEnemyWalkers[i].GameLoopAction();

                _rockProcessor.ProcessRock();
                if (_frameCounter == 100) _frameCounter = 0;
                _frameCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
            }
            else if (_endScreen == 1) {
                _afterLevelScreen.DrawGameLose();
            }
            else if (_endScreen == 2) {
                _afterLevelScreen.DrawGameWin(Player.Score, Player.AllScores);
            }
        }

        private void Win() {
            _endScreen = 2;
            _afterLevelScreen.DrawGameWin(Player.Score, Player.AllScores);
            GameEngine.DataInterlayer.ChangeGameSave(CurrentSave, _currentLevel.LevelName, Player.Score);
            Thread.Sleep(3000);
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Enter)
                GameEngine.ResumeGame(CurrentSave);
            else
                GameEngine.ChangeIsGame();
        }

        private void Lose() {
            _endScreen = 1;
            GameEngine.DataInterlayer.addBestScore(Player.Name, Player.Score);
            GameEngine.DataInterlayer.DeleteGameSave(CurrentSave.id);
            GameEngine.DataInterlayer.GetGameSaves();
            _afterLevelScreen.DrawGameLose();
            GameEngine.ChangeIsGame();
            Thread.Sleep(3000);
            Console.ReadKey();
        }
    }
}