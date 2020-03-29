// I use this class only to display everything in console. I`l replace it in Windows Forms version.


using System;
using System.ComponentModel;
using System.Text;

//dont know if i can delete it later

namespace ClassLibrary {
    public class UserInterface {
        //Constructor
        public UserInterface() {
            SetBackground(ConsoleBackgroundColor);
            Console.Title = "Miner Advenure";
            Console.ForegroundColor = primaryTextColor;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth, consoleHeight);
        }

        //Fields
        private protected int consoleWidth = 130;
        private protected int consoleHeight = 40;

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

        public void Draw() { }
    }
}