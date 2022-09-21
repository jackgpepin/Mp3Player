using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Mp3Player.Models;

namespace Mp3Player.Configs;

public class Settings
{
    public static Settings SettingsInstance = null;
    public static readonly string SettingsPath = "settings.json";
    public Settings()
    {
        Playlists = new List<Playlist>();
    }
    public static Settings Initialize()
    {
        if (!File.Exists(SettingsPath))
        {
            var content = new Settings();
            var json = JsonSerializer.Serialize(content);
            File.WriteAllText(SettingsPath, json);
        }
        var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(SettingsPath));
        SettingsInstance = settings;
        return settings;

    }

    public void Save()
    {
        
    }
    public List<Playlist> Playlists { get; set; }
}