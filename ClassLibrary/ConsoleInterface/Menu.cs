using System;

namespace ClassLibrary {
    public class Menu : UserInterface {
        
        //Fields

        // private System.ConsoleColor ConsoleBackgroundColor = ConsoleColor.Red;
        private new System.ConsoleColor secondTextColor = ConsoleColor.Red;
        private string[] menuActions = {"Continue", "New game","Settings", "Help","Exit",};
        // private int currentMenuAction = 0;
        private string Title1 = "█▀▄▀█ █ █▄░█ █▀▀ █▀█   ▄▀█ █▀▄ █░█ █▀▀ █▄░█ ▀█▀ █░█ █▀█ █▀▀";
        private string Title2 = "█░▀░█ █ █░▀█ ██▄ █▀▄   █▀█ █▄▀ ▀▄▀ ██▄ █░▀█ ░█░ █▄█ █▀▄ ██▄▀";

        //private methods
        
        
        
        //public methods
        public void CreateMainMenu(int currentMenuAction) {
            Console.Clear();
            // SetBackground(ConsoleBackgroundColor);
            Console.WriteLine('\n');
            LogCentered(Title1);
            LogCentered(Title2);
            Console.WriteLine('\n');

            for(int i = 0; i < menuActions.Length; i++){
                if (i == currentMenuAction) {
                    Console.ForegroundColor = secondTextColor; 
                    LogCentered(menuActions[i]);
                    Console.ForegroundColor = primaryTextColor; 
                }
                else {
                    LogCentered(menuActions[i]);
                }
            }

            for (int i = 0; i < 13; i++) {
                Console.WriteLine('\n'); //TODO: do smth with this garbage
            }
            
            Console.WriteLine("To choose actions press W, S and Enter");
            Console.WriteLine("Version: 0.01 (Only Continue is working) Esc is also working, but it is not forever");
        }
    }
}