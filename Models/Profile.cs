using System.Collections.Generic;
using Mp3Player.Configs;
using Mp3Player.Services;

namespace Mp3Player.Models;

public class Profile
{
    public static Profile? ActiveProfile;
    public string Name { get; set; } = string.Empty;
    public string Id { get; set; }
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    public Settings? Settings { get; set; }

    public static List<Profile> GetProfiles()
    {
        var profiles = new List<Profile>();
        IDatabase database = new Database();
        profiles = database.GetProfiles();
        return profiles;
    }

    public void Save()
    {
        IDatabase database = new Database();
        database.CreateProfile(this);
    }

    public void Update()
    {
        IDatabase database = new Database();
        database.UpdateProfile(this);
    }
    
    public void Delete()
    {
        IDatabase database = new Database();
        database.DeleteProfile(this);
    }
    
    public static void SaveProfiles(List<Profile> profiles)
    {
        IDatabase database = new Database();
        database.SaveProfiles(profiles);
    }
    
}