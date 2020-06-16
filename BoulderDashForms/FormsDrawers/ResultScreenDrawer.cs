using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using ClassLibrary;
using ClassLibrary.Entities;

namespace BoulderDashForms.FormsDrawers {
    public class ResultScreenDrawer : FormDrawer {
        private readonly StringFormat _alignCenter;

        public ResultScreenDrawer() {
            FontCollection = new PrivateFontCollection();
            FontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, @"Fonts\monogram.ttf"));
            FontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, @"Fonts\ThaleahFat.ttf"));
            FontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, @"Fonts\m5x7.ttf"));
            Family1 = FontCollection.Families[0];
            Family2 = FontCollection.Families[1];
            var family3 = FontCollection.Families[2];
            MenuFont = new Font(Family1, 28);
            BoldFont = new Font(family3, 28);
            HeaderFont = new Font(family3, 38);
            MainFont = new Font(Family2, 30);
            _alignCenter = new StringFormat {
                LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center
            };
        }

        public void DrawResults(
            Graphics graphics,
            GameStatusEnum gameStatus,
            string playerName,
            int playerScore,
            Dictionary<string, int[]> allPlayerScores,
            int width, int height
        ) {
            var center = width / 2;
            FillBackground(graphics, gameStatus, width, height);
            DrawCenterBlock(graphics, width, height);
            var header = gameStatus == GameStatusEnum.WinScreen ? "LEVEL COMPLETED" : "YOU LOST";
            graphics.DrawString(header, HeaderFont, RedBrush, center, 100, _alignCenter);
            graphics.DrawString($"{playerName} total score: {playerScore}", MainFont, WhiteBrush, center, 160,
                _alignCenter);
            var counter = 0;
            graphics.DrawString("Level results", BoldFont, WhiteBrush, center, 250, _alignCenter);
            foreach (var (key, value) in allPlayerScores) {
                graphics.DrawString(key, MainFont, RedBrush, center, 300 + counter * 80, _alignCenter);
                graphics.DrawString($"{value[0].ToString()}...................{value[1].ToString()}",
                    MainFont, WhiteBrush, center, 335 + counter * 80, _alignCenter);
                counter++;
            }
            graphics.DrawString("Press ENTER to continue", MainFont, WhiteBrush, center, 750, _alignCenter);
        }

        private void FillBackground(Graphics graphics, GameStatusEnum gameStatus, int width, int height) {
            for (var i = 0; i < height/16; i++)
            for (var j = 0; j < width/16; j++) {
                var destRect =
                    new Rectangle(new Point(j * GameEntity.FormsSize * 2, i * GameEntity.FormsSize * 2),
                        new Size(GameEntity.FormsSize * 2, GameEntity.FormsSize * 2));
                var srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
            var semiTransBrush = gameStatus == GameStatusEnum.WinScreen
                ? new SolidBrush(Color.FromArgb(100, 4, 114, 77))
                : new SolidBrush(Color.FromArgb(100, 178, 13, 48));
            var rect = new Rectangle(0, 0, width, height);
            graphics.FillRectangle(semiTransBrush, rect);
        }

        private void DrawCenterBlock(Graphics graphics, int width, int height) {
            var rect = new Rectangle(width/3, 0, width/3, height);
            graphics.FillRectangle(DarkBrush, rect);
        }
    }
}