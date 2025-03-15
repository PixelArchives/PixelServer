using PixelServer.Objects;
using System.Text;

namespace PixelServer.Helpers;

public static class DebugHelper
{
    ///<summary>Logs message without any additional text.</summary>
    public static void Log(string message, ConsoleColor color = ConsoleColor.White)
    {
        if (Admin.AdminPanel.isTyping) return;

        if (string.IsNullOrEmpty(message))
        {
            LogWarning("The message was empty.");
            return;
        }

        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void LogWarning(string message)
        => Log(message, ConsoleColor.Yellow);

    public static void LogError(string message)
        => Log(message, ConsoleColor.Red);

    ///<summary>Logs message with [message] (from exception) and [stack trace]</summary>
    public static void LogException(string message, Exception ex)
    {
        Log(message, ConsoleColor.Red);
        Log($"[message]: {ex.Message}", ConsoleColor.Red);
        Log($"[stack trace]: {ex.StackTrace}", ConsoleColor.Red);
    }

    ///<summary>Logs message with "[EXCEPTION]:", [message] and [stack trace]</summary>
    public static void LogException(Exception ex)
    {
        Log("[EXCEPTION]:");
        Log($"[message]: {ex.Message}", ConsoleColor.Red);
        Log($"[stack trace]: {ex.StackTrace}", ConsoleColor.Red);
    }

    ///<summary>This is for debbugging purposes, it floods console, not recommended to use.</summary>
    public static void Log(ActionForm form)
    {
        StringBuilder builder = new();

        builder.AppendLine("----- Action Log");
        builder.AppendLine($"action={form.action}");
        builder.AppendLine($"platform={form.platform}");
        builder.AppendLine($"mode={form.mode}");
        builder.AppendLine($"time={form.time}");
        builder.AppendLine($"uniq_id={form.uniq_id}");
        builder.AppendLine($"device={form.device}");
        builder.AppendLine($"app_version={form.app_version}");
        builder.AppendLine($"id={form.id}");
        builder.AppendLine($"whom={form.whom}");
        builder.AppendLine($"ids={form.ids}");
        builder.AppendLine($"rank={form.rank}");
        builder.AppendLine($"platform_id={form.platform_id}");
        builder.AppendLine($"version={form.version}");
        builder.AppendLine($"param={form.param}");
        builder.AppendLine($"auth={form.auth}");
        builder.AppendLine($"from_friends={form.from_friends}");
        builder.AppendLine($"private_messages=Exist: {form.private_messages != null}");
        builder.AppendLine($"level={form.level}");
        builder.AppendLine($"paying={form.paying}");
        builder.AppendLine($"token={form.token}");
        builder.AppendLine("----- End");

        Log("\n" + builder.ToString(), ConsoleColor.Green);
    }
}