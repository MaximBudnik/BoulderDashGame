using System;
using System.Collections.Generic;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Enemies.SmartEnemies;
using ClassLibrary.Entities.Expanding;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;
using ClassLibrary.SoundPlayer;

namespace ClassLibrary {
    public class GameLogic {
        private readonly Action<GameStatusEnum> _changeGameStatus;
        private readonly Action<SoundFilesEnum> _playSound;
        private readonly Func<DataInterlayer> _getDataLayer;
        private readonly Action _refreshEngineSaves;

        private int _chanceToDeleteAcidBlock;

        private int _chanceToSpawnWalker;
        private int _difficulty;
        public Save CurrentSave = null;

        public GameLogic(Action<GameStatusEnum> changeGameStatus,
            Func<DataInterlayer> getDataLayer,
            Action refreshEngineSaves, Action<SoundFilesEnum> playSound) {
            _changeGameStatus = changeGameStatus;
            _getDataLayer = getDataLayer;
            _refreshEngineSaves = refreshEngineSaves;
            _playSound = playSound;
        }
        public Level CurrentLevel { get; private set; }
        public Player Player { get; private set; }

        private void SetPlayer(Player pl) {
            Player = pl;
        }

        private void SubstractPlayerHp(int i) {
            Player.SubstractPlayerHp(i);
        }

        public void CreateLevel(int levelName, string playerName, int sizeX, int sizeY, int difficulty,
            Action<SoundFilesEnum> playSound
        ) {
            CurrentLevel = new Level(
                levelName, playerName,
                //() => CurrentLevel,
                Win,
                Lose,
                () => Player.PositionX,
                () => Player.PositionY,
                SubstractPlayerHp,
                SetPlayer,
                sizeX,
                sizeY,
                difficulty,
                playSound,
                () => {
                    _chanceToDeleteAcidBlock += 1;
                    CheckIfDeleteAllAcidBlocks();
                },
                () => Player
            );
            _difficulty = difficulty;
        }

        public void GameLoop() {
            try {
                var used = new List<GameEntity>();
                for (var i = 0; i < CurrentLevel.Width; i++)
                for (var j = 0; j < CurrentLevel.Height; j++) {
                    if (used.Contains(CurrentLevel[i, j])) continue;
                    var tmp = CurrentLevel[i, j];
                    tmp.GameLoopAction();
                    used.Add(tmp);
                }

                SpawnEnemies();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        private void SpawnEnemies() {
            _chanceToSpawnWalker += _difficulty;
            var randomX = Randomizer.Random(CurrentLevel.Width);
            var randomY = Randomizer.Random(CurrentLevel.Height);
            if (CurrentLevel[randomX, randomY].EntityEnumType == GameEntitiesEnum.EmptySpace &&
                _chanceToSpawnWalker >= Randomizer.Random(100))
                CurrentLevel[randomX, randomY] =
                    new SmartSkeleton(randomX, randomY, () => CurrentLevel, () => Player.PositionX,
                        () => Player.PositionY, SubstractPlayerHp,()=>Player,_playSound);
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
            _changeGameStatus(GameStatusEnum.WinScreen);
            var dataInterlayer = _getDataLayer();
            CurrentSave.LevelName = CurrentLevel.LevelName;
            CurrentSave.Score = Player.Score;
            CurrentSave.LevelName += 1;
            CurrentSave.GameLogic = this;
            dataInterlayer.ChangeGameSave(CurrentSave);
            _refreshEngineSaves();
        }

        private void Lose() {
            _changeGameStatus(GameStatusEnum.LoseScreen);
            var dataInterlayer = _getDataLayer();
            dataInterlayer.AddBestScore(Player.Name, Player.Score);
            dataInterlayer.DeleteGameSave(CurrentSave);
            _refreshEngineSaves();
        }
    }
}