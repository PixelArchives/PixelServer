using PixelServer.Helpers;

namespace PixelServer;

public static class AdminPanel
{
    public static async Task Run()
    {
        DebugHelper.Log("Input '?' for help.");

        while (true)
        {
            string? input = Console.ReadLine();

            await OnCommand(input);
        }
    }

    private static async Task OnCommand(string? command)
    {
        if (string.IsNullOrEmpty(command)) DebugHelper.Log("Command is null or empty.");

        DebugHelper.Log("Unknown command, input '?' for help");
    }
}
