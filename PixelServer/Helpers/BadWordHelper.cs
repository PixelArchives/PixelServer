using MySqlConnector;
using PixelServer.Objects;

namespace PixelServer.Helpers;

public static class BadWordHelper
{
    private static BadWordContainer? cache;

    public static async Task<BadWordContainer> GetOrCreate(bool force = false)
    {
        if (cache != null && !force) return cache;

        BadWordContainer result = new BadWordContainer();

        using var db = await Db.GetOpen();

        using MySqlCommand command = new("SELECT * FROM `badfilter`", db);

        using MySqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            try
            {
                bool isChar = reader.GetBoolean("is_symbol");

                string value = reader.GetString("value");

                if (isChar)
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

    public static async Task<bool> AddValue(string value, bool is_symbol)
    {
        if (!Settings.badWordFiltering)
        {
            DebugHelper.LogWarning("Warning: Unable to add bad word, BadWordFiltering is disabled.");
            return false;
        }

        try
        {
            if ((!is_symbol && value.Length > 255) || (is_symbol && value.Length > 1))
            {
                DebugHelper.LogWarning("Warning: Unable to add bad word, values is too long.");
                return false;
            }

            using var db = await Db.GetOpen();

            using MySqlCommand command = new("INSERT INTO `badfilter` (value, is_symbol) VALUES (@value, @is_symbol);", db);
            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@is_symbol", is_symbol ? 1 : 0);

            await command.ExecuteNonQueryAsync();

            if (cache == null) await GetOrCreate();
            else
            {
                if (is_symbol) cache.Symbols.Add(char.Parse(value));
                else cache.Words.Add(value);
            }

            return true;
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on adding value: {ex}");
            return false;
        }
    }
}
