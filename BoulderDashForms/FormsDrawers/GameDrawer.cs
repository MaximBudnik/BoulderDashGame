using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;

namespace BoulderDashForms.FormsDrawers {
    public class GameDrawer : FormDrawer {
        private readonly Image _mainSprites;
        private readonly Image _secondarySprites;
        private readonly Image _tileset;
        private readonly Image _icons;
        private readonly Image _attack;
        private readonly Image _effects;
        private readonly Image _hpFull;
        private readonly Image _hpEmpty;
        private readonly Image _shield;
        private readonly Image _energy;
        private readonly Image _keyboard;
        private List<Action> _defferedFx;

        
        private readonly Brush _guiBrush = Brushes.Crimson;

        public GameDrawer() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
            string attackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");
            string effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");
            string hpFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart.png");
            string hpEmptyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart_empty.png");
            string shieldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\shield.png");
            string energyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\energy.png");
            string keyboardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\keyboard.png");

            _mainSprites = new Bitmap(mainsSpritesPath);
            _secondarySprites = new Bitmap(secondarySpritesPath);
            _tileset = new Bitmap(TilesetPath);
            _icons = new Bitmap(iconsPath);
            _effects = new Bitmap(effectPath);
            _attack = new Bitmap(attackPath);
            _hpFull = new Bitmap(hpFullPath);
            _hpEmpty = new Bitmap(hpEmptyPath);
            _shield = new Bitmap(shieldPath);
            _energy = new Bitmap(energyPath);
            _keyboard = new Bitmap(keyboardPath);

            
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

        private Rectangle GetWalkerAnimation(EnemyWalker enemy) {
            Rectangle res = new Rectangle(new Point(23 * 16 + enemy.currentFrame * 16, 5 * 16), new Size(16, 16));
            if (enemy.currentFrame < enemy.idleFrames - 1)
                enemy.currentFrame++;
            else
                enemy.currentFrame = 0;
            return res;
        }

        private Rectangle GetDiggerAnimation(EnemyDigger enemy) {
            Rectangle res = new Rectangle(new Point(27 * 16 + enemy.currentFrame * 16, 7 * 16), new Size(16, 16));
            if (enemy.currentFrame < enemy.idleFrames - 1)
                enemy.currentFrame++;
            else
                enemy.currentFrame = 0;
            return res;
        }

        private Rectangle GetDiamondAnimation(Diamond diamond) {
            Rectangle res;
            if (diamond.currentFrame <= 256) {
                res = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
            }
            else {
                res = new Rectangle(new Point(2 * 16, 7 * 16), new Size(16, 16));
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
                            srcRect = GetWalkerAnimation((EnemyWalker) currentLevel[i, j]);
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
            DrawKeys(graphics,player);
        }
        //TODO: v obuchalke skazat chto рядоми с камнями есть шанс промахнуться по противнику
        //      currentLevel.Aim
        //     currentLevel.DiamondsQuantity, player.CollectedDiamonds, dont forget to make text on actions (kill/lucky box)

        private void DrawInterface(Graphics graphics, Level currentLevel, Player player) {
            //Draw hp bar
            Rectangle destRect;
            Rectangle srcRect;
            for (int i = 0; i < player.MaxHp; i++) {
                destRect =
                    new Rectangle(new Point(24 * i, 16),
                        new Size(32, 32));
                if (player.Hp >= i) {
                    srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                    graphics.DrawImage(_hpFull, destRect, srcRect, GraphicsUnit.Pixel);
                }
                else {
                    srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                    graphics.DrawImage(_hpEmpty, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            //Draw armor
            for (int i = 0; i < player.Inventory.ArmorLevel; i++) {
                destRect =
                    new Rectangle(new Point(16 * i, 24),
                        new Size(32, 32));
                srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                graphics.DrawImage(_shield, destRect, srcRect, GraphicsUnit.Pixel);
            }

            //mana

            for (int i = 0; i < player.Energy; i++) {
                destRect =
                    new Rectangle(new Point(16 * i, 64),
                        new Size(32, 32));
                srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                graphics.DrawImage(_energy, destRect, srcRect, GraphicsUnit.Pixel);
            }

            destRect =
                new Rectangle(new Point(256, 16),
                    new Size(32, 32));
            switch (player.Inventory.SwordLevel) {
                case 0:
                    srcRect = new Rectangle(new Point(6 * 16, 18 * 16), new Size(0, 0));
                    break;
                case 1:
                    srcRect = new Rectangle(new Point(2 * 16, 18 * 16), new Size(15, 15));
                    break;
                case 2:
                    srcRect = new Rectangle(new Point(3 * 16, 18 * 16), new Size(15, 15));
                    break;
                case 3:
                    srcRect = new Rectangle(new Point(4 * 16, 18 * 16), new Size(15, 15));
                    break;
                case 4:
                    srcRect = new Rectangle(new Point(5 * 16, 18 * 16), new Size(15, 15));
                    break;
                default:
                    srcRect = new Rectangle(new Point(6 * 16, 18 * 16), new Size(15, 15));
                    break;
            }
            graphics.DrawImage(_icons, destRect, srcRect, GraphicsUnit.Pixel);

            //bombs and converters
            for (int i = 0; i < player.Inventory.TntQuantity; i++) {
                destRect =
                    new Rectangle(new Point(16 * i + 288, 16),
                        new Size(32, 32));
                srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(15, 15));
                graphics.DrawImage(_secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < player.Inventory.StoneInDiamondsConverterQuantity; i++) {
                destRect =
                    new Rectangle(new Point(16 * i + 288, 24),
                        new Size(32, 32));
                srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                graphics.DrawImage(_secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
            }

            //diamonds left
            for (int i = 0; i < ( currentLevel.DiamondsQuantity - player.CollectedDiamonds); i++) {
                destRect =
                    new Rectangle(new Point(8 * i + 1000, 32),
                        new Size(16, 16));
                srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
                graphics.DrawImage(_icons, destRect, srcRect, GraphicsUnit.Pixel);
            }

            //text elements
            graphics.DrawString($"{currentLevel.Aim}", _menuFont, _guiBrush, 1000, 8);
            graphics.DrawString($"Level {currentLevel.LevelName}", _boldFont, _guiBrush, 1200, 8);
            graphics.DrawString($"{player.Name}", _boldFont, _guiBrush, 1200, 30);

            graphics.DrawString($"Score x{player.ScoreMultiplier.ToString()}", _boldFont, _guiBrush, 1330, 8);
            graphics.DrawString(player.Score.ToString(), _boldFont, _guiBrush, 1330, 30);
        }

        private void DrawKeys(Graphics graphics, Player player) {
            Keyboard key = player.Keyboard;
            Rectangle destRect;
            Rectangle srcRect;
            
            //w
            destRect =
                new Rectangle(new Point( 1300, 750),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(4 * 16, 2 * 16+key.W*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //a
            destRect =
                new Rectangle(new Point( 1268, 782),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(3 * 16, 3 * 16+key.A*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            
            //s
            destRect =
                new Rectangle(new Point( 1300, 782),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(4 * 16, 3 * 16+key.S*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //d
            destRect =
                new Rectangle(new Point( 1332, 782),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(5 * 16, 3 * 16+key.D*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //space
            if (key.Space==0) {
                destRect =
                    new Rectangle(new Point( 1268, 814),
                        new Size(160, 32));
                srcRect = new Rectangle(new Point(5 * 16, 5 * 16+key.Space*16), new Size(80, 16));
                graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else {
                destRect =
                    new Rectangle(new Point( 1268, 814),
                        new Size(144, 32));
                srcRect = new Rectangle(new Point(6 * 16, 5 * 16+key.Space*16), new Size(64, 16));
                graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            }
            //t
            destRect =
                new Rectangle(new Point( 1396, 750),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(7 * 16, 2 * 16+key.T*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //q
            destRect =
                new Rectangle(new Point( 1268, 750),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(3 * 16, 2 * 16+key.Q*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //e
            destRect =
                new Rectangle(new Point( 1332, 750),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(5 * 16, 2 * 16+key.E*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
            //r
            destRect =
                new Rectangle(new Point( 1364, 750),
                    new Size(32, 32));
            srcRect = new Rectangle(new Point(6 * 16, 2 * 16+key.R*16), new Size(16, 16));
            graphics.DrawImage(_keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        
    }
}