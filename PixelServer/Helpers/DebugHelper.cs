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

    public static void LogWarning(string msg) => Log(msg, ConsoleColor.Yellow);

    public static void LogError(string msg) => Log(msg, ConsoleColor.Red);

    ///<summary>Logs message with [message] (from exception) and [stack trace]</summary>
    public static void LogException(string message, Exception ex, bool includeStackTrace = true)
    {
        Log(message, ConsoleColor.Red);
        Log($"[message]: {ex.Message}", ConsoleColor.Red);
        if (includeStackTrace) Log($"[stack trace]: {ex.StackTrace}", ConsoleColor.Red);
    }

    ///<summary>This is for debbugging purposes, it floods console, not recommended to use.</summary>
    public static void Log(ActionForm form)
    {
        StringBuilder builder = new();

        builder.AppendLine("----- Action Log");
        if (!string.IsNullOrEmpty(form.action)) builder.AppendLine($"action={form.action}");
        if (form.platform != null) builder.AppendLine($"platform={form.platform}");
        if (!string.IsNullOrEmpty(form.mode)) builder.AppendLine($"mode={form.mode}");
        if (!string.IsNullOrEmpty(form.time)) builder.AppendLine($"time={form.time}");
        if (form.uniq_id != null) builder.AppendLine($"uniq_id={form.uniq_id}");
        if (!string.IsNullOrEmpty(form.device)) builder.AppendLine($"device={form.device}");
        if (!string.IsNullOrEmpty(form.app_version)) builder.AppendLine($"app_version={form.app_version}");
        if (form.id != null) builder.AppendLine($"id={form.id}");
        if (!string.IsNullOrEmpty(form.nick)) builder.AppendLine($"nick={form.nick}");
        if (!string.IsNullOrEmpty(form.skin)) builder.AppendLine($"skin={form.skin}");
        if (form.whom != null) builder.AppendLine($"whom={form.whom}");
        if (!string.IsNullOrEmpty(form.ids)) builder.AppendLine($"ids={form.ids}");
        if (!string.IsNullOrEmpty(form.rank)) builder.AppendLine($"rank={form.rank}");
        if (!string.IsNullOrEmpty(form.platform_id)) builder.AppendLine($"platform_id={form.platform_id}");
        if (!string.IsNullOrEmpty(form.version)) builder.AppendLine($"version={form.version}");
        if (!string.IsNullOrEmpty(form.param)) builder.AppendLine($"param={form.param}");
        if (!string.IsNullOrEmpty(form.auth)) builder.AppendLine($"auth={form.auth}");
        if (form.from_friends != null) builder.AppendLine($"from_friends={form.from_friends}");

        builder.AppendLine($"private_messages=Exist: {form.private_messages != null}");

        if (form.paying != null) builder.AppendLine($"paying={form.paying}");
        if (!string.IsNullOrEmpty(form.token)) builder.AppendLine($"token={form.token}");
        if (!string.IsNullOrEmpty(form.action)) builder.AppendLine("----- End");

        Log("\n" + builder.ToString(), ConsoleColor.Green);
    }
}