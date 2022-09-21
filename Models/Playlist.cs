using System.Collections.Generic;
using Mp3Player.Services;

namespace Mp3Player.Models;

public class Playlist
{
    public string Name { get; set; } = string.Empty;
    public List<PlaylistFile> PlaylistFiles { get; set; } = new List<PlaylistFile>();

    public Playlist() {}
    
    public Playlist(string name)
    {
        Name = name;
    }

    public void Create()
    {
        if (this.Name == "Default") return;

        IDatabase database = new Database();
        database.StorePlaylist(this);
    }
    public void Save()
    {
        if (this.Name == "Default") return;

        IDatabase database = new Database();
        database.UpdatePlaylist(this);
    }

    public void Delete()
    {
        if (this.Name == "Default") return;

    }

    public void Rename()
    {
        if (this.Name == "Default") return;
    }
    public static List<Playlist> GetPlaylists()
    {
        
        IDatabase database = new Database();
        var playlists = database.GetPlaylists();
        return playlists;
    }
    
    
}