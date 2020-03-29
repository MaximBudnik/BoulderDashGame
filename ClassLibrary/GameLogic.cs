using System;
using ClassLibrary.Entities;

namespace ClassLibrary {
    public class GameLogic {
        private GameInterface _gameInterface = new GameInterface();
        private Level _currentLevel;
        private Player _player;
        
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

        public void GameLoop() {
            GiGetCurrentLevel GiGetCurrentLevel = delegate { return GetCurrentLevel(); };
            _gameInterface.Draw(GiGetCurrentLevel);
        }

        public void moveEntity(Movable entity,int positionX, int positionY, int value, string direction) {
            
        }
        
    }
}