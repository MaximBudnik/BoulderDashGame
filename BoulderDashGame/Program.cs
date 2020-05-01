using System;
using ClassLibrary;


namespace BoulderDashGame {
    class Program {
        static void Main(string[] args) {
            //all app is wrapped by try/catch block 
            try {

                GameEngine.Start();

            }
            catch(Exception e) {
                Console.Clear();
                Console.WriteLine("An error occured. Restart app and contact developer.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.Data);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }
    }
}