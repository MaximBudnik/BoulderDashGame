using System;
using System.Drawing;
using System.IO;
using ClassLibrary.Entities;
using ClassLibrary.Matrix;
using ClassLibrary.Entities.Player;

namespace BoulderDashForms {
    public class FormsDrawer {
        private Image mainSprites;
        private Image secondarySprites;
        private Image Tileset;
        private Image icons;

        public FormsDrawer() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
            mainSprites = new Bitmap(mainsSpritesPath);
            secondarySprites = new Bitmap(secondarySpritesPath);
            Tileset = new Bitmap(TilesetPath);
            icons = new Bitmap(iconsPath);
        }

        private Rectangle GetPlayerAnimation(Player player) {
            Rectangle res;
            if (player.currentAnimation == 1) {
                res = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73), new Size(16, 23));
            }
            else if (player.currentAnimation == 2) {
                res = new Rectangle(new Point(16 * 16 + player.currentFrame * 16, 76), new Size(16, 20));
            }
            else {
                res = new Rectangle(new Point(8 * 16 + player.currentFrame * 16, 76), new Size(16, 20));
            }

            if (player.currentFrame < player.framesLimit-1)
                player.currentFrame++;
            else
                player.currentFrame = 0;
            return res;
        }



        public void DrawGame(Graphics graphics, Level currentLevel, Player player) {
            for (int i = 0; i < currentLevel.Width; i++) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    Rectangle destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
                    Rectangle srcRect;

                    void DrawFloorTile() {
                        //TODO: make versatile tile floor with random
                        srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                    switch (currentLevel[i, j].EntityType) {
                        case 0:
                            DrawFloorTile();
                            srcRect = GetPlayerAnimation(player);
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 1:
                            srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 2:
                            srcRect = new Rectangle(new Point(9 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(Tileset, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 3:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(7 * 16, 4 * 16), new Size(16, 16));
                            graphics.DrawImage(Tileset, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 4:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
                            graphics.DrawImage(icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 5:
                            srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 6:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(23 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 7:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 8:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(7 * 16, 10 * 16), new Size(16, 16));
                            graphics.DrawImage(secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 10:
                            srcRect = new Rectangle(new Point(2 * 16, 3 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 11:
                            srcRect = new Rectangle(new Point(4 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 12:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 20:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(20 * 16, 2 * 12), new Size(16, 22));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 21:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                            graphics.DrawImage(secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 22:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(16, 16));
                            graphics.DrawImage(secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 23:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(6 * 16, 15 * 16), new Size(16, 16));
                            graphics.DrawImage(icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        default:
                            srcRect = new Rectangle(new Point(0 * 16, 0 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                    }
                }
            }
        }
    }
}