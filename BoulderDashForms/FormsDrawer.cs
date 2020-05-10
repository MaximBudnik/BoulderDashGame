using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Matrix;
using ClassLibrary.Entities.Player;

namespace BoulderDashForms {
    public class FormsDrawer {
        private readonly Image _mainSprites;
        private readonly Image _secondarySprites;
        private readonly Image _tileset;
        private readonly Image _icons;
        private readonly Image _attack;
        private readonly Image _effects;
        private readonly Image _bars;
        private readonly Image _barsEmpty;
        private List<Action> _defferedFx;

        public FormsDrawer() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
            string attackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");
            string effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");
            string barsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\bars.png");
            string barsEmptyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\barsEmpty.png");
            _mainSprites = new Bitmap(mainsSpritesPath);
            _secondarySprites = new Bitmap(secondarySpritesPath);
            _tileset = new Bitmap(TilesetPath);
            _icons = new Bitmap(iconsPath);
            _effects = new Bitmap(effectPath);
            _attack = new Bitmap(attackPath);
            _bars = new Bitmap(barsPath);
            _barsEmpty = new Bitmap(barsEmptyPath);
        }

        private void PlayerAnimation(Player player, Graphics graphics, int i, int j) {
            
            Rectangle destRect =
                new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                    new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
            switch (player.currentAnimation) {
                case 1: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                }
                case 2: {
                    var srcRect = new Rectangle(new Point(16 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 3: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(0 * 16 + player.currentFrame * 16, 0 * 16),
                        new Size(player.reverse * 16, 16));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    _defferedFx.Add(
                        () => graphics.DrawImage(_attack, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 4: {
                    var srcRect = new Rectangle(new Point(16 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(5 * 32 + player.currentFrame * 32, 5 * 16),
                        new Size(32, 32));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    _defferedFx.Add(
                        () => graphics.DrawImage(_effects, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 5: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(6 * 16 + player.currentFrame * 16, 2 * 16),
                        new Size(16, 16));
                    destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 3, GameEntity.formsSize * 3));
                    _defferedFx.Add(
                        () => graphics.DrawImage(_effects, destRect, srcRect, GraphicsUnit.Pixel)
                    );
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                case 6: {
                    var srcRect = new Rectangle(new Point(12 * 16 + player.currentFrame * 16, 73),
                        new Size(player.reverse * 16, 23));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(11 * 16 + player.currentFrame * 16, 2 * 16),
                        new Size(16, 16));
                    graphics.DrawImage(_effects, destRect, srcRect, GraphicsUnit.Pixel);
                    
                    if (player.currentFrame == player.framesLimit - 1) player.SetAnimation(0);
                    break;
                }
                default: {
                    var srcRect = new Rectangle(new Point(9 * 16 + player.currentFrame * 16, 76),
                        new Size(player.reverse * 16, 20));
                    graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                }
            }

            if (player.currentFrame < player.framesLimit - 1)
                player.currentFrame++;
            else
                player.currentFrame = 0;
        }

        private Rectangle GetWalkerAnimation(EnemyWalker enemy){
            Rectangle res = new Rectangle(new Point(23 * 16+ enemy.currentFrame*16, 5 * 16), new Size(16, 16));
            if (enemy.currentFrame < enemy.idleFrames - 1)
                enemy.currentFrame++;
            else
                enemy.currentFrame = 0;
            return res;
        }

        private Rectangle GetDiggerAnimation(EnemyDigger enemy) {
            Rectangle res  = new Rectangle(new Point(27 * 16+ enemy.currentFrame*16, 7 * 16), new Size(16, 16));
            if (enemy.currentFrame < enemy.idleFrames - 1)
                enemy.currentFrame++;
            else
                enemy.currentFrame = 0;
            return res;
        }

        private Rectangle GetDiamondAnimation(Diamond diamond) {
            Rectangle res;
            if (diamond.currentFrame<=256) {
                res =new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
            }
            else {
                res =new Rectangle(new Point(2 * 16, 7 * 16), new Size(16, 16));
            }
            if (diamond.currentFrame < diamond.idleFrames - 1)
                diamond.currentFrame++;
            else
                diamond.currentFrame = 0;
            return res;
        }
        
        public void DrawGame(Graphics graphics, Level currentLevel, Player player) {
            _defferedFx = new List<Action>();
            for (int i = 0; i < currentLevel.Width; i++) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    Rectangle destRect =
                        new Rectangle(new Point(j * GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
                    Rectangle srcRect;

                    void DrawFloorTile() {
                        //TODO: make versatile tile floor with random
                        srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                    switch (currentLevel[i, j].EntityType) {
                        case 0:
                            DrawFloorTile();
                            PlayerAnimation(player, graphics, i, j);
                            break;
                        case 1:
                            srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 2:
                            srcRect = new Rectangle(new Point(9 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(_tileset, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 3:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(7 * 16, 4 * 16), new Size(16, 16));
                            graphics.DrawImage(_tileset, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 4:
                            DrawFloorTile();
                            srcRect = GetDiamondAnimation((Diamond) currentLevel[i, j]);
                            graphics.DrawImage(_icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 5:
                            srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 6:
                            DrawFloorTile();
                            srcRect = GetWalkerAnimation((EnemyWalker)currentLevel[i, j]);
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 7:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 8:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(7 * 16, 10 * 16), new Size(16, 16));
                            graphics.DrawImage(_secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 10:
                            srcRect = new Rectangle(new Point(2 * 16, 3 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 11:
                            srcRect = new Rectangle(new Point(4 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 12:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 13:
                            DrawFloorTile();
                            srcRect = GetDiggerAnimation((EnemyDigger) currentLevel[i, j]);
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 20:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(20 * 16, 2 * 12), new Size(16, 22));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 21:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                            graphics.DrawImage(_secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 22:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(16, 16));
                            graphics.DrawImage(_secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 23:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(6 * 16, 15 * 16), new Size(16, 16));
                            graphics.DrawImage(_icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        default:
                            srcRect = new Rectangle(new Point(0 * 16, 0 * 16), new Size(16, 16));
                            graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                    }
                }
                foreach (Action a in _defferedFx) {
                    a.Invoke();
                }
            }

            DrawInterface(graphics, currentLevel, player);
        }
        //     _gameInterface.DrawUpperInterface(currentLevel.LevelName, player.Score, currentLevel.Aim);
        //     _gameInterface.DrawPlayerInterface(currentLevel.DiamondsQuantity, player.CollectedDiamonds,
        //         player.MaxEnergy, player.Energy, player.MaxHp, player.Hp, player.Name, player.Inventory);
        private void DrawInterface(Graphics graphics, Level currentLevel, Player player) {
            for (int i = 0; i < player.MaxHp; i++) {
                Rectangle destRect =
                    new Rectangle(new Point(10*i, 10),
                         new Size(25, 25));
                Rectangle srcRect = new Rectangle(new Point(20*16, 16*16), new Size(16, 16));
                graphics.DrawImage(_mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
            // Rectangle destRect =
            //     new Rectangle(new Point(0, 0),
            //         new Size(500, 150));
             
        }
    }
}