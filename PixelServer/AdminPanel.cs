using PixelServer.Helpers;
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

        command = command.Trim();

        switch (command)
        {
            case "?": LogManual(); return;
            case "clear": Console.Clear(); return;
        }

        if (command.StartsWith("AddBadWord"))
        {
            string[] vals = command.Split(' ');
            if (vals.Length > 3)
            {
                Console.WriteLine("Too much arguments");
                return;
            }
            else if (vals.Length < 3)
            {
                Console.WriteLine("Too few arguments");
                return;
            }

            try
            {
                if (bool.TryParse(vals[2], out bool is_symbol))
                {
                    bool b = await BadWordHelper.AddValue(vals[1], is_symbol);
                    if (!b) Console.WriteLine("Couldn't add value");
                }
                else Console.WriteLine("Couldn't parse 'is_symbol'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't add value. Ex: {ex}");
            }

            Console.WriteLine($"Added value \"{vals[1]}\" successfuly.");
        }
        else Console.WriteLine($"Unknown command \"{command}\", input '?' for help");
    }

    static void LogManual()
    {
        StringBuilder builder = new();

        builder.AppendLine("Manual IDK");
        
        builder.AppendLine("?: Log this");
        builder.AppendLine("clear: Clears console.");
        builder.AppendLine("AddBadWord {string/char:value} {bool:is_symbol}: Adds bad word/symbol in database.");

        Console.WriteLine(builder);
    }
}
