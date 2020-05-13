using System;
using System.Collections.Generic;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;

namespace ClassLibrary {
    public class GameLogic {
        private readonly Action<int> _changeGameStatus;
        private readonly Func<DataInterlayer> _getDataLayer;
        private readonly Action _refreshEngineSaves;

        private int _chanceToDeleteAcidBlock;
        public Save CurrentSave = null;

        public GameLogic(Action<int> changeGameStatus,
            Func<DataInterlayer> getDataLayer,
            Action refreshEngineSaves) {
            _changeGameStatus = changeGameStatus;
            _getDataLayer = getDataLayer;
            _refreshEngineSaves = refreshEngineSaves;
        }
        public Level CurrentLevel { get; private set; }
        public Player Player { get; private set; }

        private void SetPlayer(Player pl) {
            Player = pl;
        }

        private void SubstractPlayerHp(int i) {
            Player.SubstractPlayerHp(i);
        }

        public void CreateLevel(int levelName, string playerName, int sizeX, int sizeY, int Difficulty
        ) {
            CurrentLevel = new Level(
                levelName, playerName,
                () => CurrentLevel,
                Win,
                Lose,
                () => Player.PositionX,
                () => Player.PositionY,
                SubstractPlayerHp,
                SetPlayer,
                sizeX,
                sizeY,
                Difficulty
            );
        }

        public void GameLoop() {
            var used = new List<GameEntity>();
            for (var i = 0; i < CurrentLevel.Width; i++)
            for (var j = 0; j < CurrentLevel.Height; j++) {
                if (used.Contains(CurrentLevel[i, j])) continue;
                if (CurrentLevel[i, j] is Player) {
                    var tmp = (Player) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                    Player = tmp;
                }
                if (CurrentLevel[i, j] is EnemyWalker) {
                    var tmp = (EnemyWalker) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                }
                if (CurrentLevel[i, j] is Rock) {
                    var tmp = (Rock) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                }
                if (CurrentLevel[i, j] is StoneInDiamondConverter) {
                    var tmp = (StoneInDiamondConverter) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                }
                if (CurrentLevel[i, j] is Acid) {
                    //player instantly breaks created block
                    var tmp = (Acid) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                    _chanceToDeleteAcidBlock += 1;
                    CheckIfDeleteAllAcidBlocks();
                }
                if (CurrentLevel[i, j] is EnemyDigger) {
                    var tmp = (EnemyDigger) CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                }
            }
        }
        private void CheckIfDeleteAllAcidBlocks() {
            if (_chanceToDeleteAcidBlock >= 50) {
                for (var i = 0; i < CurrentLevel.Width; i++)
                for (var j = 0; j < CurrentLevel.Height; j++)
                    if (CurrentLevel[i, j] is Acid)
                        ((Acid) CurrentLevel[i, j]).TurnIntoRock();
                _chanceToDeleteAcidBlock = 0;
            }
        }

        private void Win() {
            _changeGameStatus(2);
            var dataInterlayer = _getDataLayer();
            CurrentSave.LevelName = CurrentLevel.LevelName;
            CurrentSave.Score = Player.Score;
            CurrentSave.LevelName += 1;
            dataInterlayer.ChangeGameSave(CurrentSave);
            _refreshEngineSaves();
        }

        private void Lose() {
            _changeGameStatus(3);
            var dataInterlayer = _getDataLayer();
            dataInterlayer.AddBestScore(Player.Name, Player.Score);
            dataInterlayer.DeleteGameSave(CurrentSave);
            _refreshEngineSaves();
        }
    }
}