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

        public int FrameCounter {
            get => _frameCounter;
        }

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
            _gameInterface.Draw(GiGetCurrentLevel, _player.CollectedDiamonds, _player.MaxEnergy,_player.Energy);
        }

        // public void drawPart(int positionX, int positionY) {
        //     GiGetCurrentLevel GiGetCurrentLevel = delegate { return GetCurrentLevel(); };
        //     _gameInterface.DrawPart(GiGetCurrentLevel, positionX, positionY);
        // }

        public void GameLoop() {
            if (_frameCounter == 0) {
                drawLevel();
            }
            //for (int i = 0; i < CurrentLevel.Width; i++) {//TODO: refactor me pls
             //   for (int j = 0; j < CurrentLevel.Height; j++) {
                    //CurrentLevel[i, j].GameLoopAction(); //debugged for an hour, could find hy it is not working TODO: fix it
               // }                                          //problem is that we cant get element from game matrix. it exist, i can call GameLoopAction()
            //}                                              //for GameEntity but not for rocks
            
            _player.GameLoopAction();
            _rockProcessor.ProcessRock();
            if (_frameCounter == 100) {
                _frameCounter = 0;
            }
            _frameCounter++; //it counts frames and allows to perform some functions not in every frame, but every constant frame 
        }
        
    }
}