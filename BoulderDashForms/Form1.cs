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
using BoulderDashForms.InputProcessors;
using ClassLibrary;
using ClassLibrary.Entities;

namespace BoulderDashForms {
    public partial class Form1 : Form {
        private readonly FormsInputProcessor _formsInputProcessor = new FormsInputProcessor();
        private readonly MenuInputProcessor _menuInputProcessor = new MenuInputProcessor();
        private readonly MenuMouseProcessor _menuMouseProcessor = new MenuMouseProcessor();
        private GameEngine _gameEngine;
        private readonly GameDrawer _gameDrawer;
        private readonly MenuDrawer _menuDrawer;
        public Form1() {
            InitializeComponent();
            try {
                Cursor = new Cursor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\cursor.ico"));
                _menuDrawer = new MenuDrawer();
                _gameDrawer = new GameDrawer();
                KeyDown += KeyDownProcessor;
                KeyUp += KeyUpProcessor;
                //MouseClick += MouseClickProcessor;
                
                GC.KeepAlive(_menuDrawer);
                GC.KeepAlive(_gameDrawer);

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
                Task engineStart = new Task(() => { _gameEngine.Start(); });
                engineStart.Start();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        
        // private void MouseClickProcessor(object sender, MouseEventArgs e) {
        //     if (_gameEngine.GameStatus == 0) {
        //         _menuMouseProcessor.ProcessClick();
        //     }
        // }

        private void KeyDownProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == 1) {
                _formsInputProcessor.ProcessKeyDown(e.KeyCode, () => _gameEngine.GameLogic.Player,
                    _gameEngine.ChangeGameStatus);
            }
            else if (_gameEngine.GameStatus == 0) {
                _menuInputProcessor.ProcessKeyDown(e.KeyCode,
                    _gameEngine.ChangeCurrentMenuAction,
                    _gameEngine.PerformCurrentMenuAction,
                    _menuDrawer.NullRightBlockWidth,
                    () => { _gameEngine.IsActionActive = !_gameEngine.IsActionActive; },
                    _gameEngine.IsActionActive,
                    _gameEngine.ChangeCurrentSubAction,
                    _gameEngine.PerformSubAction,
                    _gameEngine.IsNameEntered,
                    _gameEngine.ChangeIsNameEntered,
                    s => { _gameEngine.NewGameSave.Name += s;}
                    );
            }
        }
        private void KeyUpProcessor(object sender, KeyEventArgs e) {
            if (_gameEngine.GameStatus == 1) {
                _formsInputProcessor.ProcessKeyUp(e.KeyCode, () => _gameEngine.GameLogic.Player);
            }
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;
            if(_gameEngine==null) return;
            switch (_gameEngine.GameStatus) {
                case 0:
                    _menuDrawer.DrawMenu( graphics, _gameEngine);
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

        private void ReDraw() => Invalidate();

    }
}