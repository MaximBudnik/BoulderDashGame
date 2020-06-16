using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable;
using ClassLibrary.Entities.Enemies;
using ClassLibrary.Entities.Enemies.SmartEnemies;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;

namespace BoulderDashForms.FormsDrawers {
    public class GameDrawer : FormDrawer {
        private readonly int kf = 6; //parameter to beautify hero sprite
        protected readonly SolidBrush ShieldBrush = new SolidBrush(Color.FromArgb(80, 220, 200, 100));
        private List<Action> _defferedFx;
        private float scale = 2f;

        private void DrawPlayerAnimation(Player player, Graphics graphics, int i, int j) {
            var hero = DetectHeroSprite(player);
            var height = 28 - kf;
            var width = 16;
            var pixelY = hero + kf;
            var destRect =
                new Rectangle(
                    new Point((int) (j * GameEntity.FormsSize * scale), (int) (i * GameEntity.FormsSize * scale)),
                    new Size((int) (GameEntity.FormsSize * scale), (int) (GameEntity.FormsSize * scale)));
            Rectangle srcRect;
            switch (player.PlayerAnimator.CurrentPlayerAnimationEnum) {
                case PlayerAnimationsEnum.Move: {
                    srcRect = new Rectangle(new Point(12 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    break;
                }
                case PlayerAnimationsEnum.GetDamage: {
                    srcRect = new Rectangle(new Point(16 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    NullifyPlayerAnimation(player);
                    break;
                }
                case PlayerAnimationsEnum.Attack: {
                    srcRect = new Rectangle(new Point(0 * 16 + player.CurrentFrame * 16, 0 * 16),
                        new Size(player.PlayerAnimator.Reverse * 16, 16));
                    var tmpDestRect =
                        new Rectangle(
                            new Point((int) (j * GameEntity.FormsSize * scale),
                                (int) (i * GameEntity.FormsSize * scale)),
                            new Size(GameEntity.FormsSize * 3, GameEntity.FormsSize * 3));
                    var rect = srcRect;
                    _defferedFx.Add(
                        () => graphics.DrawImage(Attack, tmpDestRect, rect, GraphicsUnit.Pixel)
                    );
                    srcRect = new Rectangle(new Point(12 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    NullifyPlayerAnimation(player);
                    break;
                }
                case PlayerAnimationsEnum.Explosion: {
                    srcRect = new Rectangle(new Point(5 * 32 + player.CurrentFrame * 32, 5 * 16),
                        new Size(32, 32));
                    var tmpDestRect =
                        new Rectangle(
                            new Point((int) (j * GameEntity.FormsSize * scale),
                                (int) (i * GameEntity.FormsSize * scale)),
                            new Size(GameEntity.FormsSize * 3, GameEntity.FormsSize * 3));
                    var rect = srcRect;
                    _defferedFx.Add(
                        () => graphics.DrawImage(Effects, tmpDestRect, rect, GraphicsUnit.Pixel)
                    );
                    srcRect = new Rectangle(new Point(16 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    NullifyPlayerAnimation(player);
                    break;
                }
                case PlayerAnimationsEnum.Teleport: {
                    srcRect = new Rectangle(new Point(6 * 16 + player.CurrentFrame * 16, 2 * 16),
                        new Size(16, 16));
                    var tmpDestRect =
                        new Rectangle(
                            new Point((int) (j * GameEntity.FormsSize * scale),
                                (int) (i * GameEntity.FormsSize * scale)),
                            new Size(GameEntity.FormsSize * 3, GameEntity.FormsSize * 3));
                    var rect = srcRect;
                    _defferedFx.Add(
                        () => graphics.DrawImage(Effects, tmpDestRect, rect, GraphicsUnit.Pixel)
                    );
                    srcRect = new Rectangle(new Point(12 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    NullifyPlayerAnimation(player);
                    break;
                }
                case PlayerAnimationsEnum.Converting: {
                    srcRect = new Rectangle(new Point(11 * 16 + player.CurrentFrame * 16, (int) (scale * 16)),
                        new Size(16, 16));
                    graphics.DrawImage(Effects, destRect, srcRect, GraphicsUnit.Pixel);
                    srcRect = new Rectangle(new Point(12 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    NullifyPlayerAnimation(player);
                    break;
                }
                default: {
                    srcRect = new Rectangle(new Point(9 * 16 + player.CurrentFrame * 16, pixelY),
                        new Size(player.PlayerAnimator.Reverse * width, height));
                    break;
                }
            }
            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
            if (player.CurrentFrame < player.PlayerAnimator.FramesLimit - 1)
                player.CurrentFrame++;
            else
                player.CurrentFrame = 0;
        }
        private static int DetectHeroSprite(Player player) {
            int hero;
            switch (player.Hero) {
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
                default:
                    throw new Exception("Unexpected player sprite");
            }
            return hero;
        }
        private void NullifyPlayerAnimation(Player player) {
            if (player.CurrentFrame == player.PlayerAnimator.FramesLimit - 1) player.SetAnimation(0);
        }

        private Rectangle GetSmartSkeletonAnimation(SmartSkeleton enemy) {
            if (enemy == null) return new Rectangle();
            var res = new Rectangle(new Point(23 * 16 + enemy.CurrentFrame * 16, 5 * 16), new Size(16, 16));
            ChangeEnemyAnimation(enemy);
            return res;
        }

        private Rectangle GetSmartPeacefulAnimation(SmartPeaceful enemy) {
            if (enemy == null) return new Rectangle();
            var res = new Rectangle(new Point(23 * 16 + enemy.CurrentFrame * 16, 1 * 16), new Size(16, 16));
            ChangeEnemyAnimation(enemy);
            return res;
        }
        private Rectangle GetSmartDevilAnimation(SmartDevil enemy) {
            if (enemy == null) return new Rectangle();
            var res = new Rectangle(new Point(23 * 16 + enemy.CurrentFrame * 16, 21 * 16), new Size(16, 16));
            ChangeEnemyAnimation(enemy);
            return res;
        }

        private Rectangle GetEnemyWalkerAnimation(EnemyWalker enemy) {
            if (enemy == null) return new Rectangle();
            var res = new Rectangle(new Point(23 * 16 + enemy.CurrentFrame * 16, 11 * 16), new Size(16, 16));
            ChangeEnemyAnimation(enemy);
            return res;
        }

        private Rectangle GetDiggerAnimation(EnemyDigger enemy) {
            if (enemy == null) return new Rectangle();
            var res = new Rectangle(new Point(27 * 16 + enemy.CurrentFrame * 16, 7 * 16), new Size(16, 16));
            ChangeEnemyAnimation(enemy);
            return res;
        }
        private static void ChangeEnemyAnimation(Enemy enemy) {
            if (enemy.CurrentFrame < enemy.IdleFrames - 1)
                enemy.CurrentFrame++;
            else
                enemy.CurrentFrame = 0;
        }

        private Rectangle GetDiamondAnimation(Diamond diamond) {
            Rectangle res;
            res = diamond.CurrentFrame <= 256
                ? new Rectangle(new Point(1 * 16, 7 * 16),
                    new Size(16, 16))
                : new Rectangle(new Point(2 * 16, 7 * 16),
                    new Size(16, 16));
            if (diamond.CurrentFrame < diamond.IdleFrames - 1)
                diamond.CurrentFrame++;
            else
                diamond.CurrentFrame = 0;
            return res;
        }

        private Rectangle GetFishAnimation(GoldenFish fish) {
            var res = fish.CurrentFrame <= 16
                ? new Rectangle(new Point(10 * 16, 7 * 16),
                    new Size(16, 16))
                : new Rectangle(new Point(12 * 16, 7 * 16),
                    new Size(16, 16));
            if (fish.CurrentFrame < fish.IdleFrames - 1)
                fish.CurrentFrame++;
            else
                fish.CurrentFrame = 0;
            return res;
        }

        public void DrawGame(Graphics graphics, Level currentLevel, Player player, int width, int height, float scale) {
            this.scale = scale;
            DrawLevel(graphics, currentLevel, player);
            DrawInterface(graphics, currentLevel, player, width, height);
            DrawAchievements(graphics, player, width, height);
            DrawKeys(graphics, player, width, height);
        }
        private void DrawLevel(Graphics graphics, Level currentLevel, Player player) {
            _defferedFx = new List<Action>();
            for (var i = 0; i < currentLevel.Width; i++) {
                for (var j = 0; j < currentLevel.Height; j++) {
                    var destRect =
                        new Rectangle(
                            new Point((int) (j * GameEntity.FormsSize * scale),
                                (int) (i * GameEntity.FormsSize * scale)),
                            new Size((int) (GameEntity.FormsSize * scale), (int) (GameEntity.FormsSize * scale)));
                    Rectangle srcRect;

                    void DrawFloorTile() {
                        srcRect = new Rectangle(new Point((2 * 16), 5 * 16), new Size(16, 16));
                        graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                    switch (currentLevel[i, j].EntityEnumType) {
                        case GameEntitiesEnum.Player:
                            DrawFloorTile();
                            DrawPlayerAnimation(player, graphics, i, j);
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
                            srcRect = GetDiamondAnimation((Diamond) currentLevel[i, j]);
                            graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;

                        case GameEntitiesEnum.Wall:
                            srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;

                        case GameEntitiesEnum.SmartSkeleton:
                            DrawFloorTile();
                            var smartSkeleton = currentLevel[i, j] as SmartSkeleton;
                            srcRect = GetSmartSkeletonAnimation(smartSkeleton);
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            DrawEnemyHp(graphics, j, i, smartSkeleton);
                            DrawEnemyShield(graphics, smartSkeleton, destRect);
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
                            var enemyDigger = currentLevel[i, j] as EnemyDigger;
                            srcRect = GetDiggerAnimation(enemyDigger);
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            DrawEnemyHp(graphics, j, i, enemyDigger);
                            break;

                        case GameEntitiesEnum.EnemyWalker:
                            DrawFloorTile();
                            var enemyWalker = currentLevel[i, j] as EnemyWalker;
                            srcRect = GetEnemyWalkerAnimation(enemyWalker);
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            DrawEnemyHp(graphics, j, i, enemyWalker);
                            break;

                        case GameEntitiesEnum.SmartPeaceful:
                            DrawFloorTile();
                            var smartPeaceful = currentLevel[i, j] as SmartPeaceful;
                            srcRect = GetSmartPeacefulAnimation(smartPeaceful);
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            DrawEnemyHp(graphics, j, i, smartPeaceful);
                            DrawEnemyShield(graphics, smartPeaceful, destRect);
                            break;

                        case GameEntitiesEnum.SmartDevil:
                            DrawFloorTile();
                            var smartDevil = currentLevel[i, j] as SmartDevil;
                            srcRect = GetSmartDevilAnimation(smartDevil);
                            graphics.DrawImage(MainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            DrawEnemyHp(graphics, j, i, smartDevil);
                            DrawEnemyShield(graphics, smartDevil, destRect);
                            break;

                        case GameEntitiesEnum.Candy:
                            DrawFloorTile();
                            srcRect = new Rectangle(new Point(12 * 16, 4 * 16), new Size(16, 16));
                            graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;

                        case GameEntitiesEnum.GoldenFish:
                            DrawFloorTile();
                            srcRect = GetFishAnimation((GoldenFish) currentLevel[i, j]);
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
                }
                foreach (var a in _defferedFx) a.Invoke();
            }
        }

        private int _achievementCounter = -100;
        private string _achievementTitle = "";
        private string _achievementSecondary = "";
        private Point _achievementIcon;

        private void DrawAchievements(Graphics graphics, Player player, int width, int height) {
            var achievementsQueue = player.AchievementsController.AchievementsQueue;
            if (_achievementCounter > -100) {
                _achievementCounter -= 3;
            }
            else {
                if (achievementsQueue.Count == 0) return;
                var achievement = achievementsQueue.Dequeue();
                player.AchievementsController.Achieved.Add(achievement);
                switch (achievement) {
                    case AchievementsEnum.Armored:
                        _achievementTitle = "Armored";
                        _achievementSecondary = "Have 5 armor";
                        _achievementIcon = new Point(9 * 16, 15 * 16);
                        break;

                    case AchievementsEnum.Bomberman:
                        _achievementTitle = "Bomberman";
                        _achievementSecondary = "Use bomb";
                        _achievementIcon = new Point(12 * 16, 11 * 16);
                        break;

                    case AchievementsEnum.Butcher:
                        _achievementTitle = "Butcher of Blaviken";
                        _achievementSecondary = "Kill 3 enemies";
                        _achievementIcon = new Point(11 * 16, 18 * 16);
                        break;

                    case AchievementsEnum.CandyLauncher:
                        _achievementTitle = "Candy launcher";
                        _achievementSecondary = "Throw candy";
                        _achievementIcon = new Point(12 * 16, 4 * 16);
                        break;

                    case AchievementsEnum.ItsShiny:
                        _achievementTitle = "It`s shiny";
                        _achievementSecondary = "Collect diamond";
                        _achievementIcon = new Point(1 * 16, 7 * 16);
                        break;

                    case AchievementsEnum.Rage:
                        _achievementTitle = "Rage";
                        _achievementSecondary = "Have 100+ Adrenaline";
                        _achievementIcon = new Point(13 * 16, 9 * 16);
                        break;
                }
                _achievementCounter = 100;
            }
            var counterInfluence = _achievementCounter > 0 ? _achievementCounter : 0;
            var baseYposition = height - 120 + counterInfluence;
            var baseXposition = width / 14 - 50;
            graphics.FillRectangle(RedBrushHalfTransparent,
                new Rectangle(baseXposition, baseYposition, 300, 60)
            );
            var destRect =
                new Rectangle(new Point(baseXposition + 5, baseYposition + 5),
                    new Size(50, 50));
            var srcRect = new Rectangle(_achievementIcon, new Size(15, 15));
            graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
            graphics.DrawString(_achievementTitle, MenuFont, WhiteBrush, baseXposition + 5 + 55, baseYposition + 5);
            graphics.DrawString(_achievementSecondary, MenuFont, WhiteBrush, baseXposition + 5 + 55,
                baseYposition + 25);
        }

        private void DrawEnemyShield(Graphics graphics, SmartEnemy enemy, Rectangle destRect) {
            if (enemy != null && enemy.IsShieldActive) graphics.FillEllipse(ShieldBrush, destRect);
        }

        private void DrawEnemyHp(Graphics graphics, int j, int i, Enemy enemyWalker) {
            var enemyWalkerHp = enemyWalker.Hp;
            if (enemyWalkerHp < enemyWalker.MaxHp) {
                var hpRectangle =
                    new Rectangle(
                        new Point((int) (j * GameEntity.FormsSize * scale),
                            (int) (i * GameEntity.FormsSize * scale - 4)),
                        new Size((int) (GameEntity.FormsSize * scale * enemyWalkerHp / enemyWalker.MaxHp), 4));
                graphics.FillRectangle(RedBrushHalfTransparent, hpRectangle);
            }
        }
        private void DrawInterface(Graphics graphics, Level currentLevel, Player player, int width, int height) {
            DrawPlayerHp(graphics, player);
            DrawPlayerArmor(graphics, player);
            DrawPlayerEnergy(graphics, player);
            DrawPlayerSword(graphics, player);
            DrawPlayerTnt(graphics, player);
            DrawPlayerConverters(graphics, player);
            DrawDiamondsLeftToWin(graphics, currentLevel, player, width);

            var aim = currentLevel.GameMode switch {
                GameModesEnum.CollectDiamonds => "Collect diamonds",
                GameModesEnum.KillEnemies => "Kill enemies",
                GameModesEnum.HuntGoldenFish => "Hunt fish",
                _ => ""
            };

            graphics.DrawString(aim, MenuFont, GuiBrush, width - 500, 8);
            graphics.DrawString($"Level {currentLevel.LevelName}", BoldFont, GuiBrush, width - 300, 8);
            //graphics.DrawString($"{player.Name}", BoldFont, GuiBrush, width - 300, 30);
            graphics.DrawString($"Score x{player.ScoreMultiplier.ToString()}", BoldFont, GuiBrush, width - 170, 8);
            graphics.DrawString(player.Score.ToString(), BoldFont, GuiBrush, width - 170, 30);
        }
        private void DrawDiamondsLeftToWin(Graphics graphics, Level currentLevel, Player player, int width) {
            var srcRect = player.GameMode switch {
                GameModesEnum.CollectDiamonds => new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16)),
                GameModesEnum.KillEnemies => new Rectangle(new Point(23 * 16 + 1 * 16, 11 * 16), new Size(16, 16)),
                GameModesEnum.HuntGoldenFish => new Rectangle(new Point(10 * 16, 7 * 16), new Size(16, 16)),
                _ => new Rectangle()
            };

            var upper = player.GameMode switch {
                GameModesEnum.CollectDiamonds => currentLevel.DiamondsQuantityToWin - player.CollectedDiamonds,
                GameModesEnum.KillEnemies => currentLevel.KillEnemiesToWin - player.KilledEnemies+5,
                GameModesEnum.HuntGoldenFish => 1,
                _ => 0
            };
            
            
            for (var i = 0; i < upper; i++) {
                var destRect =
                    new Rectangle(new Point(8 * i + width - 500, 32),
                        new Size(16, 16));
                graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }
        private void DrawPlayerConverters(Graphics graphics, Player player) {
            for (var i = 0; i < player.Inventory.StoneInDiamondsConverterQuantity; i++) {
                var destRect =
                    new Rectangle(new Point(16 * i + 288, 24),
                        new Size(32, 32));
                var srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }
        private void DrawPlayerTnt(Graphics graphics, Player player) {
            for (var i = 0; i < player.Inventory.TntQuantity; i++) {
                var destRect =
                    new Rectangle(new Point(16 * i + 288, 16),
                        new Size(32, 32));
                var srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(15, 15));
                graphics.DrawImage(SecondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }
        private void DrawPlayerSword(Graphics graphics, Player player) {
            var destRect =
                new Rectangle(new Point(256, 16),
                    new Size(32, 32));
            Rectangle srcRect;

            srcRect = player.Inventory.SwordLevel > 0
                    ? new Rectangle(new Point((1 + player.Inventory.SwordLevel) * 16, 18 * 16), new Size(15, 15))
                    : new Rectangle(new Point((1 + player.Inventory.SwordLevel) * 16, 18 * 16), new Size(0, 0))
                ;

            graphics.DrawImage(Icons, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawPlayerEnergy(Graphics graphics, Player player) {
            for (var i = 0; i < player.Energy; i++) {
                var destRect =
                    new Rectangle(new Point(16 * i, 64),
                        new Size(32, 32));
                var srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                graphics.DrawImage(player.Adrenaline > 0 ? Adrenaline : Energy, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }
        private void DrawPlayerArmor(Graphics graphics, Player player) {
            for (var i = 0; i < player.Inventory.ArmorLevel; i++) {
                var destRect =
                    new Rectangle(new Point(16 * i, 24),
                        new Size(32, 32));
                var srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                graphics.DrawImage(Shield, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }
        private void DrawPlayerHp(Graphics graphics, Player player) {
            for (var i = 0; i < player.MaxHp; i++) {
                var destRect =
                    new Rectangle(new Point(24 * i, 16),
                        new Size(32, 32));
                if (player.Hp >= i) {
                    var srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                    graphics.DrawImage(HpFull, destRect, srcRect, GraphicsUnit.Pixel);
                }
                else {
                    var srcRect = new Rectangle(new Point(0, 0), new Size(32, 32));
                    graphics.DrawImage(HpEmpty, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }

        private void DrawKeys(Graphics graphics, Player player, int width, int height) {
            var key = player.Keyboard;
            DrawKeyW(graphics, key, width, height);
            DrawKeyA(graphics, key, width, height);
            DrawKeyS(graphics, key, width, height);
            DrawKeyD(graphics, key, width, height);
            DrawKeySpace(graphics, key, width, height);
            DrawKeyT(graphics, key, width, height);
            DrawKeyQ(graphics, key, width, height);
            DrawKeyE(graphics, key, width, height);
            DrawKeyR(graphics, key, width, height);
        }
        private void DrawKeyR(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 136, height - 157),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(6 * 16, 2 * 16 + (int) key.R * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyE(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 168, height - 157),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(5 * 16, 2 * 16 + (int) key.E * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyQ(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 232, height - 157),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(3 * 16, 2 * 16 + (int) key.Q * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyT(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 104, height - 157),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(7 * 16, 2 * 16 + (int) key.T * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeySpace(Graphics graphics, Keyboard key, int width, int height) {
            Rectangle destRect;
            Rectangle srcRect;
            if (key.Space == 0) {
                destRect =
                    new Rectangle(new Point(width - 232, height - 93),
                        new Size(160, 32));
                srcRect = new Rectangle(new Point(5 * 16, 5 * 16 + (int) key.Space * 16), new Size(80, 16));
            }
            else {
                destRect =
                    new Rectangle(new Point(width - 236, height - 93),
                        new Size(144, 32));
                srcRect = new Rectangle(new Point(6 * 16, 5 * 16 + (int) key.Space * 16), new Size(64, 16));
            }
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyD(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 168, height - 125),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(5 * 16, 3 * 16 + (int) key.D * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyS(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 200, height - 125),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(4 * 16, 3 * 16 + (int) key.S * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyA(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 232, height - 125),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(3 * 16, 3 * 16 + (int) key.A * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
        private void DrawKeyW(Graphics graphics, Keyboard key, int width, int height) {
            var destRect = new Rectangle(new Point(width - 200, height - 157),
                new Size(32, 32));
            var srcRect = new Rectangle(new Point(4 * 16, 2 * 16 + (int) key.W * 16), new Size(16, 16));
            graphics.DrawImage(Keyboard, destRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}