using MySqlConnector;

namespace PixelServer.Helpers;

public static class FriendsHelper
{
    public static async Task<string> UpdateFriendsInfo(long? id)
    {
        if (id == null) return "fail";

        try
        {
            Dictionary<string, string> result = new();

            // friends, invites, invites_outcoming

            return System.Text.Json.JsonSerializer.Serialize(result);
        }
        catch (Exception ex) 
        {
            DebugHelper.LogError("Exception on updating friends info, Ex: " + ex.Message);
            return "fail";
        }
    }

    public static async Task<bool> TrySendFriendRequest(long? from, long? to)
    {
        if (from == null || to == null) return false;

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("INSERT INTO `friend_requests` (`from`, `to`) VALUES (@from, @to)", db);
        command.Parameters.AddWithValue("@from", from);
        command.Parameters.AddWithValue("@to", to);

        try
        {
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0) return true;
            else return true;
        }
        catch (Exception ex)
        {
            DebugHelper.LogError("Exception on sending friend reqeust: " + ex.Message);
            return false;
        }
    }
}
