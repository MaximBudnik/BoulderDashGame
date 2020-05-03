using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibrary {
    public class DataInterlayer {
        private readonly string _scoresFileName =
            Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "scores.txt");

        private readonly string _savesFileName = Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "saves.txt");

        public List<Save> Saves = new List<Save>();
        public SortedDictionary<int, string> GetBestScores() {
            var results = new SortedDictionary<int, string>();
            try {
                using var fs = new StreamReader(_scoresFileName);
                while (true) {
                    var temp = fs.ReadLine();
                    if (temp == null) break;
                    var name = "";
                    var score = "";
                    var flag = false;
                    for (var i = 0; i < temp.Length; i++) {
                        var symb = ' ';
                        if (temp[i] == symb) {
                            flag = true;
                            continue;
                        }
                        if (flag == false)
                            name += temp[i];
                        else if (flag) score += temp[i];
                    }
                    results[int.Parse(score)] = name;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read file with best scores");
                Console.WriteLine(e.Message);
            }
            return results;
        }

        public void GetGameSaves() {
            var allSaves = new List<Save>();
            var id = 0;
            string name = null;
            var levelName = 0;
            var score = 0;
            try {
                using (var fs = new StreamReader(_savesFileName)) {
                    while (true) {
                        var temp = fs.ReadLine();
                        if (temp == null) {
                            allSaves.Add(new Save(id, name, levelName, score));
                            break;
                        }
                        var command = "";
                        var value = "";
                        var flag = false;
                        for (var i = 0; i < temp.Length; i++) {
                            var symb = ' ';
                            if (temp[i] == symb) {
                                flag = true;
                                continue;
                            }
                            if (flag == false)
                                command += temp[i];
                            else value += temp[i];
                        }
                        switch (command) {
                            case "save":
                                allSaves.Add(new Save(id, name, levelName, score));
                                id = int.Parse(value);
                                name = null;
                                levelName = 0;
                                score = 0;
                                break;
                            case "name":
                                name = value;
                                break;
                            case "levelName":
                                levelName = int.Parse(value);
                                break;
                            case "score":
                                score = int.Parse(value);
                                break;
                        }
                    }
                }
                allSaves.RemoveAt(0);
                Saves = allSaves;
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read save file");
                Console.WriteLine(e.Message);
            }
        }
        public void AddGameSave(string name) {
            using var writer = File.AppendText(_savesFileName);
            writer.WriteLine($"save {Saves.Count}");
            writer.WriteLine($"name {name}");
            writer.WriteLine("levelName 1");
            writer.WriteLine("score 0");
            GetGameSaves();
        }

        public void DeleteGameSave(int id) {
            var text = "";
            var counter = 0;
            try {
                using (var fs = new StreamReader(_savesFileName)) {
                    while (true) {
                        var temp = fs.ReadLine();
                        if (temp == null) break;
                        var command = "";
                        var value = "";
                        var flag = false;
                        for (var i = 0; i < temp.Length; i++) {
                            var symb = ' ';
                            if (temp[i] == symb) {
                                flag = true;
                                continue;
                            }
                            if (flag == false)
                                command += temp[i];
                            else value += temp[i];
                        }
                        if (command == "save" && int.Parse(value) == id) counter = -4;
                        if (counter >= 0) text += temp + "\n";
                        counter++;
                    }
                }
                File.WriteAllText(_savesFileName, text);
                GetGameSaves();
            }
            catch (Exception e) {
                Console.WriteLine("Unable to read save file");
                Console.WriteLine(e.Message);
            }
        }

        public void ChangeGameSave(Save save, int levelName, int score) {
            var temp = File.ReadAllText(_savesFileName);
            temp = temp.Replace($"levelName {save.LevelName}", $"levelName {levelName}");
            temp = temp.Replace($"score {save.Score}", $"score {score}");
            File.WriteAllText(_savesFileName, temp);
            GetGameSaves();
        }

        public void AddBestScore(string name, int score) {
            using var writer = File.AppendText(_savesFileName);
            writer.WriteLine($"{name} {score}");
        }
    }

    public class Save {
        public int Id { get; }
        public string Name { get; }
        public int LevelName { get; }
        public int Score { get; }
        public Save(int id, string name, int levelName, int score) {
            this.Id = id;
            this.Name = name;
            this.LevelName = levelName;
            this.Score = score;
        }
    }
}