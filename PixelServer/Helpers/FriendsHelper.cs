using MySqlConnector;
using System.ComponentModel;

namespace PixelServer.Helpers;

public static class FriendsHelper
{
    /// <summary>update_friends_info.</summary>
    /// <param name="id">playre's id</param>
    /// <returns>returns bunch of info related to friends.</returns>
    public static async Task<string> UpdateFriendsInfo(long? id)
    {
        if (id == null) return "fail";

        try
        {
            Dictionary<string, List<string>> result = new();

            // friends, invites, invites_outcoming

            var invOutTask = GetInvitesOutcoming(id);
            var invTask = GetInvitesIncoming(id);

            await Task.WhenAll(invOutTask, invTask);

            result["invites_outcoming"] = invOutTask.Result;
            result["invites"] = invTask.Result;

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

    static async Task<List<string>> GetInvitesOutcoming(long? from)
    {
        List<string> result = new();

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `friend_requests` WHERE `from` = @from", db);
        command.Parameters.AddWithValue("@from", from);

        var reader = command.ExecuteReader();

        if (reader != null)
        {
            while (await reader.ReadAsync())
            {
                string? toAdd = Convert.ToString(reader["to"]);

                if (toAdd != null) result.Add(toAdd);
            }
        }

        return result;
    }

    static async Task<List<string>> GetInvitesIncoming(long? to)
    {
        List<string> result = new();

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `friend_requests` WHERE `to` = @to", db);
        command.Parameters.AddWithValue("@to", to);

        var reader = command.ExecuteReader();

        if (reader != null)
        {
            while (await reader.ReadAsync())
            {
                string? toAdd = Convert.ToString(reader["from"]);

                if (toAdd != null) result.Add(toAdd);
            }
        }

        return result;
    }
}
