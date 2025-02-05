using PixelServer.Helpers;

namespace PixelServer;

public static class AdminPanel
{
    public static async Task Run()
    {
        Console.WriteLine("Input ? for help.");

        while (true)
        {
            string? input = Console.ReadLine();

            string response = await OnCommand(input);

            DebugHelper.Log(response);
        }
    }

    private static async Task<string> OnCommand(string? command)
    {
        if (string.IsNullOrEmpty(command)) return "Command is null or empty.";
        return "";
    }
}
