using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoulderDashForms.FormsDrawers;
using BoulderDashForms.InputProcessors;
using ClassLibrary;

namespace BoulderDashForms {
    public partial class Form1 : Form {
        private readonly GameDrawer _gameDrawer;
        private readonly GameInputProcessor _gameInputProcessor = new GameInputProcessor();
        private readonly MenuDrawer _menuDrawer;
        private readonly MenuInputProcessor _menuInputProcessor = new MenuInputProcessor();
        private GameEngine _gameEngine;
        public Form1() {
            InitializeComponent();
            try {
                Cursor = new Cursor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, @"Sprites\cursor.ico"));
                _menuDrawer = new MenuDrawer();
                _gameDrawer = new GameDrawer();
                KeyDown += KeyDownProcessor;
                KeyUp += KeyUpProcessor;
                InitEngine();
            }
            catch (Exception e) {
                Console.WriteLine(e.Data);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void InitEngine() {
            try {
                _gameEngine = new GameEngine(ReDraw);
                var engineStart = new Task(() => { _gameEngine.Start(); });
                engineStart.Start();
            }
            catch (Exception e) {
                Console.WriteLine(e.Data);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }
        private void KeyDownProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == 1)
                _gameInputProcessor.ProcessKeyDown(e.KeyCode, () => _gameEngine.GameLogic.Player,
                    _gameEngine.ChangeGameStatus);
            else if (_gameEngine.GameStatus == 0)
                _menuInputProcessor.ProcessKeyDown(e.KeyCode,
                    _gameEngine.ChangeCurrentMenuAction,
                    _gameEngine.PerformCurrentMenuAction,
                    _menuDrawer.NullRightBlockWidth,
                    _gameEngine.ChangeIsActionActive,
                    _gameEngine.IsActionActive,
                    _gameEngine.ChangeCurrentSubAction,
                    _gameEngine.PerformSubAction,
                    _gameEngine.IsNameEntered,
                    _gameEngine.ChangeIsNameEntered,
                    s => { _gameEngine.NewGameSave.Name += s; },
                    _gameEngine.ChangeVolume
                );
        }
        private void KeyUpProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == 1)
                _gameInputProcessor.ProcessKeyUp(e.KeyCode, () => _gameEngine.GameLogic.Player);
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            var graphics = e.Graphics;
            if (_gameEngine == null) return;
            switch (_gameEngine.GameStatus) {
                case 0:
                    _menuDrawer.DrawMenu(graphics, _gameEngine);
                    break;
                case 1:
                    var currentLevel = _gameEngine.GameLogic.CurrentLevel;
                    var player = _gameEngine.GameLogic.Player;
                    _gameDrawer.DrawGame(graphics, currentLevel, player);
                    break;
                default:
                    throw new Exception($"Unhandled game status, can be 0-3, is {_gameEngine.GameStatus}");
            }
        }

        private void ReDraw() {
            Invalidate();
        }
    }
}