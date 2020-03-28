using System;

namespace ClassLibrary {
    public class Menu : UserInterface {
        //Fields

        // private System.ConsoleColor ConsoleBackgroundColor = ConsoleColor.Red;
        private string[] menuActions = {"Continue", "New game", "Help","Exit",};
        private int currentMenuAction = 0;
        private string Title1 = "█▀▄▀█ █ █▄░█ █▀▀ █▀█   ▄▀█ █▀▄ █░█ █▀▀ █▄░█ ▀█▀ █░█ █▀█ █▀▀";
        private string Title2 = "█░▀░█ █ █░▀█ ██▄ █▀▄   █▀█ █▄▀ ▀▄▀ ██▄ █░▀█ ░█░ █▄█ █▀▄ ██▄▀";

        //private methods
        
        
        
        //public methods
        public void СreateMainMenu() {
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
        }

        public void ChangecurrentMenuAction(int i) {
            currentMenuAction += i;
            СreateMainMenu();
        }
    }
}