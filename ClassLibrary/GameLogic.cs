using System;
using System.Collections.Generic;
using ClassLibrary.Entities;

namespace ClassLibrary {
    public class GameLogic {
        private GameInterface _gameInterface = new GameInterface();
        private Level _currentLevel;
        private Player _player;
        private int _frameCounter = 0;
        
        public Player Player {
            get => _player;
        }

        public Level CurrentLevel {
            get => _currentLevel;
            set => _currentLevel = value;
        }
        //initialize other fields

        public delegate Level GiGetCurrentLevel();

        // public delegate Player GipPlayer();

        public Level GetCurrentLevel() {
            return _currentLevel;
        }

        public void CreateLevel(string levelName) {
            _currentLevel = new Level(levelName);
            _player = new Player(_currentLevel.defaultPlayerPosition); //TODO: clown theme. Refactor #1
        }

        public void drawLevel() {
            GiGetCurrentLevel GiGetCurrentLevel = delegate { return GetCurrentLevel(); };
            _gameInterface.Draw(GiGetCurrentLevel);
        }

        public void GameLoop() {
            _frameCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
            

            if (_frameCounter == 0) {
                drawLevel();
            }
            if (_frameCounter % 5 == 0) {
                processRocks();
                drawLevel();
            }
            if (_frameCounter == 100) {
                _frameCounter = 0;
            }
        }

        public void processRocks() {// TODO: i think all 2x loops can be replaced with method + this method must be in entity.rock class
            List<int[]> falledRocks = new List<int[]>();
            for (int i = 0; i < _currentLevel.Width; i++) {
                for (int j = 0; j <_currentLevel.Height; j++) {
                    
                    int[] currentArray = {i, j};
                    
                    if (_currentLevel[i, j] == 3 && i + 1 < _currentLevel.Width && _currentLevel[i + 1, j] == 1 && !falledRocks.Contains(currentArray)) {
                        _currentLevel[i, j] = 1;
                        int[] ArrayOfInts = {i + 1, j};
                        falledRocks.Add(ArrayOfInts);
                    _currentLevel[i + 1, j] = 3;
                    }
                    
                }
            }
        }

        public void moveEntity(Movable entity, int positionX, int positionY, int value, string direction) {
            
        }
    }
}