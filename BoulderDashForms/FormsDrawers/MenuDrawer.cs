using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.DataLayer;
using ClassLibrary.Entities;

namespace BoulderDashForms.FormsDrawers {
    public class MenuDrawer : FormDrawer {
        private readonly SolidBrush _redBrush = new SolidBrush(Color.FromArgb(255, 218, 78, 56));
        private readonly SolidBrush _redBrushHalfTransparent = new SolidBrush(Color.FromArgb(200, 218, 78, 56));
        private readonly SolidBrush _redBrushTransparent = new SolidBrush(Color.FromArgb(130, 218, 78, 56));
        private readonly SolidBrush _whiteBrush = new SolidBrush(Color.FromArgb(255, 249, 245, 255));
        private readonly SolidBrush _darkBrush = new SolidBrush(Color.FromArgb(255, 34, 29, 35));
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Scores", "Help", "Exit",};
        private readonly Font _headerFont;
        private readonly Font _mainFont;
        private int _enemyPosition = 0;
        private int _currentFrameForEnemies = 0;
        private int maxFramesForEnemies = 4;
        private int _rightBlockWidth = 1000;
        private SortedDictionary<int, string> _results = null;
        public void NullRightBlockWidth() {
            _rightBlockWidth = 0;
        }

        public MenuDrawer() {
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\monogram.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\ThaleahFat.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\m5x7.ttf"));
            family1 = fontCollection.Families[0];
            family2 = fontCollection.Families[1];
            var family3 = fontCollection.Families[2];
            GC.KeepAlive(fontCollection);
            GC.KeepAlive(family1);
            GC.KeepAlive(family2);
            GC.KeepAlive(family3);
            _menuFont = new Font(family1, 28);
            _boldFont = new Font(family3, 28);
            _headerFont = new Font(family3, 38);
            _mainFont = new Font(family2, 30);
            GC.KeepAlive(_menuFont);
            GC.KeepAlive(_boldFont);
            GC.KeepAlive(_headerFont);
            GC.KeepAlive(_mainFont);
        }

        public void DrawMenu(Graphics graphics, GameEngine gameEngine) {
            FillBackground(graphics);

            //shadow rectangle
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            Rectangle rect = new Rectangle(0, 0, 1500, 900);
            graphics.FillRectangle(semiTransBrush, rect);

            //front sprites

            EnemyHeroAnimation(graphics);

            //menu rectangle
            rect = new Rectangle(150, 0, 350, 900);
            graphics.FillRectangle(_darkBrush, rect);

            graphics.DrawString("DUNGEON", _headerFont, _redBrush, 160, 30);
            graphics.DrawString("DASH", _headerFont, _redBrush, 160, 68);

            for (int i = 0; i < _menuActions.Length; i++) {
                if (gameEngine.CurrentMenuAction == i) {
                    graphics.DrawString(_menuActions[i], _mainFont, _whiteBrush, 170, 200 + i * 36);
                }
                else {
                    graphics.DrawString(_menuActions[i], _mainFont, _redBrush, 160, 200 + i * 36);
                }
            }

            //right zone (center rectangle)
            Rectangle rightBlock = new Rectangle(500, 150, _rightBlockWidth, 600);
            graphics.FillRectangle(gameEngine.IsActionActive ? _redBrushHalfTransparent : _redBrushTransparent,
                rightBlock);
            if (_rightBlockWidth < 1000) {
                _rightBlockWidth += 100;
            }

            //right zone content
            if (_rightBlockWidth < 1000) return;
            string blockHeader = "";
            switch (gameEngine.CurrentMenuAction) {
                case 0:
                    blockHeader = "Select save";
                    DrawContinue(graphics, gameEngine);
                    break;
                case 1:
                    blockHeader = "Create new game";
                    Rectangle selected = new Rectangle(520, 220 + gameEngine.CurrentSubAction * 60, 940, 40);
                    graphics.FillRectangle(_darkBrush,
                        selected);
                    int hero = 36;
                    switch (gameEngine.NewGameSave.Hero) {
                        case 0:
                            hero = 4;
                            break;
                        case 1:
                            hero = 36;
                            break;
                        case 2:
                            hero = 68;
                            break;
                        case 3:
                            hero = 100;
                            break;
                        case 4:
                            hero = 132;
                            break;
                        case 5:
                            hero = 164;
                            break;
                    }
                    int kf = 3;
                    int pixelY = hero + kf;
                    Rectangle destRect =
                        new Rectangle(new Point(820, 210),
                            new Size(32, 52));
                    var srcRect = new Rectangle(new Point(9 * 16  /*+ 1 * 16*/, pixelY),
                        new Size(16, 26));
                    graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);

                    graphics.DrawString("Choose your hero: ",
                        _mainFont, _whiteBrush, 520, 220);
                    graphics.DrawString($"Enter your name: {gameEngine.NewGameSave.Name}",
                        _mainFont, _whiteBrush, 520, 280);
                    graphics.DrawString("Start game",
                        _mainFont, _whiteBrush, 520, 340);
                    break;
                case 2:
                    blockHeader = "Adjust your settings";
                    DrawSettings(graphics, gameEngine);
                    break;
                case 3:
                    blockHeader = "Best results";
                    DrawBestScores(graphics, gameEngine);
                    break;
                case 4:
                    blockHeader = "Game guide";
                    DrawHelp(graphics);
                    break;
                case 5:
                    blockHeader = "Press ENTER to exit game";
                    break;
            }
            graphics.DrawString(blockHeader, _headerFont, _whiteBrush, 520, 160);
        }
        private void DrawContinue(Graphics graphics, GameEngine gameEngine) {
            int counter = 1;
            Rectangle selected = new Rectangle(520, 220 + gameEngine.CurrentSubAction * 40, 940, 40);
            graphics.FillRectangle(_darkBrush,
                selected);
            foreach (var result in gameEngine.Saves) {
                graphics.DrawString($" Name: {result.Name}",
                    _mainFont, _whiteBrush, 520, 180 + 40 * counter);
                graphics.DrawString($"Score: {result.Score}",
                    _mainFont, _whiteBrush, 910, 180 + 40 * counter);
                graphics.DrawString($"Level: {result.LevelName}",
                    _mainFont, _whiteBrush, 1200, 180 + 40 * counter);
                counter++;
            }
        }
        private void DrawHelp(Graphics graphics) {
            graphics.DrawString("Use W,A,S,D to move hero", _mainFont, _whiteBrush, 650, 226);
            Rectangle destRect =
                new Rectangle(new Point(550, 210),
                    new Size(32, 32));
            Rectangle srcRect = new Rectangle(new Point(4 * 16, 2 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //a
            destRect =
                new Rectangle(new Point(518, 242),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(3 * 16, 3 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);

            //s
            destRect =
                new Rectangle(new Point(550, 242),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(4 * 16, 3 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //d
            destRect =
                new Rectangle(new Point(582, 242),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(5 * 16, 3 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);

            graphics.DrawString("Use SPACE to restore energy, Q,E,R,T for abilities", _mainFont, _whiteBrush, 650, 326);
            //space
            destRect =
                new Rectangle(new Point(518, 348),
                    new Size(126, 32));
            srcRect = new Rectangle(new Point(5 * 16, 5 * 16), new Size(80, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //t
            destRect =
                new Rectangle(new Point(614, 306),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(7 * 16, 2 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //q
            destRect =
                new Rectangle(new Point(518, 306),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(3 * 16, 2 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //e
            destRect =
                new Rectangle(new Point(550, 306),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(5 * 16, 2 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //r
            destRect =
                new Rectangle(new Point(582, 306),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(6 * 16, 2 * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);

            graphics.DrawString("Collect diamonds to complete level", _mainFont, _whiteBrush, 650, 406);
            destRect =
                new Rectangle(new Point(550, 406),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(15, 16));
            graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
            graphics.DrawString("Avoid enemies...or fight with them", _mainFont, _whiteBrush, 650, 476);
            destRect =
                new Rectangle(new Point(550, 476),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(23 * 16, 5 * 16), new Size(16, 16));
            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
            graphics.DrawString("Explore different game mechanics and features", _mainFont, _whiteBrush, 650, 556);
            destRect =
                new Rectangle(new Point(550, 556),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(15, 16));
            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void EnemyHeroAnimation(Graphics graphics) {
            Rectangle destination =
                new Rectangle(new Point(0 + _enemyPosition, 810),
                    new Size(32, 32));
            Rectangle res = new Rectangle(new Point(12 * 16 + _currentFrameForEnemies * 16, 74), new Size(16, 21));
            graphics.DrawImage(MainSprites, destination, res, GraphicsUnit.Pixel);
            destination =
                new Rectangle(new Point(0 + _enemyPosition - 72, 810),
                    new Size(32, 32));
            res = new Rectangle(new Point(27 * 16 + _currentFrameForEnemies * 16, 5 * 16), new Size(16, 16));
            graphics.DrawImage(MainSprites, destination, res, GraphicsUnit.Pixel);

            _enemyPosition += 3;
            if (_enemyPosition >= 1500)
                _enemyPosition = 0;

            if (_currentFrameForEnemies < maxFramesForEnemies - 1)
                _currentFrameForEnemies++;
            else
                _currentFrameForEnemies = 0;
        }
        private void FillBackground(Graphics graphics) {
            for (int i = 0; i < 31; i++) {
                for (int j = 0; j < 53; j++) {
                    Rectangle destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
                    var srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                    graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    if (Randomizer.Random(100) > 98) {
                        srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
                        graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
            }
        }
        private void DrawBestScores(Graphics graphics, GameEngine gameEngine) {
            if (_results == null) {
                _results = gameEngine.DataInterlayer.GetBestScores();
            }
            int counter = 1;
            foreach (var result in _results) {
                graphics.DrawString($"{counter}. {result.Value}: {result.Key}",
                    _mainFont, _whiteBrush, 520, 180 + 30 * counter);
                counter++;
            }
        }
        private void DrawSettings(Graphics graphics, GameEngine gameEngine) {
            Rectangle selected = new Rectangle(520, 210 + gameEngine.CurrentSubAction * 40, 940, 40);
            graphics.FillRectangle(_darkBrush,
                selected);
            graphics.DrawString($"Difficulty: {gameEngine.DataInterlayer.Settings.Difficulty}",
                _mainFont, _whiteBrush, 520, 210);
            graphics.DrawString($"Size X: {gameEngine.DataInterlayer.Settings.SizeX}",
                _mainFont, _whiteBrush, 520, 250);
            graphics.DrawString($"Size Y: {gameEngine.DataInterlayer.Settings.SizeY}",
                _mainFont, _whiteBrush, 520, 290);
            graphics.DrawString($"Fps: {gameEngine.DataInterlayer.Settings.Fps}",
                _mainFont, _whiteBrush, 520, 330);
            graphics.DrawString($"Tickrate: {gameEngine.DataInterlayer.Settings.TickRate}",
                _mainFont, _whiteBrush, 520, 370);
            graphics.DrawString($"Use A and S to change parameters, press ENTER to save",
                _mainFont, _whiteBrush, 520, 700);
        }
    }
}