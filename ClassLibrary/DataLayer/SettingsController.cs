using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClassLibrary.DataLayer {
    public class SettingsController {
        private Settings _retrieved;

        private readonly string _settingsFile =
            Path.Combine(Environment.CurrentDirectory, @"gameFiles\", "settings.json");

        public async Task WriteSettings(Settings set) {
            await using var fs = new FileStream(_settingsFile, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, set);
        }
        private async Task ReadSettings() {
            await using var fs = new FileStream(_settingsFile, FileMode.Open);
            _retrieved = await JsonSerializer.DeserializeAsync<Settings>(fs);
        }
        public async Task<Settings> GetSettings() {
            await ReadSettings();
            return _retrieved;
        }
    }
}