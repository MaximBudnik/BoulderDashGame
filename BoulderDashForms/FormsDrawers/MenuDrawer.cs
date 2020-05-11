using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using ClassLibrary;
using ClassLibrary.Entities;

namespace BoulderDashForms.FormsDrawers {
    public class MenuDrawer : FormDrawer {
        private readonly SolidBrush _redBrush = new SolidBrush(Color.FromArgb(255, 218, 78, 56));
        private readonly SolidBrush _redBrushHalfTransparent = new SolidBrush(Color.FromArgb(200, 218, 78, 56));
        private readonly SolidBrush _redBrushTransparent = new SolidBrush(Color.FromArgb(130, 218, 78, 56));
        private readonly SolidBrush _whiteBrush = new SolidBrush(Color.FromArgb(255, 249, 245, 255));
        private readonly SolidBrush _darkBrush = new SolidBrush(Color.FromArgb(255, 34, 29, 35));
        private readonly string[] _menuActions = {"Continue", "New game", "Settings", "Scores", "Help", "Exit",};

        private Font _headerFont;
        private Font _mainFont;
        private int _enemyPosition = 0;
        private int _currentFrameForEnemies = 0;
        private int maxFramesForEnemies = 4;
        private int rightBlockWidth = 1000;

        public void nullRightBlockWidth() {
            rightBlockWidth = 0;
        }

        public MenuDrawer() {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\monogram.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\ThaleahFat.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\m5x7.ttf"));
            FontFamily family1 = fontCollection.Families[0];
            FontFamily family2 = fontCollection.Families[1];
            FontFamily family3 = fontCollection.Families[2];

            _menuFont = new Font(family1, 28);
            _boldFont =   new Font(family3,28);
            _headerFont =new Font(family3, 38);
            _mainFont = new Font(family2, 30);
        }
        
        public void DrawMenu(Graphics graphics, GameEngine gameEngine) {
            for (int i = 0; i < 31; i++) {
                for (int j = 0; j < 53; j++) {
                    Rectangle destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
                    var srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    if (Randomizer.Random(100)>98) {
                        srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
                        graphics.DrawImage(_icons, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
            }
            
            
            //shadow rectangle
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            Rectangle rect = new Rectangle(0, 0, 1500, 900);
            graphics.FillRectangle(semiTransBrush, rect);
            
            //front sprites
            
            Rectangle destination =
                new Rectangle(new Point(0 +_enemyPosition , 810),
                    new Size(32, 32));
            Rectangle res = new Rectangle(new Point(12 * 16 + _currentFrameForEnemies * 16, 74), new Size(16, 21));
            graphics.DrawImage(_mainSprites, destination, res, GraphicsUnit.Pixel);
             destination =
                new Rectangle(new Point(0 +_enemyPosition - 72, 810),
                    new Size(32, 32));
             res = new Rectangle(new Point(27 * 16 + _currentFrameForEnemies * 16, 5 * 16), new Size(16, 16));
            graphics.DrawImage(_mainSprites, destination, res, GraphicsUnit.Pixel);
            
            
            _enemyPosition += 3;
            if (_enemyPosition >= 1500)
                _enemyPosition = 0;

            if (_currentFrameForEnemies < maxFramesForEnemies - 1)
                _currentFrameForEnemies++;
            else
                _currentFrameForEnemies = 0;
            
            
            
            //menu rectangle
            rect = new Rectangle(150, 0, 350, 900);
            graphics.FillRectangle(_darkBrush, rect);


            graphics.DrawString("DUNGEON", _headerFont, _redBrush,160,30);
            graphics.DrawString("DASH", _headerFont, _redBrush, 160, 68);

            for (int i = 0; i < _menuActions.Length; i++) {
                if (gameEngine.CurrentMenuAction==i) {
                    graphics.DrawString(_menuActions[i], _mainFont, _whiteBrush,170,200+i*36);
                }
                else {
                    graphics.DrawString(_menuActions[i], _mainFont, _redBrush,160,200+i*36);
                }
            }
            
            //right zone (center rectangle)
            Rectangle rightBlock = new Rectangle(500, 150, rightBlockWidth, 600);
            graphics.FillRectangle(gameEngine.isActionActive ? _redBrushHalfTransparent: _redBrushTransparent, rightBlock);
            if (rightBlockWidth <1000) {
                rightBlockWidth += 100;
            }
            
            //right zone content
            if (rightBlockWidth < 1000) return;
            string blockHeader="";
            switch (gameEngine.CurrentMenuAction) {
                case 0:
                    blockHeader = "Select save";
                    break;
                case 1:
                    blockHeader = "Create new game";
                    break;
                case 2:
                    blockHeader = "Adjust your settings";
                    break;
                case 3:
                    blockHeader = "Best results";
                    break;
                case 4:
                    blockHeader = "Game guide";
                    break;
                case 5:
                    blockHeader = "Press ENTER to exit game";
                    break;
            }
            graphics.DrawString(blockHeader, _headerFont, _whiteBrush, 520, 160);
        }
        
    }
}