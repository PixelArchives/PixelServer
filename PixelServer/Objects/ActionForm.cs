namespace PixelServer.Objects;

///<summary>Action form for /actions.php request</summary>
public class ActionForm
{
    ///<summary>Action being requested.</summary>
    public string? action { get; set; }

    ///<summary>Platform.</summary>
    public Platform? platform { get; set; }

    ///<summary>Gamemode player was or currently in.</summary>
    public string? mode { get; set; }

    ///<summary>time spend in <see cref="mode"/>.</summary>
    public string? time { get; set; }

    public long? uniq_id { get; set; }

    ///<summary>Device unique ID, its gura-... gurua-.. fuck this, its just always unique.</summary>
    public string? device { get; set; }

    ///<summary>App version, for some reason formated in this way: $"{platform}:{appver}"</summary>
    public string? app_version { get; set; }

    ///<summary>ToD.o</summary>
    public string? ids { get; set; }

    ///<summary>Player Rank.</summary>
    public string? rank { get; set; }

    ///<summary>ToDo.</summary>
    public string? platform_id { get; set; }

    ///<summary>ToDo.</summary>
    public string? version { get; set; }

    ///<summary>ToDo</summary>
    public string? param { get; set; }

    ///<summary>Use with HashHelper.</summary>
    public string? auth { get; set; }

    ///<summary>Is reqeusted from friends scene?</summary>
    public int? from_friends { get; set; }

    ///<summary>Private messages from chat.</summary>
    public string? private_messages { get; set; }

    ///<summary>Player's level</summary>
    public int? level { get; set; }
    
    ///<summary>Is user paying?</summary>
    public int? paying { get; set; }
    
    ///<summary>Is user developer?</summary>
    public int? developer { get; set; }

    ///<summary>ToDo</summary>
    public string? token { get; set; }
}
