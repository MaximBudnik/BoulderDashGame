using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;

namespace ClassLibrary {
    public class LevelRedactor {
        public CustomLevel NewCustomLevel { get; private set; } = new CustomLevel();

        public int MapPosX = 0;
        public int MapPosY = 0;
        public GameEntitiesEnum activeBlock = GameEntitiesEnum.Player;
        public readonly List<GameEntitiesEnum> Tools = new List<GameEntitiesEnum>();
        public int CurrentTool = 0;

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
            if(CurrentTool == Tools.Count)
                CurrentTool = 0;
            activeBlock = Tools[CurrentTool];
        }

        public void PlaceBlock() {
            NewCustomLevel.Map[MapPosY, MapPosX] = activeBlock;
        }

        public LevelRedactor() {
            NewCustomLevel.Map = new GameEntitiesEnum[NewCustomLevel.SizeY, NewCustomLevel.SizeX];
            for (var i = 0; i < NewCustomLevel.SizeY; i++) {
                for (var j = 0; j < NewCustomLevel.SizeX; j++) {
                    NewCustomLevel.Map[i, j] = GameEntitiesEnum.EmptySpace;
                }
            }
        }

        public void FillNewLevel() {
            NewCustomLevel.Map = new GameEntitiesEnum[NewCustomLevel.SizeY, NewCustomLevel.SizeX];
            for (var i = 0; i < NewCustomLevel.SizeY; i++) {
                for (var j = 0; j < NewCustomLevel.SizeX; j++) {
                    NewCustomLevel.Map[i, j] = GameEntitiesEnum.EmptySpace;
                }
            }
        }

        public void FillToolArray() {
            foreach (var entity in (GameEntitiesEnum[]) Enum.GetValues(typeof(GameEntitiesEnum))) {
                Tools.Add(entity);
            }
            // Tools.Add(GameEntitiesEnum.Player);
            // Tools.Add(GameEntitiesEnum.EmptySpace);
            // Tools.Add(GameEntitiesEnum.Sand);
            // Tools.Add(GameEntitiesEnum.Rock);
        }
    }
}