﻿using System;
using ClassLibrary.DataLayer;

namespace ClassLibrary {
    public partial class GameEngine {
        private readonly int _menuItems = 6;
        private int _subActionSize = 5;
        public int CurrentMenuAction { get; private set; } = 1;
        public int CurrentSubAction { get; private set; }
        public bool IsActionActive { get; private set; }
        public bool IsNameEntered { get; private set; }
        public void ChangeIsNameEntered() {
            IsNameEntered = !IsNameEntered;
        }
        public void ChangeIsActionActive() {
            IsActionActive = !IsActionActive;
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
            if (GameStatus == GameStatusEnum.WinScreen || GameStatus == GameStatusEnum.LoseScreen) {
                GameStatus = GameStatusEnum.Menu;
                return;
            }
            switch (CurrentMenuAction) {
                case 0:
                    LaunchGame(Saves[CurrentSubAction]);
                    GameStatus = GameStatusEnum.Game;
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
                            if (DataInterlayer.Settings.Difficulty < 0) DataInterlayer.Settings.Difficulty = 0;
                            break;
                        case 1:
                            DataInterlayer.Settings.SizeX += i;
                            if (DataInterlayer.Settings.SizeX < 10) DataInterlayer.Settings.SizeX = 10;
                            break;
                        case 2:
                            DataInterlayer.Settings.SizeY += i;
                            if (DataInterlayer.Settings.SizeY < 10) DataInterlayer.Settings.SizeY = 10;
                            break;
                        case 3:
                            DataInterlayer.Settings.Fps += i;
                            if (DataInterlayer.Settings.Fps < 5) DataInterlayer.Settings.Fps = 5;
                            break;
                        case 4:
                            DataInterlayer.Settings.TickRate += i;
                            if (DataInterlayer.Settings.TickRate < 1) DataInterlayer.Settings.TickRate = 1;
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
    }
}