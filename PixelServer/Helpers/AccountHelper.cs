using MySqlConnector;
using PixelServer.Objects;
using System.Text;

namespace PixelServer.Helpers;

public static class AccountHelper
{
    #region Account Creation
    /// <summary>Called in "create_player_intent"</summary>
    /// <returns></returns>
    public static async Task<string> CreateAccountToken()
    {
        using var db = await Db.GetOpen();

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
    /// <returns>New or already registred player.</returns>
    public static async Task<string> GetOrCreate(long? id)
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
    #endregion

    #region Ban
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
    #endregion

    #region Player Info

    public static async Task UpdateInfo(ActionForm form)
    {
        try
        {
            if (form.id == null) return;

            StringBuilder commandBuilder = new();

            commandBuilder.AppendLine("UPDATE `accounts`");
            commandBuilder.AppendLine("SET `nick` = @nick,");
            commandBuilder.AppendLine("`rank` = @rank,");
            if (!string.IsNullOrEmpty(form.skin)) commandBuilder.AppendLine("`skin` = @skin,");
            commandBuilder.AppendLine("`paying` = @paying");
            commandBuilder.AppendLine("WHERE `id` = @id;");

            using var db = await Db.GetOpen();

            using var command = new MySqlCommand(commandBuilder.ToString(), db);
            command.Parameters.AddWithValue("@nick", form.nick);
            command.Parameters.AddWithValue("@rank", form.rank);
            command.Parameters.AddWithValue("@paying", form.paying);
            command.Parameters.AddWithValue("@id", form.uniq_id);
            if (!string.IsNullOrEmpty(form.skin)) command.Parameters.AddWithValue("@skin", form.skin);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            DebugHelper.LogException("Error on updating the player info", ex);
        }
    }

    public static async Task<PlayerData?> GetInfoById(long? id)
    {
        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `accounts` WHERE `id` = @id;", db);
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync()) return DatabaseToPlayerData(reader);

        return null;
    }

    public static async Task<Dictionary<long, PlayerData>> GetShortInfoById()
    {
        Dictionary<long, PlayerData> result = new();

        result.Add(7, await GetInfoById(7));

        return result;
    }

    public static async Task<List<PlayerData>> GetByParam(string? param)
    {
        List<PlayerData> result = new();

        if (param == null) return result;

        if (long.TryParse(param, out long res))
        {
            PlayerData? d = await GetInfoById(res);

            if (d != null) result.Add(d);
        }

        using var db = await Db.GetOpen();

        using var command = new MySqlCommand("SELECT * FROM `accounts` WHERE `nick` LIKE @nick;", db);

        command.Parameters.AddWithValue("@nick", $"%{param}%");

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) result.Add(DatabaseToPlayerData(reader));

        return result;
    }

    public static async Task<string> GetByParamPost(string? param)
    {
        List<PlayerData> data = new();
        return null;
    }
    #endregion

    static PlayerData DatabaseToPlayerData(MySqlDataReader reader)
    {
        // 0 = Defs.RatingDeathmatch
        // Why the fuck 1 isnt used.
        // 2 = Defs.RatingTeamBattle
        // 3 = Defs.RatingHunger
        // 4 = Defs.RatingCapturePoint
        return
            new()
            {
                id = Convert.ToInt64(reader["id"]),
                nick = Convert.ToString(reader["nick"]),
                rank = Convert.ToString(reader["rank"]),
                skin = Convert.ToString(reader["skin"]),
                //clan_name = "Database Test",
                //clan_logo = Convert.ToString(reader["nick"]),
                clan_creator_id = 22,
                wincount =
                    {
                        {0,  Convert.ToInt32(reader["RatingDeathmatch"])},
                        //{1,  Convert.ToInt32(reader["TemporaryValue"])},
                        {2,  Convert.ToInt32(reader["RatingTeamBattle"])},
                        {3,  Convert.ToInt32(reader["RatingHunger"])},
                        {4,  Convert.ToInt32(reader["RatingCapturePoint"])}
                    }
            };
    }
}
