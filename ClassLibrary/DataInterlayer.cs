using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibrary {
    public class DataInterlayer {
        private string _scoresFileName = Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "scores.txt");
        private string _savesFileName = Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "saves.txt");

        public List<save> saves = new List<save>();
        public SortedDictionary<int, string> getBestSccores() {
            SortedDictionary<int, string> results = new SortedDictionary<int, string>();
            try {
                using (StreamReader fs = new StreamReader(_scoresFileName)) {
                    while (true) {
                        string temp = fs.ReadLine();
                        if (temp == null) break;
                        string name = "";
                        string score = "";
                        bool flag = false;
                        for (int i = 0; i < temp.Length; i++) {
                            char symb = ' ';
                            if (temp[i] == symb) {
                                flag = true;
                                continue;
                            }
                            if (flag == false) {
                                name += temp[i];
                            }
                            else if (flag) {
                                score += temp[i];
                            }
                        }
                        results[Int32.Parse(score)] = name;
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read file with best scores");
                Console.WriteLine(e.Message);
            }
            return results;
        }

        public void GetGameSaves() {
            List<save> allSaves = new List<save>();
            int id = 0;
            string name = null;
            int levelName = 0;
            int score = 0;
            try {
                using (StreamReader fs = new StreamReader(_savesFileName)) {
                    while (true) {
                        string temp = fs.ReadLine();
                        if (temp == null) {
                            allSaves.Add(new save(id, name, levelName, score));
                            break;
                        }
                        string command = "";
                        string value = "";
                        bool flag = false;
                        for (int i = 0; i < temp.Length; i++) {
                            char symb = ' ';
                            if (temp[i] == symb) {
                                flag = true;
                                continue;
                            }
                            if (flag == false) {
                                command += temp[i];
                            }
                            else if (flag) {
                                value += temp[i];
                            }
                        }
                        switch (command) {
                            case "save":
                                allSaves.Add(new save(id, name, levelName, score));
                                id = Int32.Parse(value);
                                name = null;
                                levelName = 0;
                                score = 0;
                                break;
                            case "name":
                                name = value;
                                break;
                            case "levelName":
                                levelName =Int32.Parse(value);
                                break;
                            case "score":
                                score = Int32.Parse(value);
                                break;
                        }
                    }
                }
                allSaves.RemoveAt(0);
                saves = allSaves;
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read save file");
                Console.WriteLine(e.Message);
            }
        }
        public void AddGameSave(string name) {
            using (StreamWriter writer =  File.AppendText(_savesFileName)) {
                writer.WriteLine($"save {saves.Count}");
                writer.WriteLine($"name {name}");
                writer.WriteLine($"levelName 1");
                writer.WriteLine($"score 0");
            }
        }

        public void DeleteGameSave(int id) {
            string text="";
            int counter = 0;
            try {
                using (StreamReader fs = new StreamReader(_savesFileName)) {
                    while (true) {
                        string temp = fs.ReadLine();
                        if (temp == null) {
                            break;
                        }
                        string command = "";
                        string value = "";
                        bool flag = false;
                        for (int i = 0; i < temp.Length; i++) {
                            char symb = ' ';
                            if (temp[i] == symb) {
                                flag = true;
                                continue;
                            }
                            if (flag == false) {
                                command += temp[i];
                            }
                            else if (flag) {
                                value += temp[i];
                            }
                        }
                        if (command=="save" && Int32.Parse(value)==id) {
                            counter = -4;
                        }
                        if (counter >=0) {
                            text += temp+"\n";
                        }
                        counter++;
                    }
                }
                File.WriteAllText(_savesFileName, text);
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read save file");
                Console.WriteLine(e.Message);
            }
        }

        public void ChangeGameSave(save save, int levelName, int score) {
            string temp = File.ReadAllText(_savesFileName);
            temp = temp.Replace($"levelName {save.levelName}", $"levelName {levelName}");
            temp = temp.Replace($"score {save.score}",$"score {score}");
            File.WriteAllText(_savesFileName, temp);
        }

        public void addBestScore(string name, int score) {
            using (StreamWriter writer =  File.AppendText(_savesFileName)) {
                writer.WriteLine($"{name} {score}");
            }
        }
    }

    public class save {
        public int id { get; private set; }
        public string name { get; private set; }
        public int levelName { get; private set; }
        public int score { get; private set; }
        public save(int id, string name, int levelName, int score) {
            this.id = id;
            this.name = name;
            this.levelName = levelName;
            this.score = score;
        }
    }
}