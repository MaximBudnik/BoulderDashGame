using System;
using System.Collections.Generic;
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
        private Image attack;
        private Image effects;
        private List<Action> defferedFx;

        public FormsDrawer() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
            string attackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");
            string effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");
            mainSprites = new Bitmap(mainsSpritesPath);
            secondarySprites = new Bitmap(secondarySpritesPath);
            Tileset = new Bitmap(TilesetPath);
            icons = new Bitmap(iconsPath);
            effects = new Bitmap(effectPath);
            attack = new Bitmap(attackPath);
        }

        private void PlayerAnimation(Player player, Graphics graphics, int i, int j) {
            Rectangle destRect =
                new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                    new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
            switch (player.currentAnimation) {
                case 1: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                }
                case 2: {
                    var srcRect = new Rectangle(new Point(16 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 3: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(0 * 16 + player.currentFrame * 16, 0 * 16),
                        new Size(player.reverse * 16, 16));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    defferedFx.Add(
                        () => graphics.DrawImage(attack, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 4: {
                    var srcRect = new Rectangle(new Point(16 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(5 * 32 + player.currentFrame * 32, 5 * 16),
                        new Size(32, 32));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    defferedFx.Add(
                        () => graphics.DrawImage(effects, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 5: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(6 * 16 + player.currentFrame * 16, 2 * 16),
                        new Size(16, 16));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    defferedFx.Add(
                        () => graphics.DrawImage(effects, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 6: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(11 * 16 + player.currentFrame * 16, 2 * 16),
                        new Size(16, 16));
                    graphics.DrawImage(effects, destRect, srcRect, GraphicsUnit.Pixel);
                    
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                default: {
                    var srcRect = new Rectangle(new Point(9 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                }
            }

            if (player.currentFrame < player.framesLimit - 1)
                player.currentFrame++;
            else
                player.currentFrame = 0;
        }

        public void DrawGame(Graphics graphics, Level currentLevel, Player player) {
            defferedFx = new List<Action>();
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
                            PlayerAnimation(player, graphics, i, j);
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
                foreach (Action a in defferedFx) {
                    a.Invoke();
                }
            }
        }
    }
}