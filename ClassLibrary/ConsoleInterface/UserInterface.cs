// I use this class only to display everything in console. I`l replace it in Windows Forms version.


using System;
using System.Text;

//dont know if i can delete it later

namespace ClassLibrary.ConsoleInterface {
    public abstract class UserInterface {
        private readonly ConsoleColor ConsoleBackgroundColor = ConsoleColor.Black;
        private readonly int consoleHeight = 45;

        //Fields
        private readonly int consoleWidth = 131;
        private protected ConsoleColor primaryTextColor = ConsoleColor.White;

        private protected ConsoleColor SecondTextColor = ConsoleColor.DarkGray;
        //Constructor
        protected UserInterface() {
            SetBackground(ConsoleBackgroundColor);
            Console.Title = "Miner Adventure";
            Console.ForegroundColor = primaryTextColor;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth,
                consoleHeight); //interesting effect: without it, console is constantly twitches
        }

        //Private methods
        private void SetBackground(ConsoleColor color) {
            Console.BackgroundColor = color;
            Console.Clear();
        }

        private protected static void LogCentered(string data) {
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
        }

        private protected void LogCentered(int data) {
            var stringFieldData = data.ToString();
            Console.SetCursorPosition((Console.WindowWidth - stringFieldData.Length) / 2, Console.CursorTop);
            Console.WriteLine(stringFieldData);
        }

        protected void WriteColorBack(string str) {
            Console.Write(str);
        }

        protected void WriteColorBack(string str, ConsoleColor foreground) {
            Console.ForegroundColor = foreground;
            Console.Write(str);
            Console.ForegroundColor = primaryTextColor;
        }

        protected void WriteForeground(string str, ConsoleColor foreground, ConsoleColor previous) {
            Console.ForegroundColor = foreground;
            Console.Write(str);
            Console.ForegroundColor = previous;
        }

        protected void WriteColorBack(string str, ConsoleColor foreground, ConsoleColor background) {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.Write(str);
            Console.ForegroundColor = primaryTextColor;
            Console.BackgroundColor = ConsoleBackgroundColor;
        }

        protected void DrawLine() {
            for (var i = 0; i < consoleWidth; i++) Console.Write("─");
        }

        protected void SkipLine() {
            Console.WriteLine("\n");
        }

        protected void SkipLine(int i) {
            for (var j = 0; j < i; j++) Console.WriteLine("\n");
        }

        protected void ChangeForegroundColor(ConsoleColor color) {
            Console.ForegroundColor = color;
        }

        protected void ChangeBackgroundColor(ConsoleColor color) {
            Console.BackgroundColor = color;
        }

        public void Draw() { }
    }
}