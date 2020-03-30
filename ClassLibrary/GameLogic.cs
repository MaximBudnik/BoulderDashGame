using System;
using System.Collections.Generic;
using ClassLibrary.Entities;

namespace ClassLibrary {
    public class GameLogic {
        private GameInterface _gameInterface = new GameInterface();
        private RockProcessor _rockProcessor = new RockProcessor(); 
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
        public RockProcessor RockProcessor {
            get => _rockProcessor;
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

        // public void drawPart(int positionX, int positionY) {
        //     GiGetCurrentLevel GiGetCurrentLevel = delegate { return GetCurrentLevel(); };
        //     _gameInterface.DrawPart(GiGetCurrentLevel, positionX, positionY);
        // }

        public void GameLoop() {
            if (_frameCounter == 0) {
                drawLevel();
            }
            _rockProcessor.ProcessRock();
            if (_frameCounter == 100) {
                _frameCounter = 0;
            }
            _frameCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
        }

        public void processRocks() {

        }

        public void moveEntity(Movable entity, int positionX, int positionY, int value, string direction) { }
    }
}