using System;
using System.Collections.Generic;
using System.Threading;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;

namespace ClassLibrary {
    public class GameLogic {
        public Level CurrentLevel { get; private set; }
        public Player Player { get; private set; }
        public Save CurrentSave = null;

        private Action<int> _changeGameStatus;
        private Func<DataInterlayer> _getDataLayer;
        private int _chanceToDeleteAcidBlock = 0;
        
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
            CurrentLevel = new Level(
                levelName, playerName,
                () => CurrentLevel,
                Win,
                Lose,
                () => Player.PositionX,
                () => Player.PositionY,
                SubstractPlayerHp,
                SetPlayer
            );
        }

        public void GameLoop() {
            List<GameEntity> used = new List<GameEntity>();
            for (int i = 0; i < CurrentLevel.Width; i++) {
                for (int j = 0; j < CurrentLevel.Height; j++) {
                    if (used.Contains(CurrentLevel[i, j])) continue;
                    if (CurrentLevel[i, j] is Player ) {
                        var tmp = (Player) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                        Player = (Player) CurrentLevel[i, j];
                    }
                    if (CurrentLevel[i, j] is EnemyWalker ) {
                        var tmp = (EnemyWalker) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                    }
                    if (CurrentLevel[i, j] is Rock ) {
                        var tmp = (Rock) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                    }
                    if (CurrentLevel[i, j] is StoneInDiamondConverter) {
                        var tmp = (StoneInDiamondConverter) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                    }
                    if (CurrentLevel[i, j] is Acid) { //player instantly breaks created block
                        var tmp = (Acid) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                        _chanceToDeleteAcidBlock+=1;
                        CheckIfDeleteAllAcidBlocks();
                    }
                    if (CurrentLevel[i, j] is EnemyDigger) {
                        var tmp = (EnemyDigger) CurrentLevel[i, j];
                        tmp.GameLoopAction();
                        used.Add(tmp);
                    }
                }
            }
        }
        private void CheckIfDeleteAllAcidBlocks() {
            if (_chanceToDeleteAcidBlock >= 50) {
                for (int i = 0; i < CurrentLevel.Width; i++) {
                    for (int j = 0; j < CurrentLevel.Height; j++) {
                        if (CurrentLevel[i, j] is Acid) {
                            ((Acid)CurrentLevel[i, j]).TurnIntoRock();
                        }
                    }
                }
                _chanceToDeleteAcidBlock = 0;
            }
        }

        private void Win() {
            _changeGameStatus(2);
            //Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            CurrentSave.LevelName = CurrentLevel.LevelName;
            CurrentSave.Score = Player.Score;
            CurrentSave.LevelName += 1;
            dataInterlayer.ChangeGameSave(CurrentSave);
            _changeGameStatus(0);
         //   Console.ReadLine();
        }

        private void Lose() {
            _changeGameStatus(3);
          //  Thread.Sleep(1000);
            DataInterlayer dataInterlayer = _getDataLayer();
            dataInterlayer.AddBestScore(Player.Name, Player.Score);
            dataInterlayer.DeleteGameSave(CurrentSave);
            _changeGameStatus(0);
          //  Console.ReadLine();
        }
    }
}