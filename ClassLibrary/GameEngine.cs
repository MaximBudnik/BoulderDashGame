using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.DataLayer;

namespace ClassLibrary {
    public class GameEngine {
        //menu 
        private readonly int _menuItems = 6;
        private readonly Action _reDraw;
        public readonly DataInterlayer DataInterlayer = new DataInterlayer();
        public readonly GameLogic GameLogic;
        private int _subActionSize = 5;
        public GameEngine(Action reDraw) {
            _reDraw = reDraw;
            GameLogic = new GameLogic(ChangeGameStatus, () => DataInterlayer, RefreshSaves);
        }
        public int GameStatus { get; private set; } // 0 - menu; 1 - game; 2 - win screen; 3 - lose screen
        public List<Save> Saves { get; private set; }
        public int CurrentMenuAction { get; private set; } = 1;

        public int CurrentSubAction { get; private set; }

        //TODO: settings validation
        public bool IsActionActive { get; private set; }
        public bool IsNameEntered { get; private set; }
        public Save NewGameSave { get; private set; } = new Save();

        public void ChangeIsNameEntered() {
            IsNameEntered = !IsNameEntered;
        }
        public void ChangeIsActionActive() {
            IsActionActive = !IsActionActive;
        }
        public void ChangeGameStatus(int i) {
            if (i >= 0 && i < 4)
                GameStatus = i;
            else
                throw new Exception("Unknown game status");
        }
        public void ChangeCurrentMenuAction(int i) {
            if (CurrentMenuAction >= _menuItems || CurrentMenuAction < 0) return;
            CurrentMenuAction += i;
            if (CurrentMenuAction == _menuItems)
                CurrentMenuAction = 0;
            else if (CurrentMenuAction == -1) CurrentMenuAction = _menuItems - 1;
        }

        private void SetSubAction(int size) {
            CurrentSubAction = 0;
            _subActionSize = size;
        }
        public void ChangeCurrentSubAction(int i) {
            if (CurrentSubAction >= _subActionSize || CurrentSubAction < 0) return;
            CurrentSubAction += i;
            if (CurrentSubAction == _subActionSize)
                CurrentSubAction = 0;
            else if (CurrentSubAction == -1) CurrentSubAction = _subActionSize - 1;
        }

        public void PerformSubAction(int i) {
            switch (CurrentMenuAction) {
                case 0:
                    LaunchGame(Saves[CurrentSubAction]);
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
                            ChangeIsNameEntered();
                            break;
                        case 2:
                            DataInterlayer.AddGameSave(NewGameSave);
                            LaunchGame(NewGameSave);
                            break;
                    }
                    break;
                case 2:
                    if (i == 0) {
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
        private void GraphicsThread() {
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
                Thread.Sleep(1000 / DataInterlayer.Settings.TickRate);
                GameLogic.GameLoop();
            }
        }
        private void RefreshSaves() {
            Saves = DataInterlayer.GetAllGameSaves();
        }
        public void Start() {
            // Task musicPlayer = new Task(() => {
            //     SoundPlayer soundPlayer = new SoundPlayer();
            //     soundPlayer.playMusic();
            // });
            //musicPlayer.Start(); //TODO: dont forget to enable music on build!

            RefreshSaves();
            MenuGameCycle();

            void MenuGameCycle() {
                if (GameStatus == 0)
                    Parallel.Invoke(MenuGraphicsThread);
                else if (GameStatus == 1)
                    Parallel.Invoke(GraphicsThread,
                        GameLogicThread);
                MenuGameCycle();
            }
        }
        private void LaunchGame(Save save) {
            GameLogic.CreateLevel(save.LevelName, save.Name, DataInterlayer.Settings.SizeX,
                DataInterlayer.Settings.SizeY, DataInterlayer.Settings.Difficulty);
            GameLogic.Player.Score = save.Score;
            GameLogic.Player.Hero = save.Hero;
            GameLogic.CurrentSave = save;
            GameStatus = 1;
        }
    }
}