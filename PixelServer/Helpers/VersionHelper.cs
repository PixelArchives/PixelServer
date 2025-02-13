﻿using MySqlConnector;
using System;

namespace PixelServer.Helpers;

public static class VersionHelper
{
    public static List<string> allowedVersions { get; private set; } = new();

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

    public static bool IsValid(string? version)
    {
        if (allowedVersions.Count == 0)
        {
            DebugHelper.LogError("ERROR: allowedVersions IS NULL OR LENGHT IS 0, GAME IS CURRENTLY UNACCESABLE", true);
            return false;
        }
        else if (version == null) return false;

        return allowedVersions.Contains(version);
    }

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
                    DebugHelper.LogWarning($"allowed versions config didnt had {version} in it.");
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

    static async Task<string?> GetVersionString()
    {
        using var db = await Db.GetOpen();

        using MySqlCommand command = new($"SELECT `value` FROM config WHERE `key` = '{Consts.configGameVersionKey}';", db);

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToString(result);
    }
}
