using System;
using System.Collections.Generic;
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
        private Player _player;
        private int _frameCounter;
        private int _endScreen;
        public List<EnemyWalker> LevelEnemyWalkers;

        public int FrameCounter => _frameCounter;

        public Player Player => _player;

        public Level CurrentLevel => _currentLevel;

        public RockProcessor RockProcessor => _rockProcessor;

        public delegate Level GiGetCurrentLevel();

        private Level GetCurrentLevel() {
            return _currentLevel;
        }

        public void CreateLevel(string levelName) {
            _endScreen= 0;
            _frameCounter = 0;
            _gameInterface = new GameInterface();
            _rockProcessor = new RockProcessor();
            _afterLevelScreen= new AfterLevelScreen();
            LevelEnemyWalkers = new List<EnemyWalker>();
            _currentLevel = new Level(levelName);
            _player = new Player(_currentLevel.DefaultPlayerPosition); //TODO: clown theme. Refactor #1
        }

        public void DrawLevel() {
            GiGetCurrentLevel GiGetCurrentLevel = GetCurrentLevel;
            _gameInterface.NewDraw(GiGetCurrentLevel);
        }

        public void UpdatePlayerInterface() {
            _gameInterface.DrawPlayerInterface(CurrentLevel.DiamondsQuantity ,_player.CollectedDiamonds,
                _player.MaxEnergy,_player.Energy, _player.MaxHp, _player.Hp);
        }
        public void UpdateUpperInterface() {
            _gameInterface.DrawUpperInterface("Random level",2150,"Collect all diamonds");
        }
        
        public void GameLoop() {
            if (_endScreen == 0) {
                if (_frameCounter == 0) {
                    UpdateUpperInterface();
                    UpdatePlayerInterface();
                    DrawLevel();
                }
            
                _player.GameLoopAction();
                if (_frameCounter%10==0) {//processing enemies
                    for (int i = 0; i < LevelEnemyWalkers.Count; i++) {
                        LevelEnemyWalkers[i].GameLoopAction();
                    }
                }
                
                _rockProcessor.ProcessRock();
                if (_frameCounter == 100) {
                    _frameCounter = 0;
                }
                _frameCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
            }
            else if(_endScreen == 1) {
                _afterLevelScreen.DrawGameLose();
            }
            else if(_endScreen == 2) {
                _afterLevelScreen.DrawGameWin();
            }
        }

        public void Win() {
            _endScreen = 2;
            _afterLevelScreen.DrawGameWin();
        }

        public void Lose() {
            _endScreen = 1;
            _afterLevelScreen.DrawGameLose();
        }
        
    }
}