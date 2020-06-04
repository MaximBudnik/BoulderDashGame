using System;
using System.Collections.Generic;
using ClassLibrary.Entities.Player;
using ClassLibrary.Matrix;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Collectable.ItemsTiles;

namespace ClassLibrary.ConsoleInterface {
    public class GameInterface : UserInterface {
        private readonly int _bottomHeight = 3;

        // private int interfaceMultiplier = 2; //should be retrieved from settings
        private readonly ConsoleColor _secondTextColor = ConsoleColor.DarkCyan;

        private readonly Dictionary<int, string> _sprites = new Dictionary<int, string> {
            {0, "☺"},
            {1, " "},
            {2, "·"},
            {3, "○"},
            {4, "×"},
            {5, "│"},
            {6, "*"},
            {7, "?"},
            {8, "·"},
            {9, "/"},
            {10, "0"},
            {11, " "},
            {12, "!"},
            {13,"x"},
            {20, "S"},
            {21, "C"},
            {22, "T"},
            {23, "A"},
            {203, "•"}
        };

        private readonly int _topHeight = 2;
        private Level _currLevel;
        public void NewDraw(Func<Level> getLevel) {
            _currLevel = getLevel();
            var origRow = _topHeight;
            for (var i = 0; i < _currLevel.Width; i++)
            for (var j = 0; j < _currLevel.Height; j++) {
                var tmp = _currLevel[i, j].EntityEnumType;
                Console.SetCursorPosition(2 * j, 2 * i + origRow);
                Console.Write(" ");
                Console.Write(" ");
                Console.SetCursorPosition(2 * j, 2 * i + origRow);
                DrawSpriteDefault(tmp);
                DrawSpriteDefault(tmp);

                Console.SetCursorPosition(2 * j, 2 * i + origRow + 1);
                Console.Write(" ");
                Console.Write(" ");
                Console.SetCursorPosition(2 * j, 2 * i + origRow + 1);
                DrawSpriteDefault(tmp);
                DrawSpriteDefault(tmp);
            }
        }

        private void DrawSpriteDefault(GameEntitiesEnum item) {
            switch (item) {
                case GameEntitiesEnum.Player:
                    WriteColorBack(_sprites[0], ConsoleColor.Magenta);
                    break;
                case GameEntitiesEnum.EmptySpace:
                    WriteColorBack(_sprites[1]);
                    break;
                case GameEntitiesEnum.Sand:
                    WriteColorBack(_sprites[2], ConsoleColor.DarkYellow, ConsoleColor.Yellow);
                    break;
                case GameEntitiesEnum.Rock:
                    WriteColorBack(_sprites[3], ConsoleColor.Gray, ConsoleColor.DarkGray);
                    break;
                case GameEntitiesEnum.Diamond:
                    WriteColorBack(_sprites[4], ConsoleColor.Cyan);
                    break;
                case GameEntitiesEnum.Wall:
                    WriteColorBack(_sprites[5], primaryTextColor, ConsoleColor.Gray);
                    break;
                case GameEntitiesEnum.EnemyWalker:
                    WriteColorBack(_sprites[6], ConsoleColor.Black, ConsoleColor.DarkRed);
                    break;
                case GameEntitiesEnum.LuckyBox:
                    WriteColorBack(_sprites[7], primaryTextColor, ConsoleColor.DarkYellow);
                    break;
                case GameEntitiesEnum.SandTranslucent:
                    WriteColorBack(_sprites[8], ConsoleColor.DarkGray);
                    break;
                case GameEntitiesEnum.Converter:
                    WriteColorBack(_sprites[10], ConsoleColor.Cyan, ConsoleColor.Magenta);
                    break;
                case GameEntitiesEnum.Acid:
                    WriteColorBack(_sprites[11], ConsoleColor.Green, ConsoleColor.Green);
                    break;
                case GameEntitiesEnum.Barrel:
                    WriteColorBack(_sprites[12], ConsoleColor.Green, ConsoleColor.DarkYellow);
                    break;
                case GameEntitiesEnum.EnemyDigger:
                    WriteColorBack(_sprites[13], ConsoleColor.Red, ConsoleColor.White);
                    break;
                case GameEntitiesEnum.SwordTile:
                    WriteColorBack(_sprites[20], ConsoleColor.Magenta);
                    break;
                case GameEntitiesEnum.ConverterTile:
                    WriteColorBack(_sprites[21], ConsoleColor.Magenta);
                    break;
                case GameEntitiesEnum.DynamiteTile:
                    WriteColorBack(_sprites[22], ConsoleColor.Magenta);
                    break;
                case GameEntitiesEnum.ArmorTile:
                    WriteColorBack(_sprites[23], ConsoleColor.Magenta);
                    break;
            }
        }

        public void DrawPlayerInterface(int diamondsAll, int diamondsCollected, int maxEnergy, int currentEnergy,
            int hpMax, int currentHp, string name, Inventory playerInventory) {
            void WritePart(string symbol, int fill, int all, ConsoleColor primary) {
                ChangeForegroundColor(primary);
                for (var i = 0; i < fill; i++) Console.Write(symbol);
                ChangeForegroundColor(ConsoleColor.DarkGray);
                for (var i = 0; i < all - fill; i++) Console.Write(symbol);
                Console.Write("   ");
                ChangeForegroundColor(_secondTextColor);
            }
            ChangeForegroundColor(ConsoleColor.DarkCyan);
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - _bottomHeight;
            Console.WriteLine();
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - _bottomHeight;
            DrawLine();
            Console.Write(" Name: ");
            WriteForeground($"{name}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("HP: ");
            WritePart("♥", currentHp, hpMax, ConsoleColor.Red);
            Console.Write("Energy: ");
            WritePart("■", currentEnergy, maxEnergy, ConsoleColor.DarkYellow);
            Console.Write("Diamonds: ");
            WriteForeground($"{diamondsCollected}/{diamondsAll}   ", ConsoleColor.Cyan, _secondTextColor);
            Console.Write("Items: ");
            WriteForeground(
                $"S: {playerInventory.SwordLevel} A: {playerInventory.ArmorLevel} C: {playerInventory.StoneInDiamondsConverterQuantity}" +
                $" T: {playerInventory.TntQuantity}", ConsoleColor.Green, _secondTextColor);
            ChangeForegroundColor(primaryTextColor);
        }

        public void DrawUpperInterface(int levelName, int score, string aim) {
            Console.CursorTop = 0;
            SkipLine();
            Console.CursorTop = 0;
            ChangeForegroundColor(_secondTextColor);
            Console.Write(" Level: ");
            WriteForeground($"Level {levelName}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("Score: ");
            WriteForeground($"{score}   ", ConsoleColor.White, _secondTextColor);
            Console.Write("Aim: ");
            WriteForeground($"{aim}   ", ConsoleColor.Blue, _secondTextColor);
            Console.Write("\n");
            DrawLine();
            ChangeForegroundColor(primaryTextColor);
        }
    }
}