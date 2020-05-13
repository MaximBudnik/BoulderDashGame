using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.ConsoleInterface;
using ClassLibrary.DataLayer;
using ClassLibrary.InputProcessors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary {
    public class GameEngine {
        private readonly Action _reDraw;

        public GameEngine(Action reDraw) {
            _reDraw = reDraw;
        }
        //TODO: settings validation
        public int GameStatus; // 0 - menu; 1 - game; 2 - win screen; 3 - lose screen
        public int CurrentMenuAction = 1;
        public int CurrentSubAction = 0;
        private int _subActionSize = 5;
        private readonly int MenuItems = 6;
        public bool IsActionActive = false;
        public bool IsNameEntered = false;
        public List<Save> Saves;
        public Save NewGameSave = new Save();

        public void ChangeIsNameEntered() {
            IsNameEntered = !IsNameEntered;
        }
        public void ChangeGameStatus(int i) {
            if (i >= 0 && i < 4)
                GameStatus = i;
            else
                throw new Exception("Unknown game status");
        }
        public void ChangeCurrentMenuAction(int i) {
            if (CurrentMenuAction < MenuItems && CurrentMenuAction >= 0) {
                CurrentMenuAction += i;
                if (CurrentMenuAction == MenuItems)
                    CurrentMenuAction = 0;
                else if (CurrentMenuAction == -1) CurrentMenuAction = MenuItems - 1;
            }
        }

        public void SetSubAction(int size) {
            CurrentSubAction = 0;
            _subActionSize = size;
        }
        public void ChangeCurrentSubAction(int i) {
            if (CurrentSubAction < _subActionSize && CurrentSubAction >= 0) {
                CurrentSubAction += i;
                if (CurrentSubAction == _subActionSize)
                    CurrentSubAction = 0;
                else if (CurrentSubAction == -1) CurrentSubAction = _subActionSize - 1;
            }
        }

        public void PerformSubAction(int i) {
            switch (CurrentMenuAction) {
                case 0:
                    ResumeGame(Saves[CurrentSubAction]);
                    GameStatus = 1;
                    break;
                case 1:
                    switch (CurrentSubAction) {
                        case 0:
                            NewGameSave.Hero += i;
                            if (NewGameSave.Hero < 0) NewGameSave.Hero = 5;
                            if (NewGameSave.Hero > 5) NewGameSave.Hero = 0;
                            break;
                        case 1:
                            DataInterlayer.Settings.SizeX += i;
                            ChangeIsNameEntered();
                            break;
                        case 2:
                            DataInterlayer.AddGameSave(NewGameSave);
                            GameStatus = 1;
                            ResumeGame(NewGameSave);
                            break;
                    }
                    break;
                case 2:
                    if (i==0) {
                        DataInterlayer.SaveSettings();
                        IsActionActive = false;
                        return;
                    }
                    switch (CurrentSubAction) {
                        case 0:
                            DataInterlayer.Settings.Difficulty += i;
                            break;
                        case 1:
                            DataInterlayer.Settings.SizeX += i;
                            break;
                        case 2:
                            DataInterlayer.Settings.SizeY += i;
                            break;
                        case 3:
                            DataInterlayer.Settings.Fps += i;
                            break;
                        case 4:
                            DataInterlayer.Settings.TickRate += i;
                            break;
                    }
                    break;
            }
        }

        public void PerformCurrentMenuAction() {
            switch (CurrentMenuAction) {
                case 0:
                    IsActionActive = true;
                    SetSubAction(Saves.Count);
                    break;
                case 1:
                    IsActionActive = true;
                    NewGameSave = new Save();
                    SetSubAction(3);
                    break;
                case 2:
                    IsActionActive = true;
                    SetSubAction(5);
                    break;
                case 3:
                    IsActionActive = true;
                    SetSubAction(0);
                    break;
                case 4:
                    IsActionActive = true;
                    SetSubAction(0);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }

        public readonly GameLogic GameLogic = new GameLogic();
        public readonly DataInterlayer DataInterlayer = new DataInterlayer();
        private void GraphicsThread() {
            #region Console drawing
            //Console
            // while (_gameStatus == 1) {
            //     _gameInterface.DrawUpperInterface(currentLevel.LevelName, player.Score, currentLevel.Aim);
            //     _gameInterface.DrawPlayerInterface(currentLevel.DiamondsQuantity, player.CollectedDiamonds,
            //         player.MaxEnergy, player.Energy, player.MaxHp, player.Hp, player.Name, player.Inventory);
            //     _gameInterface.NewDraw(() => currentLevel);
            // }
            // if (_gameStatus == 2) _afterLevelScreen.DrawGameWin(player.Score, player.AllScores);
            // else if (_gameStatus == 3) _afterLevelScreen.DrawGameLose();
            #endregion

            while (GameStatus == 1) {
                Thread.Sleep(1000 / DataInterlayer.Settings.Fps);
                _reDraw();
            }
        }

        private void MenuGraphicsThread() {
            while (GameStatus == 0) {
                Thread.Sleep(1000 / (DataInterlayer.Settings.Fps * 2));
                _reDraw();
            }
        }

        private void GameLogicThread() {
            while (GameStatus == 1) {
                //Console.CursorVisible = false;
                Thread.Sleep(1000 / DataInterlayer.Settings.TickRate);
                GameLogic.GameLoop();
            }
        }
        
        public void Start() {
            // Task musicPlayer = new Task(() => {
            //     SoundPlayer soundPlayer = new SoundPlayer();
            //     soundPlayer.playMusic();
            // });
            //musicPlayer.Start(); //TODO: dont forget to enable music on build!


            //for develop

            
            Saves = DataInterlayer.GetAllGameSaves();

            
            MenuGameCycle();
            
            void MenuGameCycle() {
               
               if (GameStatus == 0) {
                   while (GameStatus == 0) {
                       Thread.Sleep(1000 / (DataInterlayer.Settings.Fps * 2));
                       _reDraw();
                   }
               } 
               else if (GameStatus == 1) {
                   Parallel.Invoke(GraphicsThread,
                       GameLogicThread);
               }
                MenuGameCycle();
            }
        }
        private void ResumeGame(Save save) {
            GameLogic.CreateLevel(save.LevelName, save.Name, ChangeGameStatus, () => DataInterlayer,
                DataInterlayer.Settings.SizeX,DataInterlayer.Settings.SizeY, DataInterlayer.Settings.Difficulty);
            GameLogic.Player.Score = save.Score;
            GameLogic.Player.Hero = save.Hero;
            GameLogic.CurrentSave = save;
        }
    }
}