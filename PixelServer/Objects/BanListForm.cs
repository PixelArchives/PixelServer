namespace PixelServer.Objects;

public class BanListForm
{
    ///<summary>Unique id of the player</summary>
    public long? id { get; set; }

    ///<summary>App version, for some reason formated in this way: $"{platform}:{appver}"</summary>
    public string? app_version { get; set; }
}
