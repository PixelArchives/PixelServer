using PixelServer.Objects;

namespace PixelServer.Helpers;

public static class DebugHelper
{
    public static ILogger logger;

    public static void Log(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        logger.LogInformation("\n");
        logger.LogInformation(message);
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

        logger.LogInformation(log);
    }
}