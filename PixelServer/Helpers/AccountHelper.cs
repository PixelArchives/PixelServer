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
    public static async Task<PlayerData?> GetInfoById(long? id)
    {
        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `accounts` WHERE `id` = @id;", db);
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            // 0 = Defs.RatingDeathmatch
            // Why the fuck 1 isnt used.
            // 2 = Defs.RatingTeamBattle
            // 3 = Defs.RatingHunger
            // 4 = Defs.RatingCapturePoint
            PlayerData result = new()
            {
                id = Convert.ToInt64(reader["id"]),
                nick = "Database i guess",
                rank = "22",
                skin = "iVBORw0KGgoAAAANSUhEUgAAAEAAAAAgCAYAAACinX6EAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAWwSURBVGhD5ZfNa1xVGMYvfqwEUQwiatNK2qbNVydTmy+b0AabSjW1hTitTZAuYhxCKC6MzSINElCIoJQusohKQCgoBHe6EQJKFi5UaImRrKIk6bL/wvH+ztzn8s7tmdRIHUMceHjPPfc975zfc879iuKf20pHLx72auh/oayN2k/Vu8Nn9mypUM0dpmBnKgF3DTd6WQMQkA29ta7l4r40ol1lgOCbzx+oaEBNTU2Z6NsVBgBpV54oU4i73gALyw5A2hH/GwNC8PczoD7/zO4xQOCthXrf5u6fNSCkXWOAwK3olwFbKVRzR6mlcMAhoFrOH/SwXcUW13m5pDZWeqjBr3hz4aC/EXoD4j4MUB5jGEsNXyupK6PsDZQ2YicFJ1VNWQNQWwwGCIDdY7kSaCKO+z7Ku1emj/k8DFEOY3xfUscaAKy9b8gAFJxUNcWk88mqAXHsUoM7EoM195eOgRv4oM+9+eEZ9/YnA25pacnr1audrmes1U1PT7uJiQm/8oxlDLWoSW2ABU//jjOAVWorHHbtbzX4CfuVSybfNdzqzr130v0cPetVeL/Pzc/Pp8dzc3Puxo0bbmZmxk1OTrqpqSk/zteKa1ILSLvyRJlCDE6qmtJWRbnzh1zTa3XuyMX9rnu00b189UV34VpfCnzh2km3uLiYHi8sLLibN2+62dnZdCdQw9a0sLo8tCNQcFLVlCbKirFtjw+1uKbCXtcxHINcqvNG8DsaPZxGtVl1wK9fv+5Xn7F+6yerLwMqwe+IHaBtz7bd3/286x1tdS8VG9MPmo7L8XWcQGeVj8eRQy5jGEsNanlTk3uAwHV/sO8RwUlVU/p6E3T3aItf/fzQAXcohjvY95xfbYCzsbF/r88ht2es2Y89ET8OiaqHMEk7QJJxwUlVV+UdD7Vf8YpOjPtYW1tbpmx+Vpubmx4uJM7Z2lnRH6ppRQ1+m3/86du8ce7LPZ3G0Jj7KNPBJMwEt2vAyspK2euw1cbGRhmsbes4VNMKaODv3r377xqgCT1IAzgn6HvAFQM1rbQDZICFJ4bG3EflHXaCaLsGMKnQ9kecE2glhWpayQDigzEgP+i8zn5aio2vl2JnsbRCHFvRx7kk99atW251dTXVTz/86H775VeXG6hzx680+YjoRzYXffPV1+77b79LpTzaANpzOk99iRp37txx6+vraU3VQGFoK8FnDQBU2zIB923yTk+XTIj79adoeXk5nRh/buHthDFNxgkqKwFjkMSx6ug/qCF4ovpUJwxtBQhQxATKGwBk3J9uzcSQ6I3PSjExS0BExCPRThDZSZOztraWjlEO3xk8Pnl/UJ9M4LEcMoDXdKT/tvUQ58LQVgmohwI+Fqvmr6f4nODLjDCXAH9qV3RkZMRdiB7zz3r/ThG/SNlJ23zERPnC1JujVlsGAC8D1LbwMoB+GUDf3zcAaAyIIwN6Bo/6Nm9qRAvuI+BbGDA+Pu5NQLwZYgJtrabyf799OzUA8NnoKa8vP/8iNQAJGuXjbxRBS/TZnOz5MLSRT4pBAKctA6R7DDDnEEDAEBEQmPBu9Li7Fj3hj1mxkAG0gSSHXMSxtnslA04Nd6SA6leO+qUgtNXpd7o8iB3UdrYp7Xt0MPKSETKIcShrABAfR0+mu6BYLKY7ACDtFEUBM045HGsXWLisAcSQAfQrJwhtBQSJCDjg1SbKAJmgXEkwMoEPIGC4DwDO6mr1swYwhj5kcyoZIEApa4AMsgpCW2UNELgk+Ed6S9GapB0gKMR2xwA+kri2BSY4m28NqCQLV8kA6R8ZkMvlPJAkQJlh4cnNSuACs5MH2l7/9GmnyASttlbcthlj4YhZQJ2X6NP2JwahrUJQ25GFIermWKY9x+7ZARpj86wB6hNY1gBBhnbIf2+AHpW8MySPWJlAnh2T5iePVQ/PGN5PjAGShUMCl7ZnQOT+AmZHLANVbu7rAAAAAElFTkSuQmCC",
                clan_name = "Database Test",
                clan_logo = "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAcElEQVQYGWNkYGD4D8QM\\/\\/+DKRATDBgZGcE0C6ObGsO\\/nTcZDquJQqUgFEgDk7s6Awuy5Jm97AhFQA3\\/br1mYIGJgCRNnH\\/CuHCaCc7CwYCbANKNbAXMNJBTge75j+FIW6D9IJ+ArQAxQAIwAJME8QEJJSYTV5YWAgAAAABJRU5ErkJggg==",
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

            return result;
        }

        return null;
    }

    public static async Task<Dictionary<long, PlayerData>> GetShortInfoById()
    {
        Dictionary<long, PlayerData> result = new();

        result.Add(7, await GetInfoById(7));

        return result;
    }
    

    public static async Task<List<PlayerData>> Test2()
    {
        List<PlayerData> result = new();

        result.Add(await GetInfoById(1));
        //result.Add(await GetInfoById(2));
        //result.Add(await GetInfoById(3));
        //result.Add(await GetInfoById(4));
        //result.Add(await GetInfoById(5));

        return result;
    }

    public static async Task UpdateInfo(ActionForm form)
    {
        if (form.uniq_id == null) return;

        StringBuilder builder = new();

        builder.Append("UPDATE `accounts` SET "); 

        if (form.paying != null) builder.Append("`paying` = @paying");
        if (form.developer != null) builder.Append("`developer` = @developer");

        builder.Append("WHERE `id` = @id;");

        using var db = await Db.GetOpen();

        using MySqlCommand command = new(builder.ToString(), db);
        command.Parameters.AddWithValue("@id", form.uniq_id);

        if (form.paying != null) command.Parameters.AddWithValue("@paying", form.paying);
        if (form.developer != null) command.Parameters.AddWithValue("@developer", form.developer);

        await command.ExecuteNonQueryAsync();
    }
    #endregion
}
