using System;

namespace ClassLibrary {
    public class GameLogic {
        private GameInterface _gameInterface = new GameInterface();
        private Level _currentLevel;
        
        //initialize other fields
        
        public delegate Level GiGetCurrentLevel();

        public Level GetCurrentLevel() {
            return _currentLevel;
        }
        
        public void CreateLevel(string levelName) {
            _currentLevel = new Level(levelName);
        }

        public void GameLoop() {
            GiGetCurrentLevel GiGetCurrentLevel = delegate { return GetCurrentLevel(); };
            _gameInterface.Draw(GiGetCurrentLevel);
        }
    }
}