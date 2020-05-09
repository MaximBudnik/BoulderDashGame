using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.Entities;

namespace BoulderDashForms {
    public partial class Form1 : Form {
        private Image mainSprites;
        private Image secondarySprites;
        private Image Tileset;
        private Image icons;

        private GameEngine gameEngine;
        public Form1() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");

            mainSprites = new Bitmap(mainsSpritesPath);
            secondarySprites = new Bitmap(secondarySpritesPath);
            Tileset = new Bitmap(TilesetPath);
            icons = new Bitmap(iconsPath);

            KeyDown+= new KeyEventHandler(KeyPressProcessor);
            

            try {
                gameEngine = new GameEngine(reDraw);
                Task engineStart = new Task(() => { gameEngine.Start(); });
                engineStart.Start();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void KeyPressProcessor(object sender, KeyEventArgs e) {
            var player = gameEngine.GameLogic.Player;

            switch (e.KeyCode) {
                case Keys.W:
                    player.Move("vertical", -1);
                    break;
                case Keys.S:
                    player.Move("vertical", 1);
                    break;
                case Keys.A:
                    player.Move("horizontal", -1);
                    break;
                case Keys.D:
                    player.Move("horizontal", 1);
                    break;
                case Keys.T:
                    player.Teleport();
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    break;
                case Keys.Q:
                    player.ConvertNearStonesInDiamonds();
                    break;
                case Keys.E:
                    player.UseTnt();
                    break;
                case Keys.R:
                    player.Attack();
                    break;
                case Keys.Escape:
                    gameEngine.ChangeGameStatus(0);
                    break;
            }
        }
        
        
        private void onPaint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;
            var currentLevel = gameEngine.GameLogic.CurrentLevel;
            var player = gameEngine.GameLogic.Player;
            for (int i = 0; i < currentLevel.Width; i++) {
                for (int j = 0; j < currentLevel.Height; j++) {
                    Rectangle destRect =
                        new Rectangle(new Point(j* GameEntity.formsSize * 2, i * GameEntity.formsSize * 2),
                            new Size(GameEntity.formsSize * 2, GameEntity.formsSize * 2));
                    Rectangle srcRect;

                    void drawFloorTile() {
                        //TODO: make versatile tile floor with random
                        srcRect = new Rectangle(new Point(2 * 16, 5 * 16), new Size(16, 16));
                        graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                    switch (currentLevel[i, j].EntityType) {
                        case 0:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(9 * 16, 3 * 16), new Size(16, 16));
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
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(7 * 16, 4 * 16), new Size(16, 16));
                            graphics.DrawImage(Tileset, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 4:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(1 * 16, 7 * 16), new Size(16, 16));
                            graphics.DrawImage(icons, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 5:
                            srcRect = new Rectangle(new Point(1 * 16, 1 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 6:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(23 * 16, 5 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 7:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 8:
                            drawFloorTile();
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
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(14 * 16, 13 * 16), new Size(16, 16));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 20:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(20 * 16, 2 * 12), new Size(16, 22));
                            graphics.DrawImage(mainSprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 21:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(12 * 16, 12 * 16), new Size(16, 16));
                            graphics.DrawImage(secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 22:
                            drawFloorTile();
                            srcRect = new Rectangle(new Point(15 * 16, 3 * 16), new Size(16, 16));
                            graphics.DrawImage(secondarySprites, destRect, srcRect, GraphicsUnit.Pixel);
                            break;
                        case 23:
                            drawFloorTile();
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

        private void reDraw() {
            Invalidate();
        }
    }
}