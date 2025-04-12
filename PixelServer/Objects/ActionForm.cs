namespace PixelServer.Objects;

///<summary>Action form for /actions.php request</summary>
public class ActionForm
{
    ///<summary>Action being requested.</summary>
    public string? action { get; set; }

    ///<summary>Client`s platform.</summary>
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

    ///<summary>Player's ID</summary>
    public long? id { get; set; }

    ///<summary>Player's nickname</summary>
    public string? nick { get; set; }

    ///<summary>Player's skin</summary>
    public string? skin { get; set; }

    ///<summary>ID whoever player is interacting with.</summary>
    public long? whom { get; set; }

    ///<summary>Used when getting infos by id (short) when in friend requests menu</summary>
    public string? ids { get; set; }

    ///<summary>Player's level.</summary>
    public string? rank { get; set; }

    ///<summary>ToDo.</summary>
    public string? platform_id { get; set; }

    ///<summary>ToDo.</summary>
    public string? version { get; set; }

    ///<summary>Used when in friends, getting user list by param, which is passed here.</summary>
    public string? param { get; set; }

    ///<summary>Use with HashHelper.</summary>
    public string? auth { get; set; }

    ///<summary>Is reqeusted from friends scene?</summary>
    public int? from_friends { get; set; }

    ///<summary>Private messages from chat.</summary>
    public string? private_messages { get; set; }
    
    ///<summary>Is user paying?</summary>
    public int? paying { get; set; }
    
    ///<summary>Is user a developer?</summary>
    public int? developer { get; set; }

    ///<summary>Player's unique token generated on account creation.</summary>
    public string? token { get; set; }
}
