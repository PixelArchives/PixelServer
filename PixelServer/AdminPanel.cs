using System.Text;

namespace PixelServer;

public static class AdminPanel
{
    public static bool noInput;

    // Im writing console app for the first time dont blame me for that awful code.
    public static async Task Run()
    {
        Console.WriteLine("Use '/' to run command.");
        Console.WriteLine("Input '?' for help.");

        while (true)
        {
            if (noInput)
            {
                await Task.Yield(); 

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    noInput = false;
                    Console.CursorVisible = true;
                    Console.WriteLine("Command input resumed.");
                }
                continue;
            }

            string? input = Console.ReadLine();

            await OnCommand(input);
        }
    }


    private static async Task OnCommand(string? command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;

        command = command.Trim().ToLower();

        switch (command)
        {
            case "?": LogManual(); break;
            case "log": Console.WriteLine("Logging, press `Q` to exit"); noInput = true; Console.CursorVisible = false;  break;
            case "clear": Console.Clear(); break;
            default: Console.WriteLine($"Unknown command \"{command}\", input '?' for help"); break;
        }
    }

    static void LogManual()
    {
        StringBuilder builder = new();

        builder.AppendLine("Manual IDK");
        
        builder.AppendLine("?: Log this");
        builder.AppendLine("log: Start logging actions and etc.");
        builder.AppendLine("clear: Clears console.");

        Console.WriteLine(builder);
    }
}
