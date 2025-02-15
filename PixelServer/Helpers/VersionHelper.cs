using MySqlConnector;
using PixelServer.Objects;
using System;

namespace PixelServer.Helpers;

public static class VersionHelper
{
    public static List<string> allowedVersions { get; private set; } = new();

    ///<summary>RUN AT SERVER START! Gets game version from config database, if not set, sets to <see cref="Settings.defaultGameVersion"/></summary>
    public static async Task CheckVersion()
    {
        string? result = await GetVersionString();

        if (string.IsNullOrEmpty(result))
        {
            await AddVersion(Settings.defaultGameVersion);
        }
        else
        {
            string res = Convert.ToString(result);

            List<string> verions = res.Split('#').ToList();

            allowedVersions = verions;
        }
    }

    ///<summary>Checks if <see cref="allowedVersions"/> contains <paramref name="version"/></summary>
    public static bool IsValid(string? version)
    {
        if (allowedVersions.Count == 0)
        {
            DebugHelper.LogError("ERROR: allowedVersions LENGHT IS 0, GAME IS CURRENTLY UNACCESABLE", true);
            return false;
        }
        else if (version == null) return false;

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
                DebugHelper.LogWarning("List already contains the same version.");
                if (!force) return;
            }
            else allowedVersions.Add(version);

            string? result = await GetVersionString();

            using var db = await Db.GetOpen();

            if (string.IsNullOrEmpty(result)) result = version;
            else result += $"#{version}";

            using MySqlCommand command = new(@"
                INSERT INTO `config` (`key`, `value`) 
                VALUES (@key, @ver)
                ON DUPLICATE KEY UPDATE `value` = @ver;", db);

            command.Parameters.AddWithValue("@key", Consts.configGameVersionKey);
            command.Parameters.AddWithValue("@ver", result);

            await command.ExecuteNonQueryAsync();

            DebugHelper.Log("Added value succesfully");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on adding allowed version, ex: {ex.Message}");
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
                DebugHelper.LogWarning("List does not contain same version.");
                if (!force) return;
            }
            else allowedVersions.Remove(version);

            string? result = await GetVersionString();

            List<string> resultList = new();

            if (string.IsNullOrEmpty(result))
            {
                DebugHelper.LogError("Unable to get allowed versions from database");
                return;
            }
            else
            {
                resultList = result.Split('#').ToList();

                if (resultList.Contains(version)) resultList.Remove(version);
                else
                {
                    DebugHelper.LogWarning($"Allowed versions config didnt had {version} in it.");
                    return;
                }
            }

            using var db = await Db.GetOpen();

            result = string.Empty;

            bool isFirst = true;

            foreach (string item in resultList)
            {
                if (!isFirst) result += '#';
                else isFirst = false;

                result += item;
            }

            using MySqlCommand command = new(@"
                INSERT INTO `config` (`key`, `value`) VALUES (@key, @result)
                ON DUPLICATE KEY UPDATE `value` = @result;", db);

            command.Parameters.AddWithValue("@key", Consts.configGameVersionKey);
            command.Parameters.AddWithValue("@result", result);

            await command.ExecuteNonQueryAsync();

            DebugHelper.Log("Removed value succesfully");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Exception on removing allowed version, ex: {ex.Message}");
        }
    }

    // Shortcut for getting it from database
    static async Task<string?> GetVersionString()
    {
        using var db = await Db.GetOpen();

        using MySqlCommand command = new($"SELECT `value` FROM config WHERE `key` = '{Consts.configGameVersionKey}';", db);

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToString(result);
    }
}
