using System;

namespace Mp3Player.Models;

public class PlaylistFile
{
    public string Id { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public PlaylistFile(string filePath)
    {
        Id = Guid.NewGuid().ToString();
        FilePath = filePath;
    }
}