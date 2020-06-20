using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClassLibrary.DataLayer {
    public class JsonController {
        private readonly string _customLevelsFile =
            Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "customLevels.json");

        private readonly string _settingsFile =
            Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "settings.json");

        private Settings _retrievedSettings;
        public List<CustomLevel> RetrievedLevels;

        public async Task WriteSettings(Settings set) {
            await using var fs = new FileStream(_settingsFile, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, set);
        }

        public async Task AddCustomLevel(List<CustomLevel> level) {
            await using var fs = new FileStream(_customLevelsFile, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, level);
        }

        private async Task ReadSettings() {
            await using var fs = new FileStream(_settingsFile, FileMode.Open);
            _retrievedSettings = await JsonSerializer.DeserializeAsync<Settings>(fs);
        }

        private async Task ReadCustomLevels() {
            await using var fs = new FileStream(_customLevelsFile, FileMode.Open);
            RetrievedLevels = await JsonSerializer.DeserializeAsync<List<CustomLevel>>(fs);
        }
        public async Task<Settings> GetSettings() {
            await ReadSettings();
            return _retrievedSettings;
        }

        public async Task<List<CustomLevel>> GetCustomLevels() {
            await ReadCustomLevels();
            return RetrievedLevels;
        }
    }
}