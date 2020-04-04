// I use this class only to display everything in console. I`l replace it in Windows Forms version.


using System;
using System.Text;

//dont know if i can delete it later

namespace ClassLibrary.ConsoleInterface {
    public class UserInterface {
        //Constructor
        public UserInterface() {
            SetBackground(ConsoleBackgroundColor);
            Console.Title = "Miner Adventure";
            Console.ForegroundColor = primaryTextColor;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth, consoleHeight); //interesting effect: without it, console is constantly twitches
        }

        //Fields
        private protected int consoleWidth = 128;
        private protected int consoleHeight = 39;

        private protected System.ConsoleColor ConsoleBackgroundColor = ConsoleColor.Black;
        private protected System.ConsoleColor primaryTextColor = ConsoleColor.White;
        private protected System.ConsoleColor secondTextColor = ConsoleColor.DarkGray;

        //Private methods
        private protected void SetBackground(System.ConsoleColor color) {
            Console.BackgroundColor = color;
            Console.Clear();
        }

        private protected void LogCentered(string data) {
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
        }

        private protected void LogCentered(int data) {
            string stringoFiedData = data.ToString();
            Console.SetCursorPosition((Console.WindowWidth - stringoFiedData.Length) / 2, Console.CursorTop);
            Console.WriteLine(stringoFiedData);
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

        public void Draw() { }
    }
}