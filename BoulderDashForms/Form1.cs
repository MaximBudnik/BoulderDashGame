using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoulderDashForms.FormsDrawers;
using ClassLibrary;
using ClassLibrary.Entities;

namespace BoulderDashForms {
    public partial class Form1 : Form {
        private readonly FormsInputProcessor _formsInputProcessor = new FormsInputProcessor();
        private GameEngine _gameEngine;
        private readonly GameDrawer _gameDrawer;
        private readonly MenuDrawer _menuDrawer;
        public Form1() {
            InitializeComponent();
            //  Cursor = new Cursor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\cursor.ico"));
            _menuDrawer = new MenuDrawer();
            _gameDrawer = new GameDrawer();
            KeyDown += KeyDownProcessor;
            KeyUp += KeyUpProcessor;
            Init();
        }

        private void Init() {
            try {
                _gameEngine = new GameEngine(ReDraw);
                Task engineStart = new Task(() => { _gameEngine.Start(); });
                engineStart.Start();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        private void KeyDownProcessor(object sender, KeyEventArgs e) {
            _formsInputProcessor.ProcessKeyDown(e.KeyCode, () => _gameEngine.GameLogic.Player,
                _gameEngine.ChangeGameStatus);
        }
        private void KeyUpProcessor(object sender, KeyEventArgs e) {
            _formsInputProcessor.ProcessKeyUp(e.KeyCode, () => _gameEngine.GameLogic.Player);
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;

            switch (_gameEngine.GameStatus) {
                case 1:
                    var currentLevel = _gameEngine.GameLogic.CurrentLevel;
                    var player = _gameEngine.GameLogic.Player;
                    _gameDrawer.DrawGame(graphics, currentLevel, player);
                    break;
                default:
                    throw new Exception($"Unhandled game status, can be 0-3, is {_gameEngine.GameStatus}");
            }
        }

        private void ReDraw() => Invalidate();
    }
}