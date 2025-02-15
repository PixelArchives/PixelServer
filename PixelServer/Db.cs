﻿using MySqlConnector;

namespace PixelServer;

public static class Db
{
    /// <summary>CALL ON SERVER START! Creates default tables in database if they dont exist.</summary>
    public static async Task Init()
    {
        using var conn = await GetOpen();

        var command = new MySqlCommand(GetInitCommands(), conn);
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>Get raw, not open connection with <see cref="Settings.mySqlConnectionString"/>, USE `using` WHEN TO DISPOSE AFTER USING!</summary>
    public static MySqlConnection Get()
    {
        return new MySqlConnection(Settings.mySqlConnectionString);
    }

    /// <summary>Get open connection with <see cref="Settings.mySqlConnectionString"/>, USE `using` WHEN TO DISPOSE AFTER USING!</summary>
    public static async Task<MySqlConnection> GetOpen()
    {
        MySqlConnection conn = Get();

        await conn.OpenAsync();

        return conn;
    }

    private static string GetInitCommands()
    {
        var result = new System.Text.StringBuilder();

        // accounts
        result.AppendLine("CREATE TABLE IF NOT EXISTS `accounts`");
        result.Append("("); // opening
        result.Append("`id` BIGINT AUTO_INCREMENT PRIMARY KEY,");
        result.Append("`token` VARCHAR(255) NOT NULL,");
        result.Append("`device` VARCHAR(255) NOT NULL,");
        result.Append("`banned` tinyint(1) NOT NULL DEFAULT 0,");
        result.Append("`RatingDeathmatch` INT NOT NULL DEFAULT '0',");
        //result.Append("`TemporaryValue` INT NOT NULL DEFAULT '0',");
        result.Append("`RatingTeamBattle` INT NOT NULL DEFAULT '0',");
        result.Append("`RatingHunger` INT NOT NULL DEFAULT '0',");
        result.Append("`RatingCapturePoint` INT NOT NULL DEFAULT '0'");
        result.Append(");"); // closing

        // badfilter
        result.AppendLine("CREATE TABLE IF NOT EXISTS `badfilter`");
        result.Append("("); // opening
        result.Append("`value` tinytext NOT NULL,");
        result.Append("`is_symbol` tinyint(1) NOT NULL DEFAULT 0");
        result.Append(");"); // closing

        // config
        result.AppendLine("CREATE TABLE IF NOT EXISTS `config`");
        result.Append("("); // opening
        result.Append("`key` VARCHAR(100) NOT NULL UNIQUE,");
        result.Append("`value` VARCHAR(255) NOT NULL");
        result.Append(");"); // closing

        return result.ToString();
    }
}
