namespace PixelServer.Objects;

///<summary>Action form for /actions.php request</summary>
public class ActionForm
{
    ///<summary>Action being requested.</summary>
    public string? action { get; set; }

    ///<summary>Platform.</summary>
    public Platform? platform { get; set; }

    public long? uniq_id { get; set; }

    ///<summary>Device unique ID, its gura-... gurua-.. fuck this, its just always unique.</summary>
    public string? device { get; set; }

    ///<summary>App version, for some reason formated in this way: $"{platform}:{appver}"</summary>
    public string? app_version { get; set; }

    ///<summary>use with HashHelper</summary>
    public string? auth { get; set; }

    ///<summary>ToDo</summary>
    public string? token { get; set; }
}
