using MySqlConnector;

namespace PixelServer.Helpers;

public static class ConfigHelper
{
    //W.I.P

    /*static readonly Dictionary<string, string> cache = new();

    public static void ClearCache() => cache.Clear();

    public static async Task<string?> Get(string key)
    {
        if (cache.TryGetValue(key, out string? cachedVal)) return cachedVal;

        using var db = await Db.GetOpen();

        using var command = new MySqlCommand("SELECT `value` FROM `config` WHERE `key` = @key;");
        command.Parameters.AddWithValue("@key", key);

        var resultRaw = await command.ExecuteScalarAsync();

        string? result = Convert.ToString(resultRaw);

        if (string.IsNullOrEmpty(result)) return null;

        cache.Add(key, result);

        return result;
    }

    public static async Task Set(string key, string value)
    {
        try
        {
            using var db = await Db.GetOpen();

            using var command = new MySqlCommand("INSERT INTO `config` (`key`, `value`) VALUES (@key, @value)", db);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@value", value);

            int affected = await command.ExecuteNonQueryAsync();

            cache[key] = value;

            DebugHelper.Log($"[CONFIG] key \"{key}\" set succesfully.");
        }
        catch (Exception ex)
        {
            DebugHelper.LogException($"Couldn't add {value} with key \"{key}\"", ex, false);
        }
    }

    public static async Task<bool> GetBool(string key, bool defaultValue = false)
    {
        string? theThing = await Get(key);
        DebugHelper.Log(theThing);
        bool result = defaultValue;

        if (theThing == null)
        {
            await Set(key, defaultValue.ToString());
            return result;
        }


        if (bool.TryParse(theThing, out bool res)) result = res;
        else
        {
            await Set(key, defaultValue.ToString());
        }

        return result;
    }*/
}