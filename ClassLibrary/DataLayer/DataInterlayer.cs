using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LiteDB;

namespace ClassLibrary.DataLayer {
    public class DataInterlayer {
        private readonly string _scoresDatabase =
            Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "BestScores.db");

        private readonly string _savesDatabase = Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "Saves.db");
        private readonly SettingsController settingsController = new SettingsController();
        public Settings settings;

        public DataInterlayer() {
            GetSettings();
        }
        private async void GetSettings() {
            settings = await settingsController.GetSettings();
        }

        public List<Save> GetAllGameSaves() {
            var result = new List<Save>();
            using var db = new LiteDatabase(_savesDatabase);
            var col = db.GetCollection<Save>("saves");
            var searchResult = col.FindAll();
            foreach (var save in searchResult) result.Add(save);
            return result;
        }

        public Save GetGameSaveByName(string name) {
            using var db = new LiteDatabase(_savesDatabase);
            var col = db.GetCollection<Save>("saves");
            col.EnsureIndex(x => x.Name);
            var result = col.FindOne(x => x.Name.Equals(name));
            return result;
        }

        public void AddGameSave(string name) {
            using var db = new LiteDatabase(_savesDatabase);
            var col = db.GetCollection<Save>("saves");
            var currentSave = new Save {Name = name, Score = 0, LevelName = 0};
            col.EnsureIndex(x => x.Name);
            if (col.Exists(x => x.Name == name)) col.Update(currentSave);
            else col.Insert(currentSave);
        }

        public void DeleteGameSave(Save save) {
            using var db = new LiteDatabase(_savesDatabase);
            var col = db.GetCollection<Save>("saves");
            col.Delete(save.Id);
        }

        public void ChangeGameSave(Save save) {
            using var db = new LiteDatabase(_savesDatabase);
            var col = db.GetCollection<Save>("saves");
            col.Update(save);
        }

        public void AddBestScore(string name, int score) {
            using var db = new LiteDatabase(_scoresDatabase);
            var col = db.GetCollection<Score>("scores");
            col.EnsureIndex(x => x.Name);
            var currentScore = new Score {Name = name, Value = score};
            if (col.Exists(x => x.Name == name))
                col.Update(currentScore);
            else
                col.Insert(currentScore);
        }

        public SortedDictionary<int, string> GetBestScores() {
            var result = new SortedDictionary<int, string>();
            using var db = new LiteDatabase(_scoresDatabase);
            var col = db.GetCollection<Score>("scores");
            var all = col.FindAll();
            foreach (var save in all)
                if (!result.ContainsKey(save.Value))
                    result.Add(save.Value, save.Name);
            return result;
        }
    }
}