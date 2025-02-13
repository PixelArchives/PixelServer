using MySqlConnector;
using System.Diagnostics;

namespace PixelServer.Helpers;

public static class AccountHelper
{
    /// <summary>Called in "create_player_intent"</summary>
    /// <returns></returns>
    public static async Task<string> CreateAccountToken()
    {
        var db = Db.Get();
        await db.OpenAsync();

        string token = Guid.NewGuid().ToString();

        // Insert the new account with the generated token
        using MySqlCommand insertCommand = new("INSERT INTO `accounts` (token) VALUES (@token);", db);
        insertCommand.Parameters.AddWithValue("@token", token);
        await insertCommand.ExecuteNonQueryAsync();

        // Get the last inserted ID
        using MySqlCommand idCommand = new("SELECT LAST_INSERT_ID();", db);
        object? result = await idCommand.ExecuteScalarAsync();

        // Check if result is valid
        if (result != null && long.TryParse(result.ToString(), out long accountId))
        {
            DebugHelper.Log($"Created account with ID: {accountId}, Token: {token}");
            return token;
        }

        return "fail";
    }

    /// <summary>Updates data of account with token, was cut in 2 becase of "create_account_intent" and "create_account".</summary>
    /// <param name="token">Unique token of the player.</param>
    /// <returns>Player exists?</returns>
    public static async Task<long> CreateAccount(string? token)
    {
        if (string.IsNullOrEmpty(token)) return 0;

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT `id` FROM `accounts` WHERE `token` = @token;", db);
        command.Parameters.AddWithValue("@token", token);

        object? result = await command.ExecuteScalarAsync();

        if (long.TryParse(Convert.ToString(result), out long parsed))
        {
            return parsed;
        }

        return 0;
    }

    /// <summary>Checks if account exists in database, if not, creates new.</summary>
    /// <param name="id">Unique ID of the player.</param>
    /// <returns>Player exists?</returns>
    public static async Task<string> AccountExists(long? id, bool createNewIfNotExists = true)
    {
        if (id == null) return "fail";

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT EXISTS(SELECT 1 FROM `accounts` WHERE `id` = @id);", db);
        command.Parameters.AddWithValue("@id", id);

        object? result = await command.ExecuteScalarAsync(); // Gets the result (0 or 1)

        bool exists = Convert.ToBoolean(result);

        if (exists) return "exists";

        string token = await CreateAccountToken();
        long account = await CreateAccount(token);

        return account.ToString();
    }

    // int because -1 may be isDeveloper, however, in database its boolean right now.
    // Still not sure about developer thing

    /// <summary>Checks if player is banned.</summary>
    /// <param name="id">ID of the player.</param>
    /// <returns>Not banned: 0, Banned: 1</returns>
    public static async Task<int> IsBanned(long? id)
    {
        if (id == null)
        {
            DebugHelper.LogWarning("IsBanned was called with null id, this shouldn't happen.");
            return 0;
        }

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT `banned` FROM `accounts` WHERE `id` = @id;", db);
        command.Parameters.AddWithValue("@id", id);

        object? result = await command.ExecuteScalarAsync();

        if (result != null)
        {
            return Convert.ToInt32(result);
        }

        return 0;
    }

    /// <summary>Bans/Unbans player.</summary>
    /// <param name="id">ID of the player.</param>
    /// <param name="isUnban">Optional, set to true if you want to UNBAN someone.</param>
    public static async Task BanPlayer(long id, bool isUnban = false)
    {
        using var db = await Db.GetOpen();

        string setTo = isUnban ? "0" : "1";

        using MySqlCommand command = new($"UPDATE `accounts` SET `banned` = {setTo} WHERE `id` = @id;", db);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
}
