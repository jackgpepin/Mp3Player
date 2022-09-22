using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicData;
using Mp3Player.Models;

namespace Mp3Player.Services;

public class Database: IDatabase
{
    public readonly string _dbPath = "playlists.json";
    public List<Playlist> GetPlaylists()
    {
        if (!File.Exists(_dbPath))
        {
            File.WriteAllText(_dbPath, JsonSerializer.Serialize(new List<Playlist>()));
        }
        var playlistsJson = File.ReadAllText(_dbPath);
        var playlists = JsonSerializer.Deserialize<List<Playlist>>(playlistsJson);
        return playlists;
    }

    public void StorePlaylist(Playlist playlist)
    {
        var playlists = GetPlaylists();
        playlists.Add(playlist);

        WritePlaylists(playlists);
    }

    public void UpdatePlaylist(Playlist playlist)
    {
        var playlists = GetPlaylists();
        var pOriginal = playlists.First(p => p.Id == playlist.Id);
        playlists.Replace(pOriginal, playlist);
        WritePlaylists(playlists);
    }

    public void DeletePlaylist(Playlist playlist)
    {
        var playlists = GetPlaylists();
        var pOriginal = playlists.First(p => p.Id == playlist.Id);
        playlists.Remove(pOriginal);
        WritePlaylists(playlists);
    }

    public void RemoveFile(Playlist playlist, PlaylistFile file)
    {
        var playlists = GetPlaylists();
        var pOriginal = playlists.First(p => p.Id == playlist.Id);
        var originalFile = pOriginal.PlaylistFiles.First(f => f.Id == file.Id);
        if (originalFile == null)
            return;
        pOriginal.PlaylistFiles.Remove(originalFile);
        WritePlaylists(playlists);
    }

    private void WritePlaylists(List<Playlist> playlists)
    {
       var playlistsJson = JsonSerializer.Serialize(playlists);
       File.WriteAllText(_dbPath,playlistsJson);

    }
    

}