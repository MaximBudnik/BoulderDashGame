using System;
using ClassLibrary;

namespace BoulderDashGame {
    class Program {
        static void Main(string[] args) {
            //all app is wrapped by try/catch block 
            try {

                GameEngine.Start();

            }
            catch {
                Console.WriteLine("An error occured. Restart app and contact developer.");
                Console.ReadKey();
            }
        }
    }
}