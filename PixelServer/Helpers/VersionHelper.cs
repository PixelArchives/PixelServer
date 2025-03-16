using MySqlConnector;
using PixelServer.Objects;
using System;

namespace PixelServer.Helpers;

public static class VersionHelper
{
    // Youre not supposed to destroy it.
    public static readonly List<string> allowedVersions = new();

    public static async Task CheckVersions()
    {
        try
        {
            using var db = await Db.GetOpen();

            using MySqlCommand command = new("SELECT `value` FROM `config` WHERE `key` = @key", db);
            command.Parameters.AddWithValue("@key", Consts.configGameVersionKey);

            var reader = await command.ExecuteReaderAsync();

            allowedVersions.Clear();

            while (reader.Read())
            {
                allowedVersions.Add(reader.GetString(0));
            }

            if (allowedVersions.Count == 0)
            {
                DebugHelper.LogWarning("allowedVersions IS EMPTY, GAME IS CURRENTLY ACCEPTING ALL VERSIONS");
                DebugHelper.LogWarning("USE 'version add {ver}' TO ADD VERSION");
            }
            else DebugHelper.Log($"Version check finished, resulted {allowedVersions.Count} versions allowed.");
        }
        catch (Exception ex)
        {
            DebugHelper.LogException($"Exception on checking verions: ", ex);
        }
    }

    ///<summary>Checks if <see cref="allowedVersions"/> contains <paramref name="version"/></summary>
    public static bool IsValid(string? version)
    {
        if (allowedVersions.Count == 0) return true;

        // The actual version is after :, first thing is platform (number, Platform.cs)
        string actualVer = version.Split(':')[1];

        return allowedVersions.Contains(actualVer);
    }

    /// <summary>
    /// Adds <paramref name="version"/> to the database if <see cref="allowedVersions"/> doesn't contains it,
    /// you can also force it with <paramref name="force"/>.
    /// </summary>
    public static async Task AddVersion(string version, bool force = false)
    {
        try
        {
            if (allowedVersions.Contains(version))
            {
                DebugHelper.LogWarning("allowedVersions already contains this value.");

                if (!force)
                {
                    DebugHelper.LogWarning("Aborting.");
                    return;
                }
            }

            using var db = await Db.GetOpen();

            //using var command = new MySqlCommand("SELECT * FROM `config` WHERE `key` = @key;", db);
            using var command = new MySqlCommand("INSERT INTO `config` (`key`, `value`) VALUES (@key, @value)", db);
            command.Parameters.AddWithValue("@key", Consts.configGameVersionKey);
            command.Parameters.AddWithValue("@value", version);

            int affected = await command.ExecuteNonQueryAsync();

            allowedVersions.Add(version);

            DebugHelper.Log($"Done. Rows affected: {affected}");
        }
        catch (Exception ex)
        {
            DebugHelper.LogException($"Exception on adding allowed version", ex);
        }
    }

    /// <summary>
    /// Removes <paramref name="version"/> from the database if <see cref="allowedVersions"/> contains it,
    /// you can also force it with <paramref name="force"/>.
    /// </summary>
    public static async Task RemoveVersion(string version, bool force = false)
    {
        try
        {
            if (!allowedVersions.Contains(version))
            {
                DebugHelper.LogWarning("allowedVersions does not contain this value.");

                if (!force)
                {
                    DebugHelper.LogWarning("Aborting.");
                    return;
                }
            }

            using var db = await Db.GetOpen();

            using var command = new MySqlCommand("DELETE FROM `config` WHERE `key` = @key AND `value` = @value;", db);
            command.Parameters.AddWithValue("@key", Consts.configGameVersionKey);
            command.Parameters.AddWithValue("@value", version);

            int affected = await command.ExecuteNonQueryAsync();

            if (!allowedVersions.Remove(version))
                DebugHelper.LogWarning("Value wasnt removed succesfully from the list.\nIts recommended to call 'version check' command.");

            DebugHelper.Log($"Done. Rows affected: {affected}");
        }
        catch (Exception ex)
        {
            DebugHelper.LogException($"Exception on removing allowed version", ex);
        }
    }

    // Shortcut for getting it from database
    static async Task<List<string?>> GetVersionString()
    {
        using var db = await Db.GetOpen();

        using MySqlCommand command = new($"SELECT `value` FROM config WHERE `key` = '{Consts.configGameVersionKey}';", db);

        object? result = await command.ExecuteScalarAsync();

        return null;// Convert.ToString(result);
    }
}
