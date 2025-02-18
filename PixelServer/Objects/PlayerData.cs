namespace PixelServer.Objects;

/// <summary>Player Data used in GetInfoById/Param and etc</summary>
public class PlayerData
{
    public long? id { get; set; }
    public string? nick { get; set; }
    public string? rank { get; set; }
    public string? skin { get; set; }
    public string? clan_name { get; set; }
    public string? clan_logo { get; set; }
    public long? clan_creator_id { get; set; }

    public Dictionary<int, int> wincount { get; set; } = new();
}
