using System;
using System.Collections.Generic;

namespace ClassLibrary {
    public class Randomizer {
        private static readonly Random Rand = new Random();

        public static int GetRandomFromList(List<int> list) {
            return list[Rand.Next(list.Count)];
        }
        
        public static string GetRandomFromList(List<string> list) {
            return list[Rand.Next(list.Count)];
        }

        public static int Random() {
            return Rand.Next();
        }

        public static int Random(int upper) {
            return Rand.Next(upper);
        }

        public static int Random(int lower, int upper) {
            return Rand.Next(lower, upper);
        }
    }
}