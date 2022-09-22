using System.Collections.Generic;
using Mp3Player.Models;
using Mp3Player.ViewModels;

namespace Mp3Player.Services;

public interface IDatabase
{
    public List<Playlist> GetPlaylists();
    public void StorePlaylist(Playlist playlist);
    public void UpdatePlaylist(Playlist playlist);
    public void DeletePlaylist(Playlist playlist);
}