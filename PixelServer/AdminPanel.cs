using System.Text;

namespace PixelServer;

public static class AdminPanel
{
    public static bool noInput = true;

    // Im writing console app for the first time dont blame me for that awful code.
    public static async Task Run()
    {
        Console.WriteLine("Use '/' to run command.");
        Console.WriteLine("Input '?' for help.");

        Console.CursorVisible = false;

        while (true)
        {
            if (noInput)
            {
                await Task.Yield(); 

                if (Console.KeyAvailable && Console.ReadKey(true).KeyChar == '/')
                {
                    noInput = false;
                    Console.CursorVisible = true;
                    Console.WriteLine("Input command....");
                }
                continue;
            }

            string? input = Console.ReadLine();

            await OnCommand(input);

            noInput = true;
            Console.CursorVisible = false;
        }
    }


    private static async Task OnCommand(string? command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;

        command = command.Trim().ToLower();

        switch (command)
        {
            case "?": LogManual(); break;
            case "clear": Console.Clear(); break;
            default: Console.WriteLine($"Unknown command \"{command}\", input '?' for help"); break;
        }
    }

    static void LogManual()
    {
        StringBuilder builder = new();

        builder.AppendLine("Manual IDK");
        
        builder.AppendLine("?: Log this");
        builder.AppendLine("clear: Clears console.");

        Console.WriteLine(builder);
    }
}
