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
        private readonly ResultScreenDrawer _resultScreenDrawer;
        private readonly ResultsInputProcessor _resultsInputProcessor = new ResultsInputProcessor();
        private readonly LevelRedactorDrawer _levelRedactorDrawer;
        private readonly LevelRedactorInputProcessor _levelRedactorInputProcessor = new LevelRedactorInputProcessor();
        private GameEngine _gameEngine;
        public Form1() {
            InitializeComponent();
            try {
                Cursor = new Cursor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, @"Sprites\cursor.ico"));
                _menuDrawer = new MenuDrawer();
                _gameDrawer = new GameDrawer();
                _resultScreenDrawer = new ResultScreenDrawer();
                _levelRedactorDrawer = new LevelRedactorDrawer();
                KeyDown += KeyDownProcessor;
                KeyUp += KeyUpProcessor;
                MouseWheel += MouseWheelProcessor;
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
            _gameEngine = new GameEngine(ReDraw);
            var engineStart = new Task(() => { _gameEngine.Start(); });
            engineStart.Start();
        }
        private void KeyDownProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == GameStatusEnum.Game)
                _gameInputProcessor.ProcessKeyDown(e.KeyCode, () => _gameEngine.GameLogic.Player,
                    _gameEngine.ChangeVolume);
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
                    s => {
                        if (_gameEngine.CurrentMenuAction == 1) {
                            if (s == Keys.Back.ToString()) {
                                _gameEngine.NewGameSave.Name =
                                    _gameEngine.NewGameSave.Name.Substring(0, _gameEngine.NewGameSave.Name.Length - 1);
                                return;
                            }
                            _gameEngine.NewGameSave.Name += s;
                        }
                        else if (_gameEngine.CurrentMenuAction == 2) {
                            if (s == Keys.Back.ToString()) {
                                _gameEngine.LevelRedactor.NewCustomLevel.Name =
                                    _gameEngine.LevelRedactor.NewCustomLevel.Name.Substring(0,
                                        _gameEngine.LevelRedactor.NewCustomLevel.Name.Length - 1);
                                return;
                            }
                            _gameEngine.LevelRedactor.NewCustomLevel.Name += s;
                        }
                    },
                    _gameEngine.ChangeVolume,
                    _gameEngine.PlaySound
                );
            else if (_gameEngine.GameStatus == GameStatusEnum.WinScreen ||
                     _gameEngine.GameStatus == GameStatusEnum.LoseScreen)
                _resultsInputProcessor.ProcessKeyDown(
                    e.KeyCode, _gameEngine.ChangeGameStatus, _gameEngine.ChangeVolume, _gameEngine.PlaySound,
                    _gameEngine.PerformSubAction);
            else if (_gameEngine.GameStatus == GameStatusEnum.Redactor) {
                _levelRedactorInputProcessor.ProcessKeyDown(e.KeyCode, _gameEngine.ChangeGameStatus,
                    _gameEngine.LevelRedactor.ChangeAnchorPosition, _gameEngine.ChangeVolume, _gameEngine.PlaySound,
                    _gameEngine.LevelRedactor.PlaceBlock, _gameEngine.LevelRedactor.ChangeTool,
                    _gameEngine.SaveCustomLevel);
            }
        }
        private void KeyUpProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == GameStatusEnum.Game)
                _gameInputProcessor.ProcessKeyUp(e.KeyCode, () => _gameEngine.GameLogic.Player,
                    _gameEngine.ChangeGameStatus);
        }

        private void MouseWheelProcessor(object sender, MouseEventArgs e) {
            if (e.Delta > 0) {
                _gameEngine.GuiScale += 0.1f;
            }
            else {
                _gameEngine.GuiScale -= 0.1f;
            }
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            var graphics = e.Graphics;
            if (_gameEngine == null) return;
            switch (_gameEngine.GameStatus) {
                case GameStatusEnum.Menu:
                    _menuDrawer.DrawMenu(graphics, _gameEngine, Width, Height);
                    break;
                case GameStatusEnum.Game: {
                    var currentLevel = _gameEngine.GameLogic.CurrentLevel;
                    var player = _gameEngine.GameLogic.Player;
                    _gameDrawer.DrawGame(graphics, currentLevel, player, Width, Height, _gameEngine.GuiScale);
                    break;
                }
                case GameStatusEnum.WinScreen:
                case GameStatusEnum.LoseScreen:
                    _resultScreenDrawer.DrawResults(graphics, _gameEngine.GameStatus, _gameEngine.GetPlayerName(),
                        _gameEngine.GetScores(), _gameEngine.GetAllPlayerScores(), Width, Height);
                    break;
                case GameStatusEnum.Redactor:
                    _levelRedactorDrawer.DrawRedactor(graphics, _gameEngine, Width, Height);
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