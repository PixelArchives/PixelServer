using MySqlConnector;

namespace PixelServer.Helpers;

public static class AccountHelper
{
    public static async Task<string> CreateAccountToken()
    {
        var db = Db.Get();
        await db.OpenAsync();

        string token = Guid.NewGuid().ToString();

        // Insert the new account with the generated token
        using var insertCommand = new MySqlCommand("INSERT INTO `accounts` (token) VALUES (@token);", db);
        insertCommand.Parameters.AddWithValue("@token", token);
        await insertCommand.ExecuteNonQueryAsync();

        // Get the last inserted ID
        using var idCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", db);
        object? result = await idCommand.ExecuteScalarAsync();

        // Check if result is valid
        if (result != null && long.TryParse(result.ToString(), out long accountId))
        {
            DebugHelper.Log($"Created account with ID: {accountId}, Token: {token}");
            return token;
        }

        return "fail";
    }


    public static async Task<long> CreateAccount(string? token)
    {
        if (string.IsNullOrEmpty(token)) return 0;

        using var db = await Db.GetOpen();

        using var command = new MySqlCommand("SELECT `id` FROM `accounts` WHERE `token` = @token;", db);
        command.Parameters.AddWithValue("@token", token);

        object? result = await command.ExecuteScalarAsync();

        if (long.TryParse(Convert.ToString(result), out long parsed))
        {
            return parsed;
        }

        return 0;
    }

    public static async Task<bool> AccountExists(string? id)
    {
        if (!string.IsNullOrEmpty(id) && long.TryParse(id, out long result)) return await AccountExists(result);

        DebugHelper.Log("False");
        return false;
    }

    public static async Task<bool> AccountExists(long id)
    {
        using var db = await Db.GetOpen();

        using var command = new MySqlCommand("SELECT EXISTS(SELECT 1 FROM `accounts` WHERE `id` = @id);", db);
        command.Parameters.AddWithValue("@id", id);

        object? result = await command.ExecuteScalarAsync(); // Gets the result (0 or 1)

        return Convert.ToInt32(result) == 1;
    }
}
