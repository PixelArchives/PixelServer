namespace PixelServer.Objects;

///<summary>Class containing Lobby News Item, has to be used in List or Array</summary>
public class LobbyNews
{
    /// <summary>Date in unix format</summary>
    public ulong date { get; set; }

    /// <summary>URL when clicking on Link button under post</summary>
    public string URL { get; set; } = string.Empty;

    /// <summary>URL of small preview picture</summary>
    public string previewpicture { get; set; } = string.Empty;

    /// <summary>URL of news picture</summary>
    public string fullpicture { get; set; } = string.Empty;

    /// <summary>Translated short header</summary>
    public Dictionary<string, string> short_header { get; set; } = new();

    /// <summary>Translated header</summary>
    /// <remarks>Language Name | text</remarks>
    public Dictionary<string, string> header { get; set; } = new();

    /// <summary>Translated short description</summary>
    /// <remarks>Language Name | text</remarks>
    public Dictionary<string, string> short_description { get; set; } = new();

    /// <summary>Translated description</summary>
    /// <remarks>Language Name | text</remarks>
    public Dictionary<string, string> description { get; set; } = new();

    /// <summary>Translated category</summary>
    /// <remarks>Language Name | text</remarks>
    public Dictionary<string, string> category { get; set; } = new();
}
