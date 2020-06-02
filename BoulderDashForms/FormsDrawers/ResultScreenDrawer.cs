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
            Dictionary<string, int[]> allPlayerScores
        ) {
            FillBackground(graphics, gameStatus);
            DrawCenterBlock(graphics);
            var header = gameStatus == GameStatusEnum.WinScreen ? "LEVEL COMPLETED" : "YOU LOST";
            graphics.DrawString(header, HeaderFont, RedBrush, 750, 100, _alignCenter);
            graphics.DrawString($"{playerName} total score: {playerScore}", MainFont, WhiteBrush, 750, 160,
                _alignCenter);
            var counter = 0;
            graphics.DrawString("Level results", BoldFont, WhiteBrush, 750, 250, _alignCenter);
            foreach (var (key, value) in allPlayerScores) {
                graphics.DrawString(key, MainFont, RedBrush, 750, 300 + counter * 80, _alignCenter);
                graphics.DrawString($"{value[0].ToString()}...................{value[1].ToString()}",
                    MainFont, WhiteBrush, 750, 335 + counter * 80, _alignCenter);
                counter++;
            }
            graphics.DrawString("Press ENTER to continue", MainFont, WhiteBrush, 750, 750, _alignCenter);
        }

        private void FillBackground(Graphics graphics, GameStatusEnum gameStatus) {
            for (var i = 0; i < 31; i++)
            for (var j = 0; j < 53; j++) {
                var destRect =
                    new Rectangle(new Point(j * GameEntity.FormsSize * 2, i * GameEntity.FormsSize * 2),
                        new Size(GameEntity.FormsSize * 2, GameEntity.FormsSize * 2));
                var srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
            var semiTransBrush = gameStatus == GameStatusEnum.WinScreen
                ? new SolidBrush(Color.FromArgb(100, 4, 114, 77))
                : new SolidBrush(Color.FromArgb(100, 178, 13, 48));
            var rect = new Rectangle(0, 0, 1500, 900);
            graphics.FillRectangle(semiTransBrush, rect);
        }

        private void DrawCenterBlock(Graphics graphics) {
            var rect = new Rectangle(500, 0, 500, 900);
            graphics.FillRectangle(DarkBrush, rect);
        }
    }
}