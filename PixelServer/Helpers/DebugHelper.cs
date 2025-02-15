using PixelServer.Objects;

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
        string log = "\n";

        log += "----- Action Log\n";
        log += $"action={form.action}\n";
        log += $"platform={form.platform}\n";
        log += $"uniq_id={form.uniq_id}\n";
        log += $"device={form.device}\n";
        log += $"app_version={form.app_version}\n";
        log += $"auth={form.auth}\n";
        log += $"token={form.token}\n";
        log += "----- End\n";

        Console.ForegroundColor = ConsoleColor.Green;
        Log(log, true);
        Console.ForegroundColor = ConsoleColor.White;
    }
}