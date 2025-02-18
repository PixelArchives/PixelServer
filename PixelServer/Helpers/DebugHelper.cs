using PixelServer.Objects;
using System.Text;

namespace PixelServer.Helpers;

public static class DebugHelper
{
    public static void Log(string message, bool skipLine = false)
    {
        if (!Admin.AdminPanel.noInput) return;

        if (string.IsNullOrEmpty(message)) return;

        if (skipLine) message += "\n";

        Console.WriteLine(message);
    }

    public static void LogError(string message, bool skipLine = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Log(message, skipLine);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void LogWarning(string message, bool skipLine = false)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Log(message, skipLine);
        Console.ForegroundColor = ConsoleColor.White;
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
        builder.AppendLine($"ids={form.ids}");
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

        Console.ForegroundColor = ConsoleColor.Green;
        Log(builder.ToString(), true);
        Console.ForegroundColor = ConsoleColor.White;
    }
}