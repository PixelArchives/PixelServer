using MySqlConnector;
using PixelServer.Objects;

namespace PixelServer.Helpers;

public static class BadFilterHelper
{
    ///<summary>Cache to not open database connection every time.</summary>
    private static BadFilterContainer? cache;

    ///<summary>Gets cache, or creates it from database to and returns it, force with <paramref name="force"/>.</summary>
    public static async Task<BadFilterContainer> GetOrCreate(bool force = false)
    {
        if (cache != null && !force) return cache;

        BadFilterContainer result = new();

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `badfilter`", db);

        using MySqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            try
            {
                string value = reader.GetString("value");

                if (value.Length == 1)
                {
                    char c = char.Parse(value);
                    result.Symbols.Add(c);
                    continue;
                }

                result.Words.Add(value);
            }
            catch (Exception ex)
            {
                DebugHelper.LogError($"Exception on parsing BadWords: {ex}");
            }
        }

        cache = result;

        return cache;
    }

    ///<summary>Adds value to the database and cache, if <paramref name="is_symbol"/> lenght of value MUST BE 1, otherwise returns with warning.</summary>
    public static async Task TryAddValue(string value)
    {
        if (!Settings.badWordFiltering)
        {
            DebugHelper.LogWarning("Warning: Unable to add bad word, BadWordFiltering is disabled.");
            return;
        }

        try
        {
            if (string.IsNullOrEmpty(value) || value.Length > 255)
            {
                DebugHelper.LogWarning("Warning: Unable to add bad word, values is too long.");
                return;
            }

            bool is_symbol = value.Length == 1;

            using var db = await Db.GetOpen();

            using MySqlCommand command = new("INSERT INTO `badfilter` (value) VALUES (@value);", db);
            command.Parameters.AddWithValue("@value", value);

            await command.ExecuteNonQueryAsync();

            if (cache == null) await GetOrCreate();
            else
            {
                if (is_symbol) cache.Symbols.Add(char.Parse(value));
                else cache.Words.Add(value);
            }

            DebugHelper.Log("Added value succesfully");

            return;
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on adding value: {ex}");
        }
    }

    ///<summary>Adds value to the database and cache.</summary>
    public static async Task TryRemoveValue(string value)
    {
        try
        {
            using var db = await Db.GetOpen();

            using MySqlCommand command = new("DELETE FROM `badfilter` WHERE `value` = @value", db);
            command.Parameters.AddWithValue("@value", value);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            DebugHelper.Log($"Command executed, rows affected: {rowsAffected}");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on removing value: {ex}");
        }
    }
}
