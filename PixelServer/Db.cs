﻿using System.Threading.Tasks;
using MySqlConnector;

namespace PixelServer;

public static class Db
{
    private static bool inited;

    public static async Task Init()
    {
        if (inited) return;

        using var conn = await GetOpen();

        var command = new MySqlCommand(GetInitCommands(), conn);
        await command.ExecuteScalarAsync();

        inited = true;
    }

    public static MySqlConnection Get()
    {
        return new MySqlConnection(Settings.mySqlConnectionString);
    }

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
        result.AppendLine("("); // opening
        result.AppendLine("`id` BIGINT AUTO_INCREMENT PRIMARY KEY,");
        result.AppendLine("`token` VARCHAR(255) NOT NULL,");
        result.AppendLine("`device` VARCHAR(255) NOT NULL,");
        result.AppendLine("`banned` tinyint(1) NOT NULL DEFAULT 0");
        result.AppendLine(");"); // closing

        // badfilter
        result.AppendLine("CREATE TABLE IF NOT EXISTS `badfilter`");
        result.AppendLine("("); // opening
        result.AppendLine("`value` tinytext NOT NULL,");
        result.AppendLine("`is_symbol` tinyint(1) NOT NULL DEFAULT 0");
        result.AppendLine(");"); // closing

        return result.ToString();
    }
}
