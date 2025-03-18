namespace PixelServer.Objects;

/// <summary>Player Data used in GetInfoById/Param and etc</summary>
public class PlayerData
{
    public long id;
    public string nick = string.Empty;
    public string rank = string.Empty;
    public string skin = string.Empty;
    public string clan_name = string.Empty;
    public string clan_logo = string.Empty;

    public long clan_creator_id;

    // empty, yes, its supposed to not have key '1'
    public Dictionary<int, int> wincount = new()
    {
        {0, 0}, {2, 0}, {3, 0}, {4, 0}
    };
}
