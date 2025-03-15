using System.Text;

namespace PixelServer.Admin;

///<summary>Main code for Admin Panel</summary>
public static class AdminPanel
{
    public static bool isTyping { get; private set; }

    // Im writing console app almost for the first time dont blame me for that awful code.
    public static async Task Run()
    {
        Console.WriteLine("Use '/' to run command.");
        Console.WriteLine("Input '?' for help.");

        Console.CursorVisible = false;

        while (true)
        {
            if (!isTyping)
            {
                await Task.Delay(100);

                if (Console.KeyAvailable && Console.ReadKey(true).KeyChar == '/')
                {
                    isTyping = true;
                    Console.CursorVisible = true;
                    Console.WriteLine("Input command....");
                }
                continue;
            }

            string? input = Console.ReadLine();

            isTyping = false;
            Console.CursorVisible = false;

            await OnCommand(input);
        }
    }


    public static async Task OnCommand(string? command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;

        command = command.Trim();

        switch (command)
        {
            case "?": LogManual(); return;
            case "clear": Console.Clear(); return;
        }

        if (command.StartsWith(AdminConsts.ban)) await AdminUtils.BanOrUnban(command);
        else if (command.StartsWith(AdminConsts.unban)) await AdminUtils.BanOrUnban(command);
        else if (command.StartsWith("version")) await AdminUtils.ModGameVer(command);

        else if (command.StartsWith("badfilter")) await AdminUtils.BadFilter(command);

        else Console.WriteLine($"Unknown command \"{command}\", input '?' for help");
    }

    static void LogManual()
    {
        StringBuilder builder = new();

        builder.AppendLine("Manual IDK");
        builder.AppendLine("---");
        builder.AppendLine("?: Log this");
        builder.AppendLine("clear: Clears console.");
        builder.AppendLine("-");
        builder.AppendLine($"{AdminConsts.ban} {{long:id}}: Bans player.");
        builder.AppendLine($"{AdminConsts.unban} {{long:id}}: Unbans player.");
        builder.AppendLine("version add/delete: Modifies list of allowed game versions.");
        builder.AppendLine("-");
        builder.AppendLine("badfilter log: Logs allbad words/symbols in database.");
        builder.AppendLine("badfilter {add/remove} {string/char:value}: Adds bad word/symbol in database.");

        Console.WriteLine(builder);
    }
}
