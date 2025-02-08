using PixelServer.Objects;

namespace PixelServer.Helpers;

public static class DebugHelper
{
    public static void Log(string message, bool skipLine = false)
    {
        if (!AdminPanel.noInput) return;

        if (string.IsNullOrEmpty(message)) return;

        if (skipLine) message += "\n";

        Console.WriteLine(message);
    }

    public static void ForceLog(string message)
    {
        if (!AdminPanel.noInput) AdminPanel.noInput = true;

        Console.Clear();

        Console.WriteLine("FORCE LOG:\n");

        Console.WriteLine(message);
    }

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

        Log(log, true);
    }
}