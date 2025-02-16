using MySqlConnector;
using PixelServer.Objects;

namespace PixelServer.Helpers;

public static class BadWordHelper
{
    ///<summary>Cache to not open database connection every time.</summary>
    private static BadWordContainer? cache;

    ///<summary>Gets cache, or creates it from database to and returns it, force with <paramref name="force"/>.</summary>
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

    ///<summary>Adds value to the database and cache, if <paramref name="is_symbol"/> lenght of value MUST BE 1, otherwise returns warning.</summary>
    public static async Task TryAddValue(string value, bool is_symbol)
    {
        if (!Settings.badWordFiltering)
        {
            DebugHelper.LogWarning("Warning: Unable to add bad word, BadWordFiltering is disabled.");
            return;
        }

        try
        {
            if ((!is_symbol && value.Length > 255) || (is_symbol && value.Length > 1))
            {
                DebugHelper.LogWarning("Warning: Unable to add bad word, values is too long.");
                return;
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

            DebugHelper.Log("Added value succesfully");

            return;
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on adding value: {ex}");
            return;
        }
    }
}
