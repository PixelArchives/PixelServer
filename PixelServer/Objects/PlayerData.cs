namespace PixelServer.Objects;

/// <summary>Player Data used in GetInfoById/Param and etc</summary>
public class PlayerData
{
    public long id { get; set; }
    public string nick { get; set; } = string.Empty;
    public string rank { get; set; } = string.Empty;
    public string skin { get; set; } = string.Empty;
    public string clan_name { get; set; } = string.Empty;
    public string clan_logo { get; set; } = string.Empty;
    public long clan_creator_id { get; set; } = 0;

    public Dictionary<int, int> wincount { get; set; } = new();
}
