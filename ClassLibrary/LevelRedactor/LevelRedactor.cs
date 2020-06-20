using System;
using System.Collections.Generic;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;

namespace ClassLibrary.LevelRedactor {
    public class LevelRedactor {
        public readonly List<GameEntitiesEnum> Tools = new List<GameEntitiesEnum>();
        private GameEntitiesEnum _activeBlock = GameEntitiesEnum.EmptySpace;
        private RedactorBrushes _activeBrush = RedactorBrushes.Default;

        public LevelRedactor() {
            FillAll();
        }
        public CustomLevel NewCustomLevel { get; } = new CustomLevel();

        public int MapPosX { get; private set; }
        public int MapPosY { get; private set; }
        public int CurrentTool { get; private set; } = 1;
        public int BrushSize { get; private set; } = 1;

        public int PlayersQuantity { get; private set; }
        public int FishQuantity { get; private set; }
        public bool ShowHelp { get; private set; } = true;

        public void ChangeShowHelp() {
            ShowHelp = !ShowHelp;
        }

        public void ChangeBrushSize(int value) {
            BrushSize += value;
            if (BrushSize < 1) BrushSize = 1;
        }

        public void SetActiveBrush(RedactorBrushes brush) {
            _activeBrush = brush;
        }

        public void ChangeAnchorPosition(MoveDirectionEnum direction, int value) {
            switch (direction) {
                case MoveDirectionEnum.Horizontal:
                    if (MapPosX + value >= 0 && MapPosX + value < NewCustomLevel.SizeX)
                        MapPosX += value;
                    break;
                case MoveDirectionEnum.Vertical:
                    if (MapPosY + value >= 0 && MapPosY + value < NewCustomLevel.SizeY)
                        MapPosY += value;
                    break;
            }
        }

        public void ChangeTool(int value) {
            CurrentTool += value;
            if (CurrentTool < 0)
                CurrentTool = Tools.Count - 1;
            if (CurrentTool == Tools.Count)
                CurrentTool = 0;
            _activeBlock = Tools[CurrentTool];
        }

        public void PlaceBlock() {
            for (var i = 0; i < BrushSize; i++)
            for (var j = 0; j < BrushSize; j++) {
                var willPlace = false;
                switch (_activeBrush) {
                    case RedactorBrushes.Default:
                        willPlace = true;
                        break;
                    case RedactorBrushes.Empty:
                        if (i == 0 || i == BrushSize - 1 || j == 0 || j == BrushSize - 1) willPlace = true;
                        break;
                }
                if (willPlace) CheckAndPlaceBlock(MapPosY + i, MapPosX + j);
            }
        }

        private void CheckAndPlaceBlock(int x, int y) {
            if (x >= NewCustomLevel.SizeY || y >= NewCustomLevel.SizeX) return;
            if (NewCustomLevel.Map[x, y] == GameEntitiesEnum.Player && PlayersQuantity != 0) PlayersQuantity--;
            else if (NewCustomLevel.Map[x, y] == GameEntitiesEnum.GoldenFish && PlayersQuantity != 0) FishQuantity--;
            NewCustomLevel.Map[x, y] = _activeBlock;
            if (_activeBlock == GameEntitiesEnum.Player) PlayersQuantity++;
            else if (_activeBlock == GameEntitiesEnum.GoldenFish) FishQuantity++;
        }

        public void FillAll() {
            NewCustomLevel.Map = new GameEntitiesEnum[NewCustomLevel.SizeY, NewCustomLevel.SizeX];
            for (var i = 0; i < NewCustomLevel.SizeY; i++)
            for (var j = 0; j < NewCustomLevel.SizeX; j++)
                CheckAndPlaceBlock(i, j);
        }

        public void FillToolArray() {
            foreach (var entity in (GameEntitiesEnum[]) Enum.GetValues(typeof(GameEntitiesEnum))) Tools.Add(entity);
        }
    }
}