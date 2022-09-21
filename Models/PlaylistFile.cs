namespace Mp3Player.Models;

public class PlaylistFile
{
    public string FilePath { get; set; } = string.Empty;

    public PlaylistFile(string filePath)
    {
        FilePath = filePath;
    }
}