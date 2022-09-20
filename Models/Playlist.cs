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
        IDatabase database = new Database();
        database.StorePlaylist(this);
    }
    public void Save()
    {
        IDatabase database = new Database();
        database.UpdatePlaylist(this);
    }

    public void Delete()
    {
        
    }

    public void Rename()
    {
        
    }
    public static List<Playlist> GetPlaylists()
    {
        IDatabase database = new Database();
        var playlists = database.GetPlaylists();
        return playlists;
    }
    
    
}