using System.Drawing;
using ClassLibrary;
using ClassLibrary.Entities;
using ClassLibrary.Matrix;

namespace BoulderDashForms.FormsDrawers {
    public class LevelRedactorDrawer : FormDrawer {
        private readonly Pen _mapPen = new Pen(Color.Fuchsia, 2);
        private readonly Pen _toolPen = new Pen(Color.WhiteSmoke, 2);

        private Rectangle _anchorRect;
        public void DrawRedactor(Graphics graphics, GameEngine gameEngine, int width, int height) {
            DrawMap(graphics, gameEngine, width, height);
            DrawToolBar(graphics, gameEngine, width, height);
            DrawHelp(graphics, gameEngine, width, height);
        }

        private void DrawHelp(Graphics graphics, GameEngine gameEngine, int width, int height) {
            var levelRedactor = gameEngine.LevelRedactor;
            if (!levelRedactor.ShowHelp) return;
            graphics.FillRectangle(RedBrushTransparent, new Rectangle(width - 300 - 64, 0, 300, height));
            graphics.DrawString($"Name: {levelRedactor.NewCustomLevel.Name}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 5);
            graphics.DrawString($"Size Y: {levelRedactor.NewCustomLevel.SizeX}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 35);
            graphics.DrawString($"Size X: {levelRedactor.NewCustomLevel.SizeY}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 65);
            graphics.DrawString($"Mode: {levelRedactor.NewCustomLevel.Aim}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 95);
            var playerStatus = "Good";
            if (levelRedactor.PlayersQuantity < 1) playerStatus = "Absent";
            if (levelRedactor.PlayersQuantity > 1) playerStatus = "Excess";
            graphics.DrawString($"Player OK? {playerStatus}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 125);
            var fishStatus = "Good";
            if (levelRedactor.FishQuantity < 1) fishStatus = "Absent";
            if (levelRedactor.NewCustomLevel.Aim != GameModesEnum.HuntGoldenFish) fishStatus = "Not required";
            graphics.DrawString($"Fish OK? {fishStatus}",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 155);
            graphics.DrawString("W,A,S,D to move on map",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 285);
            graphics.DrawString("Space to place block",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 315);
            graphics.DrawString("Up/Down to change block",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 345);
            graphics.DrawString("Mouse wheel to scale",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 375);
            graphics.DrawString("+/- to size area on map",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 405);
            graphics.DrawString("F to fill all map",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 435);
            graphics.DrawString("E,R to change brush",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 465);
            graphics.DrawString("H to hide/show help",
                MenuFont, WhiteBrush, width - 300 - 64 + 5, 495);
        }

        private void DrawToolBar(Graphics graphics, GameEngine gameEngine, int width, int height) {
            graphics.FillRectangle(DarkBrush, new Rectangle(width - 64, 0, 64, height));
            var tools = gameEngine.LevelRedactor.Tools;
            for (var i = 0; i < tools.Count; i++) {
                var destRect =
                    new Rectangle(
                        new Point(width - 56,
                            (int) (8 + i * GameEntity.FormsSize * 2.6)),
                        new Size(32, 32));
                Rectangle srcRect;
                //graphics.FillRectangle(Brushes.GreenYellow, destRect);
                switch (tools[i]) {
                    case GameEntitiesEnum.Player:
                        var kf = 6; //parameter to beautify hero sprite
                        var hero = 68;
                        var pHeight = 28 - kf;
                        var pWidth = 16;
                        var pixelY = hero + kf;
                        srcRect = new Rectangle(new Point(9 * 16 + 1 * 16, pixelY),
                            new Size(pWidth, pHeight));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EmptySpace:
                        srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Sand:
                        srcRect = new Rectangle(new Point(9 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(TileSet, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Rock:
                        srcRect = new Rectangle(new Point(7 * 16, 4 * 16), new Size(16, 16));
                        graphics.DrawImage(TileSet, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Diamond:
                        srcRect = new Rectangle(new Point(1 * 16, 7 * 16),
                            new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Wall:
                        srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartSkeleton:
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.LuckyBox:
                        srcRect = new Rectangle(new Point(15 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SandTranslucent:
                        srcRect = new Rectangle(new Point(7 * 16, 10 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Converter:
                        srcRect = new Rectangle(new Point(2 * 16, 3 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Acid:
                        srcRect = new Rectangle(new Point(4 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Barrel:
                        srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EnemyDigger:
                        srcRect = new Rectangle(new Point(27 * 16 + 1 * 16, 7 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EnemyWalker:
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 11 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartPeaceful:
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 1 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartDevil:
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 21 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Candy:
                        srcRect = new Rectangle(new Point(12 * 16, 4 * 16), new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.GoldenFish:
                        srcRect = new Rectangle(new Point(10 * 16, 7 * 16),
                            new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SwordTile:
                        srcRect = new Rectangle(new Point(20 * 16, 2 * 12), new Size(16, 22));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.ConverterTile:
                        srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.DynamiteTile:
                        srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.ArmorTile:
                        srcRect = new Rectangle(new Point(6 * 16, 15 * 16), new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    default:
                        srcRect = new Rectangle(new Point(0 * 16, 0 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;
                }
                if (gameEngine.LevelRedactor.CurrentTool == i) graphics.DrawRectangle(_toolPen, destRect);
            }
        }

        private void DrawMap(Graphics graphics, GameEngine gameEngine, int width, int height) {
            var scale = gameEngine.GuiScale;
            var currentLevel = gameEngine.LevelRedactor.NewCustomLevel;
            for (var i = 0; i < gameEngine.LevelRedactor.NewCustomLevel.SizeY; i++)
            for (var j = 0; j < gameEngine.LevelRedactor.NewCustomLevel.SizeX; j++) {
                var destRect =
                    new Rectangle(
                        new Point((int) (j * GameEntity.FormsSize * scale),
                            (int) (i * GameEntity.FormsSize * scale)),
                        new Size((int) (GameEntity.FormsSize * scale), (int) (GameEntity.FormsSize * scale)));
                Rectangle srcRect;

                void DrawFloorTile() {
                    srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                    graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                }
                switch (currentLevel.Map[i, j]) {
                    case GameEntitiesEnum.Player:
                        DrawFloorTile();
                        var kf = 6; //parameter to beautify hero sprite
                        var hero = 68;
                        var pHeight = 28 - kf;
                        var pWidth = 16;
                        var pixelY = hero + kf;
                        srcRect = new Rectangle(new Point(9 * 16 + 1 * 16, pixelY),
                            new Size(pWidth, pHeight));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EmptySpace:
                        srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Sand:
                        srcRect = new Rectangle(new Point(9 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(TileSet, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Rock:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(7 * 16, 4 * 16), new Size(16, 16));
                        graphics.DrawImage(TileSet, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Diamond:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(1 * 16, 7 * 16),
                            new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Wall:
                        srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartSkeleton:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.LuckyBox:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(15 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SandTranslucent:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(7 * 16, 10 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Converter:
                        srcRect = new Rectangle(new Point(2 * 16, 3 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Acid:
                        srcRect = new Rectangle(new Point(4 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Barrel:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EnemyDigger:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(27 * 16 + 1 * 16, 7 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.EnemyWalker:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 11 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartPeaceful:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 1 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SmartDevil:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(23 * 16 + 1 * 16, 21 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.Candy:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(12 * 16, 4 * 16), new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.GoldenFish:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(10 * 16, 7 * 16),
                            new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.SwordTile:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(20 * 16, 2 * 12), new Size(16, 22));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.ConverterTile:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.DynamiteTile:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(16, 16));
                        graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    case GameEntitiesEnum.ArmorTile:
                        DrawFloorTile();
                        srcRect = new Rectangle(new Point(6 * 16, 15 * 16), new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                        break;

                    default:
                        srcRect = new Rectangle(new Point(0 * 16, 0 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                        break;
                }
                if (gameEngine.LevelRedactor.MapPosX == j && gameEngine.LevelRedactor.MapPosY == i)
                    _anchorRect = new Rectangle(
                        new Point((int) (j * GameEntity.FormsSize * scale),
                            (int) (i * GameEntity.FormsSize * scale)),
                        new Size((int) (GameEntity.FormsSize * scale) * gameEngine.LevelRedactor.BrushSize,
                            (int) (GameEntity.FormsSize * scale) * gameEngine.LevelRedactor.BrushSize));
            }

            graphics.DrawRectangle(_mapPen, _anchorRect);
        }
    }
}