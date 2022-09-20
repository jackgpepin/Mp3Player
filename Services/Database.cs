using System.Collections.Generic;
using Mp3Player.Models;

namespace Mp3Player.Services;

public class Database: IDatabase
{
    public List<Playlist> GetPlaylists()
    {
        var playlists = new List<Playlist>();
        playlists.Add(new Playlist("Favorites"));
        playlists.Add(new Playlist("Best"));
        return playlists;
    }

    public void StorePlaylist(Playlist playlist)
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePlaylist(Playlist playlist)
    {
        throw new System.NotImplementedException();
    }

   
}